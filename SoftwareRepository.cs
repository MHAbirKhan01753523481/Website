using Alphasoft.Data;
using Alphasoft.IRepositories;
using Alphasoft.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Alphasoft.Repositories
{
    public class SoftwareRepository : Repository<Software>, ISoftwareRepository
    {
        public SoftwareRepository(ApplicationDbContext context) : base(context) { }

        public ApplicationDbContext Context
        {
            get
            {
                return _context as ApplicationDbContext;
            }
        }

        public Software GetAllWithFeatureAndCategory(int id)
        {
            return Context.Softwares
                .Include(x => x.SoftwareCategory)
                .Include(x => x.Features)
                .Where(x => x.Id == id)
                .FirstOrDefault();
        }

        public List<Software> GetAllWithSoftware()
        {
            return Context.Softwares.Include(x => x.SoftwareCategory).OrderBy(x => x.Order).ToList();
        }

        public List<Software> GetCategoryWiseSoftware(int id)
        {
            return Context.Softwares.Include(x => x.SoftwareCategory).Where(x => x.Id == id).ToList();
        }


    }
}
