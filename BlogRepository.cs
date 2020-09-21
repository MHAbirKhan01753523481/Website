using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alphasoft.Data;
using Alphasoft.IRepositories;
using Alphasoft.Models;

namespace Alphasoft.Repositories
{
    public class BlogRepository : Repository<Blog>,IBlogsRepository
    {
        public BlogRepository(ApplicationDbContext context) : base(context) { }

        public ApplicationDbContext Context
        {
            get { return _context as ApplicationDbContext; }
        }

        public Blog GetWithBlogsItem(int id)
        {
            return Context.Blogs.Where(x => x.Id == id).FirstOrDefault();
        }

        public Blog GetWithLatImage()
        {
            return Context.Blogs.LastOrDefault();
        }

        public Blog GetWithSingleImage()
        {
            return Context.Blogs.FirstOrDefault();
        }
    }

}
