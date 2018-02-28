using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WeiXin
{
    public partial class index : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            RaoRao.Log.LogOperater.Debug(new Exception(), "index.aspx点击了");
            this.Label1.Text = "点击了";
            string str = AppDomain.CurrentDomain.BaseDirectory;
            this.Label2.Text = str;
        }
    }
}