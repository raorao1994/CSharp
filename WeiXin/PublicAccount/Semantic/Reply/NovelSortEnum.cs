using System;

namespace PublicAccount.Semantic
{
    /// <summary>
    /// 小说排序类型：0 排序无要求（默认），1 热度高优先级，2 时间升序，3时间降序
    /// </summary>
    public enum NovelSortEnum
    {
        排序无要求 = 0,
        热度高优先级 = 1,
        时间升序 = 2,
        时间降序 = 3
    }
}