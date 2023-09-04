using System;
using MazadMarket.Resourses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace MazadMarket.Models
{
    public class Image_table

    {
        [Key]
        public int Id { get; set; }
        public string path { get; set; }
        public string extinction { get; set; }
        public virtual Products Products { get; set; }

    }
}



