using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using QuanLyGaraOto.Models;

namespace QuanLyGaraOto.ViewModel
{
    public class XeViewModel
    {
        // danh sach xe
        public IPagedList<XE> danhSachXe { get; set; }


        // danh sach loc du lieu
        public List<SelectListItem> listSearchOption = new List<SelectListItem>(){ new SelectListItem(){Text = "", Value="0"},
                                                                                    new SelectListItem(){Text = "Biển số xe", Value="1"},
                                                                                    new SelectListItem(){Text = "Hiệu xe", Value="2"},
                                                                                    new SelectListItem(){Text = "Xe bán", Value="3"}};
        public XE selectedXe { get; set; }

        // danh sach xe
        public List<HIEUXE> danhSachHieuXe { get; set; }

        // danh sach khach hang
        public List<KHACHHANG> danhSachKhachHang { get; set; }
    }
}