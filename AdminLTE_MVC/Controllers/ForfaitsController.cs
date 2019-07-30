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
    public class ForfaitsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Forfaits
        [Authorize(Roles = "Admin,Afficher les Forfaits")]
        public ActionResult Index()
        {
            var forfaits = db.Forfaits.Include(f => f.Discipline);
            return View(forfaits.ToList());
        }

        // GET: Forfaits/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Forfait forfait = db.Forfaits.Include(f => f.Discipline).Include(f => f.Abonnements).FirstOrDefault(f => f.ID == id);
            if (forfait == null)
            {
                return HttpNotFound();
            }
            return View(forfait);
        }

        // GET: Forfaits/Create
        [Authorize(Roles = "Admin,Créer des Forfaits")]
        public ActionResult Create()
        {
            ViewBag.DisciplineID = new SelectList(db.Disciplines, "ID", "Name");
            return View();
        }

        // POST: Forfaits/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,DisciplineID,Duree,Montant,Description")] Forfait forfait)
        {
            if (ModelState.IsValid)
            {
                db.Forfaits.Add(forfait);
                db.SaveChanges();
                db.Notifications.Add(new Notification()
                {
                    User = db.Users.Find(User.Identity.GetUserId()),
                    Date = DateTime.Now,
                    Archived = false,
                    Tag = "Forfait",
                    Titre = "Nouveau Forfait!",
                    UserId = User.Identity.GetUserId(),
                    Contenu = "Nouveau forfait pour " + db.Disciplines.Find(forfait.DisciplineID).Name
                });
                db.ActivityLogs.Add(new ActivityLog()
                {
                    UserEmail = System.Web.HttpContext.Current.User.Identity.GetUserName(),
                    UserId = User.Identity.GetUserId(),
                    ActivityType = ActivityType.Delete,
                    LogMessage = "Nouveau forfait pour " + db.Disciplines.Find(forfait.DisciplineID).Name,
                    Motif = "Inconnu",
                    Date = DateTime.Now
                });
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DisciplineID = new SelectList(db.Disciplines, "ID", "Name", forfait.DisciplineID);
            return View(forfait);
        }

        // GET: Forfaits/Edit/5
        [Authorize(Roles = "Admin,Modifier des Forfaits")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Forfait forfait = db.Forfaits.Find(id);
            if (forfait == null)
            {
                return HttpNotFound();
            }
            ViewBag.DisciplineID = new SelectList(db.Disciplines, "ID", "Name", forfait.DisciplineID);
            List<Abonnement> abonnements = db.Abonnements.Where(a => a.Forfait.ID == forfait.ID).ToList();
            ViewBag.AbonnementsCount = abonnements.Count();
            if (abonnements == null)
            {
                ViewBag.AbonnementsCount = 0;
            }
            ViewBag.PageHeader = "Modification du forfait";
            ViewBag.PageDescription = "La modification n'est possible que si le forfait n'est pas encore utilisé";
            ViewBag.Abonnements = abonnements;
            return View(forfait);
        }

        // POST: Forfaits/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,DisciplineID,Duree,Montant,Description")] Forfait forfait)
        {
            if (ModelState.IsValid)
            {
                db.Entry(forfait).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DisciplineID = new SelectList(db.Disciplines, "ID", "Name", forfait.DisciplineID);

            return View(forfait);
        }

        // GET: Forfaits/Delete/5
        [Authorize(Roles = "Admin,Supprimer des Forfaits")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Forfait forfait = db.Forfaits.Include(f => f.Discipline).FirstOrDefault(f => f.ID == id);
            if (forfait == null)
            {
                return HttpNotFound();
            }
            List<Abonnement> abonnements = db.Abonnements.Where(a => a.Forfait.ID == forfait.ID && a.NbJoursRestants > 0).ToList();
            ViewBag.AbonnementsCount = abonnements.Count();
            if (abonnements.Count() > 0)
            {
                this.AddToastMessage("Suppression Impossible", "Des abonnements en cours utilisent ce forfait", ToastType.Error);
                return RedirectToAction("index");
            }
            if (abonnements == null)
            {
                ViewBag.AbonnementsCount = 0;
            }
            ViewBag.Abonnements = abonnements;
            return View(forfait);
        }

        // POST: Forfaits/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id, string deleteMessage)
        {
            Forfait forfait = db.Forfaits.Include(f => f.Discipline).First(f => f.ID == id);
            db.ActivityLogs.Add(new ActivityLog()
            {
                UserEmail = System.Web.HttpContext.Current.User.Identity.GetUserName(),
                UserId = User.Identity.GetUserId(),
                ActivityType = ActivityType.Delete,
                LogMessage = "Suppression du forfait " + forfait.Discipline.Name,
                Motif = deleteMessage,
                Date = DateTime.Now
            });
            db.Forfaits.Remove(forfait);
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
