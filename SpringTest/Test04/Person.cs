using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test04
{
    public class Person
    {
        public Person()
        {
            Console.WriteLine("Person被实例");
        }

        public override string ToString()
        {
            return "我是Person";
        }

        ~Person()
        {
            Console.WriteLine("Person被销毁");
        }
    }
}
