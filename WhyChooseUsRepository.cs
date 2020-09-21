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
    public class WhyChooseUsRepository : Repository<ChooseUs>, IWhyChooseUsRepository
    {
        public WhyChooseUsRepository(ApplicationDbContext context) : base(context) { }

        public ApplicationDbContext Context
        {
            get { return _context as ApplicationDbContext; }
        }

    }
}
