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
    public class ThamSoController : Controller
    {
        // GET: ThamSo
        public ActionResult Index(string currentFilter, string searchString, int? page)
        {
            GARADBEntities context = new GARADBEntities();
            List<BANGTHAMSO> listThamSo = context.BANGTHAMSOes.ToList();
            ThamSoViewModel vmThamSo = new ThamSoViewModel();
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
                listThamSo = listThamSo.Where(p => p.TENTHAMSO.Contains(searchString)).ToList();

            }
            ViewBag.CurrentFilter = searchString;
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            vmThamSo.ListThamSo = listThamSo.ToPagedList(pageNumber, pageSize);
            return View(vmThamSo);
        }
        [HttpGet]
        public ActionResult SuaThamSo()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SuaThamSo(BANGTHAMSO thamso)
        {
            return View();
        }
    }
}