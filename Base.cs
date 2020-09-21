using System;

namespace Alphasoft.Models
{
    public class Base
    {
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? UpdateCount { get; set; }
        public int Order { get; set; }
    }
}
