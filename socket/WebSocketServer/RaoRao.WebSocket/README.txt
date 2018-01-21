 1.0、先创建WebSocket服务对象
 RaoRao.WebSocket.WebSocket web = new RaoRao.WebSocket.WebSocket(9000);
 2.0、设置一个最大连接数
 web.CreateTcpSocket(100);
 3.0、添加上线、下线事件函数
web.ClientConnected += (IPEndPoint p) => {
    Console.WriteLine("客户端：" + p.ToString() + "上线");
};
web.MessageReceived += (IPEndPoint p, string msg) => {
    Console.WriteLine("客户端：" + p.ToString() + "的信息：" + msg);
    web.SendAll("你好啊：客户端");
};