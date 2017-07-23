using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceStackAPP
{
    class Program
    {
        static void Main(string[] args)
        {
            string url = "http://*:61890/";
            var apphost = new Service.Host.AppTickServiceHost().Init().Start(url);
            Console.Write("开始监听："+url);
            Console.ReadKey();
        }
    }
}
