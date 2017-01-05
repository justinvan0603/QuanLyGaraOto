using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QuanLyGaraOto.ViewModel;
using QuanLyGaraOto.Models;
namespace QuanLyGaraOto.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            GARADBEntities context = new GARADBEntities();
            TongQuanViewModel vmTongQuan = new TongQuanViewModel();
            vmTongQuan.TongSoPhuTung = context.PHUTUNGs.Count();
            vmTongQuan.SoPTHet = context.PHUTUNGs.Count(pt => pt.SOLUONGTON == 0);
            vmTongQuan.TongSoKH = context.KHACHHANGs.Count();
            vmTongQuan.TongSoKHNo = context.KHACHHANGs.Count(kh => kh.SOTIENNO > 0);
            vmTongQuan.SoXeTiepNhan = context.XEs.Count(x => x.HINHTHUC.Value == false);
            vmTongQuan.SoXeBan = context.XEs.Count(x => x.HINHTHUC.Value == true);
            return View(vmTongQuan);
        }

        
    }
}