using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Alphasoft.ViewModels
{
    public class ProductListViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CategoryName { get; set; }
        public double? PurchasePrice { get; set; }
        public double? RetailPrice { get; set; }
        public double? WholeSellPrice { get; set; }
        public double? Discount { get; set; }
        public string Description { get; set; }
        public string ShortDescription { get; set; }
        public DateTime ReleaseDate { get; set; }
        public bool IsPopular { get; set; }
        public string Image { get; set; }
        public int Order { get; set; }
    }
}
