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

        private GARADBEntities service = new GARADBEntities(); // entity service object to manipulate data

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
                temp.hanBaoHanh = item.HANBAOHANH;
                if (this.service.PHIEU_THUTIEN.Where(e => e.ID_PHIEUBANXE == temp.id).ToList().Count > 0)
                {
                    temp.isPaid = true;
                }
                else
                {
                    temp.isPaid = false;
                }
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
            // load danh sach xe ban trong database (chi load danh sach xe ban chua duoc ban)
            phieuBanXeViewModel.listOfXes = this.service.XEs.ToList().Where(e => e.HINHTHUC == true).ToList();
            foreach (XE xe in phieuBanXeViewModel.listOfXes)
            {
                if (this.service.PHIEU_BANXE.Where(e => e.BS_XE == xe.BS_XE).Count() > 0)
                {
                    // xoa xe nay ra khoi danh sach neu xe nay da duoc ban 
                    phieuBanXeViewModel.listOfXes.Remove(xe);
                }
            }
            phieuBanXeViewModel.listOfKhachHang = this.service.KHACHHANGs.ToList();
            return View(phieuBanXeViewModel);
        }

        /// <summary>
        /// Save a new verhical new sale recepit into database. 
        /// </summary>
        /// <param name="phieuBanXeMoi"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult NhapPhieuBanXe(PHIEU_BANXE phieuBanXeMoi)
        {
            //start to save new information of the new verhical sale of bill into datbase
            // firstly, update the information of staff that create the bill via session the application
            phieuBanXeMoi.MaNV = 1; //this.service.NHANVIENs.Single(e => e.USERNAME.Equals(Session["Username"])).MA_NV;
            // insert into database
            this.service.PHIEU_BANXE.Add(phieuBanXeMoi);
            this.service.SaveChanges();
            // tien hanh lap phieu thu
            // update so tien no neu khach hang la khach quen
            if (phieuBanXeMoi.MAKH != null)
            {
                // tim thong tin khach hang theo id
                KHACHHANG customer = this.service.KHACHHANGs.Where(e => e.MA_KH == phieuBanXeMoi.MAKH).FirstOrDefault();
                customer.SOTIENNO += phieuBanXeMoi.SOTIENCONLAI;
                this.service.Entry(customer).State = System.Data.Entity.EntityState.Modified;
                this.service.SaveChanges();
            }

            return RedirectToAction("Index"); // after the opertation completes, redirect to the list screen
        }


        /// <summary>
        /// Delete a verhical sale of bill from database. 
        /// </summary>
        /// <param name="billId"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Xoa(int? billId)
        {
                PHIEU_BANXE phieuBanXe = this.service.PHIEU_BANXE.Where(e => e.ID_PHIEUBANXE == billId).FirstOrDefault();
                // after removing the verhical sale of bill => remove the correspoding receipt that is related to it
                PHIEU_THUTIEN correspondingReceiptInformation = this.service.PHIEU_THUTIEN.Where(e => e.ID_PHIEUBANXE == billId).FirstOrDefault();
                if (correspondingReceiptInformation != null)
                {
                    this.service.PHIEU_THUTIEN.Remove(correspondingReceiptInformation);
                    this.service.SaveChanges();
                }
                this.service.PHIEU_BANXE.Remove(phieuBanXe);
                this.service.SaveChanges();
                // tra ve 
                TempData["msg"] = @"<div id=""rowSuccess"" class=""row""> <div class=""col-sm-10""> <div class=""alert alert-success alert-dismissable fade in"" style=""padding-top: 5px; padding-bottom: 5px""> <a href=""#"" class=""close"" data-dismiss=""alert"" aria-label=""close"">&times;</a> Xoá thành công! </div> </div> </div>";
                return Json(new { value = "1", message = "Xóa thành công" }, JsonRequestBehavior.AllowGet);
       
        }

        /// <summary>
        /// Prepare View from updating a existing verhical sale of bill
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult SuaPhieuBanXe(int? id)
        {
            //int billId = Int32.Parse(Request.Params["billId"]);
            PHIEU_BANXE bill = this.service.PHIEU_BANXE.Single(e => e.ID_PHIEUBANXE == id); // find bill via id
            PhieuBanXeViewModel model = new PhieuBanXeViewModel();
            model.selectedPHIEUBANXE = bill; // information of the desired sale of bill
            model.listOfKhachHang = this.service.KHACHHANGs.ToList(); // list of all customers
            return View(model);
        }

        /// <summary>
        /// Update information of existing verhical sale of bill
        /// </summary>
        /// <param name="modifiedBillInformation"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SuaPhieuBanXe(PHIEU_BANXE modifiedBillInformation)
        {
            PHIEU_BANXE updatingBillInfor = this.service.PHIEU_BANXE.Single(e => e.ID_PHIEUBANXE == modifiedBillInformation.ID_PHIEUBANXE); // get the bill from database
            // so tien con lai cu cua phieu
            decimal? soTienConLaiCu = updatingBillInfor.SOTIENCONLAI;
            // start to update information (note : only update the non-readonly attribute from view)
            updatingBillInfor.HANBAOHANH = modifiedBillInformation.HANBAOHANH;
            updatingBillInfor.HANCHOTTHANHTOAN = modifiedBillInformation.HANCHOTTHANHTOAN;
            updatingBillInfor.MAKH = modifiedBillInformation.MAKH;
            updatingBillInfor.SOTIENCONLAI = modifiedBillInformation.SOTIENCONLAI;

            this.service.Entry(updatingBillInfor).State = System.Data.Entity.EntityState.Modified; // 
            this.service.SaveChanges(); // synchronize database

            if (updatingBillInfor.MAKH != null)
            {
                // update lai thong tin tien no cua khach hang neu la khach quen
                KHACHHANG customer = this.service.KHACHHANGs.Where(e => e.MA_KH == updatingBillInfor.MAKH).FirstOrDefault();
                customer.SOTIENNO -= soTienConLaiCu; // khong quan tam den so tien con lai cu
                customer.SOTIENNO += modifiedBillInformation.SOTIENCONLAI; // update lai so tien con lai moi
                this.service.Entry(customer).State = System.Data.Entity.EntityState.Modified;
                this.service.SaveChanges();
            }
            // tro lai man hinh danh sach
            return RedirectToAction("Index");
        }
    }
}