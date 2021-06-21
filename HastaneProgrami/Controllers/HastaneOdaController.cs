using FluentValidation.Results;
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
    public class HastaneOdaController : Controller
    {
        HastaneContext db = new HastaneContext();
        // GET: HastaneOda
        public ActionResult Index()
        {
            var degerler = db.hastaneOda.ToList();
            return View(degerler);
        }

        public ActionResult Detaylar(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            hastaneOda hastaneOda = db.hastaneOda.Find(id);

            var oda = db.ameliyatSonuc.Where(x => x.hastaneOdaID == hastaneOda.hastaneOdaID).ToList();

            if (hastaneOda == null)
            {
                return HttpNotFound();
            }
            return View(oda);
        }

        [HttpGet]
        public ActionResult Ekle()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken] //güvenlik
        [ValidateInput(false)]
        public ActionResult Ekle(hastaneOda p1)
        {
            HastaneOdaValidatior hastaneOdaValidation = new HastaneOdaValidatior();
            ValidationResult results = hastaneOdaValidation.Validate(p1);

            var hastaneodasi = db.hastaneOda.Where(x => x.durum == true).Where(x => x.ad == p1.ad).FirstOrDefault();

            if (results.IsValid)
            {
                if (hastaneodasi != null && hastaneodasi.ad == p1.ad)
                {
                    ViewBag.Hata = "Aynı isim de hastane odası bulunmaktadır";
                    return View(p1);
                }
                else
                {
                    p1.bitis = 0;
                    p1.durum = true;
                    db.hastaneOda.Add(p1);
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

            hastaneOda hastaneOda = db.hastaneOda.Find(id);

            var oda = db.ameliyatSonuc.Where(x => x.hastaneOdaID == hastaneOda.hastaneOdaID).ToList();

            if (hastaneOda == null)
            {
                return HttpNotFound();
            }
            return View(oda);
        }

        [HttpPost]
        [ValidateAntiForgeryToken] //güvenlik
        [ValidateInput(false)]
        public ActionResult Sil(int id)
        {
            var oda = db.hastaneOda.Find(id);

            if (oda.baslangic == oda.bitis)
            {
                oda.bitis = oda.bitis - 1;
                oda.durum = true;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                oda.bitis = oda.bitis - 1;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
        }
    }
}