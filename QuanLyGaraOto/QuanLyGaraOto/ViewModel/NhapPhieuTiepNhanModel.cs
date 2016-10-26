using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QuanLyGaraOto.Models;

namespace QuanLyGaraOto.ViewModel
{
    /// <summary>
    /// Chua thong tin lien quan toi viec nhap thong tin cua mot phieu tiep nhan
    /// </summary>
    public class NhapPhieuTiepNhanModel
    {
        /* Thong tin chung */
        public int? maPhieu { get; set; }
        public int? maKhachHang { get; set; }
        public int? maNhanVien { get; set; }
        public DateTime ngayLap { get; set; }
        public DateTime ngayHenTra { get; set; }
        public int? soCho { get; set; }
        public string tinhTrang { get; set; }

        /* Thong tin xe */
        public string bienSoXe { get; set; }
        public string hieuXe { get; set; }
        public string tinhTrangXe { get; set; }
        public string soMay { get; set; }
        public string soKhung { get; set; }
        public string doiXe { get; set; }
        public int? soKm { get; set; }

    }
}