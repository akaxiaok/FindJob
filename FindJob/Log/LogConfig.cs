using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Data;

namespace FindJob.Log
{
    public static class LogConfig
    {
        #region 辅助方法
        public static string GetAppSettings(string key)
        {
            if (ConfigurationManager.AppSettings.AllKeys.Contains(key))
            {
                return ConfigurationManager.AppSettings[key];
            }
            return string.Empty;
        }

        public static string ToCompute(this string v)
        {
            return new DataTable().Compute(v, "").ToString();
        }
        #endregion
        #region 静态属性和字段
        #region logFilePath 路径

        private static string _logFilePath;

        public static string LogFilePath
        {
            get
            {
                if (string.IsNullOrEmpty(_logFilePath))
                {
                    try
                    {
                        _logFilePath = HttpContext.Current.Server.MapPath("~/");
                    }
                    catch (System.Exception ex)
                    {
                        try
                        {
                            _logFilePath = System.Windows.Forms.Application.StartupPath + @"\";
                        }
                        catch (Exception)
                        {
                            throw new Exception("请先初始化要保存的路径:LogModel._logFilePath");

                        }
                    }
                }
                return _logFilePath;
            }
            set { _logFilePath = value; }
        }

        #endregion

        #region 检测间隔时间 分钟

        private static int _testingInterval;

        public static int TestingInterval
        {
            get
            {
                if (_testingInterval <= 0)
                {
                    var logTestingInterval = GetAppSettings("Log_TestingInterval");
                    if (string.IsNullOrEmpty(logTestingInterval))
                        _testingInterval = 1 * 60 * 24;
                    else
                        _testingInterval = Convert.ToInt32(logTestingInterval.ToCompute());
                }
                return _testingInterval;
            }
        }
        #endregion

        #region 删除N分钟（最后修改时间）之前的日志

        private static int _delInterval;
        public static int DelInterval
        {
            get
            {
                if (_delInterval <= 0)
                {
                    var logDelInterval = GetAppSettings("Log_DelInterval");
                    if (string.IsNullOrEmpty(logDelInterval))
                    {
                        _delInterval = 1 * 60 * 24 * 15;
                    }
                    else
                        _delInterval = Convert.ToInt32(logDelInterval.ToCompute());
                }
                return _delInterval;
            }
        }
        #endregion

        #region 部分日志文件大小

        private static int _sectionlogFileSize;

        public static int SectionlogFileSize
        {
            get
            {
                if (_sectionlogFileSize <= 0)
                {
                    var logSectionlogFileSize = GetAppSettings("Log__SectionlogFileSize");
                    if (string.IsNullOrEmpty(logSectionlogFileSize))
                    {
                        _sectionlogFileSize = 1024 * 1024 * 1;
                    }
                    else
                        _sectionlogFileSize = Convert.ToInt32(logSectionlogFileSize.ToCompute());
                }
                return _sectionlogFileSize;
            }
        }
        #endregion

        #region 变动文件大小

        private static int _fileSize;

        public static int FileSize
        {
            get
            {
                if (_fileSize <= 0)
                {
                    var logFileSize = GetAppSettings("Log_FileSize");
                    if (string.IsNullOrEmpty(logFileSize))
                        _fileSize = 1024 * 1024 * 4;
                    else
                        _fileSize = Convert.ToInt32(logFileSize.ToCompute());
                }
                return _fileSize;
            }
        }
        #endregion

        #endregion

    }
}