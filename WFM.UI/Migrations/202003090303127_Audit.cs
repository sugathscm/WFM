namespace WFM.UI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Audit : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DataAudit",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Guid(),
                        Entity = c.String(),
                        OldData = c.String(),
                        NewData = c.String(),
                        UpdatedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.LoginAudit",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Guid(),
                        IPAddress = c.String(),
                        DateLogged = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.LoginAudit");
            DropTable("dbo.DataAudit");
        }
    }
}
