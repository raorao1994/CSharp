using Spring.Objects.Factory.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Test06
{
    public abstract class Parent
    {
        public string Name { get; set; }
    }

    public class Child
    {
        public string Name { get; set; }
    }
    public class Dog
    {
        public void Init()
        {
            Console.WriteLine("DOG baby");
        }

        public void Destroy()
        {
            Console.WriteLine("DOG die");
        }
    }
    //注意，可以直接在配置中定义这个类的对象
    public abstract class ObjectFactory1
    {
        //或者可以是一个虚方法    
        public abstract PersonDao1 CreatePersonDao();
    }

    public class PersonDao1
    {
        public void Save()
        {
            Console.WriteLine("保存数据");
        }
    }

    public class UserDao
    {
        //虚方法
        public virtual string GetValue(string input)
        {
            return null;
        }
    }

    //实现IMethodReplacer接口
    public class ReplaceValue : IMethodReplacer
    {
        public object Implement(object target, MethodInfo method, object[] arguments)
        {
            string value = (string)arguments[0];
            return "获取到：" + value;
        }
    }
    //先定义一个委托
    public delegate string OpenHandler(string arg);

    public class Door
    {
        public event OpenHandler OpenTheDoor;

        public void OnOpen(string arg)
        {
            //调用事件
            if (OpenTheDoor != null)
            {
                Console.WriteLine(OpenTheDoor(arg));
            }
        }
    }

    public class Men
    {
        public string OpenThisDoor(string arg)
        {
            return "参数是：" + arg;
        }
    }
}
