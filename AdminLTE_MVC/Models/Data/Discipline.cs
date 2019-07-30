using IdentitySample.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gymlogic.Models.Data
{
    public class Discipline
    {
        [Key]
        public int ID { get; set; }
        [Display(Name = "Nom de la discipline")]
        public string Name { get; set; }
        [ForeignKey("Titulaire")]
        [Display(Name = "Titulaire")]
        public string TitulaireID { get; set; }
        public List<Abonnement> Abonnements { get; set; }
        public List<Forfait> Forfaits { get; set; }
        public virtual ApplicationUser Titulaire { get; set; }

    }
}