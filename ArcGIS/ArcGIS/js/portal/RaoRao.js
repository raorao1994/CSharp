this.RaoRao || (RaoRao = {}),
this["RaoRao.Window"] || (RaoRao.Window = {
    instances:[],
    InfoPanel: function (t, e, n) {
        var i = !1;
        if ($.each(RaoRao.Window.instances,
        function (t, e) {
            if (n.id || (n.id = "infoPanel_" + ++RaoRao.Window._sequnce), e && e.id == n.id) {
                i = !0;
                var o = e.obj;
                return o && "minimized" == o.status && o.normalize(),
                !1
        }
        }), i) return $.each(RaoRao.Window.instances,
        function (t, e) {
            var i = e.obj;
            return e.id == n.id ? !0 : void (i && "minimized" != i.status && i.minimize())
        }),
        !1;
        //var o = {
        //    title: "",
        //    paneltype: "normal",
        //    autoclose: 50,
        //    position: "center",
        //    resizable: "disabled",
        //    theme: "light",
        //    content: "",
        //    draggable: {
        //        appendTo: "#Main_Map_DIV",
        //        containment: "parent",
        //        opacity: 1,
        //        revertDuration: 1,
        //        drag: function (t, e) { }
        //    },
        //    callback: [function (t) { }]
        //};
        var o = {
            position: { my: "center", at: "center"},
            theme: "rebeccapurple",
            contentSize: { width: 600, height: 350 },
            headerTitle: t,
            content: "",
            callback: [function (t) { }],
            onclosed: null
        };
        t && (o.title = t),
        e && (o.content = e),
        n && (o = $.extend(!0, o, n));
        o.onclosed = function () {
            $.each(RaoRao.Window.instances, function (index,item) {
                if (item.id == n.id)
                {
                    RaoRao.Window.instances.pop(item);
                }
            });
        };
        var a = $.jsPanel(o);
        $(".jsPanel-title").css({
            "font-size": "16px",
            "margin-top": "2px"
        }),
        $(".jsPanel-state-normalized").animate({ opacity: '1', top: a.option.position.top }, 500);
        0 != n.minimizeOthers && $.each(RaoRao.Window.instances,
        function (t, e) {
            var n = e.obj;
            n && "minimized" != n.status && n.minimize()
        }),
        RaoRao.Window.instances.push({
            id: n.id,
            obj: a
        })
    },
    CloseAllInfoPanel: function () {
        RaoRao.Window._closeAll = !0,
        $.each(RaoRao.Window.instances,
        function (t, e) {
            if (!e || !e.obj) return !0;
            var n = e.obj;
            n.close()
        }),
        RaoRao.Window._closeAll = !1
    },
    MinInfoPanelByID: function (t) {
        $.each(RaoRao.Window.instances,
        function (e, n) {
            if (null != n && n.id == t) {
                var i = n.obj;
                i.smallify()
            }
        })
    },
    minimizeInfoPanelByID: function (t) {
        $.each(RaoRao.Window.instances,
        function (e, n) {
            if (null != n && n.id == t) {
                var i = n.obj;
                i.minimize()
            }
        })
    },
    normalizeInfoPanelByID: function (t) {
        $.each(RaoRao.Window.instances,
        function (e, n) {
            if (null != n && n.id == t) {
                var i = n.obj;
                i.normalize()
            }
        })
    },
    niniAll: function () {
        $.each(RaoRao.Window.instances,
        function (t, e) {
            var n = e.obj;
            n && "minimized" != n.status && n.minimize()
        })
    },
    GetInfoPanelByID: function (t) {
        var e = null;
        return $.each(RaoRao.Window.instances,
        function (n, i) {
            return null != i && i.id == t ? void (e = i.obj) : void 0
        }),
        e
    }
});