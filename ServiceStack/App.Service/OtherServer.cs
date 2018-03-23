using App.Model;
using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Service
{
    /// <summary>
    /// 服务
    /// </summary>
    public class OtherServer : ServiceStack.Service
    {
        public object Get(OtherRequestModel model)
        {
            ResponseModel model1 = new ResponseModel();
            model1.msg = "App" + model.ID + ":" + model.Name;
            model1.state = "200";
            return model1;
        }
    }
}
