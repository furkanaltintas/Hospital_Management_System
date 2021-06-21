using FluentValidation.Results;
using HastaneProgrami.Models.Entity_Framework;
using HastaneProgrami.ValidationRules;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace HastaneProgrami.Controllers
{
    [Authorize(Roles = "Yönetici,Uzman Doktor")]
    public class AmeliyatController : Controller
    {
        HastaneContext db = new HastaneContext();
        // GET: Ameliyat

        //HASTA LİSTESİ
        public ActionResult Index()
        {
            var personel = (string)Session["tc"];
            var uye = db.personel.FirstOrDefault(x => x.TCKimlikNo == personel);

            ViewBag.Title = "Hastane | Ameliyat Listesi";
            ViewBag.Description = "2021 yılında Furkan Altıntaş tarafından kodlanmıştır.";
            ViewBag.Keywords = "Hastane, Muayene, Randevu, Acil, Klinik";
            ViewBag.Author = "Furkan Altıntaş";

            var degerler = db.hasta.ToList();
            return View(degerler);
        }

        //AMELİYAT OLANLARIN LİSTESİ
        public ActionResult Liste()
        {
            var liste = db.ameliyat.ToList();
            return View(liste);
        }

        public ActionResult Ekip(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ameliyat ameliyat = db.ameliyat.Find(id);
            var degerler = db.ameliyat.Where(x => x.ameliyatEkip.ameliyatEkipID == ameliyat.ameliyatEkibiID).ToList();
            ViewBag.hasta = ameliyat.hasta.adi + " " + ameliyat.hasta.soyadi;
            if (ameliyat == null)
            {
                return HttpNotFound();
            }
            return View(degerler);

        }

        public ActionResult Kadro(int? id)
        {
            ameliyat ameliyat = db.ameliyat.Find(id);

            var ameliyatkadro = db.ameliyatKadro.Where(x => x.ameliyatEkipID == ameliyat.ameliyatEkibiID).ToList();
            return View(ameliyatkadro);
        }

        public ActionResult Detay(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ameliyat ameliyat = db.ameliyat.Find(id);
            if (ameliyat == null)
            {
                return HttpNotFound();
            }

            return View(ameliyat);
        }

        public ActionResult Sonuc(int? id)
        {
            ameliyat ameliyat = db.ameliyat.Find(id);
            ViewBag.hasta = ameliyat;

            if (id == null)
            {
                return View(ameliyat);
            }

            ViewBag.vatandas = ameliyat.hasta.adi + " " + ameliyat.hasta.soyadi;

            var ameliyatSonuc = db.ameliyatSonuc.Where(x => x.ameliyatID == ameliyat.ameliyatID).SingleOrDefault();
            ViewBag.sorgu = ameliyatSonuc;

            if (ameliyatSonuc == null)
            {
                return View(ameliyatSonuc);
            }
            return View(ameliyatSonuc);
        }

        [HttpGet]
        public ActionResult SonucEkle(int? id)
        {
            //List<SelectListItem> oda = (from i in db.hastaneOda.Where(x => x.durum == true).ToList()
            //                            select new SelectListItem
            //                            {
            //                                Text = i.ad,
            //                                Value = i.hastaneOdaID.ToString()
            //                            }).ToList();
            //ViewBag.hastaneOda = oda;

            ViewBag.hastaneOda = new SelectList(db.hastaneOda.Where(x => x.durum == true), "hastaneOdaID", "ad");

            if (id == null)
            {
                return RedirectToAction("Error", "NotFound");
            }

            ameliyat ameliyat = db.ameliyat.Find(id);

            var ameliyatsonuc = db.ameliyatSonuc.Where(x => x.ameliyatID == ameliyat.ameliyatID).ToList();
            ViewBag.sonuc = ameliyatsonuc;

            if (ameliyat == null)
            {
                return RedirectToAction("Error", "NotFound");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken] //güvenlik
        [ValidateInput(false)]
        public ActionResult SonucEkle(ameliyatSonuc ameliyatSonuc, int? id)
        {
            AmeliyatSonucValidatior ameliyatSonucValidator = new AmeliyatSonucValidatior();
            ValidationResult results = ameliyatSonucValidator.Validate(ameliyatSonuc);

            ameliyat ameliyat = db.ameliyat.Find(id);

            var hastaneOda = db.hastaneOda.Where(x => x.hastaneOdaID == ameliyatSonuc.hastaneOdaID).SingleOrDefault();

            //ViewBag.hastaneOda = new SelectList(db.hastaneOda, "hastaneOdaID", "ad");

            if (results.IsValid)
            {
                ViewBag.hastaneOda = new SelectList(db.hastaneOda.Where(x => x.durum == true), "hastaneOdaID", "ad");
                if (ameliyatSonuc.ameliyatDurum == true && ameliyatSonuc.hastaDurum == true)
                {
                    hastaneOda.bitis = hastaneOda.bitis + 1;
                }

                if (hastaneOda.baslangic == hastaneOda.bitis)
                {
                    hastaneOda.durum = false;
                }
                ameliyat.ameliyatEkip.ekipDurum = true;
                ameliyat.ameliyatOdasi.odaDurum = true;
                ameliyatSonuc.ameliyatID = ameliyat.ameliyatID;
                ameliyatSonuc.ameliyatBitisTarih = DateTime.Now;
                db.ameliyatSonuc.Add(ameliyatSonuc);
                db.SaveChanges();
                return RedirectToAction("Liste","Ameliyat");
            }
            else
            {
                foreach (var item in results.Errors)
                {
                    ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                }
            }

            return View(ameliyatSonuc);
        }

        //AMELİYAT BELGESİ + ONAYI
        [HttpGet]
        public ActionResult Ekle(int? id)
        {
            ViewBag.oda = new SelectList(db.ameliyatOdasi.Where(x => x.durum == true).Where(x => x.odaDurum == true), "ameliyatOdasiID", "ad");

            ViewBag.ekip = new SelectList(db.ameliyatEkip.Where(x => x.durum == true).Where(x => x.ekipDurum == true), "ameliyatEkipID", "ad");

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
        public ActionResult Ekle(ameliyat ameliyat, int? id)
        {
            int raporSorgusu, tahlilSorgusu;
            hastaBilgi hb = new hastaBilgi();
            hasta hasta = db.hasta.Find(id);

            var oda = db.ameliyatOdasi.Where(x => x.ameliyatOdasiID == ameliyat.ameliyatOdasiID).FirstOrDefault();

            var ekip = db.ameliyatEkip.Where(x => x.ameliyatEkipID == ameliyat.ameliyatEkibiID).FirstOrDefault();

            var bilgi = db.hastaBilgi.Where(x => x.hastaID == hasta.hastaID).OrderByDescending(x => x.hastaBilgiID).FirstOrDefault();

            var tahlil = db.tahlil.Where(x => x.hastaID == hasta.hastaID).OrderByDescending(x => x.tahlilID).FirstOrDefault();

            var degerler = db.hastaBilgi.Where(x => x.hastaID == hasta.hastaID).ToList();
            raporSorgusu = degerler.Count;

            var degerler2 = db.tahlil.Where(x => x.hastaID == hasta.hastaID).ToList();
            tahlilSorgusu = degerler2.Count;

            AmeliyatValidatior ameliyatValidator = new AmeliyatValidatior();
            ValidationResult results = ameliyatValidator.Validate(ameliyat);

            ViewBag.oda = new SelectList(db.ameliyatOdasi, "ameliyatOdasiID", "ad");
            ViewBag.ekip = new SelectList(db.ameliyatEkip, "ameliyatEkipID", "ad");

            if (results.IsValid)
            {

                if (ameliyat.onay == true && Request.Files.Count > 0)
                {
                    var file = Request.Files[0];

                    if (file != null && file.ContentLength > 0)
                    {
                        if (raporSorgusu > 0)
                        {
                            if (tahlilSorgusu > 0)
                            {
                                string dosyaAdi = Path.GetFileName(file.FileName);
                                string uzanti = Path.GetExtension(file.FileName);
                                string adres = "~/Image/" + dosyaAdi + uzanti;
                                Request.Files[0].SaveAs(Server.MapPath(adres));
                                ameliyat.belge = "/Image/" + dosyaAdi + uzanti;

                                var personel = (string)Session["tc"];
                                var uye = db.personel.FirstOrDefault(x => x.TCKimlikNo == personel);

                                oda.odaDurum = false; //oda dolu
                                ekip.ekipDurum = false; //ekip dolu

                                hb.hastaID = hasta.hastaID;
                                hb.aciklama = ameliyat.aciklama;
                                hb.ameliyat = ameliyat.ad;
                                ameliyat.hastaID = hasta.hastaID;
                                ameliyat.tahlilID = tahlil.tahlilID;
                                ameliyat.bolumID = 2;
                                ameliyat.hastaBilgiID = bilgi.hastaBilgiID;
                                ameliyat.onayTarih = DateTime.Now;
                                hb.tarih = ameliyat.onayTarih;
                                db.hastaBilgi.Add(hb);
                                db.ameliyat.Add(ameliyat);
                                db.SaveChanges();
                                return RedirectToAction("Index");
                            }
                            else
                            {
                                ViewBag.Ameliyat = "Hastanın Tahlili Bulunmamaktadır. Tahlil Eklemeniz Gereklidir.";
                                return View(ameliyat);
                            }
                        }
                        else
                        {
                            ViewBag.Ameliyat = "Hastanın Raporu Bulunmamaktadır. Rapor Eklemeniz Gereklidir.";
                            return View(ameliyat);
                        }
                    }
                    else
                    {
                        ViewBag.Ameliyat = "Ameliyat Belge Alanını boş bırakamazsınız";
                        return View(ameliyat);
                    }
                }
                else
                {
                    ViewBag.Ameliyat = "Ameliyat belgesini eklemeden ve ameliyat onayı olmadan ameliyat işlemi yapılmaz";
                    return View(ameliyat);
                }
            }
            else
            {
                foreach (var item in results.Errors)
                {
                    ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                }
            }
            ViewBag.Ameliyat = "Ameliyat belgesini eklemeden ve ameliyat onayı olmadan ameliyat işlemi yapılmaz";
            return View(ameliyat);
        }
    }
}