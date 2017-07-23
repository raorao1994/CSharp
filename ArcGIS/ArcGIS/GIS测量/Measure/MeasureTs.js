function MeasureTools(Map) {
    this.map = Map;
    this.stopPoints = [];
    this.stopDistances = [];
    this._measureLayer = new esri.layers.GraphicsLayer({ id: "graphicLayer" });
    this._textLayer = new esri.layers.GraphicsLayer({ id: "textLayer" });
    this.map.addLayer(this.textLayer);
    this.map.addLayer(this._measureLayer);
    this.DrawToolbar = new esri.toolbars.Draw(this.map);
    this.ClickHandler = null;
    this.IsMeasure = false;
    
}

MeasureTools.prototype = {
    //�������
    Distance: function (callback) {
        RaoRao.Map.ClearLayer(this.map, "graphicLayer");
        this._textLayer.clear();
        this._measureLayer.clear();
        graphicLayer = this._measureLayer;
        var textLayer = this._textLayer;
        this.DrawToolbar.activate(esri.toolbars.Draw.POLYLINE);
        //������¼�
        this.ClickHandler = dojo.connect(this.map, "onClick", function (evt) {
            this.IsMeasure = true;
            var distance = 0;
            var stopPoint = evt.mapPoint;
            var startPoint = evt.mapPoint;
            var showpoint1 = stopPoint;
            var showpoint2 = startPoint;
            if (this.stopPoints.length > 0) {
                startPoint = this.stopPoints[this.stopPoints.length - 1];
                require(["esri/geometry/geometryEngine"], function (geometryEngine) {
                    var line = new esri.geometry.Polyline(this.map.spatialReference);
                    line.addPath([startPoint, stopPoint]);
                    if (this.map.spatialReference.isWebMercator() || this.map.spatialReference.wkid == "4326") {//��web����ͶӰ��WGS84����ϵ�µļ��㷽��
                        distance += geometryEngine.geodesicLength(line, "meters");
                    } else {//������ͶӰ����ϵ�µļ��㷽��
                        distance += geometryEngine.planarLength(line, "meters")
                    }


                    //distance = geometryEngine.distance(startPoint, stopPoint, RaoRao.Setting.GlobalSetting.wkid);
                    if (this.stopDistances.length > 0) {
                        distance += this.stopDistances[this.stopDistances.length - 1];
                    }
                    this.stopDistances.push(distance);
                });
            }
            var text = this.GetText(distance, "distance");
            this.Text = text;
            var textGraphic = this.GetPointGraphic(showpoint1, text);
            this.map.graphics.add(textGraphic);
            this.stopPoints.push(stopPoint);
            PointSymbol = new esri.symbol.SimpleMarkerSymbol();
            PointSymbol.style = esri.symbol.SimpleMarkerSymbol.STYLE_CIRCLE;
            PointSymbol.setSize(2);
            PointSymbol.setColor(new dojo.Color([255, 0, 0]));
            var stopGraphic = new esri.Graphic(showpoint1, PointSymbol);
            this.map.graphics.add(stopGraphic);
            var polyline = new esri.geometry.Polyline();
            polyline.addPath([showpoint2, showpoint1]);
            symbol = new esri.symbol.SimpleLineSymbol(esri.symbol.SimpleLineSymbol.STYLE_SOLID, new dojo.Color([255, 0, 0]), 2);
            var graphic = new esri.Graphic(polyline, symbol);
            this.map.graphics.add(graphic);
            textLayer.clear();
            textLayer.show();
        });
        //����ƶ��¼�
        this.MouseMoveHandler = dojo.connect(this.map, "onMouseMove", function (evt) {
            if (!this.IsMeasure)
                return;
            var distancetext = 0;
            var stopPoint = evt.mapPoint;
            if (this.stopPoints.length > 0) {
                var startPoint = this.stopPoints[this.stopPoints.length - 1];
                require(["esri/geometry/geometryEngine"], function (geometryEngine) {

                    var line = new esri.geometry.Polyline(this.map.spatialReference);
                    line.addPath([startPoint, stopPoint]);
                    if (this.map.spatialReference.isWebMercator() || this.map.spatialReference.wkid == "4326") {//��web����ͶӰ��WGS84����ϵ�µļ��㷽��
                        distance += geometryEngine.geodesicLength(line, "meters");
                    } else {//������ͶӰ����ϵ�µļ��㷽��
                        distance += geometryEngine.planarLength(line, "meters")
                    }

                    //distancetext = geometryEngine.distance(startPoint, stopPoint, RaoRao.Setting.GlobalSetting.wkid);
                    if (this.stopDistances.length > 0) {
                        distancetext += this.stopDistances[this.stopDistances.length - 1];
                    }
                    var text = this.GetText(distancetext, "distance");
                    textGraphictext = this.GetPointGraphic(stopPoint, text);
                    textLayer.clear();
                    textLayer.add(textGraphictext);
                });
            }
        });
        //��������¼�
        this.DrawHandler = dojo.connect(this.DrawToolbar, "onDrawEnd", function (geometry) {
            this.GetLengthorArea(geometry, callback);
        });
    },
    //�������
    Area: function (callback) {
        this.DrawToolbar.activate(esri.toolbars.Draw.POLYGON);
        var textLayer = this._textLayer; //��̬��ʾ����ͼ��
        this.stopPoints = [];
        //������¼�
        this.ClickHandler = dojo.connect(this.map, "onClick", function (evt) {
            this.IsMeasure = true;
            this.stopPoints.push([point.x, point.y]);
            textLayer.clear();
        });
        //����ƶ��¼�
        this.MouseMoveHandler = dojo.connect(this.map, "onMouseMove", function (evtTEXT) {
            if (!this.IsMeasure)
                return;
            var measureArea = 0;
            var paths = [];
            paths.push([point.x, point.y]);
            var paths1 = paths.concat(this.stopPoints);
            var polygon = new esri.geometry.Polygon(paths1);
            //var showPnt = polygon.getCentroid();
            //var geo = esri.geometry.webMercatorToGeographic(polygon);

            var spatialReference = this.map.spatialReference;
            if (spatialReference.isWebMercator() || spatialReference.wkid == "4326") {
                measureArea= geometryEngine.geodesicArea(polygon, "square-meters")
            } else {
                measureArea= geometryEngine.planarArea(polygon, "square-meters")
            }

            //measureArea = esri.geometry.geodesicAreas([geo], esri.Units.SQUARE_METERS);
            //measureArea = Math.abs(parseFloat(measureArea[0]));

            var text = this.GetText(measureArea, "area");
            this.Text = text;
            var textGraphic = this.GetPointGraphic(evtTEXT.mapPoint, text);
            textLayer.clear();
            textLayer.add(textGraphic);
        });
        //��������¼�
        this.DrawHandler = dojo.connect(this.DrawToolbar, "onDrawEnd", function (geometry) {
            this.GetLengthorArea(geometry, callback);
        });
    },
    //�����������
    GetLengthorArea: function (geometry, callback) {
        this.IsMeasure = false;
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
        //��Ӳ����켣
        graphicsLayer = this._measureLayer;
        var graphic = new esri.Graphic(geometry, symbol);
        this.map.graphics.add(graphic);
        if (geometry.type == "polyline") {
            //var length = measureLength[0];
            //var lengthtext = this.GetText(length, "distance");
        }
        else {
            var area = measureArea[0];
            var areatext = this.GetText(area, "area");
            //������������
            var polygon = new esri.geometry.Polygon(paths);
            showPnt = polygon.getCentroid();
            areatext = this.Text;
            var graphic = this.GetPointGraphic(showPnt, "�����" + areatext);
            this.map.graphics.add(graphic);
        }
        if (callback != undefined && $.isFunction(callback)) {
            callback(geometry, this.Text)
        }
    },
    //����textSymbol--jianglina
    CreateTextSymbol: function (text) {
        var fontColor = new dojo.Color("#ff0000");
        var holoColor = new dojo.Color("#fff");
        var font = new esri.symbol.Font("14pt", esri.symbol.Font.STYLE_ITALIC, esri.symbol.Font.VARIANT_NORMAL, esri.symbol.Font.WEIGHT_BOLD, "Courier");
        var textSymbol = new esri.symbol.TextSymbol(text, font, fontColor);
        textSymbol.setOffset(10, 10);
        textSymbol.setAlign(esri.symbol.TextSymbol.ALIGN_MIDDLE);
        return textSymbol;
    },
    //��ȡ�������Graphics--jianglina
    GetPointGraphic: function (point, text) {
        var textSymbol = this.CreateTextSymbol(text);
        return new esri.Graphic(point, textSymbol);
    },
    //��ֵת���ɾ���/����ı�
    GetText: function (number, type) {
        var text = number;
        var value = parseFloat(number)
        if (type == "distance") {
            if (value > 1000) {
                text = (value / 1000).toFixed(2) + "ǧ��";
            } else {
                text = value.toFixed() + "��";
            }
        }
        else if (type == "area") {
            if (value > 1000000) {
                text = (value / 1000000).toFixed(2) + "ƽ��ǧ��";
            } else {
                text = value.toFixed() + "ƽ����";
            }
        }
        return text;
    }
}