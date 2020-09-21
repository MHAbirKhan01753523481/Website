using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alphasoft.Models;

namespace Alphasoft.IRepositories
{
    public interface IBlogsRepository : IRepository<Blog>
    {
        Blog GetWithSingleImage();
        Blog GetWithLatImage();
    
        Blog GetWithBlogsItem(int id);
    }
}
