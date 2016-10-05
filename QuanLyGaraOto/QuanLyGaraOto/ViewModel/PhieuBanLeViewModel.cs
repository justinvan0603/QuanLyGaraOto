using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QuanLyGaraOto.Models;
namespace QuanLyGaraOto.ViewModel
{
    public class PhieuBanLeViewModel
    {
        public PHIEU_BANLE PhieuBanLe { get; set; }
        public List<KHACHHANG> ListKhachHang { get; set; }
        public List<PHUTUNG> ListPhuTung { get; set; }
        public string TenNV { get; set; }
        public string TenKH { get; set; }
        public List<CHITIET_PHIEUBANLE> ListChiTietPhieu { get; set; }
    }
}