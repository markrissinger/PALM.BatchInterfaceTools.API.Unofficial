using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PALM.BatchInterfaceTools.API.Infrastructure.DataTransferObjects
{
    [Table("PBIT_RunHistory")]
    [PrimaryKey("RunId")]
    public class RunHistoryDTO(string interfaceId, int numberOfRecords)
    {
        public int RunId { get; }
        public string InterfaceId { get; set; } = interfaceId;
        public int NumberOfRecords { get; set; } = numberOfRecords;
        public DateTime CreateTimestamp { get; set; } = DateTime.Now;
    }
}
