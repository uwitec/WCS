using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using WCS.PlcSystem;
using WCS.DevSystem;
using System.Data;
using WCS.Data;
using System.Windows.Forms;

namespace WCS
{
    class TaskAutoRun
    {
        private New_Main_Form mainform;
        public bool closing = false;

        //private DataSet taskDs = new DataSet();

        public TaskAutoRun(New_Main_Form mainform)
        {
            this.mainform = mainform;
        }
        //public void CmdDeviceAutoRun()
        //{ 
        //    string ComandText=string.Empty;
        //    string strsql = string.Empty;
        //    while(!closing)
        //    {
        //        mainform.cmdDeviceThreadStatus = true;

        //        if (mainform.workModel == 1)
        //        {

        //        }
        //        mainform.cmdDeviceThreadStatus = false;
        //        Thread.Sleep(1000);
        //    }

        //}
        /// <summary>
        /// 刷新设备状态
        /// </summary>
        public void CmdDeviceRefreshStatus()
        {
            if (mainform.deviceInited)  //只有初始化设备后才能刷新设备状态
            {
                foreach (KeyValuePair<string, BESDevice> device in mainform.cmdDevices)
                {
                    device.Value.RefreshStatus();
                }
            }
           
        }

        /// <summary>
        /// 刷新设备状态2
        /// </summary>
        public void CmdDeviceRefreshStatus2()
        {
            if (mainform.deviceInited)  //只有初始化设备后才能刷新设备状态
            {
                foreach (KeyValuePair<string, BESDevice> device in mainform.cmdDevices2)
                {
                    device.Value.RefreshStatus();
                }
            }

        }
        /// <summary>
        /// 刷新设备状态3
        /// </summary>
        public void CmdDeviceRefreshStatus3()
        {
            if (mainform.deviceInited)  //只有初始化设备后才能刷新设备状态
            {
                foreach (KeyValuePair<string, BESDevice> device in mainform.cmdDevices3)
                {
                    device.Value.RefreshStatus();
                }
            }

        }
        /// <summary>
        //刷新设备状态4
        /// </summary>
        public void CmdDeviceRefreshStatus4()
        {
            if (mainform.deviceInited)  //只有初始化设备后才能刷新设备状态
            {
                foreach (KeyValuePair<string, BESDevice> device in mainform.cmdDevices4)
                {
                    device.Value.RefreshStatus();
                }
            }

        }
        /// <summary>
        /// 刷新设备显示
        /// </summary>
        public void CmdDeviceRefreshDisplay()
        {
            if (mainform.deviceInited)  //只有初始化设备后才能刷新设备显示
            {
                foreach (KeyValuePair<string, BESDevice> device in mainform.cmdDevices)
                {
                    device.Value.RefreshDisplay();
                }
            }
          
        }

          /// <summary>
        /// 刷新设备显示2
        /// </summary>
        public void CmdDeviceRefreshDisplay2()
        {
            if (mainform.deviceInited)  //只有初始化设备后才能刷新设备显示
            {
                foreach (KeyValuePair<string, BESDevice> device in mainform.cmdDevices2)
                {
                    device.Value.RefreshDisplay();
                }
            }
          
        }
          /// <summary>
        /// 刷新设备显示3
        /// </summary>
        public void CmdDeviceRefreshDisplay3()
        {
            if (mainform.deviceInited)  //只有初始化设备后才能刷新设备显示
            {
                foreach (KeyValuePair<string, BESDevice> device in mainform.cmdDevices3)
                {
                    device.Value.RefreshDisplay();
                }
            }
          
        }
          /// <summary>
        /// 刷新设备显示4
        /// </summary>
        public void CmdDeviceRefreshDisplay4()
        {
            if (mainform.deviceInited)  //只有初始化设备后才能刷新设备显示
            {
                foreach (KeyValuePair<string, BESDevice> device in mainform.cmdDevices4)
                {
                    device.Value.RefreshDisplay();
                }
            }
          
        }
        /// <summary>
        /// 命令设备自动运行
        /// </summary>
        public void CmdDeviceAutoRun()
        {
           
            while (!closing) //在关闭主窗口时不再进行新的循环
            {
                mainform.cmdDeviceThreadStatus = true;
                if (mainform.dbIsConned)  //连接数据库后，才有可能刷新状态数据和显示
                {
                    CmdDeviceRefreshStatus();
                    CmdDeviceRefreshDisplay();
                }
              
                mainform.cmdDeviceThreadStatus = false;
                //联机工作状态，任务处理
                #region 联机运行
                if (mainform.dbIsConned && mainform.deviceInited && mainform.plcIsOK && mainform.workModel == 1)
                {
                    mainform.cmdDeviceThreadStatus = true;
                   

                    #region 扫描命令设备
                    foreach (KeyValuePair<string, BESDevice> device in mainform.cmdDevices)
                    {
                        //检查可写命令设备的状态；当设备状态为"10"时，从数据库中取任务；
                        //并且在运行自动状态，第0位为1运行状态，第3位为1自动状态  0x0001,0x0002,0x0004,0x0008
                        if (device.Value.returnTaskStatus == "10" )
                        {
                            string strSql;
                            switch (device.Value.commandType)  //对于扫描核对SM设备，需由条码扫描器触发
                            {
                               
                                case "SMFL"://令
                                   
                                        device.Value.TaskPress();
                                    break;
                            }
                        }
                       

                    }
                    #endregion
                    mainform.cmdDeviceThreadStatus = false;
                }
                #endregion
                Thread.Sleep(50);
            }
        }

