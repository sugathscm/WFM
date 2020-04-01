namespace WFM.UI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FK_Correction : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Principal", "Country_Id", "dbo.Country");
            DropForeignKey("dbo.PrincipalContact", "Principal_Id", "dbo.Principal");
            DropForeignKey("dbo.PrincipalContact", "Designation_Id", "dbo.Designation");
            DropIndex("dbo.Principal", new[] { "Country_Id" });
            DropIndex("dbo.PrincipalContact", new[] { "Designation_Id" });
            DropIndex("dbo.PrincipalContact", new[] { "Principal_Id" });
            DropColumn("dbo.Principal", "CountryId");
            DropColumn("dbo.PrincipalContact", "PrincipalId");
            DropColumn("dbo.PrincipalContact", "DesignationId");
            RenameColumn(table: "dbo.Principal", name: "Country_Id", newName: "CountryId");
            RenameColumn(table: "dbo.PrincipalContact", name: "Principal_Id", newName: "PrincipalId");
            RenameColumn(table: "dbo.PrincipalContact", name: "Designation_Id", newName: "DesignationId");
            AlterColumn("dbo.Principal", "CountryId", c => c.Int(nullable: false));
            AlterColumn("dbo.Principal", "CountryId", c => c.Int(nullable: false));
            AlterColumn("dbo.PrincipalContact", "DesignationId", c => c.Int(nullable: false));
            AlterColumn("dbo.PrincipalContact", "PrincipalId", c => c.Int(nullable: false));
            AlterColumn("dbo.PrincipalContact", "DesignationId", c => c.Int(nullable: false));
            AlterColumn("dbo.PrincipalContact", "PrincipalId", c => c.Int(nullable: false));
            CreateIndex("dbo.Principal", "CountryId");
            CreateIndex("dbo.PrincipalContact", "DesignationId");
            CreateIndex("dbo.PrincipalContact", "PrincipalId");
            AddForeignKey("dbo.Principal", "CountryId", "dbo.Country", "Id", cascadeDelete: true);
            AddForeignKey("dbo.PrincipalContact", "PrincipalId", "dbo.Principal", "Id", cascadeDelete: true);
            AddForeignKey("dbo.PrincipalContact", "DesignationId", "dbo.Designation", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PrincipalContact", "DesignationId", "dbo.Designation");
            DropForeignKey("dbo.PrincipalContact", "PrincipalId", "dbo.Principal");
            DropForeignKey("dbo.Principal", "CountryId", "dbo.Country");
            DropIndex("dbo.PrincipalContact", new[] { "PrincipalId" });
            DropIndex("dbo.PrincipalContact", new[] { "DesignationId" });
            DropIndex("dbo.Principal", new[] { "CountryId" });
            AlterColumn("dbo.PrincipalContact", "PrincipalId", c => c.Int());
            AlterColumn("dbo.PrincipalContact", "DesignationId", c => c.Int());
            AlterColumn("dbo.PrincipalContact", "PrincipalId", c => c.String());
            AlterColumn("dbo.PrincipalContact", "DesignationId", c => c.String());
            AlterColumn("dbo.Principal", "CountryId", c => c.Int());
            AlterColumn("dbo.Principal", "CountryId", c => c.String());
            RenameColumn(table: "dbo.PrincipalContact", name: "DesignationId", newName: "Designation_Id");
            RenameColumn(table: "dbo.PrincipalContact", name: "PrincipalId", newName: "Principal_Id");
            RenameColumn(table: "dbo.Principal", name: "CountryId", newName: "Country_Id");
            AddColumn("dbo.PrincipalContact", "DesignationId", c => c.String());
            AddColumn("dbo.PrincipalContact", "PrincipalId", c => c.String());
            AddColumn("dbo.Principal", "CountryId", c => c.String());
            CreateIndex("dbo.PrincipalContact", "Principal_Id");
            CreateIndex("dbo.PrincipalContact", "Designation_Id");
            CreateIndex("dbo.Principal", "Country_Id");
            AddForeignKey("dbo.PrincipalContact", "Designation_Id", "dbo.Designation", "Id");
            AddForeignKey("dbo.PrincipalContact", "Principal_Id", "dbo.Principal", "Id");
            AddForeignKey("dbo.Principal", "Country_Id", "dbo.Country", "Id");
        }
    }
}
