using Elastic_Search.ESHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elastic_Search
{
    public class Person: IEsModel
    {
        public string Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string[] Chains { get; set; }
        public string Content { get; set; }

        public long ES_ID{get;set;}
    }
}
