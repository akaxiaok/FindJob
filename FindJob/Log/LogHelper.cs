using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Text;
using System.Threading.Tasks;


namespace FindJob.Log
{
    public class LogHelper
    {
        /// <summary>
        ///   消息队列
        /// </summary>
        private static Queue<LogModel> _logQuene = new Queue<LogModel>();

        /// <summary>
        ///   消息队列 只读
        /// </summary>
        public static Queue<LogModel> LogQuene
        {
            get { return LogHelper._logQuene; }
        }

        /// <summary>
        ///   标准锁
        /// </summary>
        private static string _myLock = "true";

        /// <summary>
        /// 写入日志  （异步单线程）
        /// </summary>
        /// <param name="logmede"></param>
        public static void LogWrite(LogModel logmede)
        {
            lock (_myLock)
            {
                _logQuene.Enqueue(logmede);
                LogStartWrite();
            }
        }

        /// <summary>
        ///   文件编码格式
        /// </summary>
        public static Encoding Encoding = Encoding.Default;

        /// <summary>
        ///   是否开始自动记录日志
        /// </summary>
        private static bool _isStart = false;

        /// <summary>
        ///   用来标识最后一次检查是否需要清理日志文件 时间
        /// </summary>
        private static DateTime _time = DateTime.MinValue;

        /// <summary>
        ///   每个日志文件夹对应的文件下标
        /// </summary>
        private static Dictionary<string, int> _logFileNum = new Dictionary<string, int>();

        /// <summary>
        ///   开始把队列消息写入文件
        /// </summary>
        private static void LogStartWrite()
        {
            if (_isStart)
                return;
            _isStart = true;

            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    if (LogHelper.LogQuene.Count >= 1)
                    {
                        LogModel m = null;
                        lock (_myLock)
                            m = LogHelper.LogQuene.Dequeue();
                        if (m == null)
                            continue;

                        if (string.IsNullOrEmpty(LogConfig.LogFilePath))
                        {
                            throw new Exception("请先初始化日志保存路径logModel._logFilePath");
                        }
                        TestingInvalid();
                        if (!Directory.Exists(LogConfig.LogFilePath + m.LogFileName + @"\"))
                            Directory.CreateDirectory(LogConfig.LogFilePath + m.LogFileName + @"\");
                        if (!_logFileNum.Keys.Contains(m.LogFileName))
                            _logFileNum.Add(m.LogFileName, 0);

                        string sectionfileFullName = LogConfig.LogFilePath + m.LogFileName + @"\" + m.LogFileName + "_" +
                                                     _logFileNum[m.LogFileName].ToString("000") + ".txt";
                        string topSectionfileFullName = sectionfileFullName;

                        string logfileFullName = LogConfig.LogFilePath + m.LogFileName + @"\" + m.LogFileName + ".txt";

                        FileInfo file = new FileInfo(sectionfileFullName);
                        while (file.Exists && file.Length >= LogConfig.SectionlogFileSize)
                        {
                            topSectionfileFullName = sectionfileFullName;
                            _logFileNum[m.LogFileName]++;
                            sectionfileFullName = LogConfig.LogFilePath + m.LogFileName + @"\" + m.LogFileName + "_" +
                                                  _logFileNum[m.LogFileName].ToString("000") + ".txt";
                            file = new FileInfo(sectionfileFullName);
                        }
                        try
                        {
                            if (!file.Exists)
                            {
                                File.WriteAllText(sectionfileFullName, m.LogMessage, Encoding);

                                FileInfo logFile = new FileInfo(logfileFullName);

                                if (logFile.Exists && logFile.Length >= LogConfig.FileSize)
                                    File.WriteAllText(logfileFullName,
                                        File.ReadAllText(topSectionfileFullName, Encoding), Encoding);
                            }
                            else
                                File.AppendAllText(sectionfileFullName, m.LogMessage, Encoding);
                        }
                        catch (System.Exception ex)
                        {
                            throw ex;
                        }

                    }
                    else
                    {
                        _isStart = false;
                        break;
                    }
                }

            });
        }


        /// <summary>
        ///   检查并删除 之前 之外的 日志文件
        /// </summary>
        public static void TestingInvalid()
        {
            if (_time.AddMinutes(LogConfig.TestingInterval) <= DateTime.Now)
            {
                try
                {
                    _time = DateTime.Now;
                    List<string> keyNames = new List<string>();

                    foreach (var logFileName in _logFileNum.Keys)
                    {
                        CreatePath(LogConfig.LogFilePath + logFileName + @"\");
                        DirectoryInfo dir = new DirectoryInfo(LogConfig.LogFilePath + logFileName + @"\");
                        if (dir.CreationTime.AddMinutes(LogConfig.DelInterval) <= DateTime.Now)

                            foreach (var fileInfo in dir.GetFiles())
                            {
                                if (fileInfo.LastWriteTime.AddMinutes(LogConfig.DelInterval) <= DateTime.Now)
                                    File.Delete(fileInfo.FullName);
                            }
                        if (dir.GetFiles().Length == 0)
                            keyNames.Add(logFileName);

                    }
                    foreach (var key in keyNames)
                    {
                        _logFileNum.Remove(key);
                        Directory.Delete(LogConfig.LogFilePath + key + @"\", false);
                    }
                }

                catch (System.Exception ex)
                {
                    LogSave.ErrLogSave("手动捕获[检测并删除日志出错!]", ex, "记录日志出错");
                }
            }
        }


        public static bool CreatePath(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                return true;
            }

            return false;
        }

    }

}