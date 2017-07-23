/*
*@作者：陈兴旺
*@说明：百度地图封装包
*@JS包：<script type="text/javascript" src="http://api.map.baidu.com/api?v=2.0&ak=o9j6mw2LBWd7tRd4GBIFeoY6"></script>
*@开源包地址：http://lbsyun.baidu.com/index.php?title=open/library
*/

if (!this["RaoRao"]) {
    RaoRao= {}
}
if (!this["RaoRao.BMap"]) {
    RaoRao.BMap = {}
}
RaoRao.BMap = {
    Map2DControl: null,
    //初始化地图
    InitMap: function (a) {
        //地图设置
        var op = {
            minZoom: RaoRao.Setting.MapSetting.minZoom,
            maxZoom: RaoRao.Setting.MapSetting.maxZoom,
            enableMapClick: RaoRao.Setting.MapSetting.enableMapClick
        };
        var map = new BMap.Map("baidu-map",op);
        map.centerAndZoom(new BMap.Point(RaoRao.Setting.MapSetting.centerPoint.x, RaoRao.Setting.MapSetting.centerPoint.y), RaoRao.Setting.MapSetting.zoom);
        map.enableScrollWheelZoom(RaoRao.Setting.MapSetting.enableScrollWheelZoom);
        RaoRao.BMap.Map2DControl = map;
        //显示logo
        if (!RaoRao.Setting.MapSetting.logo) {
            var nod = document.createElement("style");
            if (nod.styleSheet) {         //ie下  
                nod.styleSheet.cssText = ".anchorBL{display:none;}";
            } else {
                nod.innerHTML = ".anchorBL{display:none;}";
            }
            document.head.appendChild(nod);
        }
        //回调函数
        if (a != undefined && $.isFunction(a)) {
            a()
        }
    },
    //查询
    Query: function (a, f, c, e) {
        
    },
    //获取地图显示范围
    getBounds: function () {
        return RaoRao.BMap.Map2DControl.getBounds(); //获取可视区域
        //var bssw = bs.getSouthWest();   //可视区域左下角
        // var bsne = bs.getNorthEast();   //可视区域右上角
    },
    //设置地图显示范围
    setBounds: function (sw, ne) {
        //其中sw表示矩形区域的西南角，参数ne表示矩形区域的东北角
        var b = new BMap.Bounds(sw, ne);
        try {
            BMapLib.AreaRestriction.setBounds(RaoRao.BMap.Map2DControl, b);
            return true;
        } catch (e) {
            return false;
        }
    },
    //地图控件
    MapControl: {
        //比例尺控件
        ScaleControl: null,
        //平移和缩放按钮控件
        NavigationControl: null,
        //缩略地图控件
        overViewControl: null,
        //地图类型控件
        MapTypeControl: null,
        //定位控件
        GeolocationControl: null,
        //城市列表控件
        CityListControl:null,
        //控件位置
        type: [BMAP_ANCHOR_TOP_LEFT, BMAP_ANCHOR_TOP_RIGHT, BMAP_ANCHOR_BOTTOM_LEFT, BMAP_ANCHOR_BOTTOM_RIGHT],
        //比例尺控件
        Scale: function (location,AddorRemove) {
            //true代表添加，false代表删除
            if (AddorRemove) {
                RaoRao.BMap.MapControl.ScaleControl = new BMap.ScaleControl({ anchor: RaoRao.BMap.MapControl.type[location] });
                RaoRao.BMap.Map2DControl.addControl(RaoRao.BMap.MapControl.ScaleControl);
            } else {
                if(RaoRao.BMap.MapControl.ScaleControl)
                    RaoRao.BMap.Map2DControl.removeControl(RaoRao.BMap.MapControl.ScaleControl);
            }
        },
        //平移和缩放按钮控件
        Navigation: function (location, AddorRemove) {
            //true代表添加，false代表删除
            if (AddorRemove) {
                RaoRao.BMap.MapControl.NavigationControl = new BMap.NavigationControl({ anchor: RaoRao.BMap.MapControl.type[location] });
                /*缩放控件type有四种类型:BMAP_NAVIGATION_CONTROL_SMALL：仅包含平移和缩放按钮；BMAP_NAVIGATION_CONTROL_PAN:仅包含平移按钮；BMAP_NAVIGATION_CONTROL_ZOOM：仅包含缩放按钮*/
                RaoRao.BMap.Map2DControl.addControl(RaoRao.BMap.MapControl.NavigationControl);
            } else {
                if (RaoRao.BMap.MapControl.NavigationControl)
                    RaoRao.BMap.Map2DControl.removeControl(RaoRao.BMap.MapControl.NavigationControl);
            }
        },
        //缩略地图控件
        overView: function (location, AddorRemove) {
            //true代表添加，false代表删除
            if (AddorRemove) {
                RaoRao.BMap.MapControl.overViewControl = new BMap.OverviewMapControl({ isOpen: true, anchor: RaoRao.BMap.MapControl.type[location] });
                RaoRao.BMap.Map2DControl.addControl(RaoRao.BMap.MapControl.overViewControl);
            } else {
                if (RaoRao.BMap.MapControl.overViewControl)
                    RaoRao.BMap.Map2DControl.removeControl(RaoRao.BMap.MapControl.overViewControl);
            }
        },
        //缩略地图控件
        MapType: function (type,location, AddorRemove) {
            //true代表添加，false代表删除
            if (AddorRemove) {
                var op = {
                    anchor: RaoRao.BMap.MapControl.type[location]
                };
                if (type)
                {
                    op = { mapTypes: [BMAP_NORMAL_MAP, BMAP_HYBRID_MAP], anchor: RaoRao.BMap.MapControl.type[location] }
                }
                RaoRao.BMap.MapControl.MapTypeControl = new BMap.MapTypeControl(op);
                RaoRao.BMap.Map2DControl.addControl(RaoRao.BMap.MapControl.MapTypeControl);
            } else {
                if (RaoRao.BMap.MapControl.MapTypeControl)
                    RaoRao.BMap.Map2DControl.removeControl(RaoRao.BMap.MapControl.MapTypeControl);
            }
        },
        //定位控件
        Geolocation: function (location, AddorRemove,callback) {
            if (AddorRemove) {
                RaoRao.BMap.MapControl.GeolocationControl = new BMap.GeolocationControl({ anchor: RaoRao.BMap.MapControl.type[location] });
                RaoRao.BMap.MapControl.GeolocationControl.addEventListener("locationSuccess", function (e) {
                    // 定位成功事件
                    //var address = '';
                    //address += e.addressComponent.province;
                    //address += e.addressComponent.city;
                    //address += e.addressComponent.district;
                    //address += e.addressComponent.street;
                    //address += e.addressComponent.streetNumber;
                    //alert("当前定位地址为：" + address);
                    callback(e.addressComponent);
                });
                RaoRao.BMap.MapControl.GeolocationControl.addEventListener("locationError", function (e) {
                    // 定位失败事件
                    alert(e.message);
                });
                RaoRao.BMap.Map2DControl.addControl(RaoRao.BMap.MapControl.GeolocationControl);
            }
            else {
                if (RaoRao.BMap.MapControl.GeolocationControl)
                {
                    RaoRao.BMap.Map2DControl.removeControl(RaoRao.BMap.MapControl.GeolocationControl);
                } 
            }
        },
        //城市列表控件
        CityList: function (location,AddorRemove,width,height) {
            //true代表添加，false代表删除
            if (AddorRemove) {
                var size = new BMap.Size(height, width);
                RaoRao.BMap.MapControl.CityListControl = new BMap.CityListControl(
                    {
                        anchor: RaoRao.BMap.MapControl.type[location],
                        offset: size,
                        // 切换城市之间事件
                        // onChangeBefore: function(){
                        //    alert('before');
                        // },
                        // 切换城市之后事件
                        // onChangeAfter:function(){
                        //   alert('after');
                        // }
                    });
                RaoRao.BMap.Map2DControl.addControl(RaoRao.BMap.MapControl.CityListControl);
            } else {
                if (RaoRao.BMap.MapControl.CityListControl)
                    RaoRao.BMap.Map2DControl.removeControl(RaoRao.BMap.MapControl.CityListControl);
            }
        }
    },
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
        //获取两点间的距离
        getDistance: function (start,end) {
            return RaoRao.BMap.Map2DControl.getDistance(start,end);
        },
        //测量
        Measure: {
            //距离
            Distance: function () {
                //<script type="text/javascript" src="http://api.map.baidu.com/library/DistanceTool/1.2/src/DistanceTool_min.js"></script>
                var myDis = new BMapLib.DistanceTool(RaoRao.BMap.Map2DControl);
                myDis.open();  //开启鼠标测距
                //myDis.close();  //关闭鼠标测距大
            },
            //面积
            Area: function () {
                var myDis = new BMapLib.AreaTool(RaoRao.BMap.Map2DControl);
                myDis.open();  //开启鼠标测距
                //myDis.close();  //关闭鼠标测距大
            }
        }

    },
    ThemeMap: {
        //添加聚合点到地图
        AddMarkerClusterer: function (markers) {
            //<script type="text/javascript" src="http://api.map.baidu.com/library/TextIconOverlay/1.2/src/TextIconOverlay_min.js"></script>
	        //<script type="text/javascript" src="http://api.map.baidu.com/library/MarkerClusterer/1.2/src/MarkerClusterer_min.js"></script>
            //最简单的用法，生成一个marker数组，然后调用markerClusterer类即可。
            var markerClusterer = new BMapLib.MarkerClusterer(RaoRao.BMap.Map2DControl, { markers: markers });
        },
        //添加热力图
        GetHeatmapOverlay: function (radius, points, max) {
            /*详细的参数,可以查看heatmap.js的文档 https://github.com/pa7/heatmap.js/blob/master/README.md
             * <script type="text/javascript" src="http://api.map.baidu.com/library/Heatmap/2.0/src/Heatmap_min.js"></script>
             *参数说明如下:
             *visible 热力图是否显示,默认为true
             * opacity 热力的透明度,1-100
             * radius 势力图的每个点的半径大小   
             * gradient  {JSON} 热力图的渐变区间 . gradient如下所示
             *	{
                    .2:'rgb(0, 255, 255)',
                    .5:'rgb(0, 110, 255)',
                    .8:'rgb(100, 0, 255)'
                }
                其中 key 表示插值的位置, 0~1. 
                    value 为颜色值. 
             *points = [{ "lng": 116.418261, "lat": 39.921984, "count": 50 }];
             */
            var heatmapOverlay = new BMapLib.HeatmapOverlay({ "radius": radius });
            RaoRao.BMap.Map2DControl.addOverlay(heatmapOverlay);
            heatmapOverlay.setDataSet({ data: points, max: max });
            heatmapOverlay.setOptions({
                "gradient": {
                    0.2: 'rgb(0, 255, 255)',
                    0.5: 'rgb(0, 110, 255)',
                    0.8: 'rgb(100, 0, 255)'
                }
            });
            return heatmapOverlay;
        },
    },
    addLayer: function (b) {
       
    },
    /*
    *设置点是否可以拖拽
    *marker.disableDragging();
    *marker.enableDragging();
    *设置线面是否可以拖拽
    **.disableEditing();
    **.enableEditing();
    *设置覆盖物显示隐藏
    *show() hide()
    *覆盖物添加事件
    *marker.addEventListener("click",getAttr);
    */
    GetPoint: function (x, y) {
        return new BMap.Point(x, y);
    },
    GetPolygon: function (PointList) {
        return new BMap.Polygon(PointList, { strokeColor: "blue", strokeWeight: 2, strokeOpacity: 0.5 });  //创建多边形
    },
    GetPolyline: function (PointList) {
        var polyline = new BMap.Polyline(PointList, { strokeColor: "blue", strokeWeight: 2, strokeOpacity: 0.5 });   //创建折线
    },
    GetCircle: function (point,radius) {
        return new BMap.Circle(point, radius, { strokeColor: "blue", strokeWeight: 2, strokeOpacity: 0.5 }); //创建圆  //创建折线
    },
    GetSimpleMarker: function (point) {
        var marker = new BMap.Marker(point);  // 创建标注
        marker.setAnimation(BMAP_ANIMATION_BOUNCE); //跳动的动画
        return marker;
    },
    GetPicMarker: function (point,url,width,height) {
        var myIcon = new BMap.Icon(url, new BMap.Size(height, width));
        var marker = new BMap.Marker(point, { icon: myIcon });  // 创建标注
        return marker;
    },
    GetText:function(point,text){
        var opts = {
            position: point,    // 指定文本标注所在的地理位置
            offset: new BMap.Size(30, -30)    //设置文本偏移量
        }
        var label = new BMap.Label(text, opts);  // 创建文本标注对象
        label.setStyle({
            color: "red",
            fontSize: "12px",
            height: "20px",
            lineHeight: "20px",
            fontFamily: "微软雅黑"
        });
        return label;
    },
    //显示infowindow
    ShowInfoWindow: function (title,content,point,width,height) {
        var opts = {
            width: 200,     // 信息窗口宽度
            height: 100,     // 信息窗口高度
            title: title, // 信息窗口标题
            enableMessage: true//设置允许信息窗发送短息
        }
        if (width) opts.width = width;
        if (height) opts.height = height;
        var infoWindow = new BMap.InfoWindow(content, opts);  // 创建信息窗口对象 
        RaoRao.BMap.Map2DControl.openInfoWindow(infoWindow, point); //开启信息窗口
    },
    //添加地图右键菜单
    addContextMenu: function (txtMenuItem) {
        var menu = new BMap.ContextMenu();
        //var txtMenuItem = [
        //    {
        //        text: '放大',
        //        callback: function () { map.zoomIn() }
        //    },
        //    {
        //        text: '缩小',
        //        callback: function () { map.zoomOut() }
        //    }
        //];
        for (var i = 0; i < txtMenuItem.length; i++) {
            menu.addItem(new BMap.MenuItem(txtMenuItem[i].text, txtMenuItem[i].callback, 100));
        }
        RaoRao.BMap.Map2DControl.addContextMenu(menu);
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