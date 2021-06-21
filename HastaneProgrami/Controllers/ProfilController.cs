using FluentValidation.Results;
using HastaneProgrami.Models.Entity_Framework;
using HastaneProgrami.ValidationRules;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace HastaneProgrami.Controllers
{
    [Authorize(Roles = "Yönetici,Uzman Doktor,Doktor,Danışman,Sekreter")]
    public class ProfilController : Controller
    {
        private HastaneContext db = new HastaneContext();
        // GET: Profil

        [HttpGet]
        [Authorize]
        public ActionResult KimlikBilgileri()
        {
            var personel = (string)Session["tc"];
            var uye = db.personel.FirstOrDefault(x => x.TCKimlikNo == personel);

            return View(uye);
        }

        [HttpPost]
        public ActionResult KimlikBilgileri(personel p)
        {
            //if (ModelState.IsValid)
            //{
            //    var personel = (string)Session["adi"];
            //    var uye = db.personel.FirstOrDefault(x => x.adi == personel);
            //    uye.adi = p.adi;
            //    uye.soyadi = p.soyadi;
            //    uye.email = p.TCKimlikNo;
            //    uye.cinsiyet = p.cinsiyet;
            //    uye.dogumTarihi = p.dogumTarihi;
            //    db.SaveChanges();
            //    ViewBag.Basarili = "Güncelleme Başarıyla Gerçekleşmiştir";
            //    return RedirectToAction("Bilgiler");
            //}
            //return View(p);

            //KİMLİK BİLGİLERİ DEĞİŞTİRİLEMEZ !!!
            return View();
        }

        [HttpGet]
        [Authorize]
        public ActionResult IletisimBilgileri()
        {
            var personel = (string)Session["tc"];
            var uye = db.personel.FirstOrDefault(x => x.TCKimlikNo == personel);

            return View(uye);
        }

        [HttpPost]
        public ActionResult IletisimBilgileri(personel p)
        {
            var personel = (string)Session["tc"];
            var uye = db.personel.FirstOrDefault(x => x.TCKimlikNo == personel);

            if (p.cepTel == null)
            {
                ViewBag.Basarisiz = "Cep Telefon Alanı Boş Olamaz";
            }
            else if (p.cepTel.Length != 13)
            {
                ViewBag.Basarisiz = "Cep Telefon 10 Karakter Olmalıdır";
            }
            else if (p.evTel.Length != 7)
            {
                ViewBag.Basarisiz = "Ev Telefon 7 Karakter Olmalıdır";
            }
            else if (p.cepTel.Length == 13 && p.evTel.Length == 7 && p.email != null)
            {
                uye.cepTel = p.cepTel;
                uye.evTel = p.evTel;
                uye.email = p.email;
                db.SaveChanges();
                ViewBag.Basarili = "Güncelleme Başarıyla Gerçekleşmiştir";
                return RedirectToAction("IletisimBilgileri");
            }
            return View(p);
        }

        [HttpGet]
        [Authorize]
        public ActionResult ParolaIslemleri()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ParolaIslemleri(personel p)
        {
            var personel = (string)Session["tc"];
            var uye = db.personel.FirstOrDefault(x => x.TCKimlikNo == personel);

            if (uye.email == p.email && uye.sifre == p.sifre)
            {
                if (uye.sifre == p.rsifre)
                {
                    ViewBag.Basarisiz = "Yeni şifreniz eski şifre ile aynı olamaz";
                    return View(p);
                }
                else if (p.rsifre == null)
                {
                    ViewBag.Basarisiz = "Yeni Şifre Boş Olamaz";
                    return View(p);
                }
                else if (p.rsifre.Length < 5)
                {
                    ViewBag.Basarisiz = "Yeni Şifre 5 Karakterden Fazla Olmalıdır";
                    return View(p);
                }
                uye.email = p.email;
                uye.sifre = p.rsifre;
                uye.rsifre = uye.sifre;
                db.SaveChanges();
                ViewBag.Basarili = "Güncelleme Başarıyla Gerçekleşmiştir";
                return RedirectToAction("Index", "Login");
            }
            else
            {
                ViewBag.Basarisiz = "Email ve şifre hatalı";
            }
            return View(p);
        }

        //var model = db.personel.Where(x => x.email == p.email).FirstOrDefault();
        //if (model != null)
        //{
        //    Guid random = Guid.NewGuid();
        //    model.sifre = random.ToString().Substring(0, 9);
        //    db.SaveChanges();
        //    SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
        //    client.EnableSsl = true;
        //    MailMessage mail = new MailMessage();
        //    mail.From = new MailAddress("mail", "sıfırlama");
        //    mail.To.Add("gmail");
        //    mail.IsBodyHtml = true;
        //    mail.Subject = "Şifre Sıfırlama İsteği";
        //    mail.Body = "Merhaba" + model.adi + model.soyadi + "<br/> Kullanıcı Adınız =" + model.adi + "<br/> Şifreniz =" + model.sifre;
        //    NetworkCredential nc = new NetworkCredential("mail", "sifre");
        //    client.Credentials = nc;
        //    client.Send(mail);
        //    return RedirectToAction("Login", "Index");
        //}
        //ViewBag.Error = "Böyle bir email adresi sistemimizde bulunmamaktadır";
        //return View(p);
    }
}