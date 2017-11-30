using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test03
{
    public class InstanceObjectsFactory
    {
        public PersonDao CreateInstance()
        {
            return new PersonDao();
        }
    }
}
