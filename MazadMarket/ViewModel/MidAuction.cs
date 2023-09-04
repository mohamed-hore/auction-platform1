using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MazadMarket.ViewModel
{
    public class MidAuction
    {


        
        public int Id { get; set; }

        public int ProductsId { get; set; }
        public string productsName{ get; set; }

        public string userId { get; set; }
        public double riseAmount { get; set; }

    
        public DateTime dateOfRise { get; set; }


    }
}