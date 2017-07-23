/*初始化Page命名空间*/

if (!this["RaoRao.Portal"]) { RaoRao.Portal = {}; }
if (!this["RaoRao.Portal.Map"]) { RaoRao.Portal.Map = {}; }

function GetMaxLength(myCatchPipleLineResult, returnObjArry, colIndex) {
    var maxLength = 0;
    if (returnObjArry) {
        if (returnObjArry[colIndex].length > maxLength) {
            maxLength = returnObjArry[colIndex].length;
        }
    }
    if (myCatchPipleLineResult) {
        for (var i = 0; i < myCatchPipleLineResult.length; i++) {
            if (myCatchPipleLineResult[i][returnObjArry[colIndex]] && myCatchPipleLineResult[i][returnObjArry[colIndex]].length > maxLength) {
                maxLength = myCatchPipleLineResult[i][returnObjArry[colIndex]].length;
            }
        }
    }
    return maxLength;
}

RaoRao.Portal.Map.MarkInfoOnPoint = function (returnObjArry, pointX, pointY, pointInfo, type, myCatchPipeLineResult) {
    var txtConfig = {
        font: {
            size: "10",
            style: "normal",
            family: "微软雅黑"
        },
        color: [0, 0, 0],
        offset: {
            //x: 0,
            //y: -20
            x: 0,
            y: +40
        },
        text: JSON.stringify(returnObjArry)
    };
    var picConfig = {
        pic: {
            width: 0,
            height: 40 //每加一行，多加20
        },
        offset: {
            x: 0,
            //y: -25  //每多加一行，减10
            y:+35//+35
        },
        imgUrl: "images/pb.png"
    };
    if (myCatchPipeLineResult) { //扯旗标注   returnObjArry 是个字符数组
        var cheQi = {};
        cheQi.type = type;
        cheQi.returnObjArry = returnObjArry;
        cheQi.myCatchPipeLineResult = myCatchPipeLineResult;
        txtConfig.text = JSON.stringify(cheQi);
        picConfig.pic.height = picConfig.pic.height + (myCatchPipeLineResult.length - 1) * 20;
        //picConfig.offset.y = picConfig.offset.y - (myCatchPipeLineResult.length - 1) * 10;
        picConfig.offset.y = picConfig.offset.y + (myCatchPipeLineResult.length - 1) * 10;
        txtConfig.offset.y = txtConfig.offset.y + (myCatchPipeLineResult.length - 1) * 20;
        var maxLength = 0;
        for (var i = 0; i < returnObjArry.length; i++) {
            maxLength = maxLength + parseInt(GetMaxLength(myCatchPipeLineResult, returnObjArry, i)) * 10 + 20;
        }
        picConfig.pic.width = maxLength;  //需要计算
        picConfig.offset.x = parseInt(picConfig.pic.width) / 2 - 10;
    }
    else {
        for (var i = 0; i < returnObjArry.length; i++) {
            picConfig.pic.width = parseInt(picConfig.pic.width) + parseInt(returnObjArry[i].maxLength) * 10 + 20;
            picConfig.offset.x = parseInt(picConfig.pic.width) / 2 - 10;
        }
        picConfig.pic.width = parseInt(picConfig.pic.width);
    }
    var graphicLayer = RaoRao.Map.GetGraphicLayer(RaoRao.Map.Map2DControl, "pointMark");
    var textSymbol = new esri.symbol.TextSymbol(txtConfig.text, txtConfig.font, txtConfig.color).setOffset(txtConfig.offset.x, txtConfig.offset.y);
    var point = new esri.geometry.Point([parseFloat(pointX), parseFloat(pointY)], new esri.SpatialReference({
        wkid: RaoRao.Setting.GlobalSetting.wkid
    }));
    var pictureMarkerSymbol = new esri.symbol.PictureMarkerSymbol(picConfig.imgUrl, picConfig.pic.width, picConfig.pic.height).setOffset(picConfig.offset.x, picConfig.offset.y);
    graphic = new esri.Graphic(point, pictureMarkerSymbol, pointInfo, "pointMarkPicLayer");
    graphicLayer.add(graphic);
    //冒泡三角
    var pictureMarkerSymbol_t = new esri.symbol.PictureMarkerSymbol("images/triangle.png", 20, 20).setOffset(+10, +10);
    graphic = new esri.Graphic(point, pictureMarkerSymbol_t, pointInfo, "pointMarkPicLayer");
    graphicLayer.add(graphic);
    var graphic = new esri.Graphic(point, textSymbol, pointInfo, "pointMarkText");
    graphicLayer.add(graphic);
};

