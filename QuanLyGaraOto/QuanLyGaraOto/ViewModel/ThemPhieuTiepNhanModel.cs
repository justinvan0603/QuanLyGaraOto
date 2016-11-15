using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace QuanLyGaraOto.ViewModel
{
    public class ThemPhieuTiepNhanModel
    {
        // ngay tiep nhan
        public DateTime? ngayTiepNhan { get; set; }
        // thong tin khach hang

        [Required(ErrorMessage = "Tên không được để trống!")]
        [StringLength(100, ErrorMessage = "Tên chỉ chứa tối đa 100 ký tự")]
        [RegularExpression("[a-zA-Z]", ErrorMessage = "Tên vừa nhập không hợp lệ!")]
        public string tenKhachHang { get; set; }


        [Required(ErrorMessage = "Số điện thoại không được để trống!")]
        [StringLength(20, ErrorMessage = "Số điện thoại chỉ chứa tối đa 20 ký tự")]
        [RegularExpression("[0-9]", ErrorMessage = "Số điện thoại vừa nhập không hợp lệ!")]
        public string dienThoai { get; set; }

        [Required(ErrorMessage = "Số điện thoại không được để trống!")]
        [StringLength(20, ErrorMessage = "Số điện thoại chỉ chứa tối đa 20 ký tự")]
        [RegularExpression("[0-9]", ErrorMessage = "Số điện thoại vừa nhập không hợp lệ!")]
        public string diaChi { get; set; }

        [Required(ErrorMessage = "Số CMND không được để trống!")]
        [StringLength(30, ErrorMessage = "Số CMND chỉ chứa tối đa 30 ký tự")]
        [RegularExpression("[a-zA-Z]", ErrorMessage = "Số CMND vừa nhập không hợp lệ!")]
        public string soCmnd { get; set; }


        // thong tin xe
        public string bienSoXe { get; set; }
        public string hieuXe { get; set; }
        public string soMay { get; set; }
        public string soKhung { get; set; }

        public int? soKm { get; set; }

        public string doiXe { get; set; }
        public string tinhTrang { get; set; }

        public bool? hinhThuc = false; // mac dinh la xe sua


    }
}