using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alphasoft.Data;
using Alphasoft.IRepositories;
using Alphasoft.Models;
using Microsoft.EntityFrameworkCore;

namespace Alphasoft.Repositories
{
    public class OurTeamRepository : Repository<OurTeam>, IOurTeamRepository
    {
        public OurTeamRepository(ApplicationDbContext context) : base(context) { }

        public ApplicationDbContext Context
        {
            get
            {
                return _context as ApplicationDbContext;
            }
        }

        public List<OurTeam> GetAllWithDepartmentAndDesignation()
        {
            return Context.OurTeams.Include(x => x.Department).Include(x => x.Designation).ToList();
        }

        public OurTeam GetWithDepartmentDesignation(int id)
        {
            return Context.OurTeams.Where(x => x.Id == id).Include(x => x.Department).Include(x => x.Designation).FirstOrDefault();
        }

        public List<OurTeam> TeamWithUs()
        {
            return Context.OurTeams.ToList();
        }
    }
}