//地图查询
RaoRao.Portal.Map.Query = {
    IsDraw: false,
    TempGraphic:null,
    //点查询
    point_alllayer: function (callback) {
        var queryResult = [];
        var layerNames = [];
        RaoRao.Portal.Toolbar.DrawToolbar.activate(esri.toolbars.Draw.POINT);
        RaoRao.Portal.Toolbar.DrawHandler = dojo.connect(RaoRao.Portal.Toolbar.DrawToolbar, "onDrawEnd", function (evt) {
            RaoRao.Portal.Map.Query.drawEndForQuery(queryResult, layerNames, evt, callback);
        });
    },
    //矩形查询
    rectange_alllayer: function (callback) {
        var queryResult = [];
        var layerNames = [];
        RaoRao.Portal.Toolbar.DrawToolbar.activate(esri.toolbars.Draw.EXTENT);
        RaoRao.Portal.Toolbar.DrawHandler = dojo.connect(RaoRao.Portal.Toolbar.DrawToolbar, "onDrawEnd", function (evt) {
            RaoRao.Portal.Map.Query.drawEndForQuery(queryResult, layerNames, evt, callback);
        });
        //RaoRao.Portal.Toolbar.DrawToolbar.on("draw-end", function (evt) {
        //    RaoRao.Portal.Map.Query.drawEndForQuery(queryResult, layerNames, evt, callback);
        //});
    },
    //多边形查询
    polygon_alllayer: function (callback) {
        var queryResult = [];
        var layerNames = [];
        RaoRao.Portal.Toolbar.DrawToolbar.activate(esri.toolbars.Draw.POLYGON);
        RaoRao.Portal.Toolbar.DrawHandler = dojo.connect(RaoRao.Portal.Toolbar.DrawToolbar, "onDrawEnd", function (evt) {
            RaoRao.Portal.Map.Query.drawEndForQuery(queryResult, layerNames, evt, callback);
        });
        //RaoRao.Portal.Toolbar.DrawToolbar.on("draw-end", function (evt) {
        //    RaoRao.Portal.Map.Query.drawEndForQuery(queryResult, layerNames, evt, callback);
        //});
    },
    //圆形查询
    circle_alllayer: function (callback) {
        var queryResult = [];
        var layerNames = [];
        RaoRao.Portal.Toolbar.DrawToolbar.activate(esri.toolbars.Draw.CIRCLE);
        RaoRao.Portal.Toolbar.DrawHandler = dojo.connect(RaoRao.Portal.Toolbar.DrawToolbar, "onDrawEnd", function (evt) {
            RaoRao.Portal.Map.Query.drawEndForCircleQuery(queryResult, layerNames, evt, callback);
        });
        //RaoRao.Portal.Toolbar.DrawToolbar.on("draw-end", function (evt) {
        //    RaoRao.Portal.Map.Query.drawEndForQuery(queryResult, layerNames, evt, callback);
        //});
    },
    //矩形查询
    rectange: function (callback) {
        var queryResult = [];
        var layerNames = [];
        RaoRao.Portal.Toolbar.DrawToolbar.activate(esri.toolbars.Draw.EXTENT);
        RaoRao.Portal.Toolbar.DrawHandler = dojo.connect(RaoRao.Portal.Toolbar.DrawToolbar, "onDrawEnd", function (evt) {
            RaoRao.Portal.Map.Query.drawEnd(queryResult, layerNames, evt, callback, "gx");
        });
        //RaoRao.Portal.Toolbar.DrawToolbar.on("draw-end", function (evt) {
        //    RaoRao.Portal.Map.Query.drawEnd(queryResult, layerNames, evt, callback, "gx");
        //});
    },

    //线查询 chenruoxi
    polyline: function (callback) {
        var queryResult = [];
        var layerNames = [];
        RaoRao.Portal.Toolbar.DrawToolbar.activate(esri.toolbars.Draw.LINE);
        RaoRao.Portal.Toolbar.DrawHandler = dojo.connect(RaoRao.Portal.Toolbar.DrawToolbar, "onDrawEnd", function (evt) {
            $("svg").css("cursor", "default"); 
            if (RaoRao.Portal.Map.Query.IsDraw)
            {
                var polylineSymbol = new esri.symbol.SimpleLineSymbol(esri.symbol.SimpleLineSymbol.STYLE_SOLID, new dojo.Color([225, 0, 0]), 2);
                var graphic = new esri.Graphic(evt, polylineSymbol);
                RaoRao.Map.Map2DControl.graphics.add(graphic);
                RaoRao.Portal.Map.Query.TempGraphic = graphic;
                //RaoRao.Portal.Map.Query.IsDraw = false;
            }
            RaoRao.Portal.Map.Query.drawEnd(queryResult, layerNames, evt, callback, "gx");
        });
        //RaoRao.Portal.Toolbar.DrawToolbar.on("draw-end", function (evt) {
        //    $("svg").css("cursor", "default");
        //    RaoRao.Portal.Map.Query.drawEnd(queryResult, layerNames, evt, callback, "gx");
        //});
    },
    //查询
    queryTask: function (url, geometry, whereCase, outFields, returnGeometry, callback) {

        var queryTask = new esri.tasks.QueryTask(url);
        // 创建查询
        var query = new esri.tasks.Query();
        query.outFields = outFields;
        query.returnGeometry = returnGeometry;
        if (geometry) {
            query.geometry = geometry;
        }
        if (whereCase) {
            query.where = whereCase;
        }
        // 执行查询任务
        queryTask.execute(
            query,
             // 查询成功
            function (data) {
                callback(data);
            },
            function (err) {
                noty({ text: err, type: "error", layout: "topCenter" });
            });
    },
    // modify by chenruoxi
    drawEnd: function (queryResult, layerNames, evt, callback, arcGISMapServicesGx) {
        queryResult = [];
        //框选查询结果
        var geometry = evt;
        var identify, identifyParams;
        var count = 0;

        for (var i = 0; i < RaoRao.Setting.ArcGISMapServices.length; i++) {
            if (arcGISMapServicesGx && RaoRao.Setting.ArcGISMapServices[i].id != arcGISMapServicesGx) {
                count++;
                continue;
            }
            identify = new esri.tasks.IdentifyTask(RaoRao.Setting.ArcGISMapServices[i].url);
            identifyParams = new esri.tasks.IdentifyParameters();
            identifyParams.geometry = geometry;
            identifyParams.mapExtent = RaoRao.Map.Map2DControl.extent;
            identifyParams.width = RaoRao.Map.Map2DControl.width;
            identifyParams.height = RaoRao.Map.Map2DControl.height;
            identifyParams.returnGeometry = true;
            identifyParams.tolerance = 3;
            identifyParams.spatialReference = new esri.SpatialReference({ wkid: RaoRao.Setting.GlobalSetting.wkid });
            identifyParams.layerOption = esri.tasks.IdentifyParameters.LAYER_OPTION_ALL;

            identify.execute(identifyParams, function (result) {
                $.each(result, function (ii, vv) {
                    queryResult.push({
                        "layerID": vv.layerId, "layerName": vv.layerName, "attributes": vv.feature.attributes, "geometry": vv.feature.geometry, "url": RaoRao.Setting.ArcGISMapServices[count].url, "geometryExtend": geometry
                    });
                    //已经存在                      
                    if (!layerNames.Contains(vv.layerName)) {
                        layerNames.push(vv.layerName);
                    }
                });
                count++;
                if (count == RaoRao.Setting.ArcGISMapServices.length) {

                    callback(queryResult, layerNames, geometry);
                }
            },
            function (err) {
                noty({ text: err, type: "error", layout: "topCenter" });
            });
        }
    },
    // modify by jianglina
    drawEndForQuery: function (queryResult, layerNames, evt, callback) {
        queryResult = [];
        //框选查询结果
        var geometry = evt;
        //var identify, identifyParams;
        //var count = 0;
        //var layerTypes = [];
        //var layerInfos = [];
        //for (var i = 0; i < RaoRao.Setting.ArcGISMapServices.length; i++) {
        //    identify = new esri.tasks.IdentifyTask(RaoRao.Setting.ArcGISMapServices[i].url);
        //    identifyParams = new esri.tasks.IdentifyParameters();
        //    identifyParams.geometry = geometry;
        //    identifyParams.mapExtent = RaoRao.Map.Map2DControl.extent;
        //    identifyParams.width = RaoRao.Map.Map2DControl.width;
        //    identifyParams.height = RaoRao.Map.Map2DControl.height;
        //    identifyParams.returnGeometry = true;
        //    identifyParams.tolerance = 3;
        //    identifyParams.spatialReference = new esri.SpatialReference({ wkid: RaoRao.Setting.GlobalSetting.wkid });
        //    identifyParams.layerOption = esri.tasks.IdentifyParameters.LAYER_OPTION_ALL;
        //    identify.execute(identifyParams, function (result) {
        //        $.each(result, function (ii, vv) {
        //            queryResult.push({
        //                "layerID": vv.layerId, "layerName": vv.layerName, "attributes": vv.feature.attributes, "geometry": vv.feature.geometry, "url": RaoRao.Setting.ArcGISMapServices[count].url, "layertype": RaoRao.Setting.ArcGISMapServices[count].name
        //            });
        //            //已经存在                      
        //            if (!layerNames.Contains(vv.layerName)) {
        //                layerNames.push(vv.layerName);
        //                layerInfos.push([vv.layerName, vv.layerId, RaoRao.Setting.ArcGISMapServices[count].name]);
        //            }
        //            //已经存在                      
        //            if (!layerTypes.Contains(RaoRao.Setting.ArcGISMapServices[count].name)) {
        //                layerTypes.push(RaoRao.Setting.ArcGISMapServices[count].name);
        //            }
        //        });
        //        count++;
        //        if (count == RaoRao.Setting.ArcGISMapServices.length) {
        //            callback(queryResult, layerInfos, layerTypes);
        //        }
        //    },
        //    function (err) {
        //        noty({ text: err, type: "error", layout: "topCenter" });
        //    });
        //}
        Query("1=1", geometry, callback);
    },
    // modify by wt
    drawEndForCircleQuery: function (queryResult, layerNames, evt, callback) {
        queryResult = [];
        //框选查询结果
        var geometry = evt;
        var identify, identifyParams;
        var count = 0;
        var layerTypes = [];
        var layerInfos = [];
        //esriConfig.defaults.io.proxyUrl = RaoRao.Setting.GlobalSetting.proxyUrl;//数据量大使用代理
        //esriConfig.defaults.io.alwaysUseProxy = true;
        for (var i = 0; i < RaoRao.Setting.ArcGISMapServices.length; i++) {
            identify = new esri.tasks.IdentifyTask(RaoRao.Setting.ArcGISMapServices[i].url);
            identifyParams = new esri.tasks.IdentifyParameters();
            identifyParams.geometry = geometry;
            identifyParams.mapExtent = RaoRao.Map.Map2DControl.extent;
            identifyParams.width = RaoRao.Map.Map2DControl.width;
            identifyParams.height = RaoRao.Map.Map2DControl.height;
            identifyParams.returnGeometry = true;
            identifyParams.tolerance = 3;
            identifyParams.spatialReference = new esri.SpatialReference({ wkid: RaoRao.Setting.GlobalSetting.wkid });
            identifyParams.layerOption = esri.tasks.IdentifyParameters.LAYER_OPTION_ALL;

            identify.execute(identifyParams, function (result) {
                $.each(result, function (ii, vv) {
                    queryResult.push({
                        "layerID": vv.layerId, "layerName": vv.layerName, "attributes": vv.feature.attributes, "geometry": vv.feature.geometry, "url": RaoRao.Setting.ArcGISMapServices[count].url, "layertype": RaoRao.Setting.ArcGISMapServices[count].name
                    });
                    //已经存在                      
                    if (!layerNames.Contains(vv.layerName)) {
                        layerNames.push(vv.layerName);
                        layerInfos.push([vv.layerName, vv.layerId, RaoRao.Setting.ArcGISMapServices[count].name]);
                    }
                    //已经存在                      
                    if (!layerTypes.Contains(RaoRao.Setting.ArcGISMapServices[count].name)) {
                        layerTypes.push(RaoRao.Setting.ArcGISMapServices[count].name);
                    }
                });
                count++;
                if (count == RaoRao.Setting.ArcGISMapServices.length) {
                    callback(queryResult, layerInfos, layerTypes);
                    RaoRao.Portal.Toolbar.DrawToolbar.deactivate();
                }
            },
            function (err) {
                RaoRao.Portal.Toolbar.DrawToolbar.deactivate();
                //noty({ text: err, type: "error", layout: "topCenter" });
            });
        }
    },
    // chenruoxi
    queryTaskWithOutLoder: function (url, geometry, whereCase, outFields, returnGeometry, func) {
        var queryTask = new esri.tasks.QueryTask(url);
        // 创建查询
        var query = new esri.tasks.Query();
        query.outFields = outFields;
        query.returnGeometry = returnGeometry;
        if (geometry) {
            query.geometry = geometry;
        }
        if (whereCase) {
            query.where = whereCase;
        }
        // 执行查询任务
        queryTask.execute(
            query,
             // 查询成功
            function (data) {
                func(data);
            },
            function (err) {
                alert("查询失败");
            }
        );
    }
};
//图形绘制
RaoRao.Portal.Map.DrawGeometry = {
    IsDraw: false,
    TempGraphic: null,
    //画点
    point: function (callback) {
        var queryResult = [];
        var layerNames = [];
        RaoRao.Portal.Toolbar.DrawToolbar.activate(esri.toolbars.Draw.POINT);
        RaoRao.Portal.Toolbar.DrawHandler = dojo.connect(RaoRao.Portal.Toolbar.DrawToolbar, "onDrawEnd", function (evt) {
            var symbol=RaoRao.Map.Symbol.markSymbol();
            var graphic = RaoRao.Map.GetGraphic(evt, symbol);
            RaoRao.Map.Map2DControl.graphics.add(graphic);
            callback(evt);
        });
    },
    //画矩形
    rectange: function (callback) {
        var queryResult = [];
        var layerNames = [];
        RaoRao.Portal.Toolbar.DrawToolbar.activate(esri.toolbars.Draw.EXTENT);
        RaoRao.Portal.Toolbar.DrawHandler = dojo.connect(RaoRao.Portal.Toolbar.DrawToolbar, "onDrawEnd", function (evt) {
            var symbol = RaoRao.Map.Symbol.fillSymbol();
            var graphic = RaoRao.Map.GetGraphic(evt, symbol);
            RaoRao.Map.Map2DControl.graphics.add(graphic);
            callback(evt);
        });
        
    },
    //画多边形
    polygon: function (callback) {
        var queryResult = [];
        var layerNames = [];
        RaoRao.Portal.Toolbar.DrawToolbar.activate(esri.toolbars.Draw.POLYGON);
        RaoRao.Portal.Toolbar.DrawHandler = dojo.connect(RaoRao.Portal.Toolbar.DrawToolbar, "onDrawEnd", function (evt) {
            var symbol = RaoRao.Map.Symbol.fillSymbol();
            var graphic = RaoRao.Map.GetGraphic(evt, symbol);
            RaoRao.Map.Map2DControl.graphics.add(graphic);
            callback(evt);
        });
    },
    //画圆形
    circle: function (callback) {
        var queryResult = [];
        var layerNames = [];
        RaoRao.Portal.Toolbar.DrawToolbar.activate(esri.toolbars.Draw.CIRCLE);
        RaoRao.Portal.Toolbar.DrawHandler = dojo.connect(RaoRao.Portal.Toolbar.DrawToolbar, "onDrawEnd", function (evt) {
            var symbol = RaoRao.Map.Symbol.fillSymbol();
            var graphic = RaoRao.Map.GetGraphic(evt, symbol);
            RaoRao.Map.Map2DControl.graphics.add(graphic);
            callback(evt);
        });
    },
    //画矩形
    rectange: function (callback) {
        var queryResult = [];
        var layerNames = [];
        RaoRao.Portal.Toolbar.DrawToolbar.activate(esri.toolbars.Draw.EXTENT);
        RaoRao.Portal.Toolbar.DrawHandler = dojo.connect(RaoRao.Portal.Toolbar.DrawToolbar, "onDrawEnd", function (evt) {
            var symbol = RaoRao.Map.Symbol.fillSymbol();
            var graphic = RaoRao.Map.GetGraphic(evt, symbol);
            RaoRao.Map.Map2DControl.graphics.add(graphic);
            callback(evt);
        });
    },
    //画线
    polyline: function (callback) {
        var queryResult = [];
        var layerNames = [];
        RaoRao.Portal.Toolbar.DrawToolbar.activate(esri.toolbars.Draw.LINE);
        RaoRao.Portal.Toolbar.DrawHandler = dojo.connect(RaoRao.Portal.Toolbar.DrawToolbar, "onDrawEnd", function (evt) {
            var symbol = RaoRao.Map.Symbol.lineSymbol();
            var graphic = RaoRao.Map.GetGraphic(evt, symbol);
            RaoRao.Map.Map2DControl.graphics.add(graphic);
            callback(evt);
        });
    }
};
/**
 * @constructor 名称：MapPrint
 * @description 作用：实现地图打印
 * @param name  模块名称
 * @param containerid 容器id
 * @param css  依赖的css
 * @param callback  回调函数
 * @return {Array} 处理后的数组
 * @author 陈兴旺 
 */
