using System;
using Newtonsoft.Json.Linq;

namespace PublicAccount.Semantic
{
    /// <summary>
    /// 排序类型：0 较快捷（默认），1 少换乘，2 少步行
    /// </summary>
    public enum RouteSortTypeEnum
    {
        较快捷 = 0,
        少换乘 = 1,
        少步行 = 2
    }
}