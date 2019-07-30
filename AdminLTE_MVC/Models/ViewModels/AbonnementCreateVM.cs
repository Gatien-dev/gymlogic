using Gymlogic.Models.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Gymlogic.Models.ViewModels
{
    //ID,Nom,Prenom,Telephone,Adresse,Sexe,Email,Mails
    public class AbonnementCreateVM
    {
        //[Required(ErrorMessage = "Veuillez renseigner le nom du client.")]
        public string Nom { get; set; } = "";
        [Display(Name = "Prénom")]
        //[Required(ErrorMessage = "Veuillez renseigner le prénom du client.")]
        public string Prenom { get; set; } = "";
        [Display(Name = "Téléphone")]
        [Phone]
        public string Telephone { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [Display(Name = "Adresse")]
        public string Adresse { get; set; }
        [Display(Name = "Envoyer des mails")]
        public bool Mails { get; set; }
        public Sexe Sexe { get; set; }
        public int? ClientID { get; set; }
        public int ForfaitId { get; set; }
        [DataType(DataType.Date)]
        [Display(Name ="Date de Début")]
        public DateTime DateDebut { get; set; }
        [Display(Name = "Date de Reçu")]
        [DataType(DataType.Date)]
        public DateTime DateRecu { get; set; }
        [Display(Name = "Nouveau client")]
        public bool NewCLientCheck { get; set; } = true;
        public bool Activé { get; set; } = true;
        [Display(Name = "Montant Payé")]
        public double paye { get; set; }
       

    }
}