using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace XinZhiHua.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            ViewBag.Title = "首页";
            string action_name = RouteData.Values["action"].ToString().ToLower();
            string page_data = "";
            string product_data = "";
            try
            {
                ISettingService service = new SettingService();
                List<Setting> data = service.Table().Where(M => M.Type.IndexOf(action_name) != -1).ToList();
                page_data = data == null ? "" : Newtonsoft.Json.JsonConvert.SerializeObject(data);

                INewAndProductService product_service = new NewAndProductService();
                List<NewAndProduct> product = product_service.Table().Where(M=>M.Type == "productedit").Take(3).ToList();
                product_data = product == null ? "" : Newtonsoft.Json.JsonConvert.SerializeObject(product);
            }
            catch
            {
            }
            ViewBag.Data = page_data;
            ViewBag.ProductData = product_data;

            return View();
        }

        public ActionResult Introduction()
        {
            ViewBag.Title = "公司简介";
            string action_name = RouteData.Values["action"].ToString().ToLower();
            string page_data = "";
            try
            {
                ISettingService service = new SettingService();
                Setting data = service.Table().Where(M => M.Type.IndexOf(action_name) != -1).FirstOrDefault();
                page_data = data == null ? "" : Newtonsoft.Json.JsonConvert.SerializeObject(data);
            }
            catch
            {
            }
            ViewBag.Data = page_data;
            return View();
        }

        public ActionResult Statement()
        {
            ViewBag.Title = "公司宗旨";
            string action_name = RouteData.Values["action"].ToString().ToLower();
            string page_data = "";
            try
            {
                ISettingService service = new SettingService();
                Setting data = service.Table().Where(M => M.Type.IndexOf(action_name) != -1).FirstOrDefault();
                page_data = data == null ? "" : Newtonsoft.Json.JsonConvert.SerializeObject(data);
            }
            catch
            {
            }
            ViewBag.Data = page_data;
            return View();
        }

        public ActionResult Honor()
        {
            ViewBag.Title = "公司荣誉";
            string action_name = RouteData.Values["action"].ToString().ToLower();
            string page_data = "";
            try
            {
                ISettingService service = new SettingService();
                Setting data = service.Table().Where(M => M.Type.IndexOf(action_name) != -1).FirstOrDefault();
                page_data = data == null ? "" : Newtonsoft.Json.JsonConvert.SerializeObject(data);
            }
            catch
            {
            }
            ViewBag.Data = page_data;
            return View();
        }

        public ActionResult New(int page =1)
        {
            string action_name = RouteData.Values["action"].ToString().ToLower();

            ViewBag.Title = "新闻动态";

            double page_size = 8;

            INewAndProductService service = new NewAndProductService();
            List<NewAndProduct> data = service.Table().Where(M => M.Type.IndexOf(action_name) != -1).ToList();
            ViewBag.AllCount = data.Count();

            double all_page = Math.Ceiling(data.Count() / page_size);
            ViewBag.AllPage = all_page;

            if (page < 1)
            {
                page = 1;
            }
            else if (page > all_page)
            {
                page = (int)all_page;
            }
            ViewBag.NowPage = page;

            List<NewAndProduct> p_data = data.Skip((int)page_size * (page - 1)).Take((int)page_size).OrderBy(M => M.AddTime).ToList();
            string page_data = data == null ? "" : Newtonsoft.Json.JsonConvert.SerializeObject(p_data);
            ViewBag.Data = page_data;

            return View();
        }

        public ActionResult NewDetail(int id = 0)
        {
            string page_data = "";
            try
            {
                INewAndProductService service = new NewAndProductService();
                NewAndProduct data = service.Table().Where(M => M.Id == id).FirstOrDefault();
                if(data == null)
                {
                    return RedirectToAction("New","Home");
                }
                else
                {
                    page_data =  Newtonsoft.Json.JsonConvert.SerializeObject(data);
                }
            }
            catch
            {
            }
            ViewBag.Data = page_data;
            return View();
        }

        public ActionResult Product(int page =1)
        {

            string action_name = RouteData.Values["action"].ToString().ToLower();

            ViewBag.Title = "产品中心";

            double page_size = 6;

            INewAndProductService service = new NewAndProductService();
            List<NewAndProduct> data = service.Table().Where(M => M.Type.IndexOf(action_name) != -1).ToList();
            ViewBag.AllCount = data.Count();

            double all_page = Math.Ceiling(data.Count() / page_size);
            ViewBag.AllPage = all_page;

            if (page < 1)
            {
                page = 1;
            }
            else if (page > all_page)
            {
                page = (int)all_page;
            }
            ViewBag.NowPage = page;

            List<NewAndProduct> p_data = data.Skip((int)page_size * (page - 1)).Take((int)page_size).OrderBy(M => M.AddTime).ToList();
            string page_data = data == null ? "" : Newtonsoft.Json.JsonConvert.SerializeObject(p_data);
            ViewBag.Data = page_data;
            return View();
        }

        public ActionResult ProductDetail(int id = 0)
        {
            string page_data = "";
            try
            {
                INewAndProductService service = new NewAndProductService();
                NewAndProduct data = service.Table().Where(M => M.Id == id).FirstOrDefault();
                if (data == null)
                {
                    return RedirectToAction("Product", "Home");
                }
                else
                {
                    page_data = Newtonsoft.Json.JsonConvert.SerializeObject(data);
                }
            }
            catch
            {
            }
            ViewBag.Data = page_data;
            return View();
        }

        public ActionResult Show(int page= 1)
        {
            string action_name = RouteData.Values["action"].ToString().ToLower();

            ViewBag.Title = "产品展示";

            double page_size = 9;

            INewAndProductService service = new NewAndProductService();
            List<NewAndProduct> data = service.Table().Where(M => M.Type.IndexOf(action_name) != -1).ToList();
            ViewBag.AllCount = data.Count();

            double all_page = Math.Ceiling(data.Count() / page_size);
            ViewBag.AllPage = all_page;

            if (page < 1)
            {
                page = 1;
            }
            else if (page > all_page)
            {
                page = (int)all_page;
            }
            ViewBag.NowPage = page;

            List<NewAndProduct> p_data = data.Skip((int)page_size * (page - 1)).Take((int)page_size).OrderBy(M => M.AddTime).ToList();
            string page_data = data == null ? "" : Newtonsoft.Json.JsonConvert.SerializeObject(p_data);
            ViewBag.Data = page_data;

            return View();
        }

        public ActionResult ShowDetail(int id = 0)
        {
            string page_data = "";
            try
            {
                INewAndProductService service = new NewAndProductService();
                NewAndProduct data = service.Table().Where(M => M.Id == id).FirstOrDefault();
                if (data == null)
                {
                    return RedirectToAction("Show", "Home");
                }
                else
                {
                    page_data = Newtonsoft.Json.JsonConvert.SerializeObject(data);
                }
            }
            catch
            {
            }
            ViewBag.Data = page_data;
            return View();
        }


        public ActionResult Contact()
        {
            ViewBag.Title = "联系我们";
            string action_name = RouteData.Values["action"].ToString().ToLower();
            string page_data = "";
            try
            {
                ISettingService service = new SettingService();
                Setting data = service.Table().Where(M => M.Type.IndexOf(action_name) != -1).FirstOrDefault();
                page_data = data == null ? "" : Newtonsoft.Json.JsonConvert.SerializeObject(data);
            }
            catch
            {
            }
            ViewBag.Data = page_data;
            return View();
        }
    }
}