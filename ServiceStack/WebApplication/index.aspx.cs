using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ServiceStack;
using ServiceStack.Redis;
using App.Model;

namespace WebApplication
{
    public partial class index : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(TextBox1.Text);
            string name = TextBox2.Text;
            RedisResponseModel model = new RedisResponseModel();
            model.ID = id;
            model.Name = name;
            var client = new RedisClient("127.0.0.1", 6379);
            bool b=client.Set<RedisResponseModel>(id.ToString(), model);
            Label1.Text = "结果"+b.ToString();
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            string id = TextBox3.Text;
            var client = new RedisClient("127.0.0.1", 6379);
            RedisResponseModel b = client.Get<RedisResponseModel>(id);
            Label2.Text = "结果:" + b.Name;
        }
    }
}