RaoRao.Portal.MapPrint = {
    //当前视图范围的地图打印
    ViewPrint: function () {

        var title = "打印当前视图内的地图";
        var url = "../pipecloud/view/print.htm";
        RaoRao.Window.InfoPanel(title, '<iframe src="' +
            url + '" width="100%" height="100%"  frameborder="0" scrolling="no" ></iframe>',
            { id: 'mapPrint', size: { width: 450, height: 300 } });

    },

    //拉框范围内的地图打印
    RectanglePrint: function () {
        //RaoRao.Portal.Toolbar.DrawToolbar.activate(esri.toolbars.Draw.EXTENT);
        //RaoRao.Portal.Toolbar.DrawHandler = dojo.connect(RaoRao.Portal.Toolbar.DrawToolbar, "onDrawEnd", function (evt) {
        //});
        require(["esri/toolbars/draw"], function (Draw) {
            //生成工具
            var exten = RaoRao.Map.Map2DControl.extent;
            if (toolbarPrint == null) {
                toolbarPrint = new Draw(RaoRao.Map.Map2DControl, {
                    tooltipOffset: 20,
                    drawTime: 90
                });
            }

            rectanglePrintFlag = 0;
            beforRectanglePrintExtent = RaoRao.Map.Map2DControl.extent;
            RaoRao.Map.Map2DControl.on("extent-change", StartPrint);

            toolbarPrint.on("draw-complete", function (evt) {
                RaoRao.Map.Map2DControl.setExtent(evt.geometry);
            });

            toolbarPrint.activate(Draw.EXTENT);


        });

        function StartPrint() {
            toolbarPrint.deactivate();
            if (rectanglePrintFlag == 0) {
                var title = "打印框选内的地图";
                var url = "../pipecloud/view/print.htm";
                RaoRao.Window.InfoPanel(title, '<iframe src="' +
                    url + '" width="100%" height="100%"  frameborder="0" scrolling="no" ></iframe>',
                    { id: 'mapPrint', size: { width: 450, height: 300 } });
                top.rectanglePrintFlag = 1;
            }
        }

    }

};

/**
 * @constructor 名称：MapOperate
 * @description 作用：实现地图相关的操作功能
 * @param name  模块名称
 * @param containerid 容器id
 * @param css  依赖的css
 * @param callback  回调函数
 * @return {Array} 处理后的数组
 * @author 陈兴旺 
 */
RaoRao.Portal.overviewMapDijit = "";

var beforRectanglePrintExtent;//用来存储拉框打印前的地图视图范围
var rectanglePrintFlag = 0;//地图打印的表示操作符
var toolbarPrint;//矩形框打印地图时，绘制矩形框的工具
var oDivs;
var mapMouseMove, mapExtentChange;//绑定鼠标移动的事件时返回的结果，在用来解除事件绑定时使用


