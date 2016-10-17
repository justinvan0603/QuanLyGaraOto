using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QuanLyGaraOto.Models;
using System.Web.Mvc;
using PagedList;

namespace QuanLyGaraOto.ViewModel
{
    public class PhieuMuaXeViewModel
    {
        /// <summary>
        /// Danh sach phieu mua trong database
        /// </summary>
        public IPagedList<PhieuMuaXeListViewModel> danhSachPhieuMuaXe { get; set; }
        /// <summary>
        /// Phieu mua duoc chon hien tai, dung de fill cac truong view trong man hinh them moi hoac sua doi
        /// </summary>
        public PHIEU_MUAXE phieuMuaxe { get; set; }

        public List<HIEUXE> danhSachHieuXe { get; set; }

        /// <summary>
        /// Chua danh sach tuy chon de user thuan tien trong viec loc du lieu ma minh can xem
        /// </summary>
        public List<SelectListItem> filterOptions = new List<SelectListItem>() { new SelectListItem(){Text = "", Value="0"},
                                                                                    new SelectListItem(){Text = "Ngày lập", Value="1"},
                                                                                    new SelectListItem(){Text = "Mã khách hàng" , Value="2"}};
        /// <summary>
        /// Danh sach khach hang tu database
        /// </summary>
        public List<KHACHHANG> danhSachKhachHang { get; set; }


    }
}