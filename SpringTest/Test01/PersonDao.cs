using System;

namespace Test01
{
    public class PersonDao : IPersonDao
    {
        public void Save()
        {
            Console.Write("我是第一个Person类");
        }
    }
    public class PersonDao1 : IPersonDao
    {
        public void Save()
        {
            Console.Write("我是第二个Person类");
        }
    }
}