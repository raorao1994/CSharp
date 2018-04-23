using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elastic_Search.ESHelper
{
    public interface IEsModel
    {
        long ES_ID { set; get; }
    }
}
