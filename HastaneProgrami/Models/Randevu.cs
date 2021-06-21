using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HastaneProgrami.Models
{
    public class Randevu
    {
        public int poliklinikID { get; set; }
        public int personelID { get; set; }
        public int randevuSaatID { get; set; }
    }
}