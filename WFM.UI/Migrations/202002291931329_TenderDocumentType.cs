namespace WFM.UI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TenderDocumentType : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TenderDocumentType",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.TenderDocumentType");
        }
    }
}
