using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QuanLyGaraOto.Models;
using QuanLyGaraOto.ViewModel;
using PagedList;

namespace QuanLyGaraOto.Controllers
{
    public class PhieuMuaXeController : Controller
    {
        private GARADBEntities service = new GARADBEntities(); // service de tuong tac voi database

        /// <summary>
        /// Tra ve view chua danh sach cac phieu mua xe cua GARA
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? searchOption, int? page)
        {
            ViewBag.CurrentSearchOption = searchOption;
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParam = sortOrder == "name_asc" ? "name_desc" : "name_asc";
            ViewBag.OweSortParam = sortOrder == "owe_asc" ? "owe_desc" : "owe_asc";

            List<PHIEU_MUAXE> listOfPHIEUMUAXEs = this.service.PHIEU_MUAXE.ToList(); // lay tat ca record trong database
            List<PhieuMuaXeListViewModel> danhSachPhieuMuaXe = new List<PhieuMuaXeListViewModel>();
            // loop de mapping 
            foreach (PHIEU_MUAXE item in listOfPHIEUMUAXEs)
            {
                PhieuMuaXeListViewModel temp = new PhieuMuaXeListViewModel(); // instance mot PhieuBanXeDataTableModel de dap du lieu
                temp.id = item.ID_PHIEUMUAXE;
                temp.bienSoXe = item.BS_XE;
                if (item.MAKH != null)
                {
                    temp.khachHang = this.service.KHACHHANGs.Where(e => e.MA_KH == item.MAKH).First().TEN_KH; // ten khach hang
                }
                temp.nhanVien = this.service.NHANVIENs.Where(e => e.MA_NV == item.MaNV).First().HOTEN; // ten nhan vien
                temp.maPhieuMua = item.MAPHIEUMUA;
                temp.ngayLapPhieu = item.NGAYLAP;
                temp.triGia = item.TRIGIA;
                // neu ton phieu chi chua thong id phieu mua xe nay trong phieu PHIEU_CHI 
                //-> phieu nay da duoc chi tien
                if (this.service.PHIEU_CHI.Where(e => e.ID_PHIEUMUAXE == temp.id).ToList().Count > 0)
                {
                    temp.isPaid = true;
                }
                else
                {
                    temp.isPaid = false;
                }
                danhSachPhieuMuaXe.Add(temp);
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
                        case 1: break; // search theo ma phieu -> update code sau
                        case 2:
                            {
                                danhSachPhieuMuaXe = danhSachPhieuMuaXe.Where(c =>
                                    c.ngayLapPhieu.Value.Date.Equals(DateTime.Parse(searchString).Date)).ToList(); break;
                            } // loc theo ngay lap phieu

                        default: { break; }
                    }
                }

            }
            ///
            /// set up page size and page number
            ///
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            PhieuMuaXeViewModel phieuBanXeViewModel = new PhieuMuaXeViewModel();
            phieuBanXeViewModel.danhSachPhieuMuaXe = danhSachPhieuMuaXe.ToPagedList(pageNumber, pageSize);

            // tra ve view co chua view model trong no
            return View(phieuBanXeViewModel);
        }

        [HttpGet]
        public ActionResult NhapPhieuMuaXe()
        {
            PhieuMuaXeViewModel viewModel = new PhieuMuaXeViewModel();
            viewModel.danhSachKhachHang = this.service.KHACHHANGs.ToList(); // load danh sach khach hang
            viewModel.danhSachHieuXe = this.service.HIEUXEs.ToList(); // load danh sach hieu xe
            return View(viewModel);

        }

        /// <summary>
        /// Thuc hien luu thong tin xe moi cung nhu thong tin phieu mua xe khi nguoi dung click nut luu tren view. 
        /// Sau khi thuc hien xong, tu chuyen huong sang trang danh sach
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult NhapPhieuMuaXe(PhieuNhapMuaXeModel arg)
        {
            // dieu tien, kiem tra xem bien so xe do da ton tai trong du lieu cua kho chua
            if (this.service.XEs.Where(e => e.BS_XE == arg.bienSoXe).ToList().Count > 0)
            {
                // chuyen huong
                return Redirect("DuplicatedVerhicleExceptionView");
            }
            /* Cap nhat kho xe*/
            XE xeMoi = new XE();
            xeMoi.BS_XE = arg.bienSoXe;
            xeMoi.DOI_XE = arg.doiXe;
            xeMoi.HIEU_XE = arg.hieuXe;
            xeMoi.HINHTHUC = true; // true -> la xe co the ban duoc trong gara
            xeMoi.MA_KH = arg.maKhachHang; // nullable
            xeMoi.NGAY_TIEPNHAN = arg.ngayLapPhieu; // ngay tiep nhan duoc hieu ngam la ngay lap phieu
            xeMoi.SO_KHUNG = arg.soKhung;
            xeMoi.SO_KM = arg.soKm;
            xeMoi.SO_MAY = arg.soMay;
            xeMoi.TINH_TRANG = arg.tinhTrang;

            /* thong tin phieu mua xe moi */
            PHIEU_MUAXE phieuMuaXeMoi = new PHIEU_MUAXE();
            phieuMuaXeMoi.MAPHIEUMUA = arg.maPhieuMuaXe;
            phieuMuaXeMoi.NGAYLAP = arg.ngayLapPhieu;
            phieuMuaXeMoi.MAKH = arg.maNhanVien;
            phieuMuaXeMoi.MaNV = 1; // this.service.NHANVIENs.Single(e => e.USERNAME.Equals(Session["Username"])).MA_NV;
            phieuMuaXeMoi.TRIGIA = arg.tongGiaTri;


            // tien hanh luu
            this.service.XEs.Add(xeMoi);
            this.service.PHIEU_MUAXE.Add(phieuMuaXeMoi);
            this.service.SaveChanges();
            return Redirect("Index");
        }






        public ActionResult DuplicatedVerhicleExceptionView()
        {
            return View();
        }

    }
}