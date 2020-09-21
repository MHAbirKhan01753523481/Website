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
    public class BlogCommentRepository:Repository<BlogComment>,IBlogCommentRepository
    {
        public BlogCommentRepository(ApplicationDbContext context) : base(context) { }

        public ApplicationDbContext Context
        {
            get
            {
                return _context as ApplicationDbContext;
            }
        }

        public List<BlogComment> GetAllBlogs()
        {
            return Context.BlogComments.Include(x => x.Blog).ToList();
        }

        public BlogComment GetWithAllComment()
        {
            return Context.BlogComments.Where(x => x.Id >= 0).FirstOrDefault();
        }
    }
}
