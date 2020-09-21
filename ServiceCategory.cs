using System.Collections.Generic;

namespace Alphasoft.Models
{
    public class ServiceCategory : Base
    {
        public ServiceCategory()
        {
            Services = new List<Service>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public List<Service> Services { get; set; }
    }
}
