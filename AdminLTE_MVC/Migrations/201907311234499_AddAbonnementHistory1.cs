namespace AdminLTE_MVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAbonnementHistory1 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Histories", new[] { "User_Id" });
            DropColumn("dbo.Histories", "UserId");
            RenameColumn(table: "dbo.Histories", name: "User_Id", newName: "UserId");
            AlterColumn("dbo.Histories", "UserId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Histories", "UserId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Histories", new[] { "UserId" });
            AlterColumn("dbo.Histories", "UserId", c => c.Guid(nullable: false));
            RenameColumn(table: "dbo.Histories", name: "UserId", newName: "User_Id");
            AddColumn("dbo.Histories", "UserId", c => c.Guid(nullable: false));
            CreateIndex("dbo.Histories", "User_Id");
        }
    }
}
