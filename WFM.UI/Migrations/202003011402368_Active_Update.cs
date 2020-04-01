namespace WFM.UI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Active_Update : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Country", "IsActive", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Principal", "IsActive", c => c.Boolean(nullable: false));
            AlterColumn("dbo.PrincipalContact", "IsActive", c => c.Boolean(nullable: false));
            AlterColumn("dbo.ProjectSector", "IsActive", c => c.Boolean(nullable: false));
            AlterColumn("dbo.SourcingPartner", "IsActive", c => c.Boolean(nullable: false));
            AlterColumn("dbo.TenderDocumentSection", "IsActive", c => c.Boolean(nullable: false));
            AlterColumn("dbo.TenderDocumentType", "IsActive", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TenderDocumentType", "IsActive", c => c.Boolean());
            AlterColumn("dbo.TenderDocumentSection", "IsActive", c => c.Boolean());
            AlterColumn("dbo.SourcingPartner", "IsActive", c => c.Boolean());
            AlterColumn("dbo.ProjectSector", "IsActive", c => c.Boolean());
            AlterColumn("dbo.PrincipalContact", "IsActive", c => c.Boolean());
            AlterColumn("dbo.Principal", "IsActive", c => c.Boolean());
            AlterColumn("dbo.Country", "IsActive", c => c.Boolean());
        }
    }
}
