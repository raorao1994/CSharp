using Spring.Context;
using Spring.Context.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test05
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] xmlFiles = new string[]
            {
                "assembly://Test05/Test05/Objects.xml"
            };
            IApplicationContext ctx = new XmlApplicationContext(xmlFiles);
            //IApplicationContext ctx = ContextRegistry.GetContext();
            Person person = (Person)ctx.GetObject("modernPerson");
            person.Work();

            Console.ReadLine();
        }
    }
}
