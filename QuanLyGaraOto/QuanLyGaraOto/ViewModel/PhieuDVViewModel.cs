using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QuanLyGaraOto.Models;
namespace QuanLyGaraOto.ViewModel
{
    public class PhieuDVViewModel
    {
        public int MaNV { get; set; }
        public int MaPhieuTiepNhan { get; set; }
        public int MaPhieuDV { get; set; }
        public List<PHUTUNG> ListPhuTung { get; set; }
        public List<TIENCONG> ListTienCong { get; set; }
        public List<CHITIET_PHIEUDV> ListChiTietPhieu { get; set; }
        public List<THO> ListTho { get; set; }
        public DateTime NgayLap { get; set; }
        public string TenNV { get; set; }
        public string TenKH { get; set; }
        public int MaTho { get; set; }
        public string MaTienCong { get; set; }
    }
}