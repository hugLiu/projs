//公共变量 
var gdb_url = "";
var xishanyao_gdb_url = "";
var qigu_gdb_url = "";
var carboniferous_gdb_url = "";
var cur_selfun = "tab1";
var tempBaseInfo;
var currentStart;
var currentEnd;

var jogis4_options = {
    Style: { border: "solid 1px #666", width: "100%" },
    ShowEagleEye: true
};

var reserveTreeData;
var reservesQueryResultData;

//计算单元
var initUnitTree = function (jsondata) {
    $("#calculate-unit-tree")
        .jstree({
            'plugins': ['types', 'dnd'],
            'types': {
                'default': {
                    'icon': 'fa fa-folder'
                },
                'html': {
                    'icon': 'fa fa-file-code-o'
                },
                'svg': {
                    'icon': 'fa fa-file-picture-o'
                },
                'css': {
                    'icon': 'fa fa-file-code-o'
                },
                'img': {
                    'icon': 'fa fa-file-image-o'
                },
                'js': {
                    'icon': 'fa fa-file-text-o'
                }
            },
            'core': {
                //"multiple" : true,//多选
                "animation": true, //动画
                "data": jsondata
            }
        });
}

//储量类别
var initReserveTree = function (jsondata) {
    $("#reserve-category-tree").jstree({
        'plugins': ['types', 'dnd'],
        'types': {
            'default': {
                'icon': 'fa fa-folder'
            },
            'html': {
                'icon': 'fa fa-file-code-o'
            },
            'svg': {
                'icon': 'fa fa-file-picture-o'
            },
            'css': {
                'icon': 'fa fa-file-code-o'
            },
            'img': {
                'icon': 'fa fa-file-image-o'
            },
            'js': {
                'icon': 'fa fa-file-text-o'
            }
        },
        'core': {
            "multiple": true,//多选
            "animation": true, //动画
            "data": jsondata
        }
    });
}

//层系
var initLayerTree = function (jsondata) {
    $("#layer-type-tree").jstree({
        'plugins': ['types', 'dnd'],
        'types': {
            'default': {
                'icon': 'fa fa-folder'
            },
            'html': {
                'icon': 'fa fa-file-code-o'
            },
            'svg': {
                'icon': 'fa fa-file-picture-o'
            },
            'css': {
                'icon': 'fa fa-file-code-o'
            },
            'img': {
                'icon': 'fa fa-file-image-o'
            },
            'js': {
                'icon': 'fa fa-file-text-o'
            }
        },
        'core': {
            "multiple": true,//多选
            "animation": true, //动画
            "data": jsondata
        }
    });
}

function initTree() {
    $.ajax({
        url: "/BaseInfo/GetBOBaseInfo",
        type: "get",
        data: { boc: "三级储量分布/计算单元" },
        dataType: "json",
        success: function (result) {
            var arr = [];
            result.forEach(function (value) {
                arr.push({
                    "id": value.ID,
                    "parent": value.PID == null ? "#" : value.PID,
                    "text": value.Name,
                    "state": {
                        "opened": value.PID == null ? true : false,
                        "selected": value.PID == null ? true : false
                    }
                });
            });
            initUnitTree(arr);
        }
    });


    $.ajax({
        url: "/BaseInfo/ThreeLevGetReserveTreeData",
        type: "get",
        success: function (json) {
            var data = JSON.parse(json);
            reserveTreeData = eval("(" + data + ")");
            initReserveTree(reserveTreeData);
        }
    });

    $.ajax({
        url: "/BaseInfo/GetBOBaseInfo",
        type: "get",
        data: { boc: "三级储量分布/地质年代" },
        dataType: "json",
        success: function (result) {
            tempBaseInfo = result;
            var arr = [];
            result.forEach(function (value) {
                arr.push({
                    "id": value.ID,
                    "parent": value.PID == null ? "#" : value.PID,
                    "text": value.Name,
                    "bot": value.BOT,
                    "state": {
                        "opened": value.PID == null ? true : false,
                        "selected": value.PID == null ? true : false
                    }
                });
            });
            initLayerTree(arr);
        }
    });
}

