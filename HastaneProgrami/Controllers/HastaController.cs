using FluentValidation.Results;
using HastaneProgrami.Models;
using HastaneProgrami.Models.Entity_Framework;
using HastaneProgrami.ValidationRules;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using System.Web.WebPages.Html;
using SelectListItem = System.Web.Mvc.SelectListItem;

namespace HastaneProgrami.Controllers
{
    [Authorize(Roles = "Yönetici,Uzman Doktor,Doktor,Sekreter,Danışman")]
    public class HastaController : Controller
    {
        // GET: Hasta
        HastaneContext db = new HastaneContext();
        public ActionResult Index()
        {
            ViewBag.Title = "Hastane | Hasta";
            ViewBag.Description = "2021 yılında Furkan Altıntaş tarafından kodlanmıştır.";
            ViewBag.Keywords = "Hastane, Muayene, Randevu, Acil, Klinik";
            ViewBag.Author = "Furkan Altıntaş";

            var degerler = db.hasta.ToList();
            return View(degerler);
        }

        public ActionResult Guncelle(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            hasta hasta = db.hasta.Find(id);

            if (hasta == null)
            {
                return HttpNotFound();
            }
            ViewBag.kanGrubuID = new SelectList(db.kanGrubu, "kanGrubuID", "kanGrubuAdi", hasta.kanGrubuID);
            return View(hasta);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Guncelle(hasta hasta)
        {
            HastaValidatior hastavalidation = new HastaValidatior();
            ValidationResult results = hastavalidation.Validate(hasta);

            if (results.IsValid)
            {
                db.Entry(hasta).State = EntityState.Modified;
                hasta.vatandasYetkiID = 1;
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
            ViewBag.kanGrubuID = new SelectList(db.kanGrubu, "kanGrubuID", "kanGrubuAdi", hasta.kanGrubuID);
            return View(hasta);
        }

        public ActionResult Bilgiler(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            hasta hasta = db.hasta.Find(id);
            if (hasta == null)
            {
                return HttpNotFound();
            }
            return View(hasta);
        }

        [HttpGet]
        public ActionResult Rapor(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            hasta hasta = db.hasta.Find(id);
            var degerler = db.hastaBilgi.Where(x => x.hastaID == hasta.hastaID).ToList();
            ViewBag.hasta = hasta.adi + " " + hasta.soyadi;
            return View(degerler);
            //if (hasta == null)
            //{
            //    return HttpNotFound();
            //}
            //return View(hasta);
        }

        //HASTA RAPOR EKLEME
        [HttpGet]
        public ActionResult RaporEkle(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Error", "NotFound");
            }

            hasta hasta = db.hasta.Find(id);

            if (hasta == null)
            {
                return RedirectToAction("Error", "NotFound");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken] //güvenlik
        [ValidateInput(false)]
        public ActionResult RaporEkle(hastaBilgi hastaBilgi, int? id)
        {
            HastaBilgiValidatior hastaBilgiValidator = new HastaBilgiValidatior();
            ValidationResult results = hastaBilgiValidator.Validate(hastaBilgi);

            var personel = (string)Session["tc"];
            var uye = db.personel.FirstOrDefault(x => x.TCKimlikNo == personel);

            if (results.IsValid)
            {
                hasta hasta = db.hasta.Find(id);
                hastaBilgi.hastaID = hasta.hastaID;
                hastaBilgi.tarih = DateTime.Now;
                db.hastaBilgi.Add(hastaBilgi);
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
            return View(hastaBilgi);
        }

        [HttpGet]
        public ActionResult Ekle()
        {
            ViewBag.kanGrubuID = new SelectList(db.kanGrubu, "kanGrubuID", "kanGrubuAdi");
            ViewBag.vatandasYetkiID = new SelectList(db.vatandasYetki, "vatandasYetkiID", "ad");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken] //güvenlik
        [ValidateInput(false)]
        public ActionResult Ekle(hasta hasta)
        {
            HastaValidatior hastavalidation = new HastaValidatior();
            ValidationResult results = hastavalidation.Validate(hasta);

            if (results.IsValid)
            {
                db.hasta.Add(hasta);
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

            ViewBag.kanGrubuID = new SelectList(db.kanGrubu, "kanGrubuID", "kanGrubuAdi", hasta.kanGrubuID);
            ViewBag.vatandasYetkiID = new SelectList(db.vatandasYetki, "vatandasYetkiID", "ad", hasta.vatandasYetkiID);
            return View(hasta);
        }
    }
}