using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using XinZhiHua.Models.Req;

namespace XinZhiHua.Controllers
{
    public class ManagerController : Controller
    {
        public ActionResult LogOn()
        {
            ViewBag.Title = "登录";
            return View();
        }

        [HttpPost]
        public ActionResult LogOn(ModLogOn model)
        {
            if (ModelState.IsValid)
            {
                Manager result = null;
                IManagerService service = new ManagerService();
                try
                {
                    result = service.Table().Where(M => M.Name == model.Name && M.Password == model.Password).FirstOrDefault();
                }
                catch
                {

                }

                if (result != null)
                {
                    //2.存储登陆名外,再添加角色权限
                    FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                        1, // 版本号。 
                        result.Id.ToString(), // 与身份验证票关联的用户ID。 
                        DateTime.Now, // Cookie 的发出时间。 
                        DateTime.Now.AddMinutes(30),// Cookie 的到期日期。 
                        false, // 如果 Cookie 是持久的，为 true；否则为 false。 
                        "");//将存储在 Cookie 中的用户定义数据。 
                    string encryptedTicket = FormsAuthentication.Encrypt(ticket);//加密
                    //存入Cookie 
                    HttpCookie authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                    Response.Cookies.Add(authCookie);

                    return RedirectToAction("Index", "Manager");
                }
                else
                {
                    ViewBag.SubmitError = "账号密码有误";
                }
            }
            else
            {
                ViewBag.SubmitError = "登录失败";
            }
            return View();
        }

        [Authorize]
        public ActionResult Index()
        {
            ViewBag.Title = "后台管理";
            return View();
        }

    }
}