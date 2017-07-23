/*
*@作者：陈兴旺
*@说明：arcgis自定义infowindow
*/

//引用css样式
function esriDivPanelCss(Map, x, y, title, html, height, width) {
    //加载时地图坐标
    var X = x, Y = y, map = Map;
    //弹窗宽度
    var Heigth = 300;
    if (height) { Height = height }
    //弹窗高度
    var Width = 200;
    if (width) { Width = width }
    //弹窗位置
    var top = 0;
    //弹窗位置
    var left = 0;
    //弹窗对象
    var Div = null;
    //移动量
    var movX = 0, movY = 0;
    //变化量
    var tempX = 0, tempY = 0;
    //地图拖拽事件
    this._panEvt = null;
    this._panEndEvt = null;
    //地图所缩放事件
    this._zoomStartEvt = null;
    this._zoomEndEvt = null;
    //弹窗DIV
    this.div = document.createElement("div");
    Div = this.div;
    this.div.className = "esriDivPanel";
    var divcss = 'width:' + Width + 'px;height:' + Heigth + 'px;';
    this.div.style.cssText = divcss;

    this.title = document.createElement("div");
    this.title.className = "esriDivPanel_title";
    var close = document.createElement("div");
    close.className = "esriDivPanel_titleClose";
    close.innerHTML = "<span></span>";

    var titletext = document.createElement("div");
    titletext.className = "esriDivPanel_titleTxt";
    titletext.innerHTML = title;

    var content = document.createElement("div");
    content.className = "esriDivPanel_content";
    content.innerHTML = html;
    content.style.cssText = "height:" + (Heigth - 32) + "px;";

    this.title.appendChild(close);
    this.title.appendChild(titletext);

    this.div.appendChild(this.title);
    this.div.appendChild(content);

    var point = new esri.geometry.Point(x, y, map.spatialReference);
    var p = map.toScreen(point);
    top = p.y - Heigth;
    left = p.x - Width / 2;
    this.div.style.top = top + "px";
    this.div.style.left = left + "px";
    document.getElementById(map.id).appendChild(this.div);

    //定义地图缩放事件
    this._zoomStartEvt = map.on("zoom-start", function (evt) {
        var point = new esri.geometry.Point(X, Y, map.spatialReference);
        var p = map.toScreen(point);
        top = p.y - Heigth;
        left = p.x - Width / 2;
        Div.style.top = top + "px";
        Div.style.left = left + "px";
    });
    this._zoomEndEvt = map.on("zoom-end", function (evt) {
        var point = new esri.geometry.Point(X, Y, map.spatialReference);
        var p = map.toScreen(point);
        top = p.y - Heigth;
        left = p.x - Width / 2;;
        Div.style.top = top + "px";
        Div.style.left = left + "px";
    });

    //定义地图拖拽事件
    this._panEvt = map.on("pan", function (evt) {
        var point = evt.delta;
        movX = point.x - tempX;
        movY = point.y - tempY;
        tempX = point.x; tempY = point.y;
        top = top + movY;
        left = left + movX;
        Div.style.top = top + "px";
        Div.style.left = left + "px";
    });
    this._panEndEvt = map.on("pan-end", function (evt) {
        tempX = 0;
        tempY = 0;
    });

    //定义关闭事件
    close.onclick = function () {
        if (this._panEndEvt) {
            this._panEndEvt.remove();
        }
        if (this._panEvt) {
            this._panEvt.remove();
        }
        if (this._zoomEndEvt) {
            this._zoomEndEvt.remove();
        }
        if (this._zoomStartEvt) {
            this._zoomStartEvt.remove();
        }
        this._panEndEvt = null;
        this._panEvt = null;
        this._zoomEndEvt = null;
        this._zoomStartEvt = null;
        document.getElementById(map.id).removeChild(Div);
    }
    //设置标题
    this.setTitle = function (title) {
        titletext.innerHTML = title;
    }
    //设置内容
    this.setContent = function (html) {
        content.innerHTML = html;
    }

    return this;
}

