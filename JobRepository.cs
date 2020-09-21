using Alphasoft.Data;
using Alphasoft.IRepositories;
using Alphasoft.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Alphasoft.Repositories
{
    public class JobRepository:Repository<Job>,IJobRepository
    {
        public JobRepository(ApplicationDbContext context) : base(context) { }
        public ApplicationDbContext Context
        {
            get
            {
                return _context as ApplicationDbContext;
            }
        }
        public List<Job> GetWithJob()
        {
            return Context.Jobs.ToList();
        }

        public Job GetwithJobs()
        {
            return Context.Jobs.FirstOrDefault();
        }
    }
}
