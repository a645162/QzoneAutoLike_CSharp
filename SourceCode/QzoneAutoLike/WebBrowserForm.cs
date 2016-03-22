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
    public partial class WebBrowserForm : Form
    {
        public static WebBrowser Wb;
        public WebBrowserForm()
        {
            InitializeComponent();
        }
        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            //多个窗口最好设计好程序的退出代码以免造成残留 -By：孔昊旻
            Application.Exit();
        }
        // 禁止弹出窗口
        private void webBrowser1_NewWindow(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            WebBrowser wb = (WebBrowser)sender;
            string url = wb.Document.ActiveElement.GetAttribute("href");
            webBrowser1.Navigate(url);
        }
        private void Form2_Load(object sender, EventArgs e)
        {
            Wb = webBrowser1;
        }
    }
}