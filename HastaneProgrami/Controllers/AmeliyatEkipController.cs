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
    public class AmeliyatEkipController : Controller
    {
        HastaneContext db = new HastaneContext();
        // GET: AmeliyatEkip
        public ActionResult Index()
        {
            var ekip = db.ameliyatEkip.Where(x => x.durum == true).ToList();
            return View(ekip);
        }

        public ActionResult Kadro(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Error", "NotFound");
            }
            ameliyatEkip ameliyatEkip = db.ameliyatEkip.Find(id);
            var degerler = db.ameliyatKadro.Where(x => x.durum == true).Where(x => x.ameliyatEkipID == ameliyatEkip.ameliyatEkipID).ToList();
            ViewBag.ekip = ameliyatEkip.ad.ToUpper();
            return View(degerler);
        }

        [HttpGet]
        public ActionResult Guncelle(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ameliyatEkip ameliyatEkip = db.ameliyatEkip.Find(id);

            if (ameliyatEkip == null)
            {
                return HttpNotFound();
            }

            if (ameliyatEkip.ekipDurum == false)
            {
                return RedirectToAction("Index");
            }

            return View(ameliyatEkip);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Guncelle(int id, ameliyatEkip ameliyatEkip)
        {
            AmeliyatEkipValidatior ameliyatEkipValidation = new AmeliyatEkipValidatior();
            ValidationResult results = ameliyatEkipValidation.Validate(ameliyatEkip);

            ameliyatEkip ameliyat = db.ameliyatEkip.Find(id);

            if (results.IsValid)
            {
                ameliyat.ad = ameliyatEkip.ad;
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
            return View(ameliyatEkip);
        }

        [HttpGet]
        public ActionResult Ekle()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken] //güvenlik
        [ValidateInput(false)]
        public ActionResult Ekle(ameliyatEkip p1)
        {
            AmeliyatEkipValidatior ameliyatEkipValidation = new AmeliyatEkipValidatior();
            ValidationResult results = ameliyatEkipValidation.Validate(p1);

            var ameliyatekip = db.ameliyatEkip.Where(x => x.durum == true).Where(x => x.ad == p1.ad).FirstOrDefault();

            if (results.IsValid)
            {
                if (ameliyatekip.ad == p1.ad)
                {
                    ViewBag.Hata = "Aynı isim de ekip bulunmaktadır";
                    return View(p1);
                }
                else
                {
                    p1.tarih = DateTime.Now;
                    p1.ekipDurum = true;
                    p1.durum = true;
                    db.ameliyatEkip.Add(p1);
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
        public ActionResult PersonelEkle(int? id)
        {
            ViewBag.personel = new SelectList(db.personel.Where(x => x.bolumID == 2), "personelID", "TCKimlikNo");

            if (id == null)
            {
                return RedirectToAction("Error", "NotFound");
            }

            ameliyatEkip ameliyatEkip = db.ameliyatEkip.Find(id);

            if (ameliyatEkip == null)
            {
                return RedirectToAction("Error", "NotFound");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult PersonelEkle(ameliyatKadro ameliyatKadro, int? id)
        {
            if (ModelState.IsValid)
            {
                ViewBag.personel = new SelectList(db.personel.Where(x => x.bolumID == 2), "personelID", "TCKimlikNo");

                ameliyatKadro.ameliyatEkipID = id;
                ameliyatKadro.durum = true;
                ameliyatKadro.tarih = DateTime.Now;
                db.ameliyatKadro.Add(ameliyatKadro);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(ameliyatKadro);
        }

        [HttpGet]
        public ActionResult Sil(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ameliyatEkip ameliyatEkip = db.ameliyatEkip.Find(id);
            ViewBag.ad = ameliyatEkip.ad;

            if (ameliyatEkip == null)
            {
                return HttpNotFound();
            }

            if (ameliyatEkip.ekipDurum == false)
            {
                return RedirectToAction("Index");
            }

            return View(ameliyatEkip);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Sil(int id)
        {
            var ameliyatEkip = db.ameliyatEkip.Find(id);

            if (ameliyatEkip.ekipDurum == false)
            {
                ViewBag.Hata = "AMELİYAT EKİBİ, AMELİYATTA OLDUĞU İÇİN SİLEMEZSİNİZ!!!";
                return View(ameliyatEkip);
            }

            //ameliyat kadro silme
            foreach (var item in ameliyatEkip.ameliyatKadro)
            {
                item.durum = false;
            }

            //ameliyat ekip pasif etme
            ameliyatEkip.durum = false;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}