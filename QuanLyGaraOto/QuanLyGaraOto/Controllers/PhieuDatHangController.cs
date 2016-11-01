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
    public class PhieuDatHangController : Controller
    {
        // GET: PhieuDatHang
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? searchOption,
            int? page)
        {
            ViewBag.CurrentSearchOption = searchOption;
            ViewBag.CurrentSort = sortOrder;
            ViewBag.DateTakeOrderSortParam = (sortOrder == "take_asc" ? "take_desc" : "take_asc");   //Ban đầu là null -> ASC
            ViewBag.DatePlaceOrderSortParam = (sortOrder == "place_asc" ? "place_desc" : "place_asc");   //Ban đầu là null -> ASC
            GARADBEntities context = new GARADBEntities();
            List<PHIEU_DATHANG> listPDH = context.PHIEU_DATHANG.ToList();
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
                    switch (searchOption)
                    {
                        case 0: { break; }
                        case 1: { listPDH = listPDH.Where(c => c.MaPhieuDat.Contains(searchString)).ToList(); break; }
                        //case 2: { listPDH = listPDH.Where(c => c.NgayDat.Contains(searchString)).ToList(); break; }
                        //case 2: { listPDH = listPDH.Where(c => c.NgayDat.Contains(searchString)).ToList(); break; }
                        default: { break; }
                    }
                }

            }
            switch (sortOrder)
            {
                case "tate_asc": { listPDH = listPDH.OrderBy(c => c.NgayDat).ToList(); break; }
                case "tate_desc": { listPDH = listPDH.OrderByDescending(c => c.NgayDat).ToList(); break; }
                case "place_asc": { listPDH = listPDH.OrderBy(c => c.NgayGiao).ToList(); break; }
                case "place_desc": { listPDH = listPDH.OrderByDescending(c => c.NgayGiao).ToList(); break; }
                default: { break; }
            }
            int pageSize = 5;
            int pageNumber = (page ?? 1); //nếu page có giá trị thì = page, ko thì = 1
            PhieuThuViewModel pttViewModel = new PhieuThuViewModel();
            //pttViewModel.ListPhieuThu = listPDH.ToPagedList(pageNumber, pageSize);
            return View(pttViewModel);
        }
    }
}