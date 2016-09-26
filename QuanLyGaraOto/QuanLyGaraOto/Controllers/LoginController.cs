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
            //ViewBag.ErrorMessage = "Sai ten dang nhap hoac mat khau!";
            return View();
        }
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
                        Session["Username"] = nv.USERNAME;
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