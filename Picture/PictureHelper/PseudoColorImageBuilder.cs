using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PictureHelper
{
    /// <summary>  
    /// 伪彩色图像构造器  
    /// </summary>  
    public class PseudoColorImageBuilder
    {
        #region 映射表
        /// <summary>  
        /// 铁红色带映射表  
        /// 每一行代表一个彩色分类，存放顺序是RGB  
        /// </summary>  
        public static byte[,] ironTable = new byte[128, 3] {  
            {0,   0,  0},  
            {0,   0,  0},  
            {0,   0,  36},  
            {0,   0,  51},  
            {0,   0,  66},  
            {0,   0,  81},  
            {2,   0,  90},  
            {4,   0,  99},  
            {7,   0, 106},  
            {11,   0, 115},  
            {14,   0, 119},  
            {20,   0, 123},  
            {27,   0, 128},  
            {33,   0, 133},  
            {41,   0, 137},  
            {48,   0, 140},  
            {55,   0, 143},  
            {61,   0, 146},  
            {66,   0, 149},  
            {72,   0, 150},  
            {78,   0, 151},  
            {84,   0, 152},  
            {91,   0, 153},  
            {97,   0, 155},  
            {104,   0, 155},  
            {110,   0, 156},  
            {115,   0, 157},  
            {122,   0, 157},  
            {128,   0, 157},  
            {134,   0, 157},  
            {139,   0, 157},  
            {146,   0, 156},  
            {152,   0, 155},  
            {157,   0, 155},  
            {162,   0, 155},  
            {167,   0, 154},  
            {171,   0, 153},  
            {175,   1, 152},  
            {178,   1, 151},  
            {182,   2, 149},  
            {185,   4, 149},  
            {188,   5, 147},  
            {191,   6, 146},  
            {193,   8, 144},  
            {195,  11, 142},  
            {198,  13, 139},  
            {201,  17, 135},  
            {203,  20, 132},  
            {206,  23, 127},  
            {208,  26, 121},  
            {210,  29, 116},  
            {212,  33, 111},  
            {214,  37, 103},  
            {217,  41,  97},  
            {219,  46,  89},  
            {221,  49,  78},  
            {223,  53,  66},  
            {224,  56,  54},  
            {226,  60,  42},  
            {228,  64,  30},  
            {229,  68,  25},  
            {231,  72,  20},  
            {232,  76,  16},  
            {234,  78,  12},  
            {235,  82,  10},  
            {236,  86,   8},  
            {237,  90,   7},  
            {238,  93,   5},  
            {239,  96,   4},  
            {240, 100,   3},  
            {241, 103,   3},  
            {241, 106,   2},  
            {242, 109,   1},  
            {243, 113,   1},  
            {244, 116,   0},  
            {244, 120,   0},  
            {245, 125,   0},  
            {246, 129,   0},  
            {247, 133,   0},  
            {248, 136,   0},  
            {248, 139,   0},  
            {249, 142,   0},  
            {249, 145,   0},  
            {250, 149,   0},  
            {251, 154,   0},  
            {252, 159,   0},  
            {253, 163,   0},  
            {253, 168,   0},  
            {253, 172,   0},  
            {254, 176,   0},  
            {254, 179,   0},  
            {254, 184,   0},  
            {254, 187,   0},  
            {254, 191,   0},  
            {254, 195,   0},  
            {254, 199,   0},  
            {254, 202,   1},  
            {254, 205,   2},  
            {254, 208,   5},  
            {254, 212,   9},  
            {254, 216,  12},  
            {255, 219,  15},  
            {255, 221,  23},  
            {255, 224,  32},  
            {255, 227,  39},  
            {255, 229,  50},  
            {255, 232,  63},  
            {255, 235,  75},  
            {255, 238,  88},  
            {255, 239, 102},  
            {255, 241, 116},  
            {255, 242, 134},  
            {255, 244, 149},  
            {255, 245, 164},  
            {255, 247, 179},  
            {255, 248, 192},  
            {255, 249, 203},  
            {255, 251, 216},  
            {255, 253, 228},  
            {255, 254, 239},  
            {255, 255, 249},  
            {255, 255, 249},  
            {255, 255, 249},  
            {255, 255, 249},  
            {255, 255, 249},  
            {255, 255, 249},  
            {255, 255, 249},  
            {255, 255, 249} };

        /// <summary>  
        /// 彩虹色带映射表  
        /// </summary>  
        public static byte[,] rainTable = new byte[128, 3] {  
        {0,   0,   0},  
        {0,   0,   0},  
        {15,   0,  15},  
        {31,   0,  31},  
        {47,   0,  47},  
        {63,   0,  63},  
        {79,   0,  79},  
        {95,   0,  95},  
        {111,   0, 111},  
        {127,   0, 127},  
        {143,   0, 143},  
        {159,   0, 159},  
        {175,   0, 175},  
        {191,   0, 191},  
        {207,   0, 207},  
        {223,   0, 223},  
        {239,   0, 239},  
        {255,   0, 255},  
        {239,   0, 250},  
        {223,   0, 245},  
        {207,   0, 240},  
        {191,   0, 236},  
        {175,   0, 231},  
        {159,   0, 226},  
        {143,   0, 222},  
        {127,   0, 217},  
        {111,   0, 212},  
        {95,   0, 208},  
        {79,   0, 203},  
        {63,   0, 198},  
        {47,   0, 194},  
        {31,   0, 189},  
        {15,   0, 184},  
        {0,   0, 180},  
        {0,  15, 184},  
        {0,  31, 189},  
        {0,  47, 194},  
        {0,  63, 198},  
        {0,  79, 203},  
        {0,  95, 208},  
        {0, 111, 212},  
        {0, 127, 217},  
        {0, 143, 222},  
        {0, 159, 226},  
        {0, 175, 231},  
        {0, 191, 236},  
        {0, 207, 240},  
        {0, 223, 245},  
        {0, 239, 250},  
        {0, 255, 255},  
        {0, 245, 239},  
        {0, 236, 223},  
        {0, 227, 207},  
        {0, 218, 191},  
        {0, 209, 175},  
        {0, 200, 159},  
        {0, 191, 143},  
        {0, 182, 127},  
        {0, 173, 111},  
        {0, 164,  95},  
        {0, 155,  79},  
        {0, 146,  63},  
        {0, 137,  47},  
        {0, 128,  31},  
        {0, 119,  15},  
        {0, 110,   0},  
        {15, 118,   0},  
        {30, 127,   0},  
        {45, 135,   0},  
        {60, 144,   0},  
        {75, 152,   0},  
        {90, 161,   0},  
        {105, 169,  0},  
        {120, 178,  0},  
        {135, 186,  0},  
        {150, 195,  0},  
        {165, 203,  0},  
        {180, 212,  0},  
        {195, 220,  0},  
        {210, 229,  0},  
        {225, 237,  0},  
        {240, 246,  0},  
        {255, 255,  0},  
        {251, 240,  0},  
        {248, 225,  0},  
        {245, 210,  0},  
        {242, 195,  0},  
        {238, 180,  0},  
        {235, 165,  0},  
        {232, 150,  0},  
        {229, 135,  0},  
        {225, 120,  0},  
        {222, 105,  0},  
        {219,  90,  0},  
        {216,  75,  0},  
        {212,  60,  0},  
        {209,  45,  0},  
        {206,  30,  0},  
        {203,  15,  0},  
        {200,   0,  0},  
        {202,  11,  11},  
        {205,  23,  23},  
        {207,  34,  34},  
        {210,  46,  46},  
        {212,  57,  57},  
        {215,  69,  69},  
        {217,  81,  81},  
        {220,  92,  92},  
        {222, 104, 104},  
        {225, 115, 115},  
        {227, 127, 127},  
        {230, 139, 139},  
        {232, 150, 150},  
        {235, 162, 162},  
        {237, 173, 173},  
        {240, 185, 185},  
        {242, 197, 197},  
        {245, 208, 208},  
        {247, 220, 220},  
        {250, 231, 231},  
        {252, 243, 243},  
        {252, 243, 243},  
        {252, 243, 243},  
        {252, 243, 243},  
        {252, 243, 243},  
        {252, 243, 243},  
        {252, 243, 243},  
        {252, 243, 243}  
    };
        #endregion


        #region 伪彩图像
        /// <summary>  
        /// 灰度图转伪彩色图像函数（通过映射规则计算的方法）  
        /// </summary>  
        /// <param name="src">24位灰度图</param>  
        /// <returns>返回构造的伪彩色图像</returns>  
        unsafe public static Bitmap PGrayToPseudoColor1(Bitmap src)
        {
            try
            {
                Bitmap a = new Bitmap(src);

                Rectangle rect = new Rectangle(0, 0, a.Width, a.Height);
                System.Drawing.Imaging.BitmapData bmpData = a.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                int stride = bmpData.Stride;
                //unsafe
                {
                    byte* pIn = (byte*)bmpData.Scan0.ToPointer();
                    byte* P;
                    int R, G, B;
                    int temp = 0;

                    for (int y = 0; y < a.Height; y++)
                    {
                        for (int x = 0; x < a.Width; x++)
                        {
                            P = pIn;
                            B = P[0];
                            G = P[1];
                            R = P[2];

                            temp = (byte)(B * 0.114 + G * 0.587 + R * 0.299);
                            if (temp >= 0 && temp <= 63)
                            {
                                P[2] = 0;
                                P[1] = (byte)(254 - 4 * temp);
                                P[0] = (byte)255;
                            }
                            if (temp >= 64 && temp <= 127)
                            {
                                P[2] = 0;
                                P[1] = (byte)(4 * temp - 254);
                                P[0] = (byte)(510 - 4 * temp);
                            }
                            if (temp >= 128 && temp <= 191)
                            {
                                P[2] = (byte)(4 * temp - 510);
                                P[1] = (byte)(255);
                                P[0] = (byte)0;
                            }
                            if (temp >= 192 && temp <= 255)
                            {
                                P[2] = (byte)255;
                                P[1] = (byte)(1022 - 4 * temp);
                                P[0] = (byte)0;
                            }
                            pIn += 3;
                        }
                        pIn += stride - a.Width * 3;
                    }
                }
                a.UnlockBits(bmpData);
                return a;
            }
            catch (Exception e)
            {
                return null;
            }
        }
        //public static Bitmap PGrayToPseudoColor1(Bitmap src)
        //{
        //    try
        //    {
        //        Bitmap a = new Bitmap(src);
        //        int bytes = a.Width * a.Height * 3;
        //        byte[] rgbvalues = new byte[bytes];
        //        Rectangle rect = new Rectangle(0, 0, a.Width, a.Height);
        //        System.Drawing.Imaging.BitmapData bmpData = a.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
        //        int stride = bmpData.Stride;

        //        byte[] pixelData = new Byte[bmpData.Stride];
        //        byte[] P = new byte[3];
        //        int R, G, B;
        //        int temp = 0;

        //        Bitmap _img = new Bitmap(a.Width, a.Height);
        //        for (int y = 0; y < a.Height; y++)
        //        {
        //            Marshal.Copy(bmpData.Scan0 + (y * stride), pixelData, 0, stride);
        //            for (int x = 0; x < a.Width; x++)
        //            {
        //                B = pixelData[x + 0];
        //                G = pixelData[x + 1];
        //                R = pixelData[x + 2];

        //                temp = (byte)(B * 0.114 + G * 0.587 + R * 0.299);
        //                if (temp >= 0 && temp <= 63)
        //                {
        //                    P[2] = 0;
        //                    P[1] = (byte)(254 - 4 * temp);
        //                    P[0] = (byte)255;
        //                }
        //                if (temp >= 64 && temp <= 127)
        //                {
        //                    P[2] = 0;
        //                    P[1] = (byte)(4 * temp - 254);
        //                    P[0] = (byte)(510 - 4 * temp);
        //                }
        //                if (temp >= 128 && temp <= 191)
        //                {
        //                    P[2] = (byte)(4 * temp - 510);
        //                    P[1] = (byte)(255);
        //                    P[0] = (byte)0;
        //                }
        //                if (temp >= 192 && temp <= 255)
        //                {
        //                    P[2] = (byte)255;
        //                    P[1] = (byte)(1022 - 4 * temp);
        //                    P[0] = (byte)0;
        //                }
        //                rgbvalues[y * a.Height + x + 0] = P[0];
        //                rgbvalues[y * a.Height + x + 1] = P[1];
        //                rgbvalues[y * a.Height + x + 2] = P[2];
        //                Color color = Color.FromArgb(P[2], P[1], P[0]);
        //                _img.SetPixel(x, y, color);
        //            }
        //        }
        //        a.UnlockBits(bmpData);
        //        return _img;
        //    }
        //    catch (Exception e)
        //    {
        //        return null;
        //    }
        //}
        #endregion
        /// <summary>  
        /// 灰度图转伪彩色图像函数（通过查表的方法）  
        /// </summary>  
        /// <param name="src"></param>  
        /// <param name="type">转换类型（1.使用铁红  2.使用彩虹）</param>  
        /// <returns></returns>  
        unsafe public static Bitmap PGrayToPseudoColor2(Bitmap src, int type)
        {
            try
            {
                if (type == 1)
                {
                    Bitmap a = new Bitmap(src);
                    Rectangle rect = new Rectangle(0, 0, a.Width, a.Height);
                    System.Drawing.Imaging.BitmapData bmpData = a.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                    int stride = bmpData.Stride;
                    //unsafe
                    {
                        byte* pIn = (byte*)bmpData.Scan0.ToPointer();
                        int temp;
                        byte R, G, B;

                        for (int y = 0; y < a.Height; y++)
                        {
                            for (int x = 0; x < a.Width; x++)
                            {
                                temp = pIn[0] / 2;

                                R = ironTable[temp, 0];
                                G = ironTable[temp, 1];
                                B = ironTable[temp, 2];

                                pIn[0] = B;
                                pIn[1] = G;
                                pIn[2] = R;

                                pIn += 3;
                            }
                            pIn += stride - a.Width * 3;
                        }
                    }
                    a.UnlockBits(bmpData);
                    return a;
                }
                else if (type == 2)
                {
                    Bitmap a = new Bitmap(src);
                    Rectangle rect = new Rectangle(0, 0, a.Width, a.Height);
                    System.Drawing.Imaging.BitmapData bmpData = a.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                    int stride = bmpData.Stride;
                    //unsafe
                    {
                        byte* pIn = (byte*)bmpData.Scan0.ToPointer();
                        int temp;
                        byte R, G, B;

                        for (int y = 0; y < a.Height; y++)
                        {
                            for (int x = 0; x < a.Width; x++)
                            {
                                temp = pIn[0] / 2;

                                R = rainTable[temp, 0];
                                G = rainTable[temp, 1];
                                B = rainTable[temp, 2];

                                pIn[0] = B;
                                pIn[1] = G;
                                pIn[2] = R;

                                pIn += 3;
                            }
                            pIn += stride - a.Width * 3;
                        }
                    }
                    a.UnlockBits(bmpData);
                    return a;
                }
                else
                {
                    throw new Exception("type 参数不合法！");
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}
