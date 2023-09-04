using MazadMarket.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MazadMarket.ViewModel
{
    public class HomePageViewModel
    {

        public  List<Products> lastAddedProduct { get; set; }

        public List<Products> lastSoldProduct { get; set; }

        public List<Products> lastOfOurProduct { get; set; }



    }
}