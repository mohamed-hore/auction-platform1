using MazadMarket.Resourses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MazadMarket.Models
{
    public class paymentConfirmation

    {
        [Key]
        public int Id { get; set; }


        [Display(Name = "userId", ResourceType = typeof(layout))]
        public string userId { get; set; }

        [Display(Name = "productName", ResourceType = typeof(layout))]
        public int ProductsId { get; set; }

       

        [Display(Name = "depositValue", ResourceType = typeof(layout))]
        public string depositValue { get; set; }



        [Display(Name = "cCV", ResourceType = typeof(layout))]
        public string cCV { get; set; }

        
        [Display(Name = "visaCard", ResourceType = typeof(layout))]
        public string visaCard { get; set; }

        [Display(Name = "cardExpirationDate", ResourceType = typeof(layout))]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd HH:mm}")]
        [DataType(DataType.Date)]
        public DateTime cardExpirationDate { get; set; }

        [Display(Name = "canBid", ResourceType = typeof(layout))]
        public bool canBid { get; set; }


        public virtual ProductsAuction ProductsAuction { get; set; }





    }

}
    

