using Microsoft.EntityFrameworkCore;
using PALM.BatchInterfaceTools.API.Infrastructure.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PALM.BatchInterfaceTools.API.Infrastructure.Data
{
    public class AzureSQLContext : DbContext
    {
        public AzureSQLContext(DbContextOptions<AzureSQLContext> options) : base(options)
        {            
        }

        public DbSet<RunHistoryDTO> RunHistoryDTOs { get; set; }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<RunHistoryDTO>().ToTable("PBIT_RunHistory");
        //}
    }
}
