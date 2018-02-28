using System;

namespace PublicAccount.Semantic
{
    /// <summary>
    /// 意图
    /// </summary>
    public enum IntentEnum
    {
        /// <summary>
        /// 普通查询
        /// </summary>
        SEARCH,
        /// <summary>
        /// 路线查询
        /// </summary>
        ROUTE,
        /// <summary>
        /// 价格查询
        /// </summary>
        PRICE,
        /// <summary>
        /// 旅游攻略查询
        /// </summary>
        GUIDE,
        /// <summary>
        /// 把声音调到 number 或者 operator
        /// </summary>
        VOLUME_SET,
        /// <summary>
        /// 把亮度调到 number 或者 operator
        /// </summary>
        BRIGHTNESS_SET,
        /// <summary>
        /// 对 value 设置操作为 open/close，value 包括：标准模式(STANDARD)，静音模式(MUTE)，振动模式(VIBRA)，飞行模式(INAIR)
        /// </summary>
        MODEL,
        /// <summary>
        /// （1）对通用指令 value 设置操作为 open/close，value 包括：铃声设置(RING)，壁纸设置(WALLPAPER)，时间设置(TIME) ， WIFI(WIFI) ，蓝牙(BLUETOOTH) ，GPS(GPS)，移动网络(NET)；
        /// （2）对电视指令 value 设置为：IMAGE(图像设置)，SCREEN(屏幕比例)，SOUND(声音模式)，IMAGE_MODEL(图像模式)
        /// </summary>
        SETTING,
        /// <summary>
        /// 扫码
        /// </summary>
        SCAN,
        /// <summary>
        /// 上一个
        /// </summary>
        UP,
        /// <summary>
        /// 下一个
        /// </summary>
        DOWN,
        /// <summary>
        /// 向左
        /// </summary>
        LEFT,
        /// <summary>
        /// 向右
        /// </summary>
        RIGHT,
        /// <summary>
        /// 上一页
        /// </summary>
        PAGE_UP,
        /// <summary>
        /// 下一页
        /// </summary>
        PAGE_DOWN,
        /// <summary>
        /// 暂停
        /// </summary>
        PAUSE,
        /// <summary>
        /// 停止
        /// </summary>
        STOP,
        /// <summary>
        /// 播放
        /// </summary>
        PLAY,
        /// <summary>
        /// 播放列表
        /// </summary>
        PLAY_LIST,
        /// <summary>
        /// 循环播放
        /// </summary>
        LOOP,
        /// <summary>
        /// 随机播放
        /// </summary>
        RANDOM,
        /// <summary>
        /// 单曲播放
        /// </summary>
        SINGLE,
        /// <summary>
        /// 快进
        /// </summary>
        FF,
        /// <summary>
        /// 快退
        /// </summary>
        FB,
        /// <summary>
        /// 退出
        /// </summary>
        QUIT,
        /// <summary>
        /// （1）对通用指令：返回；
        /// （2）对电视指令：回看。
        /// </summary>
        BACK,
        /// <summary>
        /// 关闭
        /// </summary>
        CLOSE,
        /// <summary>
        /// 主页
        /// </summary>
        HOME,
        /// <summary>
        /// 确认
        /// </summary>
        YES,
        /// <summary>
        /// 取消
        /// </summary>
        NO,
        /// <summary>
        /// 刷新
        /// </summary>
        REFRESH,
        /// <summary>
        /// 菜单
        /// </summary>
        MENU,
        /// <summary>
        /// 截屏
        /// </summary>
        SCREENSHOT,
        /// <summary>
        /// 重启
        /// </summary>
        REBOOT,
        /// <summary>
        /// 关机
        /// </summary>
        SHUTDOWN,
        /// <summary>
        /// 换台
        /// </summary>
        CHANGE_NAME,
        /// <summary>
        /// 换频道，分为具体频道（例如，探索频道）和数字频道（例如：5 台）
        /// </summary>
        CHANGE_CHANNEL,
        /// <summary>
        /// 频道列表，operator 操作为 open/close
        /// </summary>
        CHANNEL_LIST,
        /// <summary>
        /// 电视开关，operator 操作为 open/close/standby(开机，关机，待机)
        /// </summary>
        TV,
        /// <summary>
        /// 信源选择为 value：3D，VIDEO(视频)，VIDEO1(视频1)，VIDEO2(视频 2)，HDMI，HDMI1，HDMI2，DVI，
        /// VGA，USB，ANALOG(模拟电视)，DIGITAL(数字电视)
        /// </summary>
        SOURCH,
        /// <summary>
        /// 调整台序
        /// </summary>
        CHANNEL_ORDER,
        /// <summary>
        /// 自动搜台
        /// </summary>
        AUTO_SEARCH,
        /// <summary>
        /// 节目单
        /// </summary>
        PROGRAM_LIST,
        /// <summary>
        /// 预约列表
        /// </summary>
        ORDER_LIST,
        /// <summary>
        /// 空调设置
        /// </summary>
        AIR_CONDITION,
        /// <summary>
        /// 窗户设置
        /// </summary>
        WINDOW,
        /// <summary>
        /// 查看胎压
        /// </summary>
        TIRE,
        /// <summary>
        /// 对如下设备进行 OPEN/CLOSE 操作：bluetooth(蓝牙)，radio(收音机)，guide(导航)，U(U 盘)，wiper(雨刷)
        /// </summary>
        DEVICE,
        /// <summary>
        /// 打开
        /// </summary>
        OPEN,
        /// <summary>
        /// 安装
        /// </summary>
        INSTALL,
        /// <summary>
        /// 卸载
        /// </summary>
        UNSTALL,
        /// <summary>
        /// 下载
        /// </summary>
        DOWNLOAD,
        /// <summary>
        /// 查看
        /// </summary>
        CHECK
    }
}