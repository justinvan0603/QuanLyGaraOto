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
    
    public partial class CHITIET_PHIEUDV
    {
        public int ID { get; set; }
        public int ID_PHIEUDV { get; set; }
        public int MA_PT { get; set; }
        public Nullable<int> SOLUONG { get; set; }
        public Nullable<decimal> DONGIA { get; set; }
        public Nullable<decimal> THANHTIEN { get; set; }
        public Nullable<int> THOIHAN_BAOHANH { get; set; }
    
        public virtual PHUTUNG PHUTUNG { get; set; }
        public virtual PHIEU_DICHVU PHIEU_DICHVU { get; set; }
    }
}
