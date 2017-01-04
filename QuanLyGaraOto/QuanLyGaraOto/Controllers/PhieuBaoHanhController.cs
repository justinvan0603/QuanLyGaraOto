using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using QuanLyGaraOto.Models;
using QuanLyGaraOto.ViewModel;
using System.Globalization;
using Rotativa.Options;


namespace QuanLyGaraOto.Controllers
{
    public class PhieuBaoHanhController : Controller
    {
        private readonly static string FILE_NAME_PREFIX = "phieubaohanh_";
        private GARADBEntities service = new GARADBEntities(); // service de tuong tac voi database
        // GET: PhieuBaoHanh
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? searchOption, int? page)
        {
            ViewBag.CurrentSearchOption = searchOption;
            ViewBag.CurrentSort = sortOrder;
            ViewBag.DateSortParam = sortOrder == "date_asc" ? "date_desc" : "date_asc";
            ViewBag.TotalSortParam = sortOrder == "total_asc" ? "total_desc" : "total_asc";
            ViewBag.RemainSortParam = sortOrder == "remain_asc" ? "remain_desc" : "remain_asc";
            // load danh sach phieu bao hanh trong database
            List<PHIEU_BAOHANH> listOfPhieuBaoHanh = this.service.PHIEU_BAOHANH.ToList();

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
                        case 1:
                            {
                                try
                                {
                                    // loc phieu chi theo ngay lap phieu
                                    listOfPhieuBaoHanh = listOfPhieuBaoHanh.Where(e => e.NGAYLAP.Value.Date.Equals(DateTime.Parse(searchString).Date)).ToList();
                                }
                                catch (Exception e)
                                {

                                }
                                break;

                            }
                        default: { break; }
                    }
                }

            }
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            PhieuBaoHanhViewModel viewModel = new PhieuBaoHanhViewModel();
            viewModel.danhSachPhieuBaoHanh = listOfPhieuBaoHanh.ToPagedList(pageNumber, pageSize);
            return View(viewModel);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="idPhieuDichVu"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ThemPhieuBaoHanhFromPhieuDichVu(int? idPhieuDichVu)
        {
            int UserId = int.Parse(Session["UserID"].ToString());
            NHANVIEN st = this.service.NHANVIENs.Single(staff => staff.MA_NV == UserId);
            NHOMNGUOIDUNG groupuser = this.service.NHOMNGUOIDUNGs.Single(gu => gu.MA_NHOMNGUOIDUNG == st.MA_NHOMNGUOIDUNG.Value);
            // kiem tra quyen han truoc khi xu ly, phieu chi chi lien quan voi nhan vien cham soc khach hang va super user 
            if (groupuser.CAPDO != 2 && groupuser.CAPDO != 4)
            {
                TempData["msg"] = @"<div id=""rowError"" class=""row""> <div class=""col-sm-10""> <div class=""alert alert-danger alert-dismissable fade in"" style=""padding-top: 5px; padding-bottom: 5px""> <a href=""#"" class=""close"" data-dismiss=""alert"" aria-label=""close"">&times;</a> Bạn không có quyền truy cập vào chức năng này! </div> </div> </div>";
                return RedirectToAction("Index", new { sortOrder = String.Empty, currentFilter = String.Empty, searchString = String.Empty });
            }
            // kiem tra phieu dich vu do da duoc lap phieu bao hanh hay chua ?
            if (this.service.CHITIET_PHIEUBH.Where(e => e.MA_PHIEUDV == idPhieuDichVu).Count() > 0)
            {
                // thong bao cho nguoi dung
                return View("DuplicatedBillExceptionView");
            }
            // load thong tin phieu dich vu
            PHIEU_DICHVU phieuDichVu = this.service.PHIEU_DICHVU.Where(e => e.ID_PHIEUDV == idPhieuDichVu).SingleOrDefault();
            List<CHITIET_PHIEUDV> danhSachChiTiet = this.service.CHITIET_PHIEUDV.Where(e => e.ID_PHIEUDV == idPhieuDichVu).ToList();

            // tien hanh luu phieu bao hanh
            PHIEU_BAOHANH phieuBaoHanh = new PHIEU_BAOHANH();
            phieuBaoHanh.MA_NV = 1; // test

            phieuBaoHanh.TINHTRANG = "";
            phieuBaoHanh.NGAYLAP = DateTime.Now.Date;
            this.service.PHIEU_BAOHANH.Add(phieuBaoHanh);
            this.service.SaveChanges();

            // lay id cua phieu bao hanh vua luu
            int idPhieuBaoHanh = phieuBaoHanh.MA_PHIEUBH;

            // luu chi tiet bao hanh
            List<CHITIET_PHIEUBH> danhSachChiTietBaoHanh = new List<CHITIET_PHIEUBH>();
            foreach (CHITIET_PHIEUDV detail in danhSachChiTiet)
            {
                CHITIET_PHIEUBH chiTietBaoHhanh = new CHITIET_PHIEUBH();
                chiTietBaoHhanh.MA_PHIEUBH = idPhieuBaoHanh;
                chiTietBaoHhanh.MA_PHIEUDV = detail.ID_PHIEUDV;
                chiTietBaoHhanh.MA_PHUTUNG = detail.MA_PT;
                // mac dinh, ngay bao hanh va ngay hen tra se la ngay hien tai, user co the cap nhat thong tin nay sau
                chiTietBaoHhanh.NGAYHENTRA = DateTime.Now;
                chiTietBaoHhanh.NGAYTRA = DateTime.Now;
                // 
                danhSachChiTietBaoHanh.Add(chiTietBaoHhanh);
            }

            // luu danh sach chi tiet
            this.service.CHITIET_PHIEUBH.AddRange(danhSachChiTietBaoHanh);
            this.service.SaveChanges();

            // chuyen qua trang sua phieu banh de nguoi co the xem duoc thong tin phieu bao hanh ho vua lap
            //PhieuBaoHanhViewModel viewModel = new PhieuBaoHanhViewModel();
            //viewModel.selectedItem = phieuBaoHanh;
            //viewModel.danhSachChiTietBaoHanh = danhSachChiTietBaoHanh;
            return Redirect("SuaPhieuBaoHanh?idPhieuBaoHanh=" + phieuBaoHanh.MA_PHIEUBH);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idPhieuBanLe"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ThemPhieuBaoHanhFromPhieuBanLe(int idPhieuBanLe)
        {
            int UserId = int.Parse(Session["UserID"].ToString());
            NHANVIEN st = this.service.NHANVIENs.Single(staff => staff.MA_NV == UserId);
            NHOMNGUOIDUNG groupuser = this.service.NHOMNGUOIDUNGs.Single(gu => gu.MA_NHOMNGUOIDUNG == st.MA_NHOMNGUOIDUNG.Value);
            // kiem tra quyen han truoc khi xu ly, phieu chi chi lien quan voi nhan vien cham soc khach hang va super user 
            if (groupuser.CAPDO != 2 && groupuser.CAPDO != 4)
            {
                TempData["msg"] = @"<div id=""rowError"" class=""row""> <div class=""col-sm-10""> <div class=""alert alert-danger alert-dismissable fade in"" style=""padding-top: 5px; padding-bottom: 5px""> <a href=""#"" class=""close"" data-dismiss=""alert"" aria-label=""close"">&times;</a> Bạn không có quyền truy cập vào chức năng này! </div> </div> </div>";
                return RedirectToAction("Index", new { sortOrder = String.Empty, currentFilter = String.Empty, searchString = String.Empty });
            }
            // kiem tra phieu dich vu do da duoc lap phieu bao hanh hay chua ?
            if (this.service.CHITIET_PHIEUBH.Where(e => e.MAPHIEU_BANLE == idPhieuBanLe).Count() > 0)
            {
                // thong bao cho nguoi dung
                return View("DuplicatedBillExceptionView");
            }
            // load thong tin phieu dich vu
            PHIEU_BANLE phieuBanLe = this.service.PHIEU_BANLE.Where(e => e.ID_PHIEUBANLE == idPhieuBanLe).SingleOrDefault();
            List<CHITIET_PHIEUBANLE> danhSachChiTiet = this.service.CHITIET_PHIEUBANLE.Where(e => e.ID_PHIEUBANLE == idPhieuBanLe).ToList();

            // tien hanh luu phieu bao hanh
            PHIEU_BAOHANH phieuBaoHanh = new PHIEU_BAOHANH();
            phieuBaoHanh.MA_NV = 1; // test

            phieuBaoHanh.TINHTRANG = "";
            phieuBaoHanh.NGAYLAP = DateTime.Now.Date;
            this.service.PHIEU_BAOHANH.Add(phieuBaoHanh);
            this.service.SaveChanges();

            // lay id cua phieu bao hanh vua luu
            int idPhieuBaoHanh = phieuBaoHanh.MA_PHIEUBH;

            // luu chi tiet bao hanh
            List<CHITIET_PHIEUBH> danhSachChiTietBaoHanh = new List<CHITIET_PHIEUBH>();
            foreach (CHITIET_PHIEUBANLE detail in danhSachChiTiet)
            {
                CHITIET_PHIEUBH chiTietBaoHhanh = new CHITIET_PHIEUBH();
                chiTietBaoHhanh.MA_PHIEUBH = idPhieuBaoHanh;
                chiTietBaoHhanh.MAPHIEU_BANLE = detail.ID_PHIEUBANLE;
                chiTietBaoHhanh.MA_PHUTUNG = (int)detail.MAPT;
                // mac dinh, ngay bao hanh va ngay hen tra se la ngay hien tai, user co the cap nhat thong tin nay sau
                chiTietBaoHhanh.NGAYHENTRA = DateTime.Now;
                chiTietBaoHhanh.NGAYTRA = DateTime.Now;
                // 
                danhSachChiTietBaoHanh.Add(chiTietBaoHhanh);
            }

            // luu danh sach chi tiet
            this.service.CHITIET_PHIEUBH.AddRange(danhSachChiTietBaoHanh);
            this.service.SaveChanges();

            // chuyen qua trang sua phieu banh de nguoi co the xem duoc thong tin phieu bao hanh ho vua lap
            //PhieuBaoHanhViewModel viewModel = new PhieuBaoHanhViewModel();
            //viewModel.selectedItem = phieuBaoHanh;
            //viewModel.danhSachChiTietBaoHanh = danhSachChiTietBaoHanh;
            return Redirect("SuaPhieuBaoHanh?idPhieuBaoHanh=" + phieuBaoHanh.MA_PHIEUBH);
        }

        [HttpGet]
        public ActionResult SuaPhieuBaoHanh(int idPhieuBaoHanh)
        {
            int UserId = int.Parse(Session["UserID"].ToString());
            NHANVIEN st = this.service.NHANVIENs.Single(staff => staff.MA_NV == UserId);
            NHOMNGUOIDUNG groupuser = this.service.NHOMNGUOIDUNGs.Single(gu => gu.MA_NHOMNGUOIDUNG == st.MA_NHOMNGUOIDUNG.Value);
            // kiem tra quyen han truoc khi xu ly, phieu chi chi lien quan voi nhan vien cham soc khach hang va super user 
            if (groupuser.CAPDO != 2 && groupuser.CAPDO != 4)
            {
                TempData["msg"] = @"<div id=""rowError"" class=""row""> <div class=""col-sm-10""> <div class=""alert alert-danger alert-dismissable fade in"" style=""padding-top: 5px; padding-bottom: 5px""> <a href=""#"" class=""close"" data-dismiss=""alert"" aria-label=""close"">&times;</a> Bạn không có quyền truy cập vào chức năng này! </div> </div> </div>";
                return RedirectToAction("Index", new { sortOrder = String.Empty, currentFilter = String.Empty, searchString = String.Empty });
            }
            PhieuBaoHanhViewModel viewModel = new PhieuBaoHanhViewModel();
            viewModel.selectedItem = this.service.PHIEU_BAOHANH.Where(e => e.MA_PHIEUBH == idPhieuBaoHanh).FirstOrDefault();
            viewModel.danhSachChiTietBaoHanh = this.service.CHITIET_PHIEUBH.Where(e => e.MA_PHIEUBH == idPhieuBaoHanh).ToList();
            viewModel.danhSachPhuTung = this.service.PHUTUNGs.ToList();
            return View(viewModel);
        }

        /// <summary>
        /// Luu trinh trang cua phieu bao hanh. Sau do tro lai man hinh danh sach
        /// </summary>
        /// <param name="phieuBaoHanh"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SuaPhieuBaoHanh(int id, string tinhTrang)
        {
            PHIEU_BAOHANH PHIEU_BAOHANH = this.service.PHIEU_BAOHANH.Where(e => e.MA_PHIEUBH == id).FirstOrDefault();
            PHIEU_BAOHANH.TINHTRANG = tinhTrang;
            this.service.Entry(PHIEU_BAOHANH).State = System.Data.Entity.EntityState.Modified;
            this.service.SaveChanges();

            return RedirectToAction("Index");
        }


        /// <summary>
        /// Cap nhat thong tin ngay tra va ngay hen tra cua mot chi chiet phieu bao hanh
        /// </summary>
        /// <param name="idChiTiet"></param>
        /// <param name="ngayHenTra"></param>
        /// <param name="ngayTra"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CapNhatChiTietBaoHanh(UpdateDetailInfor chitiet)
        {
            // tim chi tiet phieu can sua
            CHITIET_PHIEUBH chiTietPBH = this.service.CHITIET_PHIEUBH.Where(e => e.Id == chitiet.detailId).FirstOrDefault();
            if (chiTietPBH != null)
            {
                // tien hanh cap nhat thong tin
                chiTietPBH.NGAYHENTRA = DateTime.ParseExact(chitiet.ngayHenTra, "dd/MM/yyyy", CultureInfo.CurrentCulture);
                chiTietPBH.NGAYTRA = DateTime.ParseExact(chitiet.ngayTra, "dd/MM/yyyy", CultureInfo.CurrentCulture);
                this.service.Entry(chiTietPBH).State = System.Data.Entity.EntityState.Modified;
                this.service.SaveChanges();
                // thong bao cho nguoi dung da cap nhat thanh cong
                TempData["msg"] = @"<div id=""rowSuccess"" class=""row""> <div class=""col-sm-10""> <div class=""alert alert-success alert-dismissable fade in"" style=""padding-top: 5px; padding-bottom: 5px""> <a href=""#"" class=""close"" data-dismiss=""alert"" aria-label=""close"">&times;</a> Cập nhật thành công! </div> </div> </div>";
                return Json(new { value = "1", message = "Cập nhật thành công, chitiet ->" + chitiet.detailId + "ngay hen tra -> " + chitiet.ngayHenTra }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                // truong hop cap nhat khong thanh cong
                TempData["msg"] = @"<div id=""rowError"" class=""row""> <div class=""col-sm-10""> <div class=""alert alert-danger alert-dismissable fade in"" style=""padding-top: 5px; padding-bottom: 5px""> <a href=""#"" class=""close"" data-dismiss=""alert"" aria-label=""close"">&times;</a> Không thể xóa do đã có phiếu nhập hàng được thực hiện dựa trên phiếu này! </div> </div> </div>";
                return Json(new { value = "-1", message = "Không thể cập nhật !" }, JsonRequestBehavior.AllowGet);
            }

        }


        /// <summary>
        /// Delete a verhical sale of bill from database. 
        /// </summary>
        /// <param name="idPhieuBaoHanh"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Xoa(int? idPhieuBaoHanh)
        {
            int UserId = int.Parse(Session["UserID"].ToString());
            NHANVIEN st = this.service.NHANVIENs.Single(staff => staff.MA_NV == UserId);
            NHOMNGUOIDUNG groupuser = this.service.NHOMNGUOIDUNGs.Single(gu => gu.MA_NHOMNGUOIDUNG == st.MA_NHOMNGUOIDUNG.Value);
            // kiem tra quyen han truoc khi xu ly, phieu chi chi lien quan voi nhan vien cham soc khach hang va super user 
            if (groupuser.CAPDO != 2 && groupuser.CAPDO != 4)
            {
                TempData["msg"] = @"<div id=""rowError"" class=""row""> <div class=""col-sm-10""> <div class=""alert alert-danger alert-dismissable fade in"" style=""padding-top: 5px; padding-bottom: 5px""> <a href=""#"" class=""close"" data-dismiss=""alert"" aria-label=""close"">&times;</a> Bạn không có quyền truy cập vào chức năng này! </div> </div> </div>";
                return Json(new { value = "-1", message = "Permission denied" }, JsonRequestBehavior.AllowGet);
            }
            PHIEU_BAOHANH phieuBaoHanh = this.service.PHIEU_BAOHANH.Where(e => e.MA_PHIEUBH == idPhieuBaoHanh).FirstOrDefault();
            // after removing the verhical sale of bill => remove the correspoding receipt that is related to it
            List<CHITIET_PHIEUBH> danhSachChiTietPhieuBaoHanh = this.service.CHITIET_PHIEUBH.Where(e => e.MA_PHIEUBH == idPhieuBaoHanh).ToList();
            foreach (CHITIET_PHIEUBH chiTiet in danhSachChiTietPhieuBaoHanh)
            {
                this.service.CHITIET_PHIEUBH.Remove(chiTiet);
            }
            this.service.SaveChanges();
            this.service.PHIEU_BAOHANH.Remove(phieuBaoHanh);
            this.service.SaveChanges();
            TempData["msg"] = @"<div id=""rowSuccess"" class=""row""> <div class=""col-sm-10""> <div class=""alert alert-success alert-dismissable fade in"" style=""padding-top: 5px; padding-bottom: 5px""> <a href=""#"" class=""close"" data-dismiss=""alert"" aria-label=""close"">&times;</a> Xóa thành cồng</div> </div> </div>";
            // tra ve 
            return Json(new { value = "1", message = "Xóa thành công" }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult DuplicatedBillExceptionView()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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
            PhieuBaoHanhViewModel viewModel = new PhieuBaoHanhViewModel();
            viewModel.selectedItem = this.service.PHIEU_BAOHANH.Where(e => e.MA_PHIEUBH == id).FirstOrDefault();
            viewModel.danhSachChiTietBaoHanh = this.service.CHITIET_PHIEUBH.Where(e => e.MA_PHIEUBH == id).ToList();
            viewModel.danhSachPhuTung = this.service.PHUTUNGs.ToList();
            string fileName = FILE_NAME_PREFIX + viewModel.selectedItem.MA_PHIEUBH + ".pdf";
            return new Rotativa.ViewAsPdf("In", viewModel)
            {
                FileName = fileName,
                PageOrientation = Orientation.Landscape,
                PageSize = Size.A4,
                PageMargins = { Left = 0, Right = 0 }, // it's in millimeters
                CustomSwitches = "--disable-smart-shrinking"
            };
        }


        public class UpdateDetailInfor
        {
            public int detailId { get; set; }
            public string ngayHenTra { get; set; }
            public string ngayTra { get; set; }
        }
    }
}