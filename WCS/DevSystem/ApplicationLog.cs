using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace WCS.DevSystem
{
    /// ＜summary＞
    /// 事件日志记录类，提供事件日志记录支持 
    /// ＜remarks＞
    /// 定义了4个日志记录方法 (error, warning, info, trace) 
    /// ＜/remarks＞
    /// ＜/summary＞
    public  class ApplicationLog
    {
        public static string MachineName = "InfoSky";
        public static string ApplicationName = "InfoSky";

        /// ＜summary＞
        /// 将错误信息记录到Win2000/NT事件日志中
        /// ＜param name="message"＞需要记录的文本信息＜/param＞
        /// ＜/summary＞
        public static void WriteError(String message)
        {
            LogHelper.LogSimlpleString(message);
            //WriteLog(TraceLevel.Error, message);
        }

        /// ＜summary＞
        /// 将错误信息记录到Win2000/NT事件日志中
        /// ＜param name="message"＞需要记录的文本信息＜/param＞
        /// ＜/summary＞
        public static void WriteError(Exception ex)
        {
            LogHelper.LogException(ex);
            //WriteLog(TraceLevel.Error, ex.Message+"\n"+ex.StackTrace);
        }

        /// ＜summary＞
        /// 将警告信息记录到Win2000/NT事件日志中
        /// ＜param name="message"＞需要记录的文本信息＜/param＞
        /// ＜/summary＞
        public static void WriteWarning(String message)
        {
            LogHelper.LogSimlpleString(message);
            //WriteLog(TraceLevel.Warning, message);
        }

        /// ＜summary＞
        /// 将提示信息记录到Win2000/NT事件日志中
        /// ＜param name="message"＞需要记录的文本信息＜/param＞
        /// ＜/summary＞
        public static void WriteInfo(String message)
        {
            LogHelper.LogSimlpleString(message);
            //WriteLog(TraceLevel.Info, message);
        }
        /// ＜summary＞
        /// 将跟踪信息记录到Win2000/NT事件日志中
        /// ＜param name="message"＞需要记录的文本信息＜/param＞
        /// ＜/summary＞
        public static void WriteTrace(String message)
        {
            LogHelper.LogSimlpleString(message);
            //WriteLog(TraceLevel.Verbose, message);
        }

        /// ＜summary＞
        /// 格式化记录到事件日志的文本信息格式
        /// ＜param name="ex"＞需要格式化的异常对象＜/param＞
        /// ＜param name="catchInfo"＞异常信息标题字符串.＜/param＞
        /// ＜retvalue＞
        /// ＜para＞格式后的异常信息字符串，包括异常内容和跟踪堆栈.＜/para＞
        /// ＜/retvalue＞
        /// ＜/summary＞
        public static String FormatException(Exception ex, String catchInfo)
        {
            StringBuilder strBuilder = new StringBuilder();
            if (catchInfo != String.Empty)
            {
                strBuilder.Append(catchInfo).Append("\r\n");
            }
            strBuilder.Append(ex.Message).Append("\r\n").Append(ex.StackTrace);
            return strBuilder.ToString();
        }

        /// ＜summary＞
        /// 实际事件日志写入方法
        /// ＜param name="level"＞要记录信息的级别（error,warning,info,trace).＜/param＞
        /// ＜param name="messageText"＞要记录的文本.＜/param＞
        /// ＜/summary＞
        private static void WriteLog(TraceLevel level, String messageText)
        {
            try
            {
                EventLogEntryType LogEntryType;
                switch (level)
                {
                    case TraceLevel.Error:
                        LogEntryType = EventLogEntryType.Error;
                        break;
                    case TraceLevel.Warning:
                        LogEntryType = EventLogEntryType.Warning;
                        break;
                    case TraceLevel.Info:
                        LogEntryType = EventLogEntryType.Information;
                        break;
                    case TraceLevel.Verbose:
                        LogEntryType = EventLogEntryType.SuccessAudit;
                        break;
                    default:
                        LogEntryType = EventLogEntryType.SuccessAudit;
                        break;
                }

                EventLog eventLog = new EventLog("Application");
                eventLog.Source = "InfoSky";
                //写入事件日志
                eventLog.WriteEntry(messageText, LogEntryType);
            }
            catch { }
        }
    }
}
