using System;

namespace PublicAccount
{
    /// <summary>
    /// 微信菜单类型
    /// </summary>
    public enum MenuTypeEnum
    {
        //只能用程序创建的菜单类型
        /// <summary>
        /// 点击推事件
        /// </summary>
        click,
        /// <summary>
        /// 扫码推事件
        /// </summary>
        scancode_push,
        /// <summary>
        /// 扫码推事件且弹出“消息接收中”提示框
        /// </summary>
        scancode_waitmsg,
        /// <summary>
        /// 弹出系统拍照发图
        /// </summary>
        pic_sysphoto,
        /// <summary>
        /// 弹出拍照或者相册发图
        /// </summary>
        pic_photo_or_album,
        /// <summary>
        /// 弹出微信相册发图器
        /// </summary>
        pic_weixin,
        /// <summary>
        /// 弹出地理位置选择器
        /// </summary>
        location_select,

        //可用程序或者公众号后台创建的菜单类型
        /// <summary>
        /// 跳转url
        /// </summary>
        view,

        //只能在公众号后台创建的菜单类型
        /// <summary>
        /// 文本
        /// </summary>
        text,
        /// <summary>
        /// 图片
        /// </summary>
        img,
        /// <summary>
        /// 视频
        /// </summary>
        video,
        /// <summary>
        /// 语音
        /// </summary>
        voice,
        /// <summary>
        /// 图文
        /// </summary>
        news
    }
}
