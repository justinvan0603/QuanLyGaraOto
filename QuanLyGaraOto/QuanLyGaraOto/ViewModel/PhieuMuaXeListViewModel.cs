using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuanLyGaraOto.ViewModel
{
    public class PhieuMuaXeListViewModel
    {
        public int? id { get; set; } // id phieu mua xe, se khong duoc hien thi tren list view nhung co ich cho nhung hoat dong khac trong viec tim kiem
        public string maPhieuMua { get; set; } // id String cua phieu mua xe
        public string bienSoXe { get; set; } // bien se xe mua
        public DateTime? ngayLapPhieu { get; set; } // format : @"MM\/dd\/yyyy HH:mm"
        public string nhanVien { get; set; } // nhan vien lap phieu
        public string khachHang { get; set; } // ten khach hang ban xe cho GARA

        public decimal? triGia { get; set; } // tri gia cua phieu mua
        public bool isPaid { get; set; }  // indicate that wheater this bill is paid completely , if so the delete button should not be shown, also the button create receipt !
    }
}