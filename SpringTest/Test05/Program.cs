using Spring.Context;
using Spring.Context.Support;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test05
{
    class Program
    {
        static IApplicationContext context = null;
        static void Main(string[] args)
        {
            string[] xmlFiles = new string[] {
                "assembly://Test05/Test05/Objects1.xml",
                "assembly://Test05/Test05/List.xml",
                "assembly://Test05/Test05/Objects.xml"
            };
            context = new XmlApplicationContext(xmlFiles);
            //依赖注入(应用篇)
            test1();
            //依赖对象的注入(基础篇)
            //test2();
            //集合类型的注入(基础篇)
            //test3();
            Console.ReadLine();
        }

        private static void test3()
        {
            PersonList person = context.GetObject("PersonList") as PersonList;

            Console.WriteLine("空值");
            string bestFriend = person.Spears == null ? "我的矛" : "我只有一个矛";
            Console.WriteLine(bestFriend);
            Console.WriteLine();

            Console.WriteLine("IList");
            foreach (var item in person.HappyYears)
            {
                Console.WriteLine(item);
            }
            Console.WriteLine();

            Console.WriteLine("泛型Ilist<int>");
            foreach (int item in person.Years)
            {
                Console.WriteLine(item);
            }
            Console.WriteLine();

            Console.WriteLine("IDictionary");
            foreach (DictionaryEntry item in person.HappyDic)
            {
                Console.WriteLine(item.Key + " 是 " + item.Value);
            }
            Console.WriteLine();

            Console.WriteLine("泛型IDictionary<string,object>");
            foreach (KeyValuePair<string, object> item in person.HappyTimes)
            {
                Console.WriteLine(item.Key + " 是 " + item.Value);
            }
        }

        private static void test2()
        {
            string[] xmlFiles = new string[] { "assembly://Test05/Test05/Objects1.xml" };
            PersonDao dao = context.GetObject("personDao") as PersonDao;
            dao.Get();
        }

        static void test1()
        {
            Person person = (Person)context.GetObject("modernPerson");
            person.Work();
        }
    }
}
