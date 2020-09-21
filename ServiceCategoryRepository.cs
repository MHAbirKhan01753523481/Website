using Alphasoft.Data;
using Alphasoft.IRepositories;
using Alphasoft.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Alphasoft.Repositories
{
    public class ServiceCategoryRepository : Repository<ServiceCategory>, IServiceCategoryRepository
    {
        public ServiceCategoryRepository(ApplicationDbContext context) : base(context) { }

        public ApplicationDbContext Context
        {
            get { return _context as ApplicationDbContext; }
        }
    }
}
