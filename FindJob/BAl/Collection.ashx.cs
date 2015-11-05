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
            var page = context.Request["page"];
            var row = context.Request["rows"];
            if (cookie != null)
            {
                string guid = cookie.Value.ToUpper();

                string sql = "SELECT TOP (@row) * FROM ( SELECT row_number() OVER(ORDER BY DATE DESC) AS rownumber ,* FROM T_COLLECT where [USER]=@USER) A WHERE rownumber > @from";
                int p = int.Parse(page);
                int r = int.Parse(row);
                SqlParameter[] paras = { new SqlParameter("@row", r), new SqlParameter("@from", (p - 1) * r), new SqlParameter("@USER", guid) };
                //string sql = "select * from T_COLLECT where [User]=@USER";
                //where User=@USER
                Dictionary<string, Object> dir = new Dictionary<string, Object>();
                try
                {

                    int count = (int)SqlHelper.ExcuteScalar("select count(1) from T_COLLECT where [User]=@USER", new SqlParameter("@USER", guid));
                    DataTable data = SqlHelper.ExcuteDataTable(sql, paras);
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