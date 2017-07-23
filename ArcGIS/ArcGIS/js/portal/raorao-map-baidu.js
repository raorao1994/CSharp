//<script type="text/javascript" src="http://api.map.baidu.com/api?v=2.0&ak=o9j6mw2LBWd7tRd4GBIFeoY6"></script>
if (!this["RaoRao"]) {
    RaoRao= {}
}
if (!this["RaoRao.BMap"]) {
    RaoRao.BMap = {}
}
RaoRao.BMap = {
    Map2DControl:null,
    InitMap: function (a) {
        var map = new BMap.Map("baidu-map");
        map.centerAndZoom(new BMap.Point(116.404, 39.915), 11);
        map.enableScrollWheelZoom(true);
        RaoRao.BMap.Map2DControl = map;
        if (a != undefined && $.isFunction(a)) {
            a()
        }
    },
    Query: function (a, f, c, e) {
        
    },
    Search: {},
    Printing: function () {
        
    },
    Analysis: {},
    Geometry: {
        
    },
    Event: {
        bindClickEvent: function (a, b) {
            
        }
    },
    MapService: {
        themeLayerContainerId: "tabs-MapServer",
        layerControlContainerId: "tabs-LayerControl",
        map: null,
        GlobalTopics: [],
        GlobalDynamicTopics: [],
        tag: true,
        LoadType: 1,
        GlobalMapService: [],
        init: function (a) {
           
        },
        ChangeSlider: function (c, b) {
            
        },
        AddResourceMaps: function (a) {
           
        },
        AddLayer: function (f) {
            
        },
        GetTopicData: function () {
           
        },
        AddChildResource: function (f, b, e) {
           
        },
        RemoveAddService: function (c) {
           
        },
        ChangeOpacity: function (a, c, d) {
          
        },
        ChangeLayerVisible: function (c, a) {
           
        },
        GetChildNode: function (a, c) {
            
        },
        GetParentNode: function (b, c, a) {
           
        },
        ZTreeOnNodeCreated: function (a, c, b) {
           
        },
        ExistHasChildNode: function (b, a) {
           
        },
        ZTreeOnCheck: function (a, e, i) {
            
        }
    },
    addLayer: function (b) {
       
    },
    GetPoint: function (x, y) {
        var p1 = new BMap.Point(x, y);
        return p1;
    },
    GetPolygon: function () {
        
    },
    GetPolyline: function () {
        
    },
    GetGraphic: function (c, b, a) {
       
    },
    GetGraphicLayer: function (b, c) {
        
    },
    GetLayer: function (d, a, c) {
        
    },
    ShowGraphic: function (f, b, j, h, g, e, d) {
        
    },
    ClearLayer: function (b, c) {
        
    },
    GetGraphicByGeometry: function (a) {
        
    },
    Fly2Geometry: function (f, g, d) {
        
    },
    Extent1ContainExtent2: function (c, b) {
        
    },
    Union2Extent: function (c, a) {
       
    },
    GetFullExtentFromPoints: function (h, f) {
        
    },
    Symbol: {
        markSymbol: function () {
           
        },
        fillSymbol: function () {
           
        },
        lineSymbol: function () {
            
        },
        txtSymbol: function (b, a, e, c) {
            
        },
        customMarkSymbol: function (c, b) {
           
        },
        customFillSymbol: function (a) {
           
        },
        customPicFillSymbol: function (b, d, a) {
            
        },
        customLineSymbol: function (a, c) {
            
        },
        pictureMarkerSymbol: function (c, d, b, a, f) {
           
        }
    },
    Formatter: {
        //经纬度转墨卡托
        lonlat2mercator:function (lonlat){
            var mercator={x:0,y:0};
            var x = lonlat.x *20037508.34/180;
            var y = Math.log(Math.tan((90+lonlat.y)*Math.PI/360))/(Math.PI/180);
            y = y *20037508.34/180;
            mercator.x = x;
            mercator.y = y;
            //var p=new esri.geometry.Point(x, y, RaoRao.BMap.Map2DControl.spatialReference)
            return mercator;
        },
        //墨卡托转经纬度
        mercator2lonlat:function (mercator){
            var lonlat={x:0,y:0};
            var x = mercator.x/20037508.34*180;
            var y = mercator.y/20037508.34*180;
            y= 180/Math.PI*(2*Math.atan(Math.exp(y*Math.PI/180))-Math.PI/2);
            lonlat.x = x;
            lonlat.y = y;
            return lonlat;
        }
    },
    //导航
    Navigation: {
        //驾车
        /*
        *@start 起始位置 
        *@end   结束位置 
        *@op    经过位置 
        */
        DrivingRoute: function (start, end, op, callback) {
            //三种驾车策略：最少时间，最短距离，避开高速
            var routePolicy = [BMAP_DRIVING_POLICY_LEAST_TIME, BMAP_DRIVING_POLICY_LEAST_DISTANCE, BMAP_DRIVING_POLICY_AVOID_HIGHWAYS];
            var options = {
                renderOptions: {
                    map: RaoRao.BMap.Map2DControl,
                    enableDragging: true, //起终点可进行拖拽,
                    autoViewport: true,
                    panel: "r-result"
                },
                policy: routePolicy[0],
                onSearchComplete: function (results) {
                    if (driving.getStatus() == BMAP_STATUS_SUCCESS) {
                        //// 获取第一条方案
                        //var plan = results.getPlan(0);
                        //// 获取方案的驾车线路
                        //var route = plan.getRoute(0);
                        //// 获取每个关键步骤,并输出到页面
                        //var s = [];
                        //for (var j = 0; j < plan.getNumRoutes() ; j++) {
                        //    var route = plan.getRoute(j);
                        //    for (var i = 0; i < route.getNumSteps() ; i++) {
                        //        var step = route.getStep(i);
                        //        s.push((i + 1) + ". " + step.getDescription());
                        //    }
                        //}
                        //document.getElementById("r-result").innerHTML = s.join("<br/>");
                        callback(results);
                    }
                }
            };
            var driving = new BMap.DrivingRoute(RaoRao.BMap.Map2DControl, options);
            //driving.search("天安门", "百度大厦",{waypoints:['西直门']});
            //var p1 = new BMap.Point(116.301934, 39.977552);
            //var p2 = new BMap.Point(116.508328, 39.919141);
            //driving.search(p1, p2,{waypoints:['西直门']});
            driving.search(start, end, op);
        }
    }
};