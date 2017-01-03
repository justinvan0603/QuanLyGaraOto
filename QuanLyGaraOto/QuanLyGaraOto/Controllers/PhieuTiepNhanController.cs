using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QuanLyGaraOto.ViewModel;
using QuanLyGaraOto.Models;
using PagedList;
using Rotativa.Options;

namespace QuanLyGaraOto.Controllers
{
    public class PhieuTiepNhanController : Controller
    {
        readonly static string FILE_NAME_PREFIX = "phieutiepnhan_";
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
                                try
                                {
                                    listViewModels = listViewModels.Where(c =>
                                        c.ngayLap.Value.Date.Equals(DateTime.Parse(searchString).Date)).ToList();
                                }
                                catch (Exception e)
                                {

                                }
                                break;
                            }
                        case 2: // loc theo ngay lap hen tra xe
                            {
                                try
                                {
                                    listViewModels = listViewModels.Where(c =>
                                        c.ngayTra.Value.Date.Equals(DateTime.Parse(searchString).Date)).ToList();
                                }
                                catch (Exception e)
                                {
                                       
                                }
                                break;
                            } 
                        case 3: // loc theo ma phieu
                            {
                                listViewModels = listViewModels.Where(e => e.maPhieuTiepNhan == int.Parse(searchString)).ToList();
                                break;
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
            viewModel.service = this.service;
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
            viewModel.selectedPhieuTiepNhan = this.service.PHIEU_TIEPNHAN.Where(e => e.MA_PHIEUTIEPNHAN == maPhieu).FirstOrDefault();
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


        [HttpGet]
        public ActionResult ThemPhieuTiepNhan()
        {
            BANGTHAMSO SL_TIEPNHAN_TOIDA = this.service.BANGTHAMSOes.Where(e => e.TENTHAMSO == "SoXeTiepNhanToiDa").SingleOrDefault();

            // kiem tra so luong tiep nhan toi da trong ngay
            List<PHIEU_TIEPNHAN> temp = this.service.PHIEU_TIEPNHAN.ToList();
            if (temp.Where(e => e.NGAYLAP.Value.Date.Equals(DateTime.Now.Date)).Count() >= int.Parse(SL_TIEPNHAN_TOIDA.GIATRI))
            {
                TempData["msg"] = @"<div id=""rowError"" class=""row""> <div class=""col-sm-10""> <div class=""alert alert-danger alert-dismissable fade in"" style=""padding-top: 5px; padding-bottom: 5px""> <a href=""#"" class=""close"" data-dismiss=""alert"" aria-label=""close"">&times;</a> Quá số lượng tiếp nhận xe tối đa trong ngày ! </div> </div> </div>";
                return RedirectToAction("Index", new { sortOrder = String.Empty, currentFilter = String.Empty, searchString = String.Empty });
            }
            PhieuTiepNhanViewModel viewModel = new PhieuTiepNhanViewModel();
            viewModel.thongTinPhieuMoi = new ThemPhieuTiepNhanModel();
            viewModel.danhSachHieuXe = this.service.HIEUXEs.ToList();
            return View(viewModel);
        }


        [HttpPost]
        public ActionResult ThemPhieuTiepNhan(ThemPhieuTiepNhanModel infor)
        {
            // kiem tra du lieu xe da co trong database hay chua
            if (this.service.XEs.Where(e => e.BS_XE == infor.bienSoXe).Count() > 0)
            {
                return View("DuplicatedVehicleExceptionView");
            }

            if (ModelState.IsValid)
            {

                // luu thong tin khach hang va xe
                var khachHang = new KHACHHANG();
                khachHang.TEN_KH = infor.tenKhachHang;
                khachHang.SDT = infor.dienThoai;
                khachHang.DIACHI = infor.diaChi;
                khachHang.CMND = infor.soCmnd;
                // luu thong tin khach hang
                this.service.KHACHHANGs.Add(khachHang);
                this.service.SaveChanges();

                // thong tin xe
                var xe = new XE();
                xe.BS_XE = infor.bienSoXe;
                xe.HIEU_XE = infor.hieuXe;
                xe.MA_KH = khachHang.MA_KH;
                xe.HINHTHUC = infor.hinhThuc;
                xe.DOI_XE = infor.doiXe;
                xe.NGAY_TIEPNHAN = infor.ngayTiepNhan;
                xe.SO_KHUNG = infor.soKhung;
                xe.SO_KM = infor.soKm;
                xe.SO_MAY = infor.soMay;
                xe.TINH_TRANG = infor.tinhTrang;



                // lap phieu tiep nhan
                var phieuTiepNhan = new PHIEU_TIEPNHAN();
                phieuTiepNhan.BIENSO_XE = infor.bienSoXe;
                phieuTiepNhan.MA_KH = khachHang.MA_KH;
                phieuTiepNhan.MA_NV = int.Parse(Session["UserID"].ToString());
                phieuTiepNhan.NGAYLAP = infor.ngayTiepNhan;
                phieuTiepNhan.TINHTRANG = infor.tinhTrang;
                phieuTiepNhan.MASOCHO = this.service.PHIEU_TIEPNHAN.Count() + 1; // update ma so cho
                phieuTiepNhan.NGAYTRAXE = DateTime.Now; // mac dinh ngay tra xe sau khi tiep nhan la ngay hien tai



                this.service.XEs.Add(xe);
                this.service.SaveChanges();
                
                this.service.PHIEU_TIEPNHAN.Add(phieuTiepNhan);
                this.service.SaveChanges();

                // tro ve man hinh danh sach
                return RedirectToAction("Index");
            }
            else
            {
                PhieuTiepNhanViewModel viewModel = new PhieuTiepNhanViewModel();

                viewModel.thongTinPhieuMoi = infor;
                viewModel.danhSachHieuXe = this.service.HIEUXEs.ToList();
                return View(viewModel);
            }

        }

        /// <summary>
        /// Them phieu tiep nhan tu thong tin khach quen
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ThemPhieuTiepNhanTuKhachQuen()
        {
            BANGTHAMSO SL_TIEPNHAN_TOIDA = this.service.BANGTHAMSOes.Where(e=> e.TENTHAMSO == "SoXeTiepNhanToiDa").SingleOrDefault();

            // kiem tra so luong tiep nhan toi da trong ngay
            List<PHIEU_TIEPNHAN> temp = this.service.PHIEU_TIEPNHAN.ToList();
            if (temp.Where(e => e.NGAYLAP.Value.Date.Equals(DateTime.Now.Date)).Count() >= int.Parse(SL_TIEPNHAN_TOIDA.GIATRI))
            {
                TempData["msg"] = @"<div id=""rowError"" class=""row""> <div class=""col-sm-10""> <div class=""alert alert-danger alert-dismissable fade in"" style=""padding-top: 5px; padding-bottom: 5px""> <a href=""#"" class=""close"" data-dismiss=""alert"" aria-label=""close"">&times;</a> Quá số lượng tiếp nhận xe tối đa trong ngày ! </div> </div> </div>";
                return RedirectToAction("Index", new { sortOrder = String.Empty, currentFilter = String.Empty, searchString = String.Empty });
            }
            PhieuTiepNhanViewModel viewModel = new PhieuTiepNhanViewModel();
            viewModel.thongTinPhieuMoi = new ThemPhieuTiepNhanModel();
            viewModel.thongTinPhieuMoiTuKhachQuen = new ThemPhieuTiepNhanTuKhachQuenModel();
            // load danh sach khach hang
            viewModel.danhSachKhachHang = this.service.KHACHHANGs.ToList();
            viewModel.danhsachXe = this.service.XEs.ToList();
            return View(viewModel);
        }

        /// <summary>
        /// loc danh sach xe theo ma khach hang va tra ve View
        /// </summary>
        /// <param name="customerId">Id cua khach hang ma nguoi dung chon tren View</param>
        /// <returns>Danh sach ung voi id khach hang</returns>
        [HttpPost]
        public ActionResult getVehiclesByCustomerId(int customerId)
        {
            var listOfVehicles = this.service.XEs.Where(e => e.MA_KH == customerId && e.HINHTHUC == false).Select(pt => "<option value='"
                + pt.BS_XE + "' title='" +
                pt.BS_XE + "' id='" + pt.BS_XE + "' itemscope='" + pt.BS_XE + "' itemprop='" +
                pt.BS_XE + "'>" + pt.BS_XE + "</option>");
            return Content(String.Join("", listOfVehicles));
        }


        [HttpPost]
        public ActionResult ThemPhieuTiepNhanTuKhachQuen(ThemPhieuTiepNhanTuKhachQuenModel infor)
        {

            // tien hanh phieu phieu tiep nhan
            PHIEU_TIEPNHAN PhieuTiepNhan = new PHIEU_TIEPNHAN();
            PhieuTiepNhan.NGAYLAP = infor.ngayLap;
            PhieuTiepNhan.BIENSO_XE = infor.bienSoXe;
            PhieuTiepNhan.MA_KH = infor.maKhachHang;
            PhieuTiepNhan.MA_NV = int.Parse(Session["UserID"].ToString());
            PhieuTiepNhan.TINHTRANG = infor.tinhTrang;
            PhieuTiepNhan.MASOCHO = this.service.PHIEU_TIEPNHAN.Count() + 1;
            PhieuTiepNhan.NGAYTRAXE = DateTime.Now; // mac dinh la thoi gian hien tai.
            this.service.PHIEU_TIEPNHAN.Add(PhieuTiepNhan);
            this.service.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult DuplicatedVehicleExceptionView()
        {
            return View();
        }


        /// <summary>
        /// Xoa phieu tiep nhan 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Xoa(int id)
        {
            try
            {
                PHIEU_TIEPNHAN phieuTiepNhan = this.service.PHIEU_TIEPNHAN.Where(e => e.MA_PHIEUTIEPNHAN == id).FirstOrDefault();
                this.service.PHIEU_TIEPNHAN.Remove(phieuTiepNhan);
                this.service.SaveChanges();
                // tra ve 
                return Json(new { value = "1", message = "Xóa thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {

                return Json(new { value = "-1", message = "SYSTEM ERROR !" }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Xuat thong tin mot phieu tiep nhan duoi dang pdf format.
        /// </summary>
        /// <param name="id"> id cua phieu tiep nhan duoc in</param>
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
            PHIEU_TIEPNHAN phieuTiepNhan = this.service.PHIEU_TIEPNHAN.Where(e => e.MA_PHIEUTIEPNHAN == id).SingleOrDefault();
            PhieuTiepNhanViewModel viewModel = new PhieuTiepNhanViewModel();
            viewModel.selectedPhieuTiepNhan = phieuTiepNhan;
            string fileName = FILE_NAME_PREFIX + phieuTiepNhan.MA_PHIEUTIEPNHAN + ".pdf";
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