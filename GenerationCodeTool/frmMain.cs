using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace GenerationCodeTool
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            rtbStatus.Text = "Ready.";
        }

        /// <summary>
        /// 执行代码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExec_Click(object sender, EventArgs e)
        {
            MessageBox.Show(comboFWName.Text);
        }

        private void comboFWName_SelectedIndexChanged(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.CheckPathExists)
            {
                return;
            }
            else
            {
                //新建
                
            }
        }
    }
}
