using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alphasoft.Models;

namespace Alphasoft.ViewModels
{
    public class OurTeamViewModel
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public string DepartmentName { get; set; }
        public string DesignationName { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public string Facebook { get; set; }
        public string Twitter { get; set; }
        public string LinkedIn { get; set; }
        public int Order { get; set; }
    }
}