RaoRao.Portal.MapOperate = {
    //地图鹰眼
    overviewMapFunction: function () {
        if (RaoRao.Portal.overviewMapDijit == "") {
            require(["esri/dijit/OverviewMap",
			   "dijit/layout/BorderContainer", "dijit/layout/ContentPane", "dojo/domReady!"
            ], function () {
                RaoRao.Portal.overviewMapDijit = new esri.dijit.OverviewMap({
                    map: RaoRao.Map.Map2DControl,
                    attachTo: "bottom-right",
                    visible: true
                });
                RaoRao.Portal.overviewMapDijit.startup();
            });
        } else {
            RaoRao.Portal.overviewMapDijit.show();
        }
    },
    //地图居中
    map_centered: function () {
        require(["esri/geometry/Point", "esri/SpatialReference", "esri/layers/FeatureLayer"
        ], function (Point, SpatialReference, FeatureLayer) {
            var xmin = RaoRao.Setting.MapSetting.initExtent.xmin;
            var xmax = RaoRao.Setting.MapSetting.initExtent.xmax;
            var ymin = RaoRao.Setting.MapSetting.initExtent.ymin;
            var ymax = RaoRao.Setting.MapSetting.initExtent.ymax;
            var centerX = xmin + (xmax - xmin) / 2;
            var centerY = ymin + (ymax - ymin) / 2;
            var centerPoint = new Point(centerX, centerY, RaoRao.Map.Map2DControl.spatialReference);
            RaoRao.Map.Map2DControl.centerAt(centerPoint);
        });
    },
    map_fullshow: function (level) {
        require(["esri/geometry/Point", "esri/SpatialReference", "esri/layers/FeatureLayer"
        ], function (Point, SpatialReference, FeatureLayer) {
            var xmin = RaoRao.Setting.MapSetting.initExtent.xmin;
            var xmax = RaoRao.Setting.MapSetting.initExtent.xmax;
            var ymin = RaoRao.Setting.MapSetting.initExtent.ymin;
            var ymax = RaoRao.Setting.MapSetting.initExtent.ymax;
            var centerX = xmin + (xmax - xmin) / 2;
            var centerY = ymin + (ymax - ymin) / 2;
            var centerPoint = new Point(centerX, centerY, RaoRao.Map.Map2DControl.spatialReference);
            RaoRao.Map.Map2DControl.centerAndZoom(centerPoint, level);
        });
    }
}
/**
 * @constructor 名称：Toobar
 * @description 作用：工具条
 * @param type  工具类型：0-都禁用、1-地图导航工具条、2-绘图工具条
 * @author henry 
 */
RaoRao.Portal.Toolbar = {
    /*Navigation 地图导航工具条 */
    NavToolbar: null,
    /*Draw 地图绘图工具条 */
    DrawToolbar: null,
    /*DrawHandler 地图绘图连接句柄 */
    DrawEndHandler: null,
    /*Click 点击连接句柄 */
    ClickHandler: null,
    /*MouseOver 连接句柄 */
    MouseOverHandler: null,
    /*MouseMove 连接句柄 */
    MouseMoveHandler: null,
    /*document dblclick 句柄 */
    DblClickHandler: null,

    /*InitToobar 初始化工具条 */
    InitToobar: function () {
        if (RaoRao.Portal.Toolbar.DrawToolbar == null || !RaoRao.Portal.Toolbar.DrawToolbar.map) {
            RaoRao.Portal.Toolbar.DrawToolbar = new esri.toolbars.Draw(RaoRao.Map.Map2DControl);
        } else {
            RaoRao.Portal.Toolbar.DrawToolbar.deactivate();
            //关闭画图显示
            RaoRao.Portal.Map.Query.IsDraw = false;
            RaoRao.Portal.Map.DrawGeometry.IsDraw = false;
        }
        if (RaoRao.Portal.Toolbar.NavToolbar == null || !RaoRao.Portal.Toolbar.NavToolbar.map) {
            RaoRao.Portal.Toolbar.NavToolbar = new esri.toolbars.Navigation(RaoRao.Map.Map2DControl);
        } else {
            RaoRao.Portal.Toolbar.NavToolbar.deactivate();
        }
        if (RaoRao.Portal.Toolbar.DrawHandler) {
            dojo.disconnect(RaoRao.Portal.Toolbar.DrawHandler);
            RaoRao.Portal.Toolbar.DrawHandler = null;
        }
        if (RaoRao.Portal.Toolbar.ClickHandler) {
            dojo.disconnect(RaoRao.Portal.Toolbar.ClickHandler);
            RaoRao.Portal.Toolbar.ClickHandler = null;
        }
        if (RaoRao.Portal.Toolbar.MouseOverHandler) {
            dojo.disconnect(RaoRao.Portal.Toolbar.MouseOverHandler);
            RaoRao.Portal.Toolbar.MouseOverHandler = null;
        }
        if (RaoRao.Portal.Toolbar.MouseMoveHandler) {
            dojo.disconnect(RaoRao.Portal.Toolbar.MouseMoveHandler);
            RaoRao.Portal.Toolbar.MouseMoveHandler = null;
        }
        if (RaoRao.Portal.Toolbar.DblClickHandler) {
            dojo.disconnect(RaoRao.Portal.Toolbar.DblClickHandler);
            RaoRao.Portal.Toolbar.DblClickHandler = null;
        }
    }
}

/**
 * @constructor 名称：Location
 * @description 作用：地图定位
 * @param geometry 几何要素
 * @param addGraphic 是否添加图形
 * @author 陈兴旺 
 */
RaoRao.Portal.Location = function (geometry, addGraphic) {
    if (geometry == null) {
        return;
    }
    if (addGraphic)
        RaoRao.Portal.AddGraphic(geometry);

    switch (geometry.type) {
        case "extent": {
            if (geometry.spatialReference.wkid != RaoRao.Setting.GlobalSetting.wkid)
                geometry.setSpatialReference(new esri.SpatialReference({ wkid: RaoRao.Setting.GlobalSetting.wkid }));
            RaoRao.Map.Map2DControl.setExtent(geometry, true);
            break;
        }
        case "point": {
            if (geometry.spatialReference.wkid != RaoRao.Setting.GlobalSetting.wkid)
                geometry.setSpatialReference(new esri.SpatialReference({ wkid: RaoRao.Setting.GlobalSetting.wkid }));
            RaoRao.Map.Map2DControl.centerAndZoom(geometry, 1);
            break;
        }
        case "multipoint": {
            var xmin = geometry.points[0][0];
            var xmax = geometry.points[0][0];
            var ymin = geometry.points[0][1];
            var ymax = geometry.points[0][1];
            $.each(geometry.points, function (i, point) {
                xmin = xmin < point[0] ? xmin : point[0];
                xmax = xmax > point[0] ? xmax : point[0];
                ymin = ymin < point[1] ? ymin : point[1];
                ymax = ymax > point[1] ? ymax : point[1];
            });
            var extent = RaoRao.Portal.CreatGeomrtry.Extent(xmin, ymin, xmax, ymax);
            extent = extent.expand(1.5);
            RaoRao.Map.Map2DControl.setExtent(extent, true);
            break;
        }
        case "polyline":
        case "polygon": {
            var extent = geometry.getExtent().expand(1.5);
            if (extent.spatialReference.wkid != RaoRao.Setting.GlobalSetting.wkid)
                extent.setSpatialReference(new esri.SpatialReference({ wkid: RaoRao.Setting.GlobalSetting.wkid }));
            RaoRao.Map.Map2DControl.setExtent(extent, true);
            break;
        }
    }
};
/**
 * @constructor 名称：AddGraphic
 * @description 作用：添加临时图形
 * @param geometry 几何要素
 * @author 陈兴旺 
 */
RaoRao.Portal.AddGraphic = function (geometry) {
    if (geometry == null) {
        return;
    }
    switch (geometry.type) {
        case "extent": {
            break;
        }
        case "point": {
            var data = [];
            var myData = {};
            myData.X = geometry.x;
            myData.Y = geometry.y;
            data.push(myData);
            RaoRao.Portal.AddHighlightPoint(data, "../images/highLight.gif", "", "selectedHighLight");
            break;
        }
        case "multipoint": {
            var data = [];
            $.each(geometry.points, function (i, point) {
                var myData = {};
                myData.X = point[0];
                myData.Y = point[1];
                data.push(myData);
            });
            RaoRao.Portal.AddHighlightPoint(data, "../images/highLight.gif", "", "selectedHighLight");
            break;
        }
        case "polyline": {
            var polylineSymbol = new esri.symbol.SimpleLineSymbol(esri.symbol.SimpleLineSymbol.STYLE_SOLID, new dojo.Color([225, 33, 78]), 4);
            var graphic = new esri.Graphic(geometry, polylineSymbol);
            RaoRao.Map.Map2DControl.graphics.add(graphic);
            break;
        }
        case "polygon": {
            var polylineSymbol = new esri.symbol.SimpleLineSymbol(esri.symbol.SimpleLineSymbol.STYLE_SOLID, new dojo.Color([225, 33, 78]), 4);
            var polygonSymbol = new esri.symbol.SimpleFillSymbol(esri.symbol.SimpleFillSymbol.STYLE_SOLID, polylineSymbol, new dojo.Color([225, 33, 78, 0.25]));
            var graphic = new esri.Graphic(geometry, polygonSymbol);
            RaoRao.Map.Map2DControl.graphics.add(graphic);
            break;
        }
    }
};
/**
 * @constructor 名称：AddHighlightPoint
 * @description 作用：添加高亮临时点图形
 * @param data 点数据集合
 * @param picUrl 高亮图片连接
 * @param fieldName 字段名
 * @param layerName 图层名
 * @author 陈兴旺 
 */
