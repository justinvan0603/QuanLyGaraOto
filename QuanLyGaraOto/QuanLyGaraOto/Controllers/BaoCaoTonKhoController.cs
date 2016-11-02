using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuanLyGaraOto.Controllers
{
    public class BaoCaoTonKhoController : Controller
    {
        // GET: BaoCaoTonKho
        public ActionResult Index()
        {
            return Redirect("~/Reports/BaoCaoTonKho.aspx");
        }
    }
}