using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Alphasoft.Models
{
    public class Features
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public string IconImage { get; set; }
        public string ShortDescription { get; set; }
        public int SoftwareId { get; set; }
        public Software Software { get; set; }
    }
}
