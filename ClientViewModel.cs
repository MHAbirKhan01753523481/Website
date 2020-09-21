using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Alphasoft.ViewModels
{
    public class ClientViewModel
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public string Logo { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        [Display(Name = "Client Says About Us")]
        public string ClientSaysAboutUs { get; set; }
    }
}
