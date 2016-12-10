using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QuanLyGaraOto.Models;
using QuanLyGaraOto.App_Start;
namespace QuanLyGaraOto.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult LoginForm()
        {
            Session["Username"] = null;
            Session["UserID"] = null;
            Session["staff_name"] = null; // save name of the staff
            //ViewBag.ErrorMessage = "Sai ten dang nhap hoac mat khau!";
            return View();
        }
        //private void SetUserPermission(int permission)
        //{
            
        //    switch(permission)
        //    {
        //        case 1: { break; }
        //        case 2: {
                    
        //            Session["Menu_SuaChua"] = "none";
        //            Session["Menu_TiepNhan"] = "none";
        //            Session["Menu_NhapHang"] = "none";
        //            Session["Menu_ThietLap"] = "none";
        //                break; }
        //        case 3: {
        //            Session["Menu_ThietLap"] = "none";
        //            break; }
        //    }
        //}
        [HttpPost]
        public ActionResult LoginForm(NHANVIEN user)
        {
            GARADBEntities context = new GARADBEntities();
            NHANVIEN nv = null;
            
            try
            {
                nv =context.NHANVIENs.Single(u => u.USERNAME.Equals(user.USERNAME));
                if(nv != null)
                {
                    if(nv.PASSWORD.Equals(MD5Encryptor.MD5Hash(user.PASSWORD)))
                    {
                        int permissionLevel = context.NHOMNGUOIDUNGs.Single(gr => gr.MA_NHOMNGUOIDUNG == nv.MA_NHOMNGUOIDUNG.Value).CAPDO.Value;
                        //string groupUser = context.NHOMNGUOIDUNGs.Single(gr => gr.MA_NHOMNGUOIDUNG == nv.MA_NHOMNGUOIDUNG.Value).TEN_NHOM;
                        //SetUserPermission(permissionLevel);
                        Session.Timeout = 30;
                        Session["Username"] = nv.USERNAME;
                        Session["UserID"] = nv.MA_NV;
                        Session["staff_name"] = nv.HOTEN;
                        ViewBag.Username = nv.USERNAME;
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "Sai mật khẩu. Vui lòng đăng nhập lại!";
                        return View();
                    }
                }
            }
            catch(Exception)
            {
                ViewBag.ErrorMessage = "Sai tên đăng nhập. Vui lòng nhập lại!";
                return View();
            }
            return View();
        }
    }
}