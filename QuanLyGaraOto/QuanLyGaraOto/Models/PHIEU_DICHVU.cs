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
    
    public partial class PHIEU_DICHVU
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PHIEU_DICHVU()
        {
            this.CHITIET_PHIEUDV = new HashSet<CHITIET_PHIEUDV>();
            this.PHIEU_THUTIEN = new HashSet<PHIEU_THUTIEN>();
        }
    
        public int ID_PHIEUDV { get; set; }
        public string MA_PHIEUDV { get; set; }
        public Nullable<System.DateTime> NGAYLAP { get; set; }
        public Nullable<System.DateTime> HANCHOTTHANHTOAN { get; set; }
        public Nullable<int> MAPHIEU_TIEPNHAN { get; set; }
        public Nullable<decimal> TONGTIEN { get; set; }
        public Nullable<decimal> SOTIEN_CONLAI { get; set; }
        public Nullable<int> MATHO { get; set; }
        public Nullable<decimal> TIENCONG { get; set; }
        public Nullable<int> MA_NHANVIEN { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CHITIET_PHIEUDV> CHITIET_PHIEUDV { get; set; }
        public virtual NHANVIEN NHANVIEN { get; set; }
        public virtual PHIEU_TIEPNHAN PHIEU_TIEPNHAN { get; set; }
        public virtual THO THO { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PHIEU_THUTIEN> PHIEU_THUTIEN { get; set; }
    }
}
