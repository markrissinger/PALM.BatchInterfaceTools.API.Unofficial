using Microsoft.EntityFrameworkCore;
using PALM.BatchInterfaceTools.API.Infrastructure.Data;
using PALM.BatchInterfaceTools.API.Infrastructure.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PALM.BatchInterfaceTools.API.Infrastructure.Repositories
{
    public class RunHistoryRepository
    {
        public RunHistoryRepository(AzureSQLContext context)
        {
            _context = context;
        }

        private readonly AzureSQLContext _context;

        public async Task AddRunHistory(string interfaceId, int numberOfRecords)
        {
            _context.RunHistoryDTOs.Add(new RunHistoryDTO(interfaceId, numberOfRecords));
            await _context.SaveChangesAsync();
        }

        public async Task GetRunHistory(string? interfaceId)
        {
            if (string.IsNullOrEmpty(interfaceId))
                await _context.RunHistoryDTOs.ToListAsync();
            else
                await _context.RunHistoryDTOs.Where(x => x.InterfaceId == interfaceId).ToListAsync();
        }
    }
}
