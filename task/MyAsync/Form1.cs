using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyAsync
{
    /// <summary>
    /// 1 进程、线程、同步、异步的概念
    /// 2 回顾委托，开始异步
    /// 3 异步的三大特点
    /// 
    /// 1 异步的回调和状态参数
    /// 2 异步等待三种方式
    /// 3 获取异步的返回值
    /// 
    /// 1 回顾委托和异步多线程
    /// 2 通过Task启动多线程
    /// 3 解决多线程几大应用场景
    /// </summary>
    public partial class frmAsync : Form
    {
        public frmAsync()
        {
            Console.WriteLine("欢迎来到.net高级班公开课之核心语法特训，今天是Eleven老师为大家带来的异步多线程1");
            Console.WriteLine("欢迎来到.net高级班公开课之核心语法特训，今天是Eleven老师为大家带来的异步多线程2");
            Console.WriteLine("欢迎来到.net高级班公开课之核心语法特训，今天是Eleven老师为大家带来的Task");
            InitializeComponent();
        }

        #region 同步方法
        /// <summary>
        /// 同步方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSync_Click(object sender, EventArgs e)
        {
            Console.WriteLine();
            Console.WriteLine("******************同步方法开始，线程id：{0}", Thread.CurrentThread.ManagedThreadId);
            int j = 0;
            int k = 1;
            int m = j + k;
            for (int i = 0; i < 5; i++)
            {
                string name = string.Format("{0}_{1}", "btnSync_Click", i);
                this.DoSomethingLong(name);
            }

            Console.WriteLine("******************btnSync_Click 同步方法 end   {0}********************", Thread.CurrentThread.ManagedThreadId);
            Console.WriteLine();
        }


        /// <summary>
        /// 委托的异步调用
        /// 1 同步方法卡界面，原因是主线程(UI)忙于计算；异步多线程方法不卡界面，原因是启动了多个子线程运算，主线程已闲置
        /// 2 同步方法慢，原因是只有一个线程工作；异步多线程方法快，原因是多线程并发运算，但是会消耗更多的资源，不是越多越好
        /// 3 异步多线程的无序性：启动顺序不确定 执行时间不确定 结束顺序也不确定，所以不要试图控制线程顺序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAsync_Click(object sender, EventArgs e)
        {
            Console.WriteLine();
            Console.WriteLine("******************btnAsync_Click 异步方法 start {0}********************", Thread.CurrentThread.ManagedThreadId);

            NoReturnOnePara method = new NoReturnOnePara(this.DoSomethingLong);//2 实例化委托
            for (int i = 0; i < 5; i++)
            {
                string name = string.Format("{0}_{1}", "btnAsync_Click", i);
                //method.Invoke(name);//同步
                method.BeginInvoke(name, null, null);//委托的异步调用
            }


            Console.WriteLine("******************btnAsync_Click 异步方法 end   {0}********************", Thread.CurrentThread.ManagedThreadId);
            Console.WriteLine();
        }

        #endregion

        private delegate void NoReturnOnePara(string name);//1 委托声明

        #region 异步
        /// <summary>
        /// 1 异步的回调和状态参数
        /// 2 异步等待三种方式
        /// 3 获取异步的返回值
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAsyncAdvanced_Click(object sender, EventArgs e)
        {
            Console.WriteLine();
            Console.WriteLine("******************btnAsyncAdvanced_Click 异步方法 start {0}********************", Thread.CurrentThread.ManagedThreadId);

            NoReturnOnePara method = new NoReturnOnePara(this.DoSomethingLong);
            //AsyncCallback callback = new AsyncCallback(this.PrivateCallback);
            //callback.Invoke()

            IAsyncResult iAsyncResult = method.BeginInvoke("btnAsyncAdvanced_Click",
                t =>
                {
                    method.EndInvoke(t);
                    Console.WriteLine("异步调用开始执行。。");
                    Console.WriteLine("这里是PrivateCallback  线程id={0}", Thread.CurrentThread.ManagedThreadId.ToString("00"));
                }, "莲花未开时");

            //int i = 0;
            //while (!iAsyncResult.IsCompleted)
            ////1 完成线程等待  
            ////2 等待的时候还可以做别的操作
            ////3 等待有性能损失，可能多等待一下
            //{
            //    if (i < 10)
            //    {
            //        Console.WriteLine("中华民族复兴已完成{0}%。。", i++ * 10);
            //    }
            //    else
            //    {
            //        Console.WriteLine("中华民族已经复兴了，颤栗吧，凡人！！");
            //    }
            //    Thread.Sleep(200);
            //}
            //Console.WriteLine("大中华民族崛起了。。。。");

            //iAsyncResult.AsyncWaitHandle.WaitOne();//永久等待
            //iAsyncResult.AsyncWaitHandle.WaitOne(-1);//永久等待
            //iAsyncResult.AsyncWaitHandle.WaitOne(500);//只等500ms，超时就不等了

            //method.EndInvoke(iAsyncResult);//永久等待


            //Func<int> method = new Func<int>(() => DateTime.Now.Day);
            //int iResult = method.Invoke();

            //IAsyncResult iAsyncResult = method.BeginInvoke(null, null);
            //int iResultAsync = method.EndInvoke(iAsyncResult);



            Console.WriteLine("异步调用后执行1。。");
            Console.WriteLine("异步调用后执行2。。");
            Console.WriteLine("异步调用后执行3。。");

            Console.WriteLine("******************btnAsyncAdvanced_Click 异步方法 end   {0}********************", Thread.CurrentThread.ManagedThreadId);
            Console.WriteLine();
        }

        private void PrivateCallback(IAsyncResult iAsyncResult)
        {
            Console.WriteLine("异步调用后执行。。");
            Console.WriteLine("这里是PrivateCallback  线程id={0}", Thread.CurrentThread.ManagedThreadId.ToString("00"));
        }

        #endregion

        //委托异步调用  thread threadpool task parallel await/async
        #region 回顾委托和异步多线程
        /// <summary>
        /// Task
        /// 1 回顾委托和异步多线程
        /// 2 通过Task启动多线程
        /// 3 解决多线程几大应用场景
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTask_Click(object sender, EventArgs e)
        {
            Console.WriteLine();
            Console.WriteLine("******************btnTask_Click 异步方法 start {0}********************", Thread.CurrentThread.ManagedThreadId);

            //Action act = new Action(() =>
            //{
            //    this.DoSomethingLong("123");
            //});
            //Task task = Task.Run(act);

            Console.WriteLine("Eleven老师接到一个项目");
            Console.WriteLine("沟通需求，谈妥价格");
            Console.WriteLine("签合同，收取50%费用");
            Console.WriteLine("高级班筛选学员，组建团队");
            Console.WriteLine("需求分析，系统设计，模块划分");
            Console.WriteLine("开始coding");

            TaskFactory taskFactory = new TaskFactory();
            List<Task> taskList = new List<Task>();

            //taskList.Add(taskFactory.StartNew(() => this.Coding("落单的候鸟", "Protal")));
            //taskList.Add(taskFactory.StartNew(() => this.Coding("莲花未开时", "Client")));
            //taskList.Add(taskFactory.StartNew(() => this.Coding("一生有意义", " WCF ")));
            //taskList.Add(taskFactory.StartNew(() => this.Coding("  一点半  ", "WechatClient")));
            //taskList.Add(taskFactory.StartNew(() => this.Coding("  大明   ", "BackService")));
            //taskList.Add(taskFactory.StartNew(() => this.Coding("  李健健 ", "   DB   ")));


            taskList.Add(taskFactory.StartNew(t => this.Coding("落单的候鸟", "Protal"), "落单的候鸟"));
            taskList.Add(taskFactory.StartNew(t => this.Coding("莲花未开时", "Client"), "莲花未开时"));
            taskList.Add(taskFactory.StartNew(t => this.Coding("一生有意义", " WCF "), "一生有意义"));
            taskList.Add(taskFactory.StartNew(t => this.Coding("  一点半  ", "WechatClient"), "一点半"));
            taskList.Add(taskFactory.StartNew(t => this.Coding("  大明   ", "BackService"), "大明"));
            taskList.Add(taskFactory.StartNew(t => this.Coding("  李健健 ", "   DB   "), "李健健"));

            //谁最先完成，就去部署环境  而且有红包奖励
            taskFactory.ContinueWhenAny(taskList.ToArray(), t =>
                {
                    Console.WriteLine(t.AsyncState);
                    Console.WriteLine("最先完成，就去部署环境  而且有红包奖励,id={0}", Thread.CurrentThread.ManagedThreadId);
                });
            //大家都完成，一起联调测试
            taskList.Add(taskFactory.ContinueWhenAll(taskList.ToArray(), tList => Console.WriteLine("大家都完成，一起联调测试")));

            //taskFactory.StartNew(() =>
            //    {
            //        for (int i = 0; i < 100; i++)
            //        {
            //            Thread.Sleep(100);
            //            Console.WriteLine("老师一直发钱，大家一直收钱");
            //        }
            //    });
            //CancellationTokenSource

            //Eleven线程 应该等着某人完成后
            Task.WaitAny(taskList.ToArray());//表示当前线程(Eleven线程)等着任何一个task的完成
            Console.WriteLine("某个人完成任务后，开始测试，收取费用20%");

            Task.WaitAll(taskList.ToArray());//表示当前线程(Eleven线程)等着全部task的完成
            Console.WriteLine("六人都完成任务后，联调测试验收，收取剩余30%");

            Console.WriteLine("大家分分分分分钱。。。。");
            Console.WriteLine("******************btnTask_Click 异步方法 end   {0}********************", Thread.CurrentThread.ManagedThreadId);
            Console.WriteLine();
        }

        /// <summary>
        /// coding 也是需要时间和体力的
        /// </summary>
        /// <param name="name"></param>
        /// <param name="project"></param>
        private void Coding(string name, string project)
        {
            Console.WriteLine("******************{0} Coding {1} start {2} {3}********************",
               name, project, Thread.CurrentThread.ManagedThreadId.ToString("00"), DateTime.Now.ToString("HHmmss:fff"));

            long lResult = 0;
            for (int i = 0; i < 1000000000; i++)
            {
                lResult += i;
            }

            Console.WriteLine("******************{0} Coding {1}   end {2} {3}********************",
                name, project, Thread.CurrentThread.ManagedThreadId.ToString("00"), DateTime.Now.ToString("HHmmss:fff"), lResult);

        }

        #endregion

        #region 一个耗时耗资源的测试方法
        /// <summary>
        /// 一个耗时耗资源的测试方法
        /// </summary>
        /// <param name="name"></param>
        private void DoSomethingLong(string name)
        {
            Console.WriteLine("******************耗时方法开始运行 {0} {1} {2}*",
                name, Thread.CurrentThread.ManagedThreadId.ToString("00"), DateTime.Now.ToString("HHmmss:fff"));

            long lResult = 0;
            for (int i = 0; i < 1000000000; i++)
            {
                lResult += i;
            }

            Console.WriteLine("******************耗时方法结束{0} {1} {2} {3}",
                name, Thread.CurrentThread.ManagedThreadId.ToString("00"), DateTime.Now.ToString("HHmmss:fff"), lResult);

        }
        #endregion




    }
}
