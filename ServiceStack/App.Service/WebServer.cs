using App.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Service
{
    public class WebServer : ServiceStack.Service
    {
        public object Get(WebRequestModel model)
        {
            ResponseModel model1 = new ResponseModel();
            model1.msg = "Web" + model.ID + ":" + model.Name;
            model1.state = "200";
            return model1;
        }
        public object Post(WebRequestModel model)
        {
            ResponseModel model1 = new ResponseModel();
            model1.msg = "Web" + model.ID + ":" + model.Name;
            model1.state = "200";
            return model1;
        }
    }
}
