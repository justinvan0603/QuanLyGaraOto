using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using QuanLyGaraOto.Models;
namespace QuanLyGaraOto.ViewModel
{
    public class PhieuTiepNhanViewModel
    {
        /// <summary>
        /// Danh sach phieu tiep nhan custom de hien thi len table
        /// </summary>
        public IPagedList<PhieuTiepNhanListViewModel> danhSachPhieuTiepNhan { get; set; }

        /// <summary>
        /// Chua thong tin phieu tiep nhan dang duoc nhap hoac can duoc cap nhat
        /// </summary>
        public NhapPhieuTiepNhanModel selectedVoucher;

        /// <summary>
        /// 
        /// </summary>
        public PHIEU_TIEPNHAN selectedPhieuTiepNhan { get; set; }

        /// <summary>
        /// Chua danh sach tuy chon de user thuan tien trong viec loc du lieu ma minh can xem
        /// </summary>
        public List<SelectListItem> filterOptions = new List<SelectListItem>() { new SelectListItem(){Text = "", Value="0"},
                                                                                    new SelectListItem(){Text = "Ngày lập", Value="1"},
                                                                                    new SelectListItem(){Text = "Ngày hẹn trả" , Value="2"}};
        /// <summary>
        /// Danh sach khach hang trong databaes
        /// </summary>
        public List<KHACHHANG> danhSachKhachHang { get; set; }

        /// <summary>
        /// Danh sach hieu xe trong databae
        /// </summary>
        public List<HIEUXE> danhSachHieuXe { get; set; }

        /// <summary>
        /// Danh sach xe trong database
        /// </summary>
        public List<XE> danhsachXe { get; set; }
    }
}