using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using PALM.BatchInterfaceTools.API.Constants;
using PALM.BatchInterfaceTools.API.DTO;
using PALM.BatchInterfaceTools.API.Helpers.Parsers;
using PALM.BatchInterfaceTools.API.Helpers.Utilities;
using PALM.BatchInterfaceTools.API.Infrastructure.Repositories;
using PALM.BatchInterfaceTools.Library.Entities.PurchaseOrders.InboundEncumbranceLoad;
using PALM.BatchInterfaceTools.Library.Extensions;
using PALM.BatchInterfaceTools.Library.Interfaces.PurchaseOrders;
using System.Net;
using System.Text;

namespace PALM.BatchInterfaceTools.API.Controllers
{
    //todo: add validation step after getting DTOs
    [ApiController] 
    [Route("[controller]")]
    public class InboundEncumbranceLoadController : ControllerBase
    {
        public InboundEncumbranceLoadController(IMapper mapper, RunHistoryRepository runHistoryRepository)
        {
            _mapper = mapper;
            _runHistoryRepository = runHistoryRepository;
        }

        private readonly IMapper _mapper;
        private readonly RunHistoryRepository _runHistoryRepository;
        private const string _interfaceId = "POI002";

        #region File Output
        //todo: add file location for template
        /// <summary>
        /// Converts provided Excel file with Purchase Order data to a PALM specified flat file.
        /// </summary>
        /// <param name="file">Input Excel file based on appropriate template here. Copy can be found: </param>
        /// <param name="agency">Agency ID (Business Unit) to be used in the returned file name.</param>
        /// <param name="agencyBusinessSystem">Agency Business System (ABS) acronymn to be used in the returned file name.</param>
        /// <returns>Flat/text file based on the provided Purchase Order data in PALM specified format.</returns>
        /// <exception cref="InvalidDataException">Will throw if input file is not the correct format.</exception>
        [HttpPost("ExcelToInboundFile")]
        public async Task<IActionResult> ExcelToInboundFile(IFormFile file, string? agency, string? agencyBusinessSystem)
        {
            if (FileHelpers.InputFileTypeValidation(file))
            {
                using (var ms = new MemoryStream())
                {
                    // Copy to MemoryStream for additional functionality
                    using (var stream = file.OpenReadStream()) 
                    { 
                        file.CopyTo(ms);                        
                    }

                    // Parse incoming Excel file to DTO
                    IEnumerable<FlattenedPurchaseOrder> purchaseOrders = ExcelParser.ParseExcelUploadTemplate<FlattenedPurchaseOrder>(ms);

                    return await FlattenedPurchaseOrdersToInboundFile(purchaseOrders, agency, agencyBusinessSystem);
                }
            }
            else
            {
                throw new InvalidDataException("Unsupported file type!");
            }
        }

        /// <summary>
        /// Converts provided flat Purchase Order data to a PALM specified flat file.
        /// </summary>
        /// <param name="flattenedPurchaseOrders">Flattened (DTO) version of Purchase Orders to be converted.</param>
        /// <param name="agency">Agency ID (Business Unit) to be used in the returned file name.</param>
        /// <param name="agencyBusinessSystem">Agency Business System (ABS) acronymn to be used in the returned file name.</param>
        /// <returns>Flat/text file based on the provided Purchase Order data in PALM specified format.</returns>
        [HttpPost("FlattenedPurchaseOrdersToInboundFile")]
        public async Task<IActionResult> FlattenedPurchaseOrdersToInboundFile(IEnumerable<FlattenedPurchaseOrder> flattenedPurchaseOrders, string? agency, string? agencyBusinessSystem)
        {
            // Convert DTO --> Domain
            IEnumerable<POHeaderDetails> purchaseOrders = _mapper.Map<List<POHeaderDetails>>(flattenedPurchaseOrders.ToList());

            return await POHeaderDetailsToInboundFile(purchaseOrders, agency, agencyBusinessSystem);
        }

