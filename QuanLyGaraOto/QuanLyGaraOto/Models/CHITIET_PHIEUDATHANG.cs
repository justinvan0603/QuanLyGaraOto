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
    
    public partial class CHITIET_PHIEUDATHANG
    {
        public int ID { get; set; }
        public Nullable<int> Id_PhieuDatHang { get; set; }
        public Nullable<int> MA_PHUTUNG { get; set; }
        public Nullable<int> SOLUONG { get; set; }
    
        public virtual PHIEU_DATHANG PHIEU_DATHANG { get; set; }
        public virtual PHUTUNG PHUTUNG { get; set; }
    }
}
