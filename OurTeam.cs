using System.ComponentModel.DataAnnotations;

namespace Alphasoft.Models
{
    public class OurTeam : Base
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int DepartmentId { get; set; }
        public Department Department { get; set; }

        [Display(Name = "Designation")]
        public int DesignationId { get; set; }
        public Designation Designation { get; set; }

        public string Image { get; set; }
        public string Description { get; set; }

        [Url]
        public string Facebook { get; set; }

        [Url]
        public string Twitter { get; set; }

        [Url]
        public string LinkedIn { get; set; }
    }
}
