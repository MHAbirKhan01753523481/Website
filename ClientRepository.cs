using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alphasoft.Data;
using Alphasoft.IRepositories;
using Alphasoft.Models;

namespace Alphasoft.Repositories
{
    public class ClientRepository : Repository<Client>,IClientRepository
    {
        public ClientRepository(ApplicationDbContext context) : base(context) { }

        public ApplicationDbContext Context
        {
            get { return _context as ApplicationDbContext; }
        }

        //public List<Client> GetWithClientSayUs()
        //{
        //    return Context.Clients.TakeLast(3).ToList();
        //}
    }

   

}
