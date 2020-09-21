using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Alphasoft.Models
{
    public class Reservation
    {
        public int Id { get; set; }
     
        public string ReservationType { get; set; }
        public DateTime Date { get; set; }
        public int Adult { get; set; }
        public int Child { get; set; }
        public int Children { get; set; }
        //public DateTime ChoseTimeStart { get; set; }
        //public DateTime ChoseTimeEnd { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DeliveryAddress { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
       




    }
}
