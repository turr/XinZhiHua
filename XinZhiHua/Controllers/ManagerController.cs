using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

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
        public ActionResult LogOnPost()
        {
            string name =  Request.Form["name"];
            string password = Request.Form["password"];
            if((!string.IsNullOrEmpty(name)) && (!string.IsNullOrEmpty(password)))
            {
                Manager result = null;
                IManagerService service = new ManagerService();
                try
                {
                    result = service.Table().Where(M => M.Name == name && M.Password == password).FirstOrDefault();
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
           
            return View("Logon");
        }

        [Authorize]
        public ActionResult Index()
        {
            ViewBag.Title = "后台管理(首页)";
            string action_name = RouteData.Values["action"].ToString().ToLower();
            List<Setting> data = GetData(action_name).ToList();
            string page_data = data == null ? "" : Newtonsoft.Json.JsonConvert.SerializeObject(data);
            ViewBag.Data = page_data;
            return View();

        }

        [Authorize]
        [HttpPost]
        public ActionResult IndexPost()
        {
            bool result = false;

            string content = Request.Form["content"] == null ? "" : Request.Form["content"];
            if(Request.Form["type"]!= null)
            {
                string type = Request.Form["type"].ToString().ToLower();
                result =  SaveData(type, content);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult Introduction(string message = "")
        {
            ViewBag.Title = "后台管理(公司简介)";
            ViewBag.AlertMessage = message;
            string action_name = RouteData.Values["action"].ToString().ToLower();
            Setting data = GetData(action_name).FirstOrDefault();
            string page_data = data == null ? "" : Newtonsoft.Json.JsonConvert.SerializeObject(data);
            ViewBag.Data = page_data;
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult IntroductionPost()
        {
            bool result = false;
            string content = Request.Form["save_content"] == null ? "" : Request.Form["save_content"];
            if (Request.Form["type"] != null)
            {
                string type = Request.Form["type"].ToString().ToLower();
                result=  SaveData(type, content);
            }
            string message = result == true ? "保存成功" : "保存失败";
            return RedirectToRoute(new { controller = "Manager", action = "Introduction", message = message });
        }

        [Authorize]
        public ActionResult Statement(string message = "")
        {
            ViewBag.Title = "后台管理(公司宗旨)";
            ViewBag.AlertMessage = message;
            string action_name = RouteData.Values["action"].ToString().ToLower();
            Setting data = GetData(action_name).FirstOrDefault();
            string page_data = data == null ? "" : Newtonsoft.Json.JsonConvert.SerializeObject(data);
            ViewBag.Data = page_data;
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult StatementPost()
        {
            bool result = false;
            string content = Request.Form["save_content"] == null ? "" : Request.Form["save_content"];
            if (Request.Form["type"] != null)
            {
                string type = Request.Form["type"].ToString().ToLower();
                result = SaveData(type, content);
            }
            string message = result == true ? "保存成功" : "保存失败";
            return RedirectToRoute(new { controller = "Manager", action = "Statement", message = message });

        }

        [Authorize]
        public ActionResult Honor(string message = "")
        {
            ViewBag.Title = "后台管理(公司荣誉)";
            ViewBag.AlertMessage = message;
            string action_name = RouteData.Values["action"].ToString().ToLower();
            Setting data = GetData(action_name).FirstOrDefault();
            string page_data = data == null ? "" : Newtonsoft.Json.JsonConvert.SerializeObject(data);
            ViewBag.Data = page_data;
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult HonorPost()
        {
            bool result = false;
            string content = Request.Form["save_content"] == null ? "" : Request.Form["save_content"];
            if (Request.Form["type"] != null)
            {
                string type = Request.Form["type"].ToString().ToLower();
                result = SaveData(type, content);
            }
            string message = result == true ? "保存成功" : "保存失败";
            return RedirectToRoute(new { controller = "Manager", action = "Honor", message = message });

        }

        [Authorize]
        public ActionResult New()
        {
            ViewBag.Title = "后台管理(新闻动态)";
            return View();
        }

        [Authorize]
        public ActionResult Product()
        {
            ViewBag.Title = "后台管理(产品中心)";
            return View();
        }

        [Authorize]
        public ActionResult Show()
        {
            ViewBag.Title = "后台管理(产品展示)";
            return View();
        }

        [Authorize]
        public ActionResult Contact(string message = "")
        {
            ViewBag.Title = "后台管理(联系我们)";
            ViewBag.AlertMessage = message;
            string action_name = RouteData.Values["action"].ToString().ToLower();
            Setting data = GetData(action_name).FirstOrDefault();
            string page_data = data == null ? "" : Newtonsoft.Json.JsonConvert.SerializeObject(data);
            ViewBag.Data = page_data;
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult ContactPost()
        {
            bool result = false;
            string content = Request.Form["save_content"] == null ? "" : Request.Form["save_content"];
            if (Request.Form["type"] != null)
            {
                string type = Request.Form["type"].ToString().ToLower();
                result = SaveData(type, content);
            }
            string message = result == true ? "保存成功" : "保存失败";
            return RedirectToRoute(new { controller = "Manager", action = "Contact", message = message });

        }


        [Authorize]
        [HttpPost]
        public bool UploadImg()
        {
            bool result = false;
            try
            {
                string type = Request.QueryString["key"];
                if (!string.IsNullOrEmpty(type))
                {
                    type = type.ToString().ToLower();

                    if (Request.Files.Count > 0)
                    {
                        HttpPostedFileBase f = Request.Files[0];
                        string file_name = type+"_" + DateTime.Now.ToString("yyMMddHHmmss") + f.FileName.Substring(f.FileName.LastIndexOf("."));
                        string path = "~/Content/Img/";
                        var file = Path.Combine(Request.MapPath(path), Path.GetFileName(file_name));
                        f.SaveAs(file);

                        ISettingService service = new SettingService();
                        Setting data = service.Table().Where(M => M.Type == type).FirstOrDefault();
                        if (data == null)
                        {
                            data = new Setting();
                            data.Type = type;
                            data.Content = "";
                            data.Img = path + file_name;
                            service.Insert(data);
                        }
                        else
                        {
                            data.Img = path + file_name;
                            service.Update(data);
                        }

                        result = true;
                    }
                }
            }
            catch
            {
            }
            return result;
        }


        private IQueryable<Setting> GetData(string type)
        {
            try
            {
                ISettingService service = new SettingService();
                return service.Table().Where(M => M.Type.IndexOf(type) != -1);
            }
            catch
            {
            }
            return null;
            
        }

        private bool SaveData(string type ,string content)
        {
            try
            {
                ISettingService service = new SettingService();
                Setting data = service.Table().Where(M => M.Type == type).FirstOrDefault();
                if (data == null)
                {
                    data = new Setting();
                    data.Type = type;
                    data.Content = content;
                    data.Img = "";
                    service.Insert(data);
                }
                else
                {
                    data.Content = content;
                    service.Update(data);
                }
                return true;
            }
            catch
            {
            }
            return false;
        }
    }
}