using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuanLyGaraOto.Controllers
{
    public class BaoCaoCongNoController : Controller
    {
        // GET: BaoCaoCongNo
        public ActionResult Index()
        {
            return Redirect("~/Reports/BaoCaoCongNo.aspx");
        }
    }
}