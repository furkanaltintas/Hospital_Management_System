using HastaneProgrami.Models.Entity_Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.Entity;
using System.Web.Security;
using System.Web.DynamicData;
using HastaneProgrami.ValidationRules;
using FluentValidation.Results;

namespace HastaneProgrami.Controllers
{

    public class LoginController : Controller
    {

        // GET: Login
        HastaneContext db = new HastaneContext();

        [AllowAnonymous]
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.Title = "Hastane | Giriş";
            ViewBag.Description = "2021 yılında Furkan Altıntaş tarafından kodlanmıştır.";
            ViewBag.Keywords = "Hastane, Muayene, Randevu, Acil, Klinik";
            ViewBag.Author = "Furkan Altıntaş";

            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Index(personel personel)
        {
            var bilgiler = db.personel.Where(x => x.TCKimlikNo == personel.TCKimlikNo && x.sifre == personel.sifre && x.durum == true).FirstOrDefault();
            if (bilgiler != null)
            {
                FormsAuthentication.SetAuthCookie(bilgiler.adi, false);
                Session["bilgi"] = bilgiler.adi + " " + bilgiler.soyadi;
                Session["tc"] = bilgiler.TCKimlikNo.ToString();
                return RedirectToAction("Index", "Admin");
            }
            else
            {
                ViewBag.LoginError = "Geçersiz T.C Kimlik Numarası veya Şifre";
                return View();
            }
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult Vatandas()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Vatandas(hasta hasta)
        {
            var bilgiler = db.hasta.FirstOrDefault(a => a.TCKimlikNo == hasta.TCKimlikNo && a.sifre == hasta.sifre);
            if (bilgiler != null)
            {
                FormsAuthentication.SetAuthCookie(bilgiler.adi, false);
                Session["bilgi"] = bilgiler.adi + " " + bilgiler.soyadi;
                Session["tc"] = bilgiler.TCKimlikNo.ToString();
                return RedirectToAction("Index", "Vatandas");
            }
            else
            {
                ViewBag.LoginError = "Geçersiz TC Kimlik Numarası veya Şifre";
                return View();
            }
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult UyeOl()
        {
            ViewBag.Title = "Hastane | UyeOl";
            ViewBag.Description = "2021 yılında Furkan Altıntaş tarafından kodlanmıştır.";
            ViewBag.Keywords = "Hastane, Muayene, Randevu, Acil, Klinik";
            ViewBag.Author = "Furkan Altıntaş";
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult UyeOl(hasta u1)
        {
            HastaValidatior hastaValidator = new HastaValidatior();
            ValidationResult results = hastaValidator.Validate(u1);

            if (results.IsValid)
            {
                if (u1.sifre == null)
                {
                    ViewBag.Hata = "Şifre Alanını Girmek Zorundasınız";
                    return View(u1);
                }
                else if (u1.sifre.Length < 5)
                {
                    ViewBag.Hata = "Şifre 5 Karakterden Büyük Olmak Zorunda";
                    return View(u1);
                }
                else
                {
                    u1.vatandasYetkiID = 1;
                    u1.kayitTarihi = DateTime.Now;
                    db.hasta.Add(u1);
                    db.SaveChanges();
                    return RedirectToAction("Home", "Admin");
                }
            }
            else
            {
                foreach (var item in results.Errors)
                {
                    ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                }
            }
            return View(u1);
        }

        public ActionResult LogOut()
        {
            if (User.IsInRole("Hasta"))
            {
                FormsAuthentication.SignOut();
                Session.Abandon();
                return RedirectToAction("Vatandas", "Login");
            }
            else
            {
                FormsAuthentication.SignOut();
                Session.Abandon();
                return RedirectToAction("Index", "Login");
            }
        }
    }
}