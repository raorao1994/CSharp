using Nest;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elastic_Search.ESHelper
{
    /// <summary>
    /// ES实例
    /// </summary>
    public static class ESProvider
    {
        private static ElasticClient esClient = null;
        private static string _DefaultIndex = "";
        private static string url = "";
        /// <summary>
        /// 初始化
        /// </summary>
        static ESProvider()
        {
            url = ConfigurationManager.AppSettings["ESUrl"];
            _DefaultIndex = ConfigurationManager.AppSettings["ESIndex"];
            var node = new Uri(url);
            ConnectionSettings settings = new ConnectionSettings(node);
            settings.DefaultIndex(_DefaultIndex);
            esClient = new ElasticClient(settings);
        }
        /// <summary>
        /// 默认索引
        /// </summary>
        public static string DefaultIndex
        {
            get {
                return _DefaultIndex;
            }
        }
        /// <summary>
        /// 默认ES客户端
        /// </summary>
        public static string IPAddress
        {
            get
            {
                return url;
            }
        }
        public static ElasticClient Client
        {
            get
            {
                return esClient;
            }
        }
    }
}
