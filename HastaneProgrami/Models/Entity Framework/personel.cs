//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HastaneProgrami.Models.Entity_Framework
{
    using System;
    using System.Collections.Generic;
    
    public partial class personel
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public personel()
        {
            this.kullanici = new HashSet<kullanici>();
            this.muayene = new HashSet<muayene>();
            this.randevu = new HashSet<randevu>();
            this.recete = new HashSet<recete>();
            this.tahlil = new HashSet<tahlil>();
            this.ameliyatKadro = new HashSet<ameliyatKadro>();
        }
    
        public int personelID { get; set; }
        public Nullable<int> bolumID { get; set; }
        public Nullable<int> poliklinikID { get; set; }
        public int yetkiID { get; set; }
        public int kanGrubuID { get; set; }
        public Nullable<bool> durum { get; set; }
        public string adi { get; set; }
        public string soyadi { get; set; }
        public Nullable<bool> cinsiyet { get; set; }
        public Nullable<bool> medeniHali { get; set; }
        public string adres { get; set; }
        public string cepTel { get; set; }
        public string evTel { get; set; }
        public string email { get; set; }
        public string sifre { get; set; }
        public string rsifre { get; set; }
        public string dogumYeri { get; set; }
        public System.DateTime dogumTarihi { get; set; }
        public string TCKimlikNo { get; set; }
        public Nullable<int> sicilNo { get; set; }
        public string diplomaNo { get; set; }
        public Nullable<System.DateTime> kayitTarihi { get; set; }
        public string avatar { get; set; }
    
        public virtual kanGrubu kanGrubu { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<kullanici> kullanici { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<muayene> muayene { get; set; }
        public virtual poliklinik poliklinik { get; set; }
        public virtual yetki yetki { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<randevu> randevu { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<recete> recete { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tahlil> tahlil { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ameliyatKadro> ameliyatKadro { get; set; }
        public virtual bolum bolum { get; set; }
    }
}