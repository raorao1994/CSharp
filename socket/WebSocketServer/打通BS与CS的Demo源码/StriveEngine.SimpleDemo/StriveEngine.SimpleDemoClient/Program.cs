using System;
using System.Collections.Generic;
using System.Windows.Forms;

/*
 * 本demo采用的是StriveEngine的免费版本，若想获取StriveEngine其它版本，请联系 www.oraycn.com 或 QQ：168757008。
 * 
 */
namespace StriveEngine.SimpleDemoClient
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
