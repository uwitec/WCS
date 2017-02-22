using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Threading;
using log4net;


namespace WCS.DevSystem
{
    public class HttpHelper
    {
        #region 私有变量
        private static CookieContainer cc = new CookieContainer();
        private static string contentType = "application/x-www-form-urlencoded;charset=UTF-8";
        private static string accept = "image/gif, image/x-xbitmap, image/jpeg, image/pjpeg, application/x-shockwave-flash, application/x-silverlight, application/vnd.ms-excel, application/vnd.ms-powerpoint, application/msword, application/x-ms-application, application/x-ms-xbap, application/vnd.ms-xpsdocument, application/xaml+xml, application/x-silverlight-2-b1, */*";
        private static string userAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1; .NET CLR 2.0.50727; .NET CLR 3.0.04506.648; .NET CLR 3.5.21022)";
        private static Encoding encoding = Encoding.GetEncoding("utf-8");
        private static int delay = 3000;//延迟访问防止连续访问被发现 
        private static int maxTry = 300;
        private static int currentTry = 0;
        private static bool mFinished = true;
        #endregion

        #region 属性
        /// <summary></summary> 
        /// Cookie容器 
        ///  
        public static CookieContainer CookieContainer
        {
            get
            {
                return cc;
            }
        }

        /// <summary></summary> 
        /// 获取网页源码时使用的编码 
        ///  
        /// <value></value> 
        public static Encoding Encoding
        {
            get
            {
                return encoding;
            }
            set
            {
                encoding = value;
            }
        }

        public static int NetworkDelay
        {
            get
            {
                Random r = new Random();
                return (r.Next(delay / 1000, delay / 1000 * 2)) * 1000;
            }
            set
            {
                delay = value;
            }
        }

        public static int MaxTry
        {
            get
            {
                return maxTry;
            }
            set
            {
                maxTry = value;
            }
        }
        #endregion

        #region 公共方法
        /// <summary></summary> 
        /// 获取指定页面的HTML代码 
        ///  
        /// <param name="url">指定页面的路径 
        /// <param name="postData">回发的数据 
        /// <param name="isPost">是否以post方式发送请求 
        /// <param name="cookieCollection">Cookie集合 
        /// <returns></returns> 
        public static string GetHtml(string url, string postData, bool isPost, CookieContainer cookieContainer, out string strError)
        {
            strError = string.Empty;
            ILog log = LogManager.GetLogger("log4net");
            if (string.IsNullOrEmpty(postData))
            {
                return GetHtml(url, cookieContainer, out strError);
            }

            //Thread.Sleep(NetworkDelay);//延迟访问 

            //currentTry++;

            HttpWebRequest httpWebRequest = null;
            HttpWebResponse httpWebResponse = null;
            try
            {
                //  log.Info(string.Format("操作人:[{0}]-->请求调用开始-->{1}-->{2}", GlobalConfig.UserInfo.UserName, url, postData));
                byte[] byteRequest = Encoding.UTF8.GetBytes(postData);

                httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(url);
                httpWebRequest.CookieContainer = cookieContainer;
                httpWebRequest.ContentType = contentType;
                httpWebRequest.ServicePoint.ConnectionLimit = maxTry;
                httpWebRequest.Referer = url;
                httpWebRequest.Accept = accept;
                httpWebRequest.UserAgent = userAgent;
                httpWebRequest.Method = isPost ? "POST" : "GET";
                httpWebRequest.ContentLength = byteRequest.Length;

                Stream stream = httpWebRequest.GetRequestStream();
                stream.Write(byteRequest, 0, byteRequest.Length);
                stream.Close();

                httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                Stream responseStream = httpWebResponse.GetResponseStream();
                StreamReader streamReader = new StreamReader(responseStream, encoding);
                string html = streamReader.ReadToEnd();
                streamReader.Close();
                responseStream.Close();
                currentTry = 0;

                httpWebRequest.Abort();
                httpWebResponse.Close();

                return html;
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                if (e.Message.Equals("远程服务器返回错误: (432)。"))
                    strError = "命令不存在或用户权限不够";
                else
                    strError = e.Message;
                /*
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(DateTime.Now.ToString("HH:mm:ss ") + e.Message);
                Console.ForegroundColor = ConsoleColor.White;

                if (currentTry <= maxTry)
                {
                    GetHtml(url, postData, isPost, cookieContainer);
                }
                currentTry--;
                */
                if (httpWebRequest != null)
                {
                    httpWebRequest.Abort();
                } if (httpWebResponse != null)
                {
                    httpWebResponse.Close();
                }
                return string.Empty;
            }
            finally
            {
                log.Info("请求调用结束");
            }
        }

