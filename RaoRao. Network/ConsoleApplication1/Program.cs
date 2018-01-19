using RaoRao.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            ServerScanner serverScanner = new ServerScanner();
            View_Loaded(serverScanner);
            StartScan(serverScanner);
            Console.ReadKey();
        }
        private static void View_Loaded(ServerScanner serverScanner)
        {
            // 在界面 Load 事件中添加以下代码
            serverScanner.OnScanComplete += ServerScanner_OnScanComplete;
            serverScanner.OnScanProgressChanged += ServerScanner_OnScanProgressChanged;

            // 扫描的端口号
            serverScanner.ScanPort = 80;
        }

        private static void StartScan(ServerScanner serverScanner)
        {
            // 开始扫描
            serverScanner.StartScan();
        }


        private static void ServerScanner_OnScanComplete(object sender, List<ConnectionResult> e)
        {
            foreach (var item in e)
            {
                Console.WriteLine("ip地址："+item.Address.ToString());
            }
            
        }

        private static void ServerScanner_OnScanProgressChanged(object sender, ScanProgressEventArgs e)
        {
            Console.Write(e.ToString());
        }
    }
}
