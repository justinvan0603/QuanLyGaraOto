using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;
using QuanLyGaraOto.Models;
namespace QuanLyGaraOto.ViewModel
{
    public class ThamSoViewModel
    {
        public IPagedList<BANGTHAMSO> ListThamSo { get; set; }
    }
}