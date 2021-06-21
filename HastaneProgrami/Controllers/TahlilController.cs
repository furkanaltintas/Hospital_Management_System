using FluentValidation.Results;
using HastaneProgrami.Models.Entity_Framework;
using HastaneProgrami.ValidationRules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;

namespace HastaneProgrami.Controllers
{
    public class TahlilController : Controller
    {
        HastaneContext db = new HastaneContext();
        // GET: Tahlil
        [Authorize(Roles = "Yönetici,Uzman Doktor,Doktor,Sekreter")]
        public ActionResult Index()
        {
            var degerler = db.tahlil.ToList();
            return View(degerler);
        }

        [Authorize(Roles = "Yönetici,Doktor")]
        public ActionResult Islem()
        {
            var degerler = db.hasta.ToList();
            return View(degerler);
        }

        [HttpGet]
        public ActionResult Toplam(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Error", "NotFound");
            }
            hasta hasta = db.hasta.Find(id);
            var degerler = db.tahlil.Where(x => x.hastaID == hasta.hastaID).ToList();
            ViewBag.hasta = hasta.adi + " " + hasta.soyadi;
            return View(degerler);
        }

        [Authorize(Roles = "Yönetici,Doktor,Sekreter")]
        [HttpGet]
        public ActionResult Guncelle(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Error", "NotFound");
            }
            tahlil tahlilDurum = db.tahlil.Find(id);
            if (tahlilDurum == null)
            {
                return RedirectToAction("Error", "NotFound");
            }
            return View(tahlilDurum);
        }

        [Authorize(Roles = "Yönetici,Doktor,Sekreter")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Guncelle(tahlil tahlilDurum, int? id)
        {
            TahlilValidatior tahlilvalidation = new TahlilValidatior();
            ValidationResult results = tahlilvalidation.Validate(tahlilDurum);

            tahlil tahlil = db.tahlil.Find(id);

            if (results.IsValid)
            {
                tahlil.sonuc = tahlilDurum.sonuc;
                tahlil.durum = tahlilDurum.durum;
                tahlil.miktar = tahlilDurum.miktar;
                tahlil.aciklama = tahlilDurum.aciklama;
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
            return View(tahlilDurum);
        }

        [Authorize(Roles = "Yönetici,Doktor,Sekreter")]
        public ActionResult Bilgiler(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Error", "NotFound");
            }
            tahlil tahlil = db.tahlil.Find(id);
            if (tahlil == null)
            {
                return RedirectToAction("Error", "NotFound");
            }
            return View(tahlil);
        }


        [Authorize(Roles = "Yönetici,Doktor,Sekreter")]
        public ActionResult Sonuc(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Error", "NotFound");
            }

            tahlil tahlil = db.tahlil.Find(id);

            if (tahlil == null)
            {
                return RedirectToAction("Error", "NotFound");
            }
            return View(tahlil);
        }


        [Authorize(Roles = "Yönetici,Doktor")]
        [HttpGet]
        public ActionResult Ekle(int? id)
        {
            ViewBag.kan = new SelectList(db.kanTup, "kanTupID", "ad");
            ViewBag.idrar = new SelectList(db.idrarTup, "idrarTupID", "ad");

            //List<SelectListItem> kanTup = (from i in db.kanTup.ToList()
            //                               select new SelectListItem
            //                               {
            //                                   Text = i.ad,
            //                                   Value = i.kanTupID.ToString()
            //                               }).ToList();
            //ViewBag.kan = kanTup;

            //List<SelectListItem> idrarTup = (from i in db.idrarTup.ToList()
            //                                 select new SelectListItem
            //                                 {
            //                                     Text = i.ad,
            //                                     Value = i.idrarTupID.ToString()
            //                                 }).ToList();
            //ViewBag.idrar = idrarTup;

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

        [Authorize(Roles = "Yönetici,Doktor")]
        [HttpPost]
        [ValidateAntiForgeryToken] //güvenlik
        [ValidateInput(false)]
        public ActionResult Ekle(tahlil tahlil, int? id)
        {
            if (ModelState.IsValid)
            {
                ViewBag.kan = new SelectList(db.kanTup, "kanTupID", "ad");
                ViewBag.idrar = new SelectList(db.idrarTup, "idrarTupID", "ad");
                if (tahlil.miktar != null)
                {
                    var personel = (string)Session["tc"];
                    var uye = db.personel.FirstOrDefault(x => x.TCKimlikNo == personel);
                    hasta hasta = db.hasta.Find(id);
                    tahlil.hastaID = hasta.hastaID;
                    tahlil.personelID = uye.personelID;
                    tahlil.tarih = DateTime.Now;
                    db.tahlil.Add(tahlil);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Hata = "Miktar Boş Olamaz";
                }
            }
            return View(tahlil);
        }
    }
}