//初始化顶层命名空间
if (!this["RaoRao"]) { RaoRao = {}; }
//初始化Setting命名空间
if (!this["RaoRao.Setting"]) { RaoRao.Setting = {}; }
//ArcGIS服务地址,为了后期脱离平台准备
RaoRao.Setting.ArcGISMapServices = [
   //{ id: 'qg', name: '全国地图', type: 'ArcGISDynamicMapServiceLayer', url: 'http://www.arcgisonline.cn/ArcGIS/rest/services/ChinaOnlineCommunity/MapServer' },
   { id: 'ts', name: '唐山地图', type: 'ArcGISDynamicMapServiceLayer', url: 'http://server.morningstars.com.cn:6080/arcgis/rest/services/%E5%94%90%E5%B1%B1/MapServer' }
];

//定义系统配置变量
/** 
 * 包含了全局配置信息. 
 */
RaoRao.Setting.GlobalSetting = {
    //查询图层
    queryurl:RaoRao.Setting.ArcGISMapServices[0].url+"/10",
    /** 
     * 基础服务.  
     * @type String
     */
    BaseMapServices: {

        /** 
         * 用于各种几何分析使用的ArcGIS服务.  
         * @type String
         */
        Geometry: {
            url: "http://10.168.17.212/ArcGIS/rest/services/Geometry/GeometryServer"
        },

        /** 
         * 打印服务.  
         * @type String
         */
        Printing: {
            //url: "https://sampleserver6.arcgisonline.com/arcgis/rest/services/Utilities/PrintingTools/GPServer/Export%20Web%20Map%20Task"
            url: "http://192.168.1.101:6080/arcgis/rest/services/ExportWebMap/GPServer/导出%20Web%20地图"
        }
    },

    /** 
     * 坐标系.  
     * @type String2436
     */
    wkid: 4326,//9001 102113

    /** 
       * 代理地址.  
       * @type String
       */
   //proxyUrl: "../proxy/proxy.ashx",
    proxyUrl: "http://" + window.location.host + "/proxy/proxy.ashx",


};

/** 
　　* 包含了全局属性变量. 
　　* @class 全局属性变量. 
　　*/
RaoRao.Setting.GlobalProperty = {

    /** 
       * 地图.  
       * @type Object
       */
    map: {},

    /** 
       * 比例尺.  
       * @type number
       */
    scale: 1,
    /** 
       * 导航工具条.  
       * @type Object
       */
    navToolbar: {},

    /** 
       * 鹰眼.  
       * @type Object
       */
    overviewMap: {},

    /** 
       * 测量是否启用.  
       * @type bool
       */
    measureOn: false,

    /** 
       * 测量组件.  
       * @type Object
       */
    measurement: {},

    /** 
       * 用于标记的图形.  
       * @type Object
       */
    markerGraphic: {},

    /** 
       * 比例尺.  
       * @type Object
       */
    scalebar: {},

    /** 
       * 根据topic请求图层回调函数.  
       * @type Function
       */
    layerRequestCallback: {},

    /** 
       * 图层控制中资源控制回调函数.  
       * @type Function
       */
    mapRequestCallback: {},

    /** 
       * 全局服务存储集合.  
       * @type Array
       */
    globalMapService: [],

    /** 
       * 保存主题编号.  
       * @type Array
       */
    dlTopics: [],

    /** 
       * 保存服务编号.  
       * @type Array
       */
    dlServices: [],

    /** 
       * 资源是否可见.  
       * @type Array
       */
    dlFlag: [],

    /** 
       * 动态图层信息.  
       * @type Array
       */
    dynamicMapServiceLayers: [],

    /** 
       * 查询相关配置.  
       * @type Object
       */
    Query: {
        /** 
           * 查询结果.  
           * @type Object
           */
        result: null,
        /** 
           * 几何图形.  
           * @type Object
           */
        geometry: null,
        /** 
           * 是否使用代理.  
           * @type bool
           */
        useProxy: false
    }
};

/** 
　　* 包含了地图默认配置信息. 
　　* @class 地图默认配置. 
　　*/
RaoRao.Setting.MapSetting = {

    /** 
       * 初始缩放级别
       * @type Number
       */
    zoom: 2,
    /** 
       * 关闭Logo标记.  
       * @type Object
       */
    logo: false,

    /** 
       * 在地图上显示一个滚动条.  
       * @type Object
       */
    slider: false,

    /** 
     * 地图初始化缩放
     * @type Object
     */
    initExtent: {
        xmin: 118.1364542153776,
        xmax: 118.20301963701556,
        ymin: 39.632002774260975,
        ymax: 39.64871848782689
    }
};
