using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FindJob.Log;
using FindJob.Model;
using HtmlAgilityPack;

namespace FindJob.Class
{
    public class Zlzp : IZhaoPin
    {
        private List<JobInfo> _jobList = new List<JobInfo>();
        private Params _pars;

        public Zlzp(Params pars)
        {
            _pars = pars;
        }
        public List<JobInfo> GetJobList()
        {
            return _jobList;
        }

        public void GetJobListFromWeb( )
        {
            try
            {
                var htmlWeb = new HtmlWeb { OverrideEncoding = Encoding.GetEncoding("UTF-8") };
                HtmlDocument htmlDoc =
                    htmlWeb.Load(string.Format("http://sou.zhaopin.com/jobs/searchresult.ashx?jl={0}&kw={1}&p={2}",
                        DataClass.GetDic_zhilian(_pars.Addr), _pars.Key, _pars.Page));
                var nodeList =
                    htmlDoc.DocumentNode.SelectNodes("//*[@id='newlist_list_content_table']/table[@class='newlist']")
                        .AsParallel()
                        .ToList();
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
                    _jobList.Add(job);
                }
            }
            catch (Exception ex)
            {
                LogSave.ErrLogSave("错误【解析】", ex);
            }
        }


    }
}