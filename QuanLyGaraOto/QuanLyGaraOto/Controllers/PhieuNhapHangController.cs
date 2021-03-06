﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using QuanLyGaraOto.Models;
using QuanLyGaraOto.ViewModel;
using Rotativa.Options;

namespace QuanLyGaraOto.Controllers
{
    public class PhieuNhapHangController : Controller
    {
        // GET: PhieuNhapHang
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? searchOption,
            int? page)
        {
            ViewBag.CurrentSearchOption = searchOption;
            ViewBag.CurrentSort = sortOrder;
            ViewBag.DateNhapOrderSortParam = (sortOrder == "nhap_asc" ? "nhap_desc" : "nhap_asc");   //Ban đầu là null -> ASC
            GARADBEntities context = new GARADBEntities();
            List<PhieuNhapHangViewModel> listPNH = new List<PhieuNhapHangViewModel>();
            foreach (var item in context.PHIEU_NHAPHANG.ToList())
            {
                String tennv = context.NHANVIENs.Single(nv => nv.MA_NV == item.MA_NV).HOTEN;
                String tenncc = context.NHACUNGCAPs.Single(nv => nv.MaNCC == item.MaNCC).TenNCC;
                String maphieudh;
                if (item.ID_PHIEUDATHANG != null)
                {
                    maphieudh =
                        context.PHIEU_DATHANG.Single(pdh => pdh.Id_PhieuDatHang == item.ID_PHIEUDATHANG).MaPhieuDat;
                }
                else
                {
                    maphieudh = "Không đặt trước";
                }
                listPNH.Add(new PhieuNhapHangViewModel(item, maphieudh, tennv, tenncc));
            }
            if (searchString != null)   //Nếu là search, cho hiển thị trang 1. Nếu ko thì hiện lại searchString lần trước (null)
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
                    try
                    {
                        switch (searchOption)
                        {
                            case 0:
                            {
                                break;
                            }
                            case 1:
                            {
                                listPNH =
                                    listPNH.Where(c => c.PhieuNhapHang.MA_PHIEUNHAPHANG.Contains(searchString)).ToList();
                                break;
                            }
                            case 2:
                            {
                                listPNH = listPNH.Where(c => c.MaPhieuDatHang.Contains(searchString)).ToList();
                                break;
                            }
                            case 3:
                            {
                                listPNH =
                                    listPNH.Where(
                                        c =>
                                            c.PhieuNhapHang.NGAYLAP.Value.Date.Equals(DateTime.Parse(searchString).Date))
                                        .ToList();
                                break;
                            }
                            case 4:
                            {
                                listPNH = listPNH.Where(c => c.TenNV.Contains(searchString)).ToList();
                                break;
                            }
                            case 5:
                            {
                                listPNH = listPNH.Where(c => c.TenNCC.Contains(searchString)).ToList();
                                break;
                            }
                            default:
                            {
                                break;
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        TempData["msg"] = @"<div id=""rowSuccess"" class=""row""> <div class=""col-sm-10""> <div class=""alert alert-danger alert-dismissable fade in"" style=""padding-top: 5px; padding-bottom: 5px""> <a href=""#"" class=""close"" data-dismiss=""alert"" aria-label=""close"">&times;</a> Thời gian bạn nhập vào không đúng! </div> </div> </div>";
                    }
                }

            }
            switch (sortOrder)
            {
                case "nhap_asc": { listPNH = listPNH.OrderBy(c => c.PhieuNhapHang.NGAYLAP).ToList(); break; }
                case "nhap_desc": { listPNH = listPNH.OrderByDescending(c => c.PhieuNhapHang.NGAYLAP).ToList(); break; }
                default: { break; }
            }
            int pageSize = 5;
            int pageNumber = (page ?? 1); //nếu page có giá trị thì = page, ko thì = 1
            DSPhieuNhapHangViewModel pnhViewModel = new DSPhieuNhapHangViewModel();
            pnhViewModel.ListPhieuNhapHang = listPNH.ToPagedList(pageNumber, pageSize);
            return View(pnhViewModel);
        }

