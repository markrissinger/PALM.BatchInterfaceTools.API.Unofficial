using Microsoft.AspNetCore.Mvc;
using PALM.DeveloperTools.API.DTO;
using PALM.DeveloperTools.API.Helpers.Utilities;
using System.Net;

namespace PALM.DeveloperTools.API.Controllers
{
    [ApiController] 
    [Route("[controller]")]
    public class InboundEncumbranceLoadController : ControllerBase
    {
        public InboundEncumbranceLoadController()
        {
        }

        [HttpPost(Name = "ExcelToInboundFile")]
        public IActionResult ExcelToInboundFile(IFormFile file)
        {
            if (InputValidation.FileValidation(file))
            {
                using (var ms = new MemoryStream())
                {
                    // Copy to MemoryStream for additional functionality
                    using (var stream = file.OpenReadStream()) 
                    { 
                        file.CopyTo(ms);                        
                    }

                    // Parse incoming Excel file to DTO

                    // Convert DTO --> Domain

                    // Convert Domain --> byte[]

                    // Determine MIME & FileName

                    byte[] fileContents;
                    string fileName = "todo.txt";
                    return File(ms, "text/plain", fileName);
                }
            }

            throw new Exception("Unsupported file type!");
        }

    }
}
