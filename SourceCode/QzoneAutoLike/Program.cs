using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Windows.Forms;

namespace QzoneAutoLike
{
    static class Program
    {
        public const string localVersion = "1.0.0.0000";
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ControlForm());
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
        //WebClient
        public static string GetWebClient(string url)
        {
            string strHtml = "", url1;
            url1 = _Url.Fix(url);
            WebClient myWebClient = new WebClient();
            Stream myStream = myWebClient.OpenRead(url1);
            StreamReader sr = new StreamReader(myStream, System.Text.Encoding.GetEncoding("utf-8"));
            strHtml = sr.ReadToEnd();
            myStream.Close();
            return strHtml;
        }

        //WebRequest
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

        //HttpWebRequest
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
