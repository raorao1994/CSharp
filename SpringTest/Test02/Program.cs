using Spring.Context;
using Spring.Context.Support;
using Spring.Core.IO;
using Spring.Objects.Factory;
using Spring.Objects.Factory.Xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/*
 * 读取Object配置文件信息
 */
namespace Test02
{
    class Program
    {
        static void Main(string[] args)
        {
            AppRegistry();
            //XmlSystem();
            //FileSystem();
            Console.ReadLine();
        }

        static void AppRegistry()
        {
            IApplicationContext ctx = ContextRegistry.GetContext();
            Console.WriteLine(ctx.GetObject("PersonDao").ToString());
        }

        static void XmlSystem()
        {
            //需要把XML文件嵌入程序集
            string[] xmlFiles = new string[]
            {
                //file://文件名
　　            //assembly://程序集名/命名空名/文件名
                //"file://Objects.xml"  //, 文件名
                "assembly://Test02/Test02/Object.xml"  //程序集
            };
            IApplicationContext context = new XmlApplicationContext(xmlFiles);

            IObjectFactory factory = (IObjectFactory)context;
            Console.WriteLine(factory.GetObject("PersonDao").ToString());
        }

        static void FileSystem()
        {
            IResource input = new FileSystemResource(@"E:\Github\C#\trunk\SpringTest\Test02\Object.xml");  //实际物理路径
            IObjectFactory factory = new XmlObjectFactory(input);
            Console.WriteLine(factory.GetObject("PersonDao").ToString());
        }
    }
}
