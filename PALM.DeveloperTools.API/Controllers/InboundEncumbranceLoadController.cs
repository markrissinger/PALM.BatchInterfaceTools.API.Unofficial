using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PALM.DeveloperTools.API.Constants;
using PALM.DeveloperTools.API.DTO;
using PALM.DeveloperTools.API.Helpers.Parsers;
using PALM.DeveloperTools.API.Helpers.Utilities;
using PALM.InterfaceLayouts.Unofficial.Entities.PurchaseOrders.InboundEncumbranceLoad;
using PALM.InterfaceLayouts.Unofficial.Extensions;
using System.Net;

namespace PALM.DeveloperTools.API.Controllers
{
    [ApiController] 
    [Route("[controller]")]
    public class InboundEncumbranceLoadController : ControllerBase
    {
        public InboundEncumbranceLoadController(IMapper mapper)
        {
            _mapper = mapper;
        }

        private readonly IMapper _mapper;

        [HttpPost(Name = "ExcelToInboundFile")]
        public IActionResult ExcelToInboundFile(IFormFile file, [FromBody] string? agency, [FromBody] string? agencyBusinessSystem)
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

                    // Convert DTO --> Domain
                    IEnumerable<POHeaderDetails> poHeaders = _mapper.Map<List<POHeaderDetails>>(purchaseOrders);

                    // Convert Domain --> byte[]
                    byte[] fileContents = poHeaders.WriteRecordsToByteArray();

                    // Determine MIME & FileName
                    string fileName = FileHelpers.FileNameGenerator("POI002", agency, agencyBusinessSystem);
                    string mimeType = GeneralConstants.TextFileMimeType;

                    return File(fileContents, mimeType, fileName);
                }
            }
            else
            {
                throw new Exception("Unsupported file type!");
            }
        }

    }
}
