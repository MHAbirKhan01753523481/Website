using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Alphasoft.Models
{
    public class Company : Base
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Logo { get; set; }
        public string Slogan { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [Url]
        public string Facebook { get; set; }
        [Url]
        public string LinkedIn { get; set; }
        [Url]
        public string Twitter { get; set; }
        [Url]
        public string Youtube { get; set; }
        public string Favicon { get; set; }

        [Display(Name = "Short Description")]
        public string ShortDescription { get; set; }
        public string Description { get; set; }

        [Display(Name = "Google Map Location")]
        public string GoogleMapLocation { get; set; }
    }
}
