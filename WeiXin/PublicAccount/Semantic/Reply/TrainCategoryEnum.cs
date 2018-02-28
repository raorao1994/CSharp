using System;

namespace PublicAccount.Semantic
{
    /// <summary>
    /// 车次类型：G（高铁），D（动车），T（特快），K（快速），Z（直达），L（临时客车），P（普通）
    /// </summary>
    public enum TrainCategoryEnum
    {
        /// <summary>
        /// 高铁
        /// </summary>
        G,
        /// <summary>
        /// 动车
        /// </summary>
        D,
        /// <summary>
        /// 特快
        /// </summary>
        T,
        /// <summary>
        /// 快速
        /// </summary>
        K,
        /// <summary>
        /// 直达
        /// </summary>
        Z,
        /// <summary>
        /// 临时客车
        /// </summary>
        L,
        /// <summary>
        /// 普通
        /// </summary>
        P
    }
}