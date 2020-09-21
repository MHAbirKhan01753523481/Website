using System.Collections.Generic;
using System.Linq;
using Alphasoft.IServices;
using Alphasoft.Models;
using Alphasoft.ViewModels.ClientViewModels;

namespace Alphasoft.Services
{
    public class QueryHelper : IQueryHelper
    {
        public List<CategoryProduct> GetCategoryProducts(List<Product> products)
        {
            var categoryProducts = products
                       .GroupBy(x => x.ProductCategoryId)
                       .Select(x => new CategoryProduct
                       {
                           ProductCategory = x.Key,
                           ProductCategoryName = x.Select(r => r.ProductCategory.Name).FirstOrDefault(),
                           Products = x.Select(r => r).ToList()
                       }).ToList();

            return categoryProducts;
        }

        public List<Product> GetCategoryWiseProducts(List<Product> products, int propductCategoryId)
        {
            var categoryProducts = products
                .Where(x => x.ProductCategoryId == propductCategoryId)
                .ToList();

            return categoryProducts;
        }

        public List<Product> GetPopularProducts(List<Product> products)
        {
            var popularProducts = products
                .Where(x => x.IsPopular == true)
                .ToList();

            return popularProducts;
        }
    }
}
