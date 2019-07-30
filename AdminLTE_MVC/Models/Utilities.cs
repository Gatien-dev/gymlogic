using IdentitySample.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Web.Configuration;

namespace AdminLTE_MVC.Models
{
    public static class Utilities
    {
        public static void emptyLogs()
        {
            var db = new ApplicationDbContext();
            var targetDate = DateTime.Now.AddMonths(-3);
            var results = db.ActivityLogs.Where(a => a.Date < targetDate);
            //db.ActivityLogs.DeleteAllOnSubmit(results);
            List<int> deleteIds = results.Select(c => c.ID).ToList();
            foreach (var id in deleteIds)
            {
                var item = db.ActivityLogs.Find(id);
                db.Entry(item).State = EntityState.Deleted;
            }
            db.SaveChanges();
        }
        public static void checkAbonnements()
        {
            var db = new ApplicationDbContext();
            var abonnements = db.Abonnements.Where(a => a.NbJoursRestants > 0 && a.Activé).Select(a => a.ID);
            int abonnementAlertDays = Int32.Parse(WebConfigurationManager.AppSettings["AbonnementAlertDays"]);
            var currentDate = DateTime.Now;
            foreach (var id in abonnements)
            {
                var abonnement = db.Abonnements.Where(a => a.NbJoursRestants > 0).Include(a => a.Client).First(a => a.ID == id);
                var lastCheckDate = abonnement.LastCheckDate;
                //abonnement.LastCheckDate = currentDate;
                if (!abonnement.Suspendu && abonnement.NbJoursRestants > 0)
                {
                    abonnement.NbJoursRestants = (int)(abonnement.DateFin - DateTime.Now).TotalDays + 1;
                    if (abonnement.NbJoursRestants < 0)
                    {
                        abonnement.NbJoursRestants = 0;
                    }
                }
                //if (!abonnement.Suspendu)
                //if (!abonnement.Suspendu)
                //{
                //    if (abonnement.LastCheckDate < abonnement.DateDebut)
                //    {
                //        abonnement.LastCheckDate = abonnement.DateDebut;
                //    }
                //    while (lastCheckDate < currentDate.AddDays(-1) && abonnement.NbJoursRestants >= 1)
                //    {
                //        lastCheckDate = lastCheckDate.AddDays(1);
                //        abonnement.NbJoursRestants -= 1;
                //    }
                //    abonnement.LastCheckDate = lastCheckDate;
                //    if (abonnement.NbJoursRestants < 1)
                //    {
                //        //abonnement.DateRenouvellement = lastCheckDate;
                //    }
                //    if (abonnement.NbJoursRestants < abonnementAlertDays && abonnement.MailSent == false)
                //    {
                //        //GMailer.GmailUsername = WebConfigurationManager.AppSettings["AppMailAddress"];
                //        //GMailer.GmailPassword = WebConfigurationManager.AppSettings["AppMailPassword"];
                //        //GMailer mailer = new GMailer();
                //        //mailer.ToEmail = abonnement.Client.Email;
                //        //mailer.Subject = "Alerte de fin d'abonnement";
                //        //mailer.Body = "Nous avons le regret de vous annoncer que votre abonnement à Finesse Gym Expire dans " + abonnement.NbJoursRestants + " Jours " + Environment.NewLine + "Cordialement,";
                //        //mailer.IsHtml = false;
                //        //mailer.Send();
                //        abonnement.MailSent = true;
                //    }
                //}
            }
            db.SaveChanges();
        }
        public static void SendMail(string recipient, string subject, string body, string attachmentFilename = null)
        {
            try
            {
                SmtpClient smtpClient = new SmtpClient();
                NetworkCredential basicCredential = new NetworkCredential(WebConfigurationManager.AppSettings["AppMailAddress"], WebConfigurationManager.AppSettings["AppMailPassword"]);
                MailMessage message = new MailMessage();
                MailAddress fromAddress = new MailAddress(WebConfigurationManager.AppSettings["AppMailAddress"]);

                // setup up the host, increase the timeout to 5 minutes
                smtpClient.Host = WebConfigurationManager.AppSettings["AppMailSmtpServer"];
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = basicCredential;
                smtpClient.Timeout = (60 * 5 * 1000);
                smtpClient.EnableSsl = true;
                message.From = fromAddress;
                message.Subject = subject;
                message.IsBodyHtml = false;
                message.Body = body;
                message.To.Add(recipient);

                if (attachmentFilename != null)
                {
                    Attachment attachment = new Attachment(attachmentFilename, MediaTypeNames.Application.Octet);
                    ContentDisposition disposition = attachment.ContentDisposition;
                    disposition.CreationDate = File.GetCreationTime(attachmentFilename);
                    disposition.ModificationDate = File.GetLastWriteTime(attachmentFilename);
                    disposition.ReadDate = File.GetLastAccessTime(attachmentFilename);
                    disposition.FileName = Path.GetFileName(attachmentFilename);
                    disposition.Size = new FileInfo(attachmentFilename).Length;
                    disposition.DispositionType = DispositionTypeNames.Attachment;
                    message.Attachments.Add(attachment);
                }

                smtpClient.Send(message);
            }
            catch (Exception ex)
            {
                //throw (new Exception("Failed to send mail to " + recipient + " about " + subject + " " + body + " " + attachmentFilename + Environment.NewLine + ex.Message + ex.InnerException?.Message));



            }
        }
    }
}