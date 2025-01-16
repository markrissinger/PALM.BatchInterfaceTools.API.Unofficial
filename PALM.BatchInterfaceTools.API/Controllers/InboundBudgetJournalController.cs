using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using PALM.BatchInterfaceTools.API.Constants;
using PALM.BatchInterfaceTools.API.DTO;
using PALM.BatchInterfaceTools.API.Helpers.Parsers;
using PALM.BatchInterfaceTools.API.Helpers.Utilities;
using PALM.BatchInterfaceTools.API.Infrastructure.Repositories;
using PALM.BatchInterfaceTools.Library.Entities.CommitmentControl.InboundBudgetJournal;
using PALM.BatchInterfaceTools.Library.Entities.PurchaseOrders.InboundEncumbranceLoad;
using PALM.BatchInterfaceTools.Library.Extensions;
using System.Text;

namespace PALM.BatchInterfaceTools.API.Controllers
{
    //todo: add validation step after getting DTOs
    [ApiController]
    [Route("[controller]")]
    public class InboundBudgetJournalController : ControllerBase
    {
        public InboundBudgetJournalController(IMapper mapper, RunHistoryRepository runHistoryRepository)
        {
            _mapper = mapper;
            _runHistoryRepository = runHistoryRepository;
        }

        private readonly IMapper _mapper;
        private readonly RunHistoryRepository _runHistoryRepository;
        private const string _interfaceId = "KKI001";

        #region File Output
        //todo: add file location for template
        /// <summary>
        /// Converts provided Excel file with Budget Journal data to a PALM specified flat file.
        /// </summary>
        /// <param name="file">Input Excel file based on appropriate template here. Copy can be found: </param>
        /// <param name="agency">Agency ID (Business Unit) to be used in the returned file name.</param>
        /// <param name="agencyBusinessSystem">Agency Business System (ABS) acronymn to be used in the returned file name.</param>
        /// <returns>Flat/text file based on the provided Budget Journal data in PALM specified format.</returns>
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
                    IEnumerable<FlattenedBudgetJournal> budgetJournals = ExcelParser.ParseExcelUploadTemplate<FlattenedBudgetJournal>(ms);

                    return await FlattenedBudgetJournalsToInboundFile(budgetJournals, agency, agencyBusinessSystem);
                }
            }
            else
            {
                throw new InvalidDataException("Unsupported file type!");
            }
        }

        /// <summary>
        /// Converts provided flat Budget Journal data to a PALM specified flat file.
        /// </summary>
        /// <param name="poHeaderDetails">Flattened (DTO) version of Budget Journals to be converted.</param>
        /// <param name="agency">Agency ID (Business Unit) to be used in the returned file name.</param>
        /// <param name="agencyBusinessSystem">Agency Business System (ABS) acronymn to be used in the returned file name.</param>
        /// <returns>Flat/text file based on the provided Budget Journal data in PALM specified format.</returns>
        [HttpPost("FlattenedBudgetJournalsToInboundFile")]
        public async Task<IActionResult> FlattenedBudgetJournalsToInboundFile(IEnumerable<FlattenedBudgetJournal> flattenedBudgetJournals, string? agency, string? agencyBusinessSystem)
        {
            // Convert DTO --> Domain
            List<KKBudgetHeader> budgetJournals = _mapper.Map<List<KKBudgetHeader>>(flattenedBudgetJournals.ToList());

            return await BudgetJournalsToInboundFile(budgetJournals, agency, agencyBusinessSystem);
        }

        /// <summary>
        /// Converts provided POHeaderDetails to a PALM specified flat file.
        /// </summary>
        /// <param name="poHeaderDetails">Flattened (DTO) version of Budget Journals to be converted.</param>
        /// <param name="agency">Agency ID (Business Unit) to be used in the returned file name.</param>
        /// <param name="agencyBusinessSystem">Agency Business System (ABS) acronymn to be used in the returned file name.</param>
        /// <returns>Flat/text file based on the provided Budget Journal data in PALM specified format.</returns>
        [HttpPost("BudgetJournalsToInboundFile")]
        public async Task<IActionResult> BudgetJournalsToInboundFile(IEnumerable<KKBudgetHeader> budgetJournals, string? agency, string? agencyBusinessSystem)
        {
            // Convert Domain --> byte[]
            byte[] fileContents = budgetJournals.WriteRecordsToByteArray();

            // Determine MIME & FileName
            string fileName = FileHelpers.FileNameGenerator(_interfaceId, agency, agencyBusinessSystem);
            string mimeType = GeneralConstants.TextFileMimeType;

            var contentDisposition = new ContentDispositionHeaderValue("attachment");
            contentDisposition.SetHttpFileName(fileName);
            Response.Headers[HeaderNames.ContentDisposition] = contentDisposition.ToString();

            try
            {
                await _runHistoryRepository.AddRunHistory(_interfaceId, budgetJournals.Count());
            }
            catch (Exception ex)
            {
                // todo: need to log exception
            }

            return new FileContentResult(fileContents, mimeType);
        }
        #endregion

        #region Text Ouput
        //todo: add file location for template
        /// <summary>
        /// Converts provided Excel file with Budget Journal data to text output based on PALM specified flat file.
        /// </summary>
        /// <param name="file">Input Excel file based on appropriate template here. Copy can be found: </param>
        /// <returns>Flat/text file based on the provided Budget Journal data in PALM specified format.</returns>
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
                    IEnumerable<FlattenedBudgetJournal> budgetJournals = ExcelParser.ParseExcelUploadTemplate<FlattenedBudgetJournal>(ms);

                    return await FlattenedBudgetJournalsToInboundFileContents(budgetJournals);
                }
            }
            else
            {
                throw new InvalidDataException("Unsupported file type!");
            }
        }

        /// <summary>
        /// Converts provided flat Budget Journal data to text output based on PALM specified flat file.
        /// </summary>
        /// <param name="flattenedBudgetJournals">Flattened (DTO) version of Budget Journals to be converted.</param>
        /// <returns>Flat/text file based on the provided Budget Journal data in PALM specified format.</returns>
        [HttpPost("FlattenedBudgetJournalsToInboundFileContents")]
        public async Task<JsonResult> FlattenedBudgetJournalsToInboundFileContents(IEnumerable<FlattenedBudgetJournal> flattenedBudgetJournals)
        {
            // Convert DTO --> Domain
            IEnumerable<KKBudgetHeader> budgetJournals = _mapper.Map<List<KKBudgetHeader>>(flattenedBudgetJournals);

            return await BudgetJournalsToInboundFileContents(budgetJournals);
        }

        /// <summary>
        /// Converts provided POHeaderDetails to text output based on PALM specified flat file.
        /// </summary>
        /// <param name="budgetJournals">Flattened (DTO) version of Budget Journals to be converted.</param>
        /// <returns>Flat/text file based on the provided Budget Journal data in PALM specified format.</returns>
        [HttpPost("BudgetJournalsToInboundFileContents")]
        public async Task<JsonResult> BudgetJournalsToInboundFileContents(IEnumerable<KKBudgetHeader> budgetJournals)
        {
            // Convert Domain --> byte[]
            StringBuilder fileContents = budgetJournals.WriteRecordsToStringBuilder();

            try
            {
                await _runHistoryRepository.AddRunHistory(_interfaceId, budgetJournals.Count());
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
