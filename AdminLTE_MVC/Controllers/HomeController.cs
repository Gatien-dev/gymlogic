using AdminLTE_MVC.Models;
using Gymlogic.Models.Data;
using IdentitySample.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace IdentitySample.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            var db = new ApplicationDbContext();
            int abonnementAlertDays = Int32.Parse(WebConfigurationManager.AppSettings["AbonnementAlertDays"]);
            ViewBag.glyphicon = "fa fa-home";
            ViewBag.controller = "Accueil";
            ViewBag.PageHeader = "GymLogic";
            ViewBag.action = "Tableau de bord";
            ViewBag.PageDescription = "Votre logiciel de gestion de GYM";
            //this.AddToastMessage("My first message", "A lot of important text", ToastType.Success);

            ViewBag.suspendedAboCount = db.Abonnements.Where(a=>a.Suspendu).Count();
            ViewBag.renewalsCount = db.Abonnements.Where(a => a.NbJoursRestants >= 1 && !a.Suspendu && a.Activé).Count();
            //var twoDaysFromNow = a.DateFin.AddDays(2);
            var twoDaysBeforeNow = DateTime.Now.AddDays(-2);
            var almostExpired = db.Abonnements.Where(a => a.NbJoursRestants > 0 && a.NbJoursRestants <= abonnementAlertDays);
            ViewBag.almostExpiredCount = almostExpired.Count();
            ViewBag.abonnenemtsCount = db.Abonnements.Count();
            //var running = db.Abonnements.Where(a => a.NbJoursRestants > 0);
            ViewBag.expiredCount = db.Abonnements.Where(a => a.NbJoursRestants < 1).Count();
            if (new ApplicationDbContext().Abonnements.Any(a => a.NbJoursRestants <= abonnementAlertDays))
            {
                this.AddToastMessage("Abonnements presque terminés.", "Certains abonnements sont terminés ou presque.Merci de consulter les abonnements.");
            }
            return View();
        }
        public void TickAbonnement(Abonnement abonnement)
        {
            //var ab = 

        }
        [HttpGet]
        [Authorize]
        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        [HttpGet]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public FileContentResult UserPhotos()
        {
            if (User.Identity.IsAuthenticated)
            {
                String userId = User.Identity.GetUserId();

                //if (userId == null)
                //{
                string fileName = HttpContext.Server.MapPath(@"~/Images/" + User.Identity.GetUserId() + ".png");
                if (!System.IO.File.Exists(HttpContext.Server.MapPath(@"~/Images/" + User.Identity.GetUserId() + ".png")))
                {
                    fileName = HttpContext.Server.MapPath(@"~/Images/default-user.png");
                }
                byte[] imageData = null;
                FileInfo fileInfo = new FileInfo(fileName);
                long imageFileLength = fileInfo.Length;
                FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fs);
                imageData = br.ReadBytes((int)imageFileLength);

                return File(imageData, "image/png");

                //}
                //// to get the user details to load user Image
                //var bdUsers = HttpContext.GetOwinContext().Get<ApplicationDbContext>();
                //var userImage = bdUsers.Users.Where(x => x.Id == userId).FirstOrDefault();

                //return new FileContentResult(userImage.Photo, "image/jpeg");
            }
            else
            {
                string fileName = HttpContext.Server.MapPath(@"~/Images/user-default.png");

                byte[] imageData = null;
                FileInfo fileInfo = new FileInfo(fileName);
                long imageFileLength = fileInfo.Length;
                FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fs);
                imageData = br.ReadBytes((int)imageFileLength);
                return File(imageData, "image/png");

            }
        }


        public FileContentResult OtherUserPhotos(string userId)
        {
            if (User.Identity.IsAuthenticated)
            {
                //String userId = User.Identity.GetUserId();

                //if (userId == null)
                //{
                string fileName = HttpContext.Server.MapPath(@"~/Images/" + User.Identity.GetUserId() + ".png");
                if (!System.IO.File.Exists(HttpContext.Server.MapPath(@"~/Images/" + User.Identity.GetUserId() + ".png")))
                {
                    fileName = HttpContext.Server.MapPath(@"~/Images/default-user.png");
                }
                byte[] imageData = null;
                FileInfo fileInfo = new FileInfo(fileName);
                long imageFileLength = fileInfo.Length;
                FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fs);
                imageData = br.ReadBytes((int)imageFileLength);

                return File(imageData, "image/png");

                //}
                //// to get the user details to load user Image
                //var bdUsers = HttpContext.GetOwinContext().Get<ApplicationDbContext>();
                //var userImage = bdUsers.Users.Where(x => x.Id == userId).FirstOrDefault();

                //return new FileContentResult(userImage.Photo, "image/jpeg");
            }
            else
            {
                string fileName = HttpContext.Server.MapPath(@"~/Images/user-default.png");

                byte[] imageData = null;
                FileInfo fileInfo = new FileInfo(fileName);
                long imageFileLength = fileInfo.Length;
                FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fs);
                imageData = br.ReadBytes((int)imageFileLength);
                return File(imageData, "image/png");

            }
        }

    }
}
