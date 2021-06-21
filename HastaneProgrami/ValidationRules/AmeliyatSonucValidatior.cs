using FluentValidation;
using HastaneProgrami.Models.Entity_Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HastaneProgrami.ValidationRules
{
    public class AmeliyatSonucValidatior : AbstractValidator<ameliyatSonuc>
    {
        public AmeliyatSonucValidatior()
        {
            RuleFor(x => x.ameliyatDurum).NotEmpty().WithMessage("Ameliyat Durum Alanını Boş Geçemezsiniz");
            RuleFor(x => x.hastaDurum).NotEmpty().WithMessage("Hasta Durum Alanını Boş Geçemezsiniz");
        }
    }
}