        /// <summary></summary> 
        /// 获取指定页面的HTML代码 
        ///  
        /// <param name="url">指定页面的路径 
        /// <param name="cookieCollection">Cookie集合 
        /// <returns></returns> 
        public static string GetHtml(string url, CookieContainer cookieContainer, out string strError)
        {
            //Thread.Sleep(NetworkDelay);
            strError = string.Empty;
            //currentTry++;
            HttpWebRequest httpWebRequest = null;
            HttpWebResponse httpWebResponse = null;
            string html = string.Empty;
            try
            {
                if (mFinished)
                {
                    mFinished = false;
                    httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(url);
                    httpWebRequest.CookieContainer = cookieContainer;
                    httpWebRequest.ContentType = contentType;
                    httpWebRequest.ServicePoint.ConnectionLimit = maxTry;
                    httpWebRequest.Referer = url;
                    httpWebRequest.Accept = accept;
                    httpWebRequest.UserAgent = userAgent;
                    httpWebRequest.Method = "GET";

                    httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    Stream responseStream = httpWebResponse.GetResponseStream();
                    StreamReader streamReader = new StreamReader(responseStream, encoding);
                    html = streamReader.ReadToEnd();
                    streamReader.Close();
                    responseStream.Close();

                    //currentTry--;

                    httpWebRequest.Abort();
                    httpWebResponse.Close();

                }
                return html;
            }
            catch (Exception e)
            {
                ILog log = LogManager.GetLogger("log4net");
                log.Error(e.Message);
                strError = e.Message;
                /*
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(DateTime.Now.ToString("HH:mm:ss ") + e.Message);
                Console.ForegroundColor = ConsoleColor.White;
                if (currentTry <= maxTry)
                {
                    GetHtml(url, cookieContainer);
                }

                currentTry--;*/

                if (httpWebRequest != null)
                {
                    httpWebRequest.Abort();
                } if (httpWebResponse != null)
                {
                    httpWebResponse.Close();
                }
                return string.Empty;
            }
            finally
            {
                mFinished = true;
            }
        }

        /// <summary></summary> 
        /// 获取指定页面的HTML代码 
        ///  
        /// <param name="url">指定页面的路径 
        /// <returns></returns> 
        public static string GetHtml(string url, out string strError)
        {
            return GetHtml(url, cc, out strError);
        }

        /// <summary></summary> 
        /// 获取指定页面的HTML代码 
        ///  
        /// <param name="url">指定页面的路径 
        /// <param name="postData">回发的数据 
        /// <param name="isPost">是否以post方式发送请求 
        /// <returns></returns> 
        public static string GetHtml(string url, string postData, bool isPost, out string strError)
        {
            return GetHtml(url, postData, isPost, cc, out strError);
        }

        /// <summary></summary> 
        /// 获取指定页面的Stream 
        ///  
        /// <param name="url">指定页面的路径 
        /// <param name="postData">回发的数据 
        /// <param name="isPost">是否以post方式发送请求 
        /// <param name="cookieCollection">Cookie集合 
        /// <returns></returns> 
        public static Stream GetStream(string url, CookieContainer cookieContainer)
        {
            ////Thread.Sleep(delay); 

            //currentTry++;
            HttpWebRequest httpWebRequest = null;
            HttpWebResponse httpWebResponse = null;

            try
            {

                httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(url);
                httpWebRequest.CookieContainer = cookieContainer;
                httpWebRequest.ContentType = contentType;
                httpWebRequest.ServicePoint.ConnectionLimit = maxTry;
                httpWebRequest.Referer = url;
                httpWebRequest.Accept = accept;
                httpWebRequest.UserAgent = userAgent;
                httpWebRequest.Method = "GET";

                httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                Stream responseStream = httpWebResponse.GetResponseStream();
                //currentTry--;

                //httpWebRequest.Abort(); 
                //httpWebResponse.Close(); 

                return responseStream;
            }
            catch (Exception e)
            {
                /*
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(DateTime.Now.ToString("HH:mm:ss ") + e.Message);
                Console.ForegroundColor = ConsoleColor.White;

                if (currentTry <= maxTry)
                {
                    GetHtml(url, cookieContainer);
                }

                currentTry--;
                */
                if (httpWebRequest != null)
                {
                    httpWebRequest.Abort();
                } if (httpWebResponse != null)
                {
                    httpWebResponse.Close();
                }
                return null;
            }
        }

        #endregion
    }
}
