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
    public class PhieuBanLeController : Controller
    {
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
                var target = context.PHIEU_BANLE.Single(pbl => pbl.ID_PHIEUBANLE == id);
                if(context.PHIEU_THUTIEN.Any(pt => pt.ID_PHIEUBANLE == target.ID_PHIEUBANLE))
                {
                    return Json(new { value = "-1", message = "Không thể xóa do đã lập phiếu thu!" }, JsonRequestBehavior.AllowGet);
                }
                context.PHIEU_BANLE.Remove(target);
                context.SaveChanges();
                return Json(new { value = "1", message = "Xóa thành công!" }, JsonRequestBehavior.AllowGet);
            }
            catch(Exception)
            {
                return Json(new { value = "-1", message = "Không thể xóa do đã có tham chiếu!" }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public ActionResult NhapPhieuBanLe()
        {
            PhieuBanLeViewModel vmPhieuBanLe = new PhieuBanLeViewModel();
            GARADBEntities context = new GARADBEntities();
            vmPhieuBanLe.ListKhachHang = context.KHACHHANGs.ToList();
            vmPhieuBanLe.ListPhuTung = context.PHUTUNGs.ToList();
            vmPhieuBanLe.PhieuBanLe = new PHIEU_BANLE();
            vmPhieuBanLe.TenNV = "";
            //vmPhieuBanLe.TenNV = context.NHANVIENs.Single(s => s.USERNAME.Equals(Session["Username"])).HOTEN;
            return View(vmPhieuBanLe);
        }
        [HttpPost]
        public ActionResult NhapPhieuBanLe(PHIEU_BANLE phieubanle, List<CHITIET_PHIEUBANLE> listchitiet) 
        {
            GARADBEntities context = new GARADBEntities();
            phieubanle.NgayLap = DateTime.Now.Date;
            phieubanle.MaNV = context.NHANVIENs.Single(nv => nv.USERNAME.Equals(Session["Username"])).MA_NV;

           // phieubanle.HanChotThanhToan =
            context.PHIEU_BANLE.Add(phieubanle);
            foreach(var item in listchitiet)
            {
                item.ID_PHIEUBANLE = phieubanle.ID_PHIEUBANLE;
                context.CHITIET_PHIEUBANLE.Add(item);
            }
            context.SaveChanges();
            return View();
        }
        [HttpGet]
        public ActionResult SuaPhieuBanLe()
        {
            GARADBEntities context = new GARADBEntities();
            PhieuBanLeViewModel vmPhieuBanLe = new PhieuBanLeViewModel();
            vmPhieuBanLe.ListChiTietPhieu = new List<CHITIET_PHIEUBANLE>();
            vmPhieuBanLe.TenKH = "";
            vmPhieuBanLe.TenNV = "";
            vmPhieuBanLe.ListPhuTung = context.PHUTUNGs.ToList();
            vmPhieuBanLe.PhieuBanLe = new PHIEU_BANLE();
            return View(vmPhieuBanLe);
        }
        [HttpPost]
        public ActionResult SuaPhieuBanLe(PhieuBanLeViewModel vmPhieuBanLe)
        {
            return RedirectToAction("Index");
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
                    return Json(new { value = "-1", message = "Không thể xóa phiếu đã lập phiếu thu!" });
                }
                context.CHITIET_PHIEUBANLE.Remove(target);
                context.SaveChanges();
                return Json(new { value = "1", message = "Xóa chi tiết phiếu thành công!" });
            }
            catch(Exception)
            {
                return Json(new { value = "-1", message = "Đã có lỗi xảy ra không thể xóa!" });
            }
        }
    }
}