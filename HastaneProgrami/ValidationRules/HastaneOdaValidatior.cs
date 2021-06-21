using FluentValidation;
using HastaneProgrami.Models.Entity_Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HastaneProgrami.ValidationRules
{
    public class HastaneOdaValidatior : AbstractValidator<hastaneOda>
    {
        public HastaneOdaValidatior()
        {
            RuleFor(x => x.ad).NotEmpty().WithMessage("Oda Adı Alanını Boş Geçemezsiniz");
            RuleFor(x => x.ad).MaximumLength(50).WithMessage("Oda Adı  50 Karakterden Fazla Değer Girişi Yapmayınız");

            RuleFor(x => x.baslangic).NotEmpty().WithMessage("Oda Kontenjan Alanını Boş Geçemezsiniz");
        }
    }
}