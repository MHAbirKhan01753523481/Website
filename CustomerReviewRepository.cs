using Alphasoft.Data;
using Alphasoft.IRepositories;
using Alphasoft.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Alphasoft.Repositories
{
    public class CustomerReviewRepository:Repository<CustomerRivew>, ICustomerReviewRepository
    {
        public CustomerReviewRepository(ApplicationDbContext context) : base(context) { }
        public ApplicationDbContext Context
        {
            get
            {
                return _context as ApplicationDbContext;
            }
        }

        public List<CustomerRivew> GetWithCustomerReview()
        {
            return Context.CustomerRivew.ToList();
        }

        public CustomerRivew WithCustomerRivew()
        {
            return Context.CustomerRivew.FirstOrDefault();
        }
    }
}
