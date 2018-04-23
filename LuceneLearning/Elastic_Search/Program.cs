using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Elastic_Search.ESHelper;

namespace Elastic_Search
{
    class Program
    {
        static void Main(string[] args)
        {
            ESTool tools = new ESTool();
            tools.Search();

            var p = new Person()
            {
                Id = "4",
                ES_ID=4,
                Firstname = "chunhui",
                Lastname = "zhao",
                Chains = new string[] { "a", "b", "c" },
                Content = "dad"
            };
            tools.PopulateIndex<Person>(p);

            tools.Query();
           
            Console.ReadKey();
        }
    }
}
