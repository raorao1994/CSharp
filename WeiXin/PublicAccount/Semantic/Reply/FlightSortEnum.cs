using System;

namespace PublicAccount.Semantic
{
    /// <summary>
    /// 航班排序类型：0 排序无要求（默认），1 价格升序，2 价格降序，3 时间升序，4 时间降序
    /// </summary>
    public enum FlightSortEnum
    {
        排序无要求 = 0,
        价格升序 = 1,
        价格降序 = 2,
        时间升序 = 3,
        时间降序 = 4
    }
}