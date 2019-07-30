using AdminLTE_MVC.Models;
using Gymlogic.Models.Data;
using Gymlogic.Models.ViewModels;
using IdentitySample.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Configuration;
using System.Web.Mvc;

namespace AdminLTE_MVC.Controllers
{
    public class AbonnementsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Abonnements
        [Authorize(Roles = "Admin,Afficher les Abonnements")]
        public ActionResult Index()
        {

            var abonnements = db.Abonnements.Include(a => a.Client).Include(a => a.Forfait).Where(a => a.Approved); ViewBag.glyphicon = "fa fa-link";
            ViewBag.controller = "Abonnements";
            ViewBag.action = "Index";
            ViewBag.PageHeader = "Gestion des abonnements";
            ViewBag.PageDescription = "Ajoutez, Modifiez, suspendez ou supprimez des abonnements.";
            return View(abonnements.ToList());
        }

        public ActionResult Expiring()
        {

            int abonnementAlertDays = Int32.Parse(WebConfigurationManager.AppSettings["AbonnementAlertDays"]);
            var twoDaysBeforeNow = DateTime.Now.AddDays(-2);
            var abonnements = db.Abonnements.Where(a => a.NbJoursRestants > 0 && a.NbJoursRestants <= abonnementAlertDays);
            ViewBag.glyphicon = "fa fa-time";
            ViewBag.controller = "Abonnements";
            ViewBag.action = "Abonnements presque Terminés";
            return View(abonnements.ToList());
        }
        public ActionResult Expired()
        {
            int abonnementAlertDays = Int32.Parse(WebConfigurationManager.AppSettings["AbonnementAlertDays"]);
            var abonnements = db.Abonnements.Include(a => a.Client).Include(a => a.Forfait).Where(a => a.DateFin > DateTime.Now);
            ViewBag.controller = "Abonnements";
            ViewBag.action = "Abonnements Terminés ";
            return View(abonnements.ToList());
        }
        public ActionResult All()
        {

            ViewBag.glyphicon = "fa fa-link";
            ViewBag.controller = "Abonnements";
            ViewBag.action = "Tous";
            ViewBag.PageHeader = "Abonnements en cours";
            ViewBag.PageDescription = "Liste des abonnements en cours.";
            var abonnements = db.Abonnements.Include(a => a.Client).Include(a => a.Forfait); ViewBag.glyphicon = "fa fa-link";
            ViewBag.controller = "Abonnements";
            ViewBag.action = "Index";
            ViewBag.PageHeader = "Gestion des abonnements";
            ViewBag.PageDescription = "Ajoutez, Modifiez, suspendez ou supprimez des abonnements.";
            return View(abonnements.ToList());
        }
        public ActionResult fixEndDates()
        {
            //var allAbo = db.Abonnements.Include(a => a.Forfait).ToList();
            //for (int i = 0; i < allAbo.Count(); i++)
            //{
            //    var abo = db.Abonnements.Where(a => a.ID == allAbo[i].ID).Include(a => a.Forfait).First();
            //    abo.DateFin = abo.DateDebut.AddDays(abo.Forfait.Duree);
            //}
            foreach (var abo in db.Abonnements.Where(a => a.Forfait.Duree > 30).Include(a => a.Forfait))
            {
                abo.DateFin = abo.DateDebut.AddDays(abo.Forfait.Duree);
            }
            db.SaveChanges();

            Session["ListeAValider"] = null;
            Session["listeNonValidee"] = null;
            return RedirectToAction("all");
        }

        public ActionResult Suspended()
        {

            ViewBag.glyphicon = "fa fa-link";
            ViewBag.controller = "Abonnements";
            ViewBag.action = "Arretés";
            ViewBag.PageHeader = "Abonnements Arrêtés";
            ViewBag.PageDescription = "Liste des abonnements Arrêtés.";
            var abonnements = db.Abonnements.Include(a => a.Client).Include(a => a.Forfait).Where(a => a.Suspendu); ViewBag.glyphicon = "fa fa-link";
            ViewBag.controller = "Abonnements";
            ViewBag.action = "Index";
            return View(abonnements.ToList());
        }

