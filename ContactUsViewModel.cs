using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alphasoft.Models;

namespace Alphasoft.ViewModels
{
    public class ContactUsViewModel
    {
        public List<Banner> bannerList { get; set; }

        public AboutUs about { get; set; }

        public List<AboutUs> aboutUs { get; set; }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public Company companyInfo { get; set; }
        public List<Service> Services { get;  set; }
    }
}
