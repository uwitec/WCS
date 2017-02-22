using System;
//using System.Data.OracleClient;
using System.Text;
using System.Threading;
using System.Windows.Forms;
//using System.IO.Ports;
using System.Net;
using System.Net.Sockets;
using MySql.Data.MySqlClient;

namespace WCS.DevSystem
{
    /// <summary>
    /// 以太网通讯读码器 
    /// </summary>
    public class BarCodeSocket3
    {
        private New_Main_Form mainFrm;
        //private string lastStatus = "";
        /// <summary>
        /// 主窗口正在关闭，退出线程
        /// </summary>
        public bool closing = false;
        /// <summary>
        /// 条码读码器工作状态 true 工作，false 暂停
        /// </summary>
        public bool WorkStatusRun = false;

        #region Socket通讯参数
        ///// <summary>
        ///// 服务器IP，即康奈视master的ip
        ///// </summary>
        //public string IP_Server = string.Empty;
        ///// <summary>
        ///// 接收数据的端口 默认23
        ///// </summary>
        //public string Port_Receive = string.Empty;
        /// <summary>
        /// 客户端socket
        /// </summary>
        public Socket socket_client;
        public bool SocketConnectFlag = false;
        #endregion

        #region 串口通讯参数
        //private SerialPort sp = new SerialPort();
        #endregion

        public BarCodeSocket3(New_Main_Form mainFrm)
        {
            ThreadExceptionDialog.CheckForIllegalCrossThreadCalls = false; //允许直接访问线程之间的控件
            this.mainFrm = mainFrm;
            //OpenCom();
            //IP_Server = "10.21.171.193";
            //Port_Receive = "23";
            //OpenSocket();
        }

        /// <summary>
        /// 打开Socket
        /// </summary>
        public void OpenSocket(string IP_Server, string Port_Receive)
        {
            try
            {
                socket_client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPAddress ipAddress = IPAddress.Parse(IP_Server);
                IPEndPoint ipe = new IPEndPoint(ipAddress, int.Parse(Port_Receive));
                socket_client.Connect(ipe);
                SocketConnectFlag = true;
                // mainFrm.lblSannerStatus.Text = "初始化读码器成功";
                mainFrm.AddSuccessToListView("初始化读码器" + IP_Server + "成功");
                LogHelper.LogSimlpleString(DateTime.Now.ToString("G") + "初始化读码器" + IP_Server + "成功");
            }
            catch (Exception ex)
            {
                SocketConnectFlag = false;
                //mainFrm.lblSannerStatus.Text = "初始化读码器失败";
                mainFrm.AddErrToListView("初始化读码器" + IP_Server + "失败！异常：" + ex.Message);
                LogHelper.LogSimlpleString(DateTime.Now.ToString("G") + "初始化读码器" + IP_Server + "失败！异常：" + ex.Message);
                return;
            }
        }

        /// <summary>
        /// 读数据
        /// </summary>
        public void Scanning()
        {
            string str_receivedata = string.Empty;//返回数据
            //int cig_length = 15;//接受数据的数组长度  {13位条码}
            //byte[] by_data = new byte[cig_length];
            string oldData = string.Empty;//上次剩余的数据
            int li_start_pos = -1;
            int li_end_pos = -1;
            string barcode = string.Empty;//条码
            //string barCodes;
            //int i = 0;
            //DateTime ldt_last_time = DateTime.Now;
            //string ls_last_barcode = string.Empty;
            //double ldb_time = 1500;
            while (!closing)
            {
                mainFrm.thrBarCodeScanIsRuning = true;
                try
                {

                    #region Socket通讯
                    if (SocketConnectFlag && socket_client.Available > 0)// && socket_client.Available >= cig_length
                    {
                        byte[] by_data = new byte[socket_client.Available];
                        int k = socket_client.Receive(by_data, by_data.Length, 0);
                        if (k > 0)
                        {
                            str_receivedata =  ASCIIEncoding.ASCII.GetString(by_data);
                            li_start_pos = str_receivedata.IndexOf("kg", 0);
                           
                            while (li_start_pos >0 )
                            {
                                barcode = str_receivedata.Replace("kg","").Trim();
                                if (WorkStatusRun)
                                {
                                    mainFrm.AddErrToListView(barcode);
                                    // string strSql = "update td_wcs_barcode set BARCODE  = '" + barcode.Substring(2) + "' where SCNNER_NO = '" + barcode.Substring(0,2) + "'";

                                    string strSql = "update td_wcs_barcode set BARCODE2  = @barcode where SCNNER_NO = 22";
                                    MySqlParameter pt = new MySqlParameter("@barcode", MySqlDbType.VarChar, 30);
                                    pt.Value = barcode;

                                   
                                    MySqlConnection dbConn11;
                                    dbConn11 = new MySqlConnection(mainFrm.dbConnectionString);
                                    dbConn11.Open();
                                    MySqlCommand mc = new MySqlCommand(strSql, dbConn11);
                                    mc.Parameters.Add(pt);
                                  
                                    mc.ExecuteNonQuery();
                                    dbConn11.Close();
                                    //MySqlHelper.ExecuteNonQuery(mainFrm.dbConn,strSql,MySqlParameter)
                                    //OracleHelper.ExecuteNonQuery(mainFrm.dbConn, CommandType.Text, strSql);
                                    ////1秒内重复条码则过滤
                                    //if (!(barcode == ls_last_barcode && barcode != "NOREAD" && (DateTime.Now - ldt_last_time).TotalMilliseconds <= ldb_time))
                                    //{
                                    // mainFrm.ScanMsgOperationLogic(barcode);
                                    //}
                                    //ls_last_barcode = barcode;
                                    //ldt_last_time = DateTime.Now;

                                }
                                //if (str_receivedata.Length - 1 > li_end_pos)
                                //{
                                //    str_receivedata = str_receivedata.Substring(li_end_pos + 1);
                                //}
                                //else
                                //{
                                    str_receivedata = string.Empty;
                                    li_start_pos = 0;
                                //}
                                //li_start_pos = str_receivedata.IndexOf("{", 0);
                                //li_end_pos = str_receivedata.IndexOf("}", 0);
                            }
                           // oldData = str_receivedata;
                        }
                    }
                    else
                    {
                        Thread.Sleep(10);
                    }
                    #endregion
                }
                catch (Exception ex)
                {
                    li_start_pos = str_receivedata.IndexOf("{", 0);
                    if (li_start_pos >= 0)
                    {
                        oldData = str_receivedata.Substring(li_start_pos); ;
                    }
                    else
                    {
                        oldData = string.Empty;
                    }
                    //MessageBox.Show("条码处理异常！" + ex.Message);
                    mainFrm.AddErrToListView("条码处理异常：" + ex.Message);
                    LogHelper.LogSimlpleString(DateTime.Now.ToString("G") + "条码处理异常：" + ex.Message);
                }
                mainFrm.thrBarCodeScanIsRuning = false;
            }
            if (SocketConnectFlag)
            {
                socket_client.Shutdown(SocketShutdown.Both);
                socket_client.Close();
            }

        }

    }
}

