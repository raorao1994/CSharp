if (!this["RaoRao"]) {
    RaoRao = {}
}
if (!this["RaoRao.Map"]) {
    RaoRao.Map = {}
}
RaoRao.Map = {
    Map2DControl:null,
    InitMap: function (a) {
        require(["esri/map", "esri/SpatialReference","esri/layers/FeatureLayer",
            "esri/geometry/Point", "esri/toolbars/navigation",
            "esri/geometry/Polygon", "esri/toolbars/draw",
            "esri/symbols/SimpleMarkerSymbol", "esri/tasks/query",
            "esri/symbols/SimpleLineSymbol", "esri/symbols/SimpleFillSymbol",
            "esri/graphic", "esri/geometry/jsonUtils", "esri/Color", "dojo/parser",
            "esri/tasks/IdentifyTask", "esri/tasks/IdentifyParameters", "dijit/form/Button",
            "dijit/layout/BorderContainer", "dijit/layout/ContentPane", "dojo/domReady!"],
        function () {
            RaoRao.Map.Map2DPopup = new esri.dijit.Popup({},
            dojo.create("div"));
            dojo.addClass(RaoRao.Map.Map2DPopup.domNode, "modernGrey");
            var d, f = RaoRao.Setting.MapSetting,
            c = {
                width: "100%",
                logo: RaoRao.Setting.MapSetting.logo,
                infoWindow: RaoRao.Map.Map2DPopup,
                zoom: 2,
                slider: RaoRao.Setting.MapSetting.slider,
                fadeOnZoom: true
            };
            if (f && f.initExtent && f.initExtent.xmin && f.initExtent.ymin && f.initExtent.xmax && f.initExtent.ymax) {
                d = new esri.geometry.Extent(parseFloat(f.initExtent.xmin), parseFloat(f.initExtent.ymin), parseFloat(f.initExtent.xmax), parseFloat(f.initExtent.ymax), new esri.SpatialReference({
                    wkid: RaoRao.Setting.GlobalSetting.wkid
                }));
                c.extent = d
            }
            var e = new esri.Map("Main_Map_DIV", c);
            RaoRao.Map.Map2DControl = e;
            if (d) {
                RaoRao.Map.Map2DControl.setExtent(d);
                RaoRao.Setting.MapSetting.initExtent = RaoRao.Map.Map2DControl.extent
            }
            RaoRao.Setting.MapSetting.zoom = RaoRao.Map.Map2DControl.getZoom();
            RaoRao.Map.MapService.AddLayer(e);
            esri.config.defaults.io.proxyUrl = RaoRao.Setting.GlobalSetting.proxyUrl;
            esri.config.defaults.io.alwaysUseProxy = false;
            esri.config.defaults.io.timeout = RaoRao.Setting.GlobalSetting.timeout;
            if (a != undefined && $.isFunction(a)) {
                a()
            }
        });
    },
    Query: function (a, f, c, e) {
        var d = new esri.tasks.QueryTask(a);
        var b = new esri.tasks.Query();
        b.returnGeometry = true;
        b.outFields = ["*"];
        b.where = f + " = '" + c + "'";
        d.execute(b,
        function (g) {
            e(g)
        },
        function (g) { })
    },
    Search: {},
    Printing: function () {
        require(["esri/dijit/Print", "esri/tasks/PrintTask", "esri/tasks/PrintParameters", "esri/tasks/PrintTemplate"], function () {
            var printTask = new esri.tasks.PrintTask(RaoRao.Setting.GlobalSetting.BaseMapServices.Printing.url);
            var template = new esri.tasks.PrintTemplate();
            template.exportOptions = {
                width: 800,
                height: 600,
                dpi: 96
            };
            template.format = "PDF";
            template.layout = "MAP_ONLY";
            template.preserveScale = false;
            var params = new esri.tasks.PrintParameters();
            params.map = RaoRao.Map.Map2DControl;
            params.template = template;
            printTask.execute(params, function (evt) {
                window.open(evt.url, "_blank");
            });
        });
    },
    Analysis: {},
    Geometry: {
        drawPoint: function (b, d, c) {
            var a = new esri.geometry.Point(parseFloat(b), parseFloat(d), new esri.SpatialReference({
                wkid: c.wkid
            }));
            return a
        },
        drawPolygon: function (a) {
            return new esri.geometry.Polygon(new esri.SpatialReference({
                wkid: a.wkid
            }))
        },
        drawPolyline: function (a) {
            return esri.geometry.Polyline(new esri.SpatialReference({
                wkid: a.wkid
            }))
        },
        drawGraphic: function (d, b, a, c) {
            return new esri.Graphic(d, b, a, c)
        }
    },
    Event: {
        bindClickEvent: function (a, b) {
            dojo.connect(a, "onClick",
            function (c) {
                var d = {
                    x: c.x || c.clientX,
                    y: c.y || c.clientY,
                    graphic: c.graphic,
                    attributes: c.graphic.attributes
                };
                if (b != undefined && $.isFunction(b)) {
                    b(d)
                }
            })
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
            RaoRao.Map.MapService.map = a;
            RaoRao.Map.MapService.TopicUrl = RaoRao.Setting.GlobalSetting.SystemService + RaoRao.Map.MapService.TopicUrl;
            if (RaoRao.Setting.LiquidGISMapServices.length > 0) {
                RaoRao.Map.MapService.LoadType = 1
            } else {
                RaoRao.Map.MapService.LoadType = 2
            }
            switch (RaoRao.Map.MapService.LoadType) {
                case 1:
                    RaoRao.Map.MapService.AddLayer(RaoRao.Setting.LiquidGISMapServices);
                    RaoRao.Map.MapService.AddViewMaps(RaoRao.Setting.LiquidGISMapServices);
                    RaoRao.Map.MapService.GetTopicData();
                    break
            }
        },
        ChangeSlider: function (c, b) {
            var a = $("div[va=" + c.toLowerCase() + "]")[0];
            $(a).slider("value", b)
        },
        AddResourceMaps: function (a) {
            var b = {
                check: {
                    enable: true
                },
                data: {
                    simpleData: {
                        enable: true
                    }
                },
                callback: {
                    onNodeCreated: RaoRao.Map.MapService.ZTreeOnNodeCreated,
                    onCheck: RaoRao.Map.MapService.ZTreeOnCheck
                }
            };
            $.fn.zTree.init($("#" + RaoRao.Map.MapService.layerControlContainerId + "_tree"), b, RaoRao.Map.MapService.GlobalTopics)
        },
        AddLayer: function (f) {
            $.each(RaoRao.Setting.ArcGISMapServices, function (i, v) {
                switch (v.type) {
                    case "ArcGISTiledMapServiceLayer":
                        var l = new esri.layers.ArcGISTiledMapServiceLayer(v.url, { id: v.id });
                        f.addLayer(l);
                        break;
                    case "ArcGISDynamicMapServiceLayer":
                    default:
                        var l = new esri.layers.ArcGISDynamicMapServiceLayer(v.url, { id: v.id });
                        f.addLayer(l);
                        break
                }
            });
        },
        GetTopicData: function () {
            var d = [];
            var c = null;
            var a = RaoRao.Setting.GlobalProperty.Query.userKey;
            a = (RaoRao.Setting.MapSetting.enableUserKey ? "&" + a.substr(1, a.length - 2) : "");
            var b = RaoRao.Setting.GlobalSetting.SystemService + this.TopicUrl + a;
            $.getScript(b);
            mapRequestCallback = function (e) {
                var f = e;
                $.each(f.TopicItems,
                function (j, h) {
                    c = new Object();
                    c.id = h.TopicCode;
                    c.pId = "0";
                    c.name = h.TopicName;
                    c.obj = h;
                    d.push(c);
                    RaoRao.Map.MapService.AddChildResource(c.id, h, d)
                });
                var g = new Array();
                $.each(d,
                function (h, i) {
                    $.each(RaoRao.Setting.LiquidGISMapServices,
                    function (l, j) {
                        if (j.ServiceTopic == i.id) {
                            $.each(RaoRao.Map.MapService.GetParentNode(i, d, new Array()),
                            function (n, m) {
                                if ($.inArray(m, g) <= -1) {
                                    g.push(m);
                                    if (j.Url == "") {
                                        RaoRao.Map.MapService.GlobalDynamicTopics.push(m)
                                    }
                                }
                            });
                            g.push(i);
                            if (j.Url == "") {
                                RaoRao.Map.MapService.GlobalDynamicTopics.push(i);
                                $.each(RaoRao.Map.MapService.GetChildNode(i, d),
                                function (n, m) {
                                    RaoRao.Map.MapService.GlobalDynamicTopics.push(m)
                                })
                            }
                            $.each(RaoRao.Map.MapService.GetChildNode(i, d),
                            function (n, m) {
                                g.push(m)
                            });
                            return false
                        }
                        if (j.Url != "") {
                            var k = j.Url.toLowerCase().replace("/tile/arcgisrest/", "");
                            k = $.trim(k.replace(".gis", ""));
                            if (k == i.id.toLowerCase()) {
                                $.each(RaoRao.Map.MapService.GetParentNode(i, d, new Array()),
                                function (n, m) {
                                    if ($.inArray(m, g) <= -1) {
                                        g.push(m)
                                    }
                                });
                                g.push(i)
                            }
                        }
                    })
                });
                RaoRao.Map.MapService.GlobalTopics = g
            }
        },
        AddChildResource: function (f, b, e) {
            var c = null;
            var a = b.TopicItems;
            var d = b.ResourceItems;
            if (a && a.length > 0) {
                $.each(a,
                function (h, g) {
                    c = new Object();
                    c.id = g.TopicCode;
                    c.pId = f;
                    c.name = g.TopicName;
                    c.obj = g;
                    e.push(c);
                    RaoRao.Map.MapService.AddChildResource(c.id, g, e)
                })
            } else {
                if (d && d.length > 0) {
                    $.each(d,
                    function (h, g) {
                        c = new Object();
                        if (g.LayerType == "Tile") {
                            c.id = f + "_" + g.ResourceTarget
                        } else {
                            c.id = f + "_" + g.LayerID
                        }
                        c.pId = f;
                        c.name = g.ResourceTitle;
                        c.obj = g;
                        e.push(c)
                    })
                }
            }
        },
        RemoveAddService: function (c) {
            var b;
            var a = RaoRao.Map.MapService.map.getLayer(c);
            if (!a.visible) {
                a.setVisibility(true);
                $.each(RaoRao.Map.MapService.GlobalMapService,
                function (e, d) {
                    if (d.id == c) {
                        b = d.TopicCode;
                        $("#btn_" + c).text("移除服务");
                        var f = [];
                        if (d.type == "dynamic") {
                            $.each(d.layer.layerInfos,
                            function (g, h) {
                                f.push(h.id)
                            });
                            a.setVisibleLayers(f)
                        }
                        RaoRao.Map.MapService.ChangeSlider(d.id, 100);
                        return false
                    }
                });
                $.each($.fn.zTree.getZTreeObj(RaoRao.Map.MapService.layerControlContainerId + "_tree").getCheckedNodes(false),
                function (e, d) {
                    if (b.toLowerCase() == d.id.toLowerCase()) {
                        $.fn.zTree.getZTreeObj(RaoRao.Map.MapService.layerControlContainerId + "_tree").checkNode(d, true, true)
                    }
                })
            } else {
                $("#btn_" + c).text("添加服务");
                RaoRao.Map.MapService.ChangeSlider(c, 0);
                a.setVisibility(false);
                $.each(RaoRao.Map.MapService.GlobalMapService,
                function (e, d) {
                    if (d.id == c) {
                        b = d.TopicCode;
                        return false
                    }
                });
                $.each($.fn.zTree.getZTreeObj(RaoRao.Map.MapService.layerControlContainerId + "_tree").getCheckedNodes(true),
                function (e, d) {
                    if (b.toLowerCase() == d.id.toLowerCase()) {
                        $.fn.zTree.getZTreeObj(RaoRao.Map.MapService.layerControlContainerId + "_tree").checkNode(d, false, true)
                    }
                })
            }
        },
        ChangeOpacity: function (a, c, d) {
            var b = this.map.getLayer(a);
            if (!b.visible) {
                b.setVisibility(true)
            }
            b.setOpacity(c / 100)
        },
        ChangeLayerVisible: function (c, a) {
            var e = RaoRao.Map.MapService.map.layerIds;
            var d;
            var b = false;
            $.each(e,
            function (g, f) {
                if (f == c) {
                    b = true
                }
            });
            if (b) {
                d = RaoRao.Map.MapService.map.getLayer(c);
                d.setVisibility(a)
            }
            if (a) {
                RaoRao.Map.MapService.ChangeSlider(c, 100)
            } else {
                RaoRao.Map.MapService.ChangeSlider(c, 0)
            }
        },
        GetChildNode: function (a, c) {
            var b = new Array();
            $.each(c,
            function (e, d) {
                if (d.pId == a.id) {
                    b.push(d)
                }
            });
            return b
        },
        GetParentNode: function (b, c, a) {
            $.each(c,
            function (e, d) {
                if (d.id == b.pId) {
                    a.push(d);
                    RaoRao.Map.MapService.GetParentNode(d, c, a)
                }
            });
            return a
        },
        ZTreeOnNodeCreated: function (a, c, b) {
            $.each(RaoRao.Setting.LiquidGISMapServices,
            function (f, d) {
                if (d.Visible == "true") {
                    if (d.ServiceTopic != "") {
                        if (d.ServiceTopic == b.id) {
                            $.fn.zTree.getZTreeObj(RaoRao.Map.MapService.layerControlContainerId + "_tree").checkNode(b, true, true)
                        }
                    } else {
                        var e = d.Url.toLowerCase().replace("/tile/arcgisrest/", "");
                        e = $.trim(e.replace(".gis", ""));
                        if (e == b.id.toLowerCase()) {
                            $.fn.zTree.getZTreeObj(RaoRao.Map.MapService.layerControlContainerId + "_tree").checkNode(b, true, true)
                        }
                    }
                }
            });
            $.fn.zTree.getZTreeObj(RaoRao.Map.MapService.layerControlContainerId + "_tree").expandNode(b);
            return b
        },
        ExistHasChildNode: function (b, a) {
            if (b.children == undefined) {
                a.push(b.id.toLowerCase())
            } else {
                $.each(b.children,
                function (d, c) {
                    RaoRao.Map.MapService.ExistHasChildNode(c, a)
                })
            }
            return a
        },
        ZTreeOnCheck: function (a, e, i) {
            var h = false;
            var g = 0;
            var c = "",
            d, f;
            var b = new Array();
            if (i.checked) {
                h = true;
                g = 100
            }
            if (i.id.indexOf("_") > -1) {
                c = i.id.substring(0, i.id.indexOf("_"));
                d = i.id.substring(i.id.indexOf("_") + 1);
                $.each(RaoRao.Map.MapService.GlobalMapService,
                function (l, k) {
                    if (k.TopicCode == c) {
                        var j = i.getNextNode();
                        while (j != null) {
                            if (j.checked) {
                                b.push(j.id.substring(j.id.indexOf("_") + 1))
                            }
                            j = j.getNextNode()
                        }
                        var m = i.getPreNode();
                        while (m != null) {
                            if (m.checked) {
                                b.push(m.id.substring(m.id.indexOf("_") + 1))
                            }
                            m = m.getPreNode()
                        }
                        f = RaoRao.Map.MapService.map.getLayer(k.id);
                        if (h) {
                            b.push(d);
                            f.setVisibility(true)
                        }
                        f.setVisibleLayers(b);
                        RaoRao.Map.MapService.ChangeLayerVisible(k.id, h)
                    }
                    if (k.TopicCode == i.id) {
                        RaoRao.Map.MapService.ChangeLayerVisible(k.id, h)
                    }
                })
            } else {
                $.each(RaoRao.Map.MapService.GlobalMapService,
                function (k, j) {
                    if (j.TopicCode.indexOf(i.id) > -1) {
                        RaoRao.Map.MapService.ChangeLayerVisible(j.id, h)
                    }
                })
            }
        }
    },
    addFeatureLayer: function (url, id, infotemplate) {
        var feature = new esri.layers.FeatureLayer(url, {id:id, infoTemplate: infotemplate, outFields: ["*"] });
        RaoRao.Map.Map2DControl.addLayer(feature);
        return feature;
    },
    addLayer: function (b) {
        var a = RaoRao.Map.Map2DControl.getLayer(b.id);
        if (a != null) {
            a.clear();
            RaoRao.Map.Map2DControl.removeLayer(a)
        }
        a = new esri.layers.GraphicsLayer({
            id: b.id
        });
        RaoRao.Map.Map2DControl.addLayer(a);
        return a
    },
    GetPoint: function (a, b) {
        return new esri.geometry.Point(a, b, RaoRao.Map.Map2DControl.spatialReference)
    }, 
    GetPolygon: function (rings) {
        var polylineJson = {
            "rings": rings,
            "spatialReference": RaoRao.Map.Map2DControl.spatialReference
        };
        return new esri.geometry.Polygon(polylineJson);
        //return new esri.geometry.Polygon(RaoRao.Map.Map2DControl.spatialReference)
    },
    GetPolyline: function (paths) {
        var polylineJson = {
            //"paths": [[[-122.68, 45.53], [-122.58, 45.55],
            //[-122.57, 45.58], [-122.53, 45.6]]],
            "paths":paths,
            "spatialReference": RaoRao.Map.Map2DControl.spatialReference
        };
        return new esri.geometry.Polyline(polylineJson);
        //return new esri.geometry.Polyline(RaoRao.Map.Map2DControl.spatialReference);
    },
    GetGraphic: function (c, b, a) {
        var d = new esri.Graphic(c, b, a);
        return d
    },
    GetGraphicLayer: function (b, c) {
        var a = b.getLayer(c);
        if (a == null) {
            a = new esri.layers.GraphicsLayer({
                id: c
            });
            b.addLayer(a)
        }
        return a
    },
    GetLayer: function (d, a, c) {
        var b;
        switch (c) {
            case "ArcGISDynamicMapServiceLayer":
                b = new esri.ArcGISDynamicMapServiceLayer(a, d);
                break;
            case "ArcGISImageServiceLayer":
                b = new esri.ArcGISImageServiceLayer(a, d);
                break;
            case "ArcGISTiledMapServiceLayer":
                b = new esri.ArcGISTiledMapServiceLayer(a, d);
                break;
            default:
                break
        }
        return b
    },
    ShowGraphic: function (f, b, j, h, g, e, d) {
        var i = {
            font: {
                size: "14",
                style: "normal",
                family: "微软雅黑"
            },
            color: [0, 0, 0],
            pic: {
                src: "",
                width: 40,
                height: 40
            },
            offset: {
                x: 0,
                y: -36
            }
        };
        if (d) {
            i = $.extend(true, i, d)
        }
        var c = new esri.symbol.TextSymbol(b, i.font, i.color).setOffset(i.offset.x, i.offset.y);
        var k = new esri.geometry.Point([parseFloat(j), parseFloat(h)], new esri.SpatialReference({
            wkid: RaoRao.Setting.GlobalSetting.wkid
        }));
        var l = new esri.symbol.PictureMarkerSymbol(g, i.pic.width, i.pic.height);
        var a = new esri.Graphic(k, l, e, null);
        f.add(a);
        a = new esri.Graphic(k, c, e, null);
        f.add(a)
    },
    ClearLayer: function (b, c) {
        if (b != null) {
            var a = b.getLayer(c);
            if (a != null) {
                a.clear();
                b.removeLayer(a)
            }
        }
    },
    GetGraphicByGeometry: function (a) {
        var b = null;
        switch (a.type) {
            case "point":
            case "multipoint":
                b = new esri.Graphic(a, RaoRao.Map.Symbol.markSymbol());
                break;
            case "polyline":
                b = new esri.Graphic(a, RaoRao.Map.Symbol.lineSymbol());
                break;
            case "polygon":
            case "extent":
                b = new esri.Graphic(a, RaoRao.Map.Symbol.fillSymbol());
                break;
            default:
                break
        }
        return b
    },
    Fly2Geometry: function (f, g, d) {
        if (f != null && f.extent != null && g != null) {
            var c = g.getExtent();
            if (g.type == "point") {
                c = new esri.geometry.Extent(g.x - 1e-7, g.y - 1e-7, g.x + 1e-7, g.y + 1e-7, f.spatialReference);
                c = c.expand(1.5)
            }
            if (c != null) {
                var b = new esri.geometry.Point(c.xmin + (c.xmax - c.xmin) / 2, c.ymin + (c.ymax - c.ymin) / 2, f.spatialReference);
                var e = new esri.geometry.Extent(b.x, b.y, b.x, b.y, b.spatialReference);
                if (RaoRao.Map.Extent1ContainExtent2(f.extent, c)) {
                    f.setExtent(c);
                    if (d != null && $.isFunction(d)) {
                        d()
                    }
                } else {
                    var a = RaoRao.Map.Union2Extent(e, f.extent);
                    f.setExtent(a, true);
                    setTimeout(function () {
                        f.centerAt(b)
                    },
                    700);
                    setTimeout(function () {
                        f.setExtent(c);
                        if (d != null && $.isFunction(d)) {
                            d()
                        }
                    },
                    1400)
                }
            }
        }
    },
    Extent1ContainExtent2: function (c, b) {
        var a = false;
        if (c.xmin < b.xmin && c.ymin < b.ymin && c.xmax > b.xmax && c.ymax > b.ymax) {
            a = true
        }
        return a
    },
    Union2Extent: function (c, a) {
        var b = new esri.geometry.Extent(c.xmin, c.ymin, c.xmax, c.ymax, c.spatialReference);
        if (c != null && a != null) {
            b.xmax = (c.xmax > a.xmax ? c.xmax : a.xmax);
            b.xmin = (c.xmin < a.xmin ? c.xmin : a.xmin);
            b.ymax = (c.ymax > a.ymax ? c.ymax : a.ymax);
            b.ymin = (c.ymin < a.ymin ? c.ymin : a.ymin)
        }
        return b
    },
    GetFullExtentFromPoints: function (h, f) {
        var b = 0,
        a = 0,
        g = 0,
        d = 0;
        if (f && f.length > 0) {
            b = f[0].x;
            g = f[0].x;
            a = f[0].y;
            d = f[0].y;
            for (var c = 0; c < f.length; c++) {
                b = b > f[c].x ? b : f[c].x;
                a = a > f[c].y ? a : f[c].y;
                g = g > f[c].x ? f[c].x : g;
                d = d > f[c].y ? f[c].y : d
            }
        }
        var e = new esri.geometry.Extent(parseFloat(g - 1000), parseFloat(d - 1000), parseFloat(b + 1000), parseFloat(a + 1000), h.spatialReference);
        return e
    },
    Symbol: {
        markSymbol: function () {
            var a = new esri.symbol.SimpleMarkerSymbol();
            a.color = new dojo.Color("red");
            a.size = 12;
            return a
        },
        fillSymbol: function () {
            var a = new esri.symbol.SimpleFillSymbol();
            a.color = new dojo.Color("#6600FF00");
            a.outline = new esri.symbol.SimpleLineSymbol(esri.symbol.SimpleLineSymbol.STYLE_SOLID, new dojo.Color("red"), 2);
            return a
        },
        lineSymbol: function () {
            var a = new esri.symbol.SimpleLineSymbol();
            a.color = new dojo.Color("red");
            a.width = 4;
            return a
        },
        txtSymbol: function (b, a, e, c) {
            var d = new esri.symbol.TextSymbol();
            d.color = c;
            d.text = b;
            d.xoffset = a;
            d.yoffset = e;
            return d
        },
        customMarkSymbol: function (c, b) {
            var a = new esri.symbol.SimpleMarkerSymbol();
            a.color = b;
            a.size = c;
            return a
        },
        customFillSymbol: function (a) {
            var b = new esri.symbol.SimpleFillSymbol();
            b.color = a;
            b.outline = new esri.symbol.SimpleLineSymbol(esri.symbol.SimpleLineSymbol.STYLE_SOLID, new dojo.Color("red"), 1);
            return b
        },
        customPicFillSymbol: function (b, d, a) {
            var c = new esri.symbol.SimpleLineSymbol(esri.symbol.SimpleLineSymbol.STYLE_SOLID, new dojo.Color("red"), 1);
            var e = esri.symbol.PictureFillSymbol(b, c, d, a);
            return e
        },
        customLineSymbol: function (a, c) {
            var b = new esri.symbol.SimpleLineSymbol();
            b.color = a;
            b.width = c;
            return b
        },
        pictureMarkerSymbol: function (c, d, b, a, f) {
            var e = new esri.symbol.PictureMarkerSymbol(c, d, b);
            e.xoffset = a;
            e.yoffset = f;
            return e
        },
        fillSymbol1: function () {
            var a = new esri.symbol.SimpleFillSymbol();
            a.color = new dojo.Color([230, 35, 143,0.5]);
            a.outline = new esri.symbol.SimpleLineSymbol(esri.symbol.SimpleLineSymbol.STYLE_SOLID, new dojo.Color([255,255,255,0]), 2);
            return a
        }
    },
    Formatter: {
        //经纬度转墨卡托
        lonlat2mercator:function (lonlat){
            //var mercator={x:0,y:0};
            var x = lonlat.x *20037508.34/180;
            var y = Math.log(Math.tan((90+lonlat.y)*Math.PI/360))/(Math.PI/180);
            y = y *20037508.34/180;
            //mercator.x = x;
            //mercator.y = y;
            var p=new esri.geometry.Point(x, y, RaoRao.Map.Map2DControl.spatialReference)
            return p;
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
    Addfeature: function () {
        require(["esri/layers/FeatureLayer", "esri/InfoTemplate"], function () {
            //定义infotemplate
            var content = '公安局名称：${NAME} <br/> 管辖区域：${NAME99} <br/> 邮编：${ADCODE99} <br/> 公交车：${bus} <br/> 电话号码：${number}';
            var template = new esri.InfoTemplate("${NAME}", content);
            RaoRao.Map.Map2DControl.infoWindow.resize(300, 300);//设置弹出框大小
            var feature = new esri.layers.FeatureLayer(RaoRao.Setting.GlobalSetting.queryurl, { infoTemplate: template, outFields: ["*"] });
            RaoRao.Map.Map2DControl.addLayer(feature);
        });
    }
};