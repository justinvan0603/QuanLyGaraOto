using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using QuanLyGaraOto.Models;

namespace QuanLyGaraOto.ViewModel
{
    public class PhieuDatHangModel
    {
        public int SelectedValue { get; set; }
        public List<SelectListItem> listSearchOption = new List<SelectListItem>(){ new SelectListItem(){Text = "", Value="0"},
                                                                                    new SelectListItem(){Text = "Mã phiếu đặt hàng", Value="1"},
                                                                                    new SelectListItem(){Text = "Ngày đặt", Value="2"},
                                                                                    new SelectListItem(){Text = "Ngày giao", Value="3"}};
        public IPagedList<PHIEU_DATHANG> ListPhieuDathang { get; set; }
    }
}