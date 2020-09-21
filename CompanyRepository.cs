using Alphasoft.Data;
using Alphasoft.IRepositories;
using Alphasoft.Models;

namespace Alphasoft.Repositories
{
    public class CompanyRepository : Repository<Company>, ICompanyRepository

    {
        public CompanyRepository(ApplicationDbContext context) : base(context) { }

        public ApplicationDbContext Context
        {
            get { return _context as ApplicationDbContext; }
        }
    }
}
