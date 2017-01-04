using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuanLyGaraOto.ViewModel
{
    /// <summary>
    /// Chua thong tin cua mot phieu mua xe tren view cua client
    /// </summary>
    public class PhieuNhapMuaXeModel
    {
        /* Thong tin chung */
        public int? id { get; set; }
        public string maPhieuMuaXe { get; set; }
        public DateTime? ngayLapPhieu { get; set; }
        public decimal? tongGiaTri { get; set; }
        public int? maKhachHang { get; set; }
        public int? maNhanVien { get; set; }
        public String TenNV { get; set; }

        /* Thong tin xe*/
        public string bienSoXe { get; set; }
        public string hieuXe { get; set; }
        public string tinhTrang { get; set; }
        public string soMay { get; set; }
        public string soKhung { get; set; }
        public string doiXe { get; set; }
        public int? soKm { get; set; }
        public bool isPaid { get; set; }

    }
}