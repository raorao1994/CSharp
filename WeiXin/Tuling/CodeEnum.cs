using System;

namespace Tuling
{
    public enum CodeEnum
    {
        //以下为正常时返回的数据代码
        /// <summary>
        /// 文本
        /// </summary>
        Text = 100000,
        /// <summary>
        /// 网址
        /// </summary>
        Url = 200000,
        /// <summary>
        /// 新闻
        /// </summary>
        News = 302000,
        /// <summary>
        /// 应用、软件、下载
        /// </summary>
        AppSoftDownload = 304000,
        /// <summary>
        /// 火车
        /// </summary>
        Train = 305000,
        /// <summary>
        /// 航班
        /// </summary>
        Flight = 306000,
        /// <summary>
        /// 菜谱、视频、小说
        /// </summary>
        CookVideoNovel = 308000,
        /// <summary>
        /// 酒店
        /// </summary>
        Hotel = 309000,
        /// <summary>
        /// 价格
        /// </summary>
        Price = 311000,
        //以下为错误时返回的错误代码
        /// <summary>
        /// key长度错误
        /// </summary>
        KeyLengthError = 40001,
        /// <summary>
        /// 请求内容为空
        /// </summary>
        InfoError = 40002,
        /// <summary>
        /// key或者账户错误
        /// </summary>
        KeyOrAccountError = 40003,
        /// <summary>
        /// 请求次数已用尽
        /// </summary>
        RequestUseUp = 40004,
        /// <summary>
        /// 不支持的功能
        /// </summary>
        Unsupport = 40005,
        /// <summary>
        /// 服务器升级中
        /// </summary>
        ServerUpgrading = 40006,
        /// <summary>
        /// 服务器数据格式异常
        /// </summary>
        ServerDataError = 40007
    }
}
