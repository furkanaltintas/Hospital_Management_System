using HastaneProgrami.Models.Entity_Framework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;

namespace HastaneProgrami.Controllers
{
    [Authorize(Roles = "Yönetici,Doktor,Sekreter")]
    public class MuayeneController : Controller
    {
        // GET: MuayeneAdmin
        HastaneContext db = new HastaneContext();
        muayene m = new muayene();
        public ActionResult Index()
        {
            var personel = (string)Session["tc"];
            var uye = db.personel.FirstOrDefault(x => x.TCKimlikNo == personel);

            ViewBag.Title = "Hastane | Muayene";
            ViewBag.Description = "2021 yılında Furkan Altıntaş tarafından kodlanmıştır.";
            ViewBag.Keywords = "Hastane, Muayene, Randevu, Acil, Klinik";
            ViewBag.Author = "Furkan Altıntaş";

            if (User.IsInRole("Yönetici"))
            {
                var muayene = db.muayene.ToList();
                return View(muayene);
            }
            if (User.IsInRole("Sekreter"))
            {
                var model = db.muayene.Where(x => x.randevu.poliklinikID == uye.poliklinikID).ToList();
                return View(model);
            }
            if (User.IsInRole("Doktor"))
            {
                var model = db.muayene.Where(x => x.randevu.personelID == uye.personelID).ToList();
                return View(model);
            }
            return View();
        }

        public ActionResult Islem()
        {
            var personel = (string)Session["tc"];
            var uye = db.personel.FirstOrDefault(x => x.TCKimlikNo == personel);

            if (User.IsInRole("Yönetici"))
            {
                var yönetici = db.muayene.ToList();
                return View(yönetici);
            }
            if (User.IsInRole("Sekreter"))
            {
                var model = db.muayene.Where(x => x.randevu.poliklinikID == uye.poliklinikID).ToList();
                return View(model);
            }
            if (User.IsInRole("Doktor"))
            {
                var model = db.muayene.Where(x => x.randevu.personelID == uye.personelID).ToList();
                return View(model);
            }
            return View();
        }

        [HttpGet]
        public ActionResult Gecmis(int? id)
        {
            var personel = (string)Session["tc"];
            var uye = db.personel.FirstOrDefault(x => x.TCKimlikNo == personel);

            if (id == null)
            {
                return RedirectToAction("Error", "NotFound");
            }

            hasta hasta = db.hasta.Find(id);
            ViewBag.hasta = hasta.adi + " " + hasta.soyadi;

            if (User.IsInRole("Yönetici"))
            {
                var degerler = db.muayene.Where(x => x.hastaID == hasta.hastaID).ToList();
                return View(degerler);
            }
            if (User.IsInRole("Sekreter"))
            {
                var model = db.muayene.Where(x => x.randevu.poliklinikID == uye.poliklinikID && x.randevu.durum == false).ToList();
                return View(model);
            }
            if (User.IsInRole("Doktor"))
            {
                var model = db.muayene.Where(x => x.randevu.hastaID == hasta.hastaID && x.randevu.durum == false).ToList();
                return View(model);
            }
            return View();
        }

        public ActionResult Detay(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            muayene muayene = db.muayene.Find(id);
            if (muayene == null)
            {
                return HttpNotFound();
            }
            return View(muayene);
        }

        public ActionResult Bilgi(int? id)
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
            //muayene muayene = db.muayene.Find(id);
            //if (muayene == null)
            //{
            //    return RedirectToAction("Error", "NotFound");
            //}
            //return View(muayene);
        }

        [HttpGet]
        public ActionResult Ekle()
        {
            List<SelectListItem> degerler = (from i in db.randevu.ToList()
                                             select new SelectListItem
                                             {
                                                 Text = i.hasta.TCKimlikNo,
                                                 Value = i.hastaID.ToString()
                                             }).ToList();
            ViewBag.randevu = degerler;

            ViewBag.randevuID = new SelectList(db.randevu, "randevuID", "TCKimlikNo");
            ViewBag.hastaID = new SelectList(db.hasta, "hastaID", "TCKimlikNo");
            ViewBag.hastaSikayetID = new SelectList(db.muayene, "muayeneID", "tarih");
            ViewBag.muayeneSonucID = new SelectList(db.muayene, "muayeneID", "aciklama");
            ViewBag.personelID = new SelectList(db.personel, "personelID", "adi");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken] //güvenlik
        [ValidateInput(false)]
        public ActionResult Ekle(muayene muayene, randevu randevu) //gelmesini istediğimiz
        {
            if (ModelState.IsValid)
            {
                var personel = (string)Session["adi"];
                var uye = db.personel.FirstOrDefault(x => x.adi == personel);
                muayene.personelID = uye.personelID;
                muayene.hastaID = randevu.hastaID;
                muayene.randevuID = randevu.randevuID;
                muayene.randevu.durum = randevu.durum = false;
                muayene.tarih = DateTime.Now;
                db.muayene.Add(muayene);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.randevuID = new SelectList(db.randevu, "randevuID", "TCKimlikNo", muayene.randevuID);
            ViewBag.hastaID = new SelectList(db.hasta, "hastaID", "TCKimlikNo", muayene.hastaID);
            ViewBag.hastaSikayetID = new SelectList(db.muayene, "muayeneID", "tarih", muayene.tarih);
            ViewBag.muayeneSonucID = new SelectList(db.muayene, "muayeneID", "aciklama", muayene.aciklama);
            ViewBag.personelID = new SelectList(db.personel, "personelID", "adi", muayene.personelID);
            return View(muayene);
        }
    }
}