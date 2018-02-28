using System;

namespace PublicAccount.Semantic
{
    /// <summary>
    /// 通用协议类型
    /// </summary>
    public enum CommonProtocolTypeEnum
    {
        //时间协议
        /// <summary>
        /// 单时间协议，细分为两个类别：DT_ORI和DT_INFER
        /// </summary>
        DT_SINGLE,
        /// <summary>
        /// 字面时间，比如：“上午九点”
        /// </summary>
        DT_ORI,
        /// <summary>
        /// 推理时间，比如：“提前 5 分钟”
        /// </summary>
        DT_INFER,
        /// <summary>
        /// 时间段，比如：“明天上午九点到后天下午三点”
        /// </summary>
        DT_INTERVAL,
        /// <summary>
        /// 重复时间，细分为两个类别：DT_RORI和DT_RINFER
        /// </summary>
        DT_REPEAT,
        /// <summary>
        /// 重复字面时间，比如：“每天上午九点”
        /// </summary>
        DT_RORI,
        /// <summary>
        /// 重复推理时间，比如：“工作日除外”
        /// </summary>
        DT_RINFER,

        //地点协议
        /// <summary>
        /// 地点，细分为如下类别：LOC_COUNTRY、LOC_PROVINCE、LOC_CITY、LOC_TOWN、LOC_POI、NORMAL_POI
        /// </summary>
        LOC,
        /// <summary>
        /// 国家
        /// </summary>
        LOC_COUNTRY,
        /// <summary>
        /// 省
        /// </summary>
        LOC_PROVINCE,
        /// <summary>
        /// 市
        /// </summary>
        LOC_CITY,
        /// <summary>
        /// 县区
        /// </summary>
        LOC_TOWN,
        /// <summary>
        /// 地名除了国家、省、市、区县的详细地址
        /// </summary>
        LOC_POI,
        /// <summary>
        /// 地图上偏向机构的 poi 点，比如：饭馆、酒店、大厦等等
        /// </summary>
        NORMAL_POI,
        
        //数字协议
        /// <summary>
        /// 数字，细分为如下类别： NUM_PRICE、NUM_P ADIUS 、 NUM_DISCOUNT 、NUM_SEASON、 NUM_EPI、 NUM_CHAPTER
        /// </summary>
        NUMBER,
        /// <summary>
        /// 价格相关，例：200 元左右
        /// </summary>
        NUM_PRICE,
        /// <summary>
        /// 距离相关，例：200 米以内
        /// </summary>
        NUM_PADIUS,
        /// <summary>
        /// 折扣相关，例：八折
        /// </summary>
        NUM_DISCOUNT,
        /// <summary>
        /// 部，季相关，例：第一部
        /// </summary>
        NUM_SEASON,
        /// <summary>
        /// 集相关，例：第一集
        /// </summary>
        NUM_EPI,
        /// <summary>
        /// 章节相关，例：第一章
        /// </summary>
        NUM_CHAPTER
    }
}
