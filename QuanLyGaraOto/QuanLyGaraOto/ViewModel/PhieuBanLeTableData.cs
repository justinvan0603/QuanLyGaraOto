using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuanLyGaraOto.ViewModel
{
    public class PhieuBanLeTableData
    {
        public int ID_PHIEUBANLE { get; set; }
        public string MaPhieuBan { get; set; }
        public DateTime NgayLap { get; set; }
        public DateTime HanChotThanhToan { get; set; }
        public string TenKH { get; set; }
        public string TenNV { get; set; }
        public decimal TongTien { get; set; }
        public decimal SoTienConLai { get; set; }
    }
}