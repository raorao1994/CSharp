using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaiduTileCutter
{
    public class TileCutterUtils
    {
        #region 公共变量
        // 最小等级
        private int minlevel;
        ///最大等级
        private int maxlevel;
        // 图片当前级别
        private int piclevel;
        //图片中心点X坐标
        private double mercatorx;
        //图片中心点Y坐标
        private double mercatory;
        ///图片路径
        private string pic;
        // 保存路径
        private string savepath;
        #endregion

        #region 构造函数
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pic">图片路径</param>
        /// <param name="mercatorx">中心点x坐标</param>
        /// <param name="mercatory">中心点y坐标</param>
        /// <param name="savepath">保存路径</param>
        public TileCutterUtils(string pic, double mercatorx, double mercatory, string savepath)
        {
            this.pic = pic;
            this.mercatorx = mercatorx;
            this.mercatory = mercatory;
            this.savepath = savepath;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pic">图片路径</param>
        /// <param name="mercatorx">最小等级</param>
        /// <param name="mercatory">最大等级</param>
        /// <param name="piclevel">图片等级</param>
        /// <param name="mercatorx">中心点x</param>
        /// <param name="mercatory">中心点y</param>
        /// <param name="savepath">保存路径</param>
        public TileCutterUtils(string pic, int minlevel, int maxlevel, int piclevel, double mercatorx, double mercatory, string savepath)
        {
            this.pic = pic;
            this.minlevel = minlevel;
            this.maxlevel = maxlevel;
            this.mercatorx = mercatorx;
            this.mercatory = mercatory;
            this.savepath = savepath;
            this.piclevel = piclevel;
        }
        #endregion

        #region 切图
        /// <summary>
        /// 开始切图
        /// </summary>
        public void cutterall()
        {
            PixPoint p = LonlatToMercator(mercatorx,mercatory);
            mercatorx = p.X;
            mercatory = p.Y;
            for (int i = minlevel; i <= maxlevel; i++)
            {
                Console.WriteLine("开始切第"+i+"层");
                cutterone(i);
                Console.WriteLine("第" + i + "层结束");
            }
        }
        /// <summary>
        ///各级别切图
        /// </summary>
        /// <param name="level"></param>
        public void cutterone(int level)
        {
            /*
             *18级别的图片像素对应的墨卡托坐标是标准值
             */
            //图片中心的像素坐标(pixelx,pixely)，图片中心的平面坐标即墨卡托坐标(mercatorx, mercatory)
            //像素坐标  = 平面坐标 * Math.Pow(2, level - 18)
            double pixelPow = Math.Pow(2, level - 18);
            //计算出图片缩放到level级别时，中心点的像素坐标
            double pixelx = mercatorx * pixelPow;
            double pixely = mercatory * pixelPow;
            Image bi = Image.FromFile(pic);
            int width = bi.Width;
            int height = bi.Height;
            //图片遵循原则：当前图片所属级别piclevel不缩放即像素级别相等。
            //按照公式缩放：当前级别图片长度 = 原图片长度 * Math.Pow(2, level - piclevel)
            //minx: 图片左下角x坐标
            //miny: 图片左下角y坐标
            //maxx: 图片右上角x坐标
            //maxy: 图片右上角y坐标
            double levelpow = Math.Pow(2, level - piclevel);
            double minx = pixelx - width * levelpow / 2;
            double miny = pixely - height * levelpow / 2;
            double maxx = pixelx + width * levelpow / 2;
            double maxy = pixely + height * levelpow / 2;

            int neatminx = (int)minx / 256;
            int remminx = (int)minx % 256;
            int neatminy = (int)miny / 256;
            int remminy = (int)miny % 256;

            int neatmaxx = (int)maxx / 256;
            int remmaxx = 256 - (int)maxx % 256;
            int neatmaxy = (int)maxy / 256;
            int remmaxy = 256 - (int)maxy % 256;
            //(neatminx,neatminy)为图片左下角最近的整数图块坐标,neatminx到neatmaxx即当前级别下切割图块的图块坐标x
            //(neatmaxx,neatmaxy)为图片右上角最近的整数图块坐标,neatminy到neatmaxy即当前级别下切割图块的图块坐标y

            // 扩充原图片为width * height --- > (remminx + width + remmaxx ) * (remminy + height +remmaxy)
            int w = neatmaxx - neatminx + 1;//图片横向张数
            int h = neatmaxy - neatminy + 1;//图片纵向张数
            int extendwidth = w * 256;
            int extendheight = h * 256;
            //创建当缩放级别下的位图
            Bitmap outputimage = new Bitmap(extendwidth, extendheight);
            Graphics g = Graphics.FromImage(outputimage);
            g.Clear(Color.Empty);
            g.DrawImage(bi, new Rectangle(remminx, remmaxy, (int)(width * levelpow), (int)(height * levelpow)), new Rectangle(0, 0, width, height), GraphicsUnit.Pixel);
            //释放资源
            bi.Dispose();
            g.Dispose();
            //切割图片，共( neatmaxx - neatminx + 1) * (neatmaxy - neatminy + 1)份 256*256图片
            string dirname = savepath.Substring(0, savepath.LastIndexOf("\\")) + "\\tiles\\" + level;
            for (int i = 0; i < w; i++)
            {
                for (int j = 1; j <= h; j++)
                {
                    string croppicname = dirname + "\\tile" + (neatminx + i) + "_" + (neatminy + j - 1) + ".png";
                    cutpic(outputimage, 256 * i, 256 * (h - j), 256, 256, croppicname);
                }
            }
            outputimage.Dispose();
            Console.WriteLine("切割图片成功！");
        }

        /// <summary>
        /// 截图
        /// </summary>
        /// <param name="img">原始图片</param>
        /// <param name="x">位置x坐标</param>
        /// <param name="y">位置y坐标</param>
        /// <param name="width">截图宽度</param>
        /// <param name="height">截图高度</param>
        public void cutpic(Image img, int x, int y, int width, int height, string savepath)
        {
            //创建新图位图   
            Bitmap bitmap = new Bitmap(256, 256);
            //创建作图区域   
            Graphics graphic = Graphics.FromImage(bitmap);
            //截取原图相应区域写入作图区
            graphic.DrawImage(img, new Rectangle(0, 0, 256, 256), new Rectangle(x, y, width, height), GraphicsUnit.Pixel);
            graphic.Dispose();
            //从作图区生成新图
            //保存图象 
            var path = Directory.GetParent(savepath).FullName;
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            bitmap.Save(savepath);
            //释放资源
            bitmap.Dispose();
        } 
        #endregion

        #region 坐标转换
        public PixPoint LonlatToMercator(double X, double Y)
        {
            PixPoint p = new PixPoint();
            p.X = X * 20037508.34 / 180;
            double y = Math.Log(Math.Tan((90 + Y) * Math.PI / 360)) / (Math.PI / 180);
            p.Y = y * 20037508.34 / 180;
            return p;
        }

        public PixPoint MercatorToLonlat(double X, double Y)
        {
            PixPoint p = new PixPoint();
            var x = X / 20037508.34 * 180;
            var y = Y / 20037508.34 * 180;
            y = 180 / Math.PI * (2 * Math.Atan(Math.Exp(y * Math.PI / 180)) - Math.PI / 2);
            p.X = x;
            p.Y = y;
            return p;
        } 
        #endregion
    }
    /// <summary>
    /// 点对象
    /// </summary>
    public class PixPoint
    {
        public double X{get;set;}
        public double Y{get;set;}
    }
}
