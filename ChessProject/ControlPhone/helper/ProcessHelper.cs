using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace ControlPhone.helper
{
    public class ProcessHelper
    {
        public  Process mProcess;
        private  string AdbExePath = "";
        /// <summary>
        /// 静态构造函数
        /// </summary>
        /// <param name="AdbexePath">ADB文件路径</param>
        public ProcessHelper(string AdbexePath)
        {
            AdbExePath = AdbexePath;
            mProcess = GetProcess();
        }
        /// <summary>
        /// 获取Process对象
        /// </summary>
        /// <returns></returns>
        private Process GetProcess()
        {
            mProcess = new Process();
            mProcess.StartInfo.CreateNoWindow = true;
            mProcess.StartInfo.UseShellExecute = false;
            mProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            mProcess.StartInfo.RedirectStandardInput = true;
            mProcess.StartInfo.RedirectStandardError = true;
            mProcess.StartInfo.RedirectStandardOutput = true;
            mProcess.StartInfo.StandardOutputEncoding = Encoding.UTF8;
            mProcess.StartInfo.FileName = "cmd.exe";
            return mProcess;
        }
        /// <summary>
        /// 执行adb语句
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public string Run(string command, string command1)
        {
            string result = "false";
            try
            {
                mProcess.Start();
                mProcess.StandardInput.WriteLine("cd /d "+ AdbExePath);
                mProcess.StandardInput.WriteLine(command); //也可以用這種方式輸入要執行的命令
                mProcess.StandardInput.WriteLine(command1); //也可以用這種方式輸入要執行的命令
                mProcess.StandardInput.WriteLine("exit"); //不過要記得加上Exit要不然下一行程式執行的時候會當機
                result=mProcess.StandardOutput.ReadToEnd(); //從輸出流取得命令執行結果
            }
            catch (Exception ex)
            {
                result = "false";
            }
            return result;
        }
    }
}