function initEvent() {
    $("#calculate-unit-tree").on('changed.jstree', function (e, data) {
        var id = "";
        var boc = "三级储量分布/计算单元";
        var domId = "table_list_unit_result";
        var treeId = "calculate-unit-tree";
        if (data.action == "model") {
            id = data.selected[0];
            loadUnitGrid(domId, treeId);
        }
        else if (data.action == "select_node") {
            id = data.node.id;
            var pid = data.node.parent;
            if (pid == "#") {
                loadUnitGrid2(domId);
            } else {
                updateUnitGrid(id, domId);
            }
        }
        if (id != "") {
            loadGdb(boc, id);
        }
    });

    $("#reserve-category-tree").on('changed.jstree', function (e, data) {
        var entity = data.node;
        if (entity) {
            var id = entity.id;
            var url = "";
            if (id == "0") {
                //全部
                url = "GDB/三级储量/储量类别/8d7c5edb-f2fc-4b55-82e8-d333954a7ffc.GDB";
            } else if (id == "01") {
                //探明
                url = "GDB/三级储量/储量类别/探明/55f4920a-ab49-4fa2-8018-f5a345ccfdcc.GDBX";
            } else if (id == "0101") {
                //探明 已开发
                url = "GDB/三级储量/储量类别/探明/已开发/17317b92-1b71-4942-94e2-c34bf39f4f1a.GDBX";
            } else if (id == "0102") {
                //探明 未开发
                url = "GDB/三级储量/储量类别/探明/未开发/7c18b220-cb9b-4875-98cc-46ae4cbc0d7c.GDBX";
            } else if (id == "02") {
                //控制
                url = "GDB/三级储量/储量类别/控制/3b46bf39-fe8f-4fde-a6eb-871718416e35.GDBX";
            } else if (id == "03") {
                //预测
                url = "";
            }
            if (url == "") {
                return;
            }
            url = defaultUrl.ResourceDefaultUrl + encodeURI(url);
            loadGdbFile(url);
        }
        //if (entity && entity.id == "0") {
        //    loadGdbFile(gdb_url);
        //}
        //if (entity && entity.id != "0") {
        //    var title = [];
        //    var pid = entity.id;
        //    var _title = entity.original.layer_title ? entity.original.layer_title : "";
        //    for (var i in reserveTreeData) {
        //        var parentid = reserveTreeData[i].parent;
        //        if (parentid == pid) {
        //            title.push(reserveTreeData[i].layer_title);
        //        } else {
        //            if (_title && title.indexOf(_title) < 0)
        //                title.push(_title);
        //        }
        //    }

        //    var count = joGIS1.LayerCount;
        //    for (var i = 0; i < count; i++) {
        //        var name = joGIS1.GetLayerName(i);
        //        joGIS1.ShowLayer(name, 1);
        //    }
        //    for (var i = 0; i < count; i++) {
        //        var name = joGIS1.GetLayerName(i);
        //        if (title.indexOf(name) < 0) {
        //            joGIS1.ShowLayer(name, 0);
        //        }
        //    }
        //}
    });

    $("#layer-type-tree").on('changed.jstree', function (e, data) {
        var id = "";
        var boc = "三级储量分布/地质年代";
        var domId = "table_age_result";
        var treeId = "layer-type-tree";
        if (data.action == "model") { //init
            id = data.selected[0];
            loadUnitGrid(domId, treeId);
        }
        else if (data.action == "select_node") {
            id = data.node.id;
            var pid = data.node.parent;
            if (pid == "#") {
                loadUnitGrid2(domId);
            } else {
                var bot = data.node.original.bot;
                var name = data.node.text;
                updateLayerGrid(domId, bot, name);
            }
        }
        if (id != "") {
            loadGdb(boc, id);
        }
    });
}

function loadGdb(boc, id) {
    $.ajax({
        url: "/BaseInfo/GetBOBaseInfoUrl",
        type: "get",
        data: { boc: boc, id: id, feature: "Reserve" },
        dataType: "json",
        success: function (url) {
            if (url) {
                url = defaultUrl.ResourceDefaultUrl + encodeURI(url);
                loadGdbFile(url);
            }
        }
    });
}

function loadUnitGrid(domId, treeId) {
    getBaseInfoViewData(domId, treeId);
}

function loadUnitGrid2(domId) {
    getBaseInfoViewData2(domId);
}

function updateUnitGrid(id, domId) {
    getBaseInfoViewDataById(id, domId);
}

function updateLayerGrid(domId, bot, name) {
    $.ajax({
        url: "/BaseInfo/GetBaseInfoViewDataByBOT",
        type: "post",
        data: { bot: bot, name: name },
        success: function (result) {
            if (!result) return;
            var d = result.split('#');
            var data1 = eval('(' + d[0] + ')');
            var arr = eval('(' + d[1] + ')');
            var temp = [];
            for (var i = 0; i < arr.length; i++) {
                if (arr[i] == "id") {
                    temp.push({ "index": arr[i], "name": arr[i], hidden: true });
                } else {
                    temp.push({ "index": arr[i], "name": arr[i] });
                }
            }
            var data2 = temp;
            var data3 = eval('(' + d[2] + ')');
            var data4 = eval('(' + d[3] + ')');
            $("#" + domId).jqGrid("clearGridData");
            $("#" + domId).jqGrid("setGridParam", { data: data4 }).trigger("reloadGrid");
        }
    });
}

