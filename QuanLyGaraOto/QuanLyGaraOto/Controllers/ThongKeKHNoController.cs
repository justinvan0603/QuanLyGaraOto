using QuanLyGaraOto.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuanLyGaraOto.Controllers
{
    public class ThongKeKHNoController : Controller
    {
        // GET: ThongKeKHNo
        public ActionResult Index()
        {
            GARADBEntities context = new GARADBEntities();
            int UserId = int.Parse(Session["UserID"].ToString());
            NHANVIEN nv = context.NHANVIENs.Single(staff => staff.MA_NV == UserId);
            NHOMNGUOIDUNG groupuser = context.NHOMNGUOIDUNGs.Single(gu => gu.MA_NHOMNGUOIDUNG == nv.MA_NHOMNGUOIDUNG.Value);
            if (groupuser.CAPDO != 3 && groupuser.CAPDO != 2)
            {
                TempData["msg"] = @"<div id=""rowError"" class=""row""> <div class=""col-sm-10""> <div class=""alert alert-danger alert-dismissable fade in"" style=""padding-top: 5px; padding-bottom: 5px""> <a href=""#"" class=""close"" data-dismiss=""alert"" aria-label=""close"">&times;</a> Bạn không có quyền truy cập vào chức năng này! </div> </div> </div>";
                return RedirectToAction("Index", "Home");
            }
            return Redirect("~/Reports/ThongKeKH.aspx");
        }
    }
}