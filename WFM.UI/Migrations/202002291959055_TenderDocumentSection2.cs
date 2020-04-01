namespace WFM.UI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TenderDocumentSection2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TenderDocumentSection",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        ParentId = c.Int(nullable: false),
                        IsActive = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.TenderDocumentSection");
        }
    }
}