function getBaseInfoViewData(domId, treeId) {
    $.ajax({
        url: "/BaseInfo/GetBaseInfoViewData",
        type: "post",
        success: function (result) {
            if (!result) return;
            var d = result.split('#');
            var data1 = eval('(' + d[0] + ')');
            var arr = eval('(' + d[1] + ')');
            var temp = [];
            for (var i = 0; i < arr.length; i++) {
                if (arr[i] == "id") {
                    temp.push({ "index": arr[i], "name": arr[i], hidden: true });
                } else {
                    temp.push({ "index": arr[i], "name": arr[i] });
                }
            }
            var data2 = temp;
            var data3 = eval('(' + d[2] + ')');
            var data4 = eval('(' + d[3] + ')');
            loadGrid(domId, treeId, data1, data2, data3, data4);
        }
    });
}

function getBaseInfoViewData2(domId) {
    $.ajax({
        url: "/BaseInfo/GetBaseInfoViewData",
        type: "post",
        success: function (result) {
            if (!result) return;
            var d = result.split('#');
            var data1 = eval('(' + d[0] + ')');
            var arr = eval('(' + d[1] + ')');
            var temp = [];
            for (var i = 0; i < arr.length; i++) {
                if (arr[i] == "id") {
                    temp.push({ "index": arr[i], "name": arr[i], hidden: true });
                } else {
                    temp.push({ "index": arr[i], "name": arr[i] });
                }
            }
            var data2 = temp;
            var data3 = eval('(' + d[2] + ')');
            var data4 = eval('(' + d[3] + ')');
            $("#" + domId).jqGrid("clearGridData");
            $("#" + domId).jqGrid("setGridParam", { data: data4 }).trigger("reloadGrid");
        }
    });
}

function getBaseInfoViewDataById(id, domId) {
    $.ajax({
        url: "/BaseInfo/GetBaseInfoViewDataById",
        type: "post",
        data: { id: id },
        success: function (result) {
            if (!result) return;
            var d = result.split('#');
            var data1 = eval('(' + d[0] + ')');
            var arr = eval('(' + d[1] + ')');
            var temp = [];
            for (var i = 0; i < arr.length; i++) {
                if (arr[i] == "id") {
                    temp.push({ "index": arr[i], "name": arr[i], hidden: true });
                } else {
                    temp.push({ "index": arr[i], "name": arr[i] });
                }
            }
            var data2 = temp;
            var data3 = eval('(' + d[2] + ')');
            var data4 = eval('(' + d[3] + ')');
            $("#" + domId).jqGrid("clearGridData");
            $("#" + domId).jqGrid("setGridParam", { data: data4 }).trigger("reloadGrid");
        }
    });
}

function loadGrid(domId, treeId, data1, data2, data3, data4) {
    $("#" + domId)
        .jqGrid({
            data: data4,
            datatype: "local",
            height: 150,
            rowNum: data4.length + 1,
            colNames: data1,
            colModel: data2,
            shrinkToFit: false,
            autoScroll: true,
            gridview: true,
            scrollrows: true,
            pgbuttons: false,
            pginput: false,
            viewrecords: true,
            add: false,
            edit: false,
            hidegrid: false,
            onSelectRow: function (id) {
                var thisId = $("#" + domId).getCell(id, "id");
                $('#' + treeId).jstree('select_node', thisId);
                if (domId == "table_age_result") {
                    if (thisId != "") {
                        loadGdb("三级储量分布/计算单元", thisId);
                    }
                }
            }
        });
    $("#" + domId).setGroupHeaders({
        useColSpanStyle: true,
        groupHeaders: data3
    });
}

function loadSearchGrid(domId, data1, data2, data3, data4) {
    $("#" + domId)
        .jqGrid({
            data: data4,
            datatype: "local",
            height: 150,
            rowNum: data4.length + 1,
            colNames: data1,
            colModel: data2,
            shrinkToFit: false,
            autoScroll: true,
            gridview: true,
            scrollrows: true,
            pgbuttons: false,
            pginput: false,
            viewrecords: true,
            add: false,
            edit: false,
            hidegrid: false,
            onSelectRow: function (id) {
                var thisId = $("#" + domId).getCell(id, "id");
                if (thisId != "") {
                    loadGdb("三级储量分布/计算单元", thisId);
                }
            }
        });
    $("#" + domId).setGroupHeaders({
        useColSpanStyle: true,
        groupHeaders: data3
    });
}

function loadGdbFile(url) {
    jogis4_options.GDBPath = url;
    try {
        JoGis4 = new JoGis(joGIS1, jogis4_options);
        JoGis4.loadGeoMapFile(jogis4_options.GDBPath);
        joGIS1.SelectMode = 0;
        joGIS1.ShowToolTips(1);
        df_ZoomScale = joGIS1.ZoomScale; //记录默认比例尺
    } catch (e) {
        console.log(e);
    }
}

function createContextMenu() {
    joGIS1.ResetTools();
    // 定制右键菜单测试
    joGIS1.AddTool(0); //选择
    joGIS1.AddTool(13); //放大
    joGIS1.AddTool(14); //缩小
    joGIS1.AddTool(15); //移动/手势
    joGIS1.AddTool(17); //复位
    // 定制自定义菜单测试
    joGIS1.AddCustomTool("下载", 99); //菜单开始
}

