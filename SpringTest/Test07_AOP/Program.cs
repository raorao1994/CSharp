using Spring.Aop.Framework;
using System;
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
            ICompanyManager target = new CompanyManager() { Dao = new CompanyDao(), UserName = "admin" };
            ProxyFactory factory = new ProxyFactory(target);
            factory.AddAdvice(new AroundAdvice());
            ICompanyManager manager = (ICompanyManager)factory.GetProxy();
            manager.Save();

            Console.ReadLine();
        }
    }
}
