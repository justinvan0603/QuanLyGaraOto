using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QuanLyGaraOto.ViewModel;
namespace QuanLyGaraOto.Controllers
{
    public class BaoCaoThuChiController : Controller
    {
        // GET: BaoCaoThuChi
        public ActionResult Index()
        {

            return View();
        }
        public ActionResult XemBaoCao(BaoCaoThuChiViewModel viewmodel)
        {
            string QueryString = "?tungay=" + viewmodel.TuNgay.Date.ToString() + "&denngay=" + viewmodel.DenNgay.Date.ToString();
            string URL = "~/Reports/BaoCaoThuChi.aspx" + QueryString;
            return Redirect(URL);
        }
    }
}