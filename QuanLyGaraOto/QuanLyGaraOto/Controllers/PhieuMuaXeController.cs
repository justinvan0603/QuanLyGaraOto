using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QuanLyGaraOto.Models;
using QuanLyGaraOto.ViewModel;
using PagedList;
using Rotativa.Options;

namespace QuanLyGaraOto.Controllers
{
    public class PhieuMuaXeController : Controller
    {
        private readonly static string FILE_NAME_PREFIX = "phieumuaxe_";
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
                else
                {
                    temp.khachHang = "Khách vãng lai";
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
                        case 2: { danhSachPhieuMuaXe = danhSachPhieuMuaXe.Where(e => e.maPhieuMua.Contains(searchString)).ToList(); break; }
                        case 1:
                            {
                                try
                                {
                                    danhSachPhieuMuaXe = danhSachPhieuMuaXe.Where(c =>
                                        c.ngayLapPhieu.Value.Date.Equals(DateTime.Parse(searchString).Date)).ToList();
                                }
                                catch (Exception e)
                                {
                                }
                                break;
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
            int UserId = int.Parse(Session["UserID"].ToString());
            NHANVIEN st = this.service.NHANVIENs.Single(staff => staff.MA_NV == UserId);
            NHOMNGUOIDUNG groupuser = this.service.NHOMNGUOIDUNGs.Single(gu => gu.MA_NHOMNGUOIDUNG == st.MA_NHOMNGUOIDUNG.Value);
            // kiem tra quyen han truoc khi xu ly, phieu chi chi lien quan voi nhan vien cham soc khach hang va super user 
            if (groupuser.CAPDO != 2 && groupuser.CAPDO != 4)
            {
                TempData["msg"] = @"<div id=""rowError"" class=""row""> <div class=""col-sm-10""> <div class=""alert alert-danger alert-dismissable fade in"" style=""padding-top: 5px; padding-bottom: 5px""> <a href=""#"" class=""close"" data-dismiss=""alert"" aria-label=""close"">&times;</a> Bạn không có quyền truy cập vào chức năng này! </div> </div> </div>";
                return RedirectToAction("Index", new { sortOrder = String.Empty, currentFilter = String.Empty, searchString = String.Empty });
            }
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
            phieuMuaXeMoi.BS_XE = arg.bienSoXe;
            phieuMuaXeMoi.MAKH = arg.maKhachHang;
            phieuMuaXeMoi.MaNV = int.Parse(Session["UserID"].ToString());
            phieuMuaXeMoi.TRIGIA = arg.tongGiaTri;
            

            // tien hanh luu
            this.service.XEs.Add(xeMoi);
            this.service.PHIEU_MUAXE.Add(phieuMuaXeMoi);
            this.service.SaveChanges();
            return Redirect("Index");
        }




        /// <summary>
        /// Load thong thong tin phieu mua can sua doi len view client
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult SuaPhieuMuaXe(int id)
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
            // lay thong tin phieu mua xe tu database
            PHIEU_MUAXE phieuMuaXe = this.service.PHIEU_MUAXE.Where(e => e.ID_PHIEUMUAXE == id).FirstOrDefault();

            if (phieuMuaXe == null)
            {
                return View("ResourceNotFoundView");
            }
            XE xe = this.service.XEs.Where(e => e.BS_XE == phieuMuaXe.BS_XE).FirstOrDefault();
            if (xe == null)
            {
                // ngam xoa phieu vi du lieu xe khong ton tai 
                this.service.PHIEU_MUAXE.Remove(phieuMuaXe);
                this.service.SaveChanges();
                return View("ResourceNotFoundView");
            }
            String tennv = service.NHANVIENs.Single(nvien => nvien.MA_NV == phieuMuaXe.MaNV.Value).HOTEN;
            PhieuNhapMuaXeModel selectededBill = new PhieuNhapMuaXeModel();
            selectededBill.id = phieuMuaXe.ID_PHIEUMUAXE;
            selectededBill.maPhieuMuaXe = phieuMuaXe.MAPHIEUMUA;
            selectededBill.bienSoXe = phieuMuaXe.BS_XE;
            selectededBill.doiXe = xe.DOI_XE;
            selectededBill.hieuXe = xe.HIEU_XE;
            selectededBill.maKhachHang = phieuMuaXe.MAKH;
            selectededBill.maNhanVien = phieuMuaXe.MaNV;
            selectededBill.TenNV = tennv;
            selectededBill.ngayLapPhieu = phieuMuaXe.NGAYLAP;
            selectededBill.soKhung = xe.SO_KHUNG;
            selectededBill.soKm = xe.SO_KM;
            selectededBill.soMay = xe.SO_MAY;
            selectededBill.tinhTrang = xe.TINH_TRANG;
            selectededBill.tongGiaTri = phieuMuaXe.TRIGIA;

            PhieuMuaXeViewModel viewModel = new PhieuMuaXeViewModel();
            viewModel.selectedBill = selectededBill;
            viewModel.danhSachKhachHang = this.service.KHACHHANGs.ToList();
            viewModel.danhSachHieuXe = this.service.HIEUXEs.ToList();

            return View(viewModel);
        }

