using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tools;

namespace QzoneWebSpider
{
    public partial class Form2 : Form
    {
        public static List<string> list = new List<string>();
        public static int hitCount = 0;
        public Form2()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 指定url
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            string url = textBox1.Text.Trim();
            webBrowser1.Url = new Uri(url);
            webBrowser1.Navigate(url);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var htmldocument = (mshtml.HTMLDocument)webBrowser1.Document.DomDocument;
            string gethtml = htmldocument.documentElement.outerHTML;
            GetData(gethtml);
        }
        public void GetData(string html)
        {
            int i = 0;
            var data = html.ToRegexStringArray("(?<=nameCard_).{5,32}(?=\">)");
            foreach (var item in data)
            {
                int index=item.IndexOf("des_");
                string str = item;
                if (index > 0)
                {
                    str = item.Substring(0,index);
                }
                if (!list.Contains(str))
                {
                    list.Add(str);
                    i++;
                }
                if (index> 0)
                {
                    str = item.Substring(index+4);
                    if (!list.Contains(str))
                    {
                        list.Add(str);
                        i++;
                    }
                }
            }
        }
    }
}
