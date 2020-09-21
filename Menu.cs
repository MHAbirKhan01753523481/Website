using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Alphasoft.Models
{
    public class Menu : Base
    {
        public Menu()
        {
            SubMenus = new List<SubMenu>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        [Display(Name = "Controller Name")]
        public string ControllerName { get; set; }

        [Display(Name = "Action Name")]
        public string ActionName { get; set; }

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }
        public int ParentId { get; set; }
        public List<SubMenu> SubMenus { get; set; }
        public string DropdownType { get; set; }
    }
}
