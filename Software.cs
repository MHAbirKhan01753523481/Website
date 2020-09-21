using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Alphasoft.Models
{
    public class Software : Base
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Icon { get; set; }
        public string Image { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public int SoftwareCategoryId { get; set; }
        public SoftwareCategory SoftwareCategory { get; set; }
        public List<Features> Features { get; set; }
    }
}
