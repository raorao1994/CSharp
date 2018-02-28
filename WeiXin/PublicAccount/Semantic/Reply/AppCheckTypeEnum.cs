using System;

namespace PublicAccount.Semantic
{
    /// <summary>
    /// 应用查看的类型：install（已安装），buy（已购买），update（可更新），latest（最近运行的），home（主页）
    /// </summary>
    public enum AppCheckTypeEnum
    {
        /// <summary>
        /// 已安装
        /// </summary>
        install,
        /// <summary>
        /// 已购买
        /// </summary>
        buy,
        /// <summary>
        /// 可更新
        /// </summary>
        update,
        /// <summary>
        /// 最近运行的
        /// </summary>
        latest,
        /// <summary>
        /// 主页
        /// </summary>
        home
    }
}