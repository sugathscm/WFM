namespace WFM.UI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TenderDocumentSection : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ProjectSector", "ParentId", "dbo.ProjectSector");
            DropIndex("dbo.ProjectSector", new[] { "ParentId" });
        }
        
        public override void Down()
        {
            CreateIndex("dbo.ProjectSector", "ParentId");
            AddForeignKey("dbo.ProjectSector", "ParentId", "dbo.ProjectSector", "Id");
        }
    }
}
