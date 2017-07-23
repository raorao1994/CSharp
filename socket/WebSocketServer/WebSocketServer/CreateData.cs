using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace WebSocketServer
{
    public class CreateData
    {
        public static string Createsend()
        {
            int s = new Random().Next(-20, 20);
            double a = s * 1.0 / 10;
            double aaa = 1 + a;
            if (aaa <= 0) aaa = 0;
            var model = new { a = aaa, b = 23 + a };
            JavaScriptSerializer json = new JavaScriptSerializer();
            string ss = json.Serialize(model);
            byte[] bMsg = System.Text.Encoding.UTF8.GetBytes(ss);//消息使用UTF-8编码
            Console.WriteLine("发送数据置客户端:" + "氧气含量：" + model.b + "% \t 可燃气体含量：" + model.a + "%\n");
            return ss;
        }
    }
}
