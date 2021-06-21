using FluentValidation;
using HastaneProgrami.Models.Entity_Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HastaneProgrami.ValidationRules
{
    public class HastaBilgiValidatior : AbstractValidator<hastaBilgi>
    {
        public HastaBilgiValidatior()
        {
            RuleFor(x => x.alerjiler).NotEmpty().WithMessage("Alerjiler Boş Geçemezsiniz");
            RuleFor(x => x.alerjiler).MaximumLength(50).WithMessage("Alerjiler Alanına 50 Karakterden Fazla Değer Girişi Yapmayınız");

            RuleFor(x => x.ilaclar).NotEmpty().WithMessage("İlaçlar Boş Geçemezsiniz");
            RuleFor(x => x.ilaclar).MaximumLength(50).WithMessage("İlaçlar Alanına 50 Karakterden Fazla Değer Girişi Yapmayınız");

            RuleFor(x => x.hastalık).NotEmpty().WithMessage("Hastalık Boş Geçemezsiniz");
            RuleFor(x => x.hastalık).MaximumLength(50).WithMessage("Hastalık Alanına 50 Karakterden Fazla Değer Girişi Yapmayınız");

            RuleFor(x => x.ameliyat).NotEmpty().WithMessage("Ameliyat Boş Geçemezsiniz");
            RuleFor(x => x.ameliyat).MaximumLength(50).WithMessage("Ameliyat Alanına 50 Karakterden Fazla Değer Girişi Yapmayınız");

            RuleFor(x => x.aciklama).NotEmpty().WithMessage("Açıklama Boş Geçemezsiniz");
            RuleFor(x => x.aciklama).MaximumLength(1000).WithMessage("Açıklama Alanına 1000 Karakterden Fazla Değer Girişi Yapmayınız");
        }
    }
}