        public ActionResult Disabled()
        {

            ViewBag.glyphicon = "fa fa-link";
            ViewBag.controller = "Abonnements";
            ViewBag.action = "Inactifs";
            ViewBag.PageHeader = "Abonnements Inactifs";
            ViewBag.PageDescription = "Liste des abonnements désactivés.";
            var abonnements = db.Abonnements.Include(a => a.Client).Include(a => a.Forfait).Where(a => !a.Activé); ViewBag.glyphicon = "fa fa-link";
            ViewBag.controller = "Abonnements";
            ViewBag.action = "Index";
            return View(abonnements.ToList());
        }
        [Authorize(Roles = "Admin,Afficher les Abonnements")]
        public ActionResult Unchecked()
        {
            /*DateTime deflt = DateTime.Now.AddYears(-50)*/
            ;
            DateTime dateDebut = new DateTime(), dateFin = new DateTime();
            if (Session["dateDebut"] != null)
            {
                dateDebut = DateTime.Parse(Session["dateDebut"].ToString());
            }
            if (Session["dateFin"] != null)
            {
                dateFin = DateTime.Parse(Session["dateFin"].ToString());
            }
            List<Abonnement> listeNonValidee;
            if (Session["listeAValider"] == null)
            {
                Session["listeAValider"] = new List<Abonnement>();
            }

            if (Session["listeNonValidee"] == null)
            {
                Session["listeNonValidee"] = db.Abonnements.Where(a => !a.Approved).Include(a => a.Client).Include(a => a.Forfait).Where(a => !a.Approved || a.NbJoursRestants <= 0).ToList();
            }
            var users = db.Users.ToList();
            listeNonValidee = (List<Abonnement>)Session["listeNonValidee"];
            if (!dateDebut.Equals(new DateTime()))
            {
                listeNonValidee = listeNonValidee.Where(a => a.DateCreation > dateDebut).ToList();
            }
            if (!dateFin.Equals(new DateTime()))
            {
                listeNonValidee = listeNonValidee.Where(a => a.DateCreation < dateFin).ToList();
            }
            ViewBag.glyphicon = "fa fa-link";
            ViewBag.controller = "Abonnements";
            ViewBag.action = "Unchecked";
            ViewBag.PageHeader = "Gestion des abonnements non comptabilisés";
            ViewBag.PageDescription = "Comptabilisez les abonnements.";
            ViewBag.listeAValider = Session["listeAValider"];
            ViewBag.users = users;
            return View(listeNonValidee);
        }

        [HttpPost]
        public ActionResult ApplyDates(DateTime dateDebut, DateTime dateFin)
        {
            Session["dateDebut"] = dateDebut.ToShortDateString();
            Session["dateFin"] = dateFin.ToShortDateString();
            return RedirectToAction("Unchecked");
        }
        public ActionResult ResetFilters()
        {
            Session["dateDebut"] = null;
            Session["dateFin"] = null;
            return RedirectToAction("Unchecked");
        }
        public ActionResult AddToValider(int id)
        {
            List<Abonnement> listeNonValidee;
            listeNonValidee = (List<Abonnement>)Session["listeNonValidee"];
            List<Abonnement> listeAValider = (List<Abonnement>)Session["ListeAValider"];
            Abonnement AboAValider;
            if (listeNonValidee.Any(a => a.ID == id))
            {
                AboAValider = listeNonValidee.First(a => a.ID == id);
                listeNonValidee.Remove(AboAValider);
                listeAValider.Add(AboAValider);
            }

            return RedirectToAction("Unchecked");
        }
        [Authorize(Roles = "Admin,Valider les Abonnements")]
        public ActionResult ValiderSelection()
        {

            return View();
        }

