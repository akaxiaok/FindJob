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
            int workerThreads = 0;
            int completionPortThreads = 0;
            ThreadPool.GetAvailableThreads(out workerThreads, out completionPortThreads);
            int nowworkerThreads = 0;
            if (pars.From.Contains("zlzp"))
            {
                var zlzp = new Zlzp(pars);
                //var zlzpThread = new Thread(zlzp.GetJobListFromWeb);
                //zlzpThread.Start();
                //threads.Add(zlzpThread);
                zhaoPing.Add(zlzp);
                ThreadPool.QueueUserWorkItem(zlzp.GetJobListFromWeb);
            }
            if (pars.From.Contains("qcwy"))
            {
                var qcwy = new Qcwy(pars);
                //var qcwyThread = new Thread(qcwy.GetJobListFromWeb);
                //qcwyThread.Start();
                //threads.Add(qcwyThread);
                zhaoPing.Add(qcwy);
                ThreadPool.QueueUserWorkItem(qcwy.GetJobListFromWeb);
                // 
            }
            if (pars.From.Contains("lpw"))
            {
                var lpw = new Lpw(pars);
                //var lpwThread = new Thread(lpw.GetJobListFromWeb);
                //lpwThread.Start();
                //threads.Add(lpwThread);
                zhaoPing.Add(lpw);
                ThreadPool.QueueUserWorkItem(lpw.GetJobListFromWeb);
            }
            if (pars.From.Contains("lgw"))
            {
                var lgw = new Lgw(pars);
                //var lpwThread = new Thread(lpw.GetJobListFromWeb);
                //lpwThread.Start();
                //threads.Add(lpwThread);
                zhaoPing.Add(lgw);
                ThreadPool.QueueUserWorkItem(lgw.GetJobListFromWeb);
            }
            // foreach (var v in threads)
            // {
            //     v.Join();
            // }
            // 

            while (workerThreads != nowworkerThreads)
            {
                ThreadPool.GetAvailableThreads(out nowworkerThreads, out completionPortThreads);
                Thread.Sleep(1000);
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





    }
}