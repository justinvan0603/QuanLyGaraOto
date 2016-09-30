using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QuanLyGaraOto.Models;
using PagedList;
namespace QuanLyGaraOto.ViewModel
{
    public class NhanVienViewModel
    {
        public int selectedValue { get; set; }
        public List<SelectListItem> listSearchOption = new List<SelectListItem>() { new SelectListItem(){Text = "", Value="0"},
                                                                                    new SelectListItem(){Text = "Họ tên", Value="1"},
                                                                                    new SelectListItem(){Text = "Username", Value="2"},
                                                                                    new SelectListItem(){Text = "SĐT", Value="3"}};
        public IPagedList<NhanVienTableData> ListNhanVien { get; set; }
    }
}