using FluentValidation;
using HastaneProgrami.Models.Entity_Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HastaneProgrami.ValidationRules
{
    public class MuayeneValidatior : AbstractValidator<muayene>
    {
        public MuayeneValidatior()
        {
            RuleFor(x => x.baslik).NotEmpty().WithMessage("Başlık Alanını Boş Geçemezsiniz");
            RuleFor(x => x.baslik).MaximumLength(50).WithMessage("Başlık 100 Karakterden Fazla Değer Girişi Yapmayınız");

            RuleFor(x => x.aciklama).NotEmpty().WithMessage("Açıklama Alanını Boş Geçemezsiniz");
            RuleFor(x => x.aciklama).MaximumLength(50).WithMessage("Açıklama 200 Karakterden Fazla Değer Girişi Yapmayınız");
        }
    }
}