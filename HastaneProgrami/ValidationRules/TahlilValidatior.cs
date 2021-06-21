using FluentValidation;
using HastaneProgrami.Models.Entity_Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HastaneProgrami.ValidationRules
{
    public class TahlilValidatior : AbstractValidator<tahlil>
    {
        public TahlilValidatior()
        {
            RuleFor(x => x.sonuc).NotEmpty().WithMessage("Sonuç Boş Geçemezsiniz");
            RuleFor(x => x.sonuc).MaximumLength(50).WithMessage("Sonuç 50 Karakterden Fazla Değer Girişi Yapmayınız");

            RuleFor(x => x.durum).NotEmpty().WithMessage("Durum Boş Geçemezsiniz");
            RuleFor(x => x.durum).MaximumLength(50).WithMessage("Durum 50 Karakterden Fazla Değer Girişi Yapmayınız");

            RuleFor(x => x.aciklama).NotEmpty().WithMessage("Açıklama Boş Geçemezsiniz");
            RuleFor(x => x.aciklama).MaximumLength(500).WithMessage("Açıklama 500 Karakterden Fazla Değer Girişi Yapmayınız");
        }
    }
}