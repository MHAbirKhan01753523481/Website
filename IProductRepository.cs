using Alphasoft.Models;
using System.Collections.Generic;

namespace Alphasoft.IRepositories
{
    public interface IProductRepository : IRepository<Product>
    {
        List<Product> GetAllWithCategory();

        Product GetProductWithCategory(int productId);

        List<Product> GetPopularProducts();

        List<Product> GetCategoryWiseProducts(int categoryId);
    }
}