        [HttpGet]
        public ActionResult ThemMoi(int idphieuDH = 60)
        {
            PhieuNhapHangViewModel vmPhieuNH = new PhieuNhapHangViewModel();

            GARADBEntities context = new GARADBEntities();
            int UserId = int.Parse(Session["UserID"].ToString());
            NHANVIEN nv = context.NHANVIENs.Single(staff => staff.MA_NV == UserId);
            NHOMNGUOIDUNG groupuser = context.NHOMNGUOIDUNGs.Single(gu => gu.MA_NHOMNGUOIDUNG == nv.MA_NHOMNGUOIDUNG.Value);
            if (groupuser.CAPDO != 2 && groupuser.CAPDO != 5)
            {
                TempData["msg"] = @"<div id=""rowError"" class=""row""> <div class=""col-sm-10""> <div class=""alert alert-danger alert-dismissable fade in"" style=""padding-top: 5px; padding-bottom: 5px""> <a href=""#"" class=""close"" data-dismiss=""alert"" aria-label=""close"">&times;</a> Bạn không có quyền truy cập vào chức năng này! </div> </div> </div>";
                return RedirectToAction("Index", new { sortOrder = String.Empty, currentFilter = String.Empty, searchString = String.Empty });
            }

            vmPhieuNH.ListNhaCungCap = context.NHACUNGCAPs.ToList();
            vmPhieuNH.ListPhuTung = context.PHUTUNGs.ToList();
            vmPhieuNH.ListNhomNCC = context.NHOMNHACUNGCAPs.ToList();
            vmPhieuNH.ListHieuXe = context.HIEUXEs.ToList();
            vmPhieuNH.PhieuNhapHang.MA_NV = UserId;
            int slNhapMax = Int32.Parse(context.BANGTHAMSOes.Single(ts => ts.TENTHAMSO == "SoLuongNhapHangToiDa").GIATRI);
            vmPhieuNH.SoLuongNhapToiDa = slNhapMax;
            vmPhieuNH.PhieuNhapHang.NGAYLAP = DateTime.Now.Date;
            vmPhieuNH.TenNV = nv.HOTEN;
            PHIEU_DATHANG pdhPhieuDathang = context.PHIEU_DATHANG.Single(pdh => pdh.Id_PhieuDatHang == idphieuDH);
            vmPhieuNH.PhieuNhapHang.MaNCC = pdhPhieuDathang.MaNCC;
            vmPhieuNH.MaPhieuDatHang = pdhPhieuDathang.MaPhieuDat;
            vmPhieuNH.PhieuNhapHang.ID_PHIEUDATHANG = idphieuDH;
            return View(vmPhieuNH);
        }