RaoRao.Portal.AddHighlightPoint = function (data, picUrl, fieldName, layerName) {
    var config = {
        pic: {
            src: picUrl,
            width: 24,
            height: 24
        }
    };
    var graphicLayer = RaoRao.Map.GetGraphicLayer(RaoRao.Map.Map2DControl, layerName);
    $.each(data, function (i, item) {
        //地图渲染图标
        RaoRao.Map.ShowGraphic(graphicLayer, item[fieldName], item.X, item.Y, picUrl, item, config);
    });
};

// 标注排水流向 chenruoxi
RaoRao.Portal.HightlightDrainpipe = function (data, layerName) {
    var picConfig = {
        imgUrl: "../images/arrow.png",
        width: 16,
        height: 16
    }
    var graphicLayer = RaoRao.Map.GetGraphicLayer(RaoRao.Map.Map2DControl, layerName);

    $.each(data, function (i, item) {
        var point = new esri.geometry.Point([parseFloat(item.X), parseFloat(item.Y)], new esri.SpatialReference({
            wkid: RaoRao.Setting.GlobalSetting.wkid
        }));
        var pointInfo = {};
        pointInfo.X = parseFloat(parseFloat(item.X));
        pointInfo.Y = parseFloat(parseFloat(item.Y));
        pointInfo.ID = parseInt(100000 * Math.random());
        pointInfo.Rotate = RaoRao.Portal.GetDirection(item.Point1, item.Point2, item.Deriction);
        pointInfo.ImgUrl = picConfig.imgUrl;
        var pictureMarkerSymbol = new esri.symbol.PictureMarkerSymbol(JSON.stringify(pointInfo), picConfig.width, picConfig.height);
        graphic = new esri.Graphic(point, pictureMarkerSymbol, pointInfo, "pointMarkPicLayer");
        graphicLayer.add(graphic)
    });
}
// 计算排水流向方向 chenruoxi
RaoRao.Portal.GetDirection = function (point1, point2, deriction) {
    var x = Math.abs(point1[0] - point2[0]);
    var y = Math.abs(point1[1] - point2[1]);
    //var x = Math.abs(point2[0] - point1[0]);
    //var y = Math.abs(point2[1] - point1[1]);
    // 斜边长
    var z = Math.sqrt(Math.pow(x, 2) + Math.pow(y, 2));
    // 余弦
    var cos = y / z;
    // 弧度
    var radina = Math.acos(cos);
    // 角度
    var angle = 180 / (Math.PI / radina);
    if (point2[1] <= point1[1] && point2[0] >= point1[0]) {
        //return 90 - angle + 180;
        if (deriction == "+") {
            return 90 - angle;
        }
        else {
            return 90 - angle + 180;
        }
    }
    else if (point2[1] <= point1[1] && point2[0] <= point1[0]) {
        //return angle + 90 - 180;
        if (deriction == "+") {
            return angle + 90;
        }
        else {
            return angle + 90 - 180;
        }
    }
    else if (point2[1] >= point1[1] && point2[0] <= point1[0]) {
        //return 90 - angle;
        if (deriction == "+") {
            return 90 - angle + 180;
        }
        else {
            return 90 - angle;
        }
    }
    else if (point2[1] >= point1[1] && point2[0] >= point1[0]) {
        //return angle - 90 + 180;
        if (deriction == "+") {
            return angle - 90;
        }
        else {
            return angle - 90 + 180;
        }
    }
    //return parseFloat(360 * Math.random());
}
/**
 * @constructor 名称：CreatGeomrtry
 * @description 作用：创建几何要素
 * @author 陈兴旺 
 */
RaoRao.Portal.CreatGeomrtry = {
    Extent: function (xmin, ymin, xmax, ymax) {
        var extent = new esri.geometry.Extent(xmin, ymin, xmax, ymax, new esri.SpatialReference({ wkid: top.RaoRao.Setting.GlobalSetting.wkid }));
        return extent;
    },
    ExtentByObject: function (obj) {
        var extent = new esri.geometry.Extent(obj.xmin, obj.ymin, obj.xmax, obj.ymax, new esri.SpatialReference({ wkid: top.RaoRao.Setting.GlobalSetting.wkid }));
        return extent;
    },
    Point: function (x, y) {
        var point = new esri.geometry.Point([x, y], new esri.SpatialReference({ wkid: RaoRao.Setting.GlobalSetting.wkid }));
        return point;
    },
    Polyline: function (x1, y1, x2, y2) {
        var polyline = new esri.geometry.Polyline([[[x1, y1], [x2, y2]]], new esri.SpatialReference({ wkid: RaoRao.Setting.GlobalSetting.wkid }));
        return polyline;
    }
}

/**
 * @constructor 名称：Measure
 * @description 作用：测量工具
 * @author 陈兴旺 
 */
