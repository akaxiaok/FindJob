using System.Collections.Generic;

namespace FindJob.Class
{
    public class LagouInfo
    {
        public string Code;
        public Ccontent Content;
        public string Msg;
        public string RequestId;
        public string ResubmitToken;
        public string Success;
    }

    public class Ccontent
    {
        public string CurrentPageNo;
        public string HasNextPage;
        public string HasPreviousPage;
        public string PageNo;
        public string PageSize;
        public List<Cresult> Result;
        public string Start;
        public string TotalCount;
        public string TotalPageCount;
    }

    public class Cresult
    {
        public string AdWord;
        public string AdjustScore;
        public string City;
        public string CompanyId;
        public List<string> CompanyLabelList;
        public string CompanyLogo;
        public string CompanyName;
        public string CompanyShortName;
        public string CompanySize;
        public string CountAdjusted;
        public string CreateTime;
        public string CreateTimeSort;
        public string Education;
        public string FinanceStage;
        public string FormatCreateTime;
        public string HaveDeliver;
        public string IndustryField;
        public string JobNature;
        public string LeaderName;
        public string OrderBy;
        public string PositionAdvantage;
        public string PositionFirstType;
        public string PositionId;
        public string PositionName;
        public string PositionType;
        public string PositonTypesMap;
        public string RandomScore;
        public string RelScore;
        public string Salary;
        public string Score;
        public string SearchScore;
        public string ShowOrder;
        public string TotalCount;
        public string WorkYear;
    }
}