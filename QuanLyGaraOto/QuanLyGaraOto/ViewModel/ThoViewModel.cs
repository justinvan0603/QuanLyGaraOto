using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using QuanLyGaraOto.Models;

namespace QuanLyGaraOto.ViewModel
{
    public class ThoViewModel
    {
        public int SelectedValue { get; set; }
        public List<SelectListItem> listSearchOption = new List<SelectListItem>(){ new SelectListItem(){Text = "", Value="0"},
                                                                                    new SelectListItem(){Text = "Họ tên", Value="1"},
                                                                                    new SelectListItem(){Text = "Số điện thoại", Value="2"}};
        public IPagedList<THO> ListTho { get; set; }
    }
}