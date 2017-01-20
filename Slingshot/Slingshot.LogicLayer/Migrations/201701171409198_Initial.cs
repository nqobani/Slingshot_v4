namespace Slingshot.LogicLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Attachments",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        emailId = c.Long(nullable: false),
                        name = c.String(),
                        file = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Emails", t => t.emailId, cascadeDelete: true)
                .Index(t => t.emailId);
            
            CreateTable(
                "dbo.Campaigns",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        creatorId = c.String(),
                        name = c.String(),
                        status = c.String(),
                        thumbnail = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Emails",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        campaignId = c.Long(nullable: false),
                        subject = c.String(),
                        html = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Campaigns", t => t.campaignId, cascadeDelete: true)
                .Index(t => t.campaignId);
            
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
            
            CreateTable(
                "dbo.Events",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        title = c.String(),
                        startDateTime = c.DateTime(nullable: false),
                        endDateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Histories",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        userId = c.String(),
                        imageId = c.Long(nullable: false),
                        campaignId = c.Long(nullable: false),
                        sentDateTime = c.DateTime(nullable: false),
                        toEmail = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Images",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        imagPath = c.String(),
                        captureDateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Recipients",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        userId = c.String(),
                        firstName = c.String(),
                        lastName = c.String(),
                        email = c.String(),
                        phone = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserCampaigns",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        userId = c.String(),
                        campaignId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.VCards",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        userId = c.String(maxLength: 128),
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
                        mobileNumber = c.String(),
                        country = c.String(),
                        city = c.String(),
                        code = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.userId)
                .Index(t => t.userId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        FirstName = c.String(),
                        LastName = c.String(),
                        type = c.String(),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.VCards", "userId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Emails", "campaignId", "dbo.Campaigns");
            DropForeignKey("dbo.Attachments", "emailId", "dbo.Emails");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.VCards", new[] { "userId" });
            DropIndex("dbo.Emails", new[] { "campaignId" });
            DropIndex("dbo.Attachments", new[] { "emailId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.VCards");
            DropTable("dbo.UserCampaigns");
            DropTable("dbo.Recipients");
            DropTable("dbo.Images");
            DropTable("dbo.Histories");
            DropTable("dbo.Events");
            DropTable("dbo.ClientVCards");
            DropTable("dbo.Emails");
            DropTable("dbo.Campaigns");
            DropTable("dbo.Attachments");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
        }
    }
}
