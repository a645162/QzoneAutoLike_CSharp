using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
namespace QzoneAutoLike
{
    public partial class ControlForm : Form
    {
        public static bool haveNewVersion = false;
        private static int newVersionHeight;
        Form _we;
        public ControlForm()
        {
            InitializeComponent();
        }
        public string remoteVersion = "1.0.0.0000";
        private void Form1_Load(object sender, EventArgs e)
        {
            newVersionHeight = this.Height;
            label6.Text = "当前版本:" + Program.localVersion + "\n\n→点我检查更新←";
            newHeight(553);
            linkLabel1.Text = "代码仓库(Github)：" + Program.githubUrl;
            _we = new WebBrowserForm();
            _we.Show();
            timer_AutoLike.Enabled = true;
            timer_AutoRefresh.Enabled = true;
            timer_AutoLike.Enabled = true;
            timer_AutoRefresh.Start();
            timer_AutoLoad.Start();
        }
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            //多个窗口最好设计好程序的退出代码以免造成残留 -By：孔昊旻
            //System.Environment.Exit(0);
            Application.Exit();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            radioButton1.Checked = true;
            radioButton2.Checked = false;
            radioButton3.Checked = false;
            WebBrowserForm.Wb.Navigate("http://qzone.qq.com");
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (radioButton2.Checked == true)
            {
                runJQuery(Properties.Resources.like);
            }
            else if (radioButton3.Checked == true)
            {
                runJQuery(Properties.Resources.cancellike);
            }
        }
        public void runJQuery(string JQuerySourceCode)
        {
            HtmlElement ele = WebBrowserForm.Wb.Document.CreateElement("script");
            ele.SetAttribute("type", "text/javascript");
            ele.SetAttribute("text", JQuerySourceCode);
            WebBrowserForm.Wb.Document.Body.AppendChild(ele);
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            _we.Visible = checkBox4.Checked;
        }
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //https://github.com/a645162/QzoneAutoLike_CSharp
            //调用系统默认的浏览器  
            System.Diagnostics.Process.Start(Program.githubUrl);
        }
        private void button3_Click(object sender, EventArgs e)
        {
            runJQuery(Properties.Resources.autol);
            MessageBox.Show(this, "成功写入自动点赞脚本！", "QQ空间自动赞", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void button4_Click(object sender, EventArgs e)
        {
            runJQuery(Properties.Resources.autoc);
            MessageBox.Show(this, "成功写入自动取消赞脚本！", "QQ空间自动赞", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void button5_Click(object sender, EventArgs e)
        {
            GetMoreMessage(WebBrowserForm.Wb);
        }
        public void GetMoreMessage(WebBrowser wb)
        {
            wb.Document.Window.ScrollTo(0, wb.Document.Body.ScrollRectangle.Height);
        }
        private void timer_AutoRefresh_Tick(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
                WebBrowserForm.Wb.Refresh();
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            textBox1.Text = DeleteTheOtherThanDigitalContent(textBox1.Text);
            timer_AutoLike.Interval = int.Parse(textBox1.Text);
        }
        public string DeleteTheOtherThanDigitalContent(string text)
        {
            string temp = "";
            char[] tempArray = new char[temp.Length], Standard = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            tempArray = text.ToCharArray();
            for (int i = 0; i < tempArray.Length; i++)
            {
                if (Char.IsNumber(tempArray[i]))
                {
                    temp += tempArray[i];
                }
            }
            if (temp.Equals(""))
            {
                temp = "1000";
            }
            if (temp.Equals("0"))
            {
                temp = "1000";
            }
            return temp;
        }
        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            textBox3.Text = DeleteTheOtherThanDigitalContent(textBox3.Text);
            timer_AutoRefresh.Interval = int.Parse(textBox3.Text);
        }
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            textBox2.Text = DeleteTheOtherThanDigitalContent(textBox2.Text);
            timer_AutoLoad.Interval = int.Parse(textBox2.Text);
        }
        private void timer_AutoLoad_Tick(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
                button5_Click(sender, e);
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.khmapp.xyz/");
        }

        private void checkUpdate()
        {
            remoteVersion = GetHtmlCode.GetWebClient(Program.githubUrl + "raw/master/SourceCode/QzoneAutoLike/version.inf");
            int lV, rV;
            lV = int.Parse(deleteAllDotString(Program.localVersion));
            rV = int.Parse(deleteAllDotString(remoteVersion));
            string aboutText;
            if (lV < rV)
            {
                newHeight(newVersionHeight);
                aboutText = "发现新版本";
            }
            else
            {
                newHeight(553);
                aboutText = "服务器没有更高的版本";
            }
            label6.Text = "当前版本:" + Program.localVersion + "\n远程版本:" + remoteVersion + "\n" + aboutText;
        }

        private void newHeight(int height)
        {
            if (this.Height != height)
                this.Height = height;
        }

        private string deleteAllDotString(string source)
        {
            string ret = source;
            while (ret.IndexOf(".") != -1)
            {
                ret = ret.Replace(".", "");
            }
            return ret;
        }

        /// <summary>        
        /// c#,.net 下载文件        
        /// </summary>        
        /// <param name="URL">下载文件地址</param>       
        /// 
        /// <param name="Filename">下载后的存放地址</param>        
        /// <param name="Prog">用于显示的进度条</param>        
        /// 
        public void DownloadFile(string URL, string filename, System.Windows.Forms.ProgressBar prog, System.Windows.Forms.Label label1)
        {
            float percent = 0;
            try
            {
                System.Net.HttpWebRequest Myrq = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(URL);
                System.Net.HttpWebResponse myrp = (System.Net.HttpWebResponse)Myrq.GetResponse();
                long totalBytes = myrp.ContentLength;
                if (prog != null)
                {
                    prog.Maximum = (int)totalBytes;
                }
                System.IO.Stream st = myrp.GetResponseStream();
                System.IO.Stream so = new System.IO.FileStream(filename, System.IO.FileMode.Create);
                long totalDownloadedByte = 0;
                byte[] by = new byte[1024];
                int osize = st.Read(by, 0, (int)by.Length);
                while (osize > 0)
                {
                    totalDownloadedByte = osize + totalDownloadedByte;
                    System.Windows.Forms.Application.DoEvents();
                    so.Write(by, 0, osize);
                    if (prog != null)
                    {
                        prog.Value = (int)totalDownloadedByte;
                    }
                    osize = st.Read(by, 0, (int)by.Length);

                    percent = (float)totalDownloadedByte / (float)totalBytes * 100;
                    label1.Text = "当前补丁下载进度" + percent.ToString() + "%";
                    System.Windows.Forms.Application.DoEvents(); //必须加注这句代码，否则label1将因为循环执行太快而来不及显示信息
                }
                so.Close();
                st.Close();
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            DialogResult dR_mb = MessageBox.Show(this, "您是否要使用程序自动升级？\n[是(Y)] 使用自动升级 [否(N)]进入Github手动下载 [取消]取消操作"
                , "程序更新", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Asterisk);
            switch (dR_mb)
            {
                case DialogResult.Yes:
                    new UpdateForm().Show();
                    break;
                case DialogResult.No:
                    linkLabel1_LinkClicked(null, null);
                    break;
                case DialogResult.Cancel:
                    break;
                default:
                    break;
            }

        }
        private bool checkedUpdate = false;

        private void label6_Click(object sender, EventArgs e)
        {
            if (!checkedUpdate)
            {
                checkUpdate();
                checkedUpdate = true;
            }
        }
    }
}