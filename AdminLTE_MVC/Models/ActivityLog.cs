using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdminLTE_MVC.Models
{
    public class ActivityLog
    {
        public int ID { get; set; }
        public string UserId { get; set; }
        public string UserEmail { get; set; }
        public string LogMessage { get; set; }
        public string Motif { get; set; }
        public ActivityType ActivityType { get; set; }
        public DateTime Date { get; set; }
    }
    public enum ActivityType
    {
        Display, Create, Edit, Delete
    }
}