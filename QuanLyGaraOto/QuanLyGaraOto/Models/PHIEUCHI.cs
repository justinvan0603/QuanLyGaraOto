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
    
    public partial class PHIEUCHI
    {
        public int ID { get; set; }
        public string MA_PHIEUCHI { get; set; }
        public string NOIDUNG { get; set; }
        public Nullable<decimal> SOTIEN { get; set; }
        public Nullable<int> MA_NV { get; set; }
        public Nullable<int> MA_PHIEUNHAP { get; set; }
        public string TINHTRANG { get; set; }
    
        public virtual NHANVIEN NHANVIEN { get; set; }
        public virtual PHIEU_NHAPHANG PHIEU_NHAPHANG { get; set; }
    }
}
