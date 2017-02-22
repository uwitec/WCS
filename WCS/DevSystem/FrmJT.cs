using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WCS.DevSystem
{
    public partial class FrmJT : Form
    {
        private JT_Dev jt_dev;
        private int tag;

        private string strCommand;
        public FrmJT(JT_Dev jt_dev, int tag)
        {
            InitializeComponent();
            this.jt_dev = jt_dev;
            this.tag = tag;
            this.Text = jt_dev.deviceName[tag];
            RefrushStatus(tag);
        }

        private void RefrushStatus(int tag)
        {
            if (jt_dev.mainFrm.deviceStatusDic.ContainsKey(jt_dev.statusType[tag] + jt_dev.deviceStatus[tag].ToString()) == false)
            {

                lblDeviceInfo.Text = "未知错误，" + jt_dev.deviceStatus[tag].ToString();
            }
            else
            {
                lblDeviceInfo.Text = jt_dev.mainFrm.deviceStatusDic[jt_dev.statusType[tag] + jt_dev.deviceStatus[tag].ToString()].statusDesc;

            }
        }

        private void btnClose_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
