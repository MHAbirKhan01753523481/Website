using System.ComponentModel.DataAnnotations;

namespace Alphasoft.Models
{
    public class SubMenu : Base
    {
        public int Id { get; set; }
        public string Name { get; set; }

        [Display(Name = "Menu")]
        public int MenuId { get; set; }
        public Menu Menu { get; set; }

        [Display(Name="Controller Name")]
        public string ControllerName { get; set; }

        [Display(Name = "Action Name")]
        public string ActionName { get; set; }

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }
    }
}
