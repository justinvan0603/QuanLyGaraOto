using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuanLyGaraOto.ViewModel
{
    public class PhieuBanXeDataTableModel
    {

        public int id { get; set; } // auto generated id
        public string maPhieuBan { get; set; } // id cua phieu ban xe
        public string bienSoXe { get; set; } // bien se xe ban
        public DateTime? ngayLapPhieu { get; set; } // format : @"MM\/dd\/yyyy HH:mm"
        public int? maNhanVien { get; set; } // ma nhan vien lap phieu
        public decimal? giaTri { get; set; }
        public DateTime? hanChotThanhToan { get; set; } // format : @"MM\/dd\/yyyy HH:mm"
        public decimal? soTienConLai { get; set; }

    }
}