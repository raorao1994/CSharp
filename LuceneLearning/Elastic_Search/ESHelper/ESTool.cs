using Nest;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Elastic_Search.ESHelper
{
    public class ESTool
    {
        /// <summary>
        /// 逐条更新/插入索引
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="obj">数据类型</param>
        /// <param name="Index">索引为空则为默认索引</param>
        /// <returns>插入是否成功</returns>
        public bool PopulateIndex<T>(T obj, string Index = "DefaultIndex") where T : class, new()
        {
            //获取类名
            string strDocType = typeof(T).ToString();
            strDocType = strDocType.Split('.').Last().ToLower();
            //获取ES_ID值
            long ES_ID = 0;
            PropertyInfo Property = typeof(T).GetProperty("ES_ID");
            ES_ID = (long)Property.GetValue(obj);
            //index
            if (Index == "DefaultIndex")
                Index = ESProvider.DefaultIndex;
            var index = ESProvider.Client.Index(obj,
                i => i.Index(Index).Type(strDocType).Id(ES_ID));
            return index.IsValid;
        }

        /// <summary>
        /// 批量更新/插入索引
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="Objs">数据</param>
        /// <returns>是否执行成功</returns>
        public bool BulkPopulateIndex<T>(List<T> Objs, string Index = "DefaultIndex") where T : class, new()
        {
            //获取类名
            string strDocType = typeof(T).ToString();
            strDocType = strDocType.Split('.').Last().ToLower();
            //获取ES_ID值
            long ES_ID = 0;
            PropertyInfo Property = typeof(T).GetProperty("ES_ID");
            //index
            if (Index == "DefaultIndex")
                Index = ESProvider.DefaultIndex;
            var bulkRequest = new BulkRequest(Index, strDocType) { Operations = new List<IBulkOperation>() };
            //遍历
            var list = Objs.Select(o =>
            {
                ES_ID = (long)Property.GetValue(o);
                return new BulkIndexOperation<T>(o) { Id = ES_ID };
            });
            var idxops = list.Cast<IBulkOperation>().ToList();
            bulkRequest.Operations = idxops;
            var response = ESProvider.Client.Bulk(bulkRequest);
            return response.IsValid;
        }

        //public void Save()
        //{
        //    IEnumerable<Person> persons = new List<Person>
        //    {
        //        new Person()
        //        {
        //            Id = "0",
        //            Firstname = "chunhui",
        //            Lastname = "zhao",
        //            Chains = new string[]{ "a","b","c" },
        //            Content = "dad"
        //        },
        //        new Person()
        //        {
        //            Id = "1",
        //            Firstname = "keke",
        //            Lastname = "zhao",
        //            Chains = new string[]{ "x","y","z" },
        //            Content = "daughter"
        //        }
        //    };
        //    ESProvider.Client.IndexMany<Person>(persons, ESProvider.DefaultIndex);
        //}

        public bool Search()
        {
            //var rs = ESProvider.Client.Search<Person>(s => s.Index(ESProvider.DefaultIndex));
            var rs = ESProvider.Client.Search<Person>(s => s.Index(ESProvider.DefaultIndex).Type("person"));
            Console.WriteLine(JsonConvert.SerializeObject(rs.Documents));
            if (rs.Total > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<T> SearchAllType<T>(string Index = "DefaultIndex") where T : class, new()
        {
            //获取类名
            string type = typeof(T).ToString();
            //index
            if (Index == "DefaultIndex")
                Index = ESProvider.DefaultIndex;
            ISearchRequest request = new SearchRequest(Index);



            var rs = ESProvider.Client.Search<T>(request);
            Console.WriteLine(JsonConvert.SerializeObject(rs.Documents));
            return rs.Documents.ToList<T>();
        }

        public void Query()
        {
            string strDocType = typeof(Person).ToString();
            strDocType = strDocType.Split('.').Last().ToLower();
            SearchRequest request = new SearchRequest(ESProvider.DefaultIndex, strDocType);
            TermQuery tq = new TermQuery();
            tq.Field = "ES_ID";
            tq.Value = "0";
            request.Query = tq;
            request.Size = 100;
            request.TypedKeys = false;
            var response=ESProvider.Client.Search<Elastic_Search.Person>(request);
            List<Elastic_Search.Person> ResultList = response.Documents.ToList();
        }
    }
}
