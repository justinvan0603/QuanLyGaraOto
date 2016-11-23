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
    public class PhieuBaoHanhController : Controller
    {
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
                                // loc phieu chi theo ngay lap phieu
                                listOfPhieuBaoHanh = listOfPhieuBaoHanh.Where(e => e.NGAYLAP.Value.Date.Equals(DateTime.Parse(searchString).Date)).ToList();
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
            return RedirectToAction("SuaPhieuBaoHanh/" + phieuBaoHanh.MA_PHIEUBH);
        }

        [HttpGet]
        public ActionResult SuaPhieuBaoHanh(int idPhieuBaoHanh)
        {
            PhieuBaoHanhViewModel viewModel = new PhieuBaoHanhViewModel();
            viewModel.selectedItem = this.service.PHIEU_BAOHANH.Where(e => e.MA_PHIEUBH == idPhieuBaoHanh).FirstOrDefault();
            viewModel.danhSachChiTietBaoHanh = this.service.CHITIET_PHIEUBH.Where(e => e.MA_PHIEUBH == idPhieuBaoHanh).ToList();
            viewModel.danhSachPhuTung = this.service.PHUTUNGs.ToList();
            return View(viewModel);
        }


        public ActionResult DuplicatedBillExceptionView()
        {
            return View();
        }
    }
}