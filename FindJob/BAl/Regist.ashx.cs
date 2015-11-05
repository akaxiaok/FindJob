using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using FindJob.DAl;
using FindJob.Model;
using FindJob.Log;

namespace FindJob.BAl
{
    /// <summary>
    /// Regist 的摘要说明
    /// </summary>
    public class Regist : IHttpHandler, IRequiresSessionState
    {
        /// <summary>
        /// 数据库查询结果
        /// </summary>
        enum States
        {
            Success, EmailError, UserNameError, UnknownError
        }
        /// <summary>
        /// 处理注册 
        /// </summary>
        /// <param name="context"></param>
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            User user = new User();
            user.Name = context.Request["name"];
            user.Pwd = context.Request["pwd"];
            if (!user.Pwd.Equals(context.Request["rePwd"]))
            {
                context.Response.Write("SamePwd");
                return;
            }
            user.Email = context.Request["email"];
            user.Guid = Guid.NewGuid().ToString();
            string sql = "select count(*) from T_USERS where UserName=@UserName";


            try
            {
                var result = States.UnknownError;
                result = (int)SqlHelper.ExcuteScalar(sql, new SqlParameter("@UserName", user.Name)) == 0
                    ? States.Success
                    : States.UserNameError;

                if (result.Equals(States.UserNameError))
                {
                    context.Response.Write("NameExist");
                    return;
                }

                sql = "select count(*) from T_USERS where Email=@Email";

                result = (int)SqlHelper.ExcuteScalar(sql, new SqlParameter("@Email", user.Email)) == 0
                    ? States.Success
                    : States.EmailError;

                if (result.Equals(States.EmailError))
                {
                    context.Response.Write("EmailExist");
                    return;
                }
                SqlParameter[] pars =
                {
                    new SqlParameter("@GUID",user.Guid ),
                    new SqlParameter("@UserName", user.Name),
                    new SqlParameter("@Password", user.Pwd),
                    new SqlParameter("@Email", user.Email)
                };
                sql = "insert into T_USERS values(@GUID,@UserName,@Password,@Email)";
                result = SqlHelper.ExcuteNonQuery(sql, pars) == 1 ? States.Success : States.UnknownError;
                HttpCookie cookie = new HttpCookie("Id", user.Guid);
                cookie.Expires = DateTime.Now.AddHours(1);
                context.Response.Cookies.Add(cookie);
                //context.Session.Add("Id", user.Guid);
                context.Response.Write("Success");
            }
            catch (Exception ex)
            {
                LogSave.ErrLogSave("错误【注册】：", ex);
                context.Response.Write("Error");
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