using MazadMarket.Resourses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MazadMarket.Models
{




    public class Products
    {
        [Key]
        public int Id { get; set; }



        //[Required(ErrorMessage ="xxxxxxxxxxxxxxxxxx") ]

        [Display(Name = "productName", ResourceType = typeof(layout))]

        public string productName { get; set; }
        //forgen key

        [Display(Name = "ProductsTypeId", ResourceType = typeof(layout))]
        public int ProductsTypeId { get; set; }

        [Display(Name = "userId", ResourceType = typeof(layout))]
        public string userId { get; set; }


        [Display(Name = "fullName", ResourceType = typeof(layout))]
        public string fullName { get; set; }


        //[Display(Name = "Name:")]
        //public string fullName { get; set; }


        [Display(Name = "Picture", ResourceType = typeof(layout))]
        public string Picture { get; set; }





        [Display(Name = "description", ResourceType = typeof(layout))]
        public string Description { get; set; }


        [Display(Name = "Price", ResourceType = typeof(layout))]
        public double Price { get; set; }

        //[Required]
        [Display(Name = "minRaise", ResourceType = typeof(layout))]
        public double minRaise { get; set; }

        //[Required]
        [Display(Name = "dateOfAdd", ResourceType = typeof(layout))]
        public DateTime dateOfAdd { get; set; }

        ///[Required]
        [Display(Name = "dateOfApprove", ResourceType = typeof(layout))]
        public DateTime dateOfApprove { get; set; }



        [Display(Name = "dateOfAuctionStart", ResourceType = typeof(layout))]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd HH:mm}")]
        [DataType(DataType.Date)]
        public DateTime dateOfAuctionStart { get; set; }




        [Display(Name = "dateOfAuctionEnd", ResourceType = typeof(layout))]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd HH:mm}")]
        [DataType(DataType.Date)]
        public DateTime dateOfAuctionEnd { get; set; }


        [Display(Name = "approved", ResourceType = typeof(layout))]
        public bool approved { get; set; }
        [Display(Name = "sold", ResourceType = typeof(layout))]
        public bool sold { get; set; }


        [Display(Name = "dateOfSell", ResourceType = typeof(layout))]
        public DateTime dateOfSell { get; set; }


        [Display(Name = "bankAccountNumber", ResourceType = typeof(layout))]
        public string bankAccountNumber { get; set; }



        public virtual ProductsType ProductsType { get; set; }

        public virtual ICollection<ProductsAuction> ProductsAuction { get; set; }



        public virtual paymentConfirmation paymentConfirmation { get; set; }

        public virtual ICollection<Image_table> Image_table { get; set; }

    }
}