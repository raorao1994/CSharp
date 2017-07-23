using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISHelper.Model
{
    /// <summary>
    /// 面
    /// </summary>
    public class Polygon
    {
        public Polygon()
        {
            rings = new List<Rings>();
        }
        public List<Rings> rings { get; set; }
    }
    public class Rings
    {
        public Rings()
        {
            points = new List<Point>();
        }
            
        public List<Point> points { get; set; }
    }
}
