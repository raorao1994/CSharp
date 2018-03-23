using App.Model;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace APP.Host
{
    /// <summary>
    /// 服务主机
    /// </summary>
    public class AppHost:AppHostBase
    {
        //加载一个程序集
        public static Assembly[] assemblys = {Assembly.Load("App.Service") };
        //告诉seviceStack你的服务程序名及如何找到你的服务程序
        public AppHost() 
        : base("Hello Web Services", assemblys) { }
        public override void Configure(Funq.Container container)
        {
            //注册你的服务所使用的任何依赖
            //container.Register<ICacheClient>(new MemoryCacheClient());
        }
    }
}
