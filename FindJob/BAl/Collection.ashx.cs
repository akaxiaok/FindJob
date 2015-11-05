using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FindJob.Log;
using System.Data;
using FindJob.DAl;
using System.Data.SqlClient;
using Newtonsoft.Json;

namespace FindJob.BAl
{
    /// <summary>
    /// Collection 的摘要说明
    /// </summary>
    public class Collection : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            var cookie = context.Request.Cookies["Id"];
            if (cookie != null)
            {
                string guid = cookie.Value.ToUpper();
                string sql = "select * from T_COLLECT where [User]=@USER";
                //where User=@USER
                Dictionary<string, Object> dir = new Dictionary<string, Object>();
                try
                {

                    int count = (int)SqlHelper.ExcuteScalar("select count(1) from T_COLLECT where [User]=@USER", new SqlParameter("@USER", guid));
                    DataTable data = SqlHelper.ExcuteDataTable(sql, new SqlParameter("@USER", guid));
                    dir.Add("total", count);
                    dir.Add("rows", data);
                    string result = JsonConvert.SerializeObject(dir);
                    context.Response.Write(result);

                }
                catch (System.Exception ex)
                {
                    LogSave.ErrLogSave("错误【收藏夹】", ex);
                }
            }
            else
            {
                context.Response.Write("login");
            }
            //context.Response.Write("Hello World");
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