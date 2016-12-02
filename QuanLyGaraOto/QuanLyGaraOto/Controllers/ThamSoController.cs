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


            int UserId = int.Parse(Session["UserID"].ToString());
            NHANVIEN nv = context.NHANVIENs.Single(staff => staff.MA_NV == UserId);
            NHOMNGUOIDUNG groupuser = context.NHOMNGUOIDUNGs.Single(gu => gu.MA_NHOMNGUOIDUNG == nv.MA_NHOMNGUOIDUNG.Value);
            if (groupuser.CAPDO != 1 && groupuser.CAPDO != 2)
            {
                TempData["msg"] = @"<div id=""rowError"" class=""row""> <div class=""col-sm-10""> <div class=""alert alert-danger alert-dismissable fade in"" style=""padding-top: 5px; padding-bottom: 5px""> <a href=""#"" class=""close"" data-dismiss=""alert"" aria-label=""close"">&times;</a> Bạn không có quyền truy cập vào chức năng này! </div> </div> </div>";
                return RedirectToAction("Index", "Home");
            }

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
        public ActionResult SuaThamSo(string tenthamso)
        {
            GARADBEntities context = new GARADBEntities();
            
            BANGTHAMSO modelThamSo = new BANGTHAMSO();
            
            modelThamSo = context.BANGTHAMSOes.Single(ts => ts.TENTHAMSO.Equals(tenthamso));
            return View(modelThamSo);
        }
        [HttpPost]
        public ActionResult SuaThamSo(BANGTHAMSO thamso)
        {
            GARADBEntities context = new GARADBEntities();
            try
            {
                var target = context.BANGTHAMSOes.Single(ts => ts.TENTHAMSO.Equals(thamso.TENTHAMSO));
                target.GIATRI = thamso.GIATRI;
                TempData["msg"] = "<script>alert('Đã cập nhật thành công!');</script>";
                return RedirectToAction("Index", new { currentFilter = String.Empty, searchString = String.Empty });
            }
            catch(Exception )
            {
                TempData["msg"] = "<script>alert('Không thể cập nhật. Vui lòng thử lại!');</script>";
                return RedirectToAction("Index", new { currentFilter = String.Empty, searchString = String.Empty });
            }
           
        }
    }
}