using System;

namespace PublicAccount.Semantic
{
    /// <summary>
    /// 通用指令操作值：STANDARD(标准模式)，MUTE(静音模式)，VIBRA(振动模式)，INAIR(飞行模式)，RING(铃声)，
    /// WALLPAPER(壁纸)，TIME(时间)，WIFI(无线网络)，BLUETOOTH(蓝牙)，GPS(GPS)，NET(移动网络)，
    /// SPACE(存储空间)，INPUT(输入法设置)，LANGUAGE(语言设置)，PERSONAL(个性化设置)，
    /// SCREEN(屏幕保护)，FACTORY_SETTINGS(出厂设置)，SI(系统信息)，UPDATE(系统更新)，
    /// PLAY(播放)，OPEN_MUSIC(开机音乐)，TIME_ON(定时开机)，TIME_OFF(定时关机)
    /// </summary>
    public enum InstructionValueEnum
    {
        /// <summary>
        /// 标准模式
        /// </summary>
        STANDARD,
        /// <summary>
        /// 静音模式
        /// </summary>
        MUTE,
        /// <summary>
        /// 振动模式
        /// </summary>
        VIBRA,
        /// <summary>
        /// 飞行模式
        /// </summary>
        INAIR,
        /// <summary>
        /// 铃声
        /// </summary>
        RING,
        /// <summary>
        /// 壁纸
        /// </summary>
        WALLPAPER,
        /// <summary>
        /// 时间
        /// </summary>
        TIME,
        /// <summary>
        /// 无线网络
        /// </summary>
        WIFI,
        /// <summary>
        /// 蓝牙
        /// </summary>
        BLUETOOTH,
        /// <summary>
        /// GPS
        /// </summary>
        GPS,
        /// <summary>
        /// 移动网络
        /// </summary>
        NET,
        /// <summary>
        /// 存储空间
        /// </summary>
        SPACE,
        /// <summary>
        /// 输入法设置
        /// </summary>
        INPUT,
        /// <summary>
        /// 语言设置
        /// </summary>
        LANGUAGE,
        /// <summary>
        /// 个性化设置
        /// </summary>
        PERSONAL,
        /// <summary>
        /// 屏幕保护
        /// </summary>
        SCREEN,
        /// <summary>
        /// 出厂设置
        /// </summary>
        FACTORY_SETTINGS,
        /// <summary>
        /// 系统信息
        /// </summary>
        SI,
        /// <summary>
        /// 系统更新
        /// </summary>
        UPDATE,
        /// <summary>
        /// 播放
        /// </summary>
        PLAY,
        /// <summary>
        /// 开机音乐
        /// </summary>
        OPEN_MUSIC,
        /// <summary>
        /// 定时开机
        /// </summary>
        TIME_ON,
        /// <summary>
        /// 定时关机
        /// </summary>
        TIME_OFF
    }
}