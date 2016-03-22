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
        Form _we;
        public ControlForm()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
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
        private void button2_Click(object sender, EventArgs e)
        {
            timer_AutoLike.Interval = int.Parse(textBox1.Text);
        }
        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            _we.Visible = checkBox4.Checked;
        }
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //https://github.com/a645162/qzoneautolike
            //调用系统默认的浏览器  
            System.Diagnostics.Process.Start("https://github.com/a645162/qzoneautolike");
        }
        private void button3_Click(object sender, EventArgs e)
        {
            runJQuery(Properties.Resources.autol);
        }
        private void button4_Click(object sender, EventArgs e)
        {
            runJQuery(Properties.Resources.autoc);
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
        }
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            textBox2.Text = DeleteTheOtherThanDigitalContent(textBox2.Text);
        }
        private void timer_AutoLoad_Tick(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
                button5_Click(sender, e);
        }
        private void button6_Click(object sender, EventArgs e)
        {
            timer_AutoRefresh.Interval = int.Parse(textBox3.Text);
        }
        private void button7_Click(object sender, EventArgs e)
        {
            timer_AutoLoad.Interval = int.Parse(textBox2.Text);
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.khmapp.xyz/"); 
        }
    }
}