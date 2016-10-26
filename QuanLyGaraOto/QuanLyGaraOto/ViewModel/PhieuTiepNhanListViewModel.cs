using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuanLyGaraOto.ViewModel
{
    public class PhieuTiepNhanListViewModel
    {
        public int? maPhieuTiepNhan { get; set; }
        public int? maKhachHang { get; set; }
        public string tenKhachHang { get; set; }

        public int? maNhanVien { get; set; }
        public string tenNhanVien { get; set; }
        public DateTime? ngayLap { get; set; }
        public string bienSoXe { get; set; }
        public int? maSoCho { get; set; }
        public string tinhTrang { get; set; }
        public DateTime? ngayTra { get; set; }
    }
}