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
    public partial class Form1 : Form
    {
        /// <summary>
        /// 获取深度
        /// </summary>
        private int Deep = 1;
        private string Encoding = "";
        public Form1()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 开始运行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void start_Click(object sender, EventArgs e)
        {
            //获取参数
            Deep = Int32.Parse(DeepSelect.SelectedItem.ToString());
            Encoding = encodingselect.SelectedItem.ToString();
            //1.0 获取爬虫运行网站
            var url = URLText.Text.Trim();
            //2.0 下载爬虫页面内容
            var html = HttpHelper.GetHttpData(url);

            this.textBoxList.AppendText("网页下载完成 \n");
            DoJob.GetAllData(html);
        }
        /// <summary>
        /// 停止运行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void stopbtn_Click(object sender, EventArgs e)
        {

        }
    }
}
