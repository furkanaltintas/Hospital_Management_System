using FluentValidation.Results;
using HastaneProgrami.Models.Entity_Framework;
using HastaneProgrami.ValidationRules;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Mvc;

namespace HastaneProgrami.Controllers
{
    [Authorize(Roles = "Yönetici")]
    public class PersonelController : Controller
    {
        private HastaneContext db = new HastaneContext();
        // GET: Profil
        public ActionResult Index()
        {
            ViewBag.Title = "Hastane | Personel";
            ViewBag.Description = "2021 yılında Furkan Altıntaş tarafından kodlanmıştır.";
            ViewBag.Keywords = "Hastane, Muayene, Randevu, Acil, Klinik";
            ViewBag.Author = "Furkan Altıntaş";

            //var kullanici = db.kullanici.Include(k => k.personel).Include(k => k.yetki);
            //return View(kullanici.ToList());

            var degerler = db.personel.Where(x => x.durum == true).ToList();
            return View(degerler);
        }

        public ActionResult Gecmis()
        {
            ViewBag.Title = "Hastane | Personel";
            ViewBag.Description = "2021 yılında Furkan Altıntaş tarafından kodlanmıştır.";
            ViewBag.Keywords = "Hastane, Muayene, Randevu, Acil, Klinik";
            ViewBag.Author = "Furkan Altıntaş";

            var degerler = db.personel.Where(x => x.durum == false).ToList();
            return View(degerler);
        }

        public ActionResult TumPersoneller()
        {
            var personeller = db.personel.ToList();
            return View(personeller);
        }

        public ActionResult AktifEt(int id)
        {
            var personel = db.personel.Find(id);
            personel.durum = true;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Guncelle(int? id)
        {
            ViewBag.poliklinik = new SelectList(db.poliklinik.Where(x => x.durum == true), "poliklinikID", "ad");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            personel personel = db.personel.Find(id);

            if (personel == null)
            {
                return HttpNotFound();
            }
            return View(personel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Guncelle(int? id, personel personel)
        {

            PersonelValidatior personelValidator = new PersonelValidatior();
            ValidationResult results = personelValidator.Validate(personel);

            personel per = db.personel.Find(id);

            if (results.IsValid)
            {
                ViewBag.poliklinik = new SelectList(db.poliklinik.Where(x => x.durum == true), "poliklinikID", "ad");
                per.adi = personel.adi;
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
            return View(personel);
        }

        public ActionResult Detay(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            personel personel = db.personel.Find(id);
            if (personel == null)
            {
                return HttpNotFound();
            }
            return View(personel);
        }

        [HttpGet]
        public ActionResult Ekle()
        {
            ViewBag.personelID = new SelectList(db.personel, "personelID", "adi");
            ViewBag.bolumID = new SelectList(db.bolum, "bolumID", "ad");
            ViewBag.yetkiID = new SelectList(db.yetki, "yetkiID", "yetkiAd");
            ViewBag.poliklinikID = new SelectList(db.poliklinik, "poliklinikID", "ad");
            ViewBag.kanGrubuID = new SelectList(db.kanGrubu, "kanGrubuID", "kanGrubuAdi");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken] //güvenlik
        [ValidateInput(false)]
        public ActionResult Ekle(personel personel)
        {
            ViewBag.personelID = new SelectList(db.personel, "personelID", "adi", personel.personelID);
            ViewBag.bolumID = new SelectList(db.bolum, "bolumID", "ad", personel.bolumID);
            ViewBag.yetkiID = new SelectList(db.yetki, "yetkiID", "yetkiAd", personel.yetkiID);
            ViewBag.kanGrubuID = new SelectList(db.kanGrubu, "kanGrubuID", "kanGrubuAdi", personel.kanGrubuID);
            ViewBag.poliklinikID = new SelectList(db.poliklinik, "poliklinikID", "ad", personel.poliklinikID);

            PersonelValidatior personelValidator = new PersonelValidatior();
            ValidationResult results = personelValidator.Validate(personel);

            if (results.IsValid)
            {
                personel.durum = true;
                personel.kayitTarihi = DateTime.Now;
                db.personel.Add(personel);
                db.SaveChanges();

            }
            else
            {
                foreach (var item in results.Errors)
                {
                    ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                }
            }
            return View(personel);
        }

        [HttpGet]
        public ActionResult Cikar(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            personel personel = db.personel.Find(id);

            if (personel == null)
            {
                return HttpNotFound();
            }
            return View(personel);

            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            //kullanici kullanici = db.kullanici.Find(id);
            //if (kullanici == null)
            //{
            //    return HttpNotFound();
            //}
            //return View(kullanici);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cikar(int id)
        {
            var personel = db.personel.Find(id);
            personel.durum = false;
            db.SaveChanges();
            return RedirectToAction("Index");

            //kullanici kullanici = db.kullanici.Find(id);
            //db.kullanici.Remove(kullanici);
            //db.SaveChanges();
            //return RedirectToAction("Index");
        }
    }
}