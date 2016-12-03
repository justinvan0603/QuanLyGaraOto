using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QuanLyGaraOto.Models;

namespace QuanLyGaraOto.ViewModel
{
    public class PhieuThuViewModel
    {
        public PhieuThuViewModel()
        {
            PhieuThu = new PHIEU_THUTIEN();
        }
        public PhieuThuViewModel(PHIEU_THUTIEN ptt, string tennv)
        {
            PhieuThu = new PHIEU_THUTIEN();
            PhieuThu = ptt;
            TenNV = tennv;
        }
        public PHIEU_THUTIEN PhieuThu { get; set; }
        public string TenNV { get; set; }
        public decimal? TongTien { get; set; }
        public decimal? ConNo { get; set; }
    }
}