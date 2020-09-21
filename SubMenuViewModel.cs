using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alphasoft.Models;

namespace Alphasoft.ViewModels
{
    public class SubMenuViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string MenuName { get; set; }
      
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public bool IsActive { get; set; }
    }
}