RaoRao.Portal.Measure = {
    Text: null,
    //是否开启测量
    IsMeasure: false,
    //鼠标点击位置点列表
    stopPoints: [],
    //距离值列表
    stopDistances: [],
    //距离测量
    Distance: function (callback) {
        RaoRao.Map.ClearLayer(RaoRao.Map.Map2DControl, "graphicLayer");
        graphicLayer = RaoRao.Map.GetGraphicLayer(RaoRao.Map.Map2DControl, "graphicLayer");
        var textLayer = RaoRao.Map.GetGraphicLayer(RaoRao.Map.Map2DControl, "textLayer"); //动态显示距离图层    
        RaoRao.Map.Map2DControl.addLayer(textLayer);
        RaoRao.Portal.Toolbar.DrawToolbar.activate(esri.toolbars.Draw.POLYLINE);
        //鼠标点击事件
        RaoRao.Portal.Toolbar.ClickHandler = dojo.connect(RaoRao.Map.Map2DControl, "onClick", function (evt) {
            RaoRao.Portal.Measure.IsMeasure = true;
            var distance = 0;
            var stopPoint = evt.mapPoint;
            var startPoint = evt.mapPoint;
            var showpoint1 = stopPoint;
            var showpoint2 = startPoint;
            stopPoint = RaoRao.Map.Formatter.lonlat2mercator(stopPoint);
            startPoint = RaoRao.Map.Formatter.lonlat2mercator(stopPoint);
            if (RaoRao.Portal.Measure.stopPoints.length > 0) {
                startPoint = RaoRao.Portal.Measure.stopPoints[RaoRao.Portal.Measure.stopPoints.length - 1];
                require(["esri/geometry/geometryEngine"], function (geometryEngine) {
                    //旧方法
                    //distance = geometryEngine.distance(startPoint, stopPoint, RaoRao.Setting.GlobalSetting.wkid);
                    //新方法
                    var line = new esri.geometry.Polyline(RaoRao.Map.Map2DControl.spatialReference);
                    line.addPath([startPoint, stopPoint]);
                    if (RaoRao.Map.Map2DControl.spatialReference.isWebMercator() || RaoRao.Map.Map2DControl.spatialReference.wkid == "4326") {//在web麦卡托投影和WGS84坐标系下的计算方法
                        distance = geometryEngine.geodesicLength(line, "meters");
                    } else {//在其他投影坐标系下的计算方法
                        distance = geometryEngine.planarLength(line, "meters")
                    }

                    if (RaoRao.Portal.Measure.stopDistances.length > 0) {
                        distance += RaoRao.Portal.Measure.stopDistances[RaoRao.Portal.Measure.stopDistances.length - 1];
                    }
                    RaoRao.Portal.Measure.stopDistances.push(distance);
                });
            }
            var text = RaoRao.Portal.Measure.GetText(distance, "distance");
            RaoRao.Portal.Measure.Text = text;
            var textGraphic = RaoRao.Portal.Measure.GetPointGraphic(showpoint1, text);
            //graphicLayer.add(textGraphic);
            RaoRao.Map.Map2DControl.graphics.add(textGraphic);
            RaoRao.Portal.Measure.stopPoints.push(stopPoint);
            PointSymbol = new esri.symbol.SimpleMarkerSymbol();
            PointSymbol.style = esri.symbol.SimpleMarkerSymbol.STYLE_CIRCLE;
            PointSymbol.setSize(2);
            PointSymbol.setColor(new dojo.Color([255, 0, 0]));
            var stopGraphic = new esri.Graphic(showpoint1, PointSymbol);
            //graphicLayer.add(stopGraphic);
            RaoRao.Map.Map2DControl.graphics.add(stopGraphic);
            var polyline = new esri.geometry.Polyline();
            polyline.addPath([showpoint2, showpoint1]);
            symbol = new esri.symbol.SimpleLineSymbol(esri.symbol.SimpleLineSymbol.STYLE_SOLID, new dojo.Color([255, 0, 0]), 2);
            var graphic = new esri.Graphic(polyline, symbol);
            //graphicLayer.add(graphic);
            RaoRao.Map.Map2DControl.graphics.add(graphic);
            textLayer.clear();
            textLayer.show();
        });
        //鼠标移动事件
        RaoRao.Portal.Toolbar.MouseMoveHandler = dojo.connect(RaoRao.Map.Map2DControl, "onMouseMove", function (evt) {
            if (!RaoRao.Portal.Measure.IsMeasure)
                return;
            var distancetext = 0;
            var stopPoint = evt.mapPoint;
            stopPoint = RaoRao.Map.Formatter.lonlat2mercator(stopPoint);
            if (RaoRao.Portal.Measure.stopPoints.length > 0) {
                var startPoint = RaoRao.Portal.Measure.stopPoints[RaoRao.Portal.Measure.stopPoints.length - 1];
                require(["esri/geometry/geometryEngine"], function (geometryEngine) {
                    //distancetext = geometryEngine.distance(startPoint, stopPoint, RaoRao.Setting.GlobalSetting.wkid);//4490 9001 meters
                    //新方法
                    var line = new esri.geometry.Polyline(RaoRao.Map.Map2DControl.spatialReference);
                    line.addPath([startPoint, stopPoint]);
                    if (RaoRao.Map.Map2DControl.spatialReference.isWebMercator() || RaoRao.Map.Map2DControl.spatialReference.wkid == "4326") {//在web麦卡托投影和WGS84坐标系下的计算方法
                        distancetext = geometryEngine.geodesicLength(line, "meters");
                    } else {//在其他投影坐标系下的计算方法
                        distancetext = geometryEngine.planarLength(line, "meters")
                    }

                    //console.log(distancetext);
                    if (RaoRao.Portal.Measure.stopDistances.length > 0) {
                        distancetext += RaoRao.Portal.Measure.stopDistances[RaoRao.Portal.Measure.stopDistances.length - 1];
                    }
                    var text = RaoRao.Portal.Measure.GetText(distancetext, "distance");
                    textGraphictext = RaoRao.Portal.Measure.GetPointGraphic(stopPoint, text);
                    textLayer.clear();
                    textLayer.add(textGraphictext);
                });
            }
        });
        //测量完成事件
        RaoRao.Portal.Toolbar.DrawHandler = dojo.connect(RaoRao.Portal.Toolbar.DrawToolbar, "onDrawEnd",function(geometry){
            RaoRao.Portal.Measure.GetLengthorArea(geometry, callback);
        } );
    },
    //面积测量
    Area: function (callback) {
        RaoRao.Portal.Toolbar.DrawToolbar.activate(esri.toolbars.Draw.POLYGON);
        var textLayer = RaoRao.Map.GetGraphicLayer(RaoRao.Map.Map2DControl, "textLayer"); //动态显示距离图层    
        RaoRao.Portal.Measure.stopPoints = [];
        //鼠标点击事件
        RaoRao.Portal.Toolbar.ClickHandler = dojo.connect(RaoRao.Map.Map2DControl, "onClick", function (evt) {
            RaoRao.Portal.Measure.IsMeasure = true;
            var point = RaoRao.Map.Formatter.lonlat2mercator(evt.mapPoint);
            RaoRao.Portal.Measure.stopPoints.push([point.x, point.y]);
            textLayer.clear();
        });
        //鼠标移动事件
        RaoRao.Portal.Toolbar.MouseMoveHandler = dojo.connect(RaoRao.Map.Map2DControl, "onMouseMove", function (evtTEXT) {
            if (!RaoRao.Portal.Measure.IsMeasure)
                return;
            var measureArea = 0;
            var paths = [];
            var point = RaoRao.Map.Formatter.lonlat2mercator(evtTEXT.mapPoint);
            paths.push([point.x, point.y]);
            var paths1 = paths.concat(RaoRao.Portal.Measure.stopPoints);
            var polygon = new esri.geometry.Polygon(paths1);

            require(["esri/geometry/geometryEngine"], function (geometryEngine) {
                var spatialReference = RaoRao.Map.Map2DControl.spatialReference;
                if (spatialReference.isWebMercator() || spatialReference.wkid == "4326") {
                    measureArea = geometryEngine.geodesicArea(polygon, "square-meters")
                } else {
                    measureArea = geometryEngine.planarArea(polygon, "square-meters")
                }
                measureArea = Math.abs(parseFloat(measureArea));
                var text = RaoRao.Portal.Measure.GetText(measureArea, "area");
                RaoRao.Portal.Measure.Text = text;
                var textGraphic = RaoRao.Portal.Measure.GetPointGraphic(evtTEXT.mapPoint, text);
                textLayer.clear();
                textLayer.add(textGraphic);
            });

            //var showPnt = polygon.getCentroid();
            //var geo = esri.geometry.webMercatorToGeographic(polygon);
            //measureArea = esri.geometry.geodesicAreas([geo], esri.Units.SQUARE_METERS);
            //measureArea = Math.abs(parseFloat(measureArea[0]));
            //var text = RaoRao.Portal.Measure.GetText(measureArea, "area");
            //RaoRao.Portal.Measure.Text = text;
            //var textGraphic = RaoRao.Portal.Measure.GetPointGraphic(evtTEXT.mapPoint, text);
            //textLayer.clear();
            //textLayer.add(textGraphic);
        });
        //测量完成事件
        RaoRao.Portal.Toolbar.DrawHandler = dojo.connect(RaoRao.Portal.Toolbar.DrawToolbar, "onDrawEnd",function(geometry){
            RaoRao.Portal.Measure.GetLengthorArea(geometry, callback);
        });
    },
    //计算距离或面积
    GetLengthorArea: function (geometry, callback) {
        RaoRao.Portal.Measure.IsMeasure = false;
        RaoRao.Portal.Measure.stopPoints = [];
        RaoRao.Portal.Measure.stopDistances = [];

        var paths;
        if (geometry.type == "polyline") {
            paths = geometry.paths[0];
        }
        else if (geometry.type == "polygon") {
            paths = geometry.rings[0];
        }
        var point = paths[paths.length - 1];
        var showPnt = new esri.geometry.Point(point[0], point[1]);
        var measureArea = 0;
        var measureLength = 0;
        var symbol = null;
        if (geometry.type == "polyline") {
            symbol = new esri.symbol.SimpleLineSymbol(esri.symbol.SimpleLineSymbol.STYLE_SOLID, new dojo.Color([255, 0, 0]), 2);
        }
        else if (geometry.type == "polygon") {
            var pLineSymbol = new esri.symbol.SimpleLineSymbol(esri.symbol.SimpleLineSymbol.STYLE_SOLID, new dojo.Color([255, 0, 0]), 2);
            symbol = new esri.symbol.SimpleFillSymbol(esri.symbol.SimpleFillSymbol.STYLE_SOLID, pLineSymbol, new dojo.Color([255, 0, 0, 0.15]));
        }
        //添加测量轨迹
        graphicsLayer = RaoRao.Map.GetGraphicLayer(RaoRao.Map.Map2DControl, "graphicLayer");
        var graphic = new esri.Graphic(geometry, symbol);
        //graphicsLayer.add(graphic);
        RaoRao.Map.Map2DControl.graphics.add(graphic);
        if (geometry.type == "polyline") {
            //var length = measureLength[0];
            //var lengthtext = RaoRao.Portal.Measure.GetText(length, "distance");
        }
        else {
            var area = measureArea[0];
            var areatext = RaoRao.Portal.Measure.GetText(area, "area");
            //计算多边形中心
            var polygon = new esri.geometry.Polygon(paths);
            showPnt = polygon.getCentroid();
            areatext = RaoRao.Portal.Measure.Text;
            var graphic = RaoRao.Portal.Measure.GetPointGraphic(showPnt, "面积：" + areatext);
            //graphicsLayer.add(graphic);
            RaoRao.Map.Map2DControl.graphics.add(graphic);
        }
        if (callback != undefined && $.isFunction(callback)) {
            callback(geometry, RaoRao.Portal.Measure.Text)
        }
    },
    //创建textSymbol--jianglina
    CreateTextSymbol: function (text) {
        var fontColor = new dojo.Color("#ff0000");
        var holoColor = new dojo.Color("#fff");
        var font = new esri.symbol.Font("14pt", esri.symbol.Font.STYLE_ITALIC, esri.symbol.Font.VARIANT_NORMAL, esri.symbol.Font.WEIGHT_BOLD, "Courier");
        var textSymbol = new esri.symbol.TextSymbol(text, font, fontColor);
        textSymbol.setOffset(10, 10);
        textSymbol.setAlign(esri.symbol.TextSymbol.ALIGN_MIDDLE);
        return textSymbol;
    },
    //获取测量点的Graphics--jianglina
    GetPointGraphic: function (point, text) {
        var textSymbol = RaoRao.Portal.Measure.CreateTextSymbol(text);
        return new esri.Graphic(point, textSymbol);
    },
    //数值转换成距离/面积文本
    GetText: function (number, type) {
        var text = number;
        var value = parseFloat(number)
        if (type == "distance") {
            if (value > 1000) {
                text = (value / 1000).toFixed(2) + "千米";
            } else {
                text = value.toFixed() + "米";
            }
        }
        else if (type == "area") {
            if (value > 1000000) {
                text = (value / 1000000).toFixed(2) + "平方千米";
            } else {
                text = value.toFixed() + "平方米";
            }
        }
        return text;
    }
};
/**
 * @constructor 名称：Measure
 * @description 作用：测量工具
 * @author 陈兴旺 
 */
