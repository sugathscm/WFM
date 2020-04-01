namespace WFM.UI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FourTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Country",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Code = c.String(),
                        IsActive = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Principal",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        AddressLine1 = c.String(),
                        AddressLine2 = c.String(),
                        City = c.String(),
                        Postcode = c.String(),
                        Province = c.String(),
                        CountryId = c.String(),
                        Website = c.String(),
                        Email = c.String(),
                        IsActive = c.Boolean(),
                        Country_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Country", t => t.Country_Id)
                .Index(t => t.Country_Id);
            
            CreateTable(
                "dbo.PrincipalContact",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Name = c.String(nullable: false),
                        Mobile = c.String(),
                        Email = c.String(),
                        FixedLine = c.String(),
                        IsActive = c.Boolean(),
                        DesignationId = c.String(),
                        PrincipalId = c.String(),
                        Designation_Id = c.Int(),
                        Principal_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Designation", t => t.Designation_Id)
                .ForeignKey("dbo.Principal", t => t.Principal_Id)
                .Index(t => t.Designation_Id)
                .Index(t => t.Principal_Id);
            
            CreateTable(
                "dbo.Designation",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        IsActive = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PrincipalContact", "Principal_Id", "dbo.Principal");
            DropForeignKey("dbo.PrincipalContact", "Designation_Id", "dbo.Designation");
            DropForeignKey("dbo.Principal", "Country_Id", "dbo.Country");
            DropIndex("dbo.PrincipalContact", new[] { "Principal_Id" });
            DropIndex("dbo.PrincipalContact", new[] { "Designation_Id" });
            DropIndex("dbo.Principal", new[] { "Country_Id" });
            DropTable("dbo.Designation");
            DropTable("dbo.PrincipalContact");
            DropTable("dbo.Principal");
            DropTable("dbo.Country");
        }
    }
}
