using AopAlliance.Intercept;
using Spring.Aop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Test07_AOP
{
    /// <summary>
    /// 环绕通知
    /// </summary>
    public class AroundAdvise : IMethodInterceptor
    {
        public object Invoke(IMethodInvocation invocation)
        {
            Console.WriteLine("---------------------------------------");
            object returnValue = null;
            try
            {
                returnValue = invocation.Proceed();
            }
            catch
            {
                Console.WriteLine(" 环绕通知： 发生异常");
            }
            Console.Out.WriteLine(string.Format(" 环绕通知:调用的方法 '{0}',返回值{1}", invocation.Method.Name,returnValue.ToString()));
            return returnValue;
        }
    }

    /// <summary>
    /// 前置通知
    /// </summary>
    public class BeforeAdvise : IMethodBeforeAdvice
    {
        public void Before(MethodInfo method, object[] args, object target)
        {
            Console.WriteLine("---------------------------------------");
            Console.Out.WriteLine("前置通知：调用的方法名 : " + method.Name);
            Console.Out.WriteLine("前置通知：目标       : " + target);
            Console.Out.WriteLine("前置通知：参数为   : ");
            if (args != null)
            {
                foreach (object arg in args)
                {
                    Console.Out.WriteLine("\t: " + arg);
                }
            }
        }
    }

    /// <summary>
    /// 异常通知
    /// </summary>
    public class ThrowsAdvise : IThrowsAdvice
    {
        public void AfterThrowing(Exception ex)
        {
            Console.WriteLine("---------------------------------------");
            string errorMsg = string.Format("异常通知： 方法抛出的异常 : {0}", ex.Message);
            Console.Error.WriteLine(errorMsg);
        }
    }
    /// <summary>
    /// 后置通知
    /// </summary>
    public class AfterReturningAdvise : IAfterReturningAdvice
    {
        public void AfterReturning(object returnValue, MethodInfo method, object[] args, object target)
        {
            Console.WriteLine("---------------------------------------");
            Console.Out.WriteLine("后置通知： 方法调用成功，方法名 : " + method.Name);
            Console.Out.WriteLine("后置通知： 目标为      : " + target);
            Console.Out.WriteLine("后置通知： 参数 : ");
            if (args != null)
            {
                foreach (object arg in args)
                {
                    Console.Out.WriteLine("\t: " + arg);
                }
            }
            Console.Out.WriteLine("后置通知：  返回值是 : " + returnValue);
        }
    }
    public interface IOrderService
    {
        object Save(object id);
    }
    public class OrderService : IOrderService
    {
        /// <summary>
        /// 拦截该方法
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public object Save(object id)
        {
            Console.WriteLine("---------------------------------------");
            //throw new Exception("由于XXX原因保存出错");  
            return "保存：" + id.ToString();
        }
    }

}
