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
    
    public partial class PHIEU_DATHANG
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PHIEU_DATHANG()
        {
            this.CHITIET_PHIEUDATHANG = new HashSet<CHITIET_PHIEUDATHANG>();
            this.PHIEU_NHAPHANG = new HashSet<PHIEU_NHAPHANG>();
        }
    
        public int Id_PhieuDatHang { get; set; }
        public string MaPhieuDat { get; set; }
        public Nullable<System.DateTime> NgayDat { get; set; }
        public Nullable<System.DateTime> NgayGiao { get; set; }
        public int MaNV { get; set; }
        public decimal TongTien { get; set; }
        public int MaNCC { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CHITIET_PHIEUDATHANG> CHITIET_PHIEUDATHANG { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PHIEU_NHAPHANG> PHIEU_NHAPHANG { get; set; }
        public virtual NHACUNGCAP NHACUNGCAP { get; set; }
        public virtual NHANVIEN NHANVIEN { get; set; }
    }
}
