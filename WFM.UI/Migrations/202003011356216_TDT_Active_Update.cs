namespace WFM.UI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TDT_Active_Update : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TenderDocumentType", "IsActive", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.TenderDocumentType", "IsActive");
        }
    }
}
