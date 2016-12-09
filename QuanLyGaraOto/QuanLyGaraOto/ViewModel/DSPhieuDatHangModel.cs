using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using QuanLyGaraOto.Models;

namespace QuanLyGaraOto.ViewModel
{
    public class DSPhieuDatHangModel
    {
        public int SelectedValue { get; set; }
        public List<SelectListItem> listSearchOption = new List<SelectListItem>(){ new SelectListItem(){Text = "", Value="0"},
                                                                                    new SelectListItem(){Text = "Mã phiếu đặt hàng", Value="1"},
                                                                                    new SelectListItem(){Text = "Ngày đặt", Value="2"},
                                                                                    new SelectListItem(){Text = "Ngày giao", Value="3"},
                                                                                    new SelectListItem(){Text = "Tên nhân viên", Value="4"},
                                                                                    new SelectListItem(){Text = "Tên nhà cung cấp", Value="5"}};
        public IPagedList<PhieuDatHangViewModel> ListPhieuDathang { get; set; }
    }
}