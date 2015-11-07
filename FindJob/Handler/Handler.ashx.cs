using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using FindJob.Class;
using FindJob.Model;
using HtmlAgilityPack;
using Newtonsoft.Json;

namespace FindJob.Handler
{
    /// <summary>
    /// Handler 的摘要说明
    /// </summary>
    public class Handler : IHttpHandler
    {

        public delegate List<JobInfo> TasksDelegate(Params pars);


        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            //context.Response.Write("Hello World");
            //JsonConvert.DeserializeObject(context.Request.Params.ToString());
            Params pars = new Params();
            pars.Key = context.Request.Params["key"];
            pars.Addr = context.Request.Params["addr"];
            pars.From = context.Request.Params["from"];
            pars.Page = context.Request.Params["page"];
            List<JobInfo> list = GetList(pars);
            context.Response.Write(JsonConvert.SerializeObject(list));
        }

        private List<JobInfo> GetList(Params pars)
        {
            List<JobInfo> jobList = new List<JobInfo>();
            List<IZhaoPin> zhaoPing = new List<IZhaoPin>();
            var threads = new List<Thread>();
            if (pars.From.Contains("zlzp"))
            {
                var zlzp = new Zlzp(pars);
                var zlzpThread = new Thread(zlzp.GetJobListFromWeb);
                zlzpThread.Start();
                threads.Add(zlzpThread);
                zhaoPing.Add(zlzp);
            }
            if (pars.From.Contains("qcwy"))
            {
                var qcwy = new Qcwy(pars);
                var qcwyThread = new Thread(qcwy.GetJobListFromWeb);
                qcwyThread.Start();
                threads.Add(qcwyThread);
                zhaoPing.Add(qcwy);
            }
            if (pars.From.Contains("lpw"))
            {
                var lpw = new Lpw(pars);
                var lpwThread = new Thread(lpw.GetJobListFromWeb);
                lpwThread.Start();
                threads.Add(lpwThread);
                zhaoPing.Add(lpw);
            }
            foreach (var v in threads)
            {
                v.Join();
            }
           foreach (var v in zhaoPing)
           {
               jobList.AddRange(v.GetJobList());
           }
          

            return jobList;

        }
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }



        #region Abandoned
        private static List<JobInfo> GetFromZlzp(Params pars)
        {
            List<JobInfo> jobList = new List<JobInfo>();
            var htmlWeb = new HtmlWeb { OverrideEncoding = Encoding.GetEncoding("UTF-8") };

            HtmlDocument htmlDoc = htmlWeb.Load(string.Format("http://sou.zhaopin.com/jobs/searchresult.ashx?jl={0}&kw={1}&p={2}", DataClass.GetDic_zhilian(pars.Addr), pars.Key, pars.Page));
            var nodeList = htmlDoc.DocumentNode.SelectNodes("//*[@id='newlist_list_content_table']/table[@class='newlist']").AsParallel().ToList();
            for (int i = 1; i < nodeList.Count; i++)
            {
                var node = nodeList[i];
                var job = new JobInfo();
                job.TitleName = node.SelectSingleNode(".//tr/td[@class='zwmc']/div/a").InnerText;
                job.InfoUrl = node.SelectSingleNode(".//tr/td[@class='zwmc']/div/a").Attributes["href"].Value;
                job.Company = node.SelectSingleNode(".//tr/td[@class='gsmc']/a").InnerText;
                job.Salary = node.SelectSingleNode(".//tr/td[@class='zwyx']").InnerText;
                job.City = node.SelectSingleNode(".//tr/td[@class='gzdd']").InnerText;
                job.Date = node.SelectSingleNode(".//tr/td[@class='gxsj']/span").InnerText;
                job.Source = "智联招聘";
                job.Method = "月薪";
                jobList.Add(job);
            }
            return jobList;
        }


        private static List<JobInfo> GetFromQcwy(Params pars)
        {
            List<JobInfo> jobList = new List<JobInfo>();
            var htmlWeb = new HtmlWeb { OverrideEncoding = Encoding.GetEncoding("gb2312") };

            HtmlDocument htmlDoc = htmlWeb.Load(string.Format("http://search.51job.com/jobsearch/search_result.php?jobarea={0}&keyword={1}&curr_page={2}", DataClass.GetDic_qiancheng(pars.Addr), pars.Key, pars.Page));

            var nodeList = htmlDoc.DocumentNode.SelectNodes("//*[@id='resultList']/tr[@class='tr0']").AsParallel().ToList();
            for (int i = 1; i < nodeList.Count; i++)
            {
                var node = nodeList[i];
                var job = new JobInfo
                {
                    TitleName = node.SelectSingleNode(".//a[@class='jobname']").InnerText,
                    InfoUrl = node.SelectSingleNode(".//a[@class='jobname']").Attributes["href"].Value,
                    Company = node.SelectSingleNode(".//a[@class='coname']").InnerText,
                    Salary = "详见原页面",
                    City = node.SelectSingleNode(".//td[@class='td3']/span").InnerText,
                    Date = node.SelectSingleNode(".//td[@class='td4']/span").InnerText,
                    Source = "前程无忧",
                    Method = "月薪"
                };
                jobList.Add(job);
            }
            return jobList;
        }
        private string GetSalary(string url)
        {
            var htmlWeb = new HtmlWeb { OverrideEncoding = Encoding.GetEncoding("gb2312") };
            HtmlDocument htmlDoc = htmlWeb.Load(url);
            try
            {
                return htmlDoc.DocumentNode.SelectSingleNode("//td[@class='txt_2 jobdetail_xsfw_color ']").InnerText;
            }
            catch (System.Exception ex)
            {
                return "";
            }
        }
        #endregion
        


    }
}