function setIonRage() {
    $.ajax({
        url: "/BaseInfo/GetBaseInfoViewDataForYear",
        type: "get",
        success: function (result) {
            var len = result.length;
            var start = parseInt(result[0].Year);//new Date(parseInt(result[0].Year.replace("/Date(", "").replace(")/", ""))).getFullYear();
            var end = parseInt(result[len - 1].Year);//new Date(parseInt(result[result.length - 1].Year.replace("/Date(", "").replace(")/", ""))).getFullYear();
            $("#ionrange_1").ionRangeSlider({
                min: start,
                max: end,
                type: 'double',
                postfix: "年",
                prettify: false,
                hasGrid: false,
                onChange: function (obj) { // function-callback, is called on every change 
                    var startY = obj.fromNumber;
                    var endY = obj.toNumber;
                    chosenEvent(startY, endY);
                },
                onFinish: function (obj) { // function-callback, is called once, after slider finished it's work
                    console.log(obj);
                }
            });
        }
    });
}

function chosenEvent(startY, endY) {
    $.ajax({
        url: "/BaseInfo/GetBaseInfoViewDataForCategory",
        data: { startYear: startY, endYear: endY },
        type: "get",
        success: function (result) {
            bindResCategory(result);
        }
    });
}

function bindResCategory(temp) { 
    $("#resCategory").empty();
    $("#resCategory").append($("<option value=\"\">选择储量类别...</option>"));
    $("#resCategory").trigger("chosen:updated");
    $("#series").empty();
    $("#series").append($("<option value=\"\">选择系...</option>"));
    $("#series").trigger("chosen:updated");
    $("#formation").empty();
    $("#formation").append($("<option value=\"\">选择组...</option>"));
    $("#formation").trigger("chosen:updated");
    $("#layer").empty();
    $("#layer").append($("<option value=\"\">选择层...</option>"));
    $("#layer").trigger("chosen:updated");
    $("#blockGuan").empty();
    $("#blockGuan").append($("<option value=\"\">选择井区...</option>"));
    $("#blockGuan").trigger("chosen:updated");
    for (var i = 0; i < temp.length; i++) {
        $("#resCategory").append($("<option value=\"" + temp[i] + "\" hassubinfo=\"true\">" + temp[i] + "</option>"));
    }
    $("#resCategory").trigger("chosen:updated");
    $("#resCategory").unbind("change", selectSeries);
    $("#resCategory").bind("change", selectSeries);
}

function selectSeries() { 
    $("#series").empty();
    $("#series").append($("<option value=\"\">选择系...</option>"));
    $("#series").trigger("chosen:updated");
    $("#formation").empty();
    $("#formation").append($("<option value=\"\">选择组...</option>"));
    $("#formation").trigger("chosen:updated");
    $("#layer").empty();
    $("#layer").append($("<option value=\"\">选择层...</option>"));
    $("#layer").trigger("chosen:updated");
    $("#blockGuan").empty();
    $("#blockGuan").append($("<option value=\"\">选择井区...</option>"));
    $("#blockGuan").trigger("chosen:updated");
    var strYear = $("#ionrange_1").val().split(';');
    if ($("#resCategory").val()) {
        $.ajax({
            url: "/BaseInfo/GetBaseInfoViewDataForSeries",
            type: "get",
            data: { startYear: strYear[0], endYear: strYear[1], category: $("#resCategory").val() },
            success: function (result) {
                $("#series").empty();
                $("#series").append($("<option value=\"\">选择系...</option>"));
                for (var i = 0; i < result.length; i++) {
                    $("#series").append($("<option value=\"" + result[i] + "\" hassubinfo=\"true\">" + result[i] + "</option>"));
                }
                $("#series").trigger("chosen:updated");
                $("#series").unbind("change", selectFormation);
                $("#series").bind("change", selectFormation);
            }
        });
    } else {
        //$("#series").empty();
        //$("#series").append($("<option value=\"\">选择系...</option>"));
        //$("#formation").empty();
        //$("#formation").append($("<option value=\"\">选择组...</option>"));
        //$("#layer").empty();
        //$("#layer").append($("<option value=\"\">选择层...</option>"));
        //$("#blockGuan").empty();
        //$("#blockGuan").append($("<option value=\"\">选择井区...</option>"));
        //$("#resCategory").trigger("chosen:updated");
        //$("#series").trigger("chosen:updated");
        //$("#formation").trigger("chosen:updated");
        //$("#layer").trigger("chosen:updated");
        //$("#blockGuan").trigger("chosen:updated");
    }
}

