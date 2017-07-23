using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ReaderPicture
{
    public partial class Form1 : Form
    {
        public static string path = "";
        public static Bitmap _img = null;
        public static Bitmap _img0 = null;
        public Form1()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 读取图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog dia = new OpenFileDialog();
            dia.Filter = "*|*.jpg|*.jpeg|*.png";
            if (dia.ShowDialog() == DialogResult.OK)
            {
                path = dia.FileName;
                Image img=Image.FromFile(dia.FileName);
                pictureBox1.BackgroundImage = img;
                _img0 = (Bitmap)img;
                pictureBox1.Width = img.Width;
                pictureBox1.Height = img.Height;
            }
        }
        /// <summary>
        /// 灰度处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            Image img = Image.FromFile(path);
            var data = img as Bitmap;
            //var nimg = PictureHelper.PictureManager.ToGray(data);
            //pictureBox1.BackgroundImage = nimg;
            //_img = nimg;
            PictureHelper.PictureManager.ToGrayPointer(data);
            pictureBox1.BackgroundImage = data;
            
        }
        /// <summary>
        /// 伪彩图
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            var img = PictureHelper.PseudoColorImageBuilder.PGrayToPseudoColor1(_img);
            pictureBox1.BackgroundImage = img;
        }
        /// <summary>
        /// 边缘提取
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        {
            pictureBox1.BackgroundImage = PictureHelper.PictureManager.EdgeExtraction(_img0);
        }
    }
}
