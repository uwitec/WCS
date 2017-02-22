using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Collections;

namespace WCS.DevSystem
{
    public  class LogHelper
    {
        public static log4net.ILog log = null;
        public string filePath = string.Empty;
        public string loggerName = string.Empty;

        #region "构造函数"
        //hzr:2008/06/17
        //默认构造函数
        static LogHelper()
        {
            //从启动目录获取配置文件
            if (log == null)
            {
                string path = string.Empty;
                //path = System.Windows.Forms.Application.StartupPath;
                ////log4net.Config.DOMConfigurator.Configure(new FileInfo(@"F:\TRAXONWJH\Common\bin\Debug\Common.dll.config"));
                //log4net.Config.DOMConfigurator.Configure(new FileInfo(path + "\\Log4net.config"));
                //log = log4net.LogManager.GetLogger("testApp.Logging");
                if (log == null)
                {
                    log4net.Config.XmlConfigurator.Configure();
                    log = log4net.LogManager.GetLogger("iLogging");
                }
            }
        }
        #endregion

        //hzr:2008/06/17
        //单独将一个STRING记录入日志 默认为DEBUG级
        public static void LogSimlpleString(string str)
        {
            try
            {
                //默认前缀为当前时间
                log.Debug(System.DateTime.Now.ToLongDateString() + ":" + str + System.Environment.NewLine);
            }
            catch (Exception ex)
            {
                //保存到系统日志中
                EventLogEntryType LogEntryType = EventLogEntryType.Error;
                EventLog eventLog = new EventLog("Application");
                eventLog.Source = "GBIA-航显";
                //写入事件日志
                eventLog.WriteEntry(ex.Message, LogEntryType);
            }
        }

        public static void LogException(Exception ex)
        {
            string strContent = "<ExceptionMessage Time=\"" + DateTime.Now.ToString() + "\"><![CDATA["
                + ex.Message + "]]></ExceptionMessage>" +
                System.Environment.NewLine
                + "<ExceptionStatck Time=\"" + DateTime.Now.ToString() + "\"><![CDATA[" +
                ex.StackTrace + "]]></ExceptionStatck>" + System.Environment.NewLine;

            //将该信息记录于本地文件中
            log.Debug(strContent);
        }

        public static void AddQueueLog(Queue que)
        {
            try
            {
                int QueueCount = 0;
                QueueCount = que.Count;
                if (QueueCount == 0) return;
                log.Debug("<TrancationItem Time=\"" + DateTime.Now.ToString() + "\">" + System.Environment.NewLine);
                for (int i = 0; i < QueueCount; i++)
                {
                    log.Debug("<ActionItem SEQ=\"" + i.ToString() + "\"><![CDATA[");
                    log.Debug(que.Dequeue().ToString());
                    log.Debug("]]></ActionItem>" + System.Environment.NewLine);
                }
                log.Debug("</TrancationItem>" + System.Environment.NewLine);
            }
            catch (Exception ex)
            { ApplicationLog.WriteError(ex); }
            que.Clear();
            return;
        }
    }
}
