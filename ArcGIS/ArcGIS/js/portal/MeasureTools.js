/*
*@作者：陈兴旺
*@说明：arcgis仿百度地图测距测面积
*/
function MeasureTools(Map) {
    var obj = {};
    obj.map = Map;
    obj.stopPoints = [];
    obj.stopDistances = [];
    obj.Text = "";
    obj._measureLayer = new esri.layers.GraphicsLayer({ id: "graphicLayer" });
    obj._textLayer = new esri.layers.GraphicsLayer({ id: "textLayer" });
    if (obj.map.getLayer("textLayer"))
    {
        obj._textLayer = obj.map.getLayer("textLayer");
    }
    if (obj.map.getLayer("graphicLayer")) {
        obj._measureLayer = obj.map.getLayer("graphicLayer");
    }
    obj.map.addLayer(obj._textLayer);
    obj.map.addLayer(obj._measureLayer);
    obj.DrawToolbar = new esri.toolbars.Draw(obj.map);
    obj.ClickHandler = null;
    obj.MouseMoveHandler = null;
    obj.DrawHandler = null;
    obj.IsMeasure = false;
    //测距
    obj.Distance = function (callback) {
        this._textLayer.clear();
        this._measureLayer.clear();
        this.DrawToolbar.activate(esri.toolbars.Draw.POLYLINE);
        //鼠标点击事件
        this.ClickHandler = dojo.connect(this.map, "onClick", function (evt) {
            obj.IsMeasure = true;
            var distance = 0;
            var stopPoint = evt.mapPoint;
            var startPoint = evt.mapPoint;
            var showpoint1 = stopPoint;
            var showpoint2 = startPoint;
            if (obj.stopPoints.length > 0) {
                startPoint = obj.stopPoints[obj.stopPoints.length - 1];
                require(["esri/geometry/geometryEngine"], function (geometryEngine) {
                    var line = new esri.geometry.Polyline(this.map.spatialReference);
                    line.addPath([startPoint, stopPoint]);
                    if (obj.map.spatialReference.isWebMercator() || obj.map.spatialReference.wkid == "4326") {//在web麦卡托投影和WGS84坐标系下的计算方法
                        distance += geometryEngine.geodesicLength(line, "meters");
                    } else {//在其他投影坐标系下的计算方法
                        distance += geometryEngine.planarLength(line, "meters")
                    }
                    if (obj.stopDistances.length > 0) {
                        distance += obj.stopDistances[obj.stopDistances.length - 1];
                    }
                    obj.stopDistances.push(distance);
                });
            }
            var text = obj.GetText(distance, "distance");
            this.Text = text;
            var textGraphic = obj.GetPointGraphic(showpoint1, text);
            obj._measureLayer.add(textGraphic);
            obj.stopPoints.push(stopPoint);
            var PointSymbol = new esri.symbol.SimpleMarkerSymbol();
            PointSymbol.style = esri.symbol.SimpleMarkerSymbol.STYLE_CIRCLE;
            PointSymbol.setSize(2);
            PointSymbol.setColor(new dojo.Color([255, 0, 0]));
            var stopGraphic = new esri.Graphic(showpoint1, PointSymbol);
            obj._measureLayer.add(stopGraphic);
            var polyline = new esri.geometry.Polyline();
            polyline.addPath([showpoint2, showpoint1]);
            symbol = new esri.symbol.SimpleLineSymbol(esri.symbol.SimpleLineSymbol.STYLE_SOLID, new dojo.Color([255, 0, 0]), 2);
            var graphic = new esri.Graphic(polyline, symbol);
            obj._measureLayer.add(graphic);
            obj._textLayer.clear();
            obj._textLayer.show();
        });
        //鼠标移动事件
        this.MouseMoveHandler = dojo.connect(this.map, "onMouseMove", function (evt) {
            if (!obj.IsMeasure)
                return;
            var distance = 0;
            var stopPoint = evt.mapPoint;
            if (obj.stopPoints.length > 0) {
                var startPoint = obj.stopPoints[obj.stopPoints.length - 1];
                require(["esri/geometry/geometryEngine"], function (geometryEngine) {
                    var line = new esri.geometry.Polyline(obj.map.spatialReference);
                    line.addPath([startPoint, stopPoint]);
                    if (obj.map.spatialReference.isWebMercator() || obj.map.spatialReference.wkid == "4326") {//在web麦卡托投影和WGS84坐标系下的计算方法
                        distance += geometryEngine.geodesicLength(line, "meters");
                    } else {//在其他投影坐标系下的计算方法
                        distance += geometryEngine.planarLength(line, "meters")
                    }
                    distancetext = distance;
                    if (obj.stopDistances.length > 0) {
                        distancetext += obj.stopDistances[obj.stopDistances.length - 1];
                    }
                    var text = obj.GetText(distancetext, "distance");
                    var textGraphictext = obj.GetPointGraphic(stopPoint, text);
                    obj._textLayer.clear();
                    obj._textLayer.add(textGraphictext);
                    obj._textLayer.show();
                });
            }
        });
        //测量完成事件
        this.DrawHandler = dojo.connect(this.DrawToolbar, "onDrawEnd", function (geometry) {
            obj.GetLengthorArea(geometry, callback);
        });
    };
    //面积测量
    obj.Area = function (callback) {
        this._textLayer.clear();
        this._measureLayer.clear();
        this.DrawToolbar.activate(esri.toolbars.Draw.POLYGON);
        var textLayer = this._textLayer; //动态显示距离图层
        this.stopPoints = [];
        //鼠标点击事件
        this.ClickHandler = dojo.connect(this.map, "onClick", function (evt) {
            obj.IsMeasure = true;
            obj.stopPoints.push([evt.mapPoint.x, evt.mapPoint.y]);
            textLayer.clear();
        });
        //鼠标移动事件
        this.MouseMoveHandler = dojo.connect(this.map, "onMouseMove", function (evt) {
            if (!obj.IsMeasure)
                return;
            var measureArea = 0;
            var paths = [];
            paths.push([evt.mapPoint.x, evt.mapPoint.y]);
            var paths1 = paths.concat(obj.stopPoints);
            var polygon = new esri.geometry.Polygon(paths1);
            //var showPnt = polygon.getCentroid();
            //var geo = esri.geometry.webMercatorToGeographic(polygon);
            require(["esri/geometry/geometryEngine"], function (geometryEngine) {
                var spatialReference = obj.map.spatialReference;
                if (spatialReference.isWebMercator() || spatialReference.wkid == "4326") {
                    measureArea = geometryEngine.geodesicArea(polygon, "square-meters")
                } else {
                    measureArea = geometryEngine.planarArea(polygon, "square-meters")
                }

                //measureArea = esri.geometry.geodesicAreas([geo], esri.Units.SQUARE_METERS);
                measureArea = Math.abs(parseFloat(measureArea));
                var text = obj.GetText(measureArea, "area");
                obj.Text = text;
                var textGraphic = obj.GetPointGraphic(evt.mapPoint, text);
                obj._textLayer.clear();
                obj._textLayer.add(textGraphic);
            });
        });
        //测量完成事件
        this.DrawHandler = dojo.connect(this.DrawToolbar, "onDrawEnd", function (geometry) {
            obj.GetLengthorArea(geometry, callback);
        });
    };
    //计算距离或面积
    obj.GetLengthorArea = function (geometry, callback) {
        this.IsMeasure = false;
        var lastpoint = this.stopPoints[this.stopPoints.length - 1];
        this.stopPoints = [];
        this.stopDistances = [];

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
        var graphicsLayer = this._measureLayer;
        var graphic = new esri.Graphic(geometry, symbol);
        this._measureLayer.add(graphic);
        if (geometry.type == "polyline") {
            //var length = measureLength[0];
            //var lengthtext = this.GetText(length, "distance");
        }
        else {
            
            //var area = measureArea[0];
            //var areatext = this.GetText(area, "area");
            //计算多边形中心
            //var polygon = new esri.geometry.Polygon(paths);
            var sPnt = geometry.getCentroid();
            var g = obj.GetPointGraphic(sPnt, "面积：" + this.Text);
            this._measureLayer.add(g);
            lastpoint = sPnt;
        }
        var _graphic = obj._createClearBtn(lastpoint);
        this._measureLayer.add(_graphic);

        this.DrawToolbar.deactivate();
        dojo.disconnect(this.ClickHandler);
        dojo.disconnect(this.MouseMoveHandler);
        dojo.disconnect(this.DrawHandler);
        this.ClickHandler = null;
        this.MouseMoveHandler = null;
        this.DrawHandler = null;
        this.IsMeasure = false;
        this._clearMapMouseClickEvent();
        if (callback != undefined && $.isFunction(callback)) {
            callback(geometry, this.Text);
        }
    };
    //创建textSymbol--jianglina
    obj.CreateTextSymbol = function (text) {
        var fontColor = new dojo.Color("#ff0000");
        var holoColor = new dojo.Color("#fff");
        var font = new esri.symbol.Font("14pt", esri.symbol.Font.STYLE_ITALIC, esri.symbol.Font.VARIANT_NORMAL, esri.symbol.Font.WEIGHT_BOLD, "Courier");
        var textSymbol = new esri.symbol.TextSymbol(text, font, fontColor);
        textSymbol.setOffset(10, 10);
        textSymbol.setAlign(esri.symbol.TextSymbol.ALIGN_MIDDLE);
        return textSymbol;
    };
    //获取测量点的Graphics--jianglina
    obj.GetPointGraphic = function (point, text) {
        var textSymbol = this.CreateTextSymbol(text);
        return new esri.Graphic(point, textSymbol);
    };
    //数值转换成距离/面积文本
    obj.GetText = function (number, type) {
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
    };
    //添加删除按钮
    obj._createClearBtn = function (point) {
        var iconPath = "M13.618,2.397 C10.513,-0.708 5.482,-0.713 2.383,2.386 C-0.718,5.488 -0.715,10.517 2.392,13.622 C5.497,16.727 10.529,16.731 13.627,13.632 C16.727,10.533 16.724,5.502 13.618,2.397 L13.618,2.397 Z M9.615,11.351 L7.927,9.663 L6.239,11.351 C5.55,12.04 5.032,12.64 4.21,11.819 C3.39,10.998 3.987,10.48 4.679,9.79 L6.367,8.103 L4.679,6.415 C3.989,5.726 3.39,5.208 4.21,4.386 C5.032,3.566 5.55,4.165 6.239,4.855 L7.927,6.541 L9.615,4.855 C10.305,4.166 10.82,3.565 11.642,4.386 C12.464,5.208 11.865,5.726 11.175,6.415 L9.487,8.102 L11.175,9.789 C11.864,10.48 12.464,10.998 11.642,11.819 C10.822,12.64 10.305,12.04 9.615,11.351 L9.615,11.351 Z"
        var iconColor = "#b81b1b";
        var clearSymbol = new esri.symbol.SimpleMarkerSymbol();
        clearSymbol.setOffset(-40, 15);
        clearSymbol.setPath(iconPath);
        clearSymbol.setColor(new dojo.Color(iconColor));
        clearSymbol.setOutline(null);
        clearSymbol.isClearBtn = true;
        clearSymbol.setOffset(-20, 0);//改变清空图标位置
        return esri.Graphic(point, clearSymbol, {id:"clear"});
    };
    //清空事件
    obj._clearMapMouseClickEvent= function () {
        dojo.connect(this._measureLayer, "onClick", function (evt) {
            if (evt.graphic.attributes.id && evt.graphic.attributes.id == "clear")
            {
                obj.clear();
                obj = null;
            }
        });
    };
    obj.clear = function () {
        this._measureLayer.clear();
        this.map.removeLayer(this._measureLayer);
        this.map.removeLayer(this._textLayer);
    };
    return obj;
}