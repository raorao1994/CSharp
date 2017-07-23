using GISHelper.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISHelper
{
    /// <summary>
    /// 图形分析帮助类
    /// </summary>
    public class PolyHelper
    {
        /// <summary>
        /// 判断点是否在面内
        /// </summary>
        /// <param name="poly"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public static bool pointInPolygon(Polygon poly, Point p)
        {
            int polySides = poly.rings.Count;
            //待测点
            double x = p.X;
            double y = p.Y;
            int i;
            int j = polySides - 1;
            bool oddNodes = false;
            //水平坐标角
            List<double> polyX = new List<double>();
            //竖坐标角
            List<double> polyY = new List<double>();

            foreach (var ring in poly.rings)
            {
                foreach (var point in ring.points)
                {
                    polyX.Add(point.X);
                    polyY.Add(point.Y);
                }
            }
            for (i = 0; i < polySides; i++)
            {
                if ((polyY[i] < y && polyY[j] >= y
                || polyY[j] < y && polyY[i] >= y)
                && (polyX[i] <= x || polyX[j] <= x))
                {
                    oddNodes ^= (polyX[i] + (y - polyY[i]) / (polyY[j] - polyY[i]) * (polyX[j] - polyX[i]) < x);
                }
                j = i;
            }
            return oddNodes;
        }
    }
}
