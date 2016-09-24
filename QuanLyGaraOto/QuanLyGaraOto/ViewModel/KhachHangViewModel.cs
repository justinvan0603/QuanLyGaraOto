using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QuanLyGaraOto.Models;
using PagedList;
using System.Collections;
using System.Web.Mvc;
namespace QuanLyGaraOto.ViewModel
{
    public class KhachHangViewModel
    {
       // public string CurrentSearchString { get; set; }
        //public int PageCount { get; set; }
        //public int PageNumber { get; set; }
        //public string SortOrderBy { get; set; }
        public int selectedValue { get; set; }
        public List<SelectListItem> listSearchOption = new List<SelectListItem>() { new SelectListItem(){Text = "", Value="0"},
                                                                                    new SelectListItem(){Text = "Họ tên", Value="1"},
                                                                                    new SelectListItem(){Text = "CMND", Value="2"},
                                                                                    new SelectListItem(){Text = "Số ĐT", Value="3"}};
        public IPagedList<KHACHHANG> ListKhachHang { get; set; }
    }
}