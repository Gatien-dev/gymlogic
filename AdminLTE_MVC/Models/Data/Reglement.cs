using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Gymlogic.Models.Data
{
    public class Reglement
    {
        [Key]
        public int ID { get; set; }
        [Display(Name = "Montant Réglé")]
        public Double MontantRegle { get; set; }
        [Display(Name = "Montant Restant")]
        public Double MontantRestant { get; set; }
        [ForeignKey("Abonnement")]
        public int AbonnementID { get; set; }
        public virtual Abonnement Abonnement { get; set; }

    }
}