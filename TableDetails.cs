using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Alphasoft.Models.RMSTable
{
    public class TableDetails
    {
        public int Id { get; set; }
     
        public int OrderSeat { get; set; }
        public int TotalChir { get; set; }
     
        public int AvaiableSeat { get; set; }

        // AvaiableSeat=TotalChair - OrderSeat//
        public DateTime ChoseTimeStart { get; set; }
        public DateTime ChoseTimeEnd { get; set; }

        public int TableId { get; set; }
        public Table Tables { get; set; }
    }
}
