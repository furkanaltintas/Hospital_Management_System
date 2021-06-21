using FluentValidation;
using HastaneProgrami.Models.Entity_Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HastaneProgrami.ValidationRules
{
    public class AmeliyatOdasiValidatior : AbstractValidator<ameliyatOdasi>
    {
        public AmeliyatOdasiValidatior()
        {
            RuleFor(x => x.ad).NotEmpty().WithMessage("Ameliyat Odası Alanını Boş Geçemezsiniz");
            RuleFor(x => x.ad).MaximumLength(50).WithMessage("Ameliyat Odası Alanı 20 Karakterden Fazla Değer Girişi Yapmayınız");
        }
    }
}