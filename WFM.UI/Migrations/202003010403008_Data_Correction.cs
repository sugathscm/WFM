namespace WFM.UI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Data_Correction : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SourcingPartner",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        Mobile = c.String(maxLength: 50),
                        Email = c.String(maxLength: 50),
                        FixedLine = c.String(maxLength: 50),
                        IsActive = c.Boolean(),
                        SourcingPartnerType = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.WFMConfiguration",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        Value = c.String(maxLength: 25),
                    })
                .PrimaryKey(t => t.Id);
            
            AlterColumn("dbo.Country", "Name", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Country", "Code", c => c.String(maxLength: 10));
            AlterColumn("dbo.Principal", "Name", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Principal", "AddressLine1", c => c.String(maxLength: 50));
            AlterColumn("dbo.Principal", "AddressLine2", c => c.String(maxLength: 50));
            AlterColumn("dbo.Principal", "City", c => c.String(maxLength: 50));
            AlterColumn("dbo.Principal", "Postcode", c => c.String(maxLength: 50));
            AlterColumn("dbo.Principal", "Province", c => c.String(maxLength: 50));
            AlterColumn("dbo.Principal", "Website", c => c.String(maxLength: 50));
            AlterColumn("dbo.Principal", "Email", c => c.String(maxLength: 50));
            AlterColumn("dbo.PrincipalContact", "Name", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.PrincipalContact", "Mobile", c => c.String(maxLength: 50));
            AlterColumn("dbo.PrincipalContact", "Email", c => c.String(maxLength: 50));
            AlterColumn("dbo.PrincipalContact", "FixedLine", c => c.String(maxLength: 50));
            AlterColumn("dbo.ProjectSector", "Name", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.TenderDocumentSection", "Name", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.TenderDocumentType", "Name", c => c.String(nullable: false, maxLength: 100));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TenderDocumentType", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.TenderDocumentSection", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.ProjectSector", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.PrincipalContact", "FixedLine", c => c.String());
            AlterColumn("dbo.PrincipalContact", "Email", c => c.String());
            AlterColumn("dbo.PrincipalContact", "Mobile", c => c.String());
            AlterColumn("dbo.PrincipalContact", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.Principal", "Email", c => c.String());
            AlterColumn("dbo.Principal", "Website", c => c.String());
            AlterColumn("dbo.Principal", "Province", c => c.String());
            AlterColumn("dbo.Principal", "Postcode", c => c.String());
            AlterColumn("dbo.Principal", "City", c => c.String());
            AlterColumn("dbo.Principal", "AddressLine2", c => c.String());
            AlterColumn("dbo.Principal", "AddressLine1", c => c.String());
            AlterColumn("dbo.Principal", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.Country", "Code", c => c.String());
            AlterColumn("dbo.Country", "Name", c => c.String(nullable: false));
            DropTable("dbo.WFMConfiguration");
            DropTable("dbo.SourcingPartner");
        }
    }
}
