using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PictureHelper
{
    /// <summary>
    /// 获取图片指定位置的图像
    /// </summary>
    public class GetCustomImg
    {
        /// <summary>
        /// 获取位图数据对象
        /// </summary>
        /// <param name="image">位图数据</param>
        /// <param name="x">开始X坐标</param>
        /// <param name="y">开始Y坐标</param>
        /// <param name="width">截取宽度</param>
        /// <param name="height">截取高度</param>
        /// <returns>数据</returns>
        public static byte[] GetImgData(Bitmap image, int x, int y, int width, int height)
        {
            // 锁位图的位。 
            Rectangle rect = new Rectangle(x, y, width, height);
            BitmapData bmpData = image.LockBits(rect, ImageLockMode.ReadWrite, image.PixelFormat);
            // 获取第一行的地址。
            IntPtr ptr = bmpData.Scan0;
            // 声明一个数组来保存位图的字节。 
            //int bytes = Math.Abs(bmpData.Stride) * image.Height;
            //int bytes = width * height * 3;
            int bytes = bmpData.Stride * bmpData.Height;
            byte[] rgbValues = new byte[bytes];
            // 将RGB值复制到数组中。
            Marshal.Copy(ptr, rgbValues, 0, bytes);
            // 解锁。
            image.UnlockBits(bmpData);
            return rgbValues;
        }
        /// <summary>
        /// 保存buffer中的所有数据
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="pixelFormat"></param>
        /// <returns></returns>
        public static Bitmap GetImgByBitData(byte[] buffer, int width, int height, PixelFormat pixelFormat)
        {
            Bitmap newimg = new Bitmap(width, height);
            Rectangle rect = new Rectangle(0, 0, width, height);
            BitmapData returnData = newimg.LockBits(rect, ImageLockMode.WriteOnly, pixelFormat);
            IntPtr saveptr = returnData.Scan0;
            int bytes = returnData.Stride * returnData.Height;
            Marshal.Copy(buffer, 0, saveptr, bytes);
            newimg.UnlockBits(returnData);
            return newimg;
        }
        /// <summary>
        /// 保存buffer中的所有数据
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="pixelFormat"></param>
        /// <returns></returns>
        public static Bitmap GetImgByBitData(byte[] buffer, int width, int height)
        {
            Bitmap bmp = new Bitmap(width, height);
            int num = 0;
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    Color newColor = Color.FromArgb(buffer[2 + num], buffer[1 + num], buffer[0 + num]);
                    bmp.SetPixel(j,i, newColor);
                    num += 3;
                }
            }
            return bmp;
        }
        /// <summary>
        /// 将图片数据保存为指定大小图片
        /// </summary>
        /// <param name="buffer">原图片数据</param>
        /// <param name="imgwidth">原图片宽度</param>
        /// <param name="x">截取原图片开始位置x</param>
        /// <param name="y">截取原图片开始位置y</param>
        /// <param name="width">截图宽度</param>
        /// <param name="height">截图高度</param>
        /// <param name="savewidth">保存图片宽度</param>
        /// <param name="saveheight">保存图片高度</param>
        /// <returns>Bitmap</returns>
        public static Bitmap GetImgByBitData(byte[] buffer,int imgwidth,int x,int y, int width, int height,int savewidth,int saveheight)
        {
            Bitmap bmp = new Bitmap(width, height);
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    int num=(imgwidth * (y+i)+(x+j))*3;
                    Color newColor = Color.FromArgb(buffer[2 + num], buffer[1 + num], buffer[0 + num]);
                    bmp.SetPixel(j, i, newColor);
                }
            }
            //创建新图位图   
            Bitmap bitmap = new Bitmap(savewidth,saveheight);
            //创建作图区域   
            Graphics graphic = Graphics.FromImage(bitmap);
            //截取原图相应区域写入作图区
            graphic.DrawImage(bmp, new Rectangle(0, 0, savewidth, saveheight), new Rectangle(0, 0, width, height), GraphicsUnit.Pixel);
            graphic.Dispose();
            //释放资源
            bmp.Dispose();
            return bitmap;
        }

        //测试
        public static void ToGrayMemoryCopy(Bitmap bmap)
        {
            int width = bmap.Width;
            int height = bmap.Height;

            Rectangle rect = new Rectangle(0, 0, 200, 50);
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
    }
}
