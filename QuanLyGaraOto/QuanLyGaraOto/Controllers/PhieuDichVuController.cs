using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QuanLyGaraOto.ViewModel;
using QuanLyGaraOto.Models;
namespace QuanLyGaraOto.Controllers
{
    public class PhieuDichVuController : Controller
    {
        // GET: PhieuDichVu
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult NhapPhieuDichVu()
        {
            GARADBEntities context = new GARADBEntities();
            PhieuDVViewModel vmPhieuDV = new PhieuDVViewModel();

            vmPhieuDV.ListPhuTung = context.PHUTUNGs.ToList();
            vmPhieuDV.ListTho = new List<THO>();

            vmPhieuDV.ListTienCong = context.TIENCONGs.ToList();
            vmPhieuDV.MaPhieuDV = 1;
            vmPhieuDV.MaPhieuTiepNhan = 1;
            vmPhieuDV.MaNV = 1;
            return View(vmPhieuDV);
        }
        [HttpPost]
        public ActionResult NhapPhieuDichVu(PhieuDVViewModel viewmodel, IEnumerable<CHITIET_PHIEUDV> listChiTiet)
        {

            return NhapPhieuDichVu();
        }
        [HttpGet]
        public ActionResult SuaPhieuDichVu()
        {
            GARADBEntities context = new GARADBEntities();
            PhieuDVViewModel vmPhieuDV = new PhieuDVViewModel();
            vmPhieuDV.ListPhuTung = context.PHUTUNGs.ToList();
            vmPhieuDV.ListTho = new List<THO>();
            vmPhieuDV.ListChiTietPhieu = new List<CHITIET_PHIEUDV>();
            vmPhieuDV.ListTienCong = context.TIENCONGs.ToList();
            vmPhieuDV.MaPhieuDV = 1;
            vmPhieuDV.MaPhieuTiepNhan = 1;
            vmPhieuDV.MaNV = 1;
            return View(vmPhieuDV);
        }

        [HttpPost]
        public JsonResult Xoa(int id)
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