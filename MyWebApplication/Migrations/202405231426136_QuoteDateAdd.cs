namespace MyWebApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class QuoteDateAdd : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Quotes", "CreateAt", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Quotes", "CreateAt");
        }
    }
}
