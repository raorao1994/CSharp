using System;

namespace PublicAccount.Semantic
{
    /// <summary>
    /// 语义服务类型
    /// </summary>
    [Flags]
    public enum ServiceTypeEnum : long
    {
        //生活类
        /// <summary>
        /// 餐馆  查询餐馆的服务，例如：中关村附近的面馆
        /// </summary>
        restaurant = 0x1,
        /// <summary>
        /// 地图  查询地图服务，例如：从银科大厦到天坛公园怎么走
        /// </summary>
        map = 0x2,
        /// <summary>
        /// 周边  查询周边的服务，例如：我想去打保龄球
        /// </summary>
        nearby = 0x4,
        /// <summary>
        /// 优惠券/团购  查询优惠券/团购的服务，例如： “附近有什么优惠券”；如果查已有类别的优惠券，比如：“附近有什么酒店优惠券”，那么就会优先是酒店类。
        /// </summary>
        coupon = 0x8,
        //旅行类
        /// <summary>
        /// 酒店  查询酒店服务，例如：查一下中关村附近有没有七天酒店
        /// </summary>
        hotel = 0x10,
        /// <summary>
        /// 旅游  查询旅游服务，例如：故宫门票多少钱
        /// </summary>
        travel = 0x20,
        /// <summary>
        /// 航班  查询航班的服务，例如：明天从北京到上海的机票
        /// </summary>
        flight = 0x40,
        /// <summary>
        /// 火车  查询火车服务，例如：查一下从北京到西安的火车
        /// </summary>
        train = 0x80,
        //娱乐类
        /// <summary>
        /// 上映电影  查询上映电影的服务，例如：最近有什么好看的电影
        /// </summary>
        movie = 0x100,
        /// <summary>
        /// 音乐  查询音乐的服务，例如：来点刘德华的歌
        /// </summary>
        music = 0x200,
        /// <summary>
        /// 视频  查询视频服务，例如：我想看甄嬛传
        /// </summary>
        video = 0x400,
        /// <summary>
        /// 小说  查询小说的服务，例如：来点言情小说看看
        /// </summary>
        novel = 0x800,
        //工具类
        /// <summary>
        /// 天气  查询天气的服务，例如：明天北京天气
        /// </summary>
        weather = 0x1000,
        /// <summary>
        /// 股票  查询股票的服务，例如：腾讯股价多少了
        /// </summary>
        stock = 0x2000,
        /// <summary>
        /// 提醒  提醒服务，例如：提醒我明天上午十点开会
        /// </summary>
        remind = 0x4000,
        /// <summary>
        /// 常用电话  查询常用电话号码服务，例如：查询一下招行信用卡的电话
        /// </summary>
        telephone = 0x8000,
        //知识类
        /// <summary>
        /// 菜谱  查询菜谱服务，例如：宫保鸡丁怎么做
        /// </summary>
        cookbook = 0x10000,
        /// <summary>
        /// 百科  查询百科服务，例如：查一下刘德华的百科资料
        /// </summary>
        baike = 0x20000,
        /// <summary>
        /// 资讯  查询新闻服务，例如：今天有什么新闻
        /// </summary>
        news = 0x40000,
        //其他类
        /// <summary>
        /// 电视节目预告  查询电视节目服务，例如：湖南台今晚有什么节目
        /// </summary>
        tv = 0x80000,
        /// <summary>
        /// 通用指令  通用指令服务，例如：把声音调高一点
        /// </summary>
        instruction = 0x100000,
        /// <summary>
        /// 电视指令  电视指令服务，例如：切换到中央五台
        /// </summary>
        tv_instruction = 0x200000,
        /// <summary>
        /// 车载指令  车载指令服务，例如：把空调设为25度
        /// </summary>
        car_instruction = 0x400000,
        /// <summary>
        /// 应用  查询应用服务，例如：打开愤怒的小鸟
        /// </summary>
        app = 0x800000,
        /// <summary>
        /// 网址  查询网址服务，例如：帮我打开腾讯网
        /// </summary>
        website = 0x1000000,
        /// <summary>
        /// 网页搜索  网页搜索服务，例如：百度一下意大利对乌拉圭
        /// </summary>
        search = 0x2000000
    }
}
