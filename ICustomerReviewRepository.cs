using Alphasoft.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Alphasoft.IRepositories
{
  public interface ICustomerReviewRepository:IRepository<CustomerRivew>
    {
        List<CustomerRivew> GetWithCustomerReview();
        CustomerRivew WithCustomerRivew();
    }
}
