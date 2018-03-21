using ControlPhone.helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace ControlPhone
{
    public partial class Form1 : Form
    {
        public ProcessHelper p;
        public string AdbExePath;
        public Form1()
        {
            InitializeComponent();
            String cmd = Application.StartupPath + "\\adb";
            AdbExePath = cmd;
            p = new ProcessHelper(cmd);
           p.mProcess.Start();
           p.mProcess.StandardInput.WriteLine("cd /d " + AdbExePath);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //p.mProcess.Start();
            //p.mProcess.StandardInput.WriteLine("cd /d " + AdbExePath);
            //string command = "adb shell screencap -p sdcard/screen.png";
            DateTime start,end;
            //while (true)
            //{
                start = DateTime.Now;
                string result = p.Run("adb shell screencap -p sdcard/screen.png", "adb pull sdcard/screen.png c:/");
                //p.mProcess.StandardInput.WriteLine("adb shell screencap -p sdcard/screen.png"); 
                //p.mProcess.StandardInput.WriteLine("adb pull sdcard/screen.png c:/");
                end = DateTime.Now;
                textBox1.Text += (end- start).TotalSeconds +"----";
            //}
        }
    }
}
