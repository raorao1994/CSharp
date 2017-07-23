using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tools;

namespace QzoneWebSpider
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
            //HttpHelper.GetURLData("https://user.qzone.qq.com/914204907");
            Application.Run(new Form2());
        }
    }
}
