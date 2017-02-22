using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WCS.PlcSystem;
using OpcRcw.Da;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace WCS.DevSystem
{
    public class DeviceBase
    {
        public String[] deviceId; //设备编号
        public String[] deviceName; //设备名称
        public String[] devicePos; //所在TABPAGE
        public String[] showInf; //显示信息

        public OPCServer opcServer = new OPCServer();  //OPCServer对象
        public string[] commandDB;
        public string[] returnDB;
        public string[] controlDB;
        public string[] statusDB;
        public string[] failureDB;

        public string[] commandType;
        public string[] controlType;
        public string[] statusType;
        public string[] failureType;

        public int[] commandHandle;
        public int[] returnHandle;
        public int[] controlHandle;
        public int[] statusHandle;
        public int[] failureHandle;

        //public string[] device_status;   //设备状态
        public int[] xCoord;             //图形的坐标
        public int[] yCoord;
        public int[] xInc;
        public int[] yInc;

        public bool isBindToPLC = false;  //是否已初始化OPCServer
        public int[] deviceStatus; //deviceStatus为当前PLC返回的设备状态
        /// <summary>
        /// 当前PLC返回的设备故障
        /// </summary>
        public int[] deviceFailure;
        //public PictureBox[] pic;//显示设备图片
        public Label[] pic;
        //public Label[] pic1;

        public Int64 lastRefreshTicks; //保存上次刷新的时间，当两次刷新时间间隔较小（小于100ms）时，不进行刷新。

        public virtual bool BindToPLC()
        {
            opcServer = new OPCServer();
            if (!opcServer.Connect()) return false;
            if (!opcServer.AddGroup()) return false;
            int client = 1;
            if (commandDB != null)
            {
                OpcRcw.Da.OPCITEMDEF[] commandItem = new OPCITEMDEF[commandDB.Length];
                for (int i = 0; i < commandDB.Length; i++)
                {
                    commandItem[i].szAccessPath = "";
                    commandItem[i].bActive = 1;
                    commandItem[i].hClient = client;
                    commandItem[i].dwBlobSize = 1;
                    commandItem[i].pBlob = IntPtr.Zero;
                    commandItem[i].vtRequestedDataType = (int)VarEnum.VT_BSTR;
                    commandItem[i].szItemID = string.Format("S7:[S7 connection_1]{0}", commandDB[i]); //状态DB
                    client++;
                }
                if (!opcServer.AddItems(commandItem, commandHandle)) return false;
            }
          
            if (returnDB != null)
            {
                OpcRcw.Da.OPCITEMDEF[] returnitem = new OPCITEMDEF[returnDB.Length];
                for (int i = 0; i < returnDB.Length; i++)
                {
                    returnitem[i].szAccessPath = "";
                    returnitem[i].bActive = 1;
                    returnitem[i].hClient = client;
                    returnitem[i].dwBlobSize = 1;
                    returnitem[i].pBlob = IntPtr.Zero;
                    returnitem[i].vtRequestedDataType = (int)VarEnum.VT_BSTR;
                    returnitem[i].szItemID = string.Format("S7:[S7 connection_1]{0}", returnDB[i]); //状态DB
                    client++;
                }
                if (!opcServer.AddItems(returnitem, returnHandle)) return false;
            }

            if (controlDB!=null)
            {
                OpcRcw.Da.OPCITEMDEF[] controlitem = new OPCITEMDEF[controlDB.Length];
                for (int i = 0; i < controlDB.Length; i++)
                {

                    controlitem[i].szAccessPath = "";
                    controlitem[i].bActive = 1;
                    controlitem[i].hClient = client;
                    controlitem[i].dwBlobSize = 1;
                    controlitem[i].pBlob = IntPtr.Zero;
                    controlitem[i].vtRequestedDataType = (int)VarEnum.VT_BSTR;
                    controlitem[i].szItemID = string.Format("S7:[S7 connection_1]{0}", controlDB[i]); //状态DB
                    client++;
                }
                if (!opcServer.AddItems(controlitem, controlHandle)) return false;
            }

            if (statusDB != null)
            {
                OpcRcw.Da.OPCITEMDEF[] statusitem = new OPCITEMDEF[statusDB.Length];
                for (int i = 0; i < statusDB.Length; i++)
                {
                    statusitem[i].szAccessPath = "";
                    statusitem[i].bActive = 1;
                    statusitem[i].hClient = client;
                    statusitem[i].dwBlobSize = 1;
                    statusitem[i].pBlob = IntPtr.Zero;
                    statusitem[i].vtRequestedDataType = (int)VarEnum.VT_BSTR;
                    statusitem[i].szItemID = string.Format("S7:[S7 connection_1]{0}", statusDB[i]); //状态DB
                    client++;
                }

                if (!opcServer.AddItems(statusitem, statusHandle)) return false;
            }

            if (failureDB != null)
            {
                OpcRcw.Da.OPCITEMDEF[] failureitem = new OPCITEMDEF[failureDB.Length];
                for (int i = 0; i < failureDB.Length; i++)
                {
                    failureitem[i].szAccessPath = "";
                    failureitem[i].bActive = 1;
                    failureitem[i].hClient = client;
                    failureitem[i].dwBlobSize = 1;
                    failureitem[i].pBlob = IntPtr.Zero;
                    failureitem[i].vtRequestedDataType = (int)VarEnum.VT_BSTR;

                    failureitem[i].szItemID = string.Format("S7:[S7 connection_1]{0}", failureDB[i]); //状态DB
                    client++;
                }

                if (!opcServer.AddItems(failureitem, failureHandle)) return false;
            }

            isBindToPLC = true;
            return isBindToPLC;
        }

        public virtual bool BindToPLC(int dwRequestedUpdateRate)
        {
            opcServer = new OPCServer();
            if (!opcServer.Connect()) return false;
            if (!opcServer.AddGroup(dwRequestedUpdateRate)) return false;
            int client = 1;
            if (commandDB != null)
            {
                OpcRcw.Da.OPCITEMDEF[] commandItem = new OPCITEMDEF[commandDB.Length];
                for (int i = 0; i < commandDB.Length; i++)
                {
                    commandItem[i].szAccessPath = "";
                    commandItem[i].bActive = 1;
                    commandItem[i].hClient = client;
                    commandItem[i].dwBlobSize = 1;
                    commandItem[i].pBlob = IntPtr.Zero;
                    commandItem[i].vtRequestedDataType = (int)VarEnum.VT_BSTR;
                    commandItem[i].szItemID = string.Format("S7:[S7 connection_1]{0}", commandDB[i]); //状态DB
                    client++;
                }
                if (!opcServer.AddItems(commandItem, commandHandle)) return false;
            }

            if (returnDB != null)
            {
                OpcRcw.Da.OPCITEMDEF[] returnitem = new OPCITEMDEF[returnDB.Length];
                for (int i = 0; i < returnDB.Length; i++)
                {
                    returnitem[i].szAccessPath = "";
                    returnitem[i].bActive = 1;
                    returnitem[i].hClient = client;
                    returnitem[i].dwBlobSize = 1;
                    returnitem[i].pBlob = IntPtr.Zero;
                    returnitem[i].vtRequestedDataType = (int)VarEnum.VT_BSTR;
                    returnitem[i].szItemID = string.Format("S7:[S7 connection_1]{0}", returnDB[i]); //状态DB
                    client++;
                }
                if (!opcServer.AddItems(returnitem, returnHandle)) return false;
            }

            if (controlDB != null)
            {
                OpcRcw.Da.OPCITEMDEF[] controlitem = new OPCITEMDEF[controlDB.Length];
                for (int i = 0; i < controlDB.Length; i++)
                {

                    controlitem[i].szAccessPath = "";
                    controlitem[i].bActive = 1;
                    controlitem[i].hClient = client;
                    controlitem[i].dwBlobSize = 1;
                    controlitem[i].pBlob = IntPtr.Zero;
                    controlitem[i].vtRequestedDataType = (int)VarEnum.VT_BSTR;
                    controlitem[i].szItemID = string.Format("S7:[S7 connection_1]{0}", controlDB[i]); //状态DB
                    client++;
                }
                if (!opcServer.AddItems(controlitem, controlHandle)) return false;
            }

            if (statusDB != null)
            {
                OpcRcw.Da.OPCITEMDEF[] statusitem = new OPCITEMDEF[statusDB.Length];
                for (int i = 0; i < statusDB.Length; i++)
                {
                    statusitem[i].szAccessPath = "";
                    statusitem[i].bActive = 1;
                    statusitem[i].hClient = client;
                    statusitem[i].dwBlobSize = 1;
                    statusitem[i].pBlob = IntPtr.Zero;
                    statusitem[i].vtRequestedDataType = (int)VarEnum.VT_BSTR;
                    statusitem[i].szItemID = string.Format("S7:[S7 connection_1]{0}", statusDB[i]); //状态DB
                    client++;
                }

                if (!opcServer.AddItems(statusitem, statusHandle)) return false;
            }

            if (failureDB != null)
            {
                OpcRcw.Da.OPCITEMDEF[] failureitem = new OPCITEMDEF[failureDB.Length];
                for (int i = 0; i < failureDB.Length; i++)
                {
                    failureitem[i].szAccessPath = "";
                    failureitem[i].bActive = 1;
                    failureitem[i].hClient = client;
                    failureitem[i].dwBlobSize = 1;
                    failureitem[i].pBlob = IntPtr.Zero;
                    failureitem[i].vtRequestedDataType = (int)VarEnum.VT_BSTR;

                    failureitem[i].szItemID = string.Format("S7:[S7 connection_1]{0}", failureDB[i]); //状态DB
                    client++;
                }

                if (!opcServer.AddItems(failureitem, failureHandle)) return false;
            }

            isBindToPLC = true;
            return isBindToPLC;
        }

        /// <summary>
        /// 设备状态字典用
        /// </summary>
        public struct DeviceStatus
        {
            /// <summary>
            /// 状态名称
            /// </summary>
            public String statusDesc;
            /// <summary>
            /// 状态性质
            /// </summary>
            public String statusKind;//0为正常，1为故障
            /// <summary>
            /// 状态颜色
            /// </summary>
            public String statusColor;
        }
        /// <summary>
        /// 设备故障字典用
        /// </summary>
        public struct DeviceFailure
        {
            /// <summary>
            /// 故障名称
            /// </summary>
            public String failureDesc;
            /// <summary>
            /// 故障性质
            /// </summary>
            public String failureKind;//0为正常，1为故障
        }
        public struct DeviceControl
        {
            public String controlType;
            public String controlId;
            public String controlDesc;
            public String condition;
        }
    }
}
