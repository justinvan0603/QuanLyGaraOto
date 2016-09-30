using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QuanLyGaraOto.ViewModel;
using QuanLyGaraOto.Models;
using PagedList;
namespace QuanLyGaraOto.Controllers
{
    public class PhieuDichVuController : Controller
    {
        // GET: PhieuDichVu
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? searchOption, int? page)
        {
            ViewBag.CurrentSearchOption = searchOption;
            ViewBag.CurrentSort = sortOrder;
            ViewBag.DateSortParam = sortOrder == "date_asc" ? "date_desc" : "date_asc";
            ViewBag.TotalSortParam = sortOrder == "total_asc" ? "total_desc" : "total_asc";
            ViewBag.RemainSortParam = sortOrder == "remain_asc" ? "remain_desc" : "remain_desc";
            GARADBEntities context = new GARADBEntities();

            var dsPhieuDVQuery = (from pdv in context.PHIEU_DICHVU
                                  join nv in context.NHANVIENs on pdv.MA_NHANVIEN equals nv.MA_NV
                                  join tho in context.THOes on pdv.MATHO equals tho.MA_THO
                                  select new
                                  {
                                      Id_PhieuDV = pdv.ID_PHIEUDV,
                                      MaPhieuDV = pdv.MA_PHIEUDV,
                                      NgayLap = pdv.NGAYLAP,
                                      TenNV = nv.HOTEN,
                                      TenTho = tho.TENTHO,
                                      TongTien = pdv.TONGTIEN,
                                      SoTienConLai = pdv.SOTIEN_CONLAI,
                                      //TinhTrang = pdv.TINHTRANG
                                  }).ToList();
           
            List<DSPhieuDVTableData> listPhieuDV = new List<DSPhieuDVTableData>();
            foreach(var item in dsPhieuDVQuery)
            {
                DSPhieuDVTableData data= new DSPhieuDVTableData();
                data.Id_PhieuDV = item.Id_PhieuDV;
                data.MaPhieuDV = item.MaPhieuDV;
                data.NgayLap = item.NgayLap.Value;
                data.TenNV = item.TenNV;
                data.TenTho = item.TenTho;
                data.TongTien = item.TongTien.Value;
                data.SoTienConLai = item.SoTienConLai.Value;
                //data.TinhTrang = item.TinhTrang;
                listPhieuDV.Add(data);
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
                        case 1: { listPhieuDV = listPhieuDV.Where(c => c.MaPhieuDV.Contains(searchString)).ToList(); break; }
                        default: { break; }
                    }
                }

            }
            switch (sortOrder)
            {
                case "date_asc": { listPhieuDV = listPhieuDV.OrderBy(c => c.NgayLap).ToList(); break; }
                case "date_desc": { listPhieuDV = listPhieuDV.OrderByDescending(c => c.NgayLap).ToList(); break; }
                case "total_asc": { listPhieuDV = listPhieuDV.OrderBy(c => c.TongTien).ToList(); break; }
                case "total_desc": { listPhieuDV = listPhieuDV.OrderByDescending(c => c.TongTien).ToList(); break; }
                case "remain_asc": { listPhieuDV = listPhieuDV.OrderBy(c => c.SoTienConLai).ToList(); break; }
                case "remain_desc": { listPhieuDV = listPhieuDV.OrderByDescending(c => c.SoTienConLai).ToList(); break; }
                default: { break; }
            }
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            DSPhieuDVViewModel vmDSPhieuDV = new DSPhieuDVViewModel();
            vmDSPhieuDV.ListData = listPhieuDV.ToPagedList(pageNumber, pageSize);
            return View(vmDSPhieuDV);
        }
        [HttpGet]
        public ActionResult NhapPhieuDichVu()
        {
            GARADBEntities context = new GARADBEntities();
            PhieuDVViewModel vmPhieuDV = new PhieuDVViewModel();
            vmPhieuDV.PhieuDichVu = new PHIEU_DICHVU();
            vmPhieuDV.ListPhuTung = context.PHUTUNGs.ToList();
            vmPhieuDV.ListTho = new List<THO>();
            vmPhieuDV.ListTho = context.THOes.ToList();
            vmPhieuDV.ListTienCong = context.TIENCONGs.ToList();
            

            return View(vmPhieuDV);
        }
        [HttpPost]
        public ActionResult NhapPhieuDichVu(PhieuDVViewModel viewmodel, IEnumerable<CHITIET_PHIEUDV> listChiTiet)
        {
            GARADBEntities context = new GARADBEntities();
            viewmodel.PhieuDichVu.NGAYLAP = DateTime.Now.Date;
            context.PHIEU_DICHVU.Add(viewmodel.PhieuDichVu);
            context.SaveChanges();
            List<CHITIET_PHIEUDV> listCT = listChiTiet.ToList();
            foreach(var item in listChiTiet)
            {
                item.ID_PHIEUDV = viewmodel.PhieuDichVu.ID_PHIEUDV;
                context.CHITIET_PHIEUDV.Add(item);
            }
            context.SaveChanges();
            return NhapPhieuDichVu();
        }
        [HttpGet]
        public ActionResult SuaPhieuDichVu()
        {
            GARADBEntities context = new GARADBEntities();
            PhieuDVViewModel vmPhieuDV = new PhieuDVViewModel();
            vmPhieuDV.PhieuDichVu = context.PHIEU_DICHVU.Single(p => p.ID_PHIEUDV == 1);
            
            vmPhieuDV.ListPhuTung = context.PHUTUNGs.ToList();
            vmPhieuDV.ListTho = new List<THO>();
            vmPhieuDV.ListTho = context.THOes.ToList();
            vmPhieuDV.ListChiTietPhieu = new List<CHITIET_PHIEUDV>();

            //vmPhieuDV.MaPhieuDV = "PDV001";
            //vmPhieuDV.MaPhieuTiepNhan = 3;
            //vmPhieuDV.MaNV = 1;
            //vmPhieuDV.ID_PhieuDV = 5;
            return View(vmPhieuDV);
        }
        [HttpPost]
        public ActionResult SuaPhieuDichVu(PhieuDVViewModel viewModel, List<CHITIET_PHIEUDV> listChiTiet)
        {
            return View();
        }
        [HttpPost]
        public JsonResult XoaChiTiet(int id)
        {
            int result = 1;
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult ThemChiTiet(CHITIET_PHIEUDV chitiet)
        {
            return Json(new { issucess = "1", newid = "1", message = "OK" }, JsonRequestBehavior.AllowGet);
        }
    }
}