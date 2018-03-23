using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ServiceStack;
using ServiceStack.Redis;

namespace WebApplication
{
    public partial class index : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //var client = new RedisClient("127.0.0.1", 6379);
            //client.Set<int>("pwd", 1111);
            //int pwd = client.Get<int>("pwd");
            //Console.WriteLine(pwd);
        }
    }
}