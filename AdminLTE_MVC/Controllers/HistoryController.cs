using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Gymlogic.Models.Data;
using IdentitySample.Models;

namespace AdminLTE_MVC.Controllers
{
    public class HistoryController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: History
        public ActionResult Index(int abonnementId)
        {
            var histories = db.Histories.Include(h => h.Abonnement).Include(h => h.User);
            if (abonnementId != 0)
            {
                var abonnement = db.Abonnements.Where(a => a.ID == abonnementId).Include(a=>a.Client).Include(a => a.Forfait.Discipline).First();
                ViewBag.abonnement = abonnement;
                histories=db.Histories.Where(h=>h.AbonnementID==abonnementId).Include(h => h.Abonnement).Include(h => h.User);
            }
            return View(histories.ToList());
        }

        // GET: History/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            History history = db.Histories.Find(id);
            if (history == null)
            {
                return HttpNotFound();
            }
            return View(history);
        }

        // GET: History/Create
        public ActionResult Create()
        {
            ViewBag.AbonnementID = new SelectList(db.Abonnements, "ID", "UserID");
            ViewBag.UserId = new SelectList(db.Users, "Id", "Nom");
            return View();
        }

        // POST: History/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Action,Date,UserId,AbonnementID")] History history)
        {
            if (ModelState.IsValid)
            {
                db.Histories.Add(history);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AbonnementID = new SelectList(db.Abonnements, "ID", "UserID", history.AbonnementID);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Nom", history.UserId);
            return View(history);
        }

        // GET: History/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            History history = db.Histories.Find(id);
            if (history == null)
            {
                return HttpNotFound();
            }
            ViewBag.AbonnementID = new SelectList(db.Abonnements, "ID", "UserID", history.AbonnementID);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Nom", history.UserId);
            return View(history);
        }

        // POST: History/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Action,Date,UserId,AbonnementID")] History history)
        {
            if (ModelState.IsValid)
            {
                db.Entry(history).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AbonnementID = new SelectList(db.Abonnements, "ID", "UserID", history.AbonnementID);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Nom", history.UserId);
            return View(history);
        }

        // GET: History/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            History history = db.Histories.Find(id);
            if (history == null)
            {
                return HttpNotFound();
            }
            return View(history);
        }

        // POST: History/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            History history = db.Histories.Find(id);
            db.Histories.Remove(history);
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
