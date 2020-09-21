using Alphasoft.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Alphasoft.ViewModels
{
    public class HomeViewModel
    {
        public HomeViewModel()
        {
            ChooseUsList = new List<ChooseUs>();
            ClientList = new List<Client>();
            BannerList = new List<Banner>();
            ClientProductList = new List<ClientProduct>();
            ProductList = new List<Product>();
            HostingPlans = new List<HostingPlan>();
        }
        public List<ChooseUs> ChooseUsList { get; set; }
        public List<Client> ClientList { get; set; }
        public List<Banner> BannerList { get; set; }
        public List<ClientProduct> ClientProductList { get; set; }
        public List<Product> ProductList { get; set; }
        public List<HostingPlan> HostingPlans { get; set; }
    }
}
