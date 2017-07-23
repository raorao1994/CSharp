using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PictureHelper
{
    public class PictureManager
    {

        #region 获取图像数据
        /// <summary>
        /// getRGB
        /// </summary>
        /// <param name="image">图片</param>
        /// <param name="startX">开始x坐标</param>
        /// <param name="startY">开始y坐标</param>
        /// <param name="w">宽度</param>
        /// <param name="h">高度</param>
        /// <param name="rgbArray">RGB数组</param>
        /// <param name="offset"></param>
        /// <param name="scansize"></param>
        public static void getRGB(Bitmap image, int startX, int startY, int w, int h, int[] rgbArray, int offset, int scansize)
        {
            const int PixelWidth = 3;
            const PixelFormat PixelFormat = PixelFormat.Format24bppRgb;

            // En garde!
            if (image == null) throw new ArgumentNullException("image");
            if (rgbArray == null) throw new ArgumentNullException("rgbArray");
            if (startX < 0 || startX + w > image.Width) throw new ArgumentOutOfRangeException("startX");
            if (startY < 0 || startY + h > image.Height) throw new ArgumentOutOfRangeException("startY");
            if (w < 0 || w > scansize || w > image.Width) throw new ArgumentOutOfRangeException("w");
            if (h < 0 || (rgbArray.Length < offset + h * scansize) || h > image.Height) throw new ArgumentOutOfRangeException("h");

            BitmapData data = image.LockBits(new Rectangle(startX, startY, w, h), System.Drawing.Imaging.ImageLockMode.ReadOnly, PixelFormat);
            try
            {
                byte[] pixelData = new Byte[data.Stride];
                for (int scanline = 0; scanline < data.Height; scanline++)
                {
                    Marshal.Copy(data.Scan0 + (scanline * data.Stride), pixelData, 0, data.Stride);
                    for (int pixeloffset = 0; pixeloffset < data.Width; pixeloffset++)
                    {
                        // PixelFormat.Format32bppRgb means the data is stored
                        // in memory as BGR. We want RGB, so we must do some 
                        // bit-shuffling.
                        rgbArray[offset + (scanline * scansize) + pixeloffset] =
                            (pixelData[pixeloffset * PixelWidth + 2] << 16) +   // R 
                            (pixelData[pixeloffset * PixelWidth + 1] << 8) +    // G
                            pixelData[pixeloffset * PixelWidth];                // B
                    }
                }
            }
            finally
            {
                image.UnlockBits(data);
            }
        }
        /// <summary>
        /// 获取RGB数组
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public static byte[] GetRGB(Bitmap image)
        {
            if (image != null)
            {
                Bitmap newbitmap = image.Clone() as Bitmap;
                Rectangle rect = new Rectangle(0, 0, newbitmap.Width, newbitmap.Height);
                System.Drawing.Imaging.BitmapData bmpdata = newbitmap.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite, newbitmap.PixelFormat);
                IntPtr ptr = bmpdata.Scan0;
                int bytes = newbitmap.Width * newbitmap.Height * 3;
                byte[] rgbvalues = new byte[bytes];
                System.Runtime.InteropServices.Marshal.Copy(ptr, rgbvalues, 0, bytes);
                double colortemp = 0;
                for (int i = 0; i < rgbvalues.Length; i += 3)
                {
                    colortemp = rgbvalues[i + 2] * 0.299 + rgbvalues[i + 1] * 0.587 + rgbvalues[i] * 0.114;
                    rgbvalues[i] = rgbvalues[i + 1] = rgbvalues[i + 2] = (byte)colortemp;
                }
                System.Runtime.InteropServices.Marshal.Copy(rgbvalues, 0, ptr, bytes);
                newbitmap.UnlockBits(bmpdata);
                return rgbvalues;
                //pictureBox1.Image = newbitmap.Clone() as Image;
            }
            return null;
        }
        #endregion

        #region 图像灰度化
        /// <summary>
        /// 内存法灰度处理
        /// </summary>
        /// <param name="bmap">Bitmap bmap</param>
        /// <returns>Bitmap</returns>
        public static void ToGrayMemoryCopy(Bitmap bmap)
        {
            int width = bmap.Width;
            int height = bmap.Height;

            Rectangle rect = new Rectangle(0, 0, width, height);
            //BitmapData对象指定了位图的属性
            //1.Height属性：被锁定位图的高度.
            //2.Width属性：被锁定位图的高度.
            //3.PixelFormat属性：数据的实际像素格式.
            //4.Scan0属性：被锁定数组的首字节地址，如果整个图像被锁定，则是图像的第一个字节地址.
            //5.Stride属性：步幅，也称为扫描宽度.它是4的倍数，因此可能会多余出空的字节，空的字节排在最后
            BitmapData data = bmap.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            IntPtr ptr = data.Scan0;
            //获取数组长度
            int bytecount = data.Stride * data.Height;
            byte[] arr = new byte[bytecount];
            //读取所有数据
            Marshal.Copy(ptr, arr, 0, bytecount);
            for (int i = 0; i < bytecount; i += 3)
            {
                byte colortemp = (byte)(arr[i + 2] * 0.299 + arr[i + 1] * 0.587 + arr[i] * 0.114);
                arr[i] = arr[i + 1] = arr[i + 2] = (byte)colortemp;
            }
            Marshal.Copy(arr, 0, ptr, bytecount);
            bmap.UnlockBits(data);
            //return bmap;
        }
        /// <summary>
        /// 指针法灰度处理
        /// </summary>
        /// <param name="bmap"></param>
        public unsafe static void ToGrayPointer(Bitmap bmap)
        {
            Rectangle rec = new Rectangle(0, 0, bmap.Width, bmap.Height);
            BitmapData data = bmap.LockBits(rec, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            byte temp = 0;
            int width = data.Width;
            int height = data.Height;
            int redundant = data.Stride - width * 3;
            byte* ptr = (byte*)data.Scan0;
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    temp = (byte)(0.299 * ptr[2] + 0.587 * ptr[1] + 0.114 * ptr[0]);
                    ptr[0] = ptr[1] = ptr[2] = temp;
                    ptr += 3;
                }
                ptr += redundant;
            }
            bmap.UnlockBits(data);
        }
        /// <summary>
        /// 图像灰度化
        /// </summary>
        /// <param name="bmp"></param>
        /// <returns></returns>
        public static Bitmap ToGray(Bitmap bmp)
        {
            for (int i = 0; i < bmp.Width; i++)
            {
                for (int j = 0; j < bmp.Height; j++)
                {
                    //获取该点的像素的RGB的颜色
                    Color color = bmp.GetPixel(i, j);
                    //利用公式计算灰度值
                    int gray = (int)(color.R * 0.3 + color.G * 0.59 + color.B * 0.11);
                    Color newColor = Color.FromArgb(gray, gray, gray);
                    bmp.SetPixel(i, j, newColor);
                }
            }
            return bmp;
        }

        /// <summary>
        /// 图像灰度反转
        /// </summary>
        /// <param name="bmp"></param>
        /// <returns></returns>
        public static Bitmap GrayReverse(Bitmap bmp)
        {
            for (int i = 0; i < bmp.Width; i++)
            {
                for (int j = 0; j < bmp.Height; j++)
                {
                    //获取该点的像素的RGB的颜色
                    Color color = bmp.GetPixel(i, j);
                    Color newColor = Color.FromArgb(255 - color.R, 255 - color.G, 255 - color.B);
                    bmp.SetPixel(i, j, newColor);
                }
            }
            return bmp;
        }

        /// <summary>
        /// 图像二值化1：取图片的平均灰度作为阈值，低于该值的全都为0，高于该值的全都为255
        /// </summary>
        /// <param name="bmp"></param>
        /// <returns></returns>
        public static Bitmap ConvertTo1Bpp1(Bitmap bmp)
        {
            int average = 0;
            for (int i = 0; i < bmp.Width; i++)
            {
                for (int j = 0; j < bmp.Height; j++)
                {
                    Color color = bmp.GetPixel(i, j);
                    average += color.B;
                }
            }
            average = (int)average / (bmp.Width * bmp.Height);

            for (int i = 0; i < bmp.Width; i++)
            {
                for (int j = 0; j < bmp.Height; j++)
                {
                    //获取该点的像素的RGB的颜色
                    Color color = bmp.GetPixel(i, j);
                    int value = 255 - color.B;
                    Color newColor = value > average ? Color.FromArgb(0, 0, 0) : Color.FromArgb(255, 255, 255);
                    bmp.SetPixel(i, j, newColor);
                }
            }
            return bmp;
        }

        /// <summary>
        /// 图像二值化2
        /// </summary>
        /// <param name="img"></param>
        /// <returns></returns>
        public static Bitmap ConvertTo1Bpp2(Bitmap img)
        {
            int w = img.Width;
            int h = img.Height;
            Bitmap bmp = new Bitmap(w, h, PixelFormat.Format1bppIndexed);
            BitmapData data = bmp.LockBits(new Rectangle(0, 0, w, h), ImageLockMode.ReadWrite, PixelFormat.Format1bppIndexed);
            for (int y = 0; y < h; y++)
            {
                byte[] scan = new byte[(w + 7) / 8];
                for (int x = 0; x < w; x++)
                {
                    Color c = img.GetPixel(x, y);
                    if (c.GetBrightness() >= 0.5) scan[x / 8] |= (byte)(0x80 >> (x % 8));
                }
                Marshal.Copy(scan, 0, (IntPtr)((int)data.Scan0 + data.Stride * y), scan.Length);
            }
            return bmp;
        }
        #endregion

        #region 边缘提取
        /// <summary>
        /// 边缘提取
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public static unsafe Bitmap EdgeExtraction(Bitmap bitmap)
        {
            int Width = bitmap.Width;
            int Height = bitmap.Height;
            Bitmap newimg = new Bitmap(Width, Height);
            BitmapData oldData = bitmap.LockBits(new Rectangle(0, 0, Width, Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb); //原图
            BitmapData newData = newimg.LockBits(new Rectangle(0, 0, Width, Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);  //新图即边缘图
            //首先第一段代码是提取边缘，边缘置为黑色，其他部分置为白色
            byte* pin_1 = (byte*)(oldData.Scan0.ToPointer());
            byte* pin_2 = pin_1 + (oldData.Stride);
            byte* pout = (byte*)(newData.Scan0.ToPointer());

            #region 边缘提取
            for (int y = 0; y < oldData.Height - 1; y++)
            {
                for (int x = 0; x < oldData.Width; x++)
                {
                    //使用robert算子
                    double b = System.Math.Sqrt(((double)pin_1[0] - (double)(pin_2[0] + 3)) * ((double)pin_1[0] - (double)(pin_2[0] + 3)) + ((double)(pin_1[0] + 3) - (double)pin_2[0]) * ((double)(pin_1[0] + 3) - (double)pin_2[0]));
                    double g = System.Math.Sqrt(((double)pin_1[1] - (double)(pin_2[1] + 3)) * ((double)pin_1[1] - (double)(pin_2[1] + 3)) + ((double)(pin_1[1] + 3) - (double)pin_2[1]) * ((double)(pin_1[1] + 3) - (double)pin_2[1]));
                    double r = System.Math.Sqrt(((double)pin_1[2] - (double)(pin_2[2] + 3)) * ((double)pin_1[2] - (double)(pin_2[2] + 3)) + ((double)(pin_1[2] + 3) - (double)pin_2[2]) * ((double)(pin_1[2] + 3) - (double)pin_2[2]));
                    double bgr = b + g + r;//博主一直在纠结要不要除以3，感觉没差，选阈值的时候调整一下就好了- -

                    if (bgr > 80) //阈值，超过阈值判定为边缘（选取适当的阈值）
                    {
                        b = 0;
                        g = 0;
                        r = 0;
                    }
                    else
                    {
                        b = 255;
                        g = 255;
                        r = 255;
                    }
                    pout[0] = (byte)(b);
                    pout[1] = (byte)(g);
                    pout[2] = (byte)(r);
                    pin_1 = pin_1 + 3;
                    pin_2 = pin_2 + 3;
                    pout = pout + 3;

                }
                pin_1 += oldData.Stride - oldData.Width * 3;
                pin_2 += oldData.Stride - oldData.Width * 3;
                pout += newData.Stride - newData.Width * 3;
            }
            #endregion

            #region 加粗了一下线条
            ////这里博主加粗了一下线条- -，不喜欢的同学可以删了这段代码
            //byte* pin_5 = (byte*)(newData.Scan0.ToPointer());
            //for (int y = 0; y < oldData.Height - 1; y++)
            //{
            //    for (int x = 3; x < oldData.Width; x++)
            //    {
            //        if (pin_5[0] == 0 && pin_5[1] == 0 && pin_5[2] == 0)
            //        {
            //            pin_5[-3] = 0;
            //            pin_5[-2] = 0;
            //            pin_5[-1] = 0;      //边缘点的前一个像素点置为黑色（注意一定要是遍历过的像素点）                                                    
            //        }
            //        pin_5 += 3;

            //    }
            //    pin_5 += newData.Stride - newData.Width * 3;
            //}
            #endregion

            #region 这段代码是把原图和边缘图重合
            ////这段代码是把原图和边缘图重合
            //byte* pin_3 = (byte*)(oldData.Scan0.ToPointer());
            //byte* pin_4 = (byte*)(newData.Scan0.ToPointer());
            //for (int y = 0; y < oldData.Height - 1; y++)
            //{
            //    for (int x = 0; x < oldData.Width; x++)
            //    {
            //        if (pin_4[0] == 255 && pin_4[1] == 255 && pin_4[2] == 255)
            //        {
            //            pin_4[0] = pin_3[0];
            //            pin_4[1] = pin_3[1];
            //            pin_4[2] = pin_3[2];
            //        }
            //        pin_3 += 3;
            //        pin_4 += 3;
            //    }
            //    pin_3 += oldData.Stride - oldData.Width * 3;
            //    pin_4 += newData.Stride - newData.Width * 3;
            //}
            #endregion

            bitmap.UnlockBits(oldData);
            newimg.UnlockBits(newData);
            return newimg;
        } 
        #endregion

        #region 缩略图
        /// <summary>  
        /// 生成缩略图  
        /// </summary>  
        /// <param name="originalImagePath">源图路径（物理路径）</param>  
        /// <param name="thumbnailPath">缩略图路径（物理路径）</param>  
        /// <param name="width">缩略图宽度</param>  
        /// <param name="height">缩略图高度</param>  
        /// <param name="mode">生成缩略图的方式</param>      
        public static void MakeThumbnail(string originalImagePath, string thumbnailPath, int width, int height, string mode)
        {
            System.Drawing.Image originalImage = System.Drawing.Image.FromFile(originalImagePath);

            int towidth = width;
            int toheight = height;

            int x = 0;
            int y = 0;
            int ow = originalImage.Width;
            int oh = originalImage.Height;

            switch (mode)
            {
                case "HW":  //指定高宽缩放（可能变形）                  
                    break;
                case "W":   //指定宽，高按比例                      
                    toheight = originalImage.Height * width / originalImage.Width;
                    break;
                case "H":   //指定高，宽按比例  
                    towidth = originalImage.Width * height / originalImage.Height;
                    break;
                case "Cut": //指定高宽裁减（不变形）                  
                    if ((double)originalImage.Width / (double)originalImage.Height > (double)towidth / (double)toheight)
                    {
                        oh = originalImage.Height;
                        ow = originalImage.Height * towidth / toheight;
                        y = 0;
                        x = (originalImage.Width - ow) / 2;
                    }
                    else
                    {
                        ow = originalImage.Width;
                        oh = originalImage.Width * height / towidth;
                        x = 0;
                        y = (originalImage.Height - oh) / 2;
                    }
                    break;
                default:
                    break;
            }

            //新建一个bmp图片  
            System.Drawing.Image bitmap = new System.Drawing.Bitmap(towidth, toheight);

            //新建一个画板  
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bitmap);

            //设置高质量插值法  
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;

            //设置高质量,低速度呈现平滑程度  
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            //清空画布并以透明背景色填充  
            g.Clear(System.Drawing.Color.Transparent);

            //在指定位置并且按指定大小绘制原图片的指定部分  
            g.DrawImage(originalImage, new System.Drawing.Rectangle(0, 0, towidth, toheight), new System.Drawing.Rectangle(x, y, ow, oh), System.Drawing.GraphicsUnit.Pixel);

            try
            {
                //以jpg格式保存缩略图  
                bitmap.Save(thumbnailPath, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            catch (System.Exception e)
            {
                throw e;
            }
            finally
            {
                originalImage.Dispose();
                bitmap.Dispose();
                g.Dispose();
            }
        }
        #endregion

        #region 图片水印
        /// <summary>  
        /// 图片水印处理方法  
        /// </summary>  
        /// <param name="path">需要加载水印的图片路径（绝对路径）</param>  
        /// <param name="waterpath">水印图片（绝对路径）</param>  
        /// <param name="location">水印位置（传送正确的代码）</param>  
        public static string ImageWatermark(string path, string waterpath, string location)
        {
            string kz_name = Path.GetExtension(path);
            if (kz_name == ".jpg" || kz_name == ".bmp" || kz_name == ".jpeg")
            {
                DateTime time = DateTime.Now;
                string filename = "" + time.Year.ToString() + time.Month.ToString() + time.Day.ToString() + time.Hour.ToString() + time.Minute.ToString() + time.Second.ToString() + time.Millisecond.ToString();
                Image img = Bitmap.FromFile(path);
                Image waterimg = Image.FromFile(waterpath);
                Graphics g = Graphics.FromImage(img);
                ArrayList loca = GetLocation(location, img, waterimg);
                g.DrawImage(waterimg, new Rectangle(int.Parse(loca[0].ToString()), int.Parse(loca[1].ToString()), waterimg.Width, waterimg.Height));
                waterimg.Dispose();
                g.Dispose();
                string newpath = Path.GetDirectoryName(path) + filename + kz_name;
                img.Save(newpath);
                img.Dispose();
                File.Copy(newpath, path, true);
                if (File.Exists(newpath))
                {
                    File.Delete(newpath);
                }
            }
            return path;
        }

        /// <summary>  
        /// 图片水印位置处理方法  
        /// </summary>  
        /// <param name="location">水印位置</param>  
        /// <param name="img">需要添加水印的图片</param>  
        /// <param name="waterimg">水印图片</param>  
        private static ArrayList GetLocation(string location, Image img, Image waterimg)
        {
            ArrayList loca = new ArrayList();
            int x = 0;
            int y = 0;

            if (location == "LT")
            {
                x = 10;
                y = 10;
            }
            else if (location == "T")
            {
                x = img.Width / 2 - waterimg.Width / 2;
                y = img.Height - waterimg.Height;
            }
            else if (location == "RT")
            {
                x = img.Width - waterimg.Width;
                y = 10;
            }
            else if (location == "LC")
            {
                x = 10;
                y = img.Height / 2 - waterimg.Height / 2;
            }
            else if (location == "C")
            {
                x = img.Width / 2 - waterimg.Width / 2;
                y = img.Height / 2 - waterimg.Height / 2;
            }
            else if (location == "RC")
            {
                x = img.Width - waterimg.Width;
                y = img.Height / 2 - waterimg.Height / 2;
            }
            else if (location == "LB")
            {
                x = 10;
                y = img.Height - waterimg.Height;
            }
            else if (location == "B")
            {
                x = img.Width / 2 - waterimg.Width / 2;
                y = img.Height - waterimg.Height;
            }
            else
            {
                x = img.Width - waterimg.Width;
                y = img.Height - waterimg.Height;
            }
            loca.Add(x);
            loca.Add(y);
            return loca;
        }
        #endregion

        #region 文字水印
        /// <summary>  
        /// 文字水印处理方法  
        /// </summary>  
        /// <param name="path">图片路径（绝对路径）</param>  
        /// <param name="size">字体大小</param>  
        /// <param name="letter">水印文字</param>  
        /// <param name="color">颜色</param>  
        /// <param name="location">水印位置</param>  
        public static string LetterWatermark(string path, int size, string letter, Color color, string location)
        {
            #region

            string kz_name = Path.GetExtension(path);
            if (kz_name == ".jpg" || kz_name == ".bmp" || kz_name == ".jpeg")
            {
                DateTime time = DateTime.Now;
                string filename = "" + time.Year.ToString() + time.Month.ToString() + time.Day.ToString() + time.Hour.ToString() + time.Minute.ToString() + time.Second.ToString() + time.Millisecond.ToString();
                Image img = Bitmap.FromFile(path);
                Graphics gs = Graphics.FromImage(img);
                ArrayList loca = GetLocation(location, img, size, letter.Length);
                Font font = new Font("宋体", size);
                Brush br = new SolidBrush(color);
                gs.DrawString(letter, font, br, float.Parse(loca[0].ToString()), float.Parse(loca[1].ToString()));
                gs.Dispose();
                string newpath = Path.GetDirectoryName(path) + filename + kz_name;
                img.Save(newpath);
                img.Dispose();
                File.Copy(newpath, path, true);
                if (File.Exists(newpath))
                {
                    File.Delete(newpath);
                }
            }
            return path;

            #endregion
        }

        /// <summary>  
        /// 文字水印位置的方法  
        /// </summary>  
        /// <param name="location">位置代码</param>  
        /// <param name="img">图片对象</param>  
        /// <param name="width">宽(当水印类型为文字时,传过来的就是字体的大小)</param>  
        /// <param name="height">高(当水印类型为文字时,传过来的就是字符的长度)</param>  
        private static ArrayList GetLocation(string location, Image img, int width, int height)
        {
            #region

            ArrayList loca = new ArrayList();  //定义数组存储位置  
            float x = 10;
            float y = 10;

            if (location == "LT")
            {
                loca.Add(x);
                loca.Add(y);
            }
            else if (location == "T")
            {
                x = img.Width / 2 - (width * height) / 2;
                loca.Add(x);
                loca.Add(y);
            }
            else if (location == "RT")
            {
                x = img.Width - width * height;
            }
            else if (location == "LC")
            {
                y = img.Height / 2;
            }
            else if (location == "C")
            {
                x = img.Width / 2 - (width * height) / 2;
                y = img.Height / 2;
            }
            else if (location == "RC")
            {
                x = img.Width - height;
                y = img.Height / 2;
            }
            else if (location == "LB")
            {
                y = img.Height - width - 5;
            }
            else if (location == "B")
            {
                x = img.Width / 2 - (width * height) / 2;
                y = img.Height - width - 5;
            }
            else
            {
                x = img.Width - width * height;
                y = img.Height - width - 5;
            }
            loca.Add(x);
            loca.Add(y);
            return loca;

            #endregion
        }
        #endregion

        #region 调整光暗
        /// <summary>  
        /// 调整光暗  
        /// </summary>  
        /// <param name="mybm">原始图片</param>  
        /// <param name="width">原始图片的长度</param>  
        /// <param name="height">原始图片的高度</param>  
        /// <param name="val">增加或减少的光暗值</param>  
        public static Bitmap LDPic(Bitmap mybm, int width, int height, int val)
        {
            Bitmap bm = new Bitmap(width, height);//初始化一个记录经过处理后的图片对象  
            int x, y, resultR, resultG, resultB;//x、y是循环次数，后面三个是记录红绿蓝三个值的  
            Color pixel;
            for (x = 0; x < width; x++)
            {
                for (y = 0; y < height; y++)
                {
                    pixel = mybm.GetPixel(x, y);//获取当前像素的值  
                    resultR = pixel.R + val;//检查红色值会不会超出[0, 255]  
                    resultG = pixel.G + val;//检查绿色值会不会超出[0, 255]  
                    resultB = pixel.B + val;//检查蓝色值会不会超出[0, 255]  
                    bm.SetPixel(x, y, Color.FromArgb(resultR, resultG, resultB));//绘图  
                }
            }
            return bm;
        }
        #endregion

        #region 反色处理
        /// <summary>  
        /// 反色处理  
        /// </summary>  
        /// <param name="mybm">原始图片</param>  
        /// <param name="width">原始图片的长度</param>  
        /// <param name="height">原始图片的高度</param>  
        public static Bitmap RePic(Bitmap mybm, int width, int height)
        {
            Bitmap bm = new Bitmap(width, height);//初始化一个记录处理后的图片的对象  
            int x, y, resultR, resultG, resultB;
            Color pixel;
            for (x = 0; x < width; x++)
            {
                for (y = 0; y < height; y++)
                {
                    pixel = mybm.GetPixel(x, y);//获取当前坐标的像素值  
                    resultR = 255 - pixel.R;//反红  
                    resultG = 255 - pixel.G;//反绿  
                    resultB = 255 - pixel.B;//反蓝  
                    bm.SetPixel(x, y, Color.FromArgb(resultR, resultG, resultB));//绘图  
                }
            }
            return bm;
        }
        #endregion

        #region 浮雕处理
        /// <summary>  
        /// 浮雕处理  
        /// </summary>  
        /// <param name="oldBitmap">原始图片</param>  
        /// <param name="Width">原始图片的长度</param>  
        /// <param name="Height">原始图片的高度</param>  
        public static Bitmap FD(Bitmap oldBitmap, int Width, int Height)
        {
            Bitmap newBitmap = new Bitmap(Width, Height);
            Color color1, color2;
            for (int x = 0; x < Width - 1; x++)
            {
                for (int y = 0; y < Height - 1; y++)
                {
                    int r = 0, g = 0, b = 0;
                    color1 = oldBitmap.GetPixel(x, y);
                    color2 = oldBitmap.GetPixel(x + 1, y + 1);
                    r = Math.Abs(color1.R - color2.R + 128);
                    g = Math.Abs(color1.G - color2.G + 128);
                    b = Math.Abs(color1.B - color2.B + 128);
                    if (r > 255) r = 255;
                    if (r < 0) r = 0;
                    if (g > 255) g = 255;
                    if (g < 0) g = 0;
                    if (b > 255) b = 255;
                    if (b < 0) b = 0;
                    newBitmap.SetPixel(x, y, Color.FromArgb(r, g, b));
                }
            }
            return newBitmap;
        }
        #endregion

        #region 拉伸图片
        /// <summary>  
        /// 拉伸图片  
        /// </summary>  
        /// <param name="bmp">原始图片</param>  
        /// <param name="newW">新的宽度</param>  
        /// <param name="newH">新的高度</param>  
        public static Bitmap ResizeImage(Bitmap bmp, int newW, int newH)
        {
            try
            {
                Bitmap bap = new Bitmap(newW, newH);
                Graphics g = Graphics.FromImage(bap);
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.DrawImage(bap, new Rectangle(0, 0, newW, newH), new Rectangle(0, 0, bap.Width, bap.Height), GraphicsUnit.Pixel);
                g.Dispose();
                return bap;
            }
            catch
            {
                return null;
            }
        }
        #endregion

        #region 滤色处理
        /// <summary>  
        /// 滤色处理  
        /// </summary>  
        /// <param name="mybm">原始图片</param>  
        /// <param name="width">原始图片的长度</param>  
        /// <param name="height">原始图片的高度</param>  
        public static Bitmap FilPic(Bitmap mybm, int width, int height)
        {
            Bitmap bm = new Bitmap(width, height);//初始化一个记录滤色效果的图片对象  
            int x, y;
            Color pixel;

            for (x = 0; x < width; x++)
            {
                for (y = 0; y < height; y++)
                {
                    pixel = mybm.GetPixel(x, y);//获取当前坐标的像素值  
                    bm.SetPixel(x, y, Color.FromArgb(0, pixel.G, pixel.B));//绘图  
                }
            }
            return bm;
        }
        #endregion

        #region 左右翻转
        /// <summary>  
        /// 左右翻转  
        /// </summary>  
        /// <param name="mybm">原始图片</param>  
        /// <param name="width">原始图片的长度</param>  
        /// <param name="height">原始图片的高度</param>  
        public static Bitmap RevPicLR(Bitmap mybm, int width, int height)
        {
            Bitmap bm = new Bitmap(width, height);
            int x, y, z; //x,y是循环次数,z是用来记录像素点的x坐标的变化的  
            Color pixel;
            for (y = height - 1; y >= 0; y--)
            {
                for (x = width - 1, z = 0; x >= 0; x--)
                {
                    pixel = mybm.GetPixel(x, y);//获取当前像素的值  
                    bm.SetPixel(z++, y, Color.FromArgb(pixel.R, pixel.G, pixel.B));//绘图  
                }
            }
            return bm;
        }
        #endregion

        #region 上下翻转
        /// <summary>  
        /// 上下翻转  
        /// </summary>  
        /// <param name="mybm">原始图片</param>  
        /// <param name="width">原始图片的长度</param>  
        /// <param name="height">原始图片的高度</param>  
        public static Bitmap RevPicUD(Bitmap mybm, int width, int height)
        {
            Bitmap bm = new Bitmap(width, height);
            int x, y, z;
            Color pixel;
            for (x = 0; x < width; x++)
            {
                for (y = height - 1, z = 0; y >= 0; y--)
                {
                    pixel = mybm.GetPixel(x, y);//获取当前像素的值  
                    bm.SetPixel(x, z++, Color.FromArgb(pixel.R, pixel.G, pixel.B));//绘图  
                }
            }
            return bm;
        }
        #endregion

        #region 压缩图片
        /// <summary>  
        /// 压缩到指定尺寸  
        /// </summary>  
        /// <param name="oldfile">原文件</param>  
        /// <param name="newfile">新文件</param>  
        public static bool Compress(string oldfile, string newfile)
        {
            try
            {
                System.Drawing.Image img = System.Drawing.Image.FromFile(oldfile);
                System.Drawing.Imaging.ImageFormat thisFormat = img.RawFormat;
                Size newSize = new Size(100, 125);
                Bitmap outBmp = new Bitmap(newSize.Width, newSize.Height);
                Graphics g = Graphics.FromImage(outBmp);
                g.CompositingQuality = CompositingQuality.HighQuality;
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(img, new Rectangle(0, 0, newSize.Width, newSize.Height), 0, 0, img.Width, img.Height, GraphicsUnit.Pixel);
                g.Dispose();
                EncoderParameters encoderParams = new EncoderParameters();
                long[] quality = new long[1];
                quality[0] = 100;
                EncoderParameter encoderParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
                encoderParams.Param[0] = encoderParam;
                ImageCodecInfo[] arrayICI = ImageCodecInfo.GetImageEncoders();
                ImageCodecInfo jpegICI = null;
                for (int x = 0; x < arrayICI.Length; x++)
                    if (arrayICI[x].FormatDescription.Equals("JPEG"))
                    {
                        jpegICI = arrayICI[x]; //设置JPEG编码  
                        break;
                    }
                img.Dispose();
                if (jpegICI != null) outBmp.Save(newfile, System.Drawing.Imaging.ImageFormat.Jpeg);
                outBmp.Dispose();
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region 图片灰度化
        public static Color Gray(Color c)
        {
            int rgb = Convert.ToInt32((double)(((0.3 * c.R) + (0.59 * c.G)) + (0.11 * c.B)));
            return Color.FromArgb(rgb, rgb, rgb);
        }
        #endregion

        #region 转换为黑白图片
        /// <summary>  
        /// 转换为黑白图片  
        /// </summary>  
        /// <param name="mybt">要进行处理的图片</param>  
        /// <param name="width">图片的长度</param>  
        /// <param name="height">图片的高度</param>  
        public static Bitmap BWPic(Bitmap mybm, int width, int height)
        {
            Bitmap bm = new Bitmap(width, height);
            int x, y, result; //x,y是循环次数，result是记录处理后的像素值  
            Color pixel;
            for (x = 0; x < width; x++)
            {
                for (y = 0; y < height; y++)
                {
                    pixel = mybm.GetPixel(x, y);//获取当前坐标的像素值  
                    result = (pixel.R + pixel.G + pixel.B) / 3;//取红绿蓝三色的平均值  
                    bm.SetPixel(x, y, Color.FromArgb(result, result, result));
                }
            }
            return bm;
        }
        #endregion

        #region 获取图片中的各帧
        /// <summary>  
        /// 获取图片中的各帧  
        /// </summary>  
        /// <param name="pPath">图片路径</param>  
        /// <param name="pSavePath">保存路径</param>  
        public static void GetFrames(string pPath, string pSavedPath)
        {
            Image gif = Image.FromFile(pPath);
            FrameDimension fd = new FrameDimension(gif.FrameDimensionsList[0]);
            int count = gif.GetFrameCount(fd); //获取帧数(gif图片可能包含多帧，其它格式图片一般仅一帧)  
            for (int i = 0; i < count; i++)    //以Jpeg格式保存各帧  
            {
                gif.SelectActiveFrame(fd, i);
                gif.Save(pSavedPath + "\\frame_" + i + ".jpg", ImageFormat.Jpeg);
            }
        }
        #endregion
    }
}
