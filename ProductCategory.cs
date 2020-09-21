using System.Collections.Generic;

namespace Alphasoft.Models
{
    public class ProductCategory : Base
    {
        public ProductCategory()
        {
            Products = new List<Product>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public List<Product> Products { get; set; }
    }
}
