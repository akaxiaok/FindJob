using System;
using System.Text;
using System.Web;
using HtmlAgilityPack;

namespace FindJob.Handler
{
    /// <summary>
    /// Content 的摘要说明
    /// </summary>
    public class Content : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            //context.Response.Write("Hello World");
            string url = context.Request["URL"];
            context.Response.Write(GetContent(url));
        }

        public string GetContent(string url)
        {
            string result = string.Empty;
            if (url.Contains("zhaopin"))
            {
                GetFromZlzp(url, out result);
            }
            else if (url.Contains("51job"))
            {
                GetFromQcwy(url, out result);
            }
            else if (url.Contains("liepin"))
            {
                GetFromLPW(url, out result);
            }

            return result;

        }

        private void GetFromZlzp(string url, out string result)
        {
            var htmlWeb = new HtmlWeb { OverrideEncoding = Encoding.GetEncoding("UTF-8") };
            HtmlDocument htmlDoc = htmlWeb.Load(url);
            result = htmlDoc.DocumentNode.SelectSingleNode("//*[@class='tab-inner-cont']").InnerHtml;
            int index = result.IndexOf("<b>", StringComparison.Ordinal);
            result = result.Substring(0, index);
        }

        private void GetFromQcwy(string url, out string result)
        {
            var htmlWeb = new HtmlWeb { OverrideEncoding = Encoding.GetEncoding("gb2312") };
            HtmlDocument htmlDoc = htmlWeb.Load(url);
            result = htmlDoc.DocumentNode.SelectSingleNode("//*[@class='txt_4 wordBreakNormal job_detail ']").InnerHtml;
        }

        private void GetFromLPW(string url, out string result)
        {
            var htmlWeb = new HtmlWeb { OverrideEncoding = Encoding.GetEncoding("UTF-8") };
            HtmlDocument htmlDoc = htmlWeb.Load(url);
            result = htmlDoc.DocumentNode.SelectSingleNode("//div[@class='content content-word']").InnerHtml;
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