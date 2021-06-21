using FluentValidation;
using HastaneProgrami.Models.Entity_Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HastaneProgrami.ValidationRules
{
    public class PersonelValidatior : AbstractValidator<personel>
    {
        public PersonelValidatior()
        {
            RuleFor(x => x.adi).NotEmpty().WithMessage("Adı Boş Geçemezsiniz");
            RuleFor(x => x.adi).MaximumLength(45).WithMessage("Ad Alanı 45 Karakterden Fazla Değer Girişi Yapmayınız");
            RuleFor(x => x.soyadi).NotEmpty().WithMessage("Soyadı Boş Geçemezsiniz");
            RuleFor(x => x.soyadi).MaximumLength(45).WithMessage("Soyadı Alanı 45 Karakterden Fazla Değer Girişi Yapmayınız");
            RuleFor(x => x.TCKimlikNo).NotEmpty().WithMessage("T.C Kimlik No Geçemezsiniz");
            RuleFor(x => x.TCKimlikNo).Length(11).WithMessage("T.C Kimlik Alanı 11 Karakter Olmak Zorundadır");
            RuleFor(x => x.dogumTarihi).NotEmpty().WithMessage("Doğum Tarihi Boş Geçemezsiniz");
            RuleFor(x => x.diplomaNo).NotEmpty().WithMessage("Diploma No Boş Geçilemez");
            RuleFor(x => x.sifre).NotEmpty().WithMessage("Şifre Boş Geçilemez");
            RuleFor(x => x.sifre).MinimumLength(8).WithMessage("Şifre 5 Karakterden Büyük Olmak Zorundadır");
            RuleFor(x => x.sifre).MaximumLength(50).WithMessage("Şifre 50 Karakterden Küçük Olmak Zorundadır");
            RuleFor(x => x.cepTel).NotEmpty().WithMessage("Cep Telefon Boş Geçilemez");
            RuleFor(x => x.cepTel).Length(13).WithMessage("Cep Telefon 10 Karakter Olmalıdır");
            RuleFor(x => x.evTel).NotEmpty().WithMessage("Ev Telefon Boş Geçilemez");
            RuleFor(x => x.evTel).Length(7).WithMessage("Ev Telefon 7 Karakter Olmalıdır");
            RuleFor(x => x.email).NotEmpty().WithMessage("Email Boş Geçilemez");
            RuleFor(x => x.email).MaximumLength(45).WithMessage("Email Alanı 45 Karakterden Fazla Değer Girişi Yapmayınız");

        }
    }
}