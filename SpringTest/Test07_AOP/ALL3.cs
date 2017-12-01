using AopAlliance.Intercept;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//http://www.cnblogs.com/GoodHelper/archive/2009/11/16/SpringNet_Aop_Config.html
namespace Test07_AOP
{
    public class AroundAdvice1 : IMethodInterceptor
    {
        public object Invoke(IMethodInvocation invocation)
        {
            Console.WriteLine("开始:  " + invocation.TargetType.Name + "." + invocation.Method.Name);
            object result = invocation.Proceed();
            Console.WriteLine("结束:  " + invocation.TargetType.Name + "." + invocation.Method.Name);
            return result;
        }
    }
    public interface IService
    {
        IList FindAll();

        void Save(object entity);
    }

    public class CategoryService : IService
    {
        public IList FindAll()
        {
            return new ArrayList();
        }

        public void Save(object entity)
        {
            Console.WriteLine("保存：" + entity);
        }
    }

    public class ProductService : IService
    {
        public IList FindAll()
        {
            return new ArrayList();
        }

        public void Save(object entity)
        {
            Console.WriteLine("保存：" + entity);
        }
    }

    public class ConsoleDebugAttribute : Attribute
    {
        /*
        <object id="aroundAdvisor" type="Spring.Aop.Support.AttributeMatchMethodPointcutAdvisor, Spring.Aop">
            <property name="Advice" ref="aroundAdvice"/>
            <property name="Attribute" value="ConfigAttribute.Attributes.ConsoleDebugAttribute, ConfigAttribute" />
         </object>
          <object id="proxyFactoryObject" type="Spring.Aop.Framework.ProxyFactoryObject">
            <property name="Target">
              <object type="ConfigAttribute.Service.AttributeService, ConfigAttribute" />
            </property>
            <property name="InterceptorNames">
              <list>
                <value>aroundAdvisor</value>
              </list>
            </property>
          </object>
          <object id="aroundAdvice" type="Common.AroundAdvice, Common"/>
         */
    }

    public class AttributeService : IService
    {
        [ConsoleDebug]
        public IList FindAll()
        {
            return new ArrayList();
        }

        public void Save(object entity)
        {
            Console.WriteLine("保存：" + entity);
        }
    }
}
