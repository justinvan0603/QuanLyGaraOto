using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QuanLyGaraOto.Models;

namespace QuanLyGaraOto.ViewModel
{
    public class LapPhieuThuViewModel
    {
        public LapPhieuThuViewModel()
        {
            PhieuThuTien = new PHIEU_THUTIEN();
        }
        public PHIEU_THUTIEN PhieuThuTien { get; set; }
        public decimal? TongTien { get; set; }
        public decimal? ConNo { get; set; }
        public string TenNV { get; set; }
        public int SoTienThuToiThieu { get; set; }
    }
}