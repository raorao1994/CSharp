using Spring.Context;
using Spring.Context.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/*
 * 我们使用Spring.NET框架经常用到的一下几个文件：
*Common.Logging.dll(必要)
*Spring.Core.dll(必要)
*Spring.Data.dll
*Spring.Aop.dll(可选)
*Spring.Data.NHibernate21.dll
*Spring.Web.dll
*在以后的博客里我们会学习一些与NHibernate和Asp.NET MVC结合的例子，可以到Hibernate的官方网站和Asp.NET的官方网站下载各自的框架安装文件。
*控制反转（Inversion of Control，英文缩写为IoC），也叫依赖注入（Dependency Injection）。
*/
namespace Test01
{
    class Program
    {
        static void Main(string[] args)
        {
            //NormalMethod();  // 一般方法
            //FactoryMethod();  // 工厂方法
            IoCMethod();  // IoC方法"
            Console.ReadLine();
        }
        private static void NormalMethod()
        {
            IPersonDao dao = new PersonDao();
            dao.Save();
            Console.WriteLine("我是一般方法");
        }

        private static void FactoryMethod()
        {
            IPersonDao dao = DataAccess.CreatePersonDao();
            dao.Save();
            Console.WriteLine("我是工厂方法");
        }

        private static void IoCMethod()
        {
            IApplicationContext ctx = ContextRegistry.GetContext();
            IPersonDao dao = ctx.GetObject("p1") as IPersonDao;
            if (dao != null)
            {
                dao.Save();
                Console.WriteLine("我是IoC方法");
            }
        }
    }
}
