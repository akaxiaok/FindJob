using System.Collections.Generic;
using System.Linq;

namespace FindJob.Model
{
    public class DataClass
    {
        #region dic_zhilian

        private static Dictionary<string, string> DicZhilian = new Dictionary<string, string>();
        public static string GetDic_zhilian(string key)
        {
            if (DicZhilian.Count <= 0)
            {
                DicZhilian.Add("北京", "北京");
                DicZhilian.Add("上海", "上海");
                DicZhilian.Add("广州", "广州");
                DicZhilian.Add("深圳", "深圳");
                DicZhilian.Add("天津", "天津");
                DicZhilian.Add("苏州", "苏州");
                DicZhilian.Add("重庆", "重庆");
                DicZhilian.Add("南京", "南京");
                DicZhilian.Add("杭州", "杭州");
                DicZhilian.Add("大连", "大连");
                DicZhilian.Add("成都", "成都");
                DicZhilian.Add("武汉", "武汉");
                DicZhilian.Add("长沙", "长沙");
                DicZhilian.Add("沈阳", "沈阳");
            }
            if (DicZhilian.Keys.Contains(key))
                return DicZhilian[key];
            return string.Empty;
        }
        #endregion

        #region dic_qiancheng

        private static Dictionary<string, string> DicQiancheng = new Dictionary<string, string>();
        public static string GetDic_qiancheng(string key)
        {
            if (DicQiancheng.Count <= 0)
            {
                DicQiancheng.Add("北京", "010000");
                DicQiancheng.Add("上海", "020000");
                DicQiancheng.Add("广州", "030200");
                DicQiancheng.Add("深圳", "040000");
                DicQiancheng.Add("天津", "050000");
                DicQiancheng.Add("苏州", "070300");
                DicQiancheng.Add("重庆", "060000");
                DicQiancheng.Add("南京", "070200");
                DicQiancheng.Add("杭州", "080200");
                DicQiancheng.Add("大连", "230300");
                DicQiancheng.Add("成都", "090200");
                DicQiancheng.Add("武汉", "180200");
                DicQiancheng.Add("长沙", "190200");
                DicQiancheng.Add("沈阳", "230200");
            }
            if (DicQiancheng.Keys.Contains(key))
                return DicQiancheng[key];
            return string.Empty;
        }
        #endregion

        #region dic_liepin

        private static Dictionary<string, string> DicLiepin = new Dictionary<string, string>();
        public static string GetDic_liepin(string key)
        {
            if (DicLiepin.Count <= 0)
            {
                DicLiepin.Add("北京", "010");
                DicLiepin.Add("上海", "020");
                DicLiepin.Add("广州", "050020");
                DicLiepin.Add("深圳", "050090");
                DicLiepin.Add("天津", "030");
                DicLiepin.Add("苏州", "060080");
                DicLiepin.Add("重庆", "040");
                DicLiepin.Add("南京", "060020");
                DicLiepin.Add("杭州", "070020");
                DicLiepin.Add("大连", "210040");
                DicLiepin.Add("成都", "280020");
                DicLiepin.Add("武汉", "170020");
                DicLiepin.Add("长沙", "180020");
                DicLiepin.Add("沈阳", "210020");
            }
            if (DicLiepin.Keys.Contains(key))
                return DicLiepin[key];
            return string.Empty;
        }
        #endregion
    }
}
