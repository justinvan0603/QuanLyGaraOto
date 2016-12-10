using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuanLyGaraOto.ViewModel
{
    public class NhanVienTableData
    {
        public int MaNV { get; set; }
        public string Username { get; set; }
        public string HoTen { get; set; }
        public string SDT { get; set; }
        public string DiaChi { get; set; }
        public int MaNhomNguoiDung { get; set; }
        public string NhomNguoiDung { get; set; }
        public bool GioiTinh { get; set; }
    }
}