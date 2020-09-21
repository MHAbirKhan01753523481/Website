using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Alphasoft.Models
{
    public class HostingPlan
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Space { get; set; }
        public string Bandwidth { get; set; }
        public int Domain { get; set; }
        public string SubDomain { get; set; }
        public string Alias { get; set; }
        public string Email { get; set; }
        public bool CPanel { get; set; }
        public decimal YearlyPrice { get; set; }
        public decimal MonthlyPrice { get; set; }
        public string PriceUnit { get; set; }
    }
}
