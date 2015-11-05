using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using FindJob.Model;
using HtmlAgilityPack;
using FindJob.Log;

namespace FindJob.DAl
{
    public class Qcwy : IZhaoPin
    {
        private List<JobInfo> _jobList = new List<JobInfo>();
        private Params _pars;
        public Qcwy(Params pars)
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
                var htmlWeb = new HtmlWeb { OverrideEncoding = Encoding.GetEncoding("gb2312") };

                HtmlDocument htmlDoc =
                    htmlWeb.Load(
                        string.Format(
                            "http://search.51job.com/jobsearch/search_result.php?jobarea={0}&keyword={1}&curr_page={2}",
                            DataClass.GetDic_qiancheng(_pars.Addr), _pars.Key, _pars.Page));

                var nodeList =
                    htmlDoc.DocumentNode.SelectNodes("//*[@id='resultList']/tr[@class='tr0']").AsParallel().ToList();
                for (int i = 0; i < nodeList.Count; i++)
                {
                    var node = nodeList[i];
                    var job = new JobInfo
                    {
                        TitleName = node.SelectSingleNode(".//a[@class='jobname']").InnerText,
                        InfoUrl = node.SelectSingleNode(".//a[@class='jobname']").Attributes["href"].Value,
                        Company = node.SelectSingleNode(".//a[@class='coname']").InnerText,
                        Salary = "详见原页面",
                        City = node.SelectSingleNode(".//td[@class='td3']/span").InnerText,
                        Date = node.SelectSingleNode(".//td[@class='td4']/span").InnerText.Substring(5),
                        Source = "前程无忧",
                        Method = "月薪"
                    };
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