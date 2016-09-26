using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuanLyGaraOto.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult Error(string errorMessage)
        {
            ViewBag.ErrorMessage = "Đã có lỗi xảy ra";
            return View();
        }
    }
}