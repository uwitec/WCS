using System;
using OpcRcw.Da;
using OpcRcw.Comn;
using System.Windows.Forms;
using System.Runtime.InteropServices;


namespace WCS.PlcSystem
{
   public class OPCServer
    {
        private OpcRcw.Da.IOPCServer ServerObj;//OPCServer对象
        private Object GroupObj = null; //OPC组对象，创建组对象时产生，添加Item时用
        private int pSvrGroupHandle = 0; //组的句柄，创建组对象时产生，释放内存时用
        private OpcRcw.Da.IOPCSyncIO IOPCSyncObj = null; //同步读写对象的句柄，创建组对象时产生，执行同步读写时用
        private const int LOCALE_ID = 0x804;//OPCServer返回文本的语言，0x407为英语实测返回的德语，0x804为中文，实测返回的是英语

        private bool isConnected = false;
        private bool isAddGroup = false;
        private bool isAddItems = false;
        private String objName;

        public OPCServer(string name)
        {
            objName = name;
        }
        public OPCServer() { }
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
        /// <summary>
        /// 初始化组对象
        /// </summary>
        public bool AddGroup() //返回值false为失败，true为成功
        {
            Int32 dwRequestedUpdateRate = 1000;
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
                IOPCSyncObj = (IOPCSyncIO)GroupObj;//为组同步读写定义句柄
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

        /// <summary>
        /// 初始化组对象
        /// </summary>
        public bool AddGroup(Int32 dwRequestedUpdateRate) //返回值false为失败，true为成功
        {
            //Int32 dwRequestedUpdateRate = 1000;
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
                IOPCSyncObj = (IOPCSyncIO)GroupObj;//为组同步读写定义句柄
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

        /// </summary>
        /// <param name="items">添加读写的数据项，Items为读写对象数组</param>
        /// <returns>添加Items是否执行成功</returns>
        public bool AddItems(OPCITEMDEF[] items, int[] itemHandle)
        {
            string errText = string.Empty;
            IntPtr pResults = IntPtr.Zero;
            IntPtr pErrors = IntPtr.Zero;
            if (!isAddGroup)
            {
                if (!AddGroup()) return false;  //如果还没有没有添加组，先添加组
            }
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
                        MessageBox.Show(string.Format("添加第{0}个Item对象时出错", i + 1), "添加Item对象出错" + items.Length.ToString(),
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    }
                }
            }
            catch (System.Exception ex) // catch for add item  
            {
                isAddItems = false;
                MessageBox.Show(string.Format("添加Item对象时出错:-{0}", ex.Message), "添加Item对象出错" + items.Length.ToString(),
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
        /// <summary>
        /// 断开连接，调用需要捕捉异常
        /// </summary>
        public void DisConnect()   //断开和OPCServer的连接，并释放内存
        {
            try
            {
                isConnected = false;
                isAddGroup = false;
                isAddItems = false;
                if (IOPCSyncObj != null)
                {
                    Marshal.ReleaseComObject(IOPCSyncObj);
                    IOPCSyncObj = null;
                }
                ServerObj.RemoveGroup(pSvrGroupHandle, 0);
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
                MessageBox.Show(error.Message, "断开OPCServer连接出错", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        public bool SyncWrite(object[] values, int[] itemHandle) //由编程人员保证，所写数据和添加Item的数据说明相对应
        {
            IntPtr pErrors = IntPtr.Zero;
            bool isWrited = false;
            try
            {
                if (values.Length != itemHandle.Length)
                {
                    MessageBox.Show(string.Format("写入数据的个数与添加Item的数据说明长度不一致"), "写数据出错",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                IOPCSyncObj.Write(values.Length, itemHandle, values, out pErrors);
                int[] errors = new int[values.Length];
                Marshal.Copy(pErrors, errors, 0, values.Length);
                for (int i = 0; i < values.Length; i++)
                {
                    if (errors[i] != 0)  //写数据不成功
                    {
                        String pstrError;   //需不需要释放？
                        ServerObj.GetErrorString(errors[i], LOCALE_ID, out pstrError);
                        MessageBox.Show(string.Format("写入第{0}个数据时出错:{1}", i, pstrError), "写数据出错",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        isWrited = false;
                        break;
                    }
                    else
                    {
                        isWrited = true;
                    }
                }
            }
            catch (System.Exception error)
            {
                isWrited = false;
                MessageBox.Show(error.Message, "写数据出错", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (pErrors != IntPtr.Zero)
                {
                    Marshal.FreeCoTaskMem(pErrors);
                    pErrors = IntPtr.Zero;
                }
            }

            return isWrited;
        }

        public bool SyncRead(object[] values, int[] itemHandle) //同步读，读的结果存放在values中，读成功返回true，失败返回false
        {
            IntPtr pItemValues = IntPtr.Zero;
            IntPtr pErrors = IntPtr.Zero;
            bool isRead = false;
            try
            {
                if (values.Length != itemHandle.Length)
                {
                    MessageBox.Show(string.Format("需要读出数据的个数与添加Item的数据说明长度不一致"), "读数据出错",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                IOPCSyncObj.Read(OPCDATASOURCE.OPC_DS_DEVICE, itemHandle.Length, itemHandle, out pItemValues, out pErrors);
                int[] errors = new int[itemHandle.Length];
                Marshal.Copy(pErrors, errors, 0, itemHandle.Length);
                OPCITEMSTATE pItemState = new OPCITEMSTATE();
                for (int i = 0; i < itemHandle.Length; i++)
                {
                    if (errors[i] == 0)
                    {
                        pItemState = (OPCITEMSTATE)Marshal.PtrToStructure(pItemValues, typeof(OPCITEMSTATE));
                        values[i] = pItemState.vDataValue.ToString();   //pItemState中还包含质量和时间等信息，目前只使用了读取的数据值
                        //                        values[i] = String.Format("{0}", pItemState.vDataValue);   //pItemState中还包含质量和时间等信息，目前只使用了读取的数据值
                        pItemValues = new IntPtr(pItemValues.ToInt32() + Marshal.SizeOf(typeof(OPCITEMSTATE)));
                        isRead = true;
                    }
                    else
                    {
                        String pstrError;   //需不需要释放？
                        ServerObj.GetErrorString(errors[i], LOCALE_ID, out pstrError);
                        MessageBox.Show(string.Format("读出第{0}个数据时出错:{1}", i, pstrError), "读数据出错",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        isRead = false;
                        break;
                    }
                }
            }
            catch (System.Exception error)
            {
                isRead = false;
                MessageBox.Show(error.Message, "Result-Read Items", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                if (pItemValues != IntPtr.Zero)
                {
                    Marshal.FreeCoTaskMem(pItemValues);
                    pItemValues = IntPtr.Zero;
                }
                if (pErrors != IntPtr.Zero)
                {
                    Marshal.FreeCoTaskMem(pErrors);
                    pErrors = IntPtr.Zero;
                }
                
            }
            return isRead;
        }

        private String GetQuality(long wQuality) //判断通讯质量，目前没有使用
        {
            String strQuality = "";
            switch (wQuality)
            {
                case Qualities.OPC_QUALITY_GOOD:
                    strQuality = "Good";
                    break;
                case Qualities.OPC_QUALITY_BAD:
                    strQuality = "Bad";
                    break;
                case Qualities.OPC_QUALITY_CONFIG_ERROR:
                    strQuality = "BadConfigurationError";
                    break;
                case Qualities.OPC_QUALITY_NOT_CONNECTED:
                    strQuality = "BadNotConnected";
                    break;
                case Qualities.OPC_QUALITY_DEVICE_FAILURE:
                    strQuality = "BadDeviceFailure";
                    break;
                case Qualities.OPC_QUALITY_SENSOR_FAILURE:
                    strQuality = "BadSensorFailure";
                    break;
                case Qualities.OPC_QUALITY_COMM_FAILURE:
                    strQuality = "BadCommFailure";
                    break;
                case Qualities.OPC_QUALITY_OUT_OF_SERVICE:
                    strQuality = "BadOutOfService";
                    break;
                case Qualities.OPC_QUALITY_WAITING_FOR_INITIAL_DATA:
                    strQuality = "BadWaitingForInitialData";
                    break;
                case Qualities.OPC_QUALITY_EGU_EXCEEDED:
                    strQuality = "UncertainEGUExceeded";
                    break;
                case Qualities.OPC_QUALITY_SUB_NORMAL:
                    strQuality = "UncertainSubNormal";
                    break;
                default:
                    strQuality = "Not handled";
                    break;
            }

            return strQuality;
        }
        private DateTime ToDateTime(OpcRcw.Da.FILETIME ft) //对OPCServer读出数据时间标签的计量形式转换，目前没有使用。
        {
            long highbuf = (long)ft.dwHighDateTime;
            long buffer = (highbuf << 32) + ft.dwLowDateTime + 8 * 36000000000L;
            return DateTime.FromFileTimeUtc(buffer);
        }
    }
}
