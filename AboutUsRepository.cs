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
    public class AboutUsRepository : Repository<AboutUs>, IAboutUsRepository
    {
        public AboutUsRepository(ApplicationDbContext context) : base(context) { }

        public ApplicationDbContext Context
        {
            get
            {
                return _context as ApplicationDbContext;
            }
        }

      
        public List<AboutUs> GetWithAboutUs()
        {
            
            return Context.AboutUs.ToList();
        }

        public AboutUs GetWithAbout()
        {
            return Context.AboutUs.FirstOrDefault();
        }

       
    }


}


