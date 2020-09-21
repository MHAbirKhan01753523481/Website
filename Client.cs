using System;
using System.ComponentModel.DataAnnotations;

namespace Alphasoft.Models
{
    public class Client : Base
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
