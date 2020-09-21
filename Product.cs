using System;
using System.ComponentModel.DataAnnotations;

namespace Alphasoft.Models
{
    public class Product : Base
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public double TotalPrice { get; set; }
        public string Image { get; set; }

        [Display(Name = "Purchase Price")]
        public double? PurchasePrice { get; set; }

        [Display(Name = "Retail Price")]
        public double? RetailPrice { get; set; }

        [Display(Name = "Wholesell Price")]
        public double? WholeSellPrice { get; set; }
        public double? Discount { get; set; }
        public string Description { get; set; }
        public DateTime ReleaseDate { get; set; }

        [Display(Name = "Short Description")]
        public string ShortDescription { get; set; }

        [Display(Name="Popular")]
        public bool IsPopular { get; set; }

        [Display(Name = "Category")]
        public int ProductCategoryId { get; set; }
        public ProductCategory ProductCategory { get; set; }
    }
}
