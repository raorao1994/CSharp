namespace PublicAccount
{
    /// <summary>
    /// 处理请求消息的委托
    /// </summary>
    /// <param name="requestMessage">请求消息</param>
    /// <returns>返回响应消息</returns>
    public delegate ResponseBaseMessage HandleRequestMessageDelegate(RequestBaseMessage requestMessage);
}