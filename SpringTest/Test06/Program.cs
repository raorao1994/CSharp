using Spring.Context;
using Spring.Context.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test06
{
    class Program
    {
        static void Main(string[] args)
        {
            ObjectFactory factory = ObjectFactory.Instance(@"E:\Github\C#\trunk\SpringTest\Test06\Objects.xml");
            MyClass ccccc = (MyClass)factory.GetObject("MyClass");
            ccccc.func();
            //打造简易的依赖注入框架(练习篇)
            //test1();
            //方法的注入(基础篇)
            //test2();
            //自定义对象行为(基础篇) 
            test3();
            Console.ReadLine();
        }

        private static void test3()
        {
            string[] xmlFiles = new string[] {
                "assembly://Test06/Test06/Objects2.xml"
            };
            IApplicationContext context = new XmlApplicationContext(xmlFiles);

            Dog d = context.GetObject("dog") as Dog;
            //继承了parent
            Child c = context.GetObject("child") as Child;
            Console.WriteLine(c.Name);
        }

        private static void test2()
        {
            //IApplicationContext ctx = ContextRegistry.GetContext();

            string[] xmlFiles = new string[] {
                "assembly://Test06/Test06/Objects2.xml"
            };

            IApplicationContext ctx =new  XmlApplicationContext(xmlFiles);

            Console.WriteLine("查询方法");
            ObjectFactory1 factory = (ObjectFactory1)ctx.GetObject("objectFactory1");
            factory.CreatePersonDao().Save();
            Console.WriteLine();

            Console.WriteLine("替换方法");
            UserDao dao = (UserDao)ctx.GetObject("userDao");
            Console.WriteLine(dao.GetValue("Liu Dong"));
            Console.WriteLine();

            Console.WriteLine("事件注册");
            Door door = (Door)ctx.GetObject("door");
            door.OnOpen("Opening!");
            Console.WriteLine();

        }

        //自实现ioc
        private static void test1()
        {
            ObjectFactory factory = ObjectFactory.Instance(@"E:\Github\C#\trunk\SpringTest\Test06\Objects.xml");
            PersonDao dao = (PersonDao)factory.GetObject("personDao");
            Console.WriteLine("姓名：" + dao.Entity.Name);
            Console.WriteLine("年龄：" + dao.Entity.Age);
            Console.WriteLine(dao);
        }
    }
}
