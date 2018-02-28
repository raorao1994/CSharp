using System;
using Newtonsoft.Json.Linq;

namespace PublicAccount.Semantic
{
    /// <summary>
    /// 出行方式：walk（步行）, taxi（打车） , bus （公交） , subway （地铁） , drive（自驾）
    /// </summary>
    public enum RouteTypeEnum
    {
        /// <summary>
        /// 步行
        /// </summary>
        walk,
        /// <summary>
        /// 打车
        /// </summary>
        taxi,
        /// <summary>
        /// 公交
        /// </summary>
        bus,
        /// <summary>
        /// 地铁
        /// </summary>
        subway,
        /// <summary>
        /// 自驾
        /// </summary>
        drive
    }
}