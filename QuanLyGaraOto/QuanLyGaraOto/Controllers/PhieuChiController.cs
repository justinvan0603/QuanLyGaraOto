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
    public class PhieuChiController : Controller
    {
        private GARADBEntities service = new GARADBEntities(); // service de tuong tac voi database
        private readonly static string FILE_NAME_PREFIX = "phieuchi_";
        // GET: PhieuChi
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? searchOption, int? page)
        {
            ViewBag.CurrentSearchOption = searchOption;
            ViewBag.CurrentSort = sortOrder;
            ViewBag.DateSortParam = sortOrder == "date_asc" ? "date_desc" : "date_asc";
            ViewBag.TotalSortParam = sortOrder == "total_asc" ? "total_desc" : "total_asc";
            ViewBag.RemainSortParam = sortOrder == "remain_asc" ? "remain_desc" : "remain_asc";

            List<PHIEU_CHI> listOfPhieuChis = this.service.PHIEU_CHI.ToList();

            if (searchString != null)
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
                        case 1: // loc the ma phieu chi
                            {
                                listOfPhieuChis = listOfPhieuChis.Where(e => e.MA_PHIEUCHI.Contains(searchString)).ToList();
                                break;
                            }
                        case 2:
                            {
                                try
                                {
                                    // loc phieu chi theo ngay lap phieu
                                    listOfPhieuChis = listOfPhieuChis.Where(e => e.NGAYLAP.Value.Date.Equals(DateTime.Parse(searchString).Date)).ToList();
                                }
                                catch (Exception e)
                                {
                                    listOfPhieuChis = this.service.PHIEU_CHI.ToList();
                                }
                                break;
                            }
                        default: { break; }
                    }
                }

            }
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            PhieuChiViewModel viewModel = new PhieuChiViewModel();
            viewModel.danhSachPhieuChi = listOfPhieuChis.ToPagedList(pageNumber, pageSize);
            return View(viewModel);
        }

        [HttpGet]
        public ActionResult NhapPhieuChi()
        {
            int UserId = int.Parse(Session["UserID"].ToString());
            NHANVIEN st = this.service.NHANVIENs.Single(staff => staff.MA_NV == UserId);
            NHOMNGUOIDUNG groupuser = this.service.NHOMNGUOIDUNGs.Single(gu => gu.MA_NHOMNGUOIDUNG == st.MA_NHOMNGUOIDUNG.Value);
            // kiem tra quyen han truoc khi xu ly, phieu chi chi lien quan voi ke toan va super user 
            if (groupuser.CAPDO != 2 && groupuser.CAPDO != 3)
            {
                TempData["msg"] = @"<div id=""rowError"" class=""row""> <div class=""col-sm-10""> <div class=""alert alert-danger alert-dismissable fade in"" style=""padding-top: 5px; padding-bottom: 5px""> <a href=""#"" class=""close"" data-dismiss=""alert"" aria-label=""close"">&times;</a> Bạn không có quyền truy cập vào chức năng này! </div> </div> </div>";
                return RedirectToAction("Index", new { sortOrder = String.Empty, currentFilter = String.Empty, searchString = String.Empty });
            }
            return View();
        }

        [HttpPost]
        public ActionResult NhapPhieuChi(PHIEU_CHI phieuChi)
        {
            int UserId = int.Parse(Session["UserID"].ToString());
            NHANVIEN st = this.service.NHANVIENs.Single(staff => staff.MA_NV == UserId);
            NHOMNGUOIDUNG groupuser = this.service.NHOMNGUOIDUNGs.Single(gu => gu.MA_NHOMNGUOIDUNG == st.MA_NHOMNGUOIDUNG.Value);
            // kiem tra quyen han truoc khi xu ly, phieu chi chi lien quan voi ke toan va super user 
            if (groupuser.CAPDO != 2 && groupuser.CAPDO != 3)
            {
                TempData["msg"] = @"<div id=""rowError"" class=""row""> <div class=""col-sm-10""> <div class=""alert alert-danger alert-dismissable fade in"" style=""padding-top: 5px; padding-bottom: 5px""> <a href=""#"" class=""close"" data-dismiss=""alert"" aria-label=""close"">&times;</a> Bạn không có quyền truy cập vào chức năng này! </div> </div> </div>";
                return RedirectToAction("Index", new { sortOrder = String.Empty, currentFilter = String.Empty, searchString = String.Empty });
            }
            // kiem tra co ton tai du lieu hay khong
            if (this.service.PHIEU_CHI.Where(e => e.MA_PHIEUCHI == phieuChi.MA_PHIEUCHI).Count() > 0)
            {
                return View("DuplicatedBillExceptionView");
            }
            else
            {
                // luu phieu chi vao trong databse
                this.service.PHIEU_CHI.Add(phieuChi);
                this.service.SaveChanges();
                return RedirectToAction("Index");
            }
        }


        /// <summary>
        /// Thong tin phieu chi tu phieu mua xe
        /// </summary>
        /// <param name="idPhieuMuaXe"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult NhapPhieuChiFromPhieuMuaXe(int? idPhieuMuaXe)
        {
            PhieuChiViewModel viewModel = new PhieuChiViewModel();
            viewModel.idPhieuMuaXe = idPhieuMuaXe;
            PHIEU_MUAXE phieuMuaXe = this.service.PHIEU_MUAXE.Where
                 (e => e.ID_PHIEUMUAXE == idPhieuMuaXe).FirstOrDefault();
            viewModel.noiDungChi = "Mua xe từ khách hàng. Mã phiếu mua : " + phieuMuaXe.MAPHIEUMUA + ".Ngày mua : " + phieuMuaXe.NGAYLAP.ToString();
            viewModel.triGia = phieuMuaXe.TRIGIA;
            return View(viewModel);
        }

        /// <summary>
        /// thong tin phieu chi tu phieu nhap hang
        /// </summary>
        /// <param name="idPhieuNhapHang"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult NhapPhieuChiFromPhieuNhapHang(int? idPhieuNhapHang)
        {
            int UserId = int.Parse(Session["UserID"].ToString());
            NHANVIEN st = this.service.NHANVIENs.Single(staff => staff.MA_NV == UserId);
            NHOMNGUOIDUNG groupuser = this.service.NHOMNGUOIDUNGs.Single(gu => gu.MA_NHOMNGUOIDUNG == st.MA_NHOMNGUOIDUNG.Value);
            // kiem tra quyen han truoc khi xu ly, phieu chi chi lien quan voi ke toan va super user 
            if (groupuser.CAPDO != 2 && groupuser.CAPDO != 3)
            {
                TempData["msg"] = @"<div id=""rowError"" class=""row""> <div class=""col-sm-10""> <div class=""alert alert-danger alert-dismissable fade in"" style=""padding-top: 5px; padding-bottom: 5px""> <a href=""#"" class=""close"" data-dismiss=""alert"" aria-label=""close"">&times;</a> Bạn không có quyền truy cập vào chức năng này! </div> </div> </div>";
                return RedirectToAction("Index", new { sortOrder = String.Empty, currentFilter = String.Empty, searchString = String.Empty });
            }
            PHIEU_NHAPHANG phieuNhapHang = this.service.PHIEU_NHAPHANG.Where(e => e.ID_PHIEUNHAPHANG == idPhieuNhapHang).FirstOrDefault();
            PhieuChiViewModel viewModel = new PhieuChiViewModel();
            viewModel.idPhieuNhapHang = idPhieuNhapHang;
            viewModel.noiDungChi = "Nhập phụ tùng. Mã phiếu nhập : " + phieuNhapHang.MA_PHIEUNHAPHANG + ".Ngày nhập : " + phieuNhapHang.NGAYLAP.ToString();
            viewModel.triGia = phieuNhapHang.TONGTIEN;
            return View(viewModel);
        }

        /// <summary>
        /// Sua thong tin phieu chi
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult SuaPhieuChi(int? id)
        {
            int UserId = int.Parse(Session["UserID"].ToString());
            NHANVIEN st = this.service.NHANVIENs.Single(staff => staff.MA_NV == UserId);
            NHOMNGUOIDUNG groupuser = this.service.NHOMNGUOIDUNGs.Single(gu => gu.MA_NHOMNGUOIDUNG == st.MA_NHOMNGUOIDUNG.Value);
            // kiem tra quyen han truoc khi xu ly, phieu chi chi lien quan voi ke toan va super user 
            if (groupuser.CAPDO != 2 && groupuser.CAPDO != 3)
            {
                TempData["msg"] = @"<div id=""rowError"" class=""row""> <div class=""col-sm-10""> <div class=""alert alert-danger alert-dismissable fade in"" style=""padding-top: 5px; padding-bottom: 5px""> <a href=""#"" class=""close"" data-dismiss=""alert"" aria-label=""close"">&times;</a> Bạn không có quyền truy cập vào chức năng này! </div> </div> </div>";
                return RedirectToAction("Index", new { sortOrder = String.Empty, currentFilter = String.Empty, searchString = String.Empty });
            }
            PhieuChiViewModel viewModel = new PhieuChiViewModel();
            viewModel.selectedItem = this.service.PHIEU_CHI.Where(e => e.ID == id).FirstOrDefault();
            return View(viewModel);
        }

        public ActionResult SuaPhieuChi(PHIEU_CHI arg)
        {
            PHIEU_CHI modifiedInfor = this.service.PHIEU_CHI.Where(e => e.ID == arg.ID).FirstOrDefault();
            // update mot so thong tin
            modifiedInfor.NOIDUNG = arg.NOIDUNG;
            this.service.Entry(modifiedInfor).State = System.Data.Entity.EntityState.Modified;
            this.service.SaveChanges();

            return RedirectToAction("Index");

        }


        /// <summary>
        /// Xoa phieu tiep nhan 
        /// </summary>
        /// <param name="billId"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Xoa(int billId)
        {
            int UserId = int.Parse(Session["UserID"].ToString());
            NHANVIEN st = this.service.NHANVIENs.Single(staff => staff.MA_NV == UserId);
            NHOMNGUOIDUNG groupuser = this.service.NHOMNGUOIDUNGs.Single(gu => gu.MA_NHOMNGUOIDUNG == st.MA_NHOMNGUOIDUNG.Value);
            // kiem tra quyen han truoc khi xu ly, phieu chi chi lien quan voi ke toan va super user 
            if (groupuser.CAPDO != 2 && groupuser.CAPDO != 3)
            {
                TempData["msg"] = @"<div id=""rowError"" class=""row""> <div class=""col-sm-10""> <div class=""alert alert-danger alert-dismissable fade in"" style=""padding-top: 5px; padding-bottom: 5px""> <a href=""#"" class=""close"" data-dismiss=""alert"" aria-label=""close"">&times;</a> Bạn không có quyền truy cập vào chức năng này! </div> </div> </div>";
                return Json(new { value = "-1", message = "Permission denied" }, JsonRequestBehavior.AllowGet);
            }
            try
            {
                PHIEU_CHI phieuChi = this.service.PHIEU_CHI.Where(e => e.ID == billId).FirstOrDefault();
                this.service.PHIEU_CHI.Remove(phieuChi);
                this.service.SaveChanges();
                // tra ve 
                return Json(new { value = "1", message = "Xóa thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { value = "-1", message = "SYSTEM ERROR !" }, JsonRequestBehavior.AllowGet);
            }
        }



        [HttpGet]
        public ActionResult DuplicatedBillExceptionView()
        {
            return View();
        }

        public ActionResult In(int? id)
        {
            int UserId = int.Parse(Session["UserID"].ToString());
            NHANVIEN st = this.service.NHANVIENs.Single(staff => staff.MA_NV == UserId);
            NHOMNGUOIDUNG groupuser = this.service.NHOMNGUOIDUNGs.Single(gu => gu.MA_NHOMNGUOIDUNG == st.MA_NHOMNGUOIDUNG.Value);
            if (groupuser.CAPDO != 2 && groupuser.CAPDO != 4)
            {
                TempData["msg"] = @"<div id=""rowError"" class=""row""> <div class=""col-sm-10""> <div class=""alert alert-danger alert-dismissable fade in"" style=""padding-top: 5px; padding-bottom: 5px""> <a href=""#"" class=""close"" data-dismiss=""alert"" aria-label=""close"">&times;</a> Bạn không có quyền truy cập vào chức năng này! </div> </div> </div>";
                return RedirectToAction("Index", new { sortOrder = String.Empty, currentFilter = String.Empty, searchString = String.Empty });
            }
            PhieuChiViewModel viewModel = new PhieuChiViewModel();
            viewModel.selectedItem = this.service.PHIEU_CHI.Where(e => e.ID == id).FirstOrDefault();
            string fileName = FILE_NAME_PREFIX + viewModel.selectedItem.ID + ".pdf";
            return new Rotativa.ViewAsPdf("In", viewModel)
            {
                FileName = fileName,
                PageOrientation = Orientation.Landscape,
                PageSize = Size.A4,
                PageMargins = { Left = 0, Right = 0 }, // it's in millimeters
                CustomSwitches = "--disable-smart-shrinking"
            };
        }
    }
}