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
        public ActionResult NhapPhieuDichVu(int? maphieutiepnhan)
        {
            GARADBEntities context = new GARADBEntities();
            PhieuDVViewModel vmPhieuDV = new PhieuDVViewModel();
            vmPhieuDV.PhieuDichVu = new PHIEU_DICHVU();
            vmPhieuDV.ListPhuTung = context.PHUTUNGs.ToList();
            vmPhieuDV.ListTho = new List<THO>();
            vmPhieuDV.ListTho = context.THOes.ToList();
            vmPhieuDV.ListTienCong = context.TIENCONGs.ToList();
            BANGTHAMSO a = context.BANGTHAMSOes.Single(ts => ts.TENTHAMSO.Equals("HanChotPhieuDichVu"));
            vmPhieuDV.PhieuDichVu.NGAYLAP = DateTime.Now.Date;
            vmPhieuDV.PhieuDichVu.HANCHOTTHANHTOAN = DateTime.Now.Date.AddDays(int.Parse(a.GIATRI)).Date;
            if (maphieutiepnhan != null)
            {
                string bienso = context.PHIEU_TIEPNHAN.Single(ptn => ptn.MA_PHIEUTIEPNHAN == maphieutiepnhan.Value).BIENSO_XE;
                string hieuxe = context.XEs.Single(x => x.BS_XE.Equals(bienso)).HIEU_XE;
                vmPhieuDV.ListHieuXe = context.HIEUXEs.Where(hx => hx.MA_HIEUXE.Equals(hieuxe) || hx.MA_HIEUXE.Equals("Tất cả")).ToList();
            }
            else
                vmPhieuDV.ListHieuXe = context.HIEUXEs.ToList();



            return View(vmPhieuDV);
        }
        [HttpPost]
        public ActionResult NhapPhieuDichVu(PhieuDVViewModel viewmodel, IEnumerable<CHITIET_PHIEUDV> listChiTiet)
        {
            List<CHITIET_PHIEUDV> listCT = listChiTiet.ToList();
            try
            {
                var listdelete = listCT.Where(ct => ct.MA_PT == 0).ToList();
                foreach (var i in listdelete)
                {
                    listCT.Remove(i);
                }
            }
            catch (Exception) { }
            GARADBEntities context = new GARADBEntities();
            //viewmodel.PhieuDichVu.MA_NHANVIEN = context.NHANVIENs.Single(nv => nv.USERNAME.Equals(Session["Username"])).MA_NV;
            viewmodel.PhieuDichVu.NGAYLAP = DateTime.Now.Date;
            BANGTHAMSO a = context.BANGTHAMSOes.Single(ts => ts.TENTHAMSO.Equals("HanChotPhieuDichVu"));

            viewmodel.PhieuDichVu.HANCHOTTHANHTOAN = DateTime.Now.Date.AddDays(int.Parse(a.GIATRI)).Date;
            viewmodel.PhieuDichVu.SOTIEN_CONLAI = viewmodel.PhieuDichVu.TIENCONG;
            viewmodel.PhieuDichVu.TONGTIEN = viewmodel.PhieuDichVu.TIENCONG;
            //viewmodel.PhieuDichVu.TONGTIEN = listCT.Sum(ct => ct.THANHTIEN);
            context.PHIEU_DICHVU.Add(viewmodel.PhieuDichVu);
            context.SaveChanges();

            foreach (var item in listChiTiet)
            {
                item.ID_PHIEUDV = viewmodel.PhieuDichVu.ID_PHIEUDV;
                context.CHITIET_PHIEUDV.Add(item);
            }
            context.SaveChanges();
            //TempData["msg"] = "<script>alert('Đã thêm thành công!');</script>";
            TempData["msg"] = @"<div id=""rowSuccess"" class=""row""> <div class=""col-sm-10""> <div class=""alert alert-success alert-dismissable fade in"" style=""padding-top: 5px; padding-bottom: 5px""> <a href=""#"" class=""close"" data-dismiss=""alert"" aria-label=""close"">&times;</a> Thêm mới thành công! </div> </div> </div>";
            return RedirectToAction("Index", new { sortOrder = String.Empty, currentFilter = String.Empty, searchString = String.Empty });
        }
        [HttpGet]
        public ActionResult SuaPhieuDichVu(int? id)
        {
            
            GARADBEntities context = new GARADBEntities();
            if(context.PHIEU_THUTIEN.Any(pt => pt.ID_PHIEUDV == id.Value))
            {
                TempData["msg"] = @"<div id=""rowError"" class=""row""> <div class=""col-sm-10""> <div class=""alert alert-danger alert-dismissable fade in"" style=""padding-top: 5px; padding-bottom: 5px""> <a href=""#"" class=""close"" data-dismiss=""alert"" aria-label=""close"">&times;</a> Không thể sửa phiếu dịch vụ đã lập phiếu thu tiền! </div> </div> </div>";
                //TempData["msg"] = "<script>alert('Không thể sửa phiếu dịch vụ đã lập phiếu thu tiền!');</script>";
                return RedirectToAction("Index", new { sortOrder = String.Empty, currentFilter = String.Empty, searchString = String.Empty });
            }
            PhieuDVViewModel vmPhieuDV = new PhieuDVViewModel();
            vmPhieuDV.PhieuDichVu = context.PHIEU_DICHVU.Single(p => p.ID_PHIEUDV == id.Value);
            
            vmPhieuDV.ListPhuTung = context.PHUTUNGs.ToList();
            vmPhieuDV.ListTho = new List<THO>();
            vmPhieuDV.ListTho = context.THOes.ToList();
            vmPhieuDV.ListChiTietPhieu = new List<CHITIET_PHIEUDV>();
            vmPhieuDV.ListChiTietPhieu = context.CHITIET_PHIEUDV.Where(ct => ct.ID_PHIEUDV == id.Value).ToList();
            vmPhieuDV.ListHieuXe = context.HIEUXEs.ToList();
            
            return View(vmPhieuDV);
            //vmPhieuDV.MaPhieuDV = "PDV001";
            //vmPhieuDV.MaPhieuTiepNhan = 3;
            //vmPhieuDV.MaNV = 1;
            //vmPhieuDV.ID_PhieuDV = 5;
            //TempData["msg"] = "<script>alert('Đã sửa thành công!');</script>";
            //return RedirectToAction("Index", new { sortOrder = String.Empty, currentFilter = String.Empty, searchString = String.Empty });
        }
        [HttpPost]
        public ActionResult SuaPhieuDichVu(PhieuDVViewModel viewModel, List<CHITIET_PHIEUDV> listChiTiet)
        {
            GARADBEntities context = new GARADBEntities();
            var target = context.PHIEU_DICHVU.Single(pdv => pdv.ID_PHIEUDV == viewModel.PhieuDichVu.ID_PHIEUDV);
            target.MATHO = viewModel.PhieuDichVu.MATHO;
            decimal tiencongcu = target.TIENCONG.Value;
            decimal chenhlech = viewModel.PhieuDichVu.TIENCONG.Value - tiencongcu;
            target.TIENCONG += viewModel.PhieuDichVu.TIENCONG;
            target.TONGTIEN += chenhlech;
            target.SOTIEN_CONLAI += chenhlech;

            context.SaveChanges();
            TempData["msg"] = @"<div id=""rowSuccess"" class=""row""> <div class=""col-sm-10""> <div class=""alert alert-success alert-dismissable fade in"" style=""padding-top: 5px; padding-bottom: 5px""> <a href=""#"" class=""close"" data-dismiss=""alert"" aria-label=""close"">&times;</a> Cập nhật thành công! </div> </div> </div>";
            return RedirectToAction("Index", new { sortOrder = "", currentFilter = "", searchString = "" });
        }
        [HttpPost]
        public JsonResult Xoa(int id)
        {
            try
            {
                GARADBEntities context = new GARADBEntities();
                var target = context.PHIEU_DICHVU.Single(pdv => pdv.ID_PHIEUDV == id);
                

                if (context.PHIEU_THUTIEN.Any(pt => pt.ID_PHIEUDV == target.ID_PHIEUDV))
                {
                    TempData["msg"] = @"<div id=""rowError"" class=""row""> <div class=""col-sm-10""> <div class=""alert alert-danger alert-dismissable fade in"" style=""padding-top: 5px; padding-bottom: 5px""> <a href=""#"" class=""close"" data-dismiss=""alert"" aria-label=""close"">&times;</a> Không thể xóa phiếu đã lập phiếu thu! </div> </div> </div>";
                    return Json(new { value = "-1", message = "Không thể xóa phiếu đã lập phiếu thu!" }, JsonRequestBehavior.AllowGet);

                }
                else
                {

                    List<CHITIET_PHIEUDV> listCT = context.CHITIET_PHIEUDV.Where(ct => ct.ID_PHIEUDV == id).ToList();
                    foreach(var item in listCT)
                    {
                        context.CHITIET_PHIEUDV.Remove(item);
                        context.SaveChanges();
                    }
                    context.PHIEU_DICHVU.Remove(target);
                    TempData["msg"] = @"<div id=""rowSuccess"" class=""row""> <div class=""col-sm-10""> <div class=""alert alert-danger alert-dismissable fade in"" style=""padding-top: 5px; padding-bottom: 5px""> <a href=""#"" class=""close"" data-dismiss=""alert"" aria-label=""close"">&times;</a> Xóa thành công! </div> </div> </div>";
                    return Json(new { value = "1", message = "Xóa thành công!" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch(Exception)
            {
                TempData["msg"] = @"<div id=""rowError"" class=""row""> <div class=""col-sm-10""> <div class=""alert alert-danger alert-dismissable fade in"" style=""padding-top: 5px; padding-bottom: 5px""> <a href=""#"" class=""close"" data-dismiss=""alert"" aria-label=""close"">&times;</a> Không thể xóa phiếu! </div> </div> </div>";
                return Json(new { value = "-1", message = "Không thể xóa phiếu!" }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult XoaChiTiet(int id)
        {
           // try
          //  {
                GARADBEntities context = new GARADBEntities();
                var target = context.CHITIET_PHIEUDV.Single(ct => ct.ID == id);
                if(context.PHIEU_THUTIEN.Any(pt => pt.ID_PHIEUDV == target.ID_PHIEUDV))
                {
                    TempData["msg"] = @"<div id=""rowError"" class=""row""> <div class=""col-sm-10""> <div class=""alert alert-danger alert-dismissable fade in"" style=""padding-top: 5px; padding-bottom: 5px""> <a href=""#"" class=""close"" data-dismiss=""alert"" aria-label=""close"">&times;</a> Không thể xóa chi tiết phiếu đã lập phiếu thu! </div> </div> </div>";
                    return Json(new { value = "-1", message = "Không thể xóa do đã lập phiếu thu tiền!" }, JsonRequestBehavior.AllowGet);
                }
                context.CHITIET_PHIEUDV.Remove(target);
                context.SaveChanges();
                TempData["msg"] = @"<div id=""rowSucess"" class=""row""> <div class=""col-sm-10""> <div class=""alert alert-success alert-dismissable fade in"" style=""padding-top: 5px; padding-bottom: 5px""> <a href=""#"" class=""close"" data-dismiss=""alert"" aria-label=""close"">&times;</a> Xóa thành công! </div> </div> </div>";
                return Json(new { value = "1", message = "Xóa thành công!" }, JsonRequestBehavior.AllowGet);
          //  }
          //  catch(Exception)
           // {
                TempData["msg"] = @"<div id=""rowError"" class=""row""> <div class=""col-sm-10""> <div class=""alert alert-danger alert-dismissable fade in"" style=""padding-top: 5px; padding-bottom: 5px""> <a href=""#"" class=""close"" data-dismiss=""alert"" aria-label=""close"">&times;</a> Không thể xóa chi tiết phiếu! Vui lòng thử lại! </div> </div> </div>";
                return Json(new { value = "-1", message = "Không thể xóa chi tiết phiếu!" }, JsonRequestBehavior.AllowGet);
           // }

        }
        [HttpPost]
        public JsonResult ThemChiTiet(CHITIET_PHIEUDV chitiet)
        {
            GARADBEntities context = new GARADBEntities();
            context.CHITIET_PHIEUDV.Add(chitiet);
            context.SaveChanges();
            TempData["msg"] = @"<div id=""rowSucess"" class=""row""> <div class=""col-sm-10""> <div class=""alert alert-success alert-dismissable fade in"" style=""padding-top: 5px; padding-bottom: 5px""> <a href=""#"" class=""close"" data-dismiss=""alert"" aria-label=""close"">&times;</a> Thêm chi tiết thành công! </div> </div> </div>";
            return Json(new { issucess = "1", newid = "1", message = "OK" }, JsonRequestBehavior.AllowGet);
        }
     [HttpPost]
        public ActionResult GetPhuTungByHieuXe(string hieuxe)
        {
            GARADBEntities context= new GARADBEntities();
            var ListPhuTung = context.PHUTUNGs.Where(pt => pt.MA_HIEUXE.Equals(hieuxe)).Select(pt => "<option value='" + pt.ID + "' title='" + pt.SOLUONGTON + "' id='" + pt.MA_PHUTUNG + "' itemscope='"+ pt.TG_BAOHANH + "' itemprop='" + pt.DONGIAXUAT + "'>"+pt.TEN_PHUTUNG +"</option>");
            return Content(String.Join("",ListPhuTung));
        }
    }
}