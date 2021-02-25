namespace HomeBudgetManagement.Domain
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IncomeFileExtensionField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Income", "FileExtension", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Income", "FileExtension");
        }
    }
}
