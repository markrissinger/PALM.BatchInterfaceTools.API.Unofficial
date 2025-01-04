using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using PALM.BatchInterfaceTools.API.Constants;
using PALM.BatchInterfaceTools.API.DTO;
using PALM.BatchInterfaceTools.API.Helpers.Parsers;
using PALM.BatchInterfaceTools.API.Helpers.Utilities;
using PALM.BatchInterfaceTools.Library.Entities.AccountsPayables.InboundVoucherLoad;
using PALM.BatchInterfaceTools.Library.Extensions;
using System.Text;

namespace PALM.BatchInterfaceTools.API.Controllers
{
    //todo: add validation step after getting DTOs
    [ApiController]
    [Route("[controller]")]
    public class InboundVoucherLoadController : ControllerBase
    {
        public InboundVoucherLoadController(IMapper mapper)
        {
            _mapper = mapper;
        }

        private readonly IMapper _mapper;
        private const string _interfaceId = "API002";

        #region File Output
        //todo: add file location for template
        /// <summary>
        /// Converts provided Excel file with Accounts Payable data to a PALM specified flat file.
        /// </summary>
        /// <param name="file">Input Excel file based on appropriate template here. Copy can be found: </param>
        /// <param name="agency">Agency ID (Business Unit) to be used in the returned file name.</param>
        /// <param name="agencyBusinessSystem">Agency Business System (ABS) acronymn to be used in the returned file name.</param>
        /// <returns>Flat/text file based on the provided Accounts Payable data in PALM specified format.</returns>
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
                    IEnumerable<FlattenedVoucher> vouchers = ExcelParser.ParseExcelUploadTemplate<FlattenedVoucher>(ms);

                    return await FlattenedVouchersToInboundFile(vouchers, agency, agencyBusinessSystem);
                }
            }
            else
            {
                throw new InvalidDataException("Unsupported file type!");
            }
        }

        /// <summary>
        /// Converts provided flat Accounts Payable data to a PALM specified flat file.
        /// </summary>
        /// <param name="flattenedVouchers">Flattened (DTO) version of Accounts Payables to be converted.</param>
        /// <param name="agency">Agency ID (Business Unit) to be used in the returned file name.</param>
        /// <param name="agencyBusinessSystem">Agency Business System (ABS) acronymn to be used in the returned file name.</param>
        /// <returns>Flat/text file based on the provided Accounts Payable data in PALM specified format.</returns>
        [HttpPost("FlattenedPurchaseOrdersToInboundFile")]
        public async Task<IActionResult> FlattenedVouchersToInboundFile(IEnumerable<FlattenedVoucher> flattenedVouchers, string? agency, string? agencyBusinessSystem)
        {
            // Convert DTO --> Domain
            IEnumerable<VoucherHeader> vouchers = _mapper.Map<List<VoucherHeader>>(flattenedVouchers.ToList());

            return await VoucherHeadersToInboundFile(vouchers, agency, agencyBusinessSystem);
        }

        /// <summary>
        /// Converts provided POHeaderDetails to a PALM specified flat file.
        /// </summary>
        /// <param name="voucherHeaders">Flattened (DTO) version of Accounts Payables to be converted.</param>
        /// <param name="agency">Agency ID (Business Unit) to be used in the returned file name.</param>
        /// <param name="agencyBusinessSystem">Agency Business System (ABS) acronymn to be used in the returned file name.</param>
        /// <returns>Flat/text file based on the provided Accounts Payable data in PALM specified format.</returns>
        //[HttpPost("POHeaderDetailsToInboundFile")] keeping this private due to potentially sensitive data that could be passed in from non-DTO object
        private async Task<IActionResult> VoucherHeadersToInboundFile(IEnumerable<VoucherHeader> voucherHeaders, string? agency, string? agencyBusinessSystem)
        {
            // Convert Domain --> byte[]
            byte[] fileContents = voucherHeaders.WriteRecordsToByteArray();

            // Determine MIME & FileName
            string fileName = FileHelpers.FileNameGenerator(_interfaceId, agency, agencyBusinessSystem);
            string mimeType = GeneralConstants.TextFileMimeType;

            var contentDisposition = new ContentDispositionHeaderValue("attachment");
            contentDisposition.SetHttpFileName(fileName);
            Response.Headers[HeaderNames.ContentDisposition] = contentDisposition.ToString();

            return new FileContentResult(fileContents, mimeType);
        }
        #endregion

        #region Text Ouput
        //todo: add file location
        /// <summary>
        /// Converts provided Excel file with Accounts Payable data to text output based on PALM specified flat file.
        /// </summary>
        /// <param name="file">Input Excel file based on appropriate template here. Copy can be found: </param>
        /// <param name="agency">Agency ID (Business Unit) to be used in the returned file name.</param>
        /// <param name="agencyBusinessSystem">Agency Business System (ABS) acronymn to be used in the returned file name.</param>
        /// <returns>Flat/text file based on the provided Accounts Payable data in PALM specified format.</returns>
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
                    IEnumerable<FlattenedVoucher> purchaseOrders = ExcelParser.ParseExcelUploadTemplate<FlattenedVoucher>(ms);

                    return await FlattenedVoucherToInboundFileContents(purchaseOrders);
                }
            }
            else
            {
                throw new InvalidDataException("Unsupported file type!");
            }
        }

        /// <summary>
        /// Converts provided flat Accounts Payable data to text output based on PALM specified flat file.
        /// </summary>
        /// <param name="poHeaderDetails">Flattened (DTO) version of Accounts Payables to be converted.</param>
        /// <param name="agency">Agency ID (Business Unit) to be used in the returned file name.</param>
        /// <param name="agencyBusinessSystem">Agency Business System (ABS) acronymn to be used in the returned file name.</param>
        /// <returns>Flat/text file based on the provided Accounts Payable data in PALM specified format.</returns>
        [HttpPost("FlattenedVoucherToInboundFileContents")]
        public async Task<JsonResult> FlattenedVoucherToInboundFileContents(IEnumerable<FlattenedVoucher> flattenedVouchers)
        {
            // Convert DTO --> Domain
            IEnumerable<VoucherHeader> vouchers = _mapper.Map<List<VoucherHeader>>(flattenedVouchers);

            return await VoucherHeadersToInboundFileContents(vouchers);
        }

        /// <summary>
        /// Converts provided POHeaderDetails to text output based on PALM specified flat file.
        /// </summary>
        /// <param name="voucherHeaders">Flattened (DTO) version of Accounts Payables to be converted.</param>
        /// <param name="agency">Agency ID (Business Unit) to be used in the returned file name.</param>
        /// <param name="agencyBusinessSystem">Agency Business System (ABS) acronymn to be used in the returned file name.</param>
        /// <returns>Flat/text file based on the provided Accounts Payable data in PALM specified format.</returns>
        //[HttpPost("POHeaderDetailsToInboundFileContents")] keeping this private due to potentially sensitive data that could be passed in from non-DTO object
        private async Task<JsonResult> VoucherHeadersToInboundFileContents(IEnumerable<VoucherHeader> voucherHeaders)
        {
            // Convert Domain --> byte[]
            StringBuilder fileContents = voucherHeaders.WriteRecordsToStringBuilder(false);

            return new JsonResult(fileContents.ToString());
        }
        #endregion
    }
}
