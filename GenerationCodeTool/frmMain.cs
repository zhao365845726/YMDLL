using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using ML.Utilities;

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
            string strModuleName = txtModuleName.Text;
            if(strModuleName == "")
            {
                MessageBox.Show("模块名称不能为空");
                return;
            }
            AutomaticGenerationOfCode(strModuleName);
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

        /// <summary>
        /// 自动生成代码
        /// </summary>
        /// <returns></returns>
        public bool AutomaticGenerationOfCode(string ModuleName)
        {
            string strRootDir = ConfigurationSettings.AppSettings["RootPath"].ToString();
            string strFolderSplit = "\\";
            List<string> lst_Module = new List<string>();
            lst_Module.Add("Business");
            lst_Module.Add("IBusiness");
            lst_Module.Add("IRepository");
            lst_Module.Add("Repository");
            lst_Module.Add("Entity");
            lst_Module.Add("Controller");
            lst_Module.Add("View");
            bool isExist = DirFileHelper.IsExistDirectory(strRootDir);
            if (isExist)
            {
                foreach(string module in lst_Module)
                {
                    string strTempModuleName = string.Empty;
                    string strFileSuffix = string.Empty;
                    string strFileName = string.Empty;
                    if(module != "View")
                    {
                        strTempModuleName = ModuleName.ToLower();
                        strFileSuffix = ".cs";
                        strFileName = ModuleName + module;
                    }
                    else
                    {
                        strTempModuleName = ModuleName;
                        strFileSuffix = ".cshtml";
                        strFileName = "Index" + module;
                    }

                    if (CreateFolder(strRootDir + strFolderSplit + module + strFolderSplit + strTempModuleName + strFolderSplit))
                    {
                        CreateDocument(strRootDir + strFolderSplit + module + strFolderSplit + strTempModuleName + strFileName + strFileSuffix);
                    }
                    else
                    {
                        CreateDocument(strRootDir + strFolderSplit + module + strFolderSplit + strTempModuleName + strFolderSplit + strFileName + strFileSuffix);
                    }
                }
                //CreateFolder(strRootDir + "\\Business\\proposal");
                //CreateFolder(strRootDir + "\\IBusiness\\proposal");
                //CreateFolder(strRootDir + "\\IRepository\\proposal");
                //CreateFolder(strRootDir + "\\Repository\\proposal");
                //CreateFolder(strRootDir + "\\Entity\\proposal");
                //CreateFolder(strRootDir + "\\Controller\\proposal");
                //CreateFolder(strRootDir + "\\View\\Proposal");

                //CreateDocument(strRootDir + "\\Business\\proposal\\ProposalBusiness.cs");
                //CreateDocument(strRootDir + "\\IBusiness\\proposal\\ProposalIBusiness.cs");
                //CreateDocument(strRootDir + "\\IRepository\\proposal\\ProposalIRepository.cs");
                //CreateDocument(strRootDir + "\\Repository\\proposal\\ProposalRepository.cs");
                //CreateDocument(strRootDir + "\\Entity\\proposal\\ProposalEntity.cs");
                //CreateDocument(strRootDir + "\\Controller\\proposal\\ProposalController.cs");
                //CreateDocument(strRootDir + "\\View\\Proposal\\Index.cshtml");
            }
            else
            {
                MessageBox.Show("根目录：" + strRootDir + " 不存在");
            }
            return true;
        }

        /// <summary>
        /// 创建文件夹
        /// </summary>
        /// <param name="FolderPath"></param>
        /// <returns></returns>
        public bool CreateFolder(string FolderPath)
        {
            if (DirFileHelper.IsExistDirectory(FolderPath))
            {
                return false;
            }
            else
            {
                DirFileHelper.CreateDirectory(FolderPath);
                return true;
            }
        }

        /// <summary>
        /// 创建文件
        /// </summary>
        /// <param name="FilePath"></param>
        /// <returns></returns>
        public bool CreateDocument(string FilePath)
        {
            if (DirFileHelper.IsExistFile(FilePath))
            {
                return false;
            }
            else
            {
                DirFileHelper.CreateFile(FilePath);
                return true;
            }
        }
    }
}
