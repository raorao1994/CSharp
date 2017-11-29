using System;

namespace Test01
{
    public class PersonDao : IPersonDao
    {
        public void Save()
        {
            Console.Write("保运");
        }
    }
}