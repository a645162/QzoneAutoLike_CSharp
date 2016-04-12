using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
namespace QzoneAutoLike
{
    public partial class ControlForm : Form
    {
        public static bool haveNewVersion = false;
        public static int newVersionHeight;
        Form _we;
        public ControlForm()
        {
            InitializeComponent();
        }
        public static string remoteVersion = "1.0.0.0000";
        private void Form1_Load(object sender, EventArgs e)
        {
            Rectangle rect = new Rectangle();
            rect = Screen.GetWorkingArea(this);
            this.Left = rect.Left;
            this.Top = rect.Top;
            //保证在工作区左上角，以免当任务栏不在底部时无法操作控制按钮

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
        public Thread t1;
        public void checkUpdate()
        {
            ControlForm.CheckForIllegalCrossThreadCalls = false;
            System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = false;
            label6.Text = "当前版本:" + Program.localVersion + "\n\n→检查更新中←";
            label6.ForeColor = Color.Blue;
            try
            {
                remoteVersion = GetHtmlCode.GetWebClient(Program.githubUrl + "raw/master/SourceCode/QzoneAutoLike/version.inf");
            }
            catch (Exception e)
            {
                label6.Text = "当前版本:" + Program.localVersion + "\n\n→检查失败←";
                label6.ForeColor = Color.Black;
                label6.Cursor = Cursors.Hand;
                MessageBox.Show(this,"检查失败！\n" + e.Message.ToString(), "检查更新错误",MessageBoxButtons.OK,MessageBoxIcon.Error);
                t1.Abort();
            }
            int lV, rV;
            lV = int.Parse(deleteAllDotString(Program.localVersion));
            rV = int.Parse(deleteAllDotString(remoteVersion));
            string aboutText;
            if (lV < rV)
            {
                newHeight(newVersionHeight);
                aboutText = "发现新版本";
                label6.ForeColor = Color.Red;
            }
            else
            {
                newHeight(553);
                aboutText = "服务器没有更高的版本";
                label6.ForeColor = Color.Green;
            }
            label6.Text = "当前版本:" + Program.localVersion + "\n远程版本:" + remoteVersion + "\n" + aboutText;
            t1.Abort();
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
        private void label6_Click(object sender, EventArgs e)
        {
            if (label6.Cursor == Cursors.Hand)
            {

                t1 = new Thread(new ThreadStart(checkUpdate));
                t1.Start();
                label6.Cursor = Cursors.No;
            }
        }
    }
}