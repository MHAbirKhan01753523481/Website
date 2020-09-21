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
    public class SubMenuRepository : Repository<SubMenu>, ISubMenuRepository
    {
        public SubMenuRepository(ApplicationDbContext context) : base(context) { }

        public ApplicationDbContext Context { get { return _context as ApplicationDbContext; } }

        public List<SubMenu> GetAllWithMenu()
        {
            return Context.SubMenus.Include(x => x.Menu).ToList();
        }

    }
}
