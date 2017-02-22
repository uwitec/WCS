using System;
using OpcRcw.Da;
using System.Runtime.InteropServices;
using WCS.DevSystem;
using System.Threading;

namespace WCS.PlcSystem
{
    public class PlcSystemMS
    {
        public New_Main_Form mainFrm;
        private OPCServer opcServer = new OPCServer();  //OPCServer对象
        //DBB0	PLC心跳        DB304
        //DBB1	一楼上位机心跳
        //DBB2	二楼上位机心跳
        private string plcHeartDB = "DB304,W0";//PLC心跳 DBW0	PLC上电运行时间
        private string wcsHeartDB = "DB304,W2";//DBW2	IT上电运行时间
        //private string pickModeDB = "DB302,W8";//“0”PLC控制模式，“1”WCS控制模式
        //PLC定义号	功能定义	备注
        //BDX6.0	系统运行	 “1”系统运行，“0”系统停止
        //BDX6.1	系统故障	“1”有效
        //BDX6.2	启动预警	“1”有效
        //BDX6.3	自动方式	“1”有效
        private string plcStatusDB = "DB304,B6";//分拣线状态字
        //private string weightModeDB = "DB302,W10";//

        private int[] plcHeartHandle = new int[1];
        private int[] wcsHeartHandle = new int[1];
        //private int[] pickModeHandle = new int[1];
        private int[] plcStatusHandle = new int[1];
        //private int[] weightModeHandle = new int[1];

        private String[] readValues = new String[1];      //从OPCServer中读出的数据
        private bool isBindToPLC = false;  //是否已初始化OPCServer

        private int plcHeartValue = 0;//plc心跳值在1至99之间
        private int plcHeartValueOld = 999;//上次检测到的PLC心跳值
        private int wcsHeartValue = 0;//wcs心跳值在1至99之间

        public bool plcStatus = false; //PLC的总运行状态

        private int plcStatusValue = 0;
        public bool is_auto;//03	字节4.位3 dbx4.3	分拣线自动运行
        //private string PickModeReadValue = "False";//
        //private string PickModeWriteValue = "False";//
        //private string weightModeValue = string.Empty;

        public PlcSystemMS(New_Main_Form mainFrm)
        {
            this.mainFrm = mainFrm;
        }

        public bool RefreshStatus()
        {
            if (!isBindToPLC)
                if (!BindToPLC()) return false;
            //1 读取PLC的分拣线状态字
            if (!opcServer.SyncRead(readValues, plcStatusHandle)) return false;
            plcStatusValue = Convert.ToInt32(readValues[0]);
            //01	字节4.位1 dbx4.1	分拣线正在分拣
            //03	字节4.位3 dbx4.3	分拣线自动运行
            //04	字节4.位4 dbx4.4	分拣线急停

            //if ((error4[i] & 0x04) != 0)  //4.2位
            //    errorText += mainFrm.pickerStatusDic["WSJ42"] + ";";
            if ((plcStatusValue & 0x08) != 0)  //03	字节4.位3 dbx4.3	分拣线自动运行
            {
                is_auto = true;
            }
            else
            {
                is_auto = false;
            }

            //读取PLC的心跳，判定PLC与WCS的连接状态
            if (!opcServer.SyncRead(readValues, plcHeartHandle)) return false;
            plcHeartValue = Convert.ToInt32(readValues[0]);
            if (plcHeartValue == plcHeartValueOld)
            {
                plcStatus = false;
            }
            else
            {
                plcStatus = true;
                plcHeartValueOld = plcHeartValue;
            }
            //写入上位机心跳
            if (plcStatus == true)
            {
                wcsHeartValue = wcsHeartValue + 1;
                if (wcsHeartValue >= 100)
                {
                    wcsHeartValue = 1;
                }
                object[] writeValues = new object[1];
                writeValues[0] = wcsHeartValue;
                opcServer.SyncWrite(writeValues, wcsHeartHandle);
            }

            return plcStatus;
        }

        public bool BindToPLC()
        {
            if (!opcServer.Connect()) return false;
            if (!opcServer.AddGroup()) return false;

            OpcRcw.Da.OPCITEMDEF[] Items = new OPCITEMDEF[1];
            Items[0].szAccessPath = "";
            Items[0].bActive = 1;
            Items[0].hClient = 1;
            Items[0].dwBlobSize = 1;
            Items[0].pBlob = IntPtr.Zero;
            Items[0].vtRequestedDataType = (int)VarEnum.VT_BSTR;
            Items[0].szItemID = string.Format("S7:[S7 connection_1]{0}", plcHeartDB); //PLC心跳
            if (!opcServer.AddItems(Items, plcHeartHandle)) return false;

            Items[0].hClient = 2;
            Items[0].vtRequestedDataType = (int)VarEnum.VT_BSTR;
            Items[0].szItemID = string.Format("S7:[S7 connection_1]{0}", wcsHeartDB); //上位机心跳
            if (!opcServer.AddItems(Items, wcsHeartHandle)) return false;

            ////pickMode
            //Items[0].hClient = 3;
            //Items[0].vtRequestedDataType = (int)VarEnum.VT_BSTR;
            //Items[0].szItemID = string.Format("S7:[S7 connection_1]{0}", pickModeDB);
            //if (!opcServer.AddItems(Items, pickModeHandle)) return false;

            Items[0].hClient = 3;
            Items[0].vtRequestedDataType = (int)VarEnum.VT_BSTR;
            Items[0].szItemID = string.Format("S7:[S7 connection_1]{0}", plcStatusDB); //分拣线状态字
            if (!opcServer.AddItems(Items, plcStatusHandle)) return false;

            isBindToPLC = true;
            return isBindToPLC;
        }
    }
}