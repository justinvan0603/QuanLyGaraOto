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
    public class XeController : Controller
    {
        private GARADBEntities service = new GARADBEntities();
        // GET: Xe
        /// <summary>
        /// Load danh sach xe trong database
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? searchOption, int? page)
        {
            ViewBag.CurrentSearchOption = searchOption;
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParam = sortOrder == "name_asc" ? "name_desc" : "name_asc";
            ViewBag.OweSortParam = sortOrder == "owe_asc" ? "owe_desc" : "owe_asc";
            List<XE> listOfXe = this.service.XEs.ToList(); // lay tat ca record trong database
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
            switch (searchOption)
            {
                case 0: { break; }
                case 1:
                    {// loc theo bien so xe
                        if (!String.IsNullOrEmpty(searchString))
                        {
                            listOfXe = listOfXe.Where(e => e.BS_XE.ToLower().Contains(searchString.ToLower())).ToList();
                        }
                        break;
                    }
                case 2:
                    {
                        // loc theo hieu xe
                        if (!String.IsNullOrEmpty(searchString))
                        {
                            listOfXe = listOfXe.Where(e => e.HIEU_XE.ToLower().Contains(searchString.ToLower())).ToList();
                        }
                        break;
                    }
                case 3:
                    {
                        // loc theo xe ban
                        listOfXe = listOfXe.Where(e => e.HINHTHUC == true).ToList();
                        break;
                    }
                case 4:
                    {
                        // loc theo xe sua chua
                        listOfXe = listOfXe.Where(e => e.HINHTHUC == false).ToList();
                        break;
                    }

                default: { break; }
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
            XeViewModel viewModel = new XeViewModel();
            viewModel.danhSachXe = listOfXe.ToPagedList(pageNumber, pageSize);

            // tra ve view co chua view model trong no
            return View(viewModel);
        }

        /// <summary>
        /// Nhap thong tin xe moi
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult NhapXe()
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
            XeViewModel viewModel = new XeViewModel();
            viewModel.danhSachHieuXe = this.service.HIEUXEs.ToList();
            viewModel.danhSachKhachHang = this.service.KHACHHANGs.ToList();
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult NhapXe(XE thongTinXeMoi)
        {
            //
            // to code here
            //
            // kiem tra xem da co xe voi bien so xe moi da ton tai trong database hay khong
            if (this.service.XEs.Where(e => e.BS_XE == thongTinXeMoi.BS_XE).ToList().Count > 0)
            {
                return View("DuplicatedVehicleExceptionView");
            }
            thongTinXeMoi.BS_XE = thongTinXeMoi.BS_XE.ToUpper(); // in hoa ki tu bien so xe
            thongTinXeMoi.HINHTHUC = false; // xe sua tu khach hang
            this.service.XEs.Add(thongTinXeMoi);
            this.service.SaveChanges();
            TempData["msg"] = @"<div id=""rowSuccess"" class=""row""> <div class=""col-sm-10""> <div class=""alert alert-success alert-dismissable fade in"" style=""padding-top: 5px; padding-bottom: 5px""> <a href=""#"" class=""close"" data-dismiss=""alert"" aria-label=""close"">&times;</a> Thêm mới thành công! </div> </div> </div>";
            // tro ve trang danh sach
            return Redirect("Index");
        }

        /// <summary>
        /// Load thong tin xe can sua len view
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult SuaThongTinXe(String bienSoXe)
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
            XeViewModel viewModel = new XeViewModel();
            XE selectedXe = this.service.XEs.Where(e => e.BS_XE == bienSoXe).Single();
            if (selectedXe == null)
            {
                return View("ResourceNotFoundView");
            }
            else
            {
                viewModel.selectedXe = selectedXe;
                // load danh sach hieu xe
                viewModel.danhSachHieuXe = this.service.HIEUXEs.ToList();
                return View(viewModel);
            }
        }

        [HttpPost]
        public ActionResult SuaThongTinXe(XE arg)
        {
            // luu thong tin sua doi vao trong database
            XE thongTinXeMoi = this.service.XEs.Where(e => e.BS_XE == arg.BS_XE).Single();
            // chi update nhung thong tin can sua
            thongTinXeMoi.HIEU_XE = arg.HIEU_XE;
            thongTinXeMoi.SO_KHUNG = arg.SO_KHUNG;
            thongTinXeMoi.SO_KM = arg.SO_KM;
            thongTinXeMoi.SO_MAY = arg.SO_MAY;
            thongTinXeMoi.TINH_TRANG = arg.TINH_TRANG;
            thongTinXeMoi.DOI_XE = arg.DOI_XE;
            this.service.Entry(thongTinXeMoi).State = System.Data.Entity.EntityState.Modified;
            this.service.SaveChanges();
            TempData["msg"] = @"<div id=""rowSuccess"" class=""row""> <div class=""col-sm-10""> <div class=""alert alert-success alert-dismissable fade in"" style=""padding-top: 5px; padding-bottom: 5px""> <a href=""#"" class=""close"" data-dismiss=""alert"" aria-label=""close"">&times;</a> Cập nhật thành công! </div> </div> </div>";
            return RedirectToAction("Index");
        }


        [HttpGet]
        public ActionResult ResourceNotFoundView()
        {
            return View();
        }

        [HttpGet]
        public ActionResult DuplicatedVehicleExceptionView()
        {
            return View();
        }
    }

}