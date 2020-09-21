using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Alphasoft.Models
{
    public class BlogComment : Base
    {
       

        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Comment { get; set; }
        [DisplayName("Name")]
        public string RName { get; set; }
        [DisplayName("Email")]
        public string REmail { get; set; }
        [DisplayName("Comment")]
        public string Reply { get; set; }
        public int Rating { get; set; }
        public string Review { get; set; }
        public int BlogId { get; set; }
        public Blog Blog { get; set; }

    }
}
