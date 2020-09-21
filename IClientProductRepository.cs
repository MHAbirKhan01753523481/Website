using System.Collections.Generic;
using Alphasoft.IRepositories;
using Alphasoft.Models;

namespace Alphasoft.Repositories
{
    public interface IClientProductRepository:IRepository<ClientProduct>
    {
        List<ClientProduct> ClientProjectsByProduct(int productId);

        List<ClientProduct> GetAllWithClientProduct();
       
    }
}