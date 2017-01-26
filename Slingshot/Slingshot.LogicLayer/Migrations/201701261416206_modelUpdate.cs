namespace Slingshot.LogicLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modelUpdate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Recipients", "company", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Recipients", "company");
        }
    }
}