RaoRao.Portal.Measure = {
    Text: null,
    //是否开启测量
    IsMeasure: false,
    //鼠标点击位置点列表
    stopPoints: [],
    //距离值列表
    stopDistances: [],
    //距离测量
    Distance: function (callback) {
        RaoRao.Map.ClearLayer(RaoRao.Map.Map2DControl, "graphicLayer");
        graphicLayer = RaoRao.Map.GetGraphicLayer(RaoRao.Map.Map2DControl, "graphicLayer");
        var textLayer = RaoRao.Map.GetGraphicLayer(RaoRao.Map.Map2DControl, "textLayer"); //动态显示距离图层    
        RaoRao.Map.Map2DControl.addLayer(textLayer);
        RaoRao.Portal.Toolbar.DrawToolbar.activate(esri.toolbars.Draw.POLYLINE);
        //鼠标点击事件
        RaoRao.Portal.Toolbar.ClickHandler = dojo.connect(RaoRao.Map.Map2DControl, "onClick", function (evt) {
            RaoRao.Portal.Measure.IsMeasure = true;
            var distance = 0;
            var stopPoint = evt.mapPoint;
            var startPoint = evt.mapPoint;
            var showpoint1 = stopPoint;
            var showpoint2 = startPoint;
            stopPoint = RaoRao.Map.Formatter.lonlat2mercator(stopPoint);
            startPoint = RaoRao.Map.Formatter.lonlat2mercator(stopPoint);
            if (RaoRao.Portal.Measure.stopPoints.length > 0) {
                startPoint = RaoRao.Portal.Measure.stopPoints[RaoRao.Portal.Measure.stopPoints.length - 1];
                require(["esri/geometry/geometryEngine"], function (geometryEngine) {
                    //旧方法
                    //distance = geometryEngine.distance(startPoint, stopPoint, RaoRao.Setting.GlobalSetting.wkid);
                    //新方法
                    var line = new esri.geometry.Polyline(RaoRao.Map.Map2DControl.spatialReference);
                    line.addPath([startPoint, stopPoint]);
                    if (RaoRao.Map.Map2DControl.spatialReference.isWebMercator() || RaoRao.Map.Map2DControl.spatialReference.wkid == "4326") {//在web麦卡托投影和WGS84坐标系下的计算方法
                        distance = geometryEngine.geodesicLength(line, "meters");
                    } else {//在其他投影坐标系下的计算方法
                        distance = geometryEngine.planarLength(line, "meters")
                    }

                    if (RaoRao.Portal.Measure.stopDistances.length > 0) {
                        distance += RaoRao.Portal.Measure.stopDistances[RaoRao.Portal.Measure.stopDistances.length - 1];
                    }
                    RaoRao.Portal.Measure.stopDistances.push(distance);
                });
            }
            var text = RaoRao.Portal.Measure.GetText(distance, "distance");
            RaoRao.Portal.Measure.Text = text;
            var textGraphic = RaoRao.Portal.Measure.GetPointGraphic(showpoint1, text);
            //graphicLayer.add(textGraphic);
            RaoRao.Map.Map2DControl.graphics.add(textGraphic);
            RaoRao.Portal.Measure.stopPoints.push(stopPoint);
            PointSymbol = new esri.symbol.SimpleMarkerSymbol();
            PointSymbol.style = esri.symbol.SimpleMarkerSymbol.STYLE_CIRCLE;
            PointSymbol.setSize(2);
            PointSymbol.setColor(new dojo.Color([255, 0, 0]));
            var stopGraphic = new esri.Graphic(showpoint1, PointSymbol);
            //graphicLayer.add(stopGraphic);
            RaoRao.Map.Map2DControl.graphics.add(stopGraphic);
            var polyline = new esri.geometry.Polyline();
            polyline.addPath([showpoint2, showpoint1]);
            symbol = new esri.symbol.SimpleLineSymbol(esri.symbol.SimpleLineSymbol.STYLE_SOLID, new dojo.Color([255, 0, 0]), 2);
            var graphic = new esri.Graphic(polyline, symbol);
            //graphicLayer.add(graphic);
            RaoRao.Map.Map2DControl.graphics.add(graphic);
            textLayer.clear();
            textLayer.show();
        });
        //鼠标移动事件
        RaoRao.Portal.Toolbar.MouseMoveHandler = dojo.connect(RaoRao.Map.Map2DControl, "onMouseMove", function (evt) {
            if (!RaoRao.Portal.Measure.IsMeasure)
                return;
            var distancetext = 0;
            var stopPoint = evt.mapPoint;
            stopPoint = RaoRao.Map.Formatter.lonlat2mercator(stopPoint);
            if (RaoRao.Portal.Measure.stopPoints.length > 0) {
                var startPoint = RaoRao.Portal.Measure.stopPoints[RaoRao.Portal.Measure.stopPoints.length - 1];
                require(["esri/geometry/geometryEngine"], function (geometryEngine) {
                    //distancetext = geometryEngine.distance(startPoint, stopPoint, RaoRao.Setting.GlobalSetting.wkid);//4490 9001 meters
                    //新方法
                    var line = new esri.geometry.Polyline(RaoRao.Map.Map2DControl.spatialReference);
                    line.addPath([startPoint, stopPoint]);
                    if (RaoRao.Map.Map2DControl.spatialReference.isWebMercator() || RaoRao.Map.Map2DControl.spatialReference.wkid == "4326") {//在web麦卡托投影和WGS84坐标系下的计算方法
                        distancetext = geometryEngine.geodesicLength(line, "meters");
                    } else {//在其他投影坐标系下的计算方法
                        distancetext = geometryEngine.planarLength(line, "meters")
                    }

                    //console.log(distancetext);
                    if (RaoRao.Portal.Measure.stopDistances.length > 0) {
                        distancetext += RaoRao.Portal.Measure.stopDistances[RaoRao.Portal.Measure.stopDistances.length - 1];
                    }
                    var text = RaoRao.Portal.Measure.GetText(distancetext, "distance");
                    textGraphictext = RaoRao.Portal.Measure.GetPointGraphic(stopPoint, text);
                    textLayer.clear();
                    textLayer.add(textGraphictext);
                });
            }
        });
        //测量完成事件
        RaoRao.Portal.Toolbar.DrawHandler = dojo.connect(RaoRao.Portal.Toolbar.DrawToolbar, "onDrawEnd",function(geometry){
            RaoRao.Portal.Measure.GetLengthorArea(geometry, callback);
        } );
    },
    //面积测量
    Area: function (callback) {
        RaoRao.Portal.Toolbar.DrawToolbar.activate(esri.toolbars.Draw.POLYGON);
        var textLayer = RaoRao.Map.GetGraphicLayer(RaoRao.Map.Map2DControl, "textLayer"); //动态显示距离图层    
        RaoRao.Portal.Measure.stopPoints = [];
        //鼠标点击事件
        RaoRao.Portal.Toolbar.ClickHandler = dojo.connect(RaoRao.Map.Map2DControl, "onClick", function (evt) {
            RaoRao.Portal.Measure.IsMeasure = true;
            var point = RaoRao.Map.Formatter.lonlat2mercator(evt.mapPoint);
            RaoRao.Portal.Measure.stopPoints.push([point.x, point.y]);
            textLayer.clear();
        });
        //鼠标移动事件
        RaoRao.Portal.Toolbar.MouseMoveHandler = dojo.connect(RaoRao.Map.Map2DControl, "onMouseMove", function (evtTEXT) {
            if (!RaoRao.Portal.Measure.IsMeasure)
                return;
            var measureArea = 0;
            var paths = [];
            var point = RaoRao.Map.Formatter.lonlat2mercator(evtTEXT.mapPoint);
            paths.push([point.x, point.y]);
            var paths1 = paths.concat(RaoRao.Portal.Measure.stopPoints);
            var polygon = new esri.geometry.Polygon(paths1);

            require(["esri/geometry/geometryEngine"], function (geometryEngine) {
                var spatialReference = RaoRao.Map.Map2DControl.spatialReference;
                if (spatialReference.isWebMercator() || spatialReference.wkid == "4326") {
                    measureArea = geometryEngine.geodesicArea(polygon, "square-meters")
                } else {
                    measureArea = geometryEngine.planarArea(polygon, "square-meters")
                }
                measureArea = Math.abs(parseFloat(measureArea));
                var text = RaoRao.Portal.Measure.GetText(measureArea, "area");
                RaoRao.Portal.Measure.Text = text;
                var textGraphic = RaoRao.Portal.Measure.GetPointGraphic(evtTEXT.mapPoint, text);
                textLayer.clear();
                textLayer.add(textGraphic);
            });

            //var showPnt = polygon.getCentroid();
            //var geo = esri.geometry.webMercatorToGeographic(polygon);
            //measureArea = esri.geometry.geodesicAreas([geo], esri.Units.SQUARE_METERS);
            //measureArea = Math.abs(parseFloat(measureArea[0]));
            //var text = RaoRao.Portal.Measure.GetText(measureArea, "area");
            //RaoRao.Portal.Measure.Text = text;
            //var textGraphic = RaoRao.Portal.Measure.GetPointGraphic(evtTEXT.mapPoint, text);
            //textLayer.clear();
            //textLayer.add(textGraphic);
        });
        //测量完成事件
        RaoRao.Portal.Toolbar.DrawHandler = dojo.connect(RaoRao.Portal.Toolbar.DrawToolbar, "onDrawEnd",function(geometry){
            RaoRao.Portal.Measure.GetLengthorArea(geometry, callback);
        });
    },
    //计算距离或面积
    GetLengthorArea: function (geometry, callback) {
        RaoRao.Portal.Measure.IsMeasure = false;
        RaoRao.Portal.Measure.stopPoints = [];
        RaoRao.Portal.Measure.stopDistances = [];

        var paths;
        if (geometry.type == "polyline") {
            paths = geometry.paths[0];
        }
        else if (geometry.type == "polygon") {
            paths = geometry.rings[0];
        }
        var point = paths[paths.length - 1];
        var showPnt = new esri.geometry.Point(point[0], point[1]);
        var measureArea = 0;
        var measureLength = 0;
        var symbol = null;
        if (geometry.type == "polyline") {
            symbol = new esri.symbol.SimpleLineSymbol(esri.symbol.SimpleLineSymbol.STYLE_SOLID, new dojo.Color([255, 0, 0]), 2);
        }
        else if (geometry.type == "polygon") {
            var pLineSymbol = new esri.symbol.SimpleLineSymbol(esri.symbol.SimpleLineSymbol.STYLE_SOLID, new dojo.Color([255, 0, 0]), 2);
            symbol = new esri.symbol.SimpleFillSymbol(esri.symbol.SimpleFillSymbol.STYLE_SOLID, pLineSymbol, new dojo.Color([255, 0, 0, 0.15]));
        }
        //添加测量轨迹
        graphicsLayer = RaoRao.Map.GetGraphicLayer(RaoRao.Map.Map2DControl, "graphicLayer");
        var graphic = new esri.Graphic(geometry, symbol);
        //graphicsLayer.add(graphic);
        RaoRao.Map.Map2DControl.graphics.add(graphic);
        if (geometry.type == "polyline") {
            //var length = measureLength[0];
            //var lengthtext = RaoRao.Portal.Measure.GetText(length, "distance");
        }
        else {
            var area = measureArea[0];
            var areatext = RaoRao.Portal.Measure.GetText(area, "area");
            //计算多边形中心
            var polygon = new esri.geometry.Polygon(paths);
            showPnt = polygon.getCentroid();
            areatext = RaoRao.Portal.Measure.Text;
            var graphic = RaoRao.Portal.Measure.GetPointGraphic(showPnt, "面积：" + areatext);
            //graphicsLayer.add(graphic);
            RaoRao.Map.Map2DControl.graphics.add(graphic);
        }
        if (callback != undefined && $.isFunction(callback)) {
            callback(geometry, RaoRao.Portal.Measure.Text)
        }
    },
    //创建textSymbol--jianglina
    CreateTextSymbol: function (text) {
        var fontColor = new dojo.Color("#ff0000");
        var holoColor = new dojo.Color("#fff");
        var font = new esri.symbol.Font("14pt", esri.symbol.Font.STYLE_ITALIC, esri.symbol.Font.VARIANT_NORMAL, esri.symbol.Font.WEIGHT_BOLD, "Courier");
        var textSymbol = new esri.symbol.TextSymbol(text, font, fontColor);
        textSymbol.setOffset(10, 10);
        textSymbol.setAlign(esri.symbol.TextSymbol.ALIGN_MIDDLE);
        return textSymbol;
    },
    //获取测量点的Graphics--jianglina
    GetPointGraphic: function (point, text) {
        var textSymbol = RaoRao.Portal.Measure.CreateTextSymbol(text);
        return new esri.Graphic(point, textSymbol);
    },
    //数值转换成距离/面积文本
    GetText: function (number, type) {
        var text = number;
        var value = parseFloat(number)
        if (type == "distance") {
            if (value > 1000) {
                text = (value / 1000).toFixed(2) + "千米";
            } else {
                text = value.toFixed() + "米";
            }
        }
        else if (type == "area") {
            if (value > 1000000) {
                text = (value / 1000000).toFixed(2) + "平方千米";
            } else {
                text = value.toFixed() + "平方米";
            }
        }
        return text;
    }
};
/**
 * @constructor 名称：ThemeMap
 * @description 作用：专题地图
 * @author 陈兴旺 
 */
