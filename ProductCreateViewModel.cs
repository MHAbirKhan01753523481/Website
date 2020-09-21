using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Alphasoft.ViewModels
{
    public class ProductCreateViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IFormFile Image { get; set; }
        public double? PurchasePrice { get; set; }
        public double? RetailPrice { get; set; }
        public double? WholeSellPrice { get; set; }
        public double? Discount { get; set; }
        public string Description { get; set; }
        public string ShortDescription { get; set; }
        public DateTime RealiseDate { get; set; }
        public int ProductCategoryId { get; set; }
       
    }
}
