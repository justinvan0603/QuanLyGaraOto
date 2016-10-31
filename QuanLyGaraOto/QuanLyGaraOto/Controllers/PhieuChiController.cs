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
    public class PhieuChiController : Controller
    {
        private GARADBEntities service = new GARADBEntities(); // service de tuong tac voi database
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
                        case 1:
                            {
                                // loc phieu chi theo ngay lap phieu
                                listOfPhieuChis = listOfPhieuChis.Where(e => e.NGAYLAP.Value.Date.Equals(DateTime.Parse(searchString).Date)).ToList();
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
            return View();
        }

        [HttpPost]
        public ActionResult NhapPhieuChi(PHIEU_CHI phieuChi)
        {
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



        [HttpGet]
        public ActionResult DuplicatedBillExceptionView()
        {
            return View();
        }
    }
}