using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QuanLyGaraOto.Models;

namespace QuanLyGaraOto.ViewModel
{
    public class PhieuDatHangViewModel
    {
        public PhieuDatHangViewModel()
        {
            PhieuDatHang = new PHIEU_DATHANG();
        }
        public PhieuDatHangViewModel(PHIEU_DATHANG pdh, string tennv, string tenncc)
        {
            PhieuDatHang = new PHIEU_DATHANG();
            PhieuDatHang = pdh;
            TenNV = tennv;
            TenNCC = tenncc;
        }
        public PHIEU_DATHANG PhieuDatHang { get; set; }
        public string TenNV { get; set; }
        public string TenNCC { get; set; }
        public List<PHUTUNG> ListPhuTung { get; set; }
        public List<NHACUNGCAP> ListNhaCungCap { get; set; }
        public List<NHOMNHACUNGCAP> ListNhomNCC { get; set; }
        public List<HIEUXE> ListHieuXe { get; set; }
        public List<CHITIET_PHIEUDATHANG> ListChiTietPhieuDH { get; set; }
    }
}