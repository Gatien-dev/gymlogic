using IdentitySample.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Gymlogic.Models.Data
{
    public class Notification
    {
        public int ID { get; set; }
        public string Titre { get; set; }
        public string Contenu { get; set; }
        public DateTime Date { get; set; }
        public bool Archived { get; set; }
        public string Tag { get; set; }
        [ForeignKey("User")]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}