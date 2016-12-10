using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QuanLyGaraOto.Models;
using PagedList;
using QuanLyGaraOto.ViewModel;
using QuanLyGaraOto.Models;
namespace QuanLyGaraOto.Controllers
{
 
    public class KhachHangController : Controller
    {
        // GET: KhachHang
        public ActionResult Index(string sortOrder, string currentFilter, string searchString,int? searchOption, int? page)
        {
            ViewBag.CurrentSearchOption = searchOption;
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParam = sortOrder == "name_asc"? "name_desc" : "name_asc";
            ViewBag.OweSortParam = sortOrder == "owe_asc" ? "owe_desc" : "owe_asc";
            GARADBEntities context = new GARADBEntities();
            List<KHACHHANG> listClient = context.KHACHHANGs.ToList();
            if(searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewBag.CurrentFilter = searchString;
            if(!String.IsNullOrEmpty(searchString))
            {
                if (searchOption != null)
                {
                    switch (searchOption)
                    {
                        case 0: { break; }
                        case 1: { listClient = listClient.Where(c => c.TEN_KH.Contains(searchString)).ToList(); break; }
                        case 2: { listClient = listClient.Where(c => c.CMND.Contains(searchString)).ToList(); break; }
                        case 3: { listClient = listClient.Where(c => c.SDT.Contains(searchString)).ToList(); break; }
                        default: { break; }
                    }
                }
                
            }

            switch(sortOrder)
            {
                case "name_asc": { listClient = listClient.OrderBy(c => c.TEN_KH).ToList(); break; }
                case "name_desc": { listClient = listClient.OrderByDescending(c => c.TEN_KH).ToList(); break; }
                case "owe_asc": { listClient = listClient.OrderBy(c => c.SOTIENNO).ToList(); break; }
                case "owe_desc": { listClient = listClient.OrderByDescending(c => c.SOTIENNO).ToList(); break; }
                default: { break; }
            }
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            KhachHangViewModel KHViewModel = new KhachHangViewModel();
            KHViewModel.ListKhachHang = listClient.ToPagedList(pageNumber,pageSize);

            return View(KHViewModel);
        }
       

        [HttpGet]
        public ActionResult ThemMoi()
        {
           
            return View();
        }
        
        public ActionResult ThemMoi(KHACHHANG client)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    GARADBEntities context = new Models.GARADBEntities();
                    context.KHACHHANGs.Add(client);
                    context.SaveChanges();
                    TempData["msg"] = "<script>alert('Đã thêm thành công');</script>";
                }
                else
                {
                    
                    //client.TEN_KH=ModelState["TEN_KH"].Value.AttemptedValue;
                    return View(client);
                }
            }
            catch(Exception)
            {
                TempData["msg"] = "<script>alert('Đã xảy ra lỗi. Vui lòng thử lại!');</script>";
            }
            TempData["msg"] = @"<div id=""rowSuccess"" class=""row""> <div class=""col-sm-10""> <div class=""alert alert-success alert-dismissable fade in"" style=""padding-top: 5px; padding-bottom: 5px""> <a href=""#"" class=""close"" data-dismiss=""alert"" aria-label=""close"">&times;</a> Thêm mới thành công! </div> </div> </div>";
            return RedirectToAction("Index", new { sortOrder = String.Empty, currentFilter = String.Empty, searchString = String.Empty });
            //return RedirectToAction("ThemMoi");
        }
        [HttpGet]
        public ActionResult CapNhat(int? id)
        {
            GARADBEntities context = new Models.GARADBEntities();
            KHACHHANG client = context.KHACHHANGs.Single(c => c.MA_KH == id.Value);
            return View(client);
        }
        [HttpPost]
        public ActionResult CapNhat(KHACHHANG client)
        {
            GARADBEntities context = new Models.GARADBEntities();
            if (ModelState.IsValid)
            {
                var target = context.KHACHHANGs.Find(client.MA_KH);
                target.TEN_KH = client.TEN_KH;
                target.SDT = client.SDT;
                target.CMND = client.CMND;
                target.DIACHI = client.DIACHI;
                context.SaveChanges();
                TempData["msg"] = @"<div id=""rowSuccess"" class=""row""> <div class=""col-sm-10""> <div class=""alert alert-success alert-dismissable fade in"" style=""padding-top: 5px; padding-bottom: 5px""> <a href=""#"" class=""close"" data-dismiss=""alert"" aria-label=""close"">&times;</a> Đã cập nhật thành công! </div> </div> </div>";
                //TempData["msg"] = "<script>alert('Đã cập nhật thành công');</script>";
                return RedirectToAction("Index", new { sortOrder = String.Empty, currentFilter = String.Empty, searchString = String.Empty });
            }
            else
            {
                KHACHHANG KH = context.KHACHHANGs.Single(c => c.MA_KH == client.MA_KH);
                return View(KH);
            }
            //return View();
        }
        [ValidateInput(false)]
        [HttpPost]
        public JsonResult Xoa(int id)
        { 
            try
            {
                GARADBEntities context = new Models.GARADBEntities();
                var target = context.KHACHHANGs.Find(id);
                context.KHACHHANGs.Remove(target);
                context.SaveChanges();
                TempData["msg"] = @"<div id=""rowSuccess"" class=""row""> <div class=""col-sm-10""> <div class=""alert alert-success alert-dismissable fade in"" style=""padding-top: 5px; padding-bottom: 5px""> <a href=""#"" class=""close"" data-dismiss=""alert"" aria-label=""close"">&times;</a> Xóa thành công! </div> </div> </div>";
                return Json(new { value = "1", message = "Xóa thành công" },JsonRequestBehavior.AllowGet);
            }
            catch(Exception)
            {
                TempData["msg"] = @"<div id=""rowError"" class=""row""> <div class=""col-sm-10""> <div class=""alert alert-danger alert-dismissable fade in"" style=""padding-top: 5px; padding-bottom: 5px""> <a href=""#"" class=""close"" data-dismiss=""alert"" aria-label=""close"">&times;</a> Không thể xóa khách hàng đã có giao dịch! </div> </div> </div>";
                return Json(new { value = "-1", message = "Không thể xóa do đã có tham chiếu" }, JsonRequestBehavior.AllowGet);
            }

        }
    }
}