//样式为js
function esriDivPanel(Map, x, y, title, html, height, width) {
    //加载时地图坐标
    var X = x, Y = y, map = Map;
    //弹窗宽度
    var Heigth = 300;
    if (height) { Height = height }
    //弹窗高度
    var Width = 200;
    if (width) { Width = width }
    //弹窗位置
    var top = 0;
    //弹窗位置
    var left = 0;
    //弹窗对象
    var Div = null;
    //移动量
    var movX = 0, movY = 0;
    //变化量
    var tempX = 0, tempY = 0;
    //地图拖拽事件
    this._panEvt = null;
    this._panEndEvt = null;
    //地图所缩放事件
    this._zoomStartEvt = null;
    this._zoomEndEvt = null;
    //弹窗DIV
    this.div = document.createElement("div");
    Div = this.div;
    this.div.className = "esriDivPanel";
    var divcss = 'width:' + Width + 'px;height:' + Heigth + 'px;';
    divcss += 'position:absolute;z-index:100;';
    this.div.style.cssText = divcss;

    this.title = document.createElement("div");
    this.title.className = "esriDivPanel_title";
    this.title.style.cssText = "border: 2px solid rgb(140, 151, 148);height: 32px;width: 100%;background-color:#8c9794;border-radius: 5px 5px 0px 0px;";
    var close = document.createElement("div");
    close.className = "esriDivPanel_titleClose";
    close.style.cssText = "float: right;background-color: blue;width: 24px;height: 24px;margin: 5px;";

    var titletext = document.createElement("div");
    titletext.className = "esriDivPanel_titleTxt";
    titletext.innerHTML = title;
    titletext.style.cssText = "overflow: hidden;width: 75%;height: 32px;line-height: 32px;margin-left: 5px;color: white;";

    var content = document.createElement("div");
    content.className = "esriDivPanel_content";
    content.innerHTML = html;
    content.style.cssText = "width: 100%;border: 2px solid #8c9794;background-color: white;height:" + (Heigth - 32) + "px;";

    this.title.appendChild(close);
    this.title.appendChild(titletext);

    this.div.appendChild(this.title);
    this.div.appendChild(content);

    var point = new esri.geometry.Point(x, y, map.spatialReference);
    var p = map.toScreen(point);
    top = p.y - Heigth;
    left = p.x - Width / 2;
    this.div.style.top = top + "px";
    this.div.style.left = left + "px";
    document.getElementById(map.id).appendChild(this.div);

    //定义地图缩放事件
    this._zoomStartEvt = map.on("zoom-start", function (evt) {
        var point = new esri.geometry.Point(X, Y, map.spatialReference);
        var p = map.toScreen(point);
        top = p.y - Heigth;
        left = p.x - Width / 2;
        Div.style.top = top + "px";
        Div.style.left = left + "px";
    });

    this._zoomEndEvt = map.on("zoom-end", function (evt) {
        var point = new esri.geometry.Point(X, Y, map.spatialReference);
        var p = map.toScreen(point);
        top = p.y - Heigth;
        left = p.x - Width / 2;;
        Div.style.top = top + "px";
        Div.style.left = left + "px";
    });

    //定义地图拖拽事件
    this._panEvt = map.on("pan", function (evt) {
        var point = evt.delta;
        movX = point.x - tempX;
        movY = point.y - tempY;
        tempX = point.x; tempY = point.y;
        top = top + movY;
        left = left + movX;
        Div.style.top = top + "px";
        Div.style.left = left + "px";
    });

    this._panEndEvt = map.on("pan-end", function (evt) {
        tempX = 0;
        tempY = 0;
    });

    //定义关闭事件
    close.onclick = function () {
        if (this._panEndEvt) {
            this._panEndEvt.remove();
        }
        if (this._panEvt) {
            this._panEvt.remove();
        }
        if (this._zoomEndEvt) {
            this._zoomEndEvt.remove();
        }
        if (this._zoomStartEvt) {
            this._zoomStartEvt.remove();
        }
        this._panEndEvt = null;
        this._panEvt = null;
        this._zoomEndEvt = null;
        this._zoomStartEvt = null;
        document.getElementById(map.id).removeChild(Div);
    }

    //设置标题
    this.setTitle = function (title) {
        titletext.innerHTML = title;
    }
    //设置内容
    this.setContent = function (html) {
        content.innerHTML = html;
    }

    return this;
}

//样式
//<style>
//        .esriDivPanel {
//    position: absolute;
//    z-index: 100;
//}

//.esriDivPanel_title {
//    border: 2px solid rgb(140, 151, 148);
//    height: 32px;
//    width: 100%;
//    background-color: #8c9794;
//    border-radius: 5px 5px 0px 0px;
//}

//.esriDivPanel_titleClose {
//    float: right;
//    width: 24px;
//    height: 24px;
//    margin: 5px;
//}

//.esriDivPanel_titleClose span {
//    display: inline-block;
//    width: 100%;
//    height: 100%;
//    text-align: center;
//    overflow: hidden;
//    position: relative;
//}

//.esriDivPanel_titleClose span:hover {
//    background-color: slategrey;
//}

//.esriDivPanel_titleClose span::before, .esriDivPanel_titleClose span::after {
//    position: absolute;
//    content: '';
//    top: 50%;
//    left: 0;
//    margin-top: -1px;
//    background-color: #000000;
//    width: 100%;
//    height: 3px;
//}

//.esriDivPanel_titleClose span::before {
//    -webkit-transform: rotate(45deg);
//    -moz-transform: rotate(45deg);
//}

//.esriDivPanel_titleClose span::after {
//    -webkit-transform: rotate(-45deg);
//    -moz-transform: rotate(-45deg);
//}

//.esriDivPanel_titleTxt {
//    overflow: hidden;
//    width: 75%;
//    height: 32px;
//    line-height: 32px;
//    margin-left: 5px;
//    color: white;
//}

//.esriDivPanel_content {
//    width: 100%;
//    border: 2px solid #8c9794;
//    background-color: white;
//}
//</style>