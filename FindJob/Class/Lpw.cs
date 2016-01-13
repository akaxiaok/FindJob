using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FindJob.Log;
using FindJob.Model;
using HtmlAgilityPack;

namespace FindJob.Class
{
    public class Lpw : IZhaoPin
    {
        private List<JobInfo> _jobList = new List<JobInfo>();
        private Params _pars;
        public Lpw(Params pars)
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
                    htmlWeb.Load(string.Format("http://www.liepin.com/zhaopin/?key={0}&dqs={1}&curPage={2}", _pars.Key,
                        DataClass.GetDic_liepin(_pars.Addr), _pars.Page));

                var nodeList =
                    htmlDoc.DocumentNode.SelectNodes("//ul[@class='sojob-list']/li ").AsParallel().ToList();
                for (int i = 0; i < nodeList.Count; i++)
                {
                    var node = nodeList[i];
                    var job = new JobInfo();
                    job.TitleName = node.SelectSingleNode(".//div/div[@class='job-info']/h3").Attributes["title"].Value.Substring(2);
                    job.InfoUrl = node.SelectSingleNode(".//div/div[@class='job-info']/h3/a").Attributes["href"].Value;
                    job.Company = node.SelectSingleNode(".//div/div[@class='company-info']/p/a").Attributes["title"].Value;
                    job.Salary = node.SelectSingleNode(".//div/div[@class='job-info']/p[@class='condition clearfix']/span[@class='text-warning']").InnerText;
                    job.City = node.SelectSingleNode(".//div/div[@class='job-info']/p[@class='condition clearfix']/*[@class='area']").InnerText;
                    job.Date = node.SelectSingleNode(".//div/div[@class='job-info']/p[@class='time-info clearfix']/time").InnerText.Substring(4);
                    job.Source = "猎聘网";
                    job.Method = "年薪";
                    _jobList.Add(job);
                }
            }
            catch (Exception ex)
            {
                LogSave.ErrLogSave("错误【解析】：", ex);
            }
        }
    }

}