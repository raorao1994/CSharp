using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace 计算线外一点在直线上的垂足
{
    class Program
    {
        static void Main(string[] args)
        {
            calculat();
            //cacult(0, 0, 5, 5, 1.1, 50);
            string datastr = File.ReadAllText(@"C:\Users\chenxw\Desktop\data.txt");
            JavaScriptSerializer json = new JavaScriptSerializer();
            List<Line> objects = json.Deserialize(datastr, typeof(List<Line>)) as List<Line>;
            Dictionary<int, LineData> Data = GetMyData(objects);
            string s = json.Serialize(Data.Values.ToList());
            string path=GetPath(Data, 1702, 1607);
        }

        private static void calculat()
        {
            int i = 200;
            DateTime d1 = DateTime.Now;
            while (true)
            {
                if (i == 0) break;
                Thread.Sleep(20);//Ticks = 20056     Ticks = 62506655
                i--;
            }
            //while (i != 0)
            //{
            //    Thread.Sleep(20);//Ticks = 20044   Ticks = 60725219
            //    i--;
            //}
            DateTime d2 = DateTime.Now;
            var s = d2 - d1;
        }

        /// <summary>
        /// 计算线外一点
        /// </summary>
        /// <param name="X1"></param>
        /// <param name="Y1"></param>
        /// <param name="X2"></param>
        /// <param name="Y2"></param>
        /// <param name="X3"></param>
        /// <param name="Y3"></param>
        public static void cacult(double X1, double Y1, double X2, double Y2, double X3, double Y3)
        {
            double X, Y;
            //求爆管点位置
            if ((X2 - X1) != 0 && (Y2 - Y1) != 0)
            {
                double a = (Y2 - Y1) / (X2 - X1);
                double b1 = Y1 - a * X1;
                double b2 = Y3 + (1 / a) * X3;
                X = ((b2 - b1) * a) / (a + 1);
                Y = a * X + b1;
            }
            else if ((X2 - X1) == 0)
            {
                X = X1;
                Y = Y3;
            }
            else if ((Y2 - Y1) == 0)
            {
                X = X3;
                Y = Y1;
            }
        }
        /// <summary>
        /// 获取路径分析数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        static Dictionary<int, LineData> GetMyData(List<Line> data)
        {

            Dictionary<int, LineData> list = new Dictionary<int, LineData>();
            //1.0 获取第一个对象并加入字典
            var d = new LineData() { Point = data[0].FOID.ToString(), LineObj = data[0] };
            list.Add(data[0].FOID, d);
            //2.0 设置对比对象
            LineData temp = d;
            int abc = 0;
            //3.0 正向循环遍历数据
            foreach (var item in data)
            {
                if (abc == 0) { abc++; continue; }

                //3.1 如果属于下一接点
                if (item.Exp2 == temp.LineObj.Exp1)
                {
                    abc++;
                    LineData _model = new LineData() { Point = temp.Point + "/" + item.FOID.ToString(), LineObj = item };
                    list.Add(item.FOID, _model);
                    temp = _model;
                }
                //3.2 如果不属于下一接点，那么查看是否属于列表中某一结点的接点
                else
                {
                    abc++;
                    //3.2.1 获取所有键值
                    List<int> keys = list.Keys.ToList();
                    bool islink = false;
                    //3.2.2 反向遍历所有已添加的值
                    for (int i = keys.Count - 1; i >= 0; i--)
                    {
                        //3.2.2.1 判断是否能跟当前遍历对象链接上
                        LineData _data = list[keys[i]];
                        if (item.Exp2 == _data.LineObj.Exp1)
                        {
                            LineData _model1 = new LineData() { Point = _data.Point + "/" + item.FOID.ToString(), LineObj = item };
                            list.Add(item.FOID, _model1);
                            temp = _model1;
                            islink = true;
                            break;
                        }
                    }
                    if (!islink)
                    {
                        LineData _model1 = new LineData() { Point = item.FOID.ToString(), LineObj = item };
                        list.Add(item.FOID, _model1);
                        temp = _model1;
                    }
                }
            }
            return list;
        }
        /// <summary>
        /// 获取正确路径值
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        static string GetPath(Dictionary<int, LineData> data,int start,int stop)
        {
            if (CheckLine.Contains(stop)) return null;
            string Path = "";
            //1.0 获取到结尾点的路径
            LineData stopObj = data[stop];
            Path = stopObj.Point;
            //2.0 判断结尾点路径是否存在起始点
            if (Path.IndexOf(start.ToString()+"/") > -1)
            {
                CheckLine.Add(stop);
                return Path;
            }
            //3.0 如果不存在起始路径的话，则便利前一路径节点的是否存在起始路径
            if (Path.IndexOf("/") < 0)
            {
                CheckLine.Add(stop);
                return "";
            }
            string[] Paths = Path.Split('/');
            for (int i=0;i<Paths.Length-2; i++)
            {
                string item = Paths[i];
                //3.1 判断前一节点是否存在其实路径
                LineData temp = data[Convert.ToInt16(item)];
                //3.1.1 存在其实路径
                if (temp.Point.IndexOf(start.ToString() + "/") > -1)
                {
                    CheckLine.Add(stop);
                    Path += temp.Point;
                    return Path;
                }
                //3.1.2 不存在起始路径，则继续向下一节点进行遍历
                else
                {
                    try
                    {
                        Path = Path.Substring(item.Length + 1);
                    }
                    catch (Exception ex) { Path = ""; }
                    if (temp.Point.IndexOf("/") > 0)
                    {
                        string s= GetPath(data, start, Convert.ToInt16(item));
                        Path += s;
                        if (s!=null)
                        {
                            CheckLine.Add(Convert.ToInt16(item));
                            return Path;
                        }
                    }
                }
            }
            CheckLine.Add(Convert.ToInt16(stop));
            return null;
        }
        /// <summary>
        /// 获取以判断的管线
        /// </summary>
        static List<int> CheckLine = new List<int>();
    }

    [Serializable]
    public class Line
    {
        public string Exp1 { set; get; }
        public string Exp2 { set; get; }
        public double Deep1 { set; get; }
        public double Deep2 { set; get; }
        public string Ds { set; get; }
        public string Material { set; get; }
        public string DType { set; get; }
        public string Belong { set; get; }
        public string RoadName { set; get; }
        public string MDate { set; get; }
        public string Pressure { set; get; }
        public string Voltage { set; get; }
        public double No { set; get; }
        public int FOID { set; get; }
        public string TypeName { set; get; }
        public string TypeCode { set; get; }
        public string SubTypeName { set; get; }
        public string SubTypeCode { set; get; }
        public string LayerTableName { set; get; }
        public string LayerName { set; get; }
    }
    [Serializable]
    public class LineData
    {
        /// <summary>
        /// 路径
        /// </summary>
        public string Point { get; set; }
        /// <summary>
        /// 管线对象
        /// </summary>
        public Line LineObj { get; set; }
    }
}
