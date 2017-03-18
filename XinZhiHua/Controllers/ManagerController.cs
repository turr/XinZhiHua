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
        #region 登录

        public ActionResult LogOn()
        {
            ViewBag.Title = "登录";
            return View();
        }

        [HttpPost]
        public ActionResult LogOnPost()
        {
            string name = Request.Form["name"];
            string password = Request.Form["password"];
            if ((!string.IsNullOrEmpty(name)) && (!string.IsNullOrEmpty(password)))
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

        #endregion

        #region 首页

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

        #endregion

        #region 公司简介

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
                result = SaveData(type, content);
            }
            string message = result == true ? "保存成功" : "保存失败";
            return RedirectToRoute(new { controller = "Manager", action = "Introduction", message = message });
        }

        #endregion

        #region 公司宗旨
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
        #endregion

        #region 公司荣誉
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
        #endregion

        #region 新闻动态
        [Authorize]
        public ActionResult New(int page = 1)
        {
            ViewBag.Title = "后台管理(新闻动态)";

            double page_size = 5;

            INewService service = new NewService();
            List<New> data = service.Table().ToList();
            ViewBag.AllCount = data.Count();

            double all_page = Math.Ceiling(data.Count() / page_size);
            ViewBag.AllPage = all_page;

            if (page < 1)
            {
                page = 1;
            }else if (page > all_page)
            {
                page =(int) all_page;
            }
            ViewBag.NowPage = page;



            List<New> p_data = data.Skip((int)page_size * (page - 1)).Take((int)page_size).ToList();
            string page_data = data == null ? "" : Newtonsoft.Json.JsonConvert.SerializeObject(p_data);
            ViewBag.Data = page_data;
            return View();
        }

        [Authorize]
        public ActionResult NewEdit(int id = 0)
        {
            ViewBag.Title = "后台管理(新闻动态|添加/修改)";
            string page_data = "";
            try
            {
                INewService service = new NewService();
                New data = service.Table().Where(M => M.Id == id).FirstOrDefault();
                page_data = data == null ? "" : Newtonsoft.Json.JsonConvert.SerializeObject(data);
            }
            catch
            {
            }
            ViewBag.Data = page_data;
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult NewEditPost()
        {
            int result = 0;

            int id = 0;
            if(Request.Form["id"] != null)
            {
                int.TryParse(Request.Form["id"].ToString(),out id);
            }
            string title = Request.Form["title"] == null ? "" : Request.Form["title"];
            string detail = Request.Form["detail"] == null ? "" : Request.Form["detail"];
            string content = Request.Form["content"] == null ? "" : Request.Form["content"];

            try
            {
                INewService service = new NewService();
                New data = service.Table().Where(M=>M.Id == id).FirstOrDefault();
                if (data == null )
                {
                    data = new New();
                    data.Title = title;
                    data.Detail = detail;
                    data.Content = content;
                    data.AddTime = DateTime.Now;
                    data.Img = "";
                    if (!string.IsNullOrEmpty(data.Title))
                    {
                        service.Insert(data);
                        result = data.Id;
                    }
                }
                else
                {
                    data.Title = title;
                    data.Detail = detail;
                    data.Content = content;
                    service.Update(data);
                    result = data.Id;
                }
            }
            catch
            {

            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult NewDelete(int id = 0)
        {
            try
            {
                INewService service = new NewService();
                service.Delete(id);
            }
            catch
            {
            }
            return RedirectToAction("New", "Manager");
        }

        #endregion

        [Authorize]
        public ActionResult Product()
        {
            ViewBag.Title = "后台管理(产品中心)";
            return View();
        }

        [Authorize]
        public ActionResult ProductEdit()
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
        public ActionResult ShowEdit()
        {
            ViewBag.Title = "后台管理(产品展示)";
            return View();
        }

        #region 联系我们
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
        #endregion


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

                        string from = Request.QueryString["from"];
                        if (from != null &&  from.ToString().ToLower() == "new")
                        {
                            path += "New/";
                        }
                        if (Directory.Exists(Request.MapPath(path)) == false)//如果不存在就创建file文件夹
                        {
                            Directory.CreateDirectory(Request.MapPath(path));
                        }
                        var file = Path.Combine(Request.MapPath(path), Path.GetFileName(file_name));
                        f.SaveAs(file);

                        if (from == "new")
                        {
                            int data_id = 0;
                            if(Request.QueryString["dataId"] != null)
                            {
                                int.TryParse(Request.QueryString["dataId"].ToString(),out data_id);
                            }
                            INewService service = new NewService();
                            New new_data =  service.GetByID(data_id);
                            if(new_data != null)
                            {
                                new_data.Img = path + file_name;
                                service.Update(new_data);
                            }
                        }
                        else
                        {
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


        //-------------------------------------

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