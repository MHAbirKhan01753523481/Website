using System.Collections.Generic;
using System.Linq;
using Alphasoft.Data;
using Alphasoft.IRepositories;
using Alphasoft.Models;
using Microsoft.EntityFrameworkCore;

namespace Alphasoft.Repositories
{
    public class ServiceRepository : Repository<Service>, IServiceRepository 
    {
        public ServiceRepository(ApplicationDbContext context) : base(context) { }

        public ApplicationDbContext Context
        {
            get { return _context as ApplicationDbContext; }
        }

        public List<Service> GetAllWithServiceCategory()
        {
            return Context.Services.Include(x => x.ServiceCategory).ToList();
        }
    }
}
