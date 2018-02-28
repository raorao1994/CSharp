using System;

namespace PublicAccount
{
    /// <summary>
    /// 用户来源
    /// </summary>
    public enum UserSourceEnum
    {
        /// <summary>
        /// 0代表其他
        /// </summary>
        Other=0,
        /// <summary>
        /// 17代表名片分享
        /// </summary>
        CardShare=17,
        /// <summary>
        /// 30代表扫二维码
        /// </summary>
        ScanQrCode=30,
        /// <summary>
        /// 35代表搜号码（即微信添加朋友页的搜索）
        /// </summary>
        SearchNumber=35,
        /// <summary>
        /// 39代表查询微信公众帐号
        /// </summary>
        SearchAccount=39,
        /// <summary>
        /// 43代表图文页右上角菜单
        /// </summary>
        NewsMenu=43
    }
}