        /// <summary>
        /// Thuc hien cap nhat thong tin phieu mua xe khi nguoi dung click nut sua
        /// </summary>
        /// <param name="modifiedInfor"> Thong tin da duoc sua doi</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SuaPhieuMuaXe(PhieuNhapMuaXeModel modifiedInfor)
        {
            PHIEU_MUAXE phieuMuaXe = this.service.PHIEU_MUAXE.Single(e => e.ID_PHIEUMUAXE == modifiedInfor.id);
            if (phieuMuaXe == null)
            {
                return View("ResourceNotFoundView");
            }
            XE xe = this.service.XEs.Where(e => e.BS_XE == phieuMuaXe.BS_XE).FirstOrDefault();
            if (xe == null)
            {
                // ngam xoa phieu vi du lieu xe khong ton tai 
                this.service.PHIEU_MUAXE.Remove(phieuMuaXe);
                this.service.SaveChanges();
                return View("ResourceNotFoundView");
            }
            // start to update

            phieuMuaXe.MAKH = modifiedInfor.maKhachHang;
           // phieuMuaXe.MaNV = modifiedInfor.maNhanVien;
            phieuMuaXe.MAPHIEUMUA = modifiedInfor.maPhieuMuaXe;
            phieuMuaXe.NGAYLAP = modifiedInfor.ngayLapPhieu;
            phieuMuaXe.TRIGIA = modifiedInfor.tongGiaTri;

            // xe
            xe.DOI_XE = modifiedInfor.doiXe;
            xe.HIEU_XE = modifiedInfor.hieuXe;
            xe.HINHTHUC = true;
            xe.MA_KH = modifiedInfor.maKhachHang;
            xe.NGAY_TIEPNHAN = modifiedInfor.ngayLapPhieu;
            xe.SO_KHUNG = modifiedInfor.soKhung;
            xe.SO_KM = modifiedInfor.soKm;
            xe.SO_MAY = modifiedInfor.soMay;
            xe.TINH_TRANG = modifiedInfor.tinhTrang;

            this.service.Entry(xe).State = System.Data.Entity.EntityState.Modified; // cap nhat thong tin xe
            this.service.Entry(phieuMuaXe).State = System.Data.Entity.EntityState.Modified; // cap nhat thong tin phieu
            this.service.SaveChanges();
            return RedirectToAction("Index");
        }



        /// <summary>
        /// Xoa thong tin mot phieu mua xe 
        /// </summary>
        /// <param name="billId"> Id phieu mua xe can xoa</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Xoa(int? billId)
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
            try
            {
                PHIEU_MUAXE phieuMuaXe = this.service.PHIEU_MUAXE.Single(e => e.ID_PHIEUMUAXE == billId);
                // lay thong tin xe tu thong tin phieu mua xe
                XE xe = this.service.XEs.Single(e => e.BS_XE == phieuMuaXe.BS_XE);
                /**
                 * Xoa thong tin xe truoc, sau do tien hanh xoa thong tin phieu mua xe
                 * */
                this.service.XEs.Remove(xe);
                this.service.PHIEU_MUAXE.Remove(phieuMuaXe);
                // after removing the verhincal sale of bill => remove the correspoding receipt that is related to it
                this.service.SaveChanges();
                // tra ve 
                TempData["msg"] = @"<div id=""rowSuccess"" class=""row""> <div class=""col-sm-10""> <div class=""alert alert-success alert-dismissable fade in"" style=""padding-top: 5px; padding-bottom: 5px""> <a href=""#"" class=""close"" data-dismiss=""alert"" aria-label=""close"">&times;</a> Xoá thành công! </div> </div> </div>";
                return Json(new { value = "1", message = "Xóa thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {

                return Json(new { value = "-1", message = "SYSTEM ERROR !" }, JsonRequestBehavior.AllowGet);
            }
        }


        /// <summary>
        /// Error screen for duplication data 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult DuplicatedVerhicleExceptionView()
        {
            return View();
        }

        /// <summary>
        /// 404 Resource not found screen
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ResourceNotFoundView()
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
            PHIEU_MUAXE phieuMuaXe = this.service.PHIEU_MUAXE.Where(e => e.ID_PHIEUMUAXE == id).FirstOrDefault();
            if (phieuMuaXe == null)
            {
                return View("ResourceNotFoundView");
            }
            XE xe = this.service.XEs.Where(e => e.BS_XE == phieuMuaXe.BS_XE).FirstOrDefault();
            if (xe == null)
            {
                // ngam xoa phieu vi du lieu xe khong ton tai 
                this.service.PHIEU_MUAXE.Remove(phieuMuaXe);
                this.service.SaveChanges();
                return View("ResourceNotFoundView");
            }
            PhieuNhapMuaXeModel selectededBill = new PhieuNhapMuaXeModel();
            selectededBill.id = phieuMuaXe.ID_PHIEUMUAXE;
            selectededBill.maPhieuMuaXe = phieuMuaXe.MAPHIEUMUA;
            selectededBill.bienSoXe = phieuMuaXe.BS_XE;
            selectededBill.doiXe = xe.DOI_XE;
            selectededBill.hieuXe = xe.HIEU_XE;
            selectededBill.maKhachHang = phieuMuaXe.MAKH;
            selectededBill.maNhanVien = phieuMuaXe.MaNV;
            selectededBill.ngayLapPhieu = phieuMuaXe.NGAYLAP;
            selectededBill.soKhung = xe.SO_KHUNG;
            selectededBill.soKm = xe.SO_KM;
            selectededBill.soMay = xe.SO_MAY;
            selectededBill.tinhTrang = xe.TINH_TRANG;
            selectededBill.tongGiaTri = phieuMuaXe.TRIGIA;

            PhieuMuaXeViewModel viewModel = new PhieuMuaXeViewModel();
            viewModel.selectedBill = selectededBill;
            viewModel.danhSachKhachHang = this.service.KHACHHANGs.ToList();
            viewModel.danhSachHieuXe = this.service.HIEUXEs.ToList();
            string fileName = FILE_NAME_PREFIX + viewModel.selectedBill.id + ".pdf";
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