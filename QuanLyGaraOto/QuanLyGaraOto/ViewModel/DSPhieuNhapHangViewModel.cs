using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace QuanLyGaraOto.ViewModel
{
    public class DSPhieuNhapHangViewModel
    {
        public int SelectedValue { get; set; }
        public List<SelectListItem> listSearchOption = new List<SelectListItem>(){ new SelectListItem(){Text = "", Value="0"},
                                                                                    new SelectListItem(){Text = "Mã phiếu nhập hàng", Value="1"},                                                                                   
                                                                                    new SelectListItem(){Text = "Mã phiếu đặt hàng", Value="2"},
                                                                                    new SelectListItem(){Text = "Ngày lập", Value="3"},
                                                                                    new SelectListItem(){Text = "Tên nhân viên", Value="4"},
                                                                                    new SelectListItem(){Text = "Tên nhà cung câp", Value="5"}};
        public IPagedList<PhieuNhapHangViewModel> ListPhieuNhapHang { get; set; }
    }
}