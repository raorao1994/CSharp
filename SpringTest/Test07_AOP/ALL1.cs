using AopAlliance.Intercept;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test07_AOP
{

    public interface ISecurityManager
    {
        bool IsPass(string userName);
    }

    public class SecurityManager : ISecurityManager
    {
        /// <summary>
        /// 判断权限
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public bool IsPass(string userName)
        {
            return userName == "admin";
        }
    }

    public class CompanyDao
    {
        public void Save()
        {
            Console.WriteLine("保存数据");
        }
    }

    public interface ICompanyManager
    {
        string UserName { get; set; }

        void Save();
    }

    public class CompanyManager : ICompanyManager
    {
        #region 可通过外部注入的属性

        public string UserName { get; set; }

        public CompanyDao Dao { get; set; }

        #endregion

        public void Save()
        {
            //执行业务方法
            //.
            //调用DAO层方法
            Dao.Save();
        }
    }

    //public class CompanyProxyManager : ICompanyManager
    //{
    //    public string UserName { get; set; }

    //    private ICompanyManager target = new CompanyManager();

    //    public void Save()
    //    {
    //        //判断权限
    //        ISecurityManager security = new SecurityManager();
    //        if (security.IsPass(UserName))
    //        {
    //            //调用目标对象Save方法
    //            target.Save();
    //        }
    //        else
    //        {
    //            Console.WriteLine("您没有该权限");
    //        }
    //    }
    //}

    public class AroundAdvice : IMethodInterceptor
    {
        //权限系统类(可外部注入)
        private ISecurityManager manager = new SecurityManager();

        public object Invoke(IMethodInvocation invocation)
        {
            //拦截Save方法
            if (invocation.Method.Name == "Save")
            {
                ICompanyManager target = (ICompanyManager)invocation.Target;

                return manager.IsPass(target.UserName) ? invocation.Proceed() : null;
            }
            else
            {
                return invocation.Proceed();
            }
        }
    }
}
