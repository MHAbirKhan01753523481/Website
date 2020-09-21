using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Alphasoft.ViewModels
{
    public class JobVm
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public string Qualification { get; set; }
        public DateTime JobCreateDate { get; set; }
        public string JobCreateDateTo { get; set; }
        public DateTime DeadLine { get; set; }
        public string DeadLineTo { get; set; }
        public int Order { get; set; }
    }
}