RaoRao.Portal.ThemeMap = {
    HeatMapLayer: function (map,features, Radius, field, colors, max, min) {
        var maxval = 250;
        var minval = 10;
        if (max) maxval = max;
        if (min) minval = min;
        var Colors = ["rgba(0, 0, 255, 0.5)", "rgb(0, 0, 255)", "rgb(255, 0, 255)", "rgb(255, 0, 0)"];
        if (colors) Colors = colors;
        var featureLayer = null;
        require(["esri/layers/FeatureLayer","esri/renderers/HeatmapRenderer"], function () {
            var layerDefinition = {
                "geometryType": "esriGeometryPoint",
                "fields": [{
                    "name": "Val",
                    "type": "esriFieldTypeInteger",
                    "alias": "值"
                }]
            };
            var featureCollection = {
                layerDefinition: layerDefinition,
                featureSet: null
            };
            featureLayer = new esri.layers.FeatureLayer(featureCollection, {
                showLabels: true
            });
            featureLayer.graphics = features;
            var heatmapRenderer = new esri.renderer.HeatmapRenderer({
                field: field,
                //colors: Colors,
                blurRadius: Radius,
                maxPixelIntensity: maxval,
                minPixelIntensity: minval
            });
            heatmapRenderer.setColorStops([
                { ratio: 0, color: "rgba(248, 195, 213,0)" },
                { ratio: 0.2, color: "rgb(127, 235, 56)" },
                { ratio: 0.65, color: "rgb(234, 249, 40)" },
                { ratio: 1, color: "rgb(247, 3, 0)" }
            ]);
            featureLayer.setRenderer(heatmapRenderer);
            map.addLayer(featureLayer);
        });
    }
}
