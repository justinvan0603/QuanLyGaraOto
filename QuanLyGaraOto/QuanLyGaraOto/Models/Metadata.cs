﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace QuanLyGaraOto.Models
{

    public class KHACHHANGMetadata
    {
        // bi loi khi them phieu tiep nhan
       // public int MA_NHOMNGUOIDUNG { get; set; }
       // public int MA_NV {get; set;}
        [Required(ErrorMessage = "Tên không được để trống!")]
        [StringLength(100, ErrorMessage = "Tên chỉ chứa tối đa 100 ký tự")]
        [RegularExpression(@"^[\p{L}\p{N} ]*$", ErrorMessage = "Tên vừa nhập không hợp lệ!")]
        public string TEN_KH { get; set; }

        [Required(ErrorMessage = "Số CMND không được để trống!")]
        [StringLength(30, ErrorMessage = "Số CMND chỉ chứa tối đa 30 ký tự")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Số CMND vừa nhập không hợp lệ!")]
        public string CMND { get; set; }

        [Required(ErrorMessage = "Số điện thoại không được để trống!")]
        [StringLength(20, ErrorMessage = "Số điện thoại chỉ chứa tối đa 20 ký tự")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Số điện thoại vừa nhập không hợp lệ!")]
        public string SDT { get; set; }

        [Required(ErrorMessage = "Địa chỉ không được để trống!")]
        [StringLength(100, ErrorMessage = "Địa chỉ chỉ chứa tối đa 20 ký tự")]
        public string DIACHI { get; set; }
        public bool GIOITINH { get; set; }
    }
    public class BANGTHAMSOMetadata
    {
        public string TENTHAMSO { get; set; }
        public string NOIDUNG { get; set; }
        [Required(ErrorMessage = "Giá trị không được để trống!")]
        [StringLength(30, ErrorMessage = "Giá trị chỉ chứa tối đa 50 ký tự")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Số Giá trị vừa nhập không hợp lệ!")]
        public string GIATRI { get; set; }
    }
    public class NHANVIENMetadata
    {
        [Required(ErrorMessage = "Tên đăng nhập không được để trống!")]
        [StringLength(30, ErrorMessage = "Username chỉ tối đa 30 ký tự!")]
        [RegularExpression(@"^\p{L}+$", ErrorMessage = "Username không hợp lệ!")]
        public string USERNAME { get; set; }

        [Required(ErrorMessage = "Mật khẩu không được để trống!")]
        [StringLength(30, ErrorMessage = "Mật khẩu chỉ tối đa 30 ký tự!")]

        public string PASSWORD { get; set; }

        [Required(ErrorMessage = "Họ tên không được để trống!")]
        [StringLength(20, ErrorMessage = "Họ tên chỉ tối đa 20 ký tự!")]
        [RegularExpression(@"^[\p{L}\p{N} ]*$", ErrorMessage = "Họ tên không hợp lệ!")]
        public string HOTEN { get; set; }

        [Required(ErrorMessage = "Số điện thoại không được để trống!")]
        [StringLength(100, ErrorMessage = "Số điện thoại chỉ tối đa 100 ký tự!")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Số điện thoại không hợp lệ!")]
        public string SDT { get; set; }

        [Required(ErrorMessage = "Địa chỉ không được để trống!")]
        [StringLength(100, ErrorMessage = "Địa chỉ chỉ tối đa 100 ký tự!")]
        //[RegularExpression(@"[^a-zA-Z0-9\s]", ErrorMessage = "Địa chỉ không hợp lệ!")]
        public string DIACHI { get; set; }
        public bool GIOITINH { get; set; }
    }

    public class THOMetadata
    {
        [Required(ErrorMessage = "Tên không được để trống!")]
        [StringLength(100, ErrorMessage = "Tên chỉ chứa tối đa 100 ký tự")]
        [RegularExpression(@"^[\p{L}\p{N} ]*$", ErrorMessage = "Tên vừa nhập không hợp lệ!")]
        public string TENTHO { get; set; }

        [Required(ErrorMessage = "Số điện thoại không được để trống!")]
        [StringLength(20, ErrorMessage = "Số điện thoại chỉ chứa tối đa 20 ký tự")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Số điện thoại vừa nhập không hợp lệ!")]
        public string SDT { get; set; }

        [Required(ErrorMessage = "Địa chỉ không được để trống!")]
        [StringLength(100, ErrorMessage = "Địa chỉ chỉ chứa tối đa 100 ký tự")]
        public string DIACHI { get; set; }
    }
    public class PHUTUNGMetadata
    {
        [Required(ErrorMessage = "Mã phụ tùng không được để trống!")]
        [StringLength(20, ErrorMessage = "Mã phụ tùng chỉ chứa tối đa 20 ký tự")]
        public string MA_PHUTUNG { get; set; }
        public string TEN_PHUTUNG { get; set; }
    }
    public class NHACUNGCAPMetadata
    {

    }
    public class PHIEUTHUMetadata
    {
        
    }

    public class PHIEUNHAPHANGMetadata
    {
        
    }
    public class PHIEUDATHANGMetadata
    {

    }
}