namespace MazadMarket.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class Model1 : DbContext
    {
        public Model1()
            : base("name=Model1")
        {
        }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }






        public System.Data.Entity.DbSet<MazadMarket.Models.Products> Products { get; set; }

        public System.Data.Entity.DbSet<MazadMarket.Models.ProductsType> ProductsTypes { get; set; }

        public System.Data.Entity.DbSet<MazadMarket.Models.ProductsAuction> ProductsAuctions { get; set; }

        public System.Data.Entity.DbSet<MazadMarket.Models.paymentConfirmation> paymentConfirmation { get; set; }






    }
}
