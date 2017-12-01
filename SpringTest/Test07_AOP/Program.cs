using Spring.Aop.Framework;
using Spring.Context;
using Spring.Context.Support;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test07_AOP
{
    class Program
    {
        static void Main(string[] args)
        {
            //test1();
           
            //test2();

            test3();
            Console.ReadLine();
        }

        private static void test3()
        {
            IApplicationContext ctx = ContextRegistry.GetContext();
            IDictionary speakerDictionary = ctx.GetObjectsOfType(typeof(IService));
            foreach (DictionaryEntry entry in speakerDictionary)
            {
                string name = (string)entry.Key;
                IService service = (IService)entry.Value;
                Console.WriteLine(name + " 拦截： ");

                service.FindAll();

                Console.WriteLine();

                service.Save("数据");

                Console.WriteLine();
            }

        }

        private static void test1()
        {
            ICompanyManager target = new CompanyManager() { Dao = new CompanyDao(), UserName = "admin" };
            ProxyFactory factory = new ProxyFactory(target);
            factory.AddAdvice(new AroundAdvice());
            ICompanyManager manager = (ICompanyManager)factory.GetProxy();
            manager.Save();
        }

        private static void test2()
        {
            ProxyFactory factory = new ProxyFactory(new OrderService());

            factory.AddAdvice(new AroundAdvise());
            factory.AddAdvice(new BeforeAdvise());
            factory.AddAdvice(new AfterReturningAdvise());
            factory.AddAdvice(new ThrowsAdvise());

            IOrderService service = (IOrderService)factory.GetProxy();

            object result = service.Save(110);

            Console.WriteLine("---------------------------------------");
            Console.WriteLine(string.Format("客户端返回值：{0}", result));
        }
    }
}
