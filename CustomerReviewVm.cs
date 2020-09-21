using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Alphasoft.ViewModels
{
    public class CustomerReviewVm
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public double Rating { get; set; }
        public string Review { get; set; }
    }
}
