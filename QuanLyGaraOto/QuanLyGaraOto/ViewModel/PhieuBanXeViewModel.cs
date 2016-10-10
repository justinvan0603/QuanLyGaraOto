using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QuanLyGaraOto.Models;
using System.Web.Mvc;
using PagedList;

namespace QuanLyGaraOto.ViewModel
{
    /// <summary>
    /// 
    /// </summary>
    public class PhieuBanXeViewModel
    {
        /// <summary>
        /// List of all PHIEU_BANXEs from database
        /// </summary>
        public PHIEU_BANXE selectedPHIEUBANXE { get; set; } // hold the current bill the user choose 

        public List<SelectListItem> listSearchOptions = new List<SelectListItem>() { new SelectListItem(){Text = "", Value="0"},
                                                                                    new SelectListItem(){Text = "Mã phiếu", Value="1"},
                                                                                    new SelectListItem(){Text = "Ngày lập", Value="2"},
                                                                                    new SelectListItem(){Text = "Hạn chót thanh toán", Value="3"}};
        public IPagedList<PhieuBanXeDataTableModel> listOfPHIEUBANXEs { get; set; } // list nay se duoc su dung de fill data tren view

        public List<XE> listOfXes { get; set; } // danh sach xe de nguoi dung chon de thuc hien ban xe (NOTE : chi load nhung xe ban trong GARA)

        public List<KHACHHANG> listOfKhachHang { get; set; } // danh sach khach hang trong GARA

    }
}