        [HttpPost]
        public ActionResult ValiderSelection(int nothing = 0)
        {
            if (Session["ListeAValider"] != null)
            {
                List<Abonnement> listeAValider = (List<Abonnement>)Session["ListeAValider"];
                foreach (var abo in listeAValider)
                {
                    var abonnement = db.Abonnements.FirstOrDefault(a => a.ID == abo.ID);
                    abonnement.Approved = true;
                    abonnement.ApproverUID = User.Identity.GetUserId();
                }
                db.SaveChanges();
            }
            Session["ListeAValider"] = null;
            Session["listeNonValidee"] = null;
            return RedirectToAction("Unchecked");
        }

        public ActionResult RemoveFromValider(int id)
        {
            List<Abonnement> listeAValider;
            listeAValider = (List<Abonnement>)Session["ListeAValider"];
            List<Abonnement> listeNonValidee = (List<Abonnement>)Session["listeNonValidee"];
            Abonnement AboAValider;
            if (listeAValider.Any(a => a.ID == id))
            {
                AboAValider = listeAValider.First(a => a.ID == id);
                listeAValider.Remove(AboAValider);
                listeNonValidee.Add(AboAValider);
            }

            return RedirectToAction("Unchecked");
        }
        public ActionResult CheckAll()
        {
            var abonnements = db.Abonnements.Where(a => !a.Approved);
            //for (int i = 0; i < abonnements.Count(); i++)
            //{
            //    var abo = abonnements.ElementAt(i);
            //    abo.Approved = true;
            //    abo.ApproverUID = User.Identity.GetUserId();
            //}
            foreach (var abo in abonnements)
            {
                //abonnements.ElementAt(i);
                abo.Approved = true;
                abo.ApproverUID = User.Identity.GetUserId();
            }
            db.SaveChanges();
            return RedirectToAction("Unchecked");
        }

