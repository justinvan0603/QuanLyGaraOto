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
            this.CHITIET_PHIEUNH = new HashSet<CHITIET_PHIEUNH>();
            this.PHIEU_CHI = new HashSet<PHIEU_CHI>();
        }
    
        public int ID_PHIEUNHAPHANG { get; set; }
        public string MA_PHIEUNHAPHANG { get; set; }
        public Nullable<int> ID_PHIEUDATHANG { get; set; }
        public Nullable<System.DateTime> NGAYLAP { get; set; }
        public Nullable<int> MA_NV { get; set; }
        public decimal TONGTIEN { get; set; }
        public Nullable<int> MaNCC { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CHITIET_PHIEUNH> CHITIET_PHIEUNH { get; set; }
        public virtual NHACUNGCAP NHACUNGCAP { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PHIEU_CHI> PHIEU_CHI { get; set; }
        public virtual PHIEU_DATHANG PHIEU_DATHANG { get; set; }
        public virtual NHANVIEN NHANVIEN { get; set; }
    }
}
