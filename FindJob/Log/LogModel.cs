using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace FindJob.Log
{
    public class LogModel
    {
        #region logFileName
        private string _logFileName;
        public string LogFileName
        {
            get { return _logFileName + "_" + DateTime.Now.ToString("yyMMdd"); }
            set { _logFileName = value; }
        }

        #endregion


        #region logMessage
        private string _logMessage;
        public string LogMessage
        {
            get
            {
                return "----begin----" + DateTime.Now.ToString(CultureInfo.CurrentCulture) + "----Queue.Count: " + LogHelper.LogQuene.Count +
                       "-----------------------------------\r\n\r\n" + _logMessage + "\r\n\r\n----end--------" + DateTime.Now.ToString(CultureInfo.CurrentCulture) + "----Quene.Count: " + LogHelper.LogQuene.Count + "-----------------------------------\r\n\r\n\r\n";
            }
            set { _logMessage = value; }
        }
        #endregion


    }
}