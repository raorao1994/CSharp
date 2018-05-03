/**
 * @作者 RaoRao
 * @命名空间 Robin.Log.ExceptionLess
 */
(function () {
	//获取当前路径
	var scripts = document.getElementsByTagName("script")
	var script = scripts[scripts.length - 1];
	//IE8直接.src
	strJsPath = document.querySelector ? script.src : script.getAttribute("src", 4);
	var path = strJsPath.substr(0, strJsPath.lastIndexOf('/'));
	//添加js配置文件脚本
	var file = document.createElement("script");
	//显示当前正在执行js文件的地址
	file.src = path + "/exceptionless.config.js";
	var s = document.getElementsByTagName("script")[0];
	s.parentNode.insertBefore(file, s);
	//添加js依赖文件包
	var file2 = document.createElement("script");
	file2.src = path + "/exceptionless.js";
	//s.parentNode.insertBefore(file2, s);
})();

//访问日志静态类
var AccessLog = function () {
	//静态私有属性方法
	var ApiKey = exceptionless_config.EL_AccessLog;
	var serverUrl = exceptionless_config.EL_ServerUrl;
	var client = new exceptionless.ExceptionlessClient(ApiKey, serverUrl);
}
/**
*Exception 异常日志
*erro 异常信息
*/
AccessLog.Exception = function (erro) {
	this.client.submitException(erro);
}
/**
*Log Log日志
*msg 异常信息
*leve 异常级别(可空)
*/
AccessLog.Log = function (msg, leve) {
	this.client.createLog("", msg, leve).submit();
}
/**
*FeatureUsage FeatureUsage日志
*msg 异常信息
*/
AccessLog.FeatureUsage = function (msg) {
	this.client.createFeatureUsage(msg).submit();
}
/**
*BrokenLinks BrokenLinks日志
*msg 异常信息
*/
AccessLog.BrokenLinks = function (msg) {
	this.client.createNotFound(msg).submit();
}

//安全日志静态类
var SecurityLog = function () {
	//静态私有属性方法
	var ApiKey = exceptionless_config.EL_SecurityLog;
	var serverUrl = exceptionless_config.EL_ServerUrl;
	var client = new exceptionless.ExceptionlessClient(ApiKey, serverUrl);
}
/**
*Exception 异常日志
*erro 异常信息
*/
SecurityLog.Exception = function (erro) {
	this.client.submitException(erro);
}
/**
*Log Log日志
*msg 异常信息
*leve 异常级别(可空)
*/
SecurityLog.Log = function (msg, leve) {
	this.client.createLog("", msg, leve).submit();
}
/**
*FeatureUsage FeatureUsage日志
*msg 异常信息
*/
SecurityLog.FeatureUsage = function (msg) {
	this.client.createFeatureUsage(msg).submit();
}
/**
*BrokenLinks BrokenLinks日志
*msg 异常信息
*/
SecurityLog.BrokenLinks = function (msg) {
	this.client.createNotFound(msg).submit();
}

//操作日志静态类
var OperationLog = function () {
	//静态私有属性方法
	var ApiKey = exceptionless_config.EL_OperationLog;
	var serverUrl = exceptionless_config.EL_ServerUrl;
	var client = new exceptionless.ExceptionlessClient(ApiKey, serverUrl);
}
/**
*Exception 异常日志
*erro 异常信息
*/
OperationLog.Exception = function (erro) {
	console.log(OperationLog.ApiKey);
	this.client.submitException(erro);
}
/**
*Log Log日志
*msg 异常信息
*leve 异常级别(可空)
*/
OperationLog.Log = function (msg, leve) {
	this.client.createLog("", msg, leve).submit();
}
/**
*FeatureUsage FeatureUsage日志
*msg 异常信息
*/
OperationLog.FeatureUsage = function (msg) {
	this.client.createFeatureUsage(msg).submit();
}
/**
*BrokenLinks BrokenLinks日志
*msg 异常信息
*/
OperationLog.BrokenLinks = function (msg) {
	this.client.createNotFound(msg).submit();
}

//操作日志静态类
var SystemLog = function () {
	//静态私有属性方法
	var ApiKey = exceptionless_config.EL_SystemLog;
	var serverUrl = exceptionless_config.EL_ServerUrl;
	var client = new exceptionless.ExceptionlessClient(ApiKey, serverUrl);
}
/**
*Exception 异常日志
*erro 异常信息
*/
SystemLog.Exception = function (erro) {
	this.client.submitException(erro);
}
/**
*Log Log日志
*msg 异常信息
*leve 异常级别(可空)
*/
SystemLog.Log = function (msg, leve) {
	this.client.createLog("", msg, leve).submit();
}
/**
*FeatureUsage FeatureUsage日志
*msg 异常信息
*/
SystemLog.FeatureUsage = function (msg) {
	this.client.createFeatureUsage(msg).submit();
}
/**
*BrokenLinks BrokenLinks日志
*msg 异常信息
*/
SystemLog.BrokenLinks = function (msg) {
	this.client.createNotFound(msg).submit();
}


