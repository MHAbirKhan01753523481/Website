using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alphasoft.Data;
using Alphasoft.IRepositories;
using Alphasoft.Models;

namespace Alphasoft.Repositories
{
    public class DesignationRepository : Repository<Designation>, IDesignationRepository
    {
        public DesignationRepository(ApplicationDbContext context) : base(context) { }

        public ApplicationDbContext Context
        {
            get { return _context as ApplicationDbContext; }
        }
    }
}
