using Alphasoft.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Alphasoft.IRepositories
{
   public interface IJobRepository:IRepository<Job>
    {
       List<Job> GetWithJob();
        Job GetwithJobs();

    }
}
