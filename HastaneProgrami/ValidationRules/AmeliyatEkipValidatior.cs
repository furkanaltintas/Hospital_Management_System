using FluentValidation;
using HastaneProgrami.Models.Entity_Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HastaneProgrami.ValidationRules
{
    public class AmeliyatEkipValidatior : AbstractValidator<ameliyatEkip>
    {
        public AmeliyatEkipValidatior()
        {
            RuleFor(x => x.ad).NotEmpty().WithMessage("Ameliyat Ekibi Alanını Boş Geçemezsiniz");
            RuleFor(x => x.ad).MaximumLength(50).WithMessage("Ameliyat Ekibi 50 Karakterden Fazla Değer Girişi Yapmayınız");
        }
    }
}