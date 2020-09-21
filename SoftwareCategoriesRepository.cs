using Alphasoft.Data;
using Alphasoft.IRepositories;
using Alphasoft.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Alphasoft.Repositories
{
    public class SoftwareCategoriesRepository : Repository<SoftwareCategory>,ISoftwareCategoriesRepository
    {
        public SoftwareCategoriesRepository(ApplicationDbContext context) : base(context) { }

        public ApplicationDbContext Context
        {
            get { return _context as ApplicationDbContext; }
        }

        public SoftwareCategory GetSoftwareCategories()
        {
            return Context.SoftwareCategories.FirstOrDefault();
        }

        public List<SoftwareCategory> GetWithSoftwareCategories()
        {
            return Context.SoftwareCategories.ToList();
        }
    }
}
