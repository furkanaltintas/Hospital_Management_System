using FluentValidation.Results;
using HastaneProgrami.Models.Entity_Framework;
using HastaneProgrami.ValidationRules;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace HastaneProgrami.Controllers
{
    [Authorize(Roles = "Yönetici")]
    public class AmeliyatOdaController : Controller
    {
        HastaneContext db = new HastaneContext();
        // GET: AmeliyatDurum
        public ActionResult Index()
        {
            var ameliyat = db.ameliyatOdasi.Where(x => x.durum == true).ToList();
            return View(ameliyat);
        }

        public ActionResult Gecmis()
        {
            ViewBag.Title = "Hastane | Poliklinik";
            ViewBag.Description = "2021 yılında Furkan Altıntaş tarafından kodlanmıştır.";
            ViewBag.Keywords = "Hastane, Muayene, Randevu, Acil, Klinik";
            ViewBag.Author = "Furkan Altıntaş";

            var ameliyat = db.ameliyatOdasi.Where(x => x.durum == false).ToList();
            return View(ameliyat);
        }

        public ActionResult TumAmeliyatOdalari()
        {
            var ameliyat = db.ameliyatOdasi.ToList();
            return View(ameliyat);
        }

        public ActionResult AktifEt(int id)
        {
            var ameliyat = db.ameliyatOdasi.Find(id);
            ameliyat.durum = true;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Detaylar(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ameliyatOdasi ameliyatOdasi = db.ameliyatOdasi.Find(id);

            var ameliyat = db.ameliyat.Where(x => x.ameliyatOdasiID == ameliyatOdasi.ameliyatOdasiID).ToList();


            if (ameliyatOdasi == null)
            {
                return HttpNotFound();
            }

            return View(ameliyat);
        }

        [HttpGet]
        public ActionResult Guncelle(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ameliyatOdasi ameliyatOdasi = db.ameliyatOdasi.Find(id);
            if (ameliyatOdasi == null)
            {
                return HttpNotFound();
            }

            if (ameliyatOdasi.odaDurum == false)
            {
                return RedirectToAction("Index");
            }

            return View(ameliyatOdasi);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Guncelle(int id, ameliyatOdasi ameliyatOdasi)
        {
            AmeliyatOdasiValidatior ameliyatOdasiValidation = new AmeliyatOdasiValidatior();
            ValidationResult results = ameliyatOdasiValidation.Validate(ameliyatOdasi);

            ameliyatOdasi ameliyat = db.ameliyatOdasi.Find(id);

            if (results.IsValid)
            {
                ameliyat.ad = ameliyatOdasi.ad;
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
            return View(ameliyatOdasi);
        }

        [HttpGet]
        public ActionResult Ekle()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken] //güvenlik
        [ValidateInput(false)]
        public ActionResult Ekle(ameliyatOdasi p1)
        {
            AmeliyatOdasiValidatior ameliyatOdasiValidation = new AmeliyatOdasiValidatior();
            ValidationResult results = ameliyatOdasiValidation.Validate(p1);

            var ameliyatodasi = db.ameliyatOdasi.Where(x => x.durum == true).Where(x => x.ad == p1.ad).FirstOrDefault();

            if (results.IsValid)
            {
                if (ameliyatodasi.ad == p1.ad)
                {
                    ViewBag.Hata = "Aynı isim de ameliyat odası bulunmaktadır";
                    return View(p1);
                }
                else
                {
                    p1.tarih = DateTime.Now;
                    p1.odaDurum = true;
                    p1.durum = true;
                    db.ameliyatOdasi.Add(p1);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            else
            {
                foreach (var item in results.Errors)
                {
                    ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                }
            }
            return View(p1);
        }

        [HttpGet]
        public ActionResult Sil(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ameliyatOdasi ameliyatOdasi = db.ameliyatOdasi.Find(id);
            ViewBag.ad = ameliyatOdasi.ad;

            if (ameliyatOdasi == null)
            {
                return HttpNotFound();
            }

            if (ameliyatOdasi.odaDurum == false)
            {
                return RedirectToAction("Index");
            }

            return View(ameliyatOdasi);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Sil(int id)
        {
            var ameliyatOdasi = db.ameliyatOdasi.Find(id);

            if (ameliyatOdasi.odaDurum == false)
            {
                ViewBag.Hata = "BU AMELİYAT ODASI DOLU OLDUĞU İÇİN SİLEMEZSİNİZ!!!";
                return View(ameliyatOdasi);
            }

            //ameliyat kadro silme
            //foreach (var item in ameliyatOdasi.ameliyatEkip.ameliyatKadro)
            //{
            //    item.durum = false;

            //    //yönetici silme engeli
            //    if (item.durum == uye.durum)
            //    {
            //        item.durum = true;
            //    }
            //}

            //ameliyat odası pasif etme
            ameliyatOdasi.durum = false;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}