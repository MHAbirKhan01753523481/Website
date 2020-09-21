using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Alphasoft.Models.RMSTable
{
    public class ReservedTable
    {
        public int Id { get; set; }
        public int AvailableTable { get; set; }
        public DateTime BookingStartTime { get; set; }
        public DateTime BookingEndTime { get; set; }
        public int ReservedTableNumber { get; set; }
        public int TableId { get; set; }
        public Table Table { get; set; }
    }
}
