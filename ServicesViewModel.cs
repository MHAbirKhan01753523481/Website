using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Alphasoft.ViewModels
{
    public class ServicesViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Image { get; set; }

        public double? Price { get; set; }

        public string Description { get; set; }

        public string ShortDescription { get; set; }

        public string ServiceCategoryName { get; set; }
        public int Order { get; set; }

    }
}
