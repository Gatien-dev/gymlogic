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
    public class ClientsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Clients
        [Authorize(Roles = "Admin, Afficher les Clients")]
        public ActionResult Index()
        {
            ViewBag.glyphicon = "fa fa-users";
            ViewBag.controller = "Clients";
            ViewBag.action = "Index";
            ViewBag.PageHeader = "Gestion des clients";
            ViewBag.PageDescription = "Ajoutez, Modifiez ou supprimez des clients";
            var clients = db.Clients;
            var listeId = new List<int>();
            if (Session["FirstConsulting"] != null)
            {
                foreach (var client in clients)
                {
                    listeId.Add(client.ID);
                }
                foreach (var id in listeId)
                {
                    db.Clients.Find(id).Nouveau = false;
                }
                db.SaveChanges();
            }
            if (User.IsInRole("Admin") || User.IsInRole("Afficher les nouveaux Clients"))
            {
                Session["FirstConsulting"] = 0;
            }

            return View(db.Clients.Include(c => c.Abonnements).ToList());
        }

        // GET: Clients/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Client client = db.Clients.Include(c => c.Abonnements.Select(a => a.Forfait).Select(f => f.Discipline)).FirstOrDefault(c => c.ID == id);
            if (client == null)
            {
                return HttpNotFound();
            }
            ViewBag.PageHeader = "Détails du client";
            ViewBag.PageDescription = "Informations générales et abonnements en cours";
            return View(client);
        }

        // GET: Clients/Create
        [Authorize(Roles = "Admin, Créer des Clients")]
        public ActionResult Create()
        {
            ViewBag.PageHeader = "Ajout d'un client";
            ViewBag.PageDescription = "Merci de remplir les champs";
            return View();
        }

        // POST: Clients/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Nom,Prenom,Telephone,Adresse,Sexe,Email,Mails")] Client client)
        {
            if (ModelState.IsValid)
            {
                client.Nouveau = true;
                db.Clients.Add(client);
                var userId = User.Identity.GetUserId();
                db.Notifications.Add(new Notification()
                {
                    User = db.Users.Find(userId),
                    Date = DateTime.Now,
                    Archived = false,
                    Tag = "Client",
                    Titre = "Nouveau client!",
                    UserId = userId,
                    Contenu = "" + client.Nom + " " + client.Prenom + " fait desormais partie de nos clients!"
                });
                db.ActivityLogs.Add(new ActivityLog()
                {
                    UserEmail = System.Web.HttpContext.Current.User.Identity.GetUserName(),
                    UserId = User.Identity.GetUserId(),
                    ActivityType = ActivityType.Delete,
                    LogMessage = "Creation du Client" + client.Nom + " " + client.Prenom,
                    Motif = "Inconnu",
                    Date = DateTime.Now
                });
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(client);
        }

        // GET: Clients/Edit/5
        [Authorize(Roles = "Admin, Modifier des clients")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Client client = db.Clients.Find(id);
            if (client == null)
            {
                return HttpNotFound();
            }
            ViewBag.PageHeader = "Modification du client ' " + client.Nom + " " + client.Prenom + " '";
            ViewBag.PageDescription = "Merci de remplir les champs";
            return View(client);
        }

        // POST: Clients/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Nom,Prenom,Telephone,Adresse,Sexe,Email,Mails")] Client client)
        {
            if (ModelState.IsValid)
            {
                db.Entry(client).State = EntityState.Modified;
                db.ActivityLogs.Add(new ActivityLog()
                {
                    UserEmail = System.Web.HttpContext.Current.User.Identity.GetUserName(),
                    UserId = User.Identity.GetUserId(),
                    ActivityType = ActivityType.Display,
                    LogMessage = "Modification du Client" + client.Nom + " " + client.Prenom,
                    Motif = "Inconnu",
                    Date = DateTime.Now
                });
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(client);
        }

        // GET: Clients/Delete/5
        [Authorize(Roles = "Admin, Supprimer des Clients")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var clientsCount = db.Clients.Count();

            Client client = db.Clients.Include(c => c.Abonnements.Select(a => a.Forfait.Discipline)).FirstOrDefault(c => c.ID == id.Value);
            if (client == null)
            {
                return HttpNotFound();
            }
            ViewBag.PageHeader = "Supprimer le client " + client.Nom + " " + client.Prenom + "";
            ViewBag.PageDescription = "Merci de Confirmer la suppression";
            return View(client);
        }

        // POST: Clients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id, string deleteMessage)
        {
            if (deleteMessage == null || deleteMessage == "")
            {

                //Afficher une erreur
                return RedirectToAction("index");
            }


            Client client = db.Clients.Find(id);
            db.Clients.Remove(client);
            db.ActivityLogs.Add(new ActivityLog()
            {
                UserEmail = System.Web.HttpContext.Current.User.Identity.GetUserName(),
                UserId = User.Identity.GetUserId(),
                ActivityType = ActivityType.Delete,
                LogMessage = "Suppression du Client " + client.Nom + " " + client.Prenom,
                Motif = deleteMessage,
                Date = DateTime.Now
            });
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
