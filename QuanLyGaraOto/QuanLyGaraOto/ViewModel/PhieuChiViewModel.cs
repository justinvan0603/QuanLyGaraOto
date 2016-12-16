using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;
using QuanLyGaraOto.Models;
using System.Web.Mvc;


namespace QuanLyGaraOto.ViewModel
{
    /// <summary>
    /// View model cho module phieu chi
    /// </summary>
    public class PhieuChiViewModel
    {

        // danh sach phieu ban hang trong database
        public IPagedList<PHIEU_CHI> danhSachPhieuChi { get; set; }

      
        // phieu chi hien tai ma nguoi dung chon
        public PHIEU_CHI selectedItem { get; set; }

        // danh sach options de loc du lieu
        public List<SelectListItem> listSearchOption = new List<SelectListItem>() { new SelectListItem(){Text = "", Value="0"},
                                                                                    new SelectListItem(){Text = "", Value="1"},
                                                                                    new SelectListItem(){Text = "Ngày lập", Value="2"}};


        /**
         * Nhung gia tri lien quan tu phia phieu mua xe hoac phieu nhap hang
         * */
        public int? idPhieuMuaXe { get; set; }
        public int? idPhieuNhapHang { get; set; }

        public string noiDungChi { get; set; }

        public decimal? triGia { get; set; }


    }
}