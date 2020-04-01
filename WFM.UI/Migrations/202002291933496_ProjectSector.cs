namespace WFM.UI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProjectSector : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProjectSector",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        ParentId = c.Int(nullable: false),
                        IsActive = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ProjectSector", t => t.ParentId)
                .Index(t => t.ParentId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProjectSector", "ParentId", "dbo.ProjectSector");
            DropIndex("dbo.ProjectSector", new[] { "ParentId" });
            DropTable("dbo.ProjectSector");
        }
    }
}
