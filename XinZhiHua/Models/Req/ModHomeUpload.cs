using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XinZhiHua.Models.Req
{
    public class ModHomeUpload
    {
        public HttpPostedFileBase Img1 { get; set; }

        public HttpPostedFileBase Img2 { get; set; }

        public HttpPostedFileBase Img3 { get; set; }

    }
}