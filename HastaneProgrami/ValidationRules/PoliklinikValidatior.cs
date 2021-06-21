using FluentValidation;
using HastaneProgrami.Models.Entity_Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HastaneProgrami.ValidationRules
{
    public class PoliklinikValidatior : AbstractValidator<poliklinik>
    {
        public PoliklinikValidatior()
        {
            RuleFor(x => x.ad).NotEmpty().WithMessage("Poliklinik Adını Boş Geçemezsiniz");
            RuleFor(x => x.ad).MaximumLength(50).WithMessage("Poliklinik Adı Alanı 50 Karakterden Fazla Değer Girişi Yapmayınız");
        }
    }
}