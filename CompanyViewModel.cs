using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Alphasoft.ViewModels
{
    public class CompanyViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Logo { get; set; }
        public string Slogan { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Facebook { get; set; }
        public string LinkedIn { get; set; }
        public string Twitter { get; set; }
        public string Youtube { get; set; }
        public string Favicon { get; set; }

        [Display(Name = "Short Description")]
        public string ShortDescription { get; set; }
        public string Description { get; set; }

        [Display(Name = "Google Map Location")]
        public string GoogleMapLocation { get; set; }
    }
}
