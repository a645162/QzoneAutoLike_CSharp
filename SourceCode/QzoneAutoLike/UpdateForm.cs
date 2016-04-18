using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace QzoneAutoLike
{
    public partial class UpdateForm : Form
    {
        public UpdateForm()
        {
            InitializeComponent();
        }

        private string fileUrl = null;

        private void button1_Click(object sender, EventArgs e)
        {
            if (button1.Text == "开始更新")
            {
                button1.Enabled = false;
                button1.Text = "更新中";
                label4.Text = "即将开始，并创建更新线程";
                progressBar1.Value = 10;
                t = new Thread(new ThreadStart(updateProgress));
                t.Start();
            }
            else if (button1.Text == "完成")
            {

            }
        }

        private Thread t;

        private void updateProgress()
        {
            Thread.Sleep(1000);
            Control.CheckForIllegalCrossThreadCalls = false;
            radioButton1.Enabled = radioButton2.Enabled = false;
            progressBar1.Value = 20;
            string dotNet = "35";
            if (radioButton1.Checked) dotNet = "45";
            fileUrl = Program.githubUrl + "blob/master/Binary/QzoneAutoLike" + dotNet + ".exe?raw=true";
            label4.Text = "准备任务完成，即将开始下载";
            Thread.Sleep(2000);
            
            DownloadFile(fileUrl, Program.tempPath, progressBar2, label4);
            if (progressBar2.Value == 100)
            {
                progressBar1.Value = 60;
            }
            Thread.Sleep(1000);


            button1.Text = "完成";
            button1.Enabled = true;
            t.Abort();
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

        private void UpdateForm_Load(object sender, EventArgs e)
        {
            radioButton1.Checked = Program.dotNetVersion != "35";
            radioButton2.Checked = !radioButton1.Checked;
        }
    }
}
