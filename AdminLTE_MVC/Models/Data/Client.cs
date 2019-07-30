using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Gymlogic.Models.Data
{
    public class Client
    {
        [Key]
        public int ID { get; set; }
        [Required(ErrorMessage = "Veuillez renseigner le nom du client.")]
        public string Nom { get; set; }
        [Display(Name = "Prénom")]
        [Required(ErrorMessage = "Veuillez renseigner le prénom du client.")]
        public string Prenom { get; set; }
        [Display(Name = "Téléphone")]
        [Phone]
        public string Telephone { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [Display(Name = "Adresse")]

        public string Adresse { get; set; }
        public Sexe Sexe { get; set; }
        [InverseProperty("Client")]
        public List<Abonnement> Abonnements { get; set; }
        [InverseProperty("ClientCouple")]
        public List<Abonnement> AbonnementsCouples { get; set; }
        public bool Nouveau { get; set; }
        [Display(Name = "Envoyer des mails")]
        public bool Mails { get; set; }
    }
    public enum Sexe { Homme, Femme, Autre }
}