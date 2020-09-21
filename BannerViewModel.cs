using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Alphasoft.ViewModels
{
    public class BannerViewModel
    {
        public int Id { get; set; }
        public string Image { get; set; }
        public string SolganOne { get; set; }
        public string SolganTwo { get; set; }
        public string SolganThree { get; set; }
        public int Order { get; set; }
    }
}