function selectFormation() { 
    $("#formation").empty();
    $("#formation").append($("<option value=\"\">选择组...</option>"));
    $("#formation").trigger("chosen:updated");
    $("#layer").empty();
    $("#layer").append($("<option value=\"\">选择层...</option>"));
    $("#layer").trigger("chosen:updated");
    $("#blockGuan").empty();
    $("#blockGuan").append($("<option value=\"\">选择井区...</option>"));
    $("#blockGuan").trigger("chosen:updated");
    var strYear = $("#ionrange_1").val().split(';');
    if ($("#resCategory").val() && $("#series").val()) {
        $.ajax({
            url: "/BaseInfo/GetBaseInfoViewDataForFormation",
            type: "get",
            data: { startYear: strYear[0], endYear: strYear[1], category: $("#resCategory").val(), series: $("#series").val() },
            success: function (result) {
                $("#formation").empty();
                $("#formation").append($("<option value=\"\">选择组...</option>"));
                for (var i = 0; i < result.length; i++) {
                    $("#formation").append($("<option value=\"" + result[i] + "\" hassubinfo=\"true\">" + result[i] + "</option>"));
                }
                $("#formation").trigger("chosen:updated");
                $("#formation").unbind("change", selectLayer);
                $("#formation").bind("change", selectLayer);
            }
        });
    } else {
        //$("#formation").empty();
        //$("#formation").append($("<option value=\"\">选择组...</option>"));
        //$("#layer").empty();
        //$("#layer").append($("<option value=\"\">选择层...</option>"));
        //$("#blockGuan").empty();
        //$("#blockGuan").append($("<option value=\"\">选择井区...</option>"));
        //$("#resCategory").trigger("chosen:updated");
        //$("#series").trigger("chosen:updated");
        //$("#formation").trigger("chosen:updated");
        //$("#layer").trigger("chosen:updated");
        //$("#blockGuan").trigger("chosen:updated");
    }
}

function selectLayer() {
    $("#layer").empty();
    $("#layer").append($("<option value=\"\">选择层...</option>"));
    $("#layer").trigger("chosen:updated");
    $("#blockGuan").empty();
    $("#blockGuan").append($("<option value=\"\">选择井区...</option>"));
    $("#blockGuan").trigger("chosen:updated");
    var strYear = $("#ionrange_1").val().split(';');
    if ($("#resCategory").val() && $("#series").val() && $("#formation").val()) {
        $.ajax({
            url: "/BaseInfo/GetBaseInfoViewDataForLayer",
            type: "get",
            data: { startYear: strYear[0], endYear: strYear[1], category: $("#resCategory").val(), series: $("#series").val(), formation: $("#formation").val() },
            success: function (result) {
                $("#layer").empty();
                $("#layer").append($("<option value=\"\">选择层...</option>"));
                for (var i = 0; i < result.length; i++) {
                    $("#layer").append($("<option value=\"" + result[i] + "\" hassubinfo=\"true\">" +
                            result[i] + "</option>"));
                }
                $("#layer").trigger("chosen:updated");
                $("#layer").unbind("change", selectBlockGuan);
                $("#layer").bind("change", selectBlockGuan);
            }
        });
    } else {
        //$("#layer").empty();
        //$("#layer").append($("<option value=\"\">选择层...</option>"));
        //$("#blockGuan").empty();
        //$("#blockGuan").append($("<option value=\"\">选择井区...</option>"));
        //$("#resCategory").trigger("chosen:updated");
        //$("#series").trigger("chosen:updated");
        //$("#formation").trigger("chosen:updated");
        //$("#layer").trigger("chosen:updated");
        //$("#blockGuan").trigger("chosen:updated");
    }
}

function selectBlockGuan() {
    $("#blockGuan").empty();
    $("#blockGuan").append($("<option value=\"\">选择井区...</option>"));
    $("#blockGuan").trigger("chosen:updated");
    var strYear = $("#ionrange_1").val().split(';');
    if ($("#resCategory").val() && $("#series").val() && $("#formation").val() && $("#layer").val()) {
        $.ajax({
            url: "/BaseInfo/GetBaseInfoViewDataForBlockGuan",
            type: "get",
            data: { startYear: strYear[0], endYear: strYear[1], category: $("#resCategory").val(), series: $("#series").val(), formation: $("#formation").val(), layer: $("#layer").val() },
            success: function (result) {
                $("#blockGuan").empty();
                $("#blockGuan").append($("<option value=\"\">选择井区...</option>"));
                for (var i = 0; i < result.length; i++) {
                    $("#blockGuan").append($("<option value=\"" + result[i] + "\" hassubinfo=\"true\">" +
                            result[i] + "</option>"));
                }
                $("#blockGuan").trigger("chosen:updated");
            }
        });
    } else {
        //$("#blockGuan").empty();
        //$("#blockGuan").append($("<option value=\"\">选择井区...</option>"));
        //$("#resCategory").trigger("chosen:updated");
        //$("#series").trigger("chosen:updated");
        //$("#formation").trigger("chosen:updated");
        //$("#layer").trigger("chosen:updated");
        //$("#blockGuan").trigger("chosen:updated");
    }
}

function fixCss() {
    $("#calculate-unit-tree").slimScroll({
        height: '100%',
        railOpacity: 0.9 //,
        //alwaysVisible: true
    });
    $("#layer-type-tree").slimScroll({
        height: '100%',
        railOpacity: 0.9 //,
        //alwaysVisible: true
    });
    $("#reserve-category-tree").slimScroll({
        height: '100%',
        railOpacity: 0.9 //,
        //alwaysVisible: true
    });
}

