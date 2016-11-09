using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QuanLyGaraOto.Models;

namespace QuanLyGaraOto.ViewModel
{
    public class PhieuNhapHangViewModel
    {
         public PhieuNhapHangViewModel()
        {
            PhieuNhapHang = new PHIEU_NHAPHANG();
        }
         public PhieuNhapHangViewModel(PHIEU_NHAPHANG pnh, String maphieudh, string tennv, string tenncc)
        {
            PhieuNhapHang = new PHIEU_NHAPHANG();
            PhieuNhapHang = pnh;
            MaPhieuDatHang = maphieudh;
            TenNV = tennv;
            TenNCC = tenncc;
        }
        public PHIEU_NHAPHANG PhieuNhapHang { get; set; }
        public string TenNV { get; set; }
        public string TenNCC { get; set; }
        public string MaPhieuDatHang { get; set; }
        public List<PHUTUNG> ListPhuTung { get; set; }
        public List<NHACUNGCAP> ListNhaCungCap { get; set; }
        public List<NHOMNHACUNGCAP> ListNhomNCC { get; set; }
        public List<HIEUXE> ListHieuXe { get; set; }
        public List<CHITIET_PHIEUNH> ListChiTietPhieuNH { get; set; }
    }
}