namespace Slingshot.LogicLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class xx : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Emails", "campaignId", "dbo.Campaigns");
            DropForeignKey("dbo.Attachments", "emailId", "dbo.Emails");
            DropIndex("dbo.Attachments", new[] { "emailId" });
            DropIndex("dbo.Emails", new[] { "campaignId" });
            DropColumn("dbo.Attachments", "Id");
            DropColumn("dbo.Emails", "Id");
            RenameColumn(table: "dbo.Emails", name: "campaignId", newName: "Id");
            RenameColumn(table: "dbo.Attachments", name: "emailId", newName: "Id");
            DropPrimaryKey("dbo.Attachments");
            DropPrimaryKey("dbo.Emails");
            AlterColumn("dbo.Attachments", "Id", c => c.Long(nullable: false));
            AlterColumn("dbo.Emails", "Id", c => c.Long(nullable: false));
            AddPrimaryKey("dbo.Attachments", "Id");
            AddPrimaryKey("dbo.Emails", "Id");
            CreateIndex("dbo.Attachments", "Id");
            CreateIndex("dbo.Emails", "Id");
            AddForeignKey("dbo.Emails", "Id", "dbo.Campaigns", "Id");
            AddForeignKey("dbo.Attachments", "Id", "dbo.Emails", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Attachments", "Id", "dbo.Emails");
            DropForeignKey("dbo.Emails", "Id", "dbo.Campaigns");
            DropIndex("dbo.Emails", new[] { "Id" });
            DropIndex("dbo.Attachments", new[] { "Id" });
            DropPrimaryKey("dbo.Emails");
            DropPrimaryKey("dbo.Attachments");
            AlterColumn("dbo.Emails", "Id", c => c.Long(nullable: false, identity: true));
            AlterColumn("dbo.Attachments", "Id", c => c.Long(nullable: false, identity: true));
            AddPrimaryKey("dbo.Emails", "Id");
            AddPrimaryKey("dbo.Attachments", "Id");
            RenameColumn(table: "dbo.Attachments", name: "Id", newName: "emailId");
            RenameColumn(table: "dbo.Emails", name: "Id", newName: "campaignId");
            AddColumn("dbo.Emails", "Id", c => c.Long(nullable: false, identity: true));
            AddColumn("dbo.Attachments", "Id", c => c.Long(nullable: false, identity: true));
            CreateIndex("dbo.Emails", "campaignId");
            CreateIndex("dbo.Attachments", "emailId");
            AddForeignKey("dbo.Attachments", "emailId", "dbo.Emails", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Emails", "campaignId", "dbo.Campaigns", "Id", cascadeDelete: true);
        }
    }
}