        /// <summary>
        /// 命令设备自动运行2
        /// </summary>
        public void CmdDeviceAutoRun2()
        {

            while (!closing) //在关闭主窗口时不再进行新的循环
            {
                mainform.cmdDeviceThreadStatus2 = true;
                if (mainform.dbIsConned)  //连接数据库后，才有可能刷新状态数据和显示
                {
                    CmdDeviceRefreshStatus2();
                    CmdDeviceRefreshDisplay2();
                }

                mainform.cmdDeviceThreadStatus = false;
                //联机工作状态，任务处理
                #region 联机运行
                if (mainform.dbIsConned && mainform.deviceInited && mainform.plcIsOK && mainform.workModel == 1)
                {
                    mainform.cmdDeviceThreadStatus = true;


                    #region 扫描命令设备
                    foreach (KeyValuePair<string, BESDevice> device in mainform.cmdDevices2)
                    {
                        //检查可写命令设备的状态；当设备状态为"10"时，从数据库中取任务；
                        //并且在运行自动状态，第0位为1运行状态，第3位为1自动状态  0x0001,0x0002,0x0004,0x0008
                        if (device.Value.returnTaskStatus == "10")
                        {
                            string strSql;
                            switch (device.Value.commandType)  //对于扫描核对SM设备，需由条码扫描器触发
                            {

                                case "SMFL"://令

                                    device.Value.TaskPress();
                                    break;
                            }
                        }


                    }
                    #endregion
                    mainform.cmdDeviceThreadStatus2 = false;
                }
                #endregion
                Thread.Sleep(50);
            }
        }
        /// <summary>
        /// 命令设备自动运行3
        /// </summary>
        public void CmdDeviceAutoRun3()
        {

            while (!closing) //在关闭主窗口时不再进行新的循环
            {
                mainform.cmdDeviceThreadStatus3 = true;
                if (mainform.dbIsConned)  //连接数据库后，才有可能刷新状态数据和显示
                {
                    CmdDeviceRefreshStatus3();
                    CmdDeviceRefreshDisplay3();
                }

                mainform.cmdDeviceThreadStatus = false;
                //联机工作状态，任务处理
                #region 联机运行
                if (mainform.dbIsConned && mainform.deviceInited && mainform.plcIsOK && mainform.workModel == 1)
                {
                    mainform.cmdDeviceThreadStatus = true;


                    #region 扫描命令设备
                    foreach (KeyValuePair<string, BESDevice> device in mainform.cmdDevices3)
                    {
                        //检查可写命令设备的状态；当设备状态为"10"时，从数据库中取任务；
                        //并且在运行自动状态，第0位为1运行状态，第3位为1自动状态  0x0001,0x0002,0x0004,0x0008
                        if (device.Value.returnTaskStatus == "10")
                        {
                            string strSql;
                            switch (device.Value.commandType)  //对于扫描核对SM设备，需由条码扫描器触发
                            {

                                case "SMFL"://令

                                    device.Value.TaskPress();
                                    break;
                            }
                        }


                    }
                    #endregion
                    mainform.cmdDeviceThreadStatus3 = false;
                }
                #endregion
                Thread.Sleep(50);
            }
        }


        /// <summary>
        /// 命令设备自动运行
        /// </summary>
        public void CmdDeviceAutoRun4()
        {

            while (!closing) //在关闭主窗口时不再进行新的循环
            {
                mainform.cmdDeviceThreadStatus4 = true;
                if (mainform.dbIsConned)  //连接数据库后，才有可能刷新状态数据和显示
                {
                    CmdDeviceRefreshStatus4();
                    CmdDeviceRefreshDisplay4();
                }

                mainform.cmdDeviceThreadStatus = false;
                //联机工作状态，任务处理
                #region 联机运行
                if (mainform.dbIsConned && mainform.deviceInited && mainform.plcIsOK && mainform.workModel == 1)
                {
                    mainform.cmdDeviceThreadStatus = true;


                    #region 扫描命令设备
                    foreach (KeyValuePair<string, BESDevice> device in mainform.cmdDevices4)
                    {
                        //检查可写命令设备的状态；当设备状态为"10"时，从数据库中取任务；
                        //并且在运行自动状态，第0位为1运行状态，第3位为1自动状态  0x0001,0x0002,0x0004,0x0008
                        if (device.Value.returnTaskStatus == "10")
                        {
                            string strSql;
                            switch (device.Value.commandType)  //对于扫描核对SM设备，需由条码扫描器触发
                            {

                                case "SMFL"://令

                                    device.Value.TaskPress();
                                    break;
                            }
                        }


                    }
                    #endregion
                    mainform.cmdDeviceThreadStatus4 = false;
                }
                #endregion
                Thread.Sleep(50);
            }
        }

        public void AutoRun()
        {
            while (!closing)
            {
                mainform.nonCmdDeviceThreadStatus = true;

                if (mainform.dbIsConned && mainform.plcIsOK && mainform.deviceInited) //只有初始化plcSystem类后才能刷新PLC的系统状态
                {
                    mainform.plcSystemMS.RefreshStatus();

                    mainform.ssj_dev.RefreshDisplay();

                }
                mainform.nonCmdDeviceThreadStatus = false;
                Thread.Sleep(1000);
            }
        }
    }
}
