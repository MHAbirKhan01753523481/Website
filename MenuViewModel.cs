using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alphasoft.Models;

namespace Alphasoft.ViewModels
{
    public class MenuViewModel
    {
        public MenuViewModel()
        {
            SubMenus = new List<SubMenu>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public bool IsActive { get; set; }
        public int ParentId { get; set; }
        public string DropdownType { get; set; }
        public int Order { get; set; }
        public List<SubMenu> SubMenus { get; set; }
    }
}
