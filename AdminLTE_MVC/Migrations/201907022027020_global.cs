namespace AdminLTE_MVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class global : DbMigration
    {
        public override void Up()
        {
            //AddColumn("dbo.Abonnements", "DateDecompte", c => c.DateTime(nullable: false));
            AddColumn("dbo.Abonnements", "DateFin", c => c.DateTime(nullable: false));
            RenameColumn("dbo.Abonnements", "DateRenouvellement", "DateDecompte");
        }
        
        public override void Down()
        {
            //AddColumn("dbo.Abonnements", "DateRenouvellement", c => c.DateTime(nullable: false));
            DropColumn("dbo.Abonnements", "DateFin");
            RenameColumn("dbo.Abonnements", "DateDecompte", "DateRenouvellement");
        }
    }
}
