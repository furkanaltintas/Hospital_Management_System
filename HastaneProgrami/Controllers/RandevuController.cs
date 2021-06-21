using FluentValidation.Results;
using HastaneProgrami.Models;
using HastaneProgrami.Models.Entity_Framework;
using HastaneProgrami.ValidationRules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace HastaneProgrami.Controllers
{
    public class RandevuController : Controller
    {
        HastaneContext db = new HastaneContext();
        // GET: Randevu
        [Authorize(Roles = "Yönetici,Doktor,Sekreter")]
        public ActionResult Index()
        {
            var personel = (string)Session["tc"];
            var uye = db.personel.FirstOrDefault(x => x.TCKimlikNo == personel);

            ViewBag.Title = "Hastane | Randevu";
            ViewBag.Description = "2021 yılında Furkan Altıntaş tarafından kodlanmıştır.";
            ViewBag.Keywords = "Hastane, Muayene, Randevu, Acil, Klinik";
            ViewBag.Author = "Furkan Altıntaş";

            if (User.IsInRole("Yönetici"))
            {
                var yonetıcı = db.randevu.Where(x => x.durum == true).ToList();
                return View(yonetıcı);
            }
            if (User.IsInRole("Sekreter"))
            {
                var sekreter = db.randevu.Where(x => x.durum == true).Where(x => x.poliklinikID == uye.poliklinikID).ToList();
                return View(sekreter);
            }
            if (User.IsInRole("Doktor"))
            {
                var doktor = db.randevu.Where(x => x.durum == true).Where(x => x.personelID == uye.personelID).ToList();
                return View(doktor);
            }
            return View();

        }

        [Authorize(Roles = "Yönetici,Doktor,Sekreter")]
        public ActionResult Gecmis()
        {
            var personel = (string)Session["tc"];
            var uye = db.personel.FirstOrDefault(x => x.TCKimlikNo == personel);

            if (User.IsInRole("Yönetici"))
            {
                var yonetıcı = db.randevu.Where(x => x.durum == false).ToList();
                return View(yonetıcı);
            }
            if (User.IsInRole("Sekreter"))
            {
                var sekreter = db.randevu.Where(x => x.durum == false).Where(x => x.poliklinikID == uye.poliklinikID).ToList();
                return View(sekreter);
            }
            if (User.IsInRole("Doktor"))
            {
                var doktor = db.randevu.Where(x => x.durum == false).Where(x => x.personelID == uye.personelID).ToList();
                return View(doktor);
            }
            return View();
        }

        [Authorize(Roles = "Yönetici,Doktor,Sekreter")]
        public ActionResult Detay(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            hasta hasta = db.hasta.Find(id);
            var degerler = db.hastaBilgi.Where(x => x.hastaID == hasta.hastaID).ToList();
            ViewBag.hasta = hasta.adi + " " + hasta.soyadi;
            return View(degerler);

            //if (id == null)
            //{
            //    return RedirectToAction("Error", "NotFound");
            //}
            //randevu randevu = db.randevu.Find(id);
            //if (randevu == null)
            //{
            //    return RedirectToAction("Error", "NotFound");
            //}
            //return View(randevu);
        }

        [Authorize(Roles = "Yönetici,Doktor,Sekreter")]
        public ActionResult Bilgi(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Error", "NotFound");
            }
            randevu randevu = db.randevu.Find(id);
            if (randevu == null)
            {
                return RedirectToAction("Error", "NotFound");
            }
            return View(randevu);
        }

        [Authorize(Roles = "Yönetici,Doktor")]
        [HttpGet]
        public ActionResult Ekle(int? id)
        {
            ViewBag.yetkiID = new SelectList(db.yetki, "yetkiID", "yetkiAd");

            if (id == null)
            {
                return RedirectToAction("Error", "NotFound");
            }
            randevu randevu = db.randevu.Find(id);
            if (randevu == null)
            {
                return RedirectToAction("Error", "NotFound");
            }
            return View();
        }

        [Authorize(Roles = "Yönetici,Doktor")]
        [HttpPost]
        [ValidateAntiForgeryToken] //güvenlik
        [ValidateInput(false)]
        public ActionResult Ekle(muayene muayene, int? id)
        {
            MuayeneValidatior muayeneValidator = new MuayeneValidatior();
            ValidationResult results = muayeneValidator.Validate(muayene);

            if (results.IsValid)
            {
                var personel = (string)Session["tc"];
                var uye = db.personel.FirstOrDefault(x => x.TCKimlikNo == personel);
                randevu randevu = db.randevu.Find(id);
                muayene.hastaID = randevu.hastaID;
                muayene.personelID = uye.personelID;
                muayene.randevuID = randevu.randevuID;
                randevu.randevuSaat.dolumBaslangic = randevu.randevuSaat.dolumBaslangic - 1;
                randevu.durum = false;
                muayene.tarih = DateTime.Now;
                db.muayene.Add(muayene);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                foreach (var item in results.Errors)
                {
                    ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                }
            }
            return View(muayene);
        }
    }
}