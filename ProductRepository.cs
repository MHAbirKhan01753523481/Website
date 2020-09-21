using Alphasoft.Data;
using Alphasoft.IRepositories;
using Alphasoft.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Alphasoft.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext context) : base(context) { }

        public ApplicationDbContext Context
        {
            get { return _context as ApplicationDbContext; }
        }

        public List<Product> GetAllWithCategory()
        {
            return Context.Products.Include(x => x.ProductCategory).ToList();
        }

        public Product GetProductWithCategory(int productId)
        {
            return Context.Products.Where(x => x.Id == productId).Include(x => x.ProductCategory).FirstOrDefault();
        }

        public List<Product> GetPopularProducts()
        {
            return Context.Products.Where(x => x.IsPopular == true).ToList();
        }

        public List<Product> GetCategoryWiseProducts(int categoryId)
        {

            return Context.Products.Include(x => x.ProductCategory).Where(x => x.ProductCategoryId == categoryId).ToList();
        }
    }
}
