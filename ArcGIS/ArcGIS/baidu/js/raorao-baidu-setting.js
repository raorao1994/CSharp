//初始化顶层命名空间
if (!this["RaoRao"]) { RaoRao = {}; }
//初始化Setting命名空间
if (!this["RaoRao.Setting"]) { RaoRao.Setting = {}; }

//定义系统配置变量
/** 
 * 包含了全局配置信息. 
 */
RaoRao.Setting.GlobalSetting = {

};

/** 
　　* 包含了全局属性变量. 
　　* @class 全局属性变量. 
　　*/
RaoRao.Setting.GlobalProperty = {

};

/** 
　　* 包含了地图默认配置信息. 
　　* @class 地图默认配置. 
　　*/
RaoRao.Setting.MapSetting = {
    /** 
       * 最小显示返回.  
       * @type number
       */
    minZoom: null,
    /** 
       * 最大显示返回.  
       * @type number
       */
    maxZoom: null,
    /** 
       * 关闭默认地图POI事件.  
       * @type bool
       */
    enableMapClick:true,
    /** 
       * 允许滚轮进行缩放
       * @type bool
       */
    enableScrollWheelZoom: true,
    /** 
       * 初始缩放级别
       * @type Number
       */
    zoom: 11,
    /** 
       * 关闭Logo标记.  
       * @type Object
       */
    logo: true,

    /** 
     * 地图初始化缩放
     * @type Object
     */
    centerPoint: {
        x: 116.404, 
        y: 39.915
    }
};
