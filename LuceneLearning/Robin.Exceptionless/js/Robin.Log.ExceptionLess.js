/**
 * @作者 RaoRao
 * @命名空间 Robin.Log.ExceptionLess
 */
//访问日志静态类
var AccessLog = {
    //静态私有属性方法
    ApiKey: exceptionless_config.EL_AccessLog,
    serverUrl: exceptionless_config.EL_ServerUrl,
    client: new exceptionless.ExceptionlessClient(exceptionless_config.EL_AccessLog, exceptionless_config.EL_ServerUrl),
    /**
    *Exception 异常日志
    *erro 异常信息
    */
    Exception: function (erro) {
        this.client.submitException(erro);
    },
    /**
    *Log Log日志
    *msg 异常信息
    *leve 异常级别(可空),leve级别有 Debug、Error、Info、Warn、Fatal、Off、Trace、Other
    */
    Log: function (msg, leve) {
        this.client.createLog("", msg, leve).submit();
    },
    /**
    *FeatureUsage FeatureUsage日志
    *msg 异常信息
    */
    FeatureUsage: function (msg) {
        this.client.createFeatureUsage(msg).submit();
    },
    /**
    *BrokenLinks BrokenLinks日志
    *msg 异常信息
    */
    BrokenLinks: function (msg) {
        this.client.createNotFound(msg).submit();
    }
}

//安全日志静态类
var SecurityLog = {
    //静态私有属性方法
    ApiKey: exceptionless_config.EL_SecurityLog,
    serverUrl: exceptionless_config.EL_ServerUrl,
    client: new exceptionless.ExceptionlessClient(exceptionless_config.EL_SecurityLog, exceptionless_config.EL_ServerUrl),
    /**
    *Exception 异常日志
    *erro 异常信息
    */
    Exception: function (erro) {
        this.client.submitException(erro);
    },
    /**
    *Log Log日志
    *msg 异常信息
    *leve 异常级别(可空),leve级别有 Debug、Error、Info、Warn、Fatal、Off、Trace、Other
    */
    Log: function (msg, leve) {
        this.client.createLog("", msg, leve).submit();
    },
    /**
    *FeatureUsage FeatureUsage日志
    *msg 异常信息
    */
    FeatureUsage: function (msg) {
        this.client.createFeatureUsage(msg).submit();
    },
    /**
    *BrokenLinks BrokenLinks日志
    *msg 异常信息
    */
    BrokenLinks: function (msg) {
        this.client.createNotFound(msg).submit();
    }
}

//操作日志静态类
var OperationLog = {
    //静态私有属性方法
    ApiKey: exceptionless_config.EL_OperationLog,
    serverUrl: exceptionless_config.EL_ServerUrl,
    client: new exceptionless.ExceptionlessClient(exceptionless_config.EL_OperationLog, exceptionless_config.EL_ServerUrl),
    /**
    *Exception 异常日志
    *erro 异常信息
    */
    Exception: function (erro) {
        this.client.submitException(erro);
    },
    /**
    *Log Log日志
    *msg 异常信息
    *leve 异常级别(可空),leve级别有 Debug、Error、Info、Warn、Fatal、Off、Trace、Other
    */
    Log: function (msg, leve) {
        this.client.createLog("", msg, leve).submit();
    },
    /**
    *FeatureUsage FeatureUsage日志
    *msg 异常信息
    */
    FeatureUsage: function (msg) {
        this.client.createFeatureUsage(msg).submit();
    },
    /**
    *BrokenLinks BrokenLinks日志
    *msg 异常信息
    */
    BrokenLinks: function (msg) {
        this.client.createNotFound(msg).submit();
    }
}

//操作日志静态类
var SystemLog = {
    //静态私有属性方法
    ApiKey: exceptionless_config.EL_SystemLog,
    serverUrl: exceptionless_config.EL_ServerUrl,
    client: new exceptionless.ExceptionlessClient(exceptionless_config.EL_SystemLog, exceptionless_config.EL_ServerUrl),
    Exception: function (erro) {
        this.client.submitException(erro);
    },
    /**
    *Log Log日志
    *msg 异常信息
    *leve 异常级别(可空),leve级别有 Debug、Error、Info、Warn、Fatal、Off、Trace、Other
    */
    Log: function (msg, leve) {
        this.client.createLog("", msg, leve).submit();
    },
    /**
    *FeatureUsage FeatureUsage日志
    *msg 异常信息
    */
    FeatureUsage: function (msg) {
        this.client.createFeatureUsage(msg).submit();
    },
    /**
    *BrokenLinks BrokenLinks日志
    *msg 异常信息
    */
    BrokenLinks: function (msg) {
        this.client.createNotFound(msg).submit();
    }
}



