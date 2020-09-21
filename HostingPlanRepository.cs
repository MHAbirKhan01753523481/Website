using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alphasoft.Data;
using Alphasoft.IRepositories;
using Alphasoft.Models;

namespace Alphasoft.Repositories
{
    public class HostingPlanRepository: Repository<HostingPlan>,IHostingPlanRepository
    {
        public HostingPlanRepository(ApplicationDbContext context):base(context)
        {

        }
        public  ApplicationDbContext context
        {
            get
            {
                return _context as ApplicationDbContext;
            }
        }
    }
}
