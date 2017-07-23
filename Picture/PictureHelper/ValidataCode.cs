using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Drawing.Imaging;

namespace PictureHelper
{
    /// <summary>
    /// 生成验证码文件
    /// </summary>
    public class ValidataCode
    {
        /// <summary>
        /// 生成随机验证码
        /// </summary>
        /// <param name="length">随机验证码长度</param>
        /// <returns>随机验证码</returns>
        public string CreateCode(int length)
        {
            Random r = new Random();
            string ValidataCode = null;
            for (int i = 0; i < length; i++)
            {
                ValidataCode += r.Next(0, 10);
            }
            return ValidataCode;
        }
        /// <summary>
        /// 画验证码
        /// </summary>
        /// <param name="txt">验证码</param>
        /// <returns>byte[]</returns>
        public byte[] DrawValidataCode(string txt)
        {
            Bitmap image = new Bitmap((int)Math.Ceiling((txt.Length * 20.5)), 22);
            Graphics g = Graphics.FromImage(image);
            //生成随机生成器
            Random random = new Random();
            //清空图片背景色
            g.Clear(Color.White);
            //画图片的背景噪音线
            for (int i = 0; i < 2; i++)
            {
                Point tem_Point_1 = new Point(random.Next(image.Width), random.Next(image.Height));
                Point tem_Point_2 = new Point(random.Next(image.Width), random.Next(image.Height));
                g.DrawLine(new Pen(Color.Black), tem_Point_1, tem_Point_2);
            }
            Font font = new Font("宋体", 12, (FontStyle.Bold));
            LinearGradientBrush brush = new LinearGradientBrush(new Rectangle(0, 0, image.Width, image.Height), Color.Blue, Color.DarkRed, 1.2f, true);
            g.DrawString(txt, font, brush, 2, 2);
            //画图片的前景噪音点
            for (int i = 0; i < 100; i++)
            {
                Point tem_point = new Point(random.Next(image.Width), random.Next(image.Height));
                image.SetPixel(tem_point.X, tem_point.Y, Color.FromArgb(random.Next()));
            }
            //画图片的边框线
            g.DrawRectangle(new Pen(Color.Silver), 0, 0, image.Width - 1, image.Height - 1);
            //生成图片image
            MemoryStream stream = new MemoryStream();//定义 Stream 流 将绘制好的验证码图片保存到stream中
            image.Save(stream, ImageFormat.Jpeg);
            byte[] buffer = stream.ToArray();
            return buffer;
        }
    }
}
