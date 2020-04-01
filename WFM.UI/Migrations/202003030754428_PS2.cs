namespace WFM.UI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PS2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ProjectSector", "ParentId", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ProjectSector", "ParentId", c => c.Int(nullable: false));
        }
    }
}
