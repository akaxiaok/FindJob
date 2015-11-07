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
        public void GetJobListFromWeb()
        {
            try
            {
                var htmlWeb = new HtmlWeb { OverrideEncoding = Encoding.GetEncoding("UTF-8") };

                HtmlDocument htmlDoc =
                    htmlWeb.Load(string.Format("http://www.liepin.com/zhaopin/?key={0}&dqs={1}&curPage={2}", _pars.Key,
                        DataClass.GetDic_liepin(_pars.Addr), _pars.Page));

                var nodeList =
                    htmlDoc.DocumentNode.SelectNodes("//ul[@class='sojob-result-list']/li ").AsParallel().ToList();
                for (int i = 0; i < nodeList.Count; i++)
                {
                    var node = nodeList[i];
                    var job = new JobInfo();
                    job.TitleName = node.SelectSingleNode(".//a").Attributes["title"].Value.Substring(2);
                    job.InfoUrl = node.SelectSingleNode(".//a").Attributes["href"].Value;
                    job.Company = node.SelectSingleNode(".//a/dl/dt[@class='company']").InnerText;
                    job.Salary = node.SelectSingleNode(".//a/dl/dt[@class='salary']/em").InnerText;
                    job.City = node.SelectSingleNode(".//a/dl/dt[@class='city']/span").InnerText;
                    job.Date = node.SelectSingleNode(".//a/dl/dt[@class='date']/span").InnerText.Substring(5);
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