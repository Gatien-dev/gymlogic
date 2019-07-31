using IdentitySample.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Gymlogic.Models.Data
{
    public class Abonnement
    {
        [Key]
        public int ID { get; set; }
        public string UserID { get; set; }
        //[ForeignKey("Discipline")]
        //public int DisciplineID { get; set; }
        //public virtual Discipline Discipline { get; set; }
        [ForeignKey("Client")]
        public int ClientID { get; set; }
        public virtual Client Client { get; set; }
        [ForeignKey("ClientCouple")]
        public int? ClientCoupleId { get; set; }
        public virtual Client ClientCouple { get; set; }
        [Display(Name = "Reste A payer")]
        public double ResteAPayer { get; set; }
        [Display(Name = "Montant")]
        public double Montant { get; set; }
        [Display(Name = "Date de reçu")]
        [DataType(DataType.Date)]
        ///Date recu
        public DateTime DateCreation { get; set; }
        public DateTime CreationDate { get; set; }
        [Display(Name = "Date de Début")]
        [DataType(DataType.Date)]
        //Date debut
        public DateTime DateDebut { get; set; }
        [Display(Name = "Durée")]
        public int NbJours { get; set; }
        [Display(Name = "Jours Restants")]
        public int NbJoursRestants { get; set; }
        [Display(Name = "Suspendu?")]
        public bool Suspendu { get; set; }
        [Display(Name = "Validé?")]
        public bool Approved { get; set; }
        [Display(Name = "Renouvellement?")]
        public bool Renewal { get; set; }
        //Utilisé pour stocker la date de fin à la fin de l'abonnement
        [DataType(DataType.Date)]
        [Display(Name = "Date de Decompte")]
        public DateTime DateDecompte { get; set; }
        public string ApproverUID { get; set; }
        [ForeignKey("Forfait")]
        public int ForfaitId { get; set; }
        public Forfait Forfait { get; set; }
        public bool MailSent { get; set; }
        public DateTime LastCheckDate { get; set; }
        [Display(Name ="Date de fin")]
        public DateTime DateFin { get; set; }
        public bool Activé { get; set; }
        public List<History> History { get; set; }

    }
    public class History
    {
        public int ID { get; set; }
        public string Action { get; set; }
        public DateTime Date { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public Abonnement Abonnement { get; set; }
        [ForeignKey("Abonnement")]
        public int AbonnementID { get; set; }
    }
}