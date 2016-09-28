using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuanLyGaraOto.ViewModel
{
    public class DSPhieuDVTableData
    {
            
        public int Id_PhieuDV { get; set; }
        public string MaPhieuDV { get; set; }
        public DateTime NgayLap { get; set; }
        public string TenNV { get; set; }
        public string TenTho { get; set; }
        public decimal TongTien { get; set; }
        public decimal SoTienConLai { get; set; }
        public string TinhTrang { get; set; }
    }
}