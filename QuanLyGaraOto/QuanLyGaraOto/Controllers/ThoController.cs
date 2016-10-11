using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using PagedList;
using QuanLyGaraOto.Models;
using QuanLyGaraOto.ViewModel;

namespace QuanLyGaraOto.Controllers
{
    public class ThoController : Controller
    {
        // GET: Tho
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? searchOption,
            int? page)
        {
            ViewBag.CurrentSearchOption = searchOption;
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParam = (sortOrder == "name_asc" ? "name_desc" : "name_asc");   //Ban đầu là null -> ASC
            GARADBEntities context = new GARADBEntities();
            List<THO> listThos = context.THOes.ToList();
            if (searchString != null)   //Nếu là search, cho hiển thị trang 1. Nếu ko thì hiện lại searchString lần trước (null)
            {
                page = 1;
            }          
            else
            {
                searchString = currentFilter;
            }
            ViewBag.CurrentFilter = searchString;   
            if (!String.IsNullOrEmpty(searchString))
            {
                if (searchOption != null)
                {
                    switch (searchOption)
                    {
                        case 0: { break; }
                        case 1: { listThos = listThos.Where(c => c.TENTHO.Contains(searchString)).ToList(); break; }
                        case 2: { listThos = listThos.Where(c => c.SDT.Contains(searchString)).ToList(); break; }
                        default: { break; }
                    }
                }

            }
            switch (sortOrder)
            {
                case "name_asc": { listThos = listThos.OrderBy(c => c.TENTHO).ToList(); break; }
                case "name_desc": { listThos = listThos.OrderByDescending(c => c.TENTHO).ToList(); break; }
                default: { break; }
            }
            int pageSize = 5;
            int pageNumber = (page ?? 1); //nếu page có giá trị thì = page, ko thì = 1
            ThoViewModel thoViewModel = new ThoViewModel();
            thoViewModel.ListTho = listThos.ToPagedList(pageNumber, pageSize);
            return View(thoViewModel);
        }


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
        public ActionResult CapNhat(int id)
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

        [ValidateInput(false)]
        [HttpPost]
        public JsonResult Xoa(int id)
        {
            try
            {
                GARADBEntities context = new GARADBEntities();
                var target = context.THOes.Find(id);
                context.THOes.Remove(target);
                context.SaveChanges();
                return Json(new { value = "1", message = "Xóa thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { value = "-1", message = "Không thể xóa do thợ này đã đảm nhận ít nhất một phiếu tiếp nhận!" }, JsonRequestBehavior.AllowGet);
            }

        }
    }
}