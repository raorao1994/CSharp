using System;

namespace PublicAccount.Semantic
{
    /// <summary>
    /// 音乐排序类型：0 排序无要求（默认），1 时间升序，2 时间降序，3 热度高优先级
    /// </summary>
    public enum MusicSortEnum
    {
        排序无要求 = 0,
        时间升序 = 1,
        时间降序 = 2,
        热度高优先级 = 3
    }
}