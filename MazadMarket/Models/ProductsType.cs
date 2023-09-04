using MazadMarket.Resourses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MazadMarket.Models
{
    public class ProductsType
    {


        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name = "typeName", ResourceType =typeof(layout))]
        public string typeName { get; set; }
     
        [Display(Name = "Picture", ResourceType = typeof(layout))]
        public string Picture { get; set; }
        public virtual ICollection<Products> Products { get; set; }
    }
}