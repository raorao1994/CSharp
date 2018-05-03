1、Robin.Log.ExceptionLess需要引用Exceptionless.DLL动态库
2、程序必须为.NET Framework4.5及以上版本
3、需要在配置文件中添加configSections节点，具体配置如下
<configuration>
  <configSections>
    <section name="exceptionless" type="Exceptionless.ExceptionlessSection, Exceptionless" />
  </configSections>
  <startup>
    <supportedRuntime version="v4.0" sku=".NETFramework,Version=v4.5.1" />
  </startup>
  <exceptionless apiKey="dnTMzCoOTRtaNKfr3Lu8PwPILf1wSdOqlufnCs25" serverUrl="http://172.30.80.25:9001" />
  <appSettings>
    <!--Exceptionless网站地址-->
    <add key="EL_ServerUrl" value="http://172.30.80.25:9001"/>
    <!--访问日志apikey-->
    <add key="EL_AccessLog" value="1ACDVYQ5B22wYbZijV4oEIvcXONusO6dFbAabQpS"/>
    <!--安全日志apikey-->
    <add key="EL_SecurityLog" value="ZMqI3R4pyR1bpEwDlt0cJAHkDC6sPjWSdWASUu3C"/>
    <!--操作日志apikey-->
    <add key="EL_OperationLog" value="erRzz8oHMbK9NDzQTzCpi5gGveh61OXqiUcBJGcx"/>
    <!--系统日志apikey-->
    <add key="EL_SystemLog" value="VY8oKQ9pAU0ZwIcytrXP09GaAmWwjLXDMHNQD6xL"/>
  </appSettings>
</configuration>
4、使用
AccessLog.Exception(new Exception("AccessLog:我是错误信息"));
AccessLog.Log("AccessLog:我的Log信息", LogLevel.Info);
AccessLog.FeatureUsage("AccessLog:我的FeatureUsage信息");
AccessLog.BrokenLinks("AccessLog:我的NotFound信息");

OperationLog.Exception(new Exception("OperationLog:我是错误信息"));
OperationLog.Log("OperationLog:我的Log信息", LogLevel.Info);
OperationLog.FeatureUsage("OperationLog:我的FeatureUsage信息");
OperationLog.BrokenLinks("OperationLog:我的NotFound信息");

SecurityLog.Exception(new Exception("SecurityLog:我是错误信息"));
SecurityLog.Log("SecurityLog:我的Log信息", LogLevel.Info);
SecurityLog.FeatureUsage("SecurityLog:我的FeatureUsage信息");
SecurityLog.BrokenLinks("SecurityLog:我的NotFound信息");

SystemLog.Exception(new Exception("SystemLog:我是错误信息"));
SystemLog.Log("SystemLog:我的Log信息", LogLevel.Info);
SystemLog.FeatureUsage("SystemLog:我的FeatureUsage信息");
SystemLog.BrokenLinks("SystemLog:我的NotFound信息");