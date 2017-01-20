namespace Slingshot.LogicLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatedCampaignAndRecipientModels : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Campaigns", "prefared", c => c.Boolean(nullable: false));
            AddColumn("dbo.Recipients", "cell", c => c.String());
            AddColumn("dbo.Recipients", "jobTitle", c => c.String());
            AddColumn("dbo.Recipients", "country", c => c.String());
            AddColumn("dbo.Recipients", "province", c => c.String());
            AddColumn("dbo.Recipients", "city", c => c.String());
            AddColumn("dbo.Recipients", "street", c => c.String());
            AddColumn("dbo.Recipients", "code", c => c.String());
            AddColumn("dbo.VCards", "province", c => c.String());
            AddColumn("dbo.VCards", "street", c => c.String());
            DropColumn("dbo.Recipients", "phone");
            DropColumn("dbo.VCards", "fileAs");
            DropTable("dbo.ClientVCards");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ClientVCards",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        clientId = c.Long(nullable: false),
                        imageId = c.Long(nullable: false),
                        profileImage = c.String(),
                        firstName = c.String(),
                        lastName = c.String(),
                        company = c.String(),
                        jobTitle = c.String(),
                        fileAs = c.String(),
                        email = c.String(),
                        twitter = c.String(),
                        webPageAddress = c.String(),
                        businessPhoneNumber = c.String(),
                        mobileNUmber = c.String(),
                        country = c.String(),
                        city = c.String(),
                        code = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.VCards", "fileAs", c => c.String());
            AddColumn("dbo.Recipients", "phone", c => c.String());
            DropColumn("dbo.VCards", "street");
            DropColumn("dbo.VCards", "province");
            DropColumn("dbo.Recipients", "code");
            DropColumn("dbo.Recipients", "street");
            DropColumn("dbo.Recipients", "city");
            DropColumn("dbo.Recipients", "province");
            DropColumn("dbo.Recipients", "country");
            DropColumn("dbo.Recipients", "jobTitle");
            DropColumn("dbo.Recipients", "cell");
            DropColumn("dbo.Campaigns", "prefared");
        }
    }
}
