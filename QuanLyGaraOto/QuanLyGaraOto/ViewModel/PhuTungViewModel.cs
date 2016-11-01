using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using QuanLyGaraOto.Models;

namespace QuanLyGaraOto.ViewModel
{
    public class PhuTungViewModel
    {
        public int SelectedValue { get; set; }
        public List<SelectListItem> listSearchOption = new List<SelectListItem>(){ new SelectListItem(){Text = "", Value="0"},
                                                                                    new SelectListItem(){Text = "Mã phụ tùng", Value="3"},
                                                                                    new SelectListItem(){Text = "Tên phụ tùng", Value="1"},
                                                                                    new SelectListItem(){Text = "Hiệu xe", Value="2"}};
        public IPagedList<PHUTUNG> ListPhuTung { get; set; }
    }
}