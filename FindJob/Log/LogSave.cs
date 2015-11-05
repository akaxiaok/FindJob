using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

namespace FindJob.Log
{
    public class LogSave
    {
        /// <summary>
        /// 获取异常信息
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static string GetExceptionInfo(Exception ex)
        {
            StringBuilder str = new StringBuilder();
            str.Append("错误信息: " + ex.Message);
            str.Append("\r\n错误源: " + ex.Source);
            str.Append("\r\n异常方法: " + ex.TargetSite);
            str.Append("\r\n堆栈信息: " + ex.StackTrace);
            return str.ToString();
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="fileName"></param>
        public static void SysErrLogSave(Exception ex, string fileName = null)
        {
            StringBuilder str = new StringBuilder();
            string ip = "";
            if (HttpContext.Current.Request.ServerVariables.Get("HTTP_X_FORWARDED_FOR") != null)
                ip = HttpContext.Current.Request.ServerVariables.Get("HTTP_X_FORWARDED_FOR").ToString().Trim();
            else
                ip = HttpContext.Current.Request.ServerVariables.Get("Remote_Addr").ToString().Trim();
            str.Append("IP: " + ip);
            str.Append("\r\n浏览器: " + HttpContext.Current.Request.Browser.Browser);
            str.Append("\r\n浏览器版本: " + HttpContext.Current.Request.Browser.MajorVersion.ToString());
            str.Append("\r\n操作系统: " + HttpContext.Current.Request.Browser.Platform);
            str.Append("\r\n页面: " + HttpContext.Current.Request.Url.ToString());
            str.Append("\r\n" + GetExceptionInfo(ex));
            LogHelper.LogWrite(new LogModel()
            {
                LogFileName = "SysErr" + fileName ?? string.Empty,
                LogMessage = str.ToString()
            });

        }

        /// <summary>
        /// 异常日志记录
        /// </summary>
        /// <param name="strmes"></param>
        /// <param name="ex"></param>
        /// <param name="fileName"></param>
        public static void ErrLogSave(string strmes, Exception ex, string fileName = null)
        {
            StringBuilder str = new StringBuilder();
            str.Append(strmes);
            if (ex != null)
                str.Append("\r\n" + GetExceptionInfo(ex));
            LogHelper.LogWrite(new LogModel()
            {
                LogFileName = fileName ?? "Err",
                LogMessage = str.ToString()
            });
        }
        /// <summary>
        /// 警告日志记录
        /// </summary>
        /// <param name="str"></param>
        /// <param name="fileName"></param>
        public static void WarnLogSave(string str, string fileName = null)
        {
            if (str != null && !string.IsNullOrEmpty(str.Trim()))
                LogHelper.LogWrite(new LogModel()
                {
                    LogFileName = fileName ?? "Warn",
                    LogMessage = str
                });
        }

        public static void TrackLogSave(string str, string fileName = null)
        {
            if (str != null && !string.IsNullOrEmpty(str.Trim()))
                LogHelper.LogWrite(new LogModel()
                {
                    LogFileName = fileName ?? "Track",
                    LogMessage = str
                });
        }
        public static void TrackLogSave(string str)
        {
            if (!string.IsNullOrEmpty(str.Trim()))
                LogHelper.LogWrite(new LogModel()
                {
                    LogFileName = "SqlTrack",
                    LogMessage = str
                });
        }
    }
}