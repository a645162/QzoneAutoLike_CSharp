using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Security.Policy;
using System.Windows.Forms;

namespace QzoneAutoLike
{
    static class Program
    {
        public static string localVersion;
        public const string dotNetVersion = "45";
        public const string githubUrl = @"https://github.com/a645162/QzoneAutoLike_CSharp/";
        public static string tempPath = "";
        public static string Path = System.Windows.Forms.Application.ExecutablePath;
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            localVersion = Application.ProductVersion;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            tempPath = new DirectoryInfo(System.Environment.GetEnvironmentVariable("TEMP")).FullName + "\\qzoneAutoLike_update.exe";
            if (args.Length != 0)
            {
                if (args[0] == "/u")
                {
                    string epath = args[1];
                    if (!File.Exists(tempPath) || !File.Exists(epath))
                    {
                        MessageBox.Show("程序内部错误！\n找不到指定文件！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    System.Diagnostics.Process[] processList = System.Diagnostics.Process.GetProcesses();
                    foreach (System.Diagnostics.Process process in processList)
                    {
                        if (process.ProcessName.ToUpper() == getFilenameFromPath(epath).ToUpper())
                        {
                            process.Kill();
                        }
                    }
                    File.Delete(epath);
                    File.Copy(tempPath, epath);
                    System.Diagnostics.Process process1 = new System.Diagnostics.Process();
                    process1.StartInfo.FileName = epath;
                    process1.StartInfo.Arguments = "/d";
                    process1.Start();
                }
                else if (args[0] == "/d")
                {
                    System.Diagnostics.Process[] processList = System.Diagnostics.Process.GetProcesses();
                    foreach (System.Diagnostics.Process process in processList)
                    {
                        if (process.ProcessName.ToUpper() == getFilenameFromPath(tempPath).ToUpper())
                        {
                            process.Kill();
                        }
                    }
                    File.Delete(tempPath);
                    MessageBox.Show("更新成功！\n即将显示主窗口！", "空间自动点赞器");
                }
                else
                {
                    MessageBox.Show("运行参数错误！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Application.Exit();
                }
            }
            Application.Run(new ControlForm());
        }
        public static string getFilenameFromPath(string path)
        {
            string filename = path;
            filename = filename.Substring(filename.LastIndexOf('\\') + 1);
            return filename;
        }
    }
    public static class _Url
    {
        public static Url StringToUrl(string url)
        {
            Url temp = null;
            string url1;
            url1 = Fix(url);
            //转换前先修复提升成功率
            try
            {
                temp = new Url(url1);
            }
            catch
            {
                ;
            }
            return temp;
        }

        public static Uri StringToUri(string uri)
        {
            Uri temp = null;
            string uri1;
            uri1 = Fix(uri);
            //转换前先修复提升成功率
            try
            {
                temp = new Uri(uri1);
            }
            catch
            {
                ;
            }
            return temp;
        }
        public static string Fix(string url)
        {
            string temp = url;
            if (temp == null)
            {
                return url;
            }
            if (temp.ToLower().IndexOf("view-source:") == 0)
            {
                temp = temp.Substring(12, temp.Length - 12);
            }
            if (temp.ToLower().IndexOf("http") == -1)
            {
                temp = "http://" + temp;
            }
            while (temp.IndexOf("\\") != -1)
            {
                temp = temp.Replace("\\", "/");
            }
            while (temp.IndexOf("///") != -1)
            {
                temp = temp.Replace("//", "/");
            }
            while (temp.IndexOf(" ") != -1)
            {
                temp = temp.Replace(" ", "");
            }
            return temp;
        }
    }
    public static class GetHtmlCode
    {
        public static string GetWebClient(string url)
        {
            string strHtml = "", url1;
            url1 = _Url.Fix(url);
            WebClient myWebClient = new WebClient();
            Stream myStream = null;
            myStream = myWebClient.OpenRead(url1);
            StreamReader sr = new StreamReader(myStream, System.Text.Encoding.GetEncoding("utf-8"));
            strHtml = sr.ReadToEnd();
            myStream.Close();
            return strHtml;
        }
        public static string GetWebRequest(string url)
        {
            Uri uri;
            uri = _Url.StringToUri(url);
            WebRequest myReq = WebRequest.Create(uri);
            WebResponse result = myReq.GetResponse();
            Stream receviceStream = result.GetResponseStream();
            StreamReader readerOfStream = new StreamReader(receviceStream, System.Text.Encoding.GetEncoding("utf-8"));
            string strHTML = readerOfStream.ReadToEnd();
            readerOfStream.Close();
            receviceStream.Close();
            result.Close();
            return strHTML;
        }
        public static string GetHttpWebRequest(string url)
        {
            Uri uri;
            uri = _Url.StringToUri(url);
            HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create(uri);
            myReq.UserAgent =
                "User-Agent:Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/48.0.2564.8 Safari/537.36";
            myReq.Accept = "*/*";
            myReq.KeepAlive = true;
            myReq.Headers.Add("Accept-Language", "zh-cn,en-us;q=0.5");
            HttpWebResponse result = (HttpWebResponse)myReq.GetResponse();
            Stream receviceStream = result.GetResponseStream();
            StreamReader readerOfStream = new StreamReader(receviceStream, System.Text.Encoding.GetEncoding("utf-8"));
            string strHTML = readerOfStream.ReadToEnd();
            readerOfStream.Close();
            receviceStream.Close();
            result.Close();
            return strHTML;
        }
        //HttpWebRequest 方式最复杂，但确提供了更多的选择性。
        //有的网站检测客户端的UserAgent！如163.com，你如果使用WebClient WebRequest方式获取时，将获取到的是错误提示页面内容。
        //而通过HttpWebRequest就没问题。
    }
}
