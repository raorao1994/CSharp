using Spring.Context;
using Spring.Context.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/*
 控制反转（Inversion of Control，英文缩写为IoC），也叫依赖注入（Dependency Injection）。
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
            IPersonDao dao = ctx.GetObject("PersonDao") as IPersonDao;
            if (dao != null)
            {
                dao.Save();
                Console.WriteLine("我是IoC方法");
            }
        }
    }
}
