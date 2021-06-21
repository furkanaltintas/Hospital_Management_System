using HastaneProgrami.Models.Entity_Framework;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace HastaneProgrami.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        HastaneContext db = new HastaneContext();
        [Authorize(Roles = "Yönetici,Uzman Doktor,Doktor,Danışman,Sekreter")]
        public ActionResult Index()
        {
            ViewBag.Title = "Hastane | Admin";
            ViewBag.Description = "2021 yılında Furkan Altıntaş tarafından kodlanmıştır.";
            ViewBag.Keywords = "Hastane, Muayene, Randevu, Acil, Klinik";
            ViewBag.Author = "Furkan Altıntaş";

            var dgr = db.hasta.Count();
            ViewBag.d1 = dgr;

            var dgr2 = db.personel.Where(x => x.durum == true).Count();
            ViewBag.d2 = dgr2;

            var dgr3 = db.poliklinik.Where(x => x.durum == true).Count();
            ViewBag.d3 = dgr3;

            var dgr4 = db.muayene.Count();
            ViewBag.d4 = dgr4;

            var dgr5 = db.personel.Where(x => x.yetkiID == 1).Count();
            ViewBag.d5 = dgr5;

            var dgr6 = db.personel.Where(x => x.yetkiID == 2).Count();
            ViewBag.d6 = dgr6;

            var dgr7 = db.personel.Where(x => x.yetkiID == 3).Count();
            ViewBag.d7 = dgr7;

            var dgr8 = db.personel.Where(x => x.yetkiID == 4).Count();
            ViewBag.d8 = dgr8;

            var dgr9 = db.muayene.Count();
            ViewBag.d9 = dgr9;

            var dgr10 = db.hasta.Where(x => x.cinsiyet == true).Count();
            ViewBag.d10 = dgr10;

            var dgr11 = db.hasta.Where(x => x.cinsiyet == false).Count();
            ViewBag.d11 = dgr11;

            var dgr12 = db.randevu.Where(x => x.durum == true).Count();
            ViewBag.d12 = dgr12;

            return View();
        }
    }
}