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
    public class PoliklinikController : Controller
    {
        HastaneContext db = new HastaneContext();
        // GET: Poliklinik
        public ActionResult Index()
        {
            ViewBag.Title = "Hastane | Poliklinik";
            ViewBag.Description = "2021 yılında Furkan Altıntaş tarafından kodlanmıştır.";
            ViewBag.Keywords = "Hastane, Muayene, Randevu, Acil, Klinik";
            ViewBag.Author = "Furkan Altıntaş";

            var degerler = db.poliklinik.Where(x => x.durum == true).ToList();
            return View(degerler);
        }

        public ActionResult Gecmis()
        {
            ViewBag.Title = "Hastane | Poliklinik";
            ViewBag.Description = "2021 yılında Furkan Altıntaş tarafından kodlanmıştır.";
            ViewBag.Keywords = "Hastane, Muayene, Randevu, Acil, Klinik";
            ViewBag.Author = "Furkan Altıntaş";

            var degerler = db.poliklinik.Where(x => x.durum == false).ToList();
            return View(degerler);
        }

        public ActionResult TumPoliklinikler()
        {
            var poliklinikler = db.poliklinik.ToList();
            return View(poliklinikler);
        }

        public ActionResult AktifEt(int id)
        {
            var poliklinikler = db.poliklinik.Find(id);
            poliklinikler.poliklinikID = Convert.ToInt32(null);

            foreach (var item in poliklinikler.personel)
            {
                item.durum = true;
            }

            poliklinikler.durum = true;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Guncelle(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            poliklinik poliklinik = db.poliklinik.Find(id);

            if (poliklinik == null)
            {
                return HttpNotFound();
            }
            return View(poliklinik);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Guncelle(int? id, poliklinik p1)
        {
            int deger;

            PoliklinikValidatior poliklinikValidator = new PoliklinikValidatior();
            ValidationResult results = poliklinikValidator.Validate(p1);

            poliklinik pol = db.poliklinik.Find(id);

            var poliklinik = db.poliklinik.Where(x => x.ad == p1.ad).ToList();

            deger = poliklinik.Count;

            if (results.IsValid)
            {
                if (deger < 0)
                {
                    pol.ad = p1.ad;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Hata = "Böyle bir poliklinik adı bulunmaktadır";
                    return View();
                }
            }
            else
            {
                foreach (var item in results.Errors)
                {
                    ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                }
            }
            return View(poliklinik);
        }

        public ActionResult Detay(int id)
        {
            var poliklinik = db.poliklinik.Find(id);

            if (poliklinik.durum == true)
            {
                var degerler = db.personel.Where(x => x.poliklinikID == id).Where(x => x.durum == true).ToList();
                return View(degerler);
            }
            else
            {
                var degerler = db.personel.Where(x => x.poliklinikID == id).Where(x => x.durum == false).ToList();
                return View(degerler);
            }
        }

        [HttpGet]
        public ActionResult Sil(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            poliklinik poliklinik = db.poliklinik.Find(id);
            ViewBag.ad = poliklinik.ad;

            if (poliklinik == null)
            {
                return HttpNotFound();
            }
            var degerler = db.personel.Where(x => x.durum == true).Where(x => x.poliklinik.ad == poliklinik.ad).ToList();
            return View(degerler);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Sil(int id)
        {
            var poliklinik = db.poliklinik.Find(id);

            //giriş yapan kullanıcı bilgi alma
            var personel = (string)Session["tc"];
            var uye = db.personel.FirstOrDefault(x => x.TCKimlikNo == personel);

            //personel silme
            foreach (var item in poliklinik.personel)
            {
                item.durum = false;

                //yönetici silme engeli
                if (item.durum == uye.durum)
                {
                    item.durum = true;
                }
            }

            //poliklinik pasif etme
            poliklinik.durum = false;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Ekle()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken] //güvenlik
        [ValidateInput(false)]
        public ActionResult Ekle(int? id, poliklinik poliklinik)
        {
            int deger;

            PoliklinikValidatior poliklinikValidator = new PoliklinikValidatior();
            ValidationResult results = poliklinikValidator.Validate(poliklinik);

            var pol = db.poliklinik.Where(x => x.ad == poliklinik.ad).ToList();

            deger = pol.Count;

            if (results.IsValid)
            {
                if (deger <= 0)
                {
                    poliklinik.bolumID = 1;
                    poliklinik.tarih = DateTime.Now;
                    poliklinik.durum = true;
                    db.poliklinik.Add(poliklinik);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Hata = "Böyle bir poliklinik adı bulunmaktadır";
                    return View();
                }
            }
            else
            {
                foreach (var item in results.Errors)
                {
                    ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                }
            }
            return View(poliklinik);
        }
    }
}