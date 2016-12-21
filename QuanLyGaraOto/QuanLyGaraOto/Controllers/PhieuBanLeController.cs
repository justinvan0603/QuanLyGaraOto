using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QuanLyGaraOto.Models;
using PagedList;
using QuanLyGaraOto.ViewModel;
using Rotativa.Options;
namespace QuanLyGaraOto.Controllers
{
    public class PhieuBanLeController : Controller
    {
        public ActionResult In(int? id)
        {
            GARADBEntities context = new GARADBEntities();

            int UserId = int.Parse(Session["UserID"].ToString());
            NHANVIEN st = context.NHANVIENs.Single(staff => staff.MA_NV == UserId);
            NHOMNGUOIDUNG groupuser = context.NHOMNGUOIDUNGs.Single(gu => gu.MA_NHOMNGUOIDUNG == st.MA_NHOMNGUOIDUNG.Value);
            if (groupuser.CAPDO != 2 && groupuser.CAPDO != 4)
            {
                TempData["msg"] = @"<div id=""rowError"" class=""row""> <div class=""col-sm-10""> <div class=""alert alert-danger alert-dismissable fade in"" style=""padding-top: 5px; padding-bottom: 5px""> <a href=""#"" class=""close"" data-dismiss=""alert"" aria-label=""close"">&times;</a> Bạn không có quyền truy cập vào chức năng này! </div> </div> </div>";
                return RedirectToAction("Index", new { sortOrder = String.Empty, currentFilter = String.Empty, searchString = String.Empty });
            }

            //if (context.PHIEU_THUTIEN.Any(pt => pt.ID_PHIEUBANLE == id.Value))
            //{
            //    TempData["msg"] = @"<div id=""rowError"" class=""row""> <div class=""col-sm-10""> <div class=""alert alert-danger alert-dismissable fade in"" style=""padding-top: 5px; padding-bottom: 5px""> <a href=""#"" class=""close"" data-dismiss=""alert"" aria-label=""close"">&times;</a> Không thể sửa phiếu đã lập phiếu thu! </div> </div> </div>";
            //    //TempData["msg"] = "<script>alert('Không thể sửa phiếu đã lập phiếu thu!');</script>";
            //    return RedirectToAction("Index", new { sortOrder = String.Empty, currentFilter = String.Empty, searchString = String.Empty });
            //}
            PhieuBanLeViewModel vmPhieuBanLe = new PhieuBanLeViewModel();
            //vmPhieuBanLe.PhieuBanLe = new PHIEU_BANLE();
            vmPhieuBanLe.PhieuBanLe = context.PHIEU_BANLE.Single(pbl => pbl.ID_PHIEUBANLE == id.Value);
            //vmPhieuBanLe.ListChiTietPhieu = new List<CHITIET_PHIEUBANLE>();
            vmPhieuBanLe.ListChiTietPhieu = context.CHITIET_PHIEUBANLE.Where(ct => ct.ID_PHIEUBANLE == id.Value).ToList();
            //vmPhieuBanLe.ListChiTietPhieu = new List<CHITIET_PHIEUBANLE>();
            //vmPhieuBanLe.TenKH = "";
            //vmPhieuBanLe.TenNV = "";
            vmPhieuBanLe.ListPhuTung = context.PHUTUNGs.ToList();
            vmPhieuBanLe.ListHieuXe = context.HIEUXEs.ToList();
            vmPhieuBanLe.TenNV = context.NHANVIENs.Single(nv => nv.MA_NV == vmPhieuBanLe.PhieuBanLe.MaNV).HOTEN;
            vmPhieuBanLe.TenKH = context.KHACHHANGs.Single(kh => kh.MA_KH == vmPhieuBanLe.PhieuBanLe.MaKH).TEN_KH;
            string fileName = vmPhieuBanLe.PhieuBanLe.MaPhieuBan + ".pdf";
            //return View(vmPhieuBanLe);
            return new Rotativa.ViewAsPdf("In", vmPhieuBanLe)
            {
                FileName = fileName,

                PageOrientation = Orientation.Landscape,
                PageSize = Size.A4,
                PageMargins = { Left = 0, Right = 0 }, // it's in millimeters
                CustomSwitches = "--disable-smart-shrinking",


            };
        }
        // GET: PhieuBanLe
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? searchOption, int? page)
        {
            ViewBag.CurrentSearchOption = searchOption;
            ViewBag.CurrentSort = sortOrder;
            ViewBag.DateSortParam = sortOrder == "date_asc" ? "date_desc" : "date_asc";
            ViewBag.TotalSortParam = sortOrder == "total_asc" ? "total_desc" : "total_asc";
            ViewBag.RemainSortParam = sortOrder == "remain_asc" ? "remain_desc" : "remain_asc";

            GARADBEntities context = new GARADBEntities();
            var dsPhieuBanLeQuery = (from pbl in context.PHIEU_BANLE
                                         join nv in context.NHANVIENs on pbl.MaNV.Value equals nv.MA_NV
                                         join kh in context.KHACHHANGs on pbl.MaKH.Value equals kh.MA_KH
                                         select new
                                         {
                                             ID_PHIEUBANLE = pbl.ID_PHIEUBANLE,
                                             MaPhieuBan = pbl.MaPhieuBan,
                                             NgayLap = pbl.NgayLap.Value,
                                             HanChotThanhToan = pbl.HanChotThanhToan.Value,
                                             TenKH = kh.TEN_KH,
                                             TenNV = nv.HOTEN,
                                             TongTien = pbl.TongTien.Value,
                                             SoTienConLai = pbl.SoTienConLai.Value

                                         });

            List<PhieuBanLeTableData> listPhieuBanLe = new List<PhieuBanLeTableData>();
            foreach(var item in dsPhieuBanLeQuery)
            {
                PhieuBanLeTableData data = new PhieuBanLeTableData();
                data.ID_PHIEUBANLE = item.ID_PHIEUBANLE;
                data.MaPhieuBan = item.MaPhieuBan;
                data.NgayLap = item.NgayLap;
                data.HanChotThanhToan = item.HanChotThanhToan;
                data.SoTienConLai = item.SoTienConLai;
                data.TongTien = item.TongTien;
                data.TenKH = item.TenKH;
                data.TenNV = item.TenNV;
                listPhieuBanLe.Add(data);
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
                        case 1: { listPhieuBanLe = listPhieuBanLe.Where(c => c.MaPhieuBan.Contains(searchString)).ToList(); break; }
                        default: { break; }
                    }
                }

            }
            switch(sortOrder)
            {
                case "date_asc": { listPhieuBanLe = listPhieuBanLe.OrderBy(pbl => pbl.NgayLap).ToList(); break; }
                case "date_desc": { listPhieuBanLe = listPhieuBanLe.OrderByDescending(pbl => pbl.NgayLap).ToList(); break; }
                case "total_asc": { listPhieuBanLe = listPhieuBanLe.OrderBy(pbl => pbl.TongTien).ToList(); break; }
                case "total_desc": { listPhieuBanLe = listPhieuBanLe.OrderByDescending(pbl => pbl.TongTien).ToList(); break; }
                case "remain_asc": { listPhieuBanLe = listPhieuBanLe.OrderBy(pbl => pbl.SoTienConLai).ToList(); break; }
                case "remain_desc": { listPhieuBanLe = listPhieuBanLe.OrderByDescending(pbl => pbl.SoTienConLai).ToList(); break; }
            }
            int pageSize = 5;
            int pageNumber = page ?? 1;
            DSPhieuBanLeViewModel vmDSPhieuBanLe = new DSPhieuBanLeViewModel();
            vmDSPhieuBanLe.ListData = listPhieuBanLe.ToPagedList(pageNumber, pageSize);
            return View(vmDSPhieuBanLe);
        }
        public JsonResult Xoa(int id)
        {
            try
            {
                GARADBEntities context = new GARADBEntities();


                int UserId = int.Parse(Session["UserID"].ToString());
                NHANVIEN nv = context.NHANVIENs.Single(staff => staff.MA_NV == UserId);
                NHOMNGUOIDUNG groupuser = context.NHOMNGUOIDUNGs.Single(gu => gu.MA_NHOMNGUOIDUNG == nv.MA_NHOMNGUOIDUNG.Value);
                if (groupuser.CAPDO != 2 && groupuser.CAPDO != 4)
                {
                    TempData["msg"] = @"<div id=""rowError"" class=""row""> <div class=""col-sm-10""> <div class=""alert alert-danger alert-dismissable fade in"" style=""padding-top: 5px; padding-bottom: 5px""> <a href=""#"" class=""close"" data-dismiss=""alert"" aria-label=""close"">&times;</a> Bạn không có quyền thực hiện thao tác này! </div> </div> </div>";
                    //return RedirectToAction("Index", new { sortOrder = String.Empty, currentFilter = String.Empty, searchString = String.Empty });
                    return Json(new { value = "-1", message = "Bạn không có quyền thực hiện thao tác này!" }, JsonRequestBehavior.AllowGet);
                }


                var target = context.PHIEU_BANLE.Single(pbl => pbl.ID_PHIEUBANLE == id);
                if (context.PHIEU_THUTIEN.Any(pt => pt.ID_PHIEUBANLE == target.ID_PHIEUBANLE))
                {
                    TempData["msg"] = @"<div id=""rowError"" class=""row""> <div class=""col-sm-10""> <div class=""alert alert-danger alert-dismissable fade in"" style=""padding-top: 5px; padding-bottom: 5px""> <a href=""#"" class=""close"" data-dismiss=""alert"" aria-label=""close"">&times;</a> Không thể xóa do đã lập phiếu thu! </div> </div> </div>";
                    return Json(new { value = "-1", message = "Không thể xóa do đã lập phiếu thu!" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    List<CHITIET_PHIEUBANLE> listCT = context.CHITIET_PHIEUBANLE.Where(ct => ct.ID_PHIEUBANLE == id).ToList();
                    foreach(var item in listCT)
                    {
                        context.CHITIET_PHIEUBANLE.Remove(item);
                        context.SaveChanges();
                    }
                    context.PHIEU_BANLE.Remove(target);
                    context.SaveChanges();
                    TempData["msg"] = @"<div id=""rowSuccess"" class=""row""> <div class=""col-sm-10""> <div class=""alert alert-success alert-dismissable fade in"" style=""padding-top: 5px; padding-bottom: 5px""> <a href=""#"" class=""close"" data-dismiss=""alert"" aria-label=""close"">&times;</a> Xóa thành công! </div> </div> </div>";
                    return Json(new { value = "1", message = "Xóa thành công!" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch(Exception)
            {
                TempData["msg"] = @"<div id=""rowError"" class=""row""> <div class=""col-sm-10""> <div class=""alert alert-danger alert-dismissable fade in"" style=""padding-top: 5px; padding-bottom: 5px""> <a href=""#"" class=""close"" data-dismiss=""alert"" aria-label=""close"">&times;</a> Không thể xóa phiếu! Vui lòng thử lại sau! </div> </div> </div>";
                return Json(new { value = "-1", message = "Không thể xóa phiếu" }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public ActionResult NhapPhieuBanLe()
        {
            PhieuBanLeViewModel vmPhieuBanLe = new PhieuBanLeViewModel();
            GARADBEntities context = new GARADBEntities();

            int UserId = int.Parse(Session["UserID"].ToString());
            NHANVIEN nv = context.NHANVIENs.Single(staff => staff.MA_NV == UserId);
            NHOMNGUOIDUNG groupuser = context.NHOMNGUOIDUNGs.Single(gu => gu.MA_NHOMNGUOIDUNG == nv.MA_NHOMNGUOIDUNG.Value);
            if (groupuser.CAPDO != 2 && groupuser.CAPDO != 4)
            {
                TempData["msg"] = @"<div id=""rowError"" class=""row""> <div class=""col-sm-10""> <div class=""alert alert-danger alert-dismissable fade in"" style=""padding-top: 5px; padding-bottom: 5px""> <a href=""#"" class=""close"" data-dismiss=""alert"" aria-label=""close"">&times;</a> Bạn không có quyền truy cập vào chức năng này! </div> </div> </div>";
                return RedirectToAction("Index", new { sortOrder = String.Empty, currentFilter = String.Empty, searchString = String.Empty });
            }




            vmPhieuBanLe.ListKhachHang = context.KHACHHANGs.ToList();
            vmPhieuBanLe.ListPhuTung = context.PHUTUNGs.ToList();
            vmPhieuBanLe.ListHieuXe = context.HIEUXEs.ToList();
            vmPhieuBanLe.PhieuBanLe = new PHIEU_BANLE();
            vmPhieuBanLe.PhieuBanLe.NgayLap = DateTime.Now.Date;
            vmPhieuBanLe.TenNV = Session["staff_name"].ToString();
            vmPhieuBanLe.PhieuBanLe.MaNV = int.Parse(Session["UserID"].ToString());
            BANGTHAMSO a = context.BANGTHAMSOes.Single(ts => ts.TENTHAMSO.Equals("HanChotPhieuBanLe"));
            vmPhieuBanLe.PhieuBanLe.HanChotThanhToan = DateTime.Now.Date.AddDays(int.Parse(a.GIATRI)).Date;
            DateTime t = DateTime.Now;
            //t.AddDays(30)
            //Sua loi khong hien thi ten nguoi lap!!
            //vmPhieuBanLe.TenNV = "";
            //vmPhieuBanLe.TenNV = context.NHANVIENs.Single(s => s.USERNAME.Equals(Session["Username"])).HOTEN;
            return View(vmPhieuBanLe);
        }
        [HttpPost]
        public ActionResult NhapPhieuBanLe(PHIEU_BANLE phieubanle, List<CHITIET_PHIEUBANLE> listchitiet) 
        {
            GARADBEntities context = new GARADBEntities();
            phieubanle.NgayLap = DateTime.Now.Date;
            phieubanle.TongTien = 0;
            phieubanle.SoTienConLai = 0;
            BANGTHAMSO a = context.BANGTHAMSOes.Single(ts => ts.TENTHAMSO.Equals("HanChotPhieuBanLe"));
            phieubanle.HanChotThanhToan = DateTime.Now.Date.AddDays(int.Parse(a.GIATRI)).Date;
            //phieubanle.MaNV = context.NHANVIENs.Single(nv => nv.USERNAME.Equals(Session["Username"])).MA_NV;
            //phieubanle.TongTien = listchitiet.Sum(ct => ct.THANHTIEN);
           // phieubanle.HanChotThanhToan =
            context.PHIEU_BANLE.Add(phieubanle);
            context.SaveChanges();
            foreach(var item in listchitiet)
            {
                
                item.ID_PHIEUBANLE = phieubanle.ID_PHIEUBANLE;
                context.CHITIET_PHIEUBANLE.Add(item);
                context.SaveChanges();
            }
            TempData["msg"] = @"<div id=""rowSuccess"" class=""row""> <div class=""col-sm-10""> <div class=""alert alert-success alert-dismissable fade in"" style=""padding-top: 5px; padding-bottom: 5px""> <a href=""#"" class=""close"" data-dismiss=""alert"" aria-label=""close"">&times;</a> Thêm phiếu bán lẻ thành công! </div> </div> </div>";
            //TempData["msg"] = "<script>alert('Đã thêm thành công!');</script>";
            return RedirectToAction("Index", new { sortOrder = String.Empty, currentFilter = String.Empty, searchString = String.Empty });
        }
        [HttpGet]
        public ActionResult SuaPhieuBanLe(int? id)
        {
            GARADBEntities context = new GARADBEntities();

            int UserId = int.Parse(Session["UserID"].ToString());
            NHANVIEN st = context.NHANVIENs.Single(staff => staff.MA_NV == UserId);
            NHOMNGUOIDUNG groupuser = context.NHOMNGUOIDUNGs.Single(gu => gu.MA_NHOMNGUOIDUNG == st.MA_NHOMNGUOIDUNG.Value);
            if (groupuser.CAPDO != 2 && groupuser.CAPDO != 4)
            {
                TempData["msg"] = @"<div id=""rowError"" class=""row""> <div class=""col-sm-10""> <div class=""alert alert-danger alert-dismissable fade in"" style=""padding-top: 5px; padding-bottom: 5px""> <a href=""#"" class=""close"" data-dismiss=""alert"" aria-label=""close"">&times;</a> Bạn không có quyền truy cập vào chức năng này! </div> </div> </div>";
                return RedirectToAction("Index", new { sortOrder = String.Empty, currentFilter = String.Empty, searchString = String.Empty });
            }

            if(context.PHIEU_THUTIEN.Any(pt => pt.ID_PHIEUBANLE == id.Value))
            {
                TempData["msg"] = @"<div id=""rowError"" class=""row""> <div class=""col-sm-10""> <div class=""alert alert-danger alert-dismissable fade in"" style=""padding-top: 5px; padding-bottom: 5px""> <a href=""#"" class=""close"" data-dismiss=""alert"" aria-label=""close"">&times;</a> Không thể sửa phiếu đã lập phiếu thu! </div> </div> </div>";
                //TempData["msg"] = "<script>alert('Không thể sửa phiếu đã lập phiếu thu!');</script>";
                return RedirectToAction("Index", new { sortOrder = String.Empty, currentFilter = String.Empty, searchString = String.Empty });
            }
            PhieuBanLeViewModel vmPhieuBanLe = new PhieuBanLeViewModel();
            //vmPhieuBanLe.PhieuBanLe = new PHIEU_BANLE();
            vmPhieuBanLe.PhieuBanLe = context.PHIEU_BANLE.Single(pbl => pbl.ID_PHIEUBANLE == id.Value);
            //vmPhieuBanLe.ListChiTietPhieu = new List<CHITIET_PHIEUBANLE>();
            vmPhieuBanLe.ListChiTietPhieu = context.CHITIET_PHIEUBANLE.Where(ct => ct.ID_PHIEUBANLE == id.Value).ToList();
            //vmPhieuBanLe.ListChiTietPhieu = new List<CHITIET_PHIEUBANLE>();
            //vmPhieuBanLe.TenKH = "";
            //vmPhieuBanLe.TenNV = "";
            vmPhieuBanLe.ListPhuTung = context.PHUTUNGs.ToList();
            vmPhieuBanLe.ListHieuXe = context.HIEUXEs.ToList();
            vmPhieuBanLe.TenNV = context.NHANVIENs.Single(nv => nv.MA_NV == vmPhieuBanLe.PhieuBanLe.MaNV).HOTEN;
            vmPhieuBanLe.TenKH = context.KHACHHANGs.Single(kh => kh.MA_KH == vmPhieuBanLe.PhieuBanLe.MaKH).TEN_KH;
            //vmPhieuBanLe.PhieuBanLe = new PHIEU_BANLE();
            return View(vmPhieuBanLe);
        }
        [HttpPost]
        public ActionResult SuaPhieuBanLe(PhieuBanLeViewModel vmPhieuBanLe)
        {
            TempData["msg"] = @"<div id=""rowSuccess"" class=""row""> <div class=""col-sm-10""> <div class=""alert alert-success alert-dismissable fade in"" style=""padding-top: 5px; padding-bottom: 5px""> <a href=""#"" class=""close"" data-dismiss=""alert"" aria-label=""close"">&times;</a> Cập nhật thành công! </div> </div> </div>";
            //TempData["msg"] = "<script>alert('Đã sửa thành công!');</script>";
            return RedirectToAction("Index", new { sortOrder = String.Empty, currentFilter = String.Empty, searchString = String.Empty });
        }
        [HttpPost]
        public JsonResult XoaChiTiet(int id)
        {
            try
            {
                
                GARADBEntities context = new GARADBEntities();
                var target = context.CHITIET_PHIEUBANLE.Single(pbl => pbl.ID == id);
                if(context.PHIEU_THUTIEN.Any(pt => pt.ID_PHIEUBANLE == target.ID_PHIEUBANLE ))
                {
                    TempData["msg"] = @"<div id=""rowError"" class=""row""> <div class=""col-sm-10""> <div class=""alert alert-danger alert-dismissable fade in"" style=""padding-top: 5px; padding-bottom: 5px""> <a href=""#"" class=""close"" data-dismiss=""alert"" aria-label=""close"">&times;</a> Không thể xóa phiếu đã lập phiếu thu! </div> </div> </div>";
                    return Json(new { value = "-1", message = "Không thể xóa phiếu đã lập phiếu thu!" });
                }
                context.CHITIET_PHIEUBANLE.Remove(target);
                context.SaveChanges();
                TempData["msg"] = @"<div id=""rowSuccess"" class=""row""> <div class=""col-sm-10""> <div class=""alert alert-success alert-dismissable fade in"" style=""padding-top: 5px; padding-bottom: 5px""> <a href=""#"" class=""close"" data-dismiss=""alert"" aria-label=""close"">&times;</a> Xóa chi tiết phiếu thành công! </div> </div> </div>";
                return Json(new { value = "1", message = "Xóa chi tiết phiếu thành công!" });
            }
            catch(Exception)
            {
                TempData["msg"] = @"<div id=""rowError"" class=""row""> <div class=""col-sm-10""> <div class=""alert alert-danger alert-dismissable fade in"" style=""padding-top: 5px; padding-bottom: 5px""> <a href=""#"" class=""close"" data-dismiss=""alert"" aria-label=""close"">&times;</a> Không thể xóa! Vui lòng thử lại sau! </div> </div> </div>";
                return Json(new { value = "-1", message = "Đã có lỗi xảy ra không thể xóa!" });
            }
        }
        [HttpPost]
        public ActionResult GetPhuTungByHieuXe(string hieuxe)
        {
            GARADBEntities context = new GARADBEntities();
            var ListPhuTung = context.PHUTUNGs.Where(pt => pt.MA_HIEUXE.Equals(hieuxe)).Select(pt => "<option value='" + pt.ID + "' title='" + pt.SOLUONGTON + "' id='" + pt.MA_PHUTUNG + "' itemscope='" + pt.TG_BAOHANH + "' itemprop='" + pt.DONGIAXUAT + "'>" + pt.TEN_PHUTUNG + "</option>");
            return Content(String.Join("", ListPhuTung));
        }
        [HttpPost]
        public JsonResult ThemChiTiet(CHITIET_PHIEUBANLE chitiet)
        {
            GARADBEntities context = new GARADBEntities();
            context.CHITIET_PHIEUBANLE.Add(chitiet);
            context.SaveChanges();
            TempData["msg"] = @"<div id=""rowSucess"" class=""row""> <div class=""col-sm-10""> <div class=""alert alert-success alert-dismissable fade in"" style=""padding-top: 5px; padding-bottom: 5px""> <a href=""#"" class=""close"" data-dismiss=""alert"" aria-label=""close"">&times;</a> Thêm chi tiết thành công! </div> </div> </div>";
            return Json(new { issucess = "1", newid = "1", message = "OK" }, JsonRequestBehavior.AllowGet);
        }
    }
}