using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QuanLyGaraOto.Models;
namespace QuanLyGaraOto.ViewModel
{
    public class SuaNhanVienViewModel
    {
        public NHANVIEN NhanVien { get; set; }
        public List<NHOMNGUOIDUNG> ListNhomNguoiDung { get; set; }
    }
}