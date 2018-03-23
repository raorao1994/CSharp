using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Model;

namespace App.Service
{
    /// <summary>
    /// 服务
    /// </summary>
    public class APPServer: ServiceStack.Service
    {
        public object Get(RequestModel model)
        {
            ResponseModel model1 = new ResponseModel();
            model1.msg = "App" + model.ID + ":" + model.Name;
            model1.state = "200";
            return model1;
        }
    }
    public class WebServer : ServiceStack.Service
    {
        public object Get(WebRequestModel model)
        {
            ResponseModel model1 = new ResponseModel();
            model1.msg = "Web"+model.ID + ":" + model.Name;
            model1.state = "200";
            return model1;
        }
    }
}
