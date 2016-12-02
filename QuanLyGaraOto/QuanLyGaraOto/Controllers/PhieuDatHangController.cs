using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QuanLyGaraOto.Models;
using PagedList;
using QuanLyGaraOto.ViewModel;

namespace QuanLyGaraOto.Controllers
{
    public class PhieuDatHangController : Controller
    {
        // GET: PhieuDatHang
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? searchOption,
            int? page)
        {
            ViewBag.CurrentSearchOption = searchOption;
            ViewBag.CurrentSort = sortOrder;
            ViewBag.DateDatOrderSortParam = (sortOrder == "dat_asc" ? "dat_desc" : "dat_asc");   //Ban đầu là null -> ASC
            ViewBag.DateGiaoOrderSortParam = (sortOrder == "giao_asc" ? "giao_desc" : "giao_asc");   //Ban đầu là null -> ASC
            GARADBEntities context = new GARADBEntities();
            List<PhieuDatHangViewModel> listPDH = new List<PhieuDatHangViewModel>();
            foreach (var item in context.PHIEU_DATHANG.ToList())
            {
                String tennv = context.NHANVIENs.Single(nv => nv.MA_NV == item.MaNV).HOTEN;
                String tenncc = context.NHACUNGCAPs.Single(nv => nv.MaNCC == item.MaNCC).TenNCC;
                listPDH.Add(new PhieuDatHangViewModel(item, tennv, tenncc));
            }
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
                        case 1: { listPDH = listPDH.Where(c => c.PhieuDatHang.MaPhieuDat.Contains(searchString)).ToList(); break; }
                        case 2: { listPDH = listPDH.Where(c => c.PhieuDatHang.NgayDat.Value.Date.Equals(DateTime.Parse(searchString).Date)).ToList(); break; }
                        case 3: { listPDH = listPDH.Where(c => c.PhieuDatHang.NgayGiao.Value.Date.Equals(DateTime.Parse(searchString).Date)).ToList(); break; }
                        case 4: { listPDH = listPDH.Where(c => c.TenNV.Contains(searchString)).ToList(); break; }
                        default: { break; }
                    }
                }

            }
            switch (sortOrder)
            {
                case "dat_asc": { listPDH = listPDH.OrderBy(c => c.PhieuDatHang.NgayDat).ToList(); break; }
                case "dat_desc": { listPDH = listPDH.OrderByDescending(c => c.PhieuDatHang.NgayDat).ToList(); break; }
                case "giao_asc": { listPDH = listPDH.OrderBy(c => c.PhieuDatHang.NgayGiao).ToList(); break; }
                case "giao_desc": { listPDH = listPDH.OrderByDescending(c => c.PhieuDatHang.NgayGiao).ToList(); break; }
                default: { break; }
            }
            int pageSize = 5;
            int pageNumber = (page ?? 1); //nếu page có giá trị thì = page, ko thì = 1
            DSPhieuDatHangModel pdhViewModel = new DSPhieuDatHangModel();
            pdhViewModel.ListPhieuDathang = listPDH.ToPagedList(pageNumber, pageSize);
            return View(pdhViewModel);
        }
        [HttpGet]
        public ActionResult ThemMoi()
        {
            PhieuDatHangViewModel vmPhieuDH = new PhieuDatHangViewModel();
            GARADBEntities context = new GARADBEntities();
            vmPhieuDH.ListNhaCungCap = context.NHACUNGCAPs.ToList();
            vmPhieuDH.ListPhuTung = context.PHUTUNGs.ToList();
            vmPhieuDH.ListNhomNCC = context.NHOMNHACUNGCAPs.ToList();
            vmPhieuDH.ListHieuXe = context.HIEUXEs.ToList();
            int UserId = int.Parse(Session["UserID"].ToString());
            vmPhieuDH.PhieuDatHang.MaNV = UserId;
            int tggh = Int32.Parse(context.BANGTHAMSOes.Single(ts => ts.TENTHAMSO == "ThoiGianGiaoHang").GIATRI);
            vmPhieuDH.PhieuDatHang.NgayGiao = DateTime.Now.Date.AddDays(tggh);
            vmPhieuDH.TenNV = context.NHANVIENs.Single(nv => nv.MA_NV == UserId).HOTEN;
            return View(vmPhieuDH);
        }

        [HttpPost]
        public ActionResult ThemMoi(PHIEU_DATHANG pdh, IEnumerable<CHITIET_PHIEUDATHANG> listChiTiet)
        {
            try
            {
                GARADBEntities context = new GARADBEntities();
                pdh.NgayDat = DateTime.Now.Date;
                context.PHIEU_DATHANG.Add(pdh);               
                context.SaveChanges();
                List<CHITIET_PHIEUDATHANG> listCT = listChiTiet.ToList();
                foreach (var item in listChiTiet)
                {
                    if (item != null)
                    {
                        item.Id_PhieuDatHang = pdh.Id_PhieuDatHang;
                        context.CHITIET_PHIEUDATHANG.Add(item);
                    }                   
                }
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
        public ActionResult CapNhat(int id = 50)
        {
            GARADBEntities context = new GARADBEntities();
            PhieuDatHangViewModel vmPhieuDH = new PhieuDatHangViewModel();
            vmPhieuDH.PhieuDatHang = context.PHIEU_DATHANG.Single(p => p.Id_PhieuDatHang == id);

            vmPhieuDH.ListPhuTung = context.PHUTUNGs.ToList();
            vmPhieuDH.ListNhaCungCap = context.NHACUNGCAPs.ToList();
            vmPhieuDH.ListNhomNCC = context.NHOMNHACUNGCAPs.ToList();
            vmPhieuDH.ListHieuXe = context.HIEUXEs.ToList();
            vmPhieuDH.TenNV = context.NHANVIENs.Single(nv => nv.MA_NV == vmPhieuDH.PhieuDatHang.MaNV).HOTEN;
            vmPhieuDH.ListChiTietPhieuDH = new List<CHITIET_PHIEUDATHANG>();
            vmPhieuDH.ListChiTietPhieuDH = context.CHITIET_PHIEUDATHANG.Where(ct => ct.Id_PhieuDatHang == id).ToList();

            return View(vmPhieuDH);
        }
        [HttpPost]
        public ActionResult CapNhat(PHIEU_DATHANG phieudh)
        {
            try
            {
                GARADBEntities context = new GARADBEntities();
                var target = context.PHIEU_DATHANG.Single(pdh => pdh.Id_PhieuDatHang == phieudh.Id_PhieuDatHang);
                target.MaPhieuDat = phieudh.MaPhieuDat;
                target.NgayGiao = phieudh.NgayGiao;
                target.MaNCC = phieudh.MaNCC;
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
                var target = context.PHIEU_DATHANG.Find(id);
                context.PHIEU_DATHANG.Remove(target);
                context.SaveChanges();
                TempData["msg"] = @"<div id=""rowSuccess"" class=""row""> <div class=""col-sm-10""> <div class=""alert alert-success alert-dismissable fade in"" style=""padding-top: 5px; padding-bottom: 5px""> <a href=""#"" class=""close"" data-dismiss=""alert"" aria-label=""close"">&times;</a> Xoá thành công! </div> </div> </div>";
                return Json(new { value = "1", message = "Xóa thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                TempData["msg"] = @"<div id=""rowError"" class=""row""> <div class=""col-sm-10""> <div class=""alert alert-danger alert-dismissable fade in"" style=""padding-top: 5px; padding-bottom: 5px""> <a href=""#"" class=""close"" data-dismiss=""alert"" aria-label=""close"">&times;</a> Không thể xóa do đã có phiếu nhập hàng được thực hiện dựa trên phiếu này! </div> </div> </div>";
                return Json(new { value = "-1", message = "Không thể xóa do phiếu đặt hàng này!" }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public ActionResult GetNCCByNhomNCC(string nhomncc)
        {
            GARADBEntities context = new GARADBEntities();
            var ListNCC = context.NHACUNGCAPs.Where(pt => pt.NhomNCC.Equals(nhomncc)).Select(pt => "<option value='" + pt.MaNCC + "' title='" + pt.TenNCC + "'>" + pt.TenNCC + "</option>");
            return Content(String.Join("", ListNCC));
        }
        [HttpPost]
        public ActionResult GetPhuTungByHieuXe(string hieuxe)
        {
            GARADBEntities context = new GARADBEntities();
            var ListPhuTung = context.PHUTUNGs.Where(pt => pt.MA_HIEUXE.Equals(hieuxe)).Select(pt => "<option value='" + pt.ID + "' title='" + pt.SOLUONGTON + "' id='" + pt.MA_PHUTUNG + "' itemscope='" + pt.TG_BAOHANH + "' itemprop='" + pt.DONGIAXUAT + "'>" + pt.TEN_PHUTUNG + "</option>");
            return Content(String.Join("", ListPhuTung));
        }
        [HttpPost]
        public JsonResult XoaChiTiet(int id)
        {
            try
            {
                GARADBEntities context = new GARADBEntities();
                var target = context.CHITIET_PHIEUDATHANG.Single(ct => ct.ID == id);
                context.CHITIET_PHIEUDATHANG.Remove(target);
                context.SaveChanges();
                TempData["msg"] = @"<div id=""rowSuccess"" class=""row""> <div class=""col-sm-10""> <div class=""alert alert-success alert-dismissable fade in"" style=""padding-top: 5px; padding-bottom: 5px""> <a href=""#"" class=""close"" data-dismiss=""alert"" aria-label=""close"">&times;</a> Xoá chi tiết phiếu thành công! </div> </div> </div>";
                return Json(new { value = "1", message = "Xóa thành công!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                TempData["msg"] = @"<div id=""rowSuccess"" class=""row""> <div class=""col-sm-10""> <div class=""alert alert-success alert-dismissable fade in"" style=""padding-top: 5px; padding-bottom: 5px""> <a href=""#"" class=""close"" data-dismiss=""alert"" aria-label=""close"">&times;</a> Đã có lỗi khi xoá. Vui lòng thử lại. </div> </div> </div>";
                return Json(new { value = "-1", message = "Không thể xóa chi tiết phiếu!" }, JsonRequestBehavior.AllowGet);
            }

        }
        [HttpPost]
        public JsonResult ThemChiTiet(CHITIET_PHIEUDATHANG chitiet)
        {
            GARADBEntities context = new GARADBEntities();
            context.CHITIET_PHIEUDATHANG.Add(chitiet);
            context.SaveChanges();
            TempData["msg"] = @"<div id=""rowSuccess"" class=""row""> <div class=""col-sm-10""> <div class=""alert alert-success alert-dismissable fade in"" style=""padding-top: 5px; padding-bottom: 5px""> <a href=""#"" class=""close"" data-dismiss=""alert"" aria-label=""close"">&times;</a> Thêm chi tiết phiếu thành công! </div> </div> </div>";
            return Json(new { issucess = "1", newid = "1", message = "Thêm chi tiết phiếu thành công!" }, JsonRequestBehavior.AllowGet);
        }
    }
}