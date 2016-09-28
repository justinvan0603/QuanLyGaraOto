using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;
using System.Web.Mvc;
namespace QuanLyGaraOto.ViewModel
{
    public class DSPhieuDVViewModel
    {
        public IPagedList<DSPhieuDVTableData> ListData { get; set; }
        public int SelectedValue { get; set; }
        public List<SelectListItem> listSearchOption = new List<SelectListItem>() { new SelectListItem(){Text = "", Value="0"},
                                                                                    new SelectListItem(){Text = "Mã số phiếu", Value="1"}};
       
    }
}