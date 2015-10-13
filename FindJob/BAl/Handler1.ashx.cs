using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace FindJob.BAl
{
    /// <summary>
    /// Handler1 的摘要说明
    /// </summary>
    public class Handler1 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        { 
            context.Response.ContentType = "text/plain";
            string file = File.ReadAllText(@"D:\visual studio 2010\Projects\FindJob\FindJob\Scripts\jquery-easyui-1.4.2\jquery-easyui-datagridview\datagrid_data.json");
            context.Response.Write(file);
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