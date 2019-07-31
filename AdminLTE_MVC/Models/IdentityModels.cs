
using Gymlogic.Models.Data;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentitySample.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        //[Display(Name ="Email")]
        //public override string UserName { get; set; }
        public string Nom { get; set; }
        public string Prenoms { get; set; }
        public List<Notification> Notifications { get; set; }
        public List<History> History { get; set; }
        [Phone]
        public string Telephone { get; set; }
        public string Adresse { get; set; }
        [Display(Name ="Date de derniere connexion")]
        public DateTime LastConnectionDate { get; set; }
        [Display(Name ="Date de la precedente connexion")]
        public DateTime PreviousConnectionDate { get; set; }
        public byte[] Photo { get; set; }
        //public string Email { get; set; }
        public virtual List<Discipline> Disciplines { get; set; }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<Forfait>().HasRequired(f => f.Discipline).WithMany(d => d.Forfaits).WillCascadeOnDelete(false);
            modelBuilder.Entity<Discipline>().HasMany(d => d.Forfaits).WithRequired(f => f.Discipline).WillCascadeOnDelete(true);
        }
        static ApplicationDbContext()
        {
            // Set the database intializer which is run once during application start
            // This seeds the database with admin user credentials and admin role
            Database.SetInitializer<ApplicationDbContext>(new ApplicationDbInitializer());

        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public System.Data.Entity.DbSet<Gymlogic.Models.Data.Client> Clients { get; set; }

        public System.Data.Entity.DbSet<Gymlogic.Models.Data.Forfait> Forfaits { get; set; }

        public System.Data.Entity.DbSet<Gymlogic.Models.Data.Discipline> Disciplines { get; set; }

        public System.Data.Entity.DbSet<Gymlogic.Models.Data.Abonnement> Abonnements { get; set; }
        public System.Data.Entity.DbSet<Gymlogic.Models.Data.Notification> Notifications { get; set; }

        public System.Data.Entity.DbSet<AdminLTE_MVC.Models.ActivityLog> ActivityLogs { get; set; }

        //public System.Data.Entity.DbSet<IdentitySample.Models.ApplicationUser> ApplicationUsers { get; set; }
    }
}