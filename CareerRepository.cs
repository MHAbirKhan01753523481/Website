using Alphasoft.Data;
using Alphasoft.IRepositories;
using Alphasoft.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Alphasoft.Repositories
{
    public class CareerRepository: Repository<Career>, ICareerRepository
    {
        public CareerRepository(ApplicationDbContext context) : base(context) { }
        public ApplicationDbContext Context
        {
            get
            {
                return _context as ApplicationDbContext;
            }
        }

        public Career GetWithCareer()
        {
            return Context.Careers.FirstOrDefault();
        }

        public List<Career> GetWithCareers()
        {
            return Context.Careers.ToList();
        }
    }
}
