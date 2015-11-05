using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using FindJob.Log;

namespace FindJob.BAl
{
    /// <summary>
    /// IsOutData 的摘要说明
    /// </summary>
    public class IsOutData : IHttpHandler
    {
        /// <summary>
        /// 判断Cookie是否过期
        /// </summary>
        /// <param name="context"></param>
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            //context.Response.Write("Hello World");
            string c = context.Request["cookie"];
            if (c != null)
            {
                LogSave.TrackLogSave("访问【Cookie】:" + c + "\r\n时间:" + DateTime.Now.ToString(CultureInfo.CurrentCulture))
                ;
                context.Response.Write("inData");
            }
            else
            {
                context.Response.Write("outData");
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