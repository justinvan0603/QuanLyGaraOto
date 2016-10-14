using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using QuanLyGaraOto.Models;
using QuanLyGaraOto.ViewModel;

namespace QuanLyGaraOto.Controllers
{
    public class NhaCungCapController : Controller
    {
        // GET: NhaCungCap
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? searchOption,
            int? page)
        {
            ViewBag.CurrentSearchOption = searchOption;
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParam = (sortOrder == "name_asc" ? "name_desc" : "name_asc");   //Ban đầu là null -> ASC
            ViewBag.TypeSortParam = (sortOrder == "type_asc" ? "type_desc" : "type_asc");   //Ban đầu là null -> ASC
            GARADBEntities context = new GARADBEntities();
            List<NHACUNGCAP> listNCC = context.NHACUNGCAPs.ToList();
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
                        case 1: { listNCC = listNCC.Where(c => c.TenNCC.Contains(searchString)).ToList(); break; }
                        case 2: { listNCC = listNCC.Where(c => c.SDT.Contains(searchString)).ToList(); break; }
                        case 3: { listNCC = listNCC.Where(c => c.NhomNCC.Contains(searchString)).ToList(); break; }
                        default: { break; }
                    }
                }

            }
            switch (sortOrder)
            {
                case "name_asc": { listNCC = listNCC.OrderBy(c => c.TenNCC).ToList(); break; }
                case "name_desc": { listNCC = listNCC.OrderByDescending(c => c.TenNCC).ToList(); break; }
                case "type_asc": { listNCC = listNCC.OrderBy(c => c.NhomNCC).ToList(); break; }
                case "type_desc": { listNCC = listNCC.OrderByDescending(c => c.NhomNCC).ToList(); break; }
                default: { break; }
            }
            int pageSize = 5;
            int pageNumber = (page ?? 1); //nếu page có giá trị thì = page, ko thì = 1
            NhaCungCapViewModel nhaCungCapViewModel = new NhaCungCapViewModel();
            nhaCungCapViewModel.ListNhaCungCap = listNCC.ToPagedList(pageNumber, pageSize);
            return View(nhaCungCapViewModel);
        }

        [HttpGet]
        public ActionResult ThemMoi()
        {
            GARADBEntities garadbEntities = new GARADBEntities();
            List<NHOMNHACUNGCAP> listNCC = new List<NHOMNHACUNGCAP>();
            listNCC = garadbEntities.NHOMNHACUNGCAPs.ToList();
            return View(listNCC);
        }

        [HttpPost]
        public ActionResult ThemMoi(NHACUNGCAP nhacungcap)
        {
            try
            {
                GARADBEntities context = new GARADBEntities();
                context.NHACUNGCAPs.Add(nhacungcap);
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
            SuaNhaCungCapViewModel suaNhaCungCapViewModel = new SuaNhaCungCapViewModel();
            NHACUNGCAP nhacungcap = context.NHACUNGCAPs.Single(c => c.MaNCC == id);
            suaNhaCungCapViewModel.NhaCungCap = nhacungcap;
            List<NHOMNHACUNGCAP> listNhomnhacungcaps = new List<NHOMNHACUNGCAP>();
            listNhomnhacungcaps = context.NHOMNHACUNGCAPs.ToList();
            suaNhaCungCapViewModel.ListNhomNhaCungCap = listNhomnhacungcaps;
            return View(suaNhaCungCapViewModel);
        }
        [HttpPost]
        public ActionResult CapNhat(NHACUNGCAP nhacungcap)
        {
            try
            {
                GARADBEntities context = new GARADBEntities();
                var target = context.NHACUNGCAPs.Find(nhacungcap.MaNCC);
                target.TenNCC = nhacungcap.TenNCC;
                target.DiaChi = nhacungcap.DiaChi;
                target.SDT = nhacungcap.SDT;
                target.NhomNCC = nhacungcap.NhomNCC;
                context.SaveChanges();
                TempData["msg"] = "<script>alert('Đã cập nhật thành công');</script>";
            }
            catch (Exception)
            {
                TempData["msg"] = "<script>alert('Đã xảy ra lỗi. Vui lòng thử lại!');</script>";
            }

            return CapNhat(nhacungcap.MaNCC);
        }

        [ValidateInput(false)]
        [HttpPost]
        public JsonResult Xoa(int id)
        {
            try
            {
                GARADBEntities context = new GARADBEntities();
                var target = context.NHACUNGCAPs.Find(id);
                context.NHACUNGCAPs.Remove(target);
                context.SaveChanges();
                return Json(new { value = "1", message = "Xóa thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { value = "-1", message = "Không thể xóa do nhà cung cấp này đã từng thực hiện giao dịch với cửa hàng!" }, JsonRequestBehavior.AllowGet);
            }

        }
    }
}