function doDefaultEvent() {
    //默认加载三级储量所有储量类别 
    $.ajax({
        url: "/BaseInfo/GetBaseInfoViewDataForCategory",
        type: "get",
        success: function (result) {
            bindResCategory(result);
        }
    });

    //默认加载三级储量 综合查询 列表信息
    $.ajax({
        url: "/BaseInfo/GetBaseInfoViewData",
        type: "post",
        success: function (result) {
            if (!result) return;
            var d = result.split('#');
            var data1 = eval('(' + d[0] + ')');
            var arr = eval('(' + d[1] + ')');
            var temp = [];
            for (var i = 0; i < arr.length; i++) {
                if (arr[i] == "id") {
                    temp.push({ "index": arr[i], "name": arr[i], hidden: true });
                } else {
                    temp.push({ "index": arr[i], "name": arr[i] });
                }
            }
            var data2 = temp;
            var data3 = eval('(' + d[2] + ')');
            var data4 = eval('(' + d[3] + ')');
            var domId = "table_search_result";
            loadSearchGrid(domId, data1, data2, data3, data4);
        }
    });
}

function loadDocument() {
    //创建右键菜单
    createContextMenu();
    //loadGdbFile(gdb_url);
    ////加载gdburl
    //getXishanyaoGdbUrl();
    //getQiguGdbUrl();
    //getCarboniferousGdbUrl();
    ////加载查询储量分布结果json
    //getQueryReservesDis();
    //空间初始化
    loadCollapse();
    fixCss();
    doDefaultEvent();
}

$(document).ready(function () {
    initTree();
    initEvent();
    loadDocument();
});

function loadCollapse() {
    $(".chosen-select").each(function () {
        $(this).chosen({
            allow_single_deselect: true,
            disable_search_threshold: 10,
            no_results_text: '未找到此选项!',
            width: '100%'
        });
    });
    $("#resCategory").append($("<option value=\"\">选择储量类别...</option>"));
    $("#series").append($("<option value=\"\">选择系...</option>"));
    $("#formation").append($("<option value=\"\">选择组...</option>"));
    $("#layer").append($("<option value=\"\">选择层...</option>"));
    $("#blockGuan").append($("<option value=\"\">选择井区...</option>"));



    $('#collapseOne').on('shown.bs.collapse', function () {
        cur_selfun = "tab1";
        $("#targetDataGrid").show();
        $("#tablist_unit").show();
        $("#geologic_age").hide();
        $("#tablist_search").hide();
        $("#targetReps").hide();
        //gdb_url = "demodata/gdb/base/sub3/准噶尔盆地2015年油气勘探部署图.GDBX";
        //loadGdbFile(gdb_url);
        fix_height();
    });

    $('#collapseTwo').on('shown.bs.collapse', function () {
        cur_selfun = "tab2";
        $("#targetDataGrid").show();
        $("#geologic_age").hide();
        $("#tablist_unit").hide();
        $("#tablist_search").hide();
        $("#targetReps").hide();
        //gdb_url = "demodata/gdb/base/sub3/准噶尔盆地2015年油气勘探部署图.GDBX";
        //loadGdbFile(gdb_url);
        fix_height();
    });

    $('#collapseThree').on('shown.bs.collapse', function () {
        cur_selfun = "tab3";
        $("#targetDataGrid").show();
        $("#tablist_unit").hide();
        $("#geologic_age").show();
        $("#tablist_search").hide();
        $("#targetReps").hide();
        //gdb_url = "demodata/gdb/base/sub3/准噶尔盆地2015年油气勘探部署图.GDBX";
        //loadGdbFile(gdb_url);
        fix_height();
    });

    $('#collapseFour').on('shown.bs.collapse', function () {
        cur_selfun = "tab4";
        $("#targetDataGrid").show();
        $("#targetReps").hide();
        $("#tablist_unit").hide();
        $("#geologic_age").hide();
        $("#tablist_search").show();

        var config = {
            '.chosen-select': {},
            '.chosen-select-deselect': {
                allow_single_deselect: true
            },
            '.chosen-select-no-single': {
                disable_search_threshold: 10
            },
            '.chosen-select-no-results': {
                no_results_text: 'Oops, nothing found!'
            },
            '.chosen-select-width': {
                width: "95%"
            }
        }
        for (var selector in config) {
            $(selector).chosen(config[selector]);
        }

        setIonRage();

        $("#btnsearch-unit").on('click', function () {
            var strYear = $("#ionrange_1").val().split(';');
            $.ajax({
                url: "/BaseInfo/GetBaseInfoViewDataByQuery",
                type: "post",
                data: { startYear: strYear[0], endYear: strYear[1], category: $("#resCategory").val(), series: $("#series").val(), formation: $("#formation").val(), layer: $("#layer").val(), blockGuan: $("#blockGuan").val() },
                success: function (result) {
                    if (!result) return;
                    var d = result.split('#');
                    var data1 = eval('(' + d[0] + ')');
                    var arr = eval('(' + d[1] + ')');
                    var temp = [];
                    for (var i = 0; i < arr.length; i++) {
                        if (arr[i] == "id") {
                            temp.push({ "index": arr[i], "name": arr[i], hidden: true });
                        } else {
                            temp.push({ "index": arr[i], "name": arr[i] });
                        }
                    }
                    var data2 = temp;
                    var data3 = eval('(' + d[2] + ')');
                    var data4 = eval('(' + d[3] + ')');
                    var domId = "table_search_result";
                    $("#" + domId).jqGrid("clearGridData");
                    $("#" + domId).jqGrid("setGridParam", { data: data4 }).trigger("reloadGrid");
                }
            });
        });
    });

    $('#collapseFive').on('shown.bs.collapse', function () {
        cur_selfun = "tab5";
        $("#targetDataGrid").hide();
        $("#tablist_unit").hide();
        $("#geologic_age").hide();
        $("#tablist_search").hide();
        $("#targetReps").show();
    });
}

