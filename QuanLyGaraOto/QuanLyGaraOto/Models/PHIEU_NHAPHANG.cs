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
    
    public partial class PHIEU_NHAPHANG
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PHIEU_NHAPHANG()
        {
            this.PHIEU_CHI = new HashSet<PHIEU_CHI>();
            this.CHITIET_PHIEUNH = new HashSet<CHITIET_PHIEUNH>();
        }
    
        public int ID_PHIEUNHAPHANG { get; set; }
        public string MA_PHIEUNHAPHANG { get; set; }
        public Nullable<System.DateTime> NGAYLAP { get; set; }
        public Nullable<int> MA_NV { get; set; }
        public Nullable<decimal> TONGTIEN { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PHIEU_CHI> PHIEU_CHI { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CHITIET_PHIEUNH> CHITIET_PHIEUNH { get; set; }
        public virtual NHANVIEN NHANVIEN { get; set; }
    }
}
