using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using FindJob.Model;
using System.Net.Http;
using System.Net.Http.Headers;

namespace FindJob.Class
{
    public class Lgw : IZhaoPin
    {
        private List<JobInfo> _jobList = new List<JobInfo>();
        private Params _pars;
        public Lgw(Params pars)
        {
            _pars = pars;
        }

        public List<JobInfo> GetJobList()
        {
            return _jobList;
        }

        public void GetJobListFromWeb(object o)
        {
            string url = "http://www.lagou.com/jobs/positionAjax.json?city=" + _pars.Addr;
            StringContent fromurlcontent = new StringContent("first=true&pn=" + _pars.Page + "&kd=" + _pars.Key);
            fromurlcontent.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
            fromurlcontent.Headers.Add("X-Requested-With", "XMLHttpRequest");
            HttpClient httpclient = new HttpClient();
            HttpResponseMessage responseMsg = httpclient.PostAsync(new Uri(url), fromurlcontent).Result;//发起post请求
            var result = responseMsg.Content.ReadAsStringAsync().Result;
            JavaScriptSerializer _jsSerializer = new JavaScriptSerializer();
            LagouInfo JPostData = _jsSerializer.Deserialize<LagouInfo>(result);
            foreach (var cresult in JPostData.Content.Result)
            {
                JobInfo info = new JobInfo();
                info.City = cresult.City;
                info.Company = cresult.CompanyName;
                info.Date = cresult.CreateTime.Substring(0, 10);
                info.InfoUrl = "http://www.lagou.com/jobs/" + cresult.PositionId + ".html";
                info.Salary = cresult.Salary;
                info.Source = "拉勾网";
                info.TitleName = cresult.PositionName;
                _jobList.Add(info);
            }
        }
    }
}