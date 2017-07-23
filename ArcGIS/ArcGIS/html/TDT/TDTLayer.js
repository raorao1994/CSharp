function TDTLayer() {
    dojo.declare("TDTTiledMapServiceLayer", esri.layers.TiledMapServiceLayer, {
        layertype: "road",//图层类型
        constructor: function (args) {
            var offsetx = 0.0059; //offsetx = 0;
            var offsety = 0.0012; //offsety = 0;
            this.spatialReference = new esri.SpatialReference({ wkid: 3857 });//4326
            dojo.declare.safeMixin(this, args);
            this.initialExtent = (
                this.fullExtent = new esri.geometry.Extent(-180.0, -90.0, 180.0, 90.0, this.spatialReference));
            this.tileInfo = new esri.layers.TileInfo({
                "rows": 256,
                "cols": 256,
                "compressionQuality": 0,
                "origin": {
                    "x": -180 + offsetx ,//- 179.9944,
                    "y": 90+offsety//90.001
                },
                "spatialReference": {
                    "wkid": 3857//3857
                },
                "lods": [
                  { "level": 0, "resolution": 0.3515625*4, "scale": 147748796.52937502*4 },
                  { "level": 1, "resolution": 0.3515625 * 2, "scale": 147748796.52937502*2 },
                  { "level": 2, "resolution": 0.3515625, "scale": 147748796.52937502 },
                  { "level": 3, "resolution": 0.17578125, "scale": 73874398.264687508 },
                  { "level": 4, "resolution": 0.087890625, "scale": 36937199.132343754 },
                  { "level": 5, "resolution": 0.0439453125, "scale": 18468599.566171877 },
                  { "level": 6, "resolution": 0.02197265625, "scale": 9234299.7830859385 },
                  { "level": 7, "resolution": 0.010986328125, "scale": 4617149.8915429693 },
                  { "level": 8, "resolution": 0.0054931640625, "scale": 2308574.9457714846 },
                  { "level": 9, "resolution": 0.00274658203125, "scale": 1154287.4728857423 },
                  { "level": 10, "resolution": 0.001373291015625, "scale": 577143.73644287116 },
                  { "level": 11, "resolution": 0.0006866455078125, "scale": 288571.86822143558 },
                  { "level": 12, "resolution": 0.00034332275390625, "scale": 144285.93411071779 },
                  { "level": 13, "resolution": 0.000171661376953125, "scale": 72142.967055358895 },
                  { "level": 14, "resolution": 8.58306884765625e-005, "scale": 36071.483527679447 },
                  { "level": 15, "resolution": 4.291534423828125e-005, "scale": 18035.741763839724 },
                  { "level": 16, "resolution": 2.1457672119140625e-005, "scale": 9017.8708819198619 },
                  { "level": 17, "resolution": 1.0728836059570313e-005, "scale": 4508.9354409599309 },
                  { "level": 18, "resolution": 5.3644180297851563e-006, "scale": 2254.4677204799655 },
                  { "level": 19, "resolution": 5.3644180297851563e-006 / 2, "scale": 2254.4677204799655 /2 },
                  { "level": 20, "resolution": 5.3644180297851563e-006 / 4, "scale": 2254.4677204799655/4},
                  { "level": 21, "resolution": 5.3644180297851563e-006/8, "scale": 2254.4677204799655 /8},
                  { "level": 22, "resolution": 5.3644180297851563e-006 / 16, "scale": 2254.4677204799655 / 16 },
                  { "level": 23, "resolution": 5.3644180297851563e-006 / 2, "scale": 2254.4677204799655 /2 },
                  { "level": 24, "resolution": 5.3644180297851563e-006 / 4, "scale": 2254.4677204799655/4},
                  { "level": 25, "resolution": 5.3644180297851563e-006/8, "scale": 2254.4677204799655 /8},
                  { "level": 26, "resolution": 5.3644180297851563e-006 / 2, "scale": 2254.4677204799655 /2 },
                  { "level": 27, "resolution": 5.3644180297851563e-006 / 4, "scale": 2254.4677204799655/4},
                  { "level": 29, "resolution": 5.3644180297851563e-006 / 8, "scale": 2254.4677204799655 / 8 },
                  { "level": 30, "resolution": 5.3644180297851563e-006 / 4, "scale": 2254.4677204799655 / 4 },
                  { "level": 31, "resolution": 5.3644180297851563e-006 / 8, "scale": 2254.4677204799655 / 8 },
                  { "level": 31, "resolution": 5.3644180297851563e-006 / 8, "scale": 2254.4677204799655 / 8 }
                ]
            });

            this.loaded = true;
            this.onLoad(this);
        },

        getTileUrl: function (level, row, col) {
            try {
                //return url = 'http://webrd0' + (col % 4 + 1) + '.is.autonavi.com/appmaptile?lang=zh_cn&size=1&scale=1&style=8&x=' + col + '&y=' + row + '&z=' + level;
                return "http://t" + col % 8 + ".tianditu.cn/vec_c/wmts?SERVICE=WMTS&REQUEST=GetTile&VERSION=1.0.0&LAYER=vec&STYLE=default&TILEMATRIXSET=c&TILEMATRIX=" + level + "&TILEROW=" + row + "&TILECOL=" + col + "&FORMAT=tiles";
            }
            catch (e) { }
            //return "http://t" + col % 8 + ".tianditu.cn/vec_c/wmts?SERVICE=WMTS&REQUEST=GetTile&VERSION=1.0.0&LAYER=vec&STYLE=default&TILEMATRIXSET=c&TILEMATRIX=" + level + "&TILEROW=" + row + "&TILECOL=" + col + "&FORMAT=tiles";
        }
    });
}