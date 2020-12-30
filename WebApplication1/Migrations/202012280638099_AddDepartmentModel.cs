namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDepartmentModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Departments",
                c => new
                    {
                        DptId = c.String(nullable: false, maxLength: 128),
                        Name_C = c.String(),
                        Name_E = c.String(),
                        Loc = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                        LastActivityDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.DptId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Departments");
        }
    }
}
