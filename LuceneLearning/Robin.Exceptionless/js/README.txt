1、Robin.Log.ExceptionLess.js需要引用exceptionless.js和exceptionless.config.js动态库
2、exceptionless.js和exceptionless.config.js动态库需要在Robin.Log.ExceptionLess前引用
3、需要在exceptionless.config.js配置文件中设置相应节点，具体配置如下
var exceptionless_config = {
    //Exceptionless网站地址
    EL_ServerUrl: "http://172.30.80.25:9001",
    //访问日志apikey
    EL_AccessLog: "1ACDVYQ5B22wYbZijV4oEIvcXONusO6dFbAabQpS",
    //安全日志apikey
    EL_SecurityLog: "ZMqI3R4pyR1bpEwDlt0cJAHkDC6sPjWSdWASUu3C",
    //操作日志apikey
    EL_OperationLog: "erRzz8oHMbK9NDzQTzCpi5gGveh61OXqiUcBJGcx",
    //系统日志apikey
    EL_SystemLog: "VY8oKQ9pAU0ZwIcytrXP09GaAmWwjLXDMHNQD6xL"
};
4、使用
AccessLog.Exception(new Exception("AccessLog:我是错误信息"));
AccessLog.Log("AccessLog:我的Log信息", "Info");
AccessLog.FeatureUsage("AccessLog:我的FeatureUsage信息");
AccessLog.BrokenLinks("AccessLog:我的NotFound信息");

OperationLog.Exception(new Exception("OperationLog:我是错误信息"));
OperationLog.Log("OperationLog:我的Log信息", "Info");
OperationLog.FeatureUsage("OperationLog:我的FeatureUsage信息");
OperationLog.BrokenLinks("OperationLog:我的NotFound信息");

SecurityLog.Exception(new Exception("SecurityLog:我是错误信息"));
SecurityLog.Log("SecurityLog:我的Log信息", "Info");
SecurityLog.FeatureUsage("SecurityLog:我的FeatureUsage信息");
SecurityLog.BrokenLinks("SecurityLog:我的NotFound信息");

SystemLog.Exception(new Exception("SystemLog:我是错误信息"));
SystemLog.Log("SystemLog:我的Log信息", "Info");
SystemLog.FeatureUsage("SystemLog:我的FeatureUsage信息");
SystemLog.BrokenLinks("SystemLog:我的NotFound信息");