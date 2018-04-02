using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Web;
using System.Net;
using System.IO;

namespace APP
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string url = textBox1.Text;
            url = "http://127.0.0.1:9002/Login/root?&Password=123&SystemCode=Smart.PipeCloud&UserType=0&format=json";
            string result=HttpGetHtml(url);
            textBox2.Text = result;
            
        }
        /// <summary>
        /// GET请求与获取结果
        /// </summary>
        /// <param name="Url">地址</param>
        /// <returns>请求结果</returns>
        private static string HttpGetHtml(string Url)
        {
            string retString = "";
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
                request.Method = "GET";
                request.ContentType = "text/html;charset=UTF-8";
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream myResponseStream = response.GetResponseStream();
                StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.UTF8);
                retString = myStreamReader.ReadToEnd();
                myStreamReader.Close();
                myResponseStream.Close();

            }
            catch (Exception ex)
            {
                retString = ex.Message;
            }
            return retString;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string result = textBox2.Text;
            string key = "1234567891234567891234567891234567893213465789";
            ///key = "123";
            string ss = Encryption.EncryptByDES(result, key);
            string mw = Encryption.DecryptByDES(ss, key);
            textBox2.Text = mw;
        }
    }
}