        /// <summary>
        /// Converts provided POHeaderDetails to a PALM specified flat file.
        /// </summary>
        /// <param name="poHeaderDetails">Flattened (DTO) version of Purchase Orders to be converted.</param>
        /// <param name="agency">Agency ID (Business Unit) to be used in the returned file name.</param>
        /// <param name="agencyBusinessSystem">Agency Business System (ABS) acronymn to be used in the returned file name.</param>
        /// <returns>Flat/text file based on the provided Purchase Order data in PALM specified format.</returns>
        [HttpPost("POHeaderDetailsToInboundFile")]
        public async Task<IActionResult> POHeaderDetailsToInboundFile(IEnumerable<POHeaderDetails> poHeaderDetails, string? agency, string? agencyBusinessSystem)
        {
            // Convert Domain --> byte[]
            byte[] fileContents = poHeaderDetails.WriteRecordsToByteArray();

            // Determine MIME & FileName
            string fileName = FileHelpers.FileNameGenerator(_interfaceId, agency, agencyBusinessSystem);
            string mimeType = GeneralConstants.TextFileMimeType;

            var contentDisposition = new ContentDispositionHeaderValue("attachment");
            contentDisposition.SetHttpFileName(fileName);
            Response.Headers[HeaderNames.ContentDisposition] = contentDisposition.ToString();

            try
            {
                await _runHistoryRepository.AddRunHistory(_interfaceId, poHeaderDetails.Count());
            }
            catch(Exception ex)
            {
                // todo: need to log exception
            }

            return new FileContentResult(fileContents, mimeType);
        }
        #endregion

        #region Text Ouput
        //todo: add file location
        /// <summary>
        /// Converts provided Excel file with Purchase Order data to text output based on PALM specified flat file.
        /// </summary>
        /// <param name="file">Input Excel file based on appropriate template here. Copy can be found: </param>
        /// <returns>Flat/text file based on the provided Purchase Order data in PALM specified format.</returns>
        /// <exception cref="InvalidDataException">Will throw if input file is not the correct format.</exception>
        [HttpPost("ExcelToInboundFileContents")]
        public async Task<JsonResult> ExcelToInboundFileContents(IFormFile file)
        {
            if (FileHelpers.InputFileTypeValidation(file))
            {
                using (var ms = new MemoryStream())
                {
                    // Copy to MemoryStream for additional functionality
                    using (var stream = file.OpenReadStream())
                    {
                        file.CopyTo(ms);
                    }

                    // Parse incoming Excel file to DTO
                    IEnumerable<FlattenedPurchaseOrder> purchaseOrders = ExcelParser.ParseExcelUploadTemplate<FlattenedPurchaseOrder>(ms);

                    return await FlattenedPurchaseOrdersToInboundFileContents(purchaseOrders);
                }
            }
            else
            {
                throw new InvalidDataException("Unsupported file type!");
            }
        }

        /// <summary>
        /// Converts provided flat Purchase Order data to text output based on PALM specified flat file.
        /// </summary>
        /// <param name="flattenedPurchaseOrders">Flattened (DTO) version of Purchase Orders to be converted.</param>
        /// <returns>Flat/text file based on the provided Purchase Order data in PALM specified format.</returns>
        [HttpPost("FlattenedPurchaseOrdersToInboundFileContents")]
        public async Task<JsonResult> FlattenedPurchaseOrdersToInboundFileContents(IEnumerable<FlattenedPurchaseOrder> flattenedPurchaseOrders)
        {
            // Convert DTO --> Domain
            IEnumerable<POHeaderDetails> poHeaders = _mapper.Map<List<POHeaderDetails>>(flattenedPurchaseOrders);

            return await POHeaderDetailsToInboundFileContents(poHeaders);
        }

        /// <summary>
        /// Converts provided POHeaderDetails to text output based on PALM specified flat file.
        /// </summary>
        /// <param name="purchaseOrders">Flattened (DTO) version of Purchase Orders to be converted.</param>
        /// <returns>Flat/text file based on the provided Purchase Order data in PALM specified format.</returns>
        [HttpPost("POHeaderDetailsToInboundFileContents")]
        public async Task<JsonResult> POHeaderDetailsToInboundFileContents(IEnumerable<POHeaderDetails> purchaseOrders)
        {
            // Convert Domain --> byte[]
            StringBuilder fileContents = purchaseOrders.WriteRecordsToStringBuilder();

            try
            {
                await _runHistoryRepository.AddRunHistory(_interfaceId, purchaseOrders.Count());
            }
            catch (Exception ex)
            {
                // todo: need to log exception
            }

            return new JsonResult(fileContents.ToString());
        }
        #endregion
    }
}
