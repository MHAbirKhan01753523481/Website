using Alphasoft.Models;
using System.Collections.Generic;

namespace Alphasoft.IRepositories
{
    public interface IServiceRepository : IRepository<Service>
    {
        List<Service> GetAllWithServiceCategory();
    }
}
