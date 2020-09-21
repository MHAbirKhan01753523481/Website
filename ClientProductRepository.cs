using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alphasoft.Data;
using Alphasoft.Models;
using Microsoft.EntityFrameworkCore;

namespace Alphasoft.Repositories
{
    public class ClientProductRepository : Repository<ClientProduct>, IClientProductRepository
    {
        public ClientProductRepository(ApplicationDbContext context) : base(context) { }

        public ApplicationDbContext Context
        {
            get { return _context as ApplicationDbContext; }
        }

        public List<ClientProduct> ClientProjectsByProduct(int productId)
        {
            return Context.ClientProducts.Where(x => x.ProductId == productId).Include(x=>x.Client).Include(x=>x.Product).ToList();
        }

        public List<ClientProduct> GetAllWithClientProduct()
        {
            return Context.ClientProducts.Include(x => x.Product).Include(x => x.Client).ToList();
        }

      


    }
}