var fix_height = function () {
    var heightWithoutNavbar;

    switch (cur_selfun) {
        case "tab2": {
            heightWithoutNavbar = $("body").height() - 70;
            break;
        }
        default: {
            heightWithoutNavbar = $("body").height() - 70 - (150 + 44);
        }
    }

    $("#joGIS1").css("height", heightWithoutNavbar);
}


//菜单事件
function joGIS1::CustomMenu(nID) {
    var filedir = null;
    switch (nID) {
        case 99: {//下载
            filedir = joGIS1.DoFileDialog(false, "GDBX", "Geomap图件");
            if (filedir == '') return;
            joGIS1.SaveMapFile(filedir);
            break;
        }
    }
}

/**
 * 储量成果报告*
 */

var achieveListData = [];
//分页组件参数
var _pagesize = 10;
var _count = 0;
var _items = 10;
//常量设置 针对储量的成果报告
currentBo = "储量成果报告";
currentProductType = "";
var _param = {
    "filter": {
        "$and": [            {
                "ep.producttype": {
                    "$regex": ""
                }
            },            {
                "indexeddate":{"$gte": ""}
            },
            {
                "indexeddate": { "$lte": "" }
            }        ]
    },    "pager": {
        "From": 0,        "Size": 10
    }
}
var ViewModel = function () {
    var self = this;
    self.myAchieveListData = ko.observableArray(achieveListData);

    //加载成果列表
    loadAchieveList = function (currentProductType, start,end,index) {
        _param.filter["$and"][0]["ep.producttype"]["$regex"] = currentProductType;
        _param.filter["$and"][1]["indexeddate"]["$gte"] = start;
        _param.filter["$and"][2]["indexeddate"]["$lte"] = end;
        _param.pager.From = index;
        var paramJson = JSON.stringify(_param);
        $.ajax({
            url: "/Document/GetMatchData",
            type: "get",
            data: { param: paramJson },
            success: function (resultMetaData) {
                var _resultMetaData = JSON.parse(resultMetaData);
                pager_demo.count = _resultMetaData.count;
                if (_resultMetaData.count <= _pagesize) {
                    pager_demo.show = false;
                }
                if (_resultMetaData.count > _pagesize) {
                    pager_demo.show = true;
                }
                self.myAchieveListData.removeAll();
                if (_resultMetaData.count > 0) {
                    for (var i = 0; i < _resultMetaData.metadatas.length; i++) {
                        //针对某条成果-->通过iiid拿到所有人浏览次数和以及当前用户这条成果是否收藏
                        var _iiid = _resultMetaData.metadatas[i].iiid;
                        var result = _resultMetaData.metadatas[i];
                        $.ajax({
                            url: "/GeoNav/GetSkimCountByiiid",
                            type: "post",
                            async:false,
                            data: { iiid: _iiid },
                            success: function (result2) {
                                self.myAchieveListData.push({
                                    "iiid": result.iiid,
                                    "name": result.dc.title[0].text ? result.dc.title[0].text : '',
                                    "author": result.dc.contributor ? result.dc.contributor[0].name : '无作者',
                                    "skimCount": result2.countLook,
                                    "flagFav": result2.flagFav,
                                    "producttype": result.ep.producttype ? result.ep.producttype : '',
                                    "indexDate": result.indexeddate.slice(0, 10),
                                    "createdDate": new Date().getFullYear() + "/" + (new Date().getMonth() + 1) + "/" + new Date().getDate(),
                                });
                            }
                        });
                    }
                }
            }
        });
    },

    //浏览
    self.openDetailPage = function ($parent) {
        var _iiid = $parent.iiid;
        var _name = $parent.name;
        var _author = $parent.author;
        var _producttype = $parent.producttype;
        window.open("/Render/PTDetail?iiid=" + _iiid);
        //当前用户成果浏览次数加1
        $.ajax({
            url: "/User/OpenDetailPage",
            type: "post",
            data: { "iiid": _iiid, name: _name, author: _author, producttype: _producttype },
            success: function (result) {
                console.log(result);
            },
            error: function (result) {
                console.log(result);
            }
        });
    },

    //收藏成果列表
    self.collectAchieveList = function ($parent, $index) {
        var data = $parent;
        $.ajax({
            url: "/GeoNav/Collect",
            type: "post",
            data: { iiid: data.iiid, name: data.name, author: data.author, producttype: data.producttype, indexDate: data.indexDate, createdDate: data.createdDate },
            success: function (result) {
                if (result == "CollectSuccess") {
                    toastr.success('收藏成功！', '成功提示');
                    var searchText = "";
                    var index = 0;
                    loadAchieveList(currentProductType, index);

                } else if (result == "CollectCancel") {
                    toastr.warning('取消收藏！', '成功提示');
                    var searchText = "";
                    var index = 0;
                    loadAchieveList(currentProductType, index);
                }
            }
        });
    }
    self.downloadAchieve = function ($parent) {
        var iiid = $parent.iiid;
        $.ajax({
            url: "/RECP/GetMateData",
            type: "get",
            data: { iiid: iiid },
            async: false,
            success: function (result) {
                var data = JSON.parse(result);
                var urlStr = data.source.url;
                var _apiPath = "@ViewBag._apiPath";
                var _achieveDown = "@ViewBag._achieveDown";
                Retrieve(urlStr, _apiPath, _achieveDown);
            }
        });
        // 下载记录收集到表
        downloadRecords($parent);
    }

};
var currentViewModel = new ViewModel();
ko.applyBindings(currentViewModel);

