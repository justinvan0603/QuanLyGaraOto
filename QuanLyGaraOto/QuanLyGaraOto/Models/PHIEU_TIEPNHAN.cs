//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace QuanLyGaraOto.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class PHIEU_TIEPNHAN
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PHIEU_TIEPNHAN()
        {
            this.PHIEU_DICHVU = new HashSet<PHIEU_DICHVU>();
        }
    
        public int MA_PHIEUTIEPNHAN { get; set; }
        public int MA_KH { get; set; }
        public Nullable<System.DateTime> NGAYLAP { get; set; }
        public string BIENSO_XE { get; set; }
        public Nullable<int> MASOCHO { get; set; }
        public Nullable<System.DateTime> NGAYTRAXE { get; set; }
        public string TINHTRANG { get; set; }
        public Nullable<int> MA_NV { get; set; }
    
        public virtual KHACHHANG KHACHHANG { get; set; }
        public virtual NHANVIEN NHANVIEN { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PHIEU_DICHVU> PHIEU_DICHVU { get; set; }
        public virtual XE XE { get; set; }
    }
}
