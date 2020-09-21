using Alphasoft.Data;
using Alphasoft.IRepositories;
using Alphasoft.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Alphasoft.Repositories
{
    public class MenuRepository : Repository<Menu>, IMenuRepository
    {
        public MenuRepository(ApplicationDbContext context) : base(context) { }

        public ApplicationDbContext Context
        {
            get
            {
                return _context as ApplicationDbContext;
            }
        }

        public List<Menu> GetAllWithSubMenus()
        {
            return Context.Menus.Include(x => x.SubMenus).ToList();
        }

        public List<Menu> GetAllActive()
        {
            return Context.Menus.Where(x => x.IsActive == true).ToList();
        }
    }
}