        // GET: Abonnements/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Abonnement abonnement = db.Abonnements.Where(a => a.ID == id).Include(a => a.Client.Abonnements.Select(ab => ab.Forfait.Discipline)).FirstOrDefault();
            if (abonnement == null)
            {
                return HttpNotFound();
            }
            ViewBag.approver = db.Users.FirstOrDefault(u => u.Id == abonnement.ApproverUID);
            return View(abonnement);
        }

        [Authorize(Roles = "Admin,Valider les Abonnements")]
        public ActionResult Valider(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Abonnement abonnement = db.Abonnements.Where(a => a.ID == id).Include(a => a.Client.Abonnements.Select(ab => ab.Forfait.Discipline)).FirstOrDefault();
            if (abonnement == null)
            {
                return HttpNotFound();
            }
            ViewBag.PageHeader = "Valider l'abonnement";
            ViewBag.PageDescription = "Merci de confirmer la validation.";
            ViewBag.approver = db.Users.FirstOrDefault(u => u.Id == abonnement.ApproverUID);
            return View(abonnement);
        }
        [HttpPost, ActionName("Valider")]
        [ValidateAntiForgeryToken]
        public ActionResult ValidationConfirmed(int id)
        {
            Abonnement abonnement = db.Abonnements.Find(id);
            abonnement.Approved = true;
            abonnement.ApproverUID = User.Identity.GetUserId();
            db.SaveChanges();
            Session["ListeAValider"] = null;
            Session["listeNonValidee"] = null;
            return RedirectToAction("Unchecked");
        }

        [Authorize(Roles = "Admin,Suspendre des Abonnements")]
        public ActionResult Suspendre(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Abonnement abonnement = db.Abonnements.Where(a => a.ID == id).Include(a => a.Client.Abonnements.Select(ab => ab.Forfait.Discipline)).FirstOrDefault();
            if (abonnement == null)
            {
                return HttpNotFound();
            }
            ViewBag.PageHeader = "Arrêter l'abonnement";
            ViewBag.PageDescription = "Merci de confirmer l'Arrêt";
            ViewBag.approver = db.Users.FirstOrDefault(u => u.Id == abonnement.ApproverUID);
            return View(abonnement);
        }
        [HttpPost, ActionName("Suspendre")]
        [ValidateAntiForgeryToken]
        public ActionResult SuspensionConfirmed(int id, DateTime? date)
        {

            Abonnement abonnement = db.Abonnements.Find(id);
            var now = DateTime.Now;
            if (date != null)
            {
                if (date > DateTime.Now)
                {
                    this.AddToastMessage("Erreur de date", "La date de suspension est posterieure a la date actuelle.", ToastType.Error);
                    return RedirectToAction("all");
                }
                var selectedDate = date.Value;
                var delta = (now - selectedDate).TotalDays;
                abonnement.NbJoursRestants += (int)delta;
            }
            abonnement.Suspendu = true;

            db.SaveChanges();
            return RedirectToAction("all");
        }

        // GET: Abonnements/Create
        [Authorize(Roles = "Admin,Créer des Abonnements")]
        public ActionResult Create()
        {
            ViewBag.PageHeader = "Nouvel abonnement";
            ViewBag.PageDescription = "Merci de remplir tous les champs.";
            var contentClients = db.Clients.Select(c => new { Id = c.ID, Nom = c.Nom + "  " + c.Prenom });
            ViewBag.ClientID = new SelectList(contentClients, "ID", "Nom");
            var content = db.Forfaits.Select(f => new { Id = f.ID, Description = f.Description + " -- " + f.Montant });
            ViewBag.ForfaitId = new SelectList(content, "ID", "Description");
            return View(new AbonnementCreateVM());
        }

        // POST: Abonnements/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AbonnementCreateVM abonnement)
        {//ID,Nom,Prenom,Telephone,Adresse,Sexe,Email,Mails


            var clientNew = new Client()
            {
                Nom = abonnement.Nom,
                Prenom = abonnement.Prenom,
                Telephone = abonnement.Telephone,
                Adresse = abonnement.Adresse,
                Sexe = abonnement.Sexe,
                Email = abonnement.Email,
                Mails = abonnement.Mails
            };
            var abo = new Abonnement()
            {
                DateDebut = abonnement.DateDebut,
                DateCreation = abonnement.DateRecu,
                CreationDate = DateTime.Now,
                ForfaitId = abonnement.ForfaitId
            };

            var client = db.Clients.FirstOrDefault(c => c.ID == abonnement.ClientID);
            if (abonnement.NewCLientCheck)
            {
                client = clientNew;
                db.Clients.Add(client);
                db.SaveChanges();
            }
            if (ModelState.IsValid)
            {
                var discipline = db.Forfaits.Include(f => f.Discipline).First(f => f.ID == abo.ForfaitId).Discipline;
                if (db.Abonnements.Any(a => a.Client.ID == abo.ClientID && a.NbJoursRestants > 1 && a.Forfait.Discipline.ID == discipline.ID))
                {
                    this.AddToastMessage("Client deja abonné", "Le client est deja abonné à cette discipline. Vous pouvez prolonger l'abonnement en procédant à un renouvellement.", ToastType.Error);
                    return RedirectToAction("all");
                }
                var forfait = db.Forfaits.FirstOrDefault(f => f.ID == abo.ForfaitId);
                abo.Montant = forfait.Montant;
                abo.ResteAPayer = forfait.Montant - abonnement.paye;
                if (abonnement.paye == 0)
                {
                    abo.ResteAPayer = 0;
                }

                abo.Client = client;
                abo.ClientID = client.ID;
                abo.DateCreation = abonnement.DateRecu;
                abo.CreationDate = DateTime.Now;
                abo.NbJours = forfait.Duree;
                abo.NbJoursRestants = forfait.Duree;
                abo.Approved = false;
                abo.Renewal = false;
                abo.Suspendu = false;
                abo.MailSent = false;
                abo.UserID = User.Identity.GetUserId();
                abo.DateFin = DateTime.Now.AddDays(forfait.Duree);
                //abo.DateRenouvellement = DateTime.Now;
                abo.DateDecompte = DateTime.Now;
                abo.LastCheckDate = DateTime.Now;
                abo.Activé = abonnement.Activé;
                db.Abonnements.Add(abo);
                db.SaveChanges();

                db.Notifications.Add(new Notification()
                {
                    User = db.Users.Find(User.Identity.GetUserId()),
                    Date = DateTime.Now,
                    Archived = false,
                    Tag = "Abonnement",
                    Titre = "Nouvel Abonnement!",
                    UserId = User.Identity.GetUserId(),
                    Contenu = "" + client.Nom + " " + client.Prenom + " fait desormais partie de nos clients!"
                });
                db.ActivityLogs.Add(new ActivityLog()
                {
                    UserEmail = System.Web.HttpContext.Current.User.Identity.GetUserName(),
                    UserId = User.Identity.GetUserId(),
                    ActivityType = ActivityType.Delete,
                    LogMessage = "Nouvel Abonnement " + client.Nom + " " + client.Prenom,
                    Motif = "Inconnu",
                    Date = DateTime.Now
                });

                Session["ListeAValider"] = null;
                Session["listeNonValidee"] = null;
                return RedirectToAction("all");
            }

            ViewBag.ClientID = new SelectList(db.Clients, "ID", "Nom", abo.ClientID);
            ViewBag.Clients = db.Clients.ToList();

            ViewBag.ForfaitId = new SelectList(db.Forfaits, "ID", "Description", abo.ForfaitId);
            return View(abonnement);
        }

        // GET: Abonnements/Edit/5
        [Authorize(Roles = "Admin,Modifier des Abonnements")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Abonnement abonnement = db.Abonnements.Find(id);
            if (abonnement == null)
            {
                return HttpNotFound();
            }

            ViewBag.PageHeader = "Modification de l'abonnement";
            //ViewBag.PageDescription =abonnement.Client.Nom+ "Ajoutez, Modifiez, suspendez ou supprimez des abonnements.";
            ViewBag.ClientID = new SelectList(db.Clients, "ID", "Nom", abonnement.ClientID);
            ViewBag.ForfaitId = new SelectList(db.Forfaits, "ID", "Description", abonnement.ForfaitId);
            return View(abonnement);
        }

        // POST: Abonnements/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,UserID,ClientID,Montant,DateCreation,DateDebut,NbJours,NbJoursRestants,Etat,Approved,NewAbo,DateDecompte,ApproverUID,ForfaitId")] Abonnement abonnement)
        {
            if (ModelState.IsValid)
            {
                var discipline = db.Forfaits.Include(f => f.Discipline).First(f => f.ID == abonnement.ForfaitId).Discipline;
                if (db.Abonnements.Any(a => a.Client.ID == abonnement.ClientID && a.NbJoursRestants > 1 && a.Forfait.Discipline.ID == discipline.ID && a.ID != abonnement.ID))
                {
                    this.AddToastMessage("Client deja abonné", "Le client est deja abonné à cette discipline. Vous pouvez prolonger l'abonnement en procédant à un renouvellement.", ToastType.Error);
                    return RedirectToAction("all");
                }
                var forfait = db.Forfaits.FirstOrDefault(f => f.ID == abonnement.ForfaitId);
                var client = db.Clients.FirstOrDefault(c => c.ID == abonnement.ClientID);
                var ab = db.Abonnements.Find(abonnement.ID);
                ab.Montant = forfait.Montant;
                ab.Client = client;
                ab.ClientID = client.ID;
                ab.Forfait = forfait;
                ab.ForfaitId = forfait.ID;
                ab.DateCreation = DateTime.Now;
                ab.MailSent = false;
                ab.NbJours = forfait.Duree;
                ab.NbJoursRestants = forfait.Duree;
                //ab.Approved = false;
                ab.Renewal = false;
                ab.Suspendu = false;
                //ab.ApproverUID = User.Identity.GetUserId();
                ab.DateDecompte = DateTime.Now;
                ab.DateDebut = abonnement.DateDebut;
                ab.DateFin = DateTime.Now.AddDays(forfait.Duree);
                //db.Abonnements.Add(abonnement);
                db.SaveChanges();
                //db.Entry(abonnement).State = EntityState.Modified;
                //db.SaveChanges();

                Session["ListeAValider"] = null;
                Session["listeNonValidee"] = null;
                return RedirectToAction("all");
            }

            Session["ListeAValider"] = null;
            Session["listeNonValidee"] = null;
            ViewBag.ClientID = new SelectList(db.Clients, "ID", "Nom", abonnement.ClientID);
            ViewBag.ForfaitId = new SelectList(db.Forfaits, "ID", "Description", abonnement.ForfaitId);
            return View(abonnement);
        }

        // GET: Abonnements/Delete/5
        [Authorize(Roles = "Admin,Supprimer les Abonnements")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Abonnement abonnement = db.Abonnements.Include(a => a.Forfait.Discipline).First(a => a.ID == id);
            if (abonnement == null)
            {
                return HttpNotFound();
            }
            ViewBag.motherAbo = db.Abonnements.Where(a => a.Client.ID == abonnement.Client.ID).Include(a => a.Forfait.Discipline).ToList();
            ViewBag.agentValidation = db.Users.FirstOrDefault(u => u.Id == abonnement.UserID);
            ViewBag.agentSaisie = db.Users.First(u => u.Id == abonnement.UserID);
            return View(abonnement);
        }

        // POST: Abonnements/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id, string deleteMessage)
        {
            Abonnement abonnement = db.Abonnements.Include(a => a.Forfait.Discipline).Include(a => a.Client).First(a => a.ID == id);
            db.ActivityLogs.Add(new ActivityLog()
            {
                UserEmail = System.Web.HttpContext.Current.User.Identity.GetUserName(),
                UserId = User.Identity.GetUserId(),
                ActivityType = ActivityType.Delete,
                LogMessage = "Suppression de l'abonnement de " + abonnement.Client.Nom + " " + abonnement.Client.Prenom + " dans la discipline " + abonnement.Forfait.Discipline.Name,
                Motif = deleteMessage,
                Date = DateTime.Now
            });
            db.Abonnements.Remove(abonnement);
            db.Entry(abonnement).State = EntityState.Deleted;
            db.SaveChanges();
            Session["ListeAValider"] = null;
            Session["listeNonValidee"] = null;
            return RedirectToAction("all");
        }


        [Authorize(Roles = "Admin,Reprendre des Abonnements")]
        public ActionResult Resume(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Abonnement abonnement = db.Abonnements.Include(a => a.Forfait.Discipline).First(a => a.ID == id);
            if (abonnement == null)
            {
                return HttpNotFound();
            }
            ViewBag.Title = "Reprendre l'abonnement";
            ViewBag.PageHeader = "Reprise de l'abonnement";
            ViewBag.PageDescription = "Merci de confirmer la reprise.";
            return View(abonnement);
        }
        public ActionResult Activer(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Abonnement abonnement = db.Abonnements.Include(a => a.Forfait.Discipline).First(a => a.ID == id);
            if (abonnement == null)
            {
                return HttpNotFound();
            }

            ViewBag.PageHeader = "Activation de l'abonnement";
            ViewBag.PageDescription = "Merci de confirmer l'activation.";
            return View(abonnement);
        }

        [Authorize(Roles = "Admin,Payer des Abonnements")]
        public ActionResult Payer(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Abonnement abonnement = db.Abonnements.Include(a => a.Forfait.Discipline).First(a => a.ID == id);
            if (abonnement == null)
            {
                return HttpNotFound();
            }

            ViewBag.PageHeader = "Activation de l'abonnement";
            ViewBag.PageDescription = "Merci de confirmer l'activation.";
            return View(abonnement);
        }
        [HttpPost, ActionName("Payer")]
        public ActionResult PayConfirmed(int id, double montant)
        {
            Abonnement abonnement = db.Abonnements.Find(id);
            abonnement.ResteAPayer -= montant;
            if (abonnement.ResteAPayer < 0)
            {
                abonnement.ResteAPayer = 0;
            }
            db.SaveChanges();


            Session["ListeAValider"] = null;
            Session["listeNonValidee"] = null;
            return RedirectToAction("all");
        }
        [HttpPost, ActionName("Activer")]
        public ActionResult ActivationConfirmed(int id, DateTime? date)
        {
            Abonnement abonnement = db.Abonnements.Where(a => a.ID == id).Include(a => a.Forfait).FirstOrDefault();
            abonnement.Activé = true; abonnement.MailSent = false; abonnement.Suspendu = false;
            abonnement.DateDebut = DateTime.Now;
            abonnement.DateFin = DateTime.Now.AddDays(abonnement.Forfait.Duree);
            if (date != null)
            {
                abonnement.DateDebut = date.Value;
                abonnement.LastCheckDate = date.Value;
            }
            db.SaveChanges();

            Session["ListeAValider"] = null;
            Session["listeNonValidee"] = null;
            return RedirectToAction("all");
        }

        // POST: Abonnements/Delete/5
        [HttpPost, ActionName("Resume")]
        [ValidateAntiForgeryToken]
        public ActionResult ResumeConfirmed(int id, DateTime? date)
        {
            Abonnement abonnement = db.Abonnements.Find(id);
            var now = DateTime.Now;
            if (date != null)
            {
                if (date > DateTime.Now)
                {
                    this.AddToastMessage("Erreur de date", "La date de suspension net peut être postérieure à la date actuelle.", ToastType.Error);
                    return RedirectToAction("unchecked");
                }
                abonnement.DateFin = date.Value.AddDays(abonnement.NbJoursRestants);
                var delta = DateTime.Now - date;
                abonnement.NbJoursRestants -= (int)delta.Value.Days;
                Utilities.checkAbonnements();
            };
            abonnement.Suspendu = false; abonnement.MailSent = false;
            db.SaveChanges();

            Session["ListeAValider"] = null;
            Session["listeNonValidee"] = null;
            return RedirectToAction("all");
        }

        [Authorize(Roles = "Admin,Renouveler des Abonnements")]
        public ActionResult Renouveler(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Abonnement abonnement = db.Abonnements.Include(a => a.Forfait.Discipline).First(a => a.ID == id);
            if (abonnement == null)
            {
                return HttpNotFound();
            }
            ViewBag.PageHeader = "Renouvellement de l'abonnement";
            ViewBag.PageDescription = "Merci de confirmer le renouvellement.";
            return View(abonnement);
        }

        // POST: Abonnements/Delete/5
        [HttpPost, ActionName("Renouveler")]
        [ValidateAntiForgeryToken]
        public ActionResult RenewConfirmed(int id, DateTime datedebut)
        {
            Abonnement abonnement = db.Abonnements.Include(a => a.Forfait.Discipline).First(a => a.ID == id);
            abonnement.NbJoursRestants += abonnement.Forfait.Duree;
            //abonnement.DateRenouvellement = DateTime.Now;
            abonnement.DateDebut = datedebut;
            abonnement.DateFin = datedebut.AddDays(abonnement.Forfait.Duree);
            abonnement.Renewal = true; abonnement.MailSent = false;
            db.SaveChanges();

            Session["ListeAValider"] = null;
            Session["listeNonValidee"] = null;
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