        [HttpGet]
        public ActionResult ThemMoiNhapLe()
        {
            PhieuNhapHangViewModel vmPhieuNH = new PhieuNhapHangViewModel();

            GARADBEntities context = new GARADBEntities();
            int UserId = int.Parse(Session["UserID"].ToString());
            NHANVIEN nv = context.NHANVIENs.Single(staff => staff.MA_NV == UserId);
            NHOMNGUOIDUNG groupuser = context.NHOMNGUOIDUNGs.Single(gu => gu.MA_NHOMNGUOIDUNG == nv.MA_NHOMNGUOIDUNG.Value);
            if (groupuser.CAPDO != 2 && groupuser.CAPDO != 5)
            {
                TempData["msg"] = @"<div id=""rowError"" class=""row""> <div class=""col-sm-10""> <div class=""alert alert-danger alert-dismissable fade in"" style=""padding-top: 5px; padding-bottom: 5px""> <a href=""#"" class=""close"" data-dismiss=""alert"" aria-label=""close"">&times;</a> Bạn không có quyền truy cập vào chức năng này! </div> </div> </div>";
                return RedirectToAction("Index", new { sortOrder = String.Empty, currentFilter = String.Empty, searchString = String.Empty });
            }

            vmPhieuNH.ListNhaCungCap = context.NHACUNGCAPs.ToList();
            vmPhieuNH.ListPhuTung = context.PHUTUNGs.ToList();
            vmPhieuNH.ListNhomNCC = context.NHOMNHACUNGCAPs.ToList();
            vmPhieuNH.ListHieuXe = context.HIEUXEs.ToList();
            vmPhieuNH.PhieuNhapHang.MA_NV = UserId;
            int slNhapMax = Int32.Parse(context.BANGTHAMSOes.Single(ts => ts.TENTHAMSO == "SoLuongNhapHangToiDa").GIATRI);
            vmPhieuNH.SoLuongNhapToiDa = slNhapMax;
            vmPhieuNH.PhieuNhapHang.NGAYLAP = DateTime.Now.Date;
            vmPhieuNH.TenNV = nv.HOTEN;
            vmPhieuNH.PhieuNhapHang.MaNCC = null;
            vmPhieuNH.MaPhieuDatHang = "Không đặt trước";
            vmPhieuNH.PhieuNhapHang.ID_PHIEUDATHANG = null;
            return View(vmPhieuNH);
        }

        [HttpPost]
        public ActionResult ThemMoi(PHIEU_NHAPHANG pnh, IEnumerable<CHITIET_PHIEUNH> listChiTiet)
        {
            try
            {
                GARADBEntities context = new GARADBEntities();
                pnh.NGAYLAP = DateTime.Now.Date;
                context.PHIEU_NHAPHANG.Add(pnh);               
                context.SaveChanges();
                List<CHITIET_PHIEUNH> listCT = listChiTiet.ToList();
                foreach (var item in listChiTiet)
                {
                    if (item != null)
                    {
                        item.ID_PHIEUNHAPHANG = pnh.ID_PHIEUNHAPHANG;
                        context.CHITIET_PHIEUNH.Add(item);
                    }                   
                }
                context.SaveChanges();
                TempData["msg"] = @"<div id=""rowSuccess"" class=""row""> <div class=""col-sm-10""> <div class=""alert alert-success alert-dismissable fade in"" style=""padding-top: 5px; padding-bottom: 5px""> <a href=""#"" class=""close"" data-dismiss=""alert"" aria-label=""close"">&times;</a> Thêm mới thành công! </div> </div> </div>";
            }
            catch (Exception)
            {
                TempData["msg"] = @"<div id=""rowError"" class=""row""> <div class=""col-sm-10""> <div class=""alert alert-danger alert-dismissable fade in"" style=""padding-top: 5px; padding-bottom: 5px""> <a href=""#"" class=""close"" data-dismiss=""alert"" aria-label=""close"">&times;</a> Đã có lỗi xảy ra! Vui lòng thử lại! </div> </div> </div>";
            }
            return RedirectToAction("Index");
        }

        
        [HttpGet]
        public ActionResult CapNhat(int id = 2)
        {
            GARADBEntities context = new GARADBEntities();
            int UserId = int.Parse(Session["UserID"].ToString());
            NHANVIEN nv = context.NHANVIENs.Single(staff => staff.MA_NV == UserId);
            NHOMNGUOIDUNG groupuser = context.NHOMNGUOIDUNGs.Single(gu => gu.MA_NHOMNGUOIDUNG == nv.MA_NHOMNGUOIDUNG.Value);
            if (groupuser.CAPDO != 2 && groupuser.CAPDO != 5)
            {
                TempData["msg"] = @"<div id=""rowError"" class=""row""> <div class=""col-sm-10""> <div class=""alert alert-danger alert-dismissable fade in"" style=""padding-top: 5px; padding-bottom: 5px""> <a href=""#"" class=""close"" data-dismiss=""alert"" aria-label=""close"">&times;</a> Bạn không có quyền truy cập vào chức năng này! </div> </div> </div>";
                return RedirectToAction("Index", new { sortOrder = String.Empty, currentFilter = String.Empty, searchString = String.Empty });
            }


            PhieuNhapHangViewModel vmPhieuNH = new PhieuNhapHangViewModel();
            vmPhieuNH.PhieuNhapHang = context.PHIEU_NHAPHANG.Single(p => p.ID_PHIEUNHAPHANG == id);
            int slNhapMax = Int32.Parse(context.BANGTHAMSOes.Single(ts => ts.TENTHAMSO == "SoLuongNhapHangToiDa").GIATRI);
            vmPhieuNH.SoLuongNhapToiDa = slNhapMax;
            vmPhieuNH.ListPhuTung = context.PHUTUNGs.ToList();
            vmPhieuNH.ListNhaCungCap = context.NHACUNGCAPs.ToList();
            vmPhieuNH.ListNhomNCC = context.NHOMNHACUNGCAPs.ToList();
            vmPhieuNH.ListHieuXe = context.HIEUXEs.ToList();
            nv = context.NHANVIENs.Single(staff => staff.MA_NV == vmPhieuNH.PhieuNhapHang.MA_NV);
            vmPhieuNH.TenNV = nv.HOTEN;
            if (vmPhieuNH.PhieuNhapHang.ID_PHIEUDATHANG != null)
            {
                PHIEU_DATHANG pdhPhieuDathang = context.PHIEU_DATHANG.Single(pdh => pdh.Id_PhieuDatHang == vmPhieuNH.PhieuNhapHang.ID_PHIEUDATHANG);
                vmPhieuNH.MaPhieuDatHang = pdhPhieuDathang.MaPhieuDat;
            }
            else
            {
                vmPhieuNH.MaPhieuDatHang = "Không đặt trước";
            }
            
            vmPhieuNH.ListChiTietPhieuNH = new List<CHITIET_PHIEUNH>();
            vmPhieuNH.ListChiTietPhieuNH = context.CHITIET_PHIEUNH.Where(ct => ct.ID_PHIEUNHAPHANG == id).ToList();
            return View(vmPhieuNH);
        }
        
