using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Web;
using System.Web.SessionState;
using FindJob.Class;
using FindJob.Log;
using FindJob.Model;

namespace FindJob.Handler
{
    /// <summary>
    /// Login 的摘要说明
    /// </summary>
    public class Login : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            User user = new User();

            user.Name = context.Request["name"];
            user.Pwd = context.Request["pwd"];
            string sql = "select * from T_USERS where UserName=@UserName and Password=@Password";
            SqlParameter[] paras =
            {
                new SqlParameter("@UserName", user.Name),
                new SqlParameter("@Password", user.Pwd)
            };
            DataTable dataTable = new DataTable();
            bool result = false;
            try
            {
                dataTable = SqlHelper.ExcuteDataTable(sql, paras);
                result = dataTable.Rows.Count == 1 ? true : false;
                if (result)
                {

                    //if(remember)
                    //Response.Cookies("MyCookie").Expires = DateAdd("h", 1, Now())
                    string guid = dataTable.Rows[0]["GUID"].ToString();
                    HttpCookie cookie = new HttpCookie("Id", guid);
                    cookie.Expires = DateTime.Now.AddHours(1);
                    context.Response.Cookies.Add(cookie);
                    LogSave.TrackLogSave("登陆：" + guid + "\r\n时间：" + DateTime.Now.ToString(CultureInfo.CurrentCulture));
                    //context.Session.Add("Id", guid);
                }

            }
            catch (System.Exception ex)
            {
                LogSave.ErrLogSave("错误【登陆】：", ex);
            }
            finally
            {
                context.Response.Write(result);
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