using System;
using OpcRcw.Da;
using OpcRcw.Comn;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace WCS.PlcSystem
{
    public class OPCServerAsyn
    { 
        #region 参数值
        public IOPCServer ServerObj;//OPCServer 
        public IOPCAsyncIO2 IOPCAsyncIO2Obj = null;// 异步读写对象
        public IOPCGroupStateMgt IOPCGroupStateMgtObj = null;// 组管理对象

        public IConnectionPointContainer pIConnectionPointContainer = null;
        public IConnectionPoint pIConnectionPoint = null;

        public const int LOCALE_ID = 0x804;//OPCServer返回文本的语言，0x407为英语实测返回的德语，0x804为中文，实测返回的是英语

        public Object GroupObj = null;// 返回增加的group 
        public int pSvrGroupHandle = 0;
        public Int32 dwCookie = 0;// 此处应该是返回连接的cookie值
        private bool boolValueChang = true;// 该属性用于是否检测plc数据改成，异步订阅

        private bool isConnected = false;
        private bool isAddGroup = false;
        private bool isAddItems = false;
        private bool isDataChange = false;
        private String objName;

        #endregion

        public OPCServerAsyn(string name)
        {
            objName = name;
        }
        public OPCServerAsyn() { }

        #region  建立和OPCServer的连接
        /// <summary>
        /// 建立和OPCServer的连接
        /// </summary>
        public bool Connect() //返回值true为成功，false为失败
        {
            try
            {
                Type svrComponenttyp = Type.GetTypeFromProgID("OPC.SimaticNet", "localhost");
                ServerObj = (IOPCServer)Activator.CreateInstance(svrComponenttyp);
                isConnected = true;
            }
            catch (System.Exception error)
            {
                isConnected = false;
                MessageBox.Show(String.Format("对象{0}建立OPCServer连接失败:-{1}", objName, error.Message),
                    "连接失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return isConnected;
        }
        #endregion

        #region 初始化组对象
        public bool AddGroup(object Form_Main) //返回值false为失败，true为成功
        {
            Int32 dwRequestedUpdateRate = 2000;
            Int32 hClientGroup = 1;
            Int32 pRevUpdaterate;
            float deadband = 0;
            int TimeBias = 0;
            GCHandle hTimeBias, hDeadband;
            hTimeBias = GCHandle.Alloc(TimeBias, GCHandleType.Pinned);
            hDeadband = GCHandle.Alloc(deadband, GCHandleType.Pinned);
            Guid iidRequiredInterface = typeof(IOPCItemMgt).GUID;
            if (!isConnected)
            {
                if (!Connect()) return false; //如果还没有没有建立连接，先建立连接
            }
            try
            {   //ServerObj.AddGroup的返回值类型为void
                ServerObj.AddGroup("OPCGroup", 0,
                         dwRequestedUpdateRate, hClientGroup,
                         hTimeBias.AddrOfPinnedObject(), hDeadband.AddrOfPinnedObject(),
                         LOCALE_ID, out pSvrGroupHandle,
                         out pRevUpdaterate, ref iidRequiredInterface, out GroupObj);
                IOPCAsyncIO2Obj = (IOPCAsyncIO2)GroupObj;//为组异步读写定义句柄
                //Query interface for Async calls on group object  

                IOPCGroupStateMgtObj = (IOPCGroupStateMgt)GroupObj;

                pIConnectionPointContainer = (IConnectionPointContainer)GroupObj;
                //定义特定组的异步调用连接  
                Guid iid = typeof(IOPCDataCallback).GUID;
                // Establish Callback for all async operations  
                pIConnectionPointContainer.FindConnectionPoint(ref iid, out pIConnectionPoint);
                // Creates a connection between the OPC servers's connection point and this client's sink (the callback object)  
                pIConnectionPoint.Advise(Form_Main, out dwCookie);
                isAddGroup = true;
            }
            catch (System.Exception error)
            {
                isAddGroup = false;
                MessageBox.Show(string.Format("创建组对象时出错:-{0}", error.Message), "建组出错",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (hDeadband.IsAllocated) hDeadband.Free();
                if (hTimeBias.IsAllocated) hTimeBias.Free();
            }
            return isAddGroup;
        }

        #endregion

        #region 添加一个读写的Items数组对象
        public bool AddItems(OPCITEMDEF[] items, int[] itemHandle)
        {
            string errText = string.Empty;
            IntPtr pResults = IntPtr.Zero;
            IntPtr pErrors = IntPtr.Zero;
            try
            {
                ((IOPCItemMgt)GroupObj).AddItems(items.Length, items, out  pResults, out pErrors);
                int[] errors = new int[items.Length];
                Marshal.Copy(pErrors, errors, 0, items.Length);
                IntPtr pos = pResults;
                OPCITEMRESULT result;
                for (int i = 0; i < items.Length; i++)
                {
                    if (errors[i] == 0)
                    {
                        result = (OPCITEMRESULT)Marshal.PtrToStructure(pos, typeof(OPCITEMRESULT));
                        itemHandle[i] = result.hServer;
                        pos = new IntPtr(pos.ToInt32() + Marshal.SizeOf(typeof(OPCITEMRESULT)));
                        isAddItems = true;
                    }
                    else
                    {
                        isAddItems = false;
                        MessageBox.Show(string.Format("添加第{0}个Item对象时出错", i + 1), "添加Item对象出错",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    }
                }
            }
            catch (System.Exception ex) // catch for add item  
            {
                isAddItems = false;
                MessageBox.Show(string.Format("添加Item对象时出错:-{0}", ex.Message), "添加Item对象出错",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Free the memory  
                if (pResults != IntPtr.Zero)
                {
                    Marshal.FreeCoTaskMem(pResults);
                    pResults = IntPtr.Zero;
                }
                if (pErrors != IntPtr.Zero)
                {
                    Marshal.FreeCoTaskMem(pErrors);
                    pErrors = IntPtr.Zero;
                }
            }
            return isAddItems;
        }
        #endregion

        #region 读、写plc的值

        public string WirtePlc(object[] values, int[] itemHandle) //由编程人员保证，所写数据和添加Item的数据说明相对应
        {
            string errText = string.Empty;

            int nCancelid;
            IntPtr pErrors = IntPtr.Zero;
            if (IOPCAsyncIO2Obj != null)
            {
                try
                {
                    IOPCAsyncIO2Obj.Write(values.Length, itemHandle,
                        values, values.Length, out nCancelid, out pErrors);

                    int[] errors = new int[values.Length];

                    Marshal.Copy(pErrors, errors, 0, values.Length);
                    if (errors[0] != 0)//Error in reading item
                    {
                        Marshal.FreeCoTaskMem(pErrors);
                        pErrors = IntPtr.Zero;
                        errText = "异步写入数据发生异常！";
                    }
                }

                catch (Exception ex)
                {
                    errText = ex.Message.ToString().Trim();
                }
            }
            return errText;

        }
        
        public string ReadPlc(int[] itemHandle)
        {
            string errText = string.Empty;

            int nCancelid;

            IntPtr pErrors = IntPtr.Zero;

            if (IOPCAsyncIO2Obj != null)
            {

                try
                {
                    IOPCAsyncIO2Obj.Read(itemHandle.Length, itemHandle,
                        itemHandle.Length, out nCancelid, out pErrors);

                    int[] errors = new int[itemHandle.Length];

                    Marshal.Copy(pErrors, errors, 0, itemHandle.Length);
                    return errText;

                }

                catch (Exception ex)
                {

                    errText = ex.Message.ToString().Trim();

                }

            }
            return errText;

        }

        #endregion

        #region  检测plc数据改变时，触发事件
        public bool DataChange()
        {
            IntPtr pRequestedUpdateRate = IntPtr.Zero;
            int nRevUpdateRate = 0;
            IntPtr hClientGroup = IntPtr.Zero;
            IntPtr pTimeBias = IntPtr.Zero;
            IntPtr pDeadband = IntPtr.Zero;
            IntPtr pLCID = IntPtr.Zero;
            int nActive = 0;
            // activates or deactivates group according to checkbox status  
            GCHandle hActive = GCHandle.Alloc(nActive, GCHandleType.Pinned);
            if (boolValueChang)
            {
                hActive.Target = 1;
            }
            else
            {
                hActive.Target = 0;
            }
            try
            {
                IOPCGroupStateMgtObj.SetState(pRequestedUpdateRate, out nRevUpdateRate,
                           hActive.AddrOfPinnedObject(), pTimeBias, pDeadband, pLCID,
                           hClientGroup);
                isDataChange = true;
            }
            catch (System.Exception ex)
            {
                isDataChange = false;
                MessageBox.Show(string.Format("添加plc数据改变触发事件时出错:-{0}", ex.Message), "添加plc触发事件出错",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                hActive.Free();
            }
            return isDataChange;
        }
        #endregion

        #region  断开PLC的连接
        public string DisConnection()
        {
            string errText = string.Empty;
            try
            {
                if (dwCookie != 0)
                {
                    pIConnectionPoint.Unadvise(dwCookie);
                    dwCookie = 0;
                }
                // Free unmanaged code  
                Marshal.ReleaseComObject(pIConnectionPoint);
                pIConnectionPoint = null;

                Marshal.ReleaseComObject(pIConnectionPointContainer);
                pIConnectionPointContainer = null;

                if (IOPCAsyncIO2Obj != null)
                {
                    Marshal.ReleaseComObject(IOPCAsyncIO2Obj);
                    IOPCAsyncIO2Obj = null;
                }
                
                ServerObj.RemoveGroup(pSvrGroupHandle, 0);
                if (IOPCGroupStateMgtObj != null)
                {
                    Marshal.ReleaseComObject(IOPCGroupStateMgtObj);
                    IOPCGroupStateMgtObj = null;
                }
                if (GroupObj != null)
                {
                    Marshal.ReleaseComObject(GroupObj);
                    GroupObj = null;
                }
                if (ServerObj != null)
                {
                    Marshal.ReleaseComObject(ServerObj);
                    ServerObj = null;
                }
            }
            catch (System.Exception error)
            {
                errText = error.Message.ToString().Trim();

            }
            return errText;

        }
        #endregion

    }
}
