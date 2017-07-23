using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PictureHelper
{
    public class PictureOperation
    {
        /// <summary>
        /// 通过二进制流创建Image对象
        /// </summary>
        /// <param name="buf">byte[] buf</param>
        /// <returns>Image</returns>
        public static Image CreateImageByBuffer(byte[] buf)
        {
            using(MemoryStream stream = new MemoryStream(buf))
            {
                Image img = Image.FromStream(stream);
                return img;
            }
        }
        /// <summary>
        /// 根据路径获取图片
        /// </summary>
        /// <param name="path">string path</param>
        /// <returns>Image</returns>
        public static Image GetImageByPath(string path)
        {
            Image img = Image.FromFile(path);
            return img;
        }
        /// <summary>
        /// 将图片转化为二进制数组
        /// </summary>
        /// <param name="img">Image img</param>
        /// <returns>byte[]</returns>
        public static byte[] GetBufferByImage(Image img)
        {
            using(MemoryStream stream = new MemoryStream())
            {
                img.Save(stream, img.RawFormat);
                return stream.ToArray();
            }
        }
        /// <summary>
        /// 保存图片到路径
        /// </summary>
        /// <param name="img">Image img</param>
        /// <param name="path">string path</param>
        /// <returns>保存成功返回true</returns>
        public static bool SaveImage(Image img, string path)
        {
            bool result=false;
            try
            {
                img.Save(path);
                result = true;
            }
            catch (Exception ex)
            { 
                
            }
            return result;
        }

        /// <summary>
        /// 图片转换成字节流
        /// </summary>
        /// <param name="img">要转换的Image对象</param>
        /// <returns>转换后返回的字节流</returns>
        public static byte[] ImgToByt(Image img)
        {
            MemoryStream ms = new MemoryStream();
            byte[] imagedata = null;
            img.Save(ms, ImageFormat.Jpeg);
            imagedata = ms.GetBuffer();
            return imagedata;
        }

        /// <summary>
        /// 字节流转换成图片
        /// </summary>
        /// <param name="byt">要转换的字节流</param>
        /// <returns>转换得到的Image对象</returns>
        public static Image BytToImg(byte[] byt)
        {
            MemoryStream ms = new MemoryStream(byt);
            Image img = Image.FromStream(ms);
            return img;
        }

        /// <summary>
        /// 根据图片路径返回图片的字节流byte[]
        /// </summary>
        /// <param name="imagePath">图片路径</param>
        /// <returns>返回的字节流</returns>
        private static byte[] GetImageByte(string imagePath)
        {
            Stream files;
            try
            {
                //如果本地含有图片则从本地读取，反之从服务器下载
                var localurl = AppDomain.CurrentDomain.BaseDirectory + imagePath.Substring(imagePath.IndexOf('D'));
                files = new FileStream(localurl, FileMode.Open);
            }
            catch
            {
                files = DownLoadImage(imagePath);
            }

            byte[] imgByte = new byte[files.Length];
            files.Read(imgByte, 0, imgByte.Length);
            files.Close();
            return imgByte;
        }

        /// <summary>
        /// 将图片下载到流
        /// </summary>
        /// <param name="path">图片路径</param>
        /// <returns></returns>
        public static Stream DownLoadImage(string path)
        {
            WebRequest request = WebRequest.Create(@path);
            WebResponse response = request.GetResponse();
            return response.GetResponseStream();
        }

        public static bool ThumbnailCallback()
        {
            return false;
        }

        /// <summary>
        /// 获取用户的Base64缩略图
        /// </summary>
        /// <param name="stream">图片流</param>
        /// <param name="width">缩略图宽度</param>
        /// <param name="height">缩略图宽度</param>
        /// <param name="isfixWidthHeight">是否固定高宽</param>
        /// <returns></returns>
        public static string GetBase64Image(int width, int height, Stream stream, bool isfixWidthHeight = false)
        {
            try
            {
                Image img = Image.FromStream(stream);
                //var id = BaseHelper.CreateGuid();
                //压缩图片大小
                var myCallback = new Image.GetThumbnailImageAbort(ThumbnailCallback);
                Image myThumbnail;
                if (isfixWidthHeight)
                {
                    myThumbnail = img.GetThumbnailImage(width, height, myCallback, IntPtr.Zero);
                }
                else
                {
                    int startX = 0, startY = 0;
                    var w = img.Width;
                    var h = img.Height;
                    int getX = w, getY = h;
                    float dw = (float)w / width;
                    float dh = (float)h / height;
                    if (Equals(dw, dh))
                    {
                        myThumbnail = img.GetThumbnailImage(width, height, myCallback, IntPtr.Zero);
                    }
                    else
                    {
                        //按比例截取中间部分
                        if (dw > dh)
                        {
                            getX = Convert.ToInt32(width * dh);
                            startX = Convert.ToInt32((w - getX) / 2);
                        }
                        else
                        {
                            getY = Convert.ToInt32(height * dw);
                            startY = Convert.ToInt32((h - getX) / 2);
                        }

                        Bitmap destBitmap = new Bitmap(getX, getY);//目标图
                        Rectangle destRect = new Rectangle(0, 0, getX, getY); //生成图区域
                        Rectangle srcRect = new Rectangle(startX, startY, getX, getY);//截取区域
                        Graphics g = Graphics.FromImage(destBitmap);
                        g.DrawImage(img, destRect, srcRect, GraphicsUnit.Pixel);
                        //SaveImage(destBitmap,id + "_Src");
                        myThumbnail = destBitmap.GetThumbnailImage(width, height, myCallback, IntPtr.Zero);
                    }
                }
                //SaveImage(myThumbnail,id+"_Thu");


                //将压缩图片转为Base64字符串
                return ImageToBase64(myThumbnail);
            }
            catch
            {
                return null;
            }

        }

        /// <summary>
        /// 将图片流转为base64编码
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static string GetBase64Image(Stream stream)
        {
            Image img = Image.FromStream(stream);
            return ImageToBase64(img);
        }


        /// <summary>
        /// 获取用户的Base64缩略图
        /// </summary>
        /// <param name="byt">图片存储字节</param>
        /// <param name="width">缩略图宽度</param>
        /// <param name="height">缩略图宽度</param>
        /// <returns></returns>
        public static string GetBase64Image(int width, int height, byte[] byt)
        {
            MemoryStream ms = new MemoryStream(byt);
            return GetBase64Image(width, height, ms);
        }

        /// <summary>
        /// 将图片字节转为base64编码
        /// </summary>
        /// <param name="byt"></param>
        /// <returns></returns>
        public static string GetBase64Image(byte[] byt)
        {
            MemoryStream stream = new MemoryStream(byt);
            Image img = Image.FromStream(stream);
            return ImageToBase64(img);
        }

        /// <summary>
        /// 获取用户的Base64缩略图
        /// </summary>
        /// <param name="imagePath">图片存储路径</param>
        /// <param name="width">缩略图宽度</param>
        /// <param name="height">缩略图宽度</param>
        /// <returns></returns>
        public static string GetBase64Image(int width, int height, string imagePath)
        {
            var stream = DownLoadImage(imagePath);
            return GetBase64Image(width, height, stream);
        }

        /// <summary>
        /// 将图片数据转换为Base64字符串
        /// </summary>
        /// <param name="img"></param>
        public static string ImageToBase64(Image img)
        {
            return "data:image/jpg;base64," + Convert.ToBase64String(ImgToByt(img));
        }

        /// <summary>
        /// 将图片流转换为Base64字符串
        /// </summary>
        /// <param name="ms"></param>
        /// <returns></returns>
        private static string StreamToBase64(MemoryStream ms)
        {
            return "data:image/gif;base64," + Convert.ToBase64String(ms.GetBuffer());
        }

        /// <summary>
        /// 将Base64字符串转换为图片
        /// </summary>
        /// <param name="str"></param>
        private static Image Base64ToImage(string str)
        {
            byte[] bytes = Convert.FromBase64String(str);
            return BytToImg(bytes);
        }


        /// <summary>
        /// 裁剪图片
        /// </summary>
        /// <param name="stream">图片流</param>
        /// <param name="startX">开始坐标X</param>
        /// <param name="startY">开始坐标Y</param>
        /// <param name="getX">截取横向距离</param>
        /// <param name="getY">截取纵向距离</param>
        /// <returns>图片base64编码</returns>
        public static string CutImage(Stream stream, int startX, int startY, int getX, int getY)
        {
            Image img = Image.FromStream(stream);

            int w = img.Width;
            int h = img.Height;

            if (startX < w && startY < h)
            {
                if (getX + startX > w)
                {
                    getX = w - startX;
                }
                if (startY + getY > h)
                {
                    getY = h - startY;
                }
            }
            //var id = BaseHelper.CreateGuid();

            Bitmap destBitmap = new Bitmap(getX, getY);//目标图
            Rectangle destRect = new Rectangle(0, 0, getX, getY); //生成图区域
            Rectangle srcRect = new Rectangle(startX, startY, getX, getY);//截取区域
            Graphics g = Graphics.FromImage(destBitmap);
            g.DrawImage(img, destRect, srcRect, GraphicsUnit.Pixel);
            //SaveImage(destBitmap,id + "_Src");
            return ImageToBase64(destBitmap);
        }


        /// <summary>
        /// 根据姓名生成默认缩略图
        /// </summary>
        /// <param name="name">姓名</param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static string DrawDefaultIcon(string name, int width = 40, int height = 40)
        {
            if (!string.IsNullOrEmpty(name))
            {
                var text = name.Substring(0, 1);
                Bitmap bit = new Bitmap(width + 1, height + 1);
                Graphics g = Graphics.FromImage(bit);
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.Clear(Color.Transparent);
                Rectangle rectangle = new Rectangle(0, 0, width, height);
                // 如要使边缘平滑，请取消下行的注释
                g.SmoothingMode = SmoothingMode.HighQuality;

                //设置文本背景色
                SolidBrush[] sbrush =
                {
                    new SolidBrush(Color.FromArgb(110,184,131)),
                    new SolidBrush(Color.FromArgb(216,174,72)),
                    new SolidBrush(Color.FromArgb(235,167,164)),
                    new SolidBrush(Color.FromArgb(166,116,163)),
                    new SolidBrush(Color.FromArgb(196,217,28)),
                    new SolidBrush(Color.FromArgb(91,186,216)),
                    new SolidBrush(Color.FromArgb(55,163,210)),
                    new SolidBrush(Color.FromArgb(129,172,60)),
                    new SolidBrush(Color.FromArgb(234,171,94))
                };

                Random r = new Random();
                //Thread.Sleep(10);
                g.FillEllipse(sbrush[r.Next(sbrush.Length)], rectangle);

                Font font = new Font(new FontFamily("微软雅黑"), 14, FontStyle.Regular);

                g.DrawString(text, font, new SolidBrush(Color.White), new Rectangle(1, 8, width, height), new StringFormat { Alignment = StringAlignment.Center });

                MemoryStream ms = new MemoryStream();
                bit.Save(ms, ImageFormat.Png);
                return "data:image/png;base64," + Convert.ToBase64String(ms.GetBuffer());
            }
            return null;
        }
    }
}
