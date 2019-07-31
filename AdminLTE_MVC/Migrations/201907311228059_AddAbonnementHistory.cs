namespace AdminLTE_MVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAbonnementHistory : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Histories",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Action = c.String(),
                        Date = c.DateTime(nullable: false),
                        UserId = c.Guid(nullable: false),
                        AbonnementID = c.Int(nullable: false),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Abonnements", t => t.AbonnementID, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.AbonnementID)
                .Index(t => t.User_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Histories", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Histories", "AbonnementID", "dbo.Abonnements");
            DropIndex("dbo.Histories", new[] { "User_Id" });
            DropIndex("dbo.Histories", new[] { "AbonnementID" });
            DropTable("dbo.Histories");
        }
    }
}
