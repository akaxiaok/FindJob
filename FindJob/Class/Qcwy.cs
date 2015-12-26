using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;
using FindJob.Log;
using FindJob.Model;
using HtmlAgilityPack;
using HtmlDocument = HtmlAgilityPack.HtmlDocument;

namespace FindJob.Class
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
        public class MyWebClient
        {
            //The cookies will be here.
            private CookieContainer _cookies = new CookieContainer();

            //In case you need to clear the cookies
            public void ClearCookies()
            {
                _cookies = new CookieContainer();
            }

            public HtmlDocument GetPage(string url)
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";

                //Set more parameters here...
                //...

                //This is the important part.
                request.CookieContainer = _cookies;

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                var stream = response.GetResponseStream();

                //When you get the response from the website, the cookies will be stored
                //automatically in "_cookies".
                if (stream != null)
                {
                    using (var reader = new StreamReader(stream, Encoding.GetEncoding("gb2312")))
                    {
                        string html = reader.ReadToEnd();
                        var doc = new HtmlDocument();
                        doc.LoadHtml(html);
                        return doc;
                    }
                }
                else
                {
                    return null;
                }

            }
        }

        public static string Gb2312ToUtf8(string text)
        {
            //声明字符集   
            Encoding utf8, gb2312;
            //gb2312   
            gb2312 = Encoding.GetEncoding("gb2312");
            //utf8   
            utf8 = Encoding.GetEncoding("utf-8");
            byte[] gb;
            gb = gb2312.GetBytes(text);
            gb = Encoding.Convert(gb2312, utf8, gb);
            //返回转换后的字符   
            return utf8.GetString(gb);
        }





        public void GetJobListFromWeb(object o)
        {
            try
            {
                var htmlWeb = new HtmlWeb { OverrideEncoding = Encoding.GetEncoding("gb2312") };

                var client = new MyWebClient();
                HtmlDocument htmlDoc = client.GetPage(string.Format("http://search.51job.com/list/010000%252C00,000000,0000,00,9,99,java,0,1.html?lang=c&stype=2&postchannel=0000&workyear=99&cotype=99&degreefrom=99&jobterm=99&companysize=99&lonlat=0%2C0&radius=-1&ord_field=0&list_type=0&confirmdate=9&dibiaoid=0"));

                htmlDoc = client.GetPage(
                                string.Format(
                            "http://search.51job.com/jobsearch/search_result.php?jobarea={0}&keyword={1}&curr_page={2}",
                            DataClass.GetDic_qiancheng(_pars.Addr), _pars.Key, _pars.Page));
                //HtmlDocument htmlDoc =
                //    htmlWeb.Load(
                //        string.Format(
                //            "http://search.51job.com/jobsearch/search_result.php?jobarea={0}&keyword={1}&curr_page={2}",
                //            DataClass.GetDic_qiancheng(_pars.Addr), _pars.Key, _pars.Page));

                //var url = htmlDoc.DocumentNode.SelectSingleNode("//*[@id='dw_close']").Attributes["href"].Value;
                //  htmlDoc =
                //    htmlWeb.Load(
                //        url);
                var nodeList =//*[@id="resultList"]
                    htmlDoc.DocumentNode.SelectNodes("//*[@id='resultList']/div[@class='el']").AsParallel().ToList();
                for (int i = 0; i < nodeList.Count; i++)
                {
                    var node = nodeList[i];
                    var job = new JobInfo
                    {
                        TitleName = Gb2312ToUtf8(node.SelectSingleNode(".//p/a").InnerText),
                        InfoUrl = Gb2312ToUtf8(node.SelectSingleNode(".//p/a").Attributes["href"].Value),
                        //Company = Gb2312ToUtf8(node.SelectSingleNode(".//span[@class =  't2']").InnerText),
                        //Salary = Gb2312ToUtf8(node.SelectSingleNode(".//span[@class =  't4']").InnerText),
                        //City = Gb2312ToUtf8(node.SelectSingleNode(".//span[@class =  't3']").InnerText),
                        //Date = Gb2312ToUtf8(node.SelectSingleNode(".//span[@class =  't5']").InnerText),
                        Company = node.SelectSingleNode(".//span[@class =  't2']").InnerText,
                        Salary = node.SelectSingleNode(".//span[@class =  't4']").InnerText,
                        City = node.SelectSingleNode(".//span[@class =  't3']").InnerText,
                        Date = node.SelectSingleNode(".//span[@class =  't5']").InnerText,
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