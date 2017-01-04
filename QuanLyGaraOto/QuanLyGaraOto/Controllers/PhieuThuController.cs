using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using QuanLyGaraOto.Models;
using QuanLyGaraOto.ViewModel;
using Rotativa.Options;

namespace QuanLyGaraOto.Controllers
{
    public class PhieuThuController : Controller
    {
        private LapPhieuThuViewModel lapPhieuThuViewModel;
        // GET: PhieuThu
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? searchOption,
            int? page)
        {
            ViewBag.CurrentSearchOption = searchOption;
            ViewBag.CurrentSort = sortOrder;
            ViewBag.DateSortParam = (sortOrder == "date_asc" ? "date_desc" : "date_asc");   //Ban đầu là null -> ASC
            GARADBEntities context = new GARADBEntities();
            List<PhieuThuViewModel> listPTT = new List<PhieuThuViewModel>(); 
            foreach (var item in context.PHIEU_THUTIEN.ToList())
            {
                String tennv = context.NHANVIENs.Single(nv => nv.MA_NV == item.MA_NV).HOTEN;
                listPTT.Add(new PhieuThuViewModel(item, tennv));
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
                        case 1: { listPTT = listPTT.Where(c => c.PhieuThu.MAPHIEUTHU.Contains(searchString)).ToList(); break; }
                        case 2: { listPTT = listPTT.Where(c => c.PhieuThu.NOIDUNG_THU.Contains(searchString)).ToList(); break; }
                        case 3: { listPTT = listPTT.Where(c => c.TenNV.Contains(searchString)).ToList(); break; }
                        default: { break; }
                    }
                }

            }
            switch (sortOrder)
            {
                case "date_asc": { listPTT = listPTT.OrderBy(c => c.PhieuThu.NGAYLAP).ToList(); break; }
                case "date_desc": { listPTT = listPTT.OrderByDescending(c => c.PhieuThu.NGAYLAP).ToList(); break; }
                default: { break; }
            }
            int pageSize = 5;
            int pageNumber = (page ?? 1); //nếu page có giá trị thì = page, ko thì = 1
            DSPhieuThuViewModel pttViewModel = new DSPhieuThuViewModel();
            pttViewModel.ListPhieuThu = listPTT.ToPagedList(pageNumber, pageSize);
            return View(pttViewModel);
        }

        [HttpGet]
        public ActionResult ThemMoi(int? idphieu, string type)
        {
            GARADBEntities context = new GARADBEntities();
            int UserId = int.Parse(Session["UserID"].ToString());
            NHANVIEN nv = context.NHANVIENs.Single(staff => staff.MA_NV == UserId);
            NHOMNGUOIDUNG groupuser = context.NHOMNGUOIDUNGs.Single(gu => gu.MA_NHOMNGUOIDUNG == nv.MA_NHOMNGUOIDUNG.Value);
            if (groupuser.CAPDO != 2 && groupuser.CAPDO != 3)
            {
                TempData["msg"] = @"<div id=""rowError"" class=""row""> <div class=""col-sm-10""> <div class=""alert alert-danger alert-dismissable fade in"" style=""padding-top: 5px; padding-bottom: 5px""> <a href=""#"" class=""close"" data-dismiss=""alert"" aria-label=""close"">&times;</a> Bạn không có quyền truy cập vào chức năng này! </div> </div> </div>";
                return RedirectToAction("Index", new { sortOrder = String.Empty, currentFilter = String.Empty, searchString = String.Empty });
            }

            lapPhieuThuViewModel = new LapPhieuThuViewModel();
            lapPhieuThuViewModel.PhieuThuTien.NGAYLAP = DateTime.Now;
            lapPhieuThuViewModel.SoTienThuToiThieu =
            int.Parse(context.BANGTHAMSOes.Single(ts => ts.TENTHAMSO == "SoTienThuToiThieu").GIATRI);
            lapPhieuThuViewModel.PhieuThuTien.MA_NV = UserId;
            lapPhieuThuViewModel.TenNV = nv.HOTEN;
            if (type.Equals("pbl"))
            {
                PHIEU_BANLE phieuBanle = context.PHIEU_BANLE.Single(c => c.ID_PHIEUBANLE == idphieu);
                lapPhieuThuViewModel.PhieuThuTien.ID_PHIEUBANLE = idphieu;
                lapPhieuThuViewModel.TongTien = phieuBanle.TongTien;
                lapPhieuThuViewModel.ConNo = phieuBanle.SoTienConLai;
                lapPhieuThuViewModel.PhieuThuTien.NOIDUNG_THU = "Thu tiền phiếu bán lẻ. Mã phiếu: " + phieuBanle.MaPhieuBan;
            } else if (type.Equals("pbx"))
            {
                PHIEU_BANXE phieuBanxe = context.PHIEU_BANXE.Single(c => c.ID_PHIEUBANXE == idphieu);
                lapPhieuThuViewModel.PhieuThuTien.ID_PHIEUBANXE = idphieu;
                lapPhieuThuViewModel.TongTien = phieuBanxe.TRIGIA;
                lapPhieuThuViewModel.ConNo = phieuBanxe.SOTIENCONLAI;
                lapPhieuThuViewModel.PhieuThuTien.NOIDUNG_THU = "Thu tiền phiếu bán xe. Mã phiếu: " + phieuBanxe.MAPHIEUBAN;

            } else if (type.Equals("pdv"))
            {
                PHIEU_DICHVU phieuDichvu = context.PHIEU_DICHVU.Single(c => c.ID_PHIEUDV == idphieu);
                lapPhieuThuViewModel.PhieuThuTien.ID_PHIEUDV = idphieu;
                lapPhieuThuViewModel.TongTien = phieuDichvu.TONGTIEN;
                lapPhieuThuViewModel.ConNo = phieuDichvu.SOTIEN_CONLAI;
                lapPhieuThuViewModel.PhieuThuTien.NOIDUNG_THU = "Thu tiền phiếu dịch vụ. Mã phiếu: " + phieuDichvu.MA_PHIEUDV;
            }
            return View(lapPhieuThuViewModel);
        }
        [HttpPost]
        public ActionResult ThemMoi(PHIEU_THUTIEN phieuThutien)
        {
            try
            {
                GARADBEntities context = new GARADBEntities();
                phieuThutien.NGAYLAP = DateTime.Today;
                context.PHIEU_THUTIEN.Add(phieuThutien);
                context.SaveChanges();
                TempData["msg"] = @"<div id=""rowSuccess"" class=""row""> <div class=""col-sm-10""> <div class=""alert alert-success alert-dismissable fade in"" style=""padding-top: 5px; padding-bottom: 5px""> <a href=""#"" class=""close"" data-dismiss=""alert"" aria-label=""close"">&times;</a> Thêm mới thành công! </div> </div> </div>";
            }
            catch (Exception)
            {
                TempData["msg"] = @"<div id=""rowError"" class=""row""> <div class=""col-sm-10""> <div class=""alert alert-danger alert-dismissable fade in"" style=""padding-top: 5px; padding-bottom: 5px""> <a href=""#"" class=""close"" data-dismiss=""alert"" aria-label=""close"">&times;</a> Đã có lỗi xảy ra! Vui lòng thử lại! </div> </div> </div>";
            }
            //In rồi return ra lại list
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult In(int id)
        {
            GARADBEntities context = new GARADBEntities();        
            PHIEU_THUTIEN ptt = context.PHIEU_THUTIEN.Single(c => c.ID_PHIEUTHUTIEN == id);
            String tennv = context.NHANVIENs.Single(nv => nv.MA_NV == ptt.MA_NV).HOTEN;
            PhieuThuViewModel phieuThuViewModel = new PhieuThuViewModel(ptt, tennv);

            string fileName = phieuThuViewModel.PhieuThu.MAPHIEUTHU + ".pdf";
            //return View(vmPhieuBanLe);
            return new Rotativa.ViewAsPdf("In", phieuThuViewModel)
            {
                FileName = fileName,
                PageOrientation = Orientation.Landscape,
                PageSize = Size.A4,
                PageMargins = { Left = 0, Right = 0 }, // it's in millimeters
                CustomSwitches = "--disable-smart-shrinking",
            };

        }

        [HttpPost]
        public ActionResult CapNhat(PhieuThuViewModel pttVM)
        {
            try
            {
                PHIEU_THUTIEN ptt = new PHIEU_THUTIEN();
                ptt = pttVM.PhieuThu;
                GARADBEntities context = new GARADBEntities();
                var target = context.PHIEU_THUTIEN.Find(ptt.ID_PHIEUTHUTIEN);
                target.MAPHIEUTHU = ptt.MAPHIEUTHU;
                target.SOTIEN = ptt.SOTIEN;
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
                int UserId = int.Parse(Session["UserID"].ToString());
                NHANVIEN nv = context.NHANVIENs.Single(staff => staff.MA_NV == UserId);
                NHOMNGUOIDUNG groupuser = context.NHOMNGUOIDUNGs.Single(gu => gu.MA_NHOMNGUOIDUNG == nv.MA_NHOMNGUOIDUNG.Value);
                if (groupuser.CAPDO != 2 && groupuser.CAPDO != 3)
                {
                    TempData["msg"] = @"<div id=""rowError"" class=""row""> <div class=""col-sm-10""> <div class=""alert alert-danger alert-dismissable fade in"" style=""padding-top: 5px; padding-bottom: 5px""> <a href=""#"" class=""close"" data-dismiss=""alert"" aria-label=""close"">&times;</a> Bạn không có quyền thực hiện thao tác này! </div> </div> </div>";
                    //return RedirectToAction("Index", new { sortOrder = String.Empty, currentFilter = String.Empty, searchString = String.Empty });
                    return Json(new { value = "-1", message = "Bạn không có quyền thực hiện thao tác này!" }, JsonRequestBehavior.AllowGet);
                }

                var target = context.PHIEU_THUTIEN.Find(id);
                context.PHIEU_THUTIEN.Remove(target);
                context.SaveChanges();
                TempData["msg"] = @"<div id=""rowSuccess"" class=""row""> <div class=""col-sm-10""> <div class=""alert alert-success alert-dismissable fade in"" style=""padding-top: 5px; padding-bottom: 5px""> <a href=""#"" class=""close"" data-dismiss=""alert"" aria-label=""close"">&times;</a> Xoá thành công! </div> </div> </div>";
                return Json(new { value = "1", message = "Xóa thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                TempData["msg"] = @"<div id=""rowError"" class=""row""> <div class=""col-sm-10""> <div class=""alert alert-danger alert-dismissable fade in"" style=""padding-top: 5px; padding-bottom: 5px""> <a href=""#"" class=""close"" data-dismiss=""alert"" aria-label=""close"">&times;</a> Không thể xóa do phiếu thu tiền này! </div> </div> </div>";
                return Json(new { value = "-1", message = "Không thể xóa do phiếu thu tiền này!" }, JsonRequestBehavior.AllowGet);
            }

        }
    }
}