using System;
using System.Data.SqlClient;
using System.Web;
using FindJob.Class;
using FindJob.Log;
using FindJob.Model;
using Newtonsoft.Json;

namespace FindJob.Handler
{
    /// <summary>
    /// Collect 的摘要说明
    /// </summary>
    public class Collect : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string jsonStr = context.Request["data"];
            JobInfo job = JsonConvert.DeserializeObject<JobInfo>(jsonStr);
            var httpCookie = context.Request.Cookies["Id"];
            if (httpCookie != null)
            {
                string user = httpCookie.Value;
                string sql =
                    "select count(*) from T_COLLECT where TitleName=@TitleName and Company=@Company and City=@City ";
                try
                {
                    SqlParameter[] paras = new SqlParameter[]
                    {
                        new SqlParameter("@TitleName", job.TitleName),
                        new SqlParameter("@Company", job.Company),
                        new SqlParameter("@City", job.City)
                    };
                    bool exist = (int)SqlHelper.ExcuteScalar(sql, paras) == 1 ? true : false;
                    if (exist)
                    {
                        context.Response.Write("exist");
                        return;
                    }

                }
                catch (Exception ex)
                {
                    LogSave.ErrLogSave("错误【收藏】：", ex);
                    context.Response.Write("false");
                    return;
                }

                sql = "insert into T_COLLECT values(@GUID,@TitleName,@Company,@City,@Date,@Salary,@Method,@InfoUrl,@Source,@User,0)";
                try
                {
                    SqlParameter[] paras = new SqlParameter[]
                    {
                        new SqlParameter("GUID", Guid.NewGuid()),
                        new SqlParameter("@TitleName", job.TitleName),
                        new SqlParameter("@Company", job.Company),
                        new SqlParameter("@City", job.City),
                        new SqlParameter("@Date", job.Date),
                        new SqlParameter("@Salary", job.Salary),
                        new SqlParameter("@Method", job.Method),
                        new SqlParameter("@InfoUrl", job.InfoUrl),
                        new SqlParameter("@Source", job.Source),
                        new SqlParameter("@User", user)


                    };

                    int result = SqlHelper.ExcuteNonQuery(sql, paras);
                    if (result != 1)
                    {
                        context.Response.WriteFile("false");
                        return;
                    }

                }
                catch (System.Exception ex)
                {
                    LogSave.ErrLogSave("错误【收藏】：", ex);
                    context.Response.Write("false");
                    return;
                }
                context.Response.Write("true");
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