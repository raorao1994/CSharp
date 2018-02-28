using System;

namespace PublicAccount
{
    /// <summary>
    /// 客服在线接待信息
    /// </summary>
    public class CustomerServiceOnlineInfo
    {
        private int _status;
        /// <summary>
        /// 客服账号
        /// </summary>
        public string kf_account { get; set; }
        /// <summary>
        /// 客服在线状态：1-PC在线，2-手机在线
        /// </summary>
        public int status
        {
            get
            {
                return _status;
            }
            set
            {
                CustomerServiceOnlineStatusEnum result;
                if (Enum.TryParse<CustomerServiceOnlineStatusEnum>(value.ToString(), out result))
                    _status = value;
                else
                    throw new ArgumentException("客服在线状态错误。", "status");
            }
        }
        /// <summary>
        /// 客服工号
        /// </summary>
        public string kf_id { get; set; }
        /// <summary>
        /// 客服设置的最大自动接待数
        /// </summary>
        public int auto_accept { get; set; }
        /// <summary>
        /// 客服当前接待的会话数
        /// </summary>
        public int accepted_case { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="kf_account"></param>
        /// <param name="status"></param>
        /// <param name="kf_id"></param>
        /// <param name="auto_accept"></param>
        /// <param name="accepted_case"></param>
        public CustomerServiceOnlineInfo(string kf_account, int status, string kf_id, int auto_accept, int accepted_case)
        {
            this.kf_account = kf_account;
            this.status = status;
            this.kf_id = kf_id;
            this.auto_accept = auto_accept;
            this.accepted_case = accepted_case;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="kf_account"></param>
        /// <param name="status"></param>
        /// <param name="kf_id"></param>
        /// <param name="auto_accept"></param>
        /// <param name="accepted_case"></param>
        public CustomerServiceOnlineInfo(string kf_account, CustomerServiceOnlineStatusEnum status, string kf_id, int auto_accept, int accepted_case)
            : this(kf_account, (int)status, kf_id, auto_accept, accepted_case)
        { }

        /// <summary>
        /// 获取客服在线状态
        /// </summary>
        /// <returns></returns>
        public CustomerServiceOnlineStatusEnum GetOnlineStatus()
        {
            return (CustomerServiceOnlineStatusEnum)status;
        }
    }
}
