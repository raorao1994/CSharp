using PictureHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BaiduTileCutter
{
    public partial class Form1 : Form
    {
        public static string picpath = "";
        public static string savepath = "";
        public int step = 0;
        public Form1()
        {
            InitializeComponent();
            label2.Enabled = false;
            label4.Enabled = false;
            label5.Enabled = false;
            label6.Enabled = false;
            label7.Enabled = false;
            button1.Enabled = false;
            button2.Enabled = false;
            label8.Text = "选择你需要的图片";
            groupBox2.Text = "选择图片";
            //text();
        }

        private void text()
        {
            Image bi = Image.FromFile(@"C:\Users\Public\Pictures\Sample Pictures\psb.jpg");
            Bitmap outputimage = new Bitmap(256, 256);
            Graphics g = Graphics.FromImage(outputimage);
            g.Clear(Color.Empty);
            g.DrawImage(bi, 0, 0, 255, 255);
            //释放资源
            bi.Dispose();
            g.Dispose();


            //创建新图位图   
            Bitmap bitmap = new Bitmap(256, 256);
            //创建作图区域   
            Graphics graphic = Graphics.FromImage(bitmap);
            //截取原图相应区域写入作图区   
            graphic.DrawImage(outputimage, new Rectangle(0, 0, 100, 100), new Rectangle(0, 0, 256, 256), GraphicsUnit.Pixel);
            graphic.Dispose();
            //从作图区生成新图   
            bitmap.Save(@"F:\Baidu\aaaa.png");
            //释放资源
            bitmap.Dispose();
        }
        /// <summary>
        /// 选择图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog dia = new OpenFileDialog();
            dia.Filter = "图像文件(*.jpg;*.jpg;*.jpeg;*.gif;*.png;*.tiff;*.tif)|*.jpg;*.jpeg;*.gif;*.png;*.tiff;*.tif";
            if (dia.ShowDialog() == DialogResult.OK)
            {
                if (step == 0)
                {
                    picpath = dia.FileName;
                    textBox1.Text = picpath;
                    pictureBox1.Image = Image.FromFile(picpath);
                    button1.Enabled = true;
                }
                if (step == 1)
                {
                    savepath = dia.FileName;
                }
            }
        }
        /// <summary>
        /// 下一步
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            step++;
            StepFunc();
            button2.Enabled = true;
            if (step == 6)
            {
                button1.Enabled = false;
            }
        }
        /// <summary>
        /// 上一步
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            step--;
            StepFunc();
            if (step == 0)
            {
                button2.Enabled = false;
            }
        }
        /// <summary>
        /// 控件显示调整
        /// </summary>
        private void StepFunc()
        {
            switch (step)
            {
                case 0:
                    {
                        HideAll();
                        label1.Enabled = true;
                        groupBox2.Text = "选择图片";
                        textBox1.Show();
                        button3.Show();
                        pictureBox1.Show();
                    } break;
                case 1: 
                    {
                        HideAll();
                        label2.Enabled = true;
                        groupBox2.Text = "路径设置";
                        textBox1.Show();
                        button3.Show();
                        pictureBox1.Show();
                        button3.Text = "选择...";
                    } break;
                case 2: break;
                case 3: break;
                case 4: break;
                case 5: break;
                case 6: break;
            }
        }

        private void HideAll()
        {
            label1.Enabled = false;
            label2.Enabled = false;
            label4.Enabled = false;
            label5.Enabled = false;
            label6.Enabled = false;
            label7.Enabled = false;
            textBox1.Hide();
            button3.Hide();
            pictureBox1.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
           
            //TileCutterUtils t = new TileCutterUtils(picpath, 1, 18, 11, 118, 39, @"F:\aaaa");
            //t.cutterall();

            

            Bitmap img = (Bitmap)PictureOperation.GetImageByPath(picpath);
            byte[] data = GetCustomImg.GetImgData(img, 0, 0, img.Width, img.Height);

            //Bitmap saveimg = GetCustomImg.GetImgByBitData(data, img.Width, img.Height, img.PixelFormat);
            //Bitmap saveimg = GetCustomImg.GetImgByBitData(data, 100,100, img.PixelFormat);
            Bitmap saveimg = GetCustomImg.GetImgByBitData(data,img.Width, 0, 0,100,100,400,400);
            saveimg.Save(@"F:\aaaa.png");
        }
    }
}
