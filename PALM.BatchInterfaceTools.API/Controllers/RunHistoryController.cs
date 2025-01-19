using Microsoft.AspNetCore.Mvc;
using PALM.BatchInterfaceTools.API.Infrastructure.DataTransferObjects;
using PALM.BatchInterfaceTools.API.Infrastructure.Repositories;

namespace PALM.BatchInterfaceTools.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RunHistoryController
    {
        public RunHistoryController(RunHistoryRepository runHistoryRepository)
        {
            _runHistoryRepository = runHistoryRepository;
        }

        private readonly RunHistoryRepository _runHistoryRepository;

        [HttpGet("RunHistoryData")]
        public async Task<List<RunHistoryDTO>> RunHistoryData(DateTime? startDate, DateTime? endDate, string? interfaceId)
        {
            return await _runHistoryRepository.GetRunHistory(startDate, endDate, interfaceId);
        }
    }
}
