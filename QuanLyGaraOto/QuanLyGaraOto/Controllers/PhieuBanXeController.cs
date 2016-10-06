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
    public class PhieuBanXeController : Controller
    {

        private GARADBEntities service = new GARADBEntities(); // entity service to manipulate data
        /// <summary>
        /// Tra ve danh sach tat ca cac phieu ban xe trong database
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? searchOption, int? page)
        {
            ViewBag.CurrentSearchOption = searchOption;
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParam = sortOrder == "name_asc" ? "name_desc" : "name_asc";
            ViewBag.OweSortParam = sortOrder == "owe_asc" ? "owe_desc" : "owe_asc";

            List<PHIEU_BANXE> listOfPHIEUBANXEs = this.service.PHIEU_BANXE.ToList(); // lay tat ca record trong database
            List<PhieuBanXeDataTableModel> PHIEUBANXEsDataTableModels = new List<PhieuBanXeDataTableModel>();
            // loop de mapping 
            foreach (PHIEU_BANXE item in listOfPHIEUBANXEs)
            {
                PhieuBanXeDataTableModel temp = new PhieuBanXeDataTableModel(); // instance mot PhieuBanXeDataTableModel de dap du lieu
                temp.id = item.ID_PHIEUBANXE;
                temp.maPhieuBan = item.MAPHIEUBAN;
                temp.maNhanVien = item.MaNV;
                temp.bienSoXe = item.BS_XE;
                temp.ngayLapPhieu = item.NGAYLAP;
                temp.soTienConLai = item.SOTIENCONLAI;
                temp.giaTri = item.TRIGIA;
                temp.hanChotThanhToan = item.HANCHOTTHANHTOAN;
                PHIEUBANXEsDataTableModels.Add(temp);
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
                                PHIEUBANXEsDataTableModels = PHIEUBANXEsDataTableModels.Where(c =>
                                    c.ngayLapPhieu.Value.Date.Equals(DateTime.Parse(searchString).Date)).ToList(); break;
                            } // loc theo ngay lap phieu
                        case 3:
                            {
                                PHIEUBANXEsDataTableModels = PHIEUBANXEsDataTableModels.Where(c =>
                                    c.hanChotThanhToan.Value.Date.Equals(DateTime.Parse(searchString).Date)).ToList(); break;
                            }

                        default: { break; }
                    }
                }

            }

            //switch (sortOrder)
            //{
            //    case "name_asc": { listClient = listClient.OrderBy(c => c.TEN_KH).ToList(); break; }
            //    case "name_desc": { listClient = listClient.OrderByDescending(c => c.TEN_KH).ToList(); break; }
            //    case "owe_asc": { listClient = listClient.OrderBy(c => c.SOTIENNO).ToList(); break; }
            //    case "owe_desc": { listClient = listClient.OrderByDescending(c => c.SOTIENNO).ToList(); break; }
            //    default: { break; }
            //}
            ///
            /// set up page size and page number
            ///
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            PhieuBanXeViewModel phieuBanXeViewModel = new PhieuBanXeViewModel();
            phieuBanXeViewModel.listOfPHIEUBANXEs = PHIEUBANXEsDataTableModels.ToPagedList(pageNumber, pageSize);

            // tra ve view co chua view model trong no
            return View(phieuBanXeViewModel);
        }

        [HttpGet]
        public ActionResult NhapPhieuBanXe()
        {
            // dau tien, load tat ca danh sach xe co trong GARA co nhu cau ban (HINHTHUC == true)
            // sau do load danh sach khach hang de nhan vien co the chon neu do la khach quan trong GARA
            PhieuBanXeViewModel phieuBanXeViewModel = new PhieuBanXeViewModel();
            phieuBanXeViewModel.listOfXes = this.service.XEs.ToList().Where(e => e.HINHTHUC == true).ToList();
            phieuBanXeViewModel.listOfKhachHang = this.service.KHACHHANGs.ToList();
            return View(phieuBanXeViewModel);
        }

        [HttpPost]
        public ActionResult NhapPhieuBanXe(PHIEU_BANXE phieuBanXeMoi)
        {
            //start to save new information of the new verhical sale of bill into datbase
            // firstly, update the information of staff that create the bill via session the application
            phieuBanXeMoi.MaNV = 1; //this.service.NHANVIENs.Single(e => e.USERNAME.Equals(Session["Username"])).MA_NV;
            // insert into database
            this.service.PHIEU_BANXE.Add(phieuBanXeMoi);
            this.service.SaveChanges();
            return Redirect("Index"); // after the opertation completes, redirect to the list screen
        }
    }
}