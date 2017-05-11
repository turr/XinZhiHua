using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace XinZhiHua.Controllers
{
    public class BaseController : Controller 
    {
        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);

            string seo = "";
            try
            {
                ISettingService service = new SettingService();
                Setting data = service.Table().Where(M => M.Type == "index4").FirstOrDefault();
                if(data != null)
                {
                    seo = string.IsNullOrEmpty(data.Content) ? "" : data.Content;
                }
            }
            catch
            {
            }
            ViewBag.SEO = seo;

        }
    }
}