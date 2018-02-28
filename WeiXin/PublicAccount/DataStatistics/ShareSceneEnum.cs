using System;

namespace PublicAccount
{
    /// <summary>
    /// 分享的场景
    /// </summary>
    public enum ShareSceneEnum
    {
        /// <summary>
        /// 好友转发
        /// </summary>
        FriendsRelay = 1,
        /// <summary>
        /// 朋友圈
        /// </summary>
        CircleOfFriends = 2,
        /// <summary>
        /// 腾讯微博
        /// </summary>
        TecentTweet = 3,
        /// <summary>
        /// 其他
        /// </summary>
        Other = 255
    }
}
