using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using QuanLyGaraOto.Models;

namespace QuanLyGaraOto.ViewModel
{
    public class NhaCungCapViewModel
    {
        public int SelectedValue { get; set; }
        public List<SelectListItem> listSearchOption = new List<SelectListItem>(){ new SelectListItem(){Text = "", Value="0"},
                                                                                    new SelectListItem(){Text = "Tên nhà cung cấp", Value="1"},
                                                                                    new SelectListItem(){Text = "Số điện thoại", Value="2"},
                                                                                    new SelectListItem(){Text = "Tên nhóm cung cấp", Value="3"}};
        public IPagedList<NHACUNGCAP> ListNhaCungCap { get; set; }
    }
}