using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Gymlogic.Models.Data
{
    public class Forfait
    {
        [Key]
        public int ID { get; set; }
        [ForeignKey("Discipline")]
        public int DisciplineID { get; set; }
        public Discipline Discipline { get; set; }
        [Display(Name ="Durée(Jours)")]
        public int Duree { get; set; }
        [Range(0, double.MaxValue, ErrorMessage = "Entrez un montant")]
        public Double Montant { get; set; }
        public string Description { get; set; }
        public virtual List<Abonnement> Abonnements { get; set; }
    }
}