        [HttpPost]
        public ActionResult CapNhat(PHIEU_NHAPHANG phieunh)
        {
            try
            {
                GARADBEntities context = new GARADBEntities();
                var target = context.PHIEU_NHAPHANG.Single(pdh => pdh.ID_PHIEUNHAPHANG == phieunh.ID_PHIEUNHAPHANG);
                target.MA_PHIEUNHAPHANG = phieunh.MA_PHIEUNHAPHANG;
                context.SaveChanges();
                TempData["msg"] = @"<div id=""rowSuccess"" class=""row""> <div class=""col-sm-10""> <div class=""alert alert-success alert-dismissable fade in"" style=""padding-top: 5px; padding-bottom: 5px""> <a href=""#"" class=""close"" data-dismiss=""alert"" aria-label=""close"">&times;</a> Cập nhật thành công! </div> </div> </div>";
            }
            catch (Exception)
            {
                TempData["msg"] = @"<div id=""rowError"" class=""row""> <div class=""col-sm-10""> <div class=""alert alert-danger alert-dismissable fade in"" style=""padding-top: 5px; padding-bottom: 5px""> <a href=""#"" class=""close"" data-dismiss=""alert"" aria-label=""close"">&times;</a> Đã có lỗi xảy ra! Vui lòng thử lại! </div> </div> </div>";
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult In(int id = 2)
        {
            GARADBEntities context = new GARADBEntities();
            PhieuNhapHangViewModel vmPhieuNH = new PhieuNhapHangViewModel();
            vmPhieuNH.PhieuNhapHang = context.PHIEU_NHAPHANG.Single(p => p.ID_PHIEUNHAPHANG == id);
            int slNhapMax = Int32.Parse(context.BANGTHAMSOes.Single(ts => ts.TENTHAMSO == "SoLuongNhapHangToiDa").GIATRI);
            vmPhieuNH.SoLuongNhapToiDa = slNhapMax;
            vmPhieuNH.ListPhuTung = context.PHUTUNGs.ToList();
            vmPhieuNH.ListNhaCungCap = context.NHACUNGCAPs.ToList();
            vmPhieuNH.ListNhomNCC = context.NHOMNHACUNGCAPs.ToList();
            vmPhieuNH.ListHieuXe = context.HIEUXEs.ToList();
            vmPhieuNH.TenNV = context.NHANVIENs.Single(nv => nv.MA_NV == vmPhieuNH.PhieuNhapHang.MA_NV).HOTEN;
            if (vmPhieuNH.PhieuNhapHang.ID_PHIEUDATHANG != null)
            {
                PHIEU_DATHANG pdhPhieuDathang = context.PHIEU_DATHANG.Single(pdh => pdh.Id_PhieuDatHang == vmPhieuNH.PhieuNhapHang.ID_PHIEUDATHANG);
                vmPhieuNH.MaPhieuDatHang = pdhPhieuDathang.MaPhieuDat;
            }
            else
            {
                vmPhieuNH.MaPhieuDatHang = "Không đặt trước";
            }

            vmPhieuNH.ListChiTietPhieuNH = new List<CHITIET_PHIEUNH>();
            vmPhieuNH.ListChiTietPhieuNH = context.CHITIET_PHIEUNH.Where(ct => ct.ID_PHIEUNHAPHANG == id).ToList();

            string fileName = vmPhieuNH.PhieuNhapHang.MA_PHIEUNHAPHANG + ".pdf";
            return new Rotativa.ViewAsPdf("In", vmPhieuNH)
            {
                FileName = fileName,
                PageOrientation = Orientation.Landscape,
                PageSize = Size.A4,
                PageMargins = { Left = 0, Right = 0 }, // it's in millimeters
                CustomSwitches = "--disable-smart-shrinking",
            };
        }

        [HttpPost]
        public JsonResult ThemChiTiet(CHITIET_PHIEUNH chitiet)
        {
            GARADBEntities context = new GARADBEntities();
            context.CHITIET_PHIEUNH.Add(chitiet);
            context.SaveChanges();
            TempData["msg"] = @"<div id=""rowSuccess"" class=""row""> <div class=""col-sm-10""> <div class=""alert alert-success alert-dismissable fade in"" style=""padding-top: 5px; padding-bottom: 5px""> <a href=""#"" class=""close"" data-dismiss=""alert"" aria-label=""close"">&times;</a> Thêm chi tiết phiếu thành công! </div> </div> </div>";
            return Json(new { issucess = "1", newid = "1", message = "Thêm chi tiết phiếu thành công!" }, JsonRequestBehavior.AllowGet);
        }       

        [HttpPost]
        public ActionResult GetNCCByNhomNCC(string nhomncc)
        {
            GARADBEntities context = new GARADBEntities();
            var ListNCC = context.NHACUNGCAPs.Where(pt => pt.NhomNCC.Equals(nhomncc)).Select(pt => "<option value='" + pt.MaNCC + "' title='" + pt.TenNCC + "'>" + pt.TenNCC + "</option>");
            return Content(String.Join("", ListNCC));
        }
        [HttpPost]
        public ActionResult GetPhuTungByHieuXe(string hieuxe)
        {
            GARADBEntities context = new GARADBEntities();
            var ListPhuTung = context.PHUTUNGs.Where(pt => pt.MA_HIEUXE.Equals(hieuxe)).Select(pt => "<option value='" + pt.ID + "' title='" + pt.SOLUONGTON + "' id='" + pt.MA_PHUTUNG + "' itemscope='" + pt.TG_BAOHANH + "' itemprop='" + pt.DONGIAXUAT + "'>" + pt.TEN_PHUTUNG + "</option>");
            return Content(String.Join("", ListPhuTung));
        }
        
    }
    
}