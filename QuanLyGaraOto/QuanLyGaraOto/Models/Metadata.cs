using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace QuanLyGaraOto.Models
{

        public class KHACHHANGMetadata
        {
            [Required(ErrorMessage="Tên không được để trống!")]
            [StringLength(100,ErrorMessage="Tên chỉ chứa tối đa 100 ký tự")]
            [RegularExpression("[a-zA-Z]",ErrorMessage="Tên vừa nhập không hợp lệ!")]
            public string TEN_KH { get; set; }
            [Required(ErrorMessage = "Số CMND không được để trống!")]
            [StringLength(30, ErrorMessage = "Số CMND chỉ chứa tối đa 30 ký tự")]
            [RegularExpression("[a-zA-Z]", ErrorMessage = "Số CMND vừa nhập không hợp lệ!")]
            public string CMND { get; set; }
            [Required(ErrorMessage = "Số điện thoại không được để trống!")]
            [StringLength(20, ErrorMessage = "Số điện thoại chỉ chứa tối đa 20 ký tự")]
            [RegularExpression("[0-9]", ErrorMessage = "Số điện thoại vừa nhập không hợp lệ!")]
            public string SDT { get; set; }
            [Required(ErrorMessage = "Địa chỉ không được để trống!")]
            [StringLength(100, ErrorMessage = "Địa chỉ chỉ chứa tối đa 20 ký tự")]
            public string DIACHI { get; set; }
        }
    
}