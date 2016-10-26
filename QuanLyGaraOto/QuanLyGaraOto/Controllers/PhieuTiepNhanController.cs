using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QuanLyGaraOto.ViewModel;
using QuanLyGaraOto.Models;
using PagedList;

namespace QuanLyGaraOto.Controllers
{
    public class PhieuTiepNhanController : Controller
    {
        private GARADBEntities service = new GARADBEntities(); // service tuong tac voi database
        // GET: PhieuTiepNhan
        /// <summary>
        /// Mac dinh , load danh sach phieu tiep nhan trong database len man hinh
        /// </summary>
        /// <returns> View </returns>
        /// 
        [HttpGet]
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? searchOption, int? page)
        {
            ViewBag.CurrentSearchOption = searchOption;
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParam = sortOrder == "name_asc" ? "name_desc" : "name_asc";
            ViewBag.OweSortParam = sortOrder == "owe_asc" ? "owe_desc" : "owe_asc";
            List<PHIEU_TIEPNHAN> danhSachPhieuTiepNhan = this.service.PHIEU_TIEPNHAN.ToList(); // load danh sach phieu tiep nhan 
            List<PhieuTiepNhanListViewModel> listViewModels = new List<PhieuTiepNhanListViewModel>(); // instantiate model cho list view
            foreach (PHIEU_TIEPNHAN voucher in danhSachPhieuTiepNhan)
            {
                // load thong tin xe cua phieu
                PhieuTiepNhanListViewModel temp = new PhieuTiepNhanListViewModel();
                temp.bienSoXe = voucher.BIENSO_XE;
                temp.maKhachHang = voucher.MA_KH;
                temp.tenKhachHang = voucher.KHACHHANG.TEN_KH;
                temp.maPhieuTiepNhan = voucher.MA_PHIEUTIEPNHAN;
                temp.maSoCho = voucher.MASOCHO;
                temp.ngayLap = voucher.NGAYLAP;
                temp.ngayTra = voucher.NGAYTRAXE;
                temp.tinhTrang = voucher.TINHTRANG;
                // add vao list
                listViewModels.Add(temp);
            }

            // loc ket qua search cua user
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
                        case 1: // loc theo ngay lap phieu
                            {
                                listViewModels = listViewModels.Where(c =>
                                    c.ngayLap.Value.Date.Equals(DateTime.Parse(searchString).Date)).ToList();
                                break;
                            }
                        case 2: // loc theo ngay lap hen tra xe
                            {
                                listViewModels = listViewModels.Where(c =>
                                    c.ngayTra.Value.Date.Equals(DateTime.Parse(searchString).Date)).ToList(); break;
                            }

                        default: { break; }
                    }
                }

            }
            ///
            /// set up page size and page number
            ///
            int pageSize = 5;
            int pageNumber = (page ?? 1);

            PhieuTiepNhanViewModel viewModel = new PhieuTiepNhanViewModel(); // instantiate view model 
            viewModel.danhSachPhieuTiepNhan = listViewModels.ToPagedList(pageNumber, pageSize);
            // viewModel.danhSachHieuXe = this.service.HIEUXEs.ToList();
            // viewModel.danhsachXe = this.service.XEs.ToList();
            //viewModel.danhSachKhachHang = this.service.KHACHHANGs.ToList();
            // to be continued here !!!!!!
            return View(viewModel);
        }

        /// <summary>
        /// Load thong tin phieu tiep nhan va up len view
        /// </summary>
        /// <param name="maPhieu">id phieu can duoc update</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult SuaPhieuTiepNhan(int? maPhieu)
        {
            PhieuTiepNhanViewModel viewModel = new PhieuTiepNhanViewModel();
            viewModel.selectedPhieuTiepNhan = this.service.PHIEU_TIEPNHAN.Where(e => e.MA_PHIEUTIEPNHAN == maPhieu).SingleOrDefault();
            return View(viewModel);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SuaPhieuTiepNhan(PHIEU_TIEPNHAN arg)
        {
            PHIEU_TIEPNHAN phieuTiepNhan = this.service.PHIEU_TIEPNHAN.Where(e => e.MA_PHIEUTIEPNHAN == arg.MA_PHIEUTIEPNHAN).SingleOrDefault();
            phieuTiepNhan.NGAYTRAXE = arg.NGAYTRAXE;
            phieuTiepNhan.TINHTRANG = arg.TINHTRANG;
            this.service.Entry(phieuTiepNhan).State = System.Data.Entity.EntityState.Modified;
            this.service.SaveChanges();
            // tro lai man hinh danh sach
            return RedirectToAction("Index");
        }


    }
}