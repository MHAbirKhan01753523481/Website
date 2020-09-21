using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Alphasoft.ViewModels
{
    public class BlogsViewModel
    {
        public int Id { get; set; }
        public string Image { get; set; }
        public string Creator { get; set; }
        public string Thumbnail { get; set; }
        public string VideoUrl { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public string ChangeDate { get; set; }
      
    }
}
