using FluentValidation;
using HastaneProgrami.Models.Entity_Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Management;

namespace HastaneProgrami.ValidationRules
{
    public class HastaValidatior : AbstractValidator<hasta>
    {
        public HastaValidatior()
        {
            RuleFor(x => x.adi).NotEmpty().WithMessage("Adı Boş Geçemezsiniz");
            RuleFor(x => x.soyadi).NotEmpty().WithMessage("Soyadı Boş Geçemezsiniz");
            RuleFor(x => x.adi).MaximumLength(45).WithMessage("Ad Alanına 45 Karakterden Fazla Değer Girişi Yapmayınız");
            RuleFor(x => x.soyadi).MaximumLength(45).WithMessage("Soyadı Alanına 45 Karakterden Fazla Değer Girişi Yapmayınız");
            RuleFor(x => x.TCKimlikNo).NotEmpty().WithMessage("T.C Kimlik No Boş Geçemezsiniz");
            RuleFor(x => x.TCKimlikNo).Length(11).WithMessage("T.C Kimlik Alanı 11 Karakter Olmak Zorundadır");
            RuleFor(x => x.dogumTarihi).NotEmpty().WithMessage("Doğum Tarihi Boş Geçemezsiniz");
        }
    }
}