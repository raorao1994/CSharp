using Spring.Context;
using Spring.Context.Support;
using Spring.Objects.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test04
{
    class Program
    {
        static void Main(string[] args)
        {
            CreateWithSingleton();
            CreateWithOutSingleton();
            Console.Read();
        }
        //单例模式
        public static void CreateWithSingleton()
        {
            Console.WriteLine("单例模式");
            string[] xmlFiles = new string[]
            {
                "assembly://Test04/Test04/Objects.xml"
            };
            IApplicationContext context = new XmlApplicationContext(xmlFiles);

            IObjectFactory factory = (IObjectFactory)context;
            object obj1 = factory.GetObject("personDao");
            object obj2 = factory.GetObject("personDao");
            bool d = obj1.Equals(obj2);
            Console.WriteLine(d);
        }
        //非单例模式
        public static void CreateWithOutSingleton()
        {
            Console.WriteLine("非单例模式");
            string[] xmlFiles = new string[]
            {
                "assembly://Test04/Test04/Objects.xml"
            };
            IApplicationContext context = new XmlApplicationContext(xmlFiles);

            IObjectFactory factory = (IObjectFactory)context;
            object obj1 = factory.GetObject("person");
            object obj2 = factory.GetObject("person");
            bool d = obj1.Equals(obj2);
            Console.WriteLine(d);
        }
    }
}
