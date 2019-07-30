namespace AdminLTE_MVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Abonnements",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        UserID = c.String(),
                        ClientID = c.Int(nullable: false),
                        ClientCoupleId = c.Int(),
                        ResteAPayer = c.Double(nullable: false),
                        Montant = c.Double(nullable: false),
                        DateCreation = c.DateTime(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        DateDebut = c.DateTime(nullable: false),
                        NbJours = c.Int(nullable: false),
                        NbJoursRestants = c.Int(nullable: false),
                        Suspendu = c.Boolean(nullable: false),
                        Approved = c.Boolean(nullable: false),
                        Renewal = c.Boolean(nullable: false),
                        DateRenouvellement = c.DateTime(nullable: false),
                        ApproverUID = c.String(),
                        ForfaitId = c.Int(nullable: false),
                        MailSent = c.Boolean(nullable: false),
                        LastCheckDate = c.DateTime(nullable: false),
                        Activé = c.Boolean(nullable: false),
                        Discipline_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Clients", t => t.ClientID, cascadeDelete: true)
                .ForeignKey("dbo.Clients", t => t.ClientCoupleId)
                .ForeignKey("dbo.Disciplines", t => t.Discipline_ID)
                .ForeignKey("dbo.Forfaits", t => t.ForfaitId, cascadeDelete: true)
                .Index(t => t.ClientID)
                .Index(t => t.ClientCoupleId)
                .Index(t => t.ForfaitId)
                .Index(t => t.Discipline_ID);
            
            CreateTable(
                "dbo.Clients",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Nom = c.String(nullable: false),
                        Prenom = c.String(nullable: false),
                        Telephone = c.String(),
                        Email = c.String(),
                        Adresse = c.String(),
                        Sexe = c.Int(nullable: false),
                        Nouveau = c.Boolean(nullable: false),
                        Mails = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Forfaits",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        DisciplineID = c.Int(nullable: false),
                        Duree = c.Int(nullable: false),
                        Montant = c.Double(nullable: false),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Disciplines", t => t.DisciplineID, cascadeDelete: true)
                .Index(t => t.DisciplineID);
            
            CreateTable(
                "dbo.Disciplines",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        TitulaireID = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AspNetUsers", t => t.TitulaireID)
                .Index(t => t.TitulaireID);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Nom = c.String(),
                        Prenoms = c.String(),
                        Telephone = c.String(),
                        Adresse = c.String(),
                        LastConnectionDate = c.DateTime(nullable: false),
                        PreviousConnectionDate = c.DateTime(nullable: false),
                        Photo = c.Binary(),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Notifications",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Titre = c.String(),
                        Contenu = c.String(),
                        Date = c.DateTime(nullable: false),
                        Archived = c.Boolean(nullable: false),
                        Tag = c.String(),
                        UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.ActivityLogs",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        UserId = c.String(),
                        UserEmail = c.String(),
                        LogMessage = c.String(),
                        Motif = c.String(),
                        ActivityType = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Abonnements", "ForfaitId", "dbo.Forfaits");
            DropForeignKey("dbo.Disciplines", "TitulaireID", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Notifications", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Forfaits", "DisciplineID", "dbo.Disciplines");
            DropForeignKey("dbo.Abonnements", "Discipline_ID", "dbo.Disciplines");
            DropForeignKey("dbo.Abonnements", "ClientCoupleId", "dbo.Clients");
            DropForeignKey("dbo.Abonnements", "ClientID", "dbo.Clients");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.Notifications", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Disciplines", new[] { "TitulaireID" });
            DropIndex("dbo.Forfaits", new[] { "DisciplineID" });
            DropIndex("dbo.Abonnements", new[] { "Discipline_ID" });
            DropIndex("dbo.Abonnements", new[] { "ForfaitId" });
            DropIndex("dbo.Abonnements", new[] { "ClientCoupleId" });
            DropIndex("dbo.Abonnements", new[] { "ClientID" });
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.ActivityLogs");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.Notifications");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Disciplines");
            DropTable("dbo.Forfaits");
            DropTable("dbo.Clients");
            DropTable("dbo.Abonnements");
        }
    }
}
