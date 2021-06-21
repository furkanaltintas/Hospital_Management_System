using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;
using HastaneProgrami.Models.Entity_Framework;
using HastaneProgrami.ValidationRules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using HastaneProgrami.ViewModel;
using System.Runtime.Remoting.Messaging;

namespace HastaneProgrami.Controllers
{
    [Authorize(Roles = "Hasta")]
    public class VatandasController : Controller
    {
        HastaneContext db = new HastaneContext();
        // GET: Vatandas
        public ActionResult Index()
        {
            var hasta = (string)Session["tc"];
            var uye = db.hasta.FirstOrDefault(x => x.TCKimlikNo == hasta);

            ViewBag.d1 = uye.adi + " " + uye.soyadi;
            ViewBag.d2 = uye.TCKimlikNo;
            ViewBag.d3 = uye.dogumTarihi.ToString("dd-MM-yyyy");
            ViewBag.d4 = uye.kanGrubu.kanGrubuAdi;


            ViewBag.Title = "Hastane | Randevu";
            ViewBag.Description = "2021 yılında Furkan Altıntaş tarafından kodlanmıştır.";
            ViewBag.Keywords = "Hastane, Muayene, Randevu, Acil, Klinik";
            ViewBag.Author = "Furkan Altıntaş";

            var r = db.randevu.Where(x => x.durum == true).Where(x => x.hastaID == uye.hastaID).ToList();
            ViewBag.aktif = r;

            var r2 = db.randevu.Where(x => x.hastaID == uye.hastaID && x.durum == false).ToList();
            ViewBag.pasif = r2;

            return View(r);
        }

        [HttpGet]
        [Authorize]
        public ActionResult KimlikBilgileri()
        {
            var hasta = (string)Session["tc"];
            var uye = db.hasta.FirstOrDefault(x => x.TCKimlikNo == hasta);
            return View();
        }

        [HttpPost]
        public ActionResult KimlikBilgileri(hasta h)
        {
            //if (ModelState.IsValid)
            //{
            //    var personel = (string)Session["adi"];
            //    var uye = db.personel.FirstOrDefault(x => x.adi == personel);
            //    uye.adi = p.adi;
            //    uye.soyadi = p.soyadi;
            //    uye.email = p.TCKimlikNo;
            //    uye.cinsiyet = p.cinsiyet;
            //    uye.dogumTarihi = p.dogumTarihi;
            //    db.SaveChanges();
            //    ViewBag.Basarili = "Güncelleme Başarıyla Gerçekleşmiştir";
            //    return RedirectToAction("Bilgiler");
            //}
            //return View(p);

            //KİMLİK BİLGİLERİ DEĞİŞTİRİLEMEZ !!!
            return View();
        }

        [HttpGet]
        [Authorize]
        public ActionResult IletisimBilgileri()
        {
            var hasta = (string)Session["tc"];
            var uye = db.hasta.FirstOrDefault(x => x.TCKimlikNo == hasta);
            return View();
        }

        [HttpPost]
        public ActionResult IletisimBilgileri(hasta h)
        {
            HastaValidatior hastaValidator = new HastaValidatior();
            ValidationResult results = hastaValidator.Validate(h);

            var hasta = (string)Session["tc"];
            var uye = db.hasta.FirstOrDefault(x => x.TCKimlikNo == hasta);

            if (h.cepTel != null && h.evTel != null && h.email != null)
            {
                uye.cepTel = h.cepTel;
                uye.evTel = h.evTel;
                uye.email = h.email;
                db.SaveChanges();
                ViewBag.Basarili = "Güncelleme Başarıyla Gerçekleşmiştir";
                return RedirectToAction("IletisimBilgileri");
            }
            else
            {
                ViewBag.Basarisiz = "İletişim Bilgileri Boş Geçilemez";
            }
            return View(h);
        }

        [HttpGet]
        [Authorize]
        public ActionResult ParolaIslemleri()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ParolaIslemleri(hasta h)
        {
            var hasta = (string)Session["adi"];
            var uye = db.hasta.FirstOrDefault(x => x.adi == hasta);

            if (uye.email != h.email && uye.sifre != h.sifre)
            {
                ViewBag.Basarisiz = "Email ve şifre hatalı";
            }
            else if (uye.sifre == h.rsifre)
            {
                ViewBag.Basarisiz = "Yeni şifreniz eski şifre ile aynı olamaz";
            }
            else
            {
                uye.email = h.email;
                uye.sifre = h.rsifre;
                uye.rsifre = uye.sifre;
                db.SaveChanges();
                ViewBag.Basarili = "Güncelleme Başarıyla Gerçekleşmiştir";
                return RedirectToAction("ParolaIslemleri");
            }
            return View(h);
        }

        [HttpGet]
        public ActionResult RandevuAl()
        {
            List<poliklinik> PoliklinikList = db.poliklinik.Where(x => x.durum == true).ToList();
            ViewBag.PoliklinikList = new SelectList(PoliklinikList, "PoliklinikID", "ad");
            return View();
        }

        [HttpPost]
        public ActionResult RandevuAl(poliklinik pol)
        {
            if (ModelState.IsValid)
            {

            }
            return View(pol);
        }

        public JsonResult PersonelGetir (int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            List<personel> PersonelList = db.personel.Where(x => x.personelID == id && x.durum == true).ToList();
            return Json(PersonelList, JsonRequestBehavior.AllowGet);
        }
    }
}