namespace MazadMarket.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class abcd : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "fullName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Products", "fullName");
        }
    }
}
