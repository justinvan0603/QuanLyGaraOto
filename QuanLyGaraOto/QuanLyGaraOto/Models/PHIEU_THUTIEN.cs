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
    
    public partial class PHIEU_THUTIEN
    {
        public int ID_PHIEUTHUTIEN { get; set; }
        public string MAPHIEUTHU { get; set; }
        public Nullable<System.DateTime> NGAYLAP { get; set; }
        public Nullable<int> ID_PHIEUDV { get; set; }
        public Nullable<int> ID_PHIEUBANXE { get; set; }
        public Nullable<int> ID_PHIEUBANLE { get; set; }
        public string NOIDUNG_THU { get; set; }
        public Nullable<decimal> SOTIEN { get; set; }
        public Nullable<int> MA_NV { get; set; }
        public string TINHTRANG { get; set; }
    
        public virtual NHANVIEN NHANVIEN { get; set; }
        public virtual PHIEUBANXE PHIEUBANXE { get; set; }
        public virtual PHIEU_DICHVU PHIEU_DICHVU { get; set; }
    }
}
