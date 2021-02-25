namespace HomeBudgetManagement.Domain
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIncome : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Income",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Amount = c.Double(nullable: false),
                        Description = c.String(),
                        Date = c.DateTime(nullable: false),
                        File = c.Binary(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Income");
        }
    }
}
