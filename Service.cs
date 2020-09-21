using System.ComponentModel.DataAnnotations;

namespace Alphasoft.Models
{
    public class Service : Base
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string PlanDes { get; set; }
        public string StrategyDes { get; set; }
        public string WorkDes { get; set; }
        public string QualityDes { get; set; }
        public string Image { get; set; }
        public double? Price { get; set; }
        public string Description { get; set; }
        public string IconClassName { get; set; }
      

        [Display(Name = "Short Description")]
        public string ShortDescription { get; set; }

        [Display(Name = "Service Category")]
        public int ServiceCategoryId { get; set; }
        public ServiceCategory ServiceCategory { get; set; }
    }
}