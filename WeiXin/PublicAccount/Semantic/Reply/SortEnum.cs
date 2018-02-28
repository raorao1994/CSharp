using System;

namespace PublicAccount.Semantic
{
    /// <summary>
    /// 排序类型：0 距离（默认），1 点评高优先级，2 服务质量高优先级，3 环境高优先级， 4 价格高到低， 5价格低到高
    /// </summary>
    public enum SortEnum
    {
        距离 = 0,
        点评高优先级 = 1,
        服务质量高优先级 = 2,
        环境高优先级 = 3,
        价格高到低 = 4,
        价格低到高 = 5
    }
}
