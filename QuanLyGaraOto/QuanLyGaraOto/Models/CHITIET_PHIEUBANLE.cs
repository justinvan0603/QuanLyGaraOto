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
    
    public partial class CHITIET_PHIEUBANLE
    {
        public int ID { get; set; }
        public Nullable<int> ID_PHIEUBANLE { get; set; }
        public Nullable<int> MAPT { get; set; }
        public Nullable<int> SOLUONG { get; set; }
        public Nullable<decimal> DONGIA { get; set; }
        public Nullable<decimal> THANHTIEN { get; set; }
        public Nullable<int> THOIHAN_BAOHANH { get; set; }
    
        public virtual PHIEU_BANLE PHIEU_BANLE { get; set; }
        public virtual PHUTUNG PHUTUNG { get; set; }
    }
}