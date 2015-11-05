using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using FindJob.Log;

namespace FindJob.BAl
{
    /// <summary>
    /// Logout 的摘要说明
    /// </summary>
    public class Logout : IHttpHandler
    {
        /// <summary>
        /// 退出登陆，设置Cookie过期
        /// </summary>
        /// <param name="context"></param>
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            //context.Response.Write("Hello World");
            var httpCookie = context.Response.Cookies["Id"];
            var id = context.Request.Cookies["Id"];
            if (id != null)
            {
                if (httpCookie != null)
                {

                    httpCookie.Expires = DateTime.Now;
                    context.Response.Write("Success");
                    LogSave.TrackLogSave("登出:" + id.Value + "\r\n时间:" + DateTime.Now.ToString(CultureInfo.CurrentCulture));
                }

            }
        }
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}