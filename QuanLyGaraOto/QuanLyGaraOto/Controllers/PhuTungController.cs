using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using PagedList;
using QuanLyGaraOto.Models;
using QuanLyGaraOto.ViewModel;

namespace QuanLyGaraOto.Controllers
{
    public class PhuTungController : Controller
    {
        // GET: PhuTung

        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? searchOption,
            int? page)
        {
            ViewBag.CurrentSearchOption = searchOption;
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParam = (sortOrder == "name_asc" ? "name_desc" : "name_asc");   //Ban đầu là null -> ASC
            ViewBag.IDCodeSortParam = (sortOrder == "id_asc" ? "id_desc" : "id_asc");
            GARADBEntities context = new GARADBEntities();
            List<PHUTUNG> listPhutungs = context.PHUTUNGs.ToList();
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
                        case 1: { listPhutungs = listPhutungs.Where(c => c.TEN_PHUTUNG.Contains(searchString)).ToList(); break; }
                        case 2: { listPhutungs = listPhutungs.Where(c => c.MA_HIEUXE.Contains(searchString)).ToList(); break; }
                        case 3: { listPhutungs = listPhutungs.Where(c => c.MA_PHUTUNG.Contains(searchString)).ToList(); break; }
                        default: { break; }
                    }
                }

            }
            switch (sortOrder)
            {
                case "name_asc": { listPhutungs = listPhutungs.OrderBy(c => c.TEN_PHUTUNG).ToList(); break; }
                case "name_desc": { listPhutungs = listPhutungs.OrderByDescending(c => c.TEN_PHUTUNG).ToList(); break; }
                case "id_asc": { listPhutungs = listPhutungs.OrderBy(c => c.MA_PHUTUNG).ToList(); break; }
                case "id_desc": { listPhutungs = listPhutungs.OrderByDescending(c => c.MA_PHUTUNG).ToList(); break; }
                default: { break; }
            }
            int pageSize = 5;
            int pageNumber = (page ?? 1); //nếu page có giá trị thì = page, ko thì = 1
            PhuTungViewModel phutungViewModel = new PhuTungViewModel();
            phutungViewModel.ListPhuTung = listPhutungs.ToPagedList(pageNumber, pageSize);
            return View(phutungViewModel);
        }

        [HttpGet]
        public ActionResult ThemMoi()
        {
            GARADBEntities garadbEntities = new GARADBEntities();
            List<HIEUXE> listHieuxes = new List<HIEUXE>();
            listHieuxes = garadbEntities.HIEUXEs.ToList();
            return View(listHieuxes);
        }

        [HttpPost]
        public ActionResult ThemMoi(PHUTUNG phutung)
        {
            try
            {
                GARADBEntities context = new GARADBEntities();
                phutung.SOLUONGTON = 0;
                phutung.DONGIAXUAT = 0;
                context.PHUTUNGs.Add(phutung);
                context.SaveChanges();
                TempData["msg"] = @"<div id=""rowSuccess"" class=""row""> <div class=""col-sm-10""> <div class=""alert alert-success alert-dismissable fade in"" style=""padding-top: 5px; padding-bottom: 5px""> <a href=""#"" class=""close"" data-dismiss=""alert"" aria-label=""close"">&times;</a> Thêm mới thành công! </div> </div> </div>";
            }
            catch (Exception)
            {
                TempData["msg"] = @"<div id=""rowError"" class=""row""> <div class=""col-sm-10""> <div class=""alert alert-danger alert-dismissable fade in"" style=""padding-top: 5px; padding-bottom: 5px""> <a href=""#"" class=""close"" data-dismiss=""alert"" aria-label=""close"">&times;</a> Đã có lỗi xảy ra! Vui lòng thử lại! </div> </div> </div>";
            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult CapNhat(int id = 5)
        {
            GARADBEntities context = new GARADBEntities();
            SuaPhuTungViewModel suaPhuTungViewModel = new SuaPhuTungViewModel();
            PHUTUNG phutung = context.PHUTUNGs.Single(c => c.ID == id);
            suaPhuTungViewModel.PhuTung = phutung;
            List<HIEUXE> listHieuxes = new List<HIEUXE>();
            listHieuxes = context.HIEUXEs.ToList();
            suaPhuTungViewModel.ListHieuXe = listHieuxes;
            return View(suaPhuTungViewModel);
        }
        [HttpPost]
        public ActionResult CapNhat(PHUTUNG phutung)
        {
            try
            {
                GARADBEntities context = new GARADBEntities();
                var target = context.PHUTUNGs.Find(phutung.ID);
                target.MA_PHUTUNG = phutung.MA_PHUTUNG;
                target.TEN_PHUTUNG = phutung.TEN_PHUTUNG;
                target.MA_HIEUXE = phutung.MA_HIEUXE;
                context.SaveChanges();
                TempData["msg"] = @"<div id=""rowSuccess"" class=""row""> <div class=""col-sm-10""> <div class=""alert alert-success alert-dismissable fade in"" style=""padding-top: 5px; padding-bottom: 5px""> <a href=""#"" class=""close"" data-dismiss=""alert"" aria-label=""close"">&times;</a> Cập nhật thành công! </div> </div> </div>";
            }
            catch (Exception)
            {
                TempData["msg"] = @"<div id=""rowError"" class=""row""> <div class=""col-sm-10""> <div class=""alert alert-danger alert-dismissable fade in"" style=""padding-top: 5px; padding-bottom: 5px""> <a href=""#"" class=""close"" data-dismiss=""alert"" aria-label=""close"">&times;</a> Đã có lỗi xảy ra! Vui lòng thử lại! </div> </div> </div>";
            }

            return RedirectToAction("Index");
        }

        [ValidateInput(false)]
        [HttpPost]
        public JsonResult Xoa(int id)
        {
            try
            {
                GARADBEntities context = new GARADBEntities();
                var target = context.PHUTUNGs.Find(id);
                context.PHUTUNGs.Remove(target);
                context.SaveChanges();
                TempData["msg"] = @"<div id=""rowSuccess"" class=""row""> <div class=""col-sm-10""> <div class=""alert alert-success alert-dismissable fade in"" style=""padding-top: 5px; padding-bottom: 5px""> <a href=""#"" class=""close"" data-dismiss=""alert"" aria-label=""close"">&times;</a> Xoá thành công! </div> </div> </div>";
                return Json(new { value = "1", message = "Xóa thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                TempData["msg"] = @"<div id=""rowError"" class=""row""> <div class=""col-sm-10""> <div class=""alert alert-danger alert-dismissable fade in"" style=""padding-top: 5px; padding-bottom: 5px""> <a href=""#"" class=""close"" data-dismiss=""alert"" aria-label=""close"">&times;</a> Không thể xóa do loại phụ tùng này đã từng được sử dụng! </div> </div> </div>";
                return Json(new { value = "-1", message = "Không thể xóa do loại phụ tùng này đã từng được sử dụng!" }, JsonRequestBehavior.AllowGet);
            }

        }
    }
}