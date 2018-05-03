using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace APP
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button_add_Click(object sender, EventArgs e)
        {
            string key = textBox_key.Text;
            string val = textBox_val.Text;
            RedisHelper.StringAppend(key, val);
            textBox_msg.AppendText("添加完成 \n");
        }

        private void button_edit_Click(object sender, EventArgs e)
        {
            string key = textBox_key.Text;
            string val = textBox_val.Text;
            RedisHelper.StringAppend(key, val);
            textBox_msg.AppendText("编辑完成 \n");
        }

        private void button_del_Click(object sender, EventArgs e)
        {
            string key = textBox_key.Text;
            
            textBox_msg.AppendText("删除完成 \n");
        }

        private void button_queryS_Click(object sender, EventArgs e)
        {
            string key = textBox_key.Text;
            string result= RedisHelper.StringGet(key).ToString();
            textBox_msg.AppendText("获取的值："+ result + " \n");
        }

        private void button_queryAll_Click(object sender, EventArgs e)
        {
            
            textBox_msg.AppendText("获取完成\n");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox_msg.Text = "";
        }

        private void button2_Click(object sender, EventArgs e)
        {

            textBox_msg.AppendText("添加完成 \n");
        }

        private void button_delall_Click(object sender, EventArgs e)
        {
            
        }
    }
}
