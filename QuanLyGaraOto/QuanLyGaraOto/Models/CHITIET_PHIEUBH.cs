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
    
    public partial class CHITIET_PHIEUBH
    {
        public int Id { get; set; }
        public Nullable<int> MA_PHIEUDV { get; set; }
        public int MA_PHUTUNG { get; set; }
        public int MA_PHIEUBH { get; set; }
        public Nullable<int> MAPHIEU_BANLE { get; set; }
        public Nullable<System.DateTime> NGAYHENTRA { get; set; }
        public Nullable<System.DateTime> NGAYTRA { get; set; }
    
        public virtual PHUTUNG PHUTUNG { get; set; }
        public virtual PHIEU_BAOHANH PHIEU_BAOHANH { get; set; }
    }
}
