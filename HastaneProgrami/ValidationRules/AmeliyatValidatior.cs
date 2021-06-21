using FluentValidation;
using HastaneProgrami.Models.Entity_Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HastaneProgrami.ValidationRules
{
    public class AmeliyatValidatior : AbstractValidator<ameliyat>
    {
        public AmeliyatValidatior()
        {
            RuleFor(x => x.ad).NotEmpty().WithMessage("Başlığı Boş Geçemezsiniz");
            RuleFor(x => x.ad).MaximumLength(100).WithMessage("Başlık Alanı 100 Karakterden Fazla Değer Girişi Yapmayınız");
            RuleFor(x => x.aciklama).NotEmpty().WithMessage("Açıklamayı Boş Geçemezsiniz");
            RuleFor(x => x.aciklama).MaximumLength(100).WithMessage("Açıklama Alanı 100 Karakterden Fazla Değer Girişi Yapmayınız");
            RuleFor(x => x.belge).NotEmpty().WithMessage("Belgeyi Boş Geçemezsiniz");
            RuleFor(x => x.belge).MaximumLength(250).WithMessage("Belge Alanı 250 Karakterden Fazla Değer Girişi Yapmayınız");
            RuleFor(x => x.ameliyatBaslangicTarih).NotEmpty().WithMessage("Ameliyat Başlangıç Tarihini Boş Geçemezsiniz");
            RuleFor(x => x.ameliyatBaslangicSaati).NotEmpty().WithMessage("Ameliyat Başlangıç Saatini Boş Geçemezsiniz");
        }
    }
}