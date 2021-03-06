﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using QuanLyGaraOto.Models;

namespace QuanLyGaraOto.ViewModel
{
    public class DSPhieuThuViewModel
    {
        public int SelectedValue { get; set; }
        public List<SelectListItem> listSearchOption = new List<SelectListItem>(){ new SelectListItem(){Text = "", Value="0"},
                                                                                    new SelectListItem(){Text = "Mã phiếu thu", Value="1"},
                                                                                    new SelectListItem(){Text = "Nội dung phiếu thu", Value="2"},
                                                                                    new SelectListItem(){Text = "Tên nhân viên lập", Value="3"}};
        public IPagedList<PhieuThuViewModel> ListPhieuThu { get; set; }
    }
}