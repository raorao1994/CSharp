using System;

namespace PublicAccount.Semantic
{
    /// <summary>
    /// 座位级别（默认无限制）：ECONOMY（经济舱），BIZ（商务舱），FIRST（头等舱）
    /// </summary>
    public enum FlightSeatEnum
    {
        /// <summary>
        /// 经济舱
        /// </summary>
        ECONOMY,
        /// <summary>
        /// 商务舱
        /// </summary>
        BIZ,
        /// <summary>
        /// 头等舱
        /// </summary>
        FIRST
    }
}