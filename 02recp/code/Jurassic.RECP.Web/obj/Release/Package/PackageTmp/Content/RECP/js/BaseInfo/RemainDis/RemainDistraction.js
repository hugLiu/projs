var cascadeData;  //cascade data
var wellData;   //well data
var oilTestData; //oilTest data
var treeData; //tree data

var gdb_url = "Mock/RECP/Gdb/ThreeLev/ReserveCategory";//默认初始gdb图件
var base_dir = "Views/BaseInfo/";
var jogis4_options = {
    Style: { border: "solid 1px #666", width: "100%" },
    ShowEagleEye: true
};
var JoGis4;
var df_ZoomScale;
//get WellDistribution-tree data
function initTree() {
    $.ajax({
        url: "/BaseInfo/WellDisGetTreeData",
        type: "get",
        success: function (json) {
            var data = JSON.parse(json);
            data = eval("(" + data + ")");
            treeData = data;
            initUnitTree(data);
        }
    });
}
//get GeologicalAgses-cascade data
function getGeoCascadeData() {
    $.ajax({
        url: "/BaseInfo/GeologicalAgesCascadeData",
        type: "get",
        async: false,
        success: function (json) {
            var data = JSON.parse(json);
            data = eval("(" + data + ")");
            cascadeData = data;
        }
    });
}
//get search well Data
function getWellData() {
    $.ajax({
        url: "/BaseInfo/WellDisGetWellData",
        type: "get",
        async: false,
        success: function (json) {
            var data = JSON.parse(json);
            data = eval("(" + data + ")");
            wellData = data;
        }
    });
}
//get oilTestResultData
function getOilTestResultData() {
    $.ajax({
        url: "/BaseInfo/OilTestGetData",
        type: "get",
        async: false,
        success: function (json) {
            var data = JSON.parse(json);
            data = eval("(" + data + ")");
            oilTestData = data;
        }
    });
}
//get ThreeLev Gdb data
function getGdbUrl() {
    $.ajax({
        url: "/BaseInfo/ThreeLevGetGdbUrl",
        type: "get",
        async: false,
        success: function (json) {
            gdb_url = json;
        }
    });
    return gdb_url;
}
//get RemainDis Gdb data
function getRemainDisGdbUrl(gdb) {
    $.ajax({
        url: "/BaseInfo/RemainDisGetGdbUrl",
        type: "get",
        data: { gdb: gdb },
        async: false,
        success: function (json) {
            gdb_url = json;
        }
    });
    return gdb_url;
}

//bind showing_dropdown to targetSelectDataBind
function initCascadeData() {
    //所有select渲染
    $(".chosen-select").each(function () {
        $(this).chosen({
            allow_single_deselect: true,
            disable_search_threshold: 10,
            no_results_text: '未找到此选项!',
            width: '100%'
        });
        //cascade init
        if (typeof ($(this).attr("cascade")) != "undefined" && typeof ($(this).attr("target")) != "undefined") {
            console.log($(this));
            var data = { typeid: $(this).attr("typeid"), typename: $(this).attr("typename"), level: $(this).attr("level"), parentsel: $(this).attr("parentsel") };
            $(this).bind('chosen:showing_dropdown', data, targetSelectDataBind);
            //chosen: hiding_dropdown
            $(this).bind('chosen:hiding_dropdown',data, targetSeletedBind);
        }
    });
}
// 正文高度
var fix_height = function () {
    var heightWithoutNavbar = $("body").height() - 140;
    $("#joGIS1").css("height", heightWithoutNavbar);
    //$("#table_list").css("height", heightWithoutNavbar);
}

//树在超过容器的时候会自动出现滚动条
function fixCss() {
    $("#well-distribution-tree").slimScroll({
        height: '100%',
        railOpacity: 0.9
    });
}

//创建右键菜单
var createContextMenu = function () {
    //joGIS1.ResetTools();
    // 定制右键菜单测试
    joGIS1.AddTool(0); //choose
    joGIS1.AddTool(13); //放大
    joGIS1.AddTool(14); //缩小
    joGIS1.AddTool(15); //移动/手势
    joGIS1.AddTool(17); //复位
    // 定制自定义菜单测试
    joGIS1.AddCustomTool("下载", 99); //菜单开始
}

//showing_dropdown -- targetSelectDataBind
function targetSelectDataBind(event) {
    var target = event.currentTarget;
    var typeid = event.data.typeid;
    var typename = event.data.typename;
    var level = event.data.level;
    var parentsel = event.data.parentsel;
    var parent = $("select[typeid=\"" + parentsel + "\"]");
    var parentid = parent.val();

    if (level == 1) parentid = "#";
    getGeoCascadeData();
    var queryresult = Enumerable.From(cascadeData)
        .Where(function (i) { return i.parent == parentid; }).ToArray();

    $(target).empty();
    $(target).append($("<option value=\"\">选择" + typename + "...</option>"));
    for (var i = 0 ; i < queryresult.length; i++) {
        $(target).append($("<option value=\"" + queryresult[i].id + "\" hassubinfo=\"true\">" + queryresult[i].text + "</option>"));
    }
    $(target).trigger("chosen:updated");
}

function targetSeletedBind(event) {
    alert(event);
    console.log(event);
}

var initUnitTree = function (jsondata) {
    $("#well-distribution-tree").jstree({
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

//tree onclick
function initEvent() {
    $("#well-distribution-tree").on('changed.jstree', function (e, data) {
        var entity = data.node;
        var obj;
        if (treeData != null) {
            obj = getCurLayerType(treeData, data.node.id);
        } else {
            initTree();
            obj = getCurLayerType(treeData, data.node.id);
        }

        if (obj.gdb && obj.gdb != null && obj.gdb != '') {
            var gdb = obj.gdb;
            // gdb_url = obj.gdb;
            //gdb_url = getGdbUrl();
            gdb_url = getRemainDisGdbUrl(gdb);
            loadGdbFile(gdb_url);
        }
    });
}

function getCurLayerType(json, id) {
    var queryresult = Enumerable.From(json)
        //.Where(function (i) { console.log(i.parent); return i.parent==parentid; });
        .Where("x=>x.id=='" + id + "'").ToArray();
    return queryresult[0];
}

//load GDB
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

var loadWellSearchTable = function (jsondata) {
    // Configuration for jqGrid
    jQuery("#table_list_2").jqGrid({
        "data": jsondata,
        "datatype": "local",
        "colModel": [
            {
                "label": '井号',
                "name": 'JH',
                "index": 'JH',
                "width": 60,
                "editable": true
            },
            {
                "label": '井别',
                "name": 'JB',
                "index": 'JB',
                "width": 50,
                "editable": true
            },
            {
                "label": '钻遇目的层',
                "name": 'ZYMDC',
                "index": 'ZYMDC',
                "width": 50,
                "editable": true
            },
            {
                "label": '区块',
                "name": 'QK',
                "index": 'QK',
                "width": 50,
                "editable": true
            },
            {
                "label": '初测6度带X',
                "name": 'X1_6',
                "index": 'X1_6',
                "width": 100,
                "editable": true
            },
            {
                "label": '初测6度带Y',
                "name": 'Y1_6',
                "index": 'Y1_6',
                "width": 100,
                "editable": true
            }
        ],
        "viewrecords": true,
        "add": false,
        "edit": false,
        "height": 150,
        "scrollPaging": true,
        "rownumbers": true,
        "shrinkToFit": false,
        onSelectRow: function (rowid) {
            var entity = $("#table_list_2").getLocalRow(rowid);//获取行数据（原始数据）
            wellSelected($("#table_list_2"), entity);
        }
    });


    // Setup buttons
    $("#table_list_2").jqGrid('navGrid', '#pager_list_2', {
        edit: false,
        add: false,
        del: false,
        search: false
    }, {
        reloadAfterSubmit: true
    });
    //$('#table_list_2').closest(".jqGrid_wrapper").css({ 'overflow-x': 'hidden' });
    // Add responsive to jqGrid
    $(window).bind('resize', function () {
        var width = $('.jqGrid_wrapper').width();
        $('#table_list_2').setGridWidth(width);
    });
}

var wellSelected = function (table, entity) {
    $("#tbWellNo").text(entity.JH + "井");
    getOilTestResultData();
    var query_oilTest = Enumerable.From(oilTestData)
       .Where("x=>x.井号=='" + entity.JH + "'").ToArray();
    $('#table_list_3').jqGrid("clearGridData");
    $('#table_list_3').jqGrid("setGridParam", { data: query_oilTest }).trigger("reloadGrid"); //重新刷新整张表
    //var width = $('.ibox-content3').width();
    //$('#table_list_3').setGridWidth(width);

    //查找gdb
    var _zu = entity.zu;
    var _layNo = entity.layer_no ? entity.layer_no : "";
    var _layTitle = entity.layer_title ? entity.layer_title : "";
    var treeDataTable = treeData == null ? initTree() : treeData;
    var query_gdb = Enumerable.From(treeDataTable)
        .Where("x=>x.text=='" + entity.zu + "'").ToArray()[0];
    var findItem;
    if (query_gdb.gdb && query_gdb.gdb != null && query_gdb.gdb != '') {
        gdb_url = getRemainDisGdbUrl(query_gdb.gdb);
        loadGdbFile(gdb_url);
    }
    var title = [];
    title.push(_layTitle);
    var count = joGIS1.LayerCount;
    for (var i = 0; i < count; i++) {
        var name = joGIS1.GetLayerName(i);
        joGIS1.ShowLayer(name, 1);
    }
    for (var i = 0; i < count; i++) {
        var name = joGIS1.GetLayerName(i);
        if (title.indexOf(name) < 0) {
            joGIS1.ShowLayer(name, 0);
        }
    }
    var lpszID = _layTitle + "-" + _layNo;
    joGIS1.Locate(lpszID, 1); //1--放大目标使之占据整个窗口  2--缩小目标使之在窗口内 
    //3--移动目标使之在窗口中心  4--在目标上绘制一个红色的矩形框	SetVisibleRange
    //joGIS1.SetVisibleRange(0, 0, 5);
    //alert(joGIS1.GetVisibleRange());
    joGIS1.SelectElement(lpszID);
    joGIS1.FocusMode = 1;
    joGIS1.ClearAllFocusElements();
    joGIS1.AddFocusElement(String(lpszID));
}

var loadOilTestResultTable = function (jsondata) {
    $("#table_list_3").jqGrid({
        data: jsondata,
        datatype: "local",
        colNames: ['井号', '层位', '解释层号', '射孔井段', '厚度', '试油或措施', '日期', '工作制度', '原油密度', '矿化度', '氯离子', '水型', '油1', '气1', '水1', '油2', '水2', '试油结果'],
        colModel: [
            {
                name: '井号',
                index: '井号',
                width: 60,
                sortable: true,
                //resizable: false
            },
            {
                //label: '层位',
                name: '层位',
                index: '层位',
                width: 50,
                sortable: true,
            },
            {
                //label: '解释层号',
                name: '解释层号',
                index: '解释层号',
                width: 100,
                sortable: true,
            },
            {
                name: '射孔井段',
                index: '射孔井段',
                width: 100,
                sortable: true,
            },
            {
                name: '厚度',
                index: '厚度',
                width: 50,
                sortable: true,
            },
            {
                name: '试油或措施',
                index: '试油或措施',
                width: 150,
                sortable: true,
            },
            {
                name: '日期',
                index: '日期',
                width: 50,
                sortable: true,
            },
            {
                name: '工作制度',
                index: '工作制度',
                width: 100,
                sortable: true,
            },
            {
                name: '原油密度',
                index: '原油密度',
                width: 100,
                sortable: true,
            },
            {
                name: '矿化度',
                index: '矿化度',
                width: 100,
                sortable: true,
            },
            {
                name: '氯离子',
                index: '氯离子',
                width: 100,
                sortable: true,
            },
            {
                name: '水型',
                index: '水型',
                width: 50,
                sortable: true,
            },
            {
                name: '油1',
                index: '油1',
                width: 50,
                sortable: true,
            },
            {
                name: '气1',
                index: '气1',
                width: 80,
                sortable: true,
            },
            {
                name: '水1',
                index: '水1',
                width: 50,
                sortable: true,
            },
            {
                name: '油2',
                index: '油2',
                width: 50,
                sortable: false,
            },
            {
                //label: '水2',
                name: '水2',
                index: '水2',
                width: 50,
                sortable: false,
            },
            {
                //label: '试油结果',
                name: '试油结果',
                index: '试油结果',
                width: 100,
                sortable: false,
            }
        ],
        viewrecords: true,
        add: false,
        edit: false,
        height: 150,
        scrollPaging: true,
        rownumbers: true,
        shrinkToFit: false,
        rowNum: 20,
    });

    $('#table_list_3').setGroupHeaders(
           {
               useColSpanStyle: true,
               groupHeaders: [
               { "numberOfColumns": 3, "titleText": "日产量", "startColumnName": "油1" },
               { "numberOfColumns": 2, "titleText": "累产量", "startColumnName": "油2" }]
           });

    $(window).bind('resize', function () {
        var width = $('.ibox-content3').width();
        $('#table_list_3').setGridWidth(width);
    });
}

var loadCollapse = function () {
    console.log("loadCollapse");
    $('#collapseOne').on('shown.bs.collapse', function () {
        cur_selfun = "tab1";
        $("#tablist_result").hide();
        $('#table_list_2').jqGrid("clearGridData");
        //gdb_url = "demodata/gdb/base/sub3/准噶尔盆地2015年油气勘探部署图.GDBX";
        //loadGdbFile(gdb_url);

        fix_height();
    });

    $('#collapseTwo').on('shown.bs.collapse', function () {
        cur_selfun = "tab2";
        $("#tablist_result").show();

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
        var currentXi, currentZu, currentCeng, searchText;
        // JQGrid是一个在jquery基础上做的一个表格控件，以ajax的方式和服务器端通信。
        $("#btnsearch-unit").on('click', function () {

            getWellData();  //拿到井的数据

            if (cascadeData != null) {
                cascadeData = cascadeData;
            } else {
                cascadeData = getGeoCascadeData();
            }
            //拿到了所有的井数据 进行判断
            currentXi = $("#select_xi").val();  //id
            currentZu = $("#select_zu").val();
            currentCeng = $("#select_ceng").val();
            searchText = $('#input-search').val().trim();
            //var currentCeng = $("#select_ceng").val();
            var wellDataSearched = [];
            if (searchText != null && searchText != "") {
                for (var i in wellData) {
                    if (wellData[i].JH.indexOf(searchText) >= 0) {
                        wellDataSearched.push(wellData[i]);
                    }
                }
            } else {
                wellDataSearched = wellData;
            }

            var currXiText, currZuText, currCeText;
            if (currentXi == []) {
                currentXi = null;
            }
            if (currentZu == []) {
                currentZu = null;
            }
            if (currentXi != null) {
                currentXi = Enumerable.From(cascadeData).Where("x=>x.id=='" + currentXi + "'").ToArray();
                currXiText = currentXi[0].text;
            }
            if (currentZu != null) {
                currentZu = Enumerable.From(cascadeData).Where("x=>x.id=='" + currentZu + "'").ToArray();
                currZuText = currentZu[0].text;
            }
            var queryresult;
            if (currentXi != null && currentZu != null) {
                queryresult = Enumerable.From(wellDataSearched).Where("x=>x.xi=='" + currXiText + "'&&x.zu=='" + currZuText + "'").ToArray();
            } else if (currentXi != null) {
                queryresult = Enumerable.From(wellDataSearched).Where("x=>x.xi=='" + currXiText + "'&&x.JH=='" + searchText + "'").ToArray();
            } else {
                queryresult = wellDataSearched;
            }
            //queryresult = wellData;
            $('#table_list_2').jqGrid("clearGridData");
            $('#table_list_2').jqGrid("setGridParam", { data: queryresult }).trigger("reloadGrid"); //重新刷新整张表

            // var width = $('.jqGrid_wrapper').width();
            // $('#table_list_2').setGridWidth(width);
            currentXi = currentZu = currentCeng = searchText = "";
        });
    });
}


var loadDocument = function () {
    loadWellSearchTable([]);  //load WellSearch table
    loadOilTestResultTable([]); //load OilTestResult Table
    createContextMenu();
    gdb_url = getGdbUrl();
    loadGdbFile(gdb_url);  //load init gdb
    fixCss();
};

$(document).ready(function () {
    initTree();
    initEvent();
    initCascadeData();
    loadCollapse();
    loadDocument();
});