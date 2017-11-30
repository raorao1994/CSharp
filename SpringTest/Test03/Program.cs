using Spring.Context;
using Spring.Context.Support;
using Spring.Objects.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test03
{
    class Program
    {
        static void Main(string[] args)
        {
            //CreateByConstructor();  // 构造器创建
            //CreateNested();  // 嵌套类型创建
            //CreateByStaticFactory();  // 静态工厂创建
            //CreateByInstanceFactory();  // 实例工厂创建
            CreateGenericClass();  // 创建泛型

            Console.ReadLine();
        }
        /// <summary>
        /// 构造器创建
        /// </summary>
        static void CreateByConstructor()
        {
            string[] xmlFiles = new string[]
            {
                "assembly://Test03/Test03/Objects.xml"
            };
            IApplicationContext context = new XmlApplicationContext(xmlFiles);

            IObjectFactory factory = (IObjectFactory)context;
            Console.WriteLine(factory.GetObject("personDao").ToString());
        }

        /// <summary>
        /// 嵌套类型创建
        /// </summary>
        static void CreateNested()
        {
            string[] xmlFiles = new string[]
            {
                "assembly://Test03/Test03/Objects.xml"
            };
            IApplicationContext context = new XmlApplicationContext(xmlFiles);

            IObjectFactory factory = (IObjectFactory)context;
            Console.WriteLine(factory.GetObject("person").ToString());
        }

        /// <summary>
        /// 静态工厂创建
        /// </summary>
        static void CreateByStaticFactory()
        {
            string[] xmlFiles = new string[]
            {
                "assembly://Test03/Test03/Objects.xml"
            };
            IApplicationContext context = new XmlApplicationContext(xmlFiles);

            IObjectFactory factory = (IObjectFactory)context;
            Console.WriteLine(factory.GetObject("staticObjectsFactory").ToString());
        }

        /// <summary>
        /// 实例工厂创建
        /// </summary>
        static void CreateByInstanceFactory()
        {
            string[] xmlFiles = new string[]
            {
                "assembly://Test03/Test03/Objects.xml"
            };
            IApplicationContext context = new XmlApplicationContext(xmlFiles);

            IObjectFactory factory = (IObjectFactory)context;
            Console.WriteLine(factory.GetObject("instancePersonDao").ToString());
        }

        /// <summary>
        /// 创建泛型
        /// </summary>
        static void CreateGenericClass()
        {
            string[] xmlFiles = new string[]
            {
                "assembly://Test03/Test03/Objects.xml"
            };
            IApplicationContext context = new XmlApplicationContext(xmlFiles);

            IObjectFactory factory = (IObjectFactory)context;

            object obj = factory.GetObject("genericClass");

            Console.WriteLine(obj.ToString());
        }
    }
}
