namespace HomeBudgetManagement.Domain
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FileColumns : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Expense",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                        Amount = c.Double(nullable: false),
                        Date = c.DateTime(nullable: false),
                        File = c.Binary(),
                        FileExtension = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Expense");
        }
    }
}
