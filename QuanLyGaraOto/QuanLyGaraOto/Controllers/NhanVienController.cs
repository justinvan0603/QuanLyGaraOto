using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QuanLyGaraOto.Models;
using PagedList;
using QuanLyGaraOto.ViewModel;
using QuanLyGaraOto.App_Start;
namespace QuanLyGaraOto.Controllers
{
    public class NhanVienController : Controller
    {
        // GET: NhanVien
        public ActionResult Index(string currentFilter, string searchString, int? searchOption, int? page)
        {
            
            ViewBag.CurrentSearchOption = searchOption;
            GARADBEntities context = new GARADBEntities();
            var dsNhanVienQuery = (from nv in context.NHANVIENs
                                   join nnd in context.NHOMNGUOIDUNGs on nv.MA_NHOMNGUOIDUNG equals nnd.MA_NHOMNGUOIDUNG
                                  select new
                                  {
                                      MaNV = nv.MA_NV,
                                      Username = nv.USERNAME,
                                      HoTen = nv.HOTEN,
                                      GioiTinh = nv.GIOITINH,
                                      SDT = nv.SDT,
                                      DiaChi = nv.DIACHI,
                                      MaNhomNguoiDung = nv.MA_NHOMNGUOIDUNG,
                                      NhomNguoiDung = nnd.TEN_NHOM,
                                      //TinhTrang = pdv.TINHTRANG
                                  }).ToList();
            List<NhanVienTableData> data = new List<NhanVienTableData>();
            foreach(var item in dsNhanVienQuery)
            {
                NhanVienTableData row = new NhanVienTableData();
                row.MaNV = item.MaNV;
                row.HoTen = item.HoTen;
                row.Username = item.Username;
                row.SDT = item.SDT;
                row.GioiTinh = item.GioiTinh.Value;
                row.MaNhomNguoiDung = item.MaNhomNguoiDung.Value;
                row.NhomNguoiDung = item.NhomNguoiDung;
                row.DiaChi = item.DiaChi;
                data.Add(row);
                
            }

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
                        case 1: { data = data.Where(nv => nv.HoTen.Contains(searchString)).ToList(); break; }
                        case 2: { data = data.Where(nv => nv.Username.Contains(searchString)).ToList(); break; }
                        case 3: { data = data.Where(nv => nv.SDT.Contains(searchString)).ToList(); break; }
                    }
                }
            }
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            NhanVienViewModel vmNhanVien = new NhanVienViewModel();
            vmNhanVien.ListNhanVien = data.ToPagedList(pageNumber, pageSize);

            return View(vmNhanVien);
        }
        [HttpGet]
        public ActionResult SuaNhanVien(int? id)
        {

            GARADBEntities context = new GARADBEntities();
            int UserId = int.Parse(Session["UserID"].ToString());
            NHANVIEN st = context.NHANVIENs.Single(staff => staff.MA_NV == UserId);
            NHOMNGUOIDUNG groupuser = context.NHOMNGUOIDUNGs.Single(gu => gu.MA_NHOMNGUOIDUNG == st.MA_NHOMNGUOIDUNG.Value);
            if (groupuser.CAPDO != 2 && groupuser.CAPDO != 1)
            {
                TempData["msg"] = @"<div id=""rowError"" class=""row""> <div class=""col-sm-10""> <div class=""alert alert-danger alert-dismissable fade in"" style=""padding-top: 5px; padding-bottom: 5px""> <a href=""#"" class=""close"" data-dismiss=""alert"" aria-label=""close"">&times;</a> Bạn không có quyền truy cập vào chức năng này! </div> </div> </div>";
                return RedirectToAction("Index", new { currentFilter = String.Empty, searchString = String.Empty });
            }
            SuaNhanVienViewModel vmNhanVien = new SuaNhanVienViewModel();
            vmNhanVien.NhanVien = context.NHANVIENs.Single(nv => nv.MA_NV == id.Value);
            vmNhanVien.ListNhomNguoiDung = context.NHOMNGUOIDUNGs.ToList();
            return View(vmNhanVien);

        }
        [HttpPost]
        public ActionResult SuaNhanVien(NHANVIEN nv)
        {
            GARADBEntities context = new GARADBEntities();
            if (ModelState.IsValid)
            {
                var target = context.NHANVIENs.Single(st => st.MA_NV == nv.MA_NV);
                target.HOTEN = nv.HOTEN;
                target.PASSWORD = MD5Encryptor.MD5Hash(nv.PASSWORD);
                target.MA_NHOMNGUOIDUNG = nv.MA_NHOMNGUOIDUNG;
                target.SDT = nv.SDT;
                target.DIACHI = nv.DIACHI;
                target.GIOITINH = nv.GIOITINH;
                context.SaveChanges();
                TempData["msg"] = @"<div id=""rowSuccess"" class=""row""> <div class=""col-sm-10""> <div class=""alert alert-success alert-dismissable fade in"" style=""padding-top: 5px; padding-bottom: 5px""> <a href=""#"" class=""close"" data-dismiss=""alert"" aria-label=""close"">&times;</a> Cập nhật tài khoản thành công! </div> </div> </div>";
                //TempData["msg"] = "<script>alert('Sửa tài khoản thành công!');</script>";
                return RedirectToAction("Index", new { currentFilter = String.Empty, searchString = String.Empty });
            }
            else
            {
                SuaNhanVienViewModel vmNhanVien = new SuaNhanVienViewModel();
                vmNhanVien.NhanVien = nv;
                vmNhanVien.ListNhomNguoiDung = context.NHOMNGUOIDUNGs.ToList();
                return View(vmNhanVien);
            }
        }
        [HttpGet]
        public ActionResult NhapNhanVien()
        {
            List<NHOMNGUOIDUNG> ListNhomNguoiDung = new List<NHOMNGUOIDUNG>();
            GARADBEntities context = new GARADBEntities();

            int UserId = int.Parse(Session["UserID"].ToString());
            NHANVIEN st = context.NHANVIENs.Single(staff => staff.MA_NV == UserId);
            NHOMNGUOIDUNG groupuser = context.NHOMNGUOIDUNGs.Single(gu => gu.MA_NHOMNGUOIDUNG == st.MA_NHOMNGUOIDUNG.Value);
            if (groupuser.CAPDO != 2 && groupuser.CAPDO != 1)
            {
                TempData["msg"] = @"<div id=""rowError"" class=""row""> <div class=""col-sm-10""> <div class=""alert alert-danger alert-dismissable fade in"" style=""padding-top: 5px; padding-bottom: 5px""> <a href=""#"" class=""close"" data-dismiss=""alert"" aria-label=""close"">&times;</a> Bạn không có quyền truy cập vào chức năng này! </div> </div> </div>";
                return RedirectToAction("Index", new { currentFilter = String.Empty, searchString = String.Empty });
            }
            ListNhomNguoiDung = context.NHOMNGUOIDUNGs.ToList();
            ViewBag.ListNhomNguoiDung = context.NHOMNGUOIDUNGs.ToList();
            return View();
        }
        [HttpPost]
        public ActionResult NhapNhanVien(NHANVIEN nv)
        {
            GARADBEntities context = new GARADBEntities();
            if (ModelState.IsValid)
            {
                if (context.NHANVIENs.Any(st => st.USERNAME.Equals(nv.USERNAME)))
                {
                    TempData["msg"] = @"<div id=""rowError"" class=""row""> <div class=""col-sm-10""> <div class=""alert alert-danger alert-dismissable fade in"" style=""padding-top: 5px; padding-bottom: 5px""> <a href=""#"" class=""close"" data-dismiss=""alert"" aria-label=""close"">&times;</a> Username đã tồn tại trong hệ thống! </div> </div> </div>";
                    //TempData["msg"] = "<script>alert('Username đã tồn tại trong hệ thống!');</script>";
                    ViewBag.ListNhomNguoiDung = context.NHOMNGUOIDUNGs.ToList();
                    return View();
                }
                else
                {
                    string pw = nv.PASSWORD;
                    nv.PASSWORD = MD5Encryptor.MD5Hash(pw);
                    context.NHANVIENs.Add(nv);
                    context.SaveChanges();
                    TempData["msg"] = @"<div id=""rowSuccess"" class=""row""> <div class=""col-sm-10""> <div class=""alert alert-success alert-dismissable fade in"" style=""padding-top: 5px; padding-bottom: 5px""> <a href=""#"" class=""close"" data-dismiss=""alert"" aria-label=""close"">&times;</a> Thêm tài khoản thành công! </div> </div> </div>";
                    //TempData["msg"] = "<script>alert('Thêm tài khoản thành công!');</script>";
                    //ViewBag.ListNhomNguoiDung = context.NHOMNGUOIDUNGs.ToList();
                    return RedirectToAction("Index", new { currentFilter = String.Empty, searchString = String.Empty });
                }
            }
            else
            {
                ViewBag.ListNhomNguoiDung = context.NHOMNGUOIDUNGs.ToList();
                return View();
            }
        }
        public JsonResult Xoa(int id)
        {
            try
            {
                
                GARADBEntities context = new GARADBEntities();
                int UserId = int.Parse(Session["UserID"].ToString());
                NHANVIEN st = context.NHANVIENs.Single(staff => staff.MA_NV == UserId);
                NHOMNGUOIDUNG groupuser = context.NHOMNGUOIDUNGs.Single(gu => gu.MA_NHOMNGUOIDUNG == st.MA_NHOMNGUOIDUNG.Value);
                if (groupuser.CAPDO != 2 && groupuser.CAPDO != 1)
                {
                    TempData["msg"] = @"<div id=""rowError"" class=""row""> <div class=""col-sm-10""> <div class=""alert alert-danger alert-dismissable fade in"" style=""padding-top: 5px; padding-bottom: 5px""> <a href=""#"" class=""close"" data-dismiss=""alert"" aria-label=""close"">&times;</a> Bạn không có quyền truy cập vào chức năng này! </div> </div> </div>";
                    return Json(new { value = "-1", message = "Bạn không có quyền thực hiện thao tác này!" }, JsonRequestBehavior.AllowGet);
                }
                var target = context.NHANVIENs.Single(s => s.MA_NV == id);
                if (!target.USERNAME.Equals(Session["Username"]))
                {
                    context.NHANVIENs.Remove(target);
                    context.SaveChanges();
                    TempData["msg"] = @"<div id=""rowSuccess"" class=""row""> <div class=""col-sm-10""> <div class=""alert alert-success alert-dismissable fade in"" style=""padding-top: 5px; padding-bottom: 5px""> <a href=""#"" class=""close"" data-dismiss=""alert"" aria-label=""close"">&times;</a> Xóa tài khoản thành công! </div> </div> </div>";
                    return Json(new { value = "1", message = "Xóa thành công" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    TempData["msg"] = @"<div id=""rowError"" class=""row""> <div class=""col-sm-10""> <div class=""alert alert-success alert-dismissable fade in"" style=""padding-top: 5px; padding-bottom: 5px""> <a href=""#"" class=""close"" data-dismiss=""alert"" aria-label=""close"">&times;</a> Không thể xóa tài khoản d8ang đăng nhập! </div> </div> </div>";
                    return Json(new { value = "-1", message = "Không thể xóa tài khoản đang đăng nhập!" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch(Exception)
            {
                TempData["msg"] = @"<div id=""rowError"" class=""row""> <div class=""col-sm-10""> <div class=""alert alert-success alert-dismissable fade in"" style=""padding-top: 5px; padding-bottom: 5px""> <a href=""#"" class=""close"" data-dismiss=""alert"" aria-label=""close"">&times;</a> Không thể xóa tài khoản đã sử dụng! </div> </div> </div>";
                return Json(new { value = "-1", message = "Không thể xóa do đã có tham chiếu" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}