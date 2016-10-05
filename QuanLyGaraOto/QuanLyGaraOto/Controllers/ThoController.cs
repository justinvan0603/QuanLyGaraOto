using System;
using System.Linq;
using System.Web.Mvc;
using QuanLyGaraOto.Models;

namespace QuanLyGaraOto.Controllers
{
    public class ThoController : Controller
    {
        // GET: Tho

        [HttpGet]
        public ActionResult ThemMoi()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ThemMoi(THO tho)
        {
            try
            {
                GARADBEntities context = new GARADBEntities();
                context.THOes.Add(tho);
                context.SaveChanges();
                TempData["msg"] = "<script>alert('Đã thêm thành công');</script>";
            }
            catch (Exception)
            {
                TempData["msg"] = "<script>alert('Đã xảy ra lỗi. Vui lòng thử lại!');</script>";
            }
            return RedirectToAction("ThemMoi");
        }

        [HttpGet]
        public ActionResult CapNhat(int id = 1)
        {
            GARADBEntities context = new GARADBEntities();
            THO tho = context.THOes.Single(c => c.MA_THO == id);
            return View(tho);
        }

        [HttpPost]
        public ActionResult CapNhat(THO tho)
        {
            try
            {
                GARADBEntities context = new GARADBEntities();
                var target = context.THOes.Find(tho.MA_THO);
                target.TENTHO = tho.TENTHO;
                target.SDT = tho.SDT;
                target.DIACHI = tho.DIACHI;
                context.SaveChanges();
                TempData["msg"] = "<script>alert('Đã cập nhật thành công');</script>";
            }
            catch (Exception)
            {
                TempData["msg"] = "<script>alert('Đã xảy ra lỗi. Vui lòng thử lại!');</script>";
            }
            return View();
        }
    }
}