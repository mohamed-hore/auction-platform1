namespace MazadMarket.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class abc : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Products", "canrise");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Products", "canrise", c => c.Boolean(nullable: false));
        }
    }
}