// 下载记录收集到表
function downloadRecords(parent) {
    console.log("download");
    var _iiid = parent.iiid;
    var _name = parent.name;
    var _author = parent.author;
    var _producttype = parent.producttype;
    //当前的下载次数+1
    $.ajax({
        url: "/User/DownLoadRecords",
        type: "post",
        data: { "iiid": _iiid, name: _name, author: _author, producttype: _producttype },
        success: function (result) {
            console.log(result);
        },
        error: function (result) {
            console.log(result);
        }
    });
}

//分页组件相关的js代码
var pager_demo = new Vue({
    el: '#app',
    data: function () {
        return {
            id: "_id1",
            activeItem: 1,
            pagesize: _pagesize,
            count: _count,
            items: _items,
            show: true
        }
    },
    computed: {
        _showFirstPrev: function () {
            return this.activeItem > 1;
        },
        _showNextLast: function () {
            return this.activeItem < Math.ceil(this.count / this.pagesize);
        },
        _items: function () {
            var temp = [],
                maxItem = Math.ceil(this.count / this.pagesize),
                len = maxItem > this.items ? this.items : maxItem,
                midIndex = this.items % 2 == 0 ? Math.ceil((this.items + 1) / 2) : Math.ceil(this.items / 2);
            for (let i = 1; i <= len; i++) {
                temp.push(i);
            }
            if (this.activeItem > midIndex) {
                var span = this.activeItem - midIndex;
                for (let i = 0; i < span && temp[temp.length - 1] < maxItem; i++) {
                    temp.shift(temp[i]);
                    temp.push(len + 1 + i);
                }
            }
            return temp;
        }
    },
    watch: {
        activeItem: function () {
            this.selectItem();
        }
    },
    methods: {
        onPrev: function () {
            this.activeItem = this.activeItem - 1;
        },
        onCurrent: function (item) {
            this.activeItem = item;
        },
        onNext: function () {
            this.activeItem = this.activeItem + 1;
        },
        selectItem: function () {
            debugger;
            var page = this.activeItem;
            var index = (page - 1) * 10;
            var searchText = $("#reserve-geo-list").val().replace(/^\s+|\s+$/g, "");
            var start = $("#reserve-geo-start-date").val();
            var end = $("#reserve-geo-end-date").val();
            loadAchieveList(currentProductType, currentStart, currentEnd, index);
        }
    }
});

//通过关键字搜索
$("#btnselgdb").on('click', function () {
    var currentProductType = $("#reserve-geo-list").val().replace(/^\s+|\s+$/g, "");
    var start;
    var end;
    try {
        start = new Date($("#reserve-geo-start-date").val()).toISOString();
    } catch (e) {
        start = new Date($("#reserve-geo-start-date").val().slice(0, 4)).toISOString();
        console.log(e);
    }
    try {
        end = new Date($("#reserve-geo-end-date").val()).toISOString();
    } catch (e) {
        end = new Date($("#reserve-geo-end-date").val().slice(0, 4)).toISOString();
        console.log(e);
    }
    currentStart = start;
    currentEnd = end;
    var index = 0;
    //加载成果列表通过搜索关键字  
    loadAchieveList(currentProductType, start,end,index);
});