using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AdminLTE_MVC.Models;
using Gymlogic.Models.Data;
using IdentitySample.Models;
using Microsoft.AspNet.Identity;

namespace AdminLTE_MVC.Controllers
{
    [Authorize]
    public class DisciplinesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Disciplines
        [Authorize(Roles = "Admin,Afficher les Disciplines")]
        public ActionResult Index()
        {
            var disciplines = db.Disciplines.Include(d => d.Titulaire);
            ViewBag.glyphicon = "ion ion-android-settings";
            ViewBag.controller = "Disciplines";
            ViewBag.action = "Index";
            ViewBag.PageHeader = "Gestion des Disciplines";
            ViewBag.PageDescription = "Ajoutez, Modifiez ou supprimez des clients";
            return View(disciplines.ToList());
        }

        // GET: Disciplines/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Discipline discipline = db.Disciplines.Include(d => d.Abonnements).FirstOrDefault();
            if (discipline == null)
            {
                return HttpNotFound();
            }
            ViewBag.glyphicon = "ion ion-android-settings";
            ViewBag.controller = "Disciplines";
            ViewBag.action = "Details";
            ViewBag.PageHeader = "Gestion des Disciplines";
            ViewBag.PageDescription = "Details de la discipline " + discipline.Name;
            return View(discipline);
        }

        // GET: Disciplines/Create
        [Authorize(Roles = "Admin, Créer des disciplines")]
        public ActionResult Create()
        {
            ViewBag.TitulaireID = new SelectList(db.Users.Where(u=>u.UserName!="Admin"), "Id", "Nom");
            return View();
        }

        // POST: Disciplines/Create
        // To protect from overposting attacks, please enable the specific p roperties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,TitulaireID")] Discipline discipline)
        {
            if (ModelState.IsValid)
            {
                db.Disciplines.Add(discipline);
                db.SaveChanges();
                db.Notifications.Add(new Notification()
                {
                    User = db.Users.Find(User.Identity.GetUserId()),
                    Date = DateTime.Now,
                    Archived = false,
                    Tag = "Discipline",
                    Titre = "Nouvelle discipline!",
                    UserId = User.Identity.GetUserId(),
                    Contenu = " " + discipline.Name + " fait desormais partie de nos disciplines"
                });
                db.ActivityLogs.Add(new ActivityLog()
                {
                    UserEmail = System.Web.HttpContext.Current.User.Identity.GetUserName(),
                    UserId = User.Identity.GetUserId(),
                    ActivityType = ActivityType.Delete,
                    LogMessage = "Creation de discipline discipline" + discipline.Name,
                    Motif = "Inconnu",
                    Date = DateTime.Now
                });
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.TitulaireID = new SelectList(db.Users, "Id", "Nom", discipline.TitulaireID);
            return View(discipline);
        }

        // GET: Disciplines/Edit/5
        [Authorize(Roles = "Admin, Modifier des disciplines")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Discipline discipline = db.Disciplines.Find(id);
            if (discipline == null)
            {
                return HttpNotFound();
            }
            ViewBag.TitulaireID = new SelectList(db.Users, "Id", "Nom", discipline.TitulaireID);
            ViewBag.glyphicon = "ion ion-android-settings";
            ViewBag.controller = "Disciplines";
            ViewBag.action = "Modifier";
            ViewBag.PageHeader = "Modification de la discipline ";
            ViewBag.PageDescription = discipline.Name;
            return View(discipline);
        }

        // POST: Disciplines/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,TitulaireID")] Discipline discipline)
        {
            if (ModelState.IsValid)
            {
                db.Entry(discipline).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TitulaireID = new SelectList(db.Users, "Id", "Nom", discipline.TitulaireID);
            return View(discipline);
        }

        // GET: Disciplines/Delete/5
        [Authorize(Roles = "Admin, Supprimer des disciplines")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Discipline discipline = db.Disciplines.Include(d => d.Abonnements).FirstOrDefault();
            if (discipline == null)
            {
                return HttpNotFound();
            }
            if (db.Forfaits.Any(f => f.Discipline.ID == id))
            {
                this.AddToastMessage("Impossible de supprimer", "La discipline est utilisee dans les forfaits, merci de supprimer les forfaits concernés.",ToastType.Error);
                return RedirectToAction("index");
            }
            return View(discipline);
        }

        // POST: Disciplines/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id, string deleteMessage)
        {
            Discipline discipline = db.Disciplines.Find(id);
            
            db.ActivityLogs.Add(new ActivityLog()
            {
                UserEmail = System.Web.HttpContext.Current.User.Identity.GetUserName(),
                UserId = User.Identity.GetUserId(),
                ActivityType = ActivityType.Delete,
                LogMessage = "Suppression de la discipline " + discipline.Name,
                Motif = deleteMessage,
                Date = DateTime.Now
            });
            db.Disciplines.Remove(discipline);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
