var data_target = [
          { "id": 38, "parentId": "#", "text": "盆地", "value": "盆地_value", "state": { "selected": false, "opened": true } },
          { "id": 39, "parentId": 38, "text": "陆相沉积盆地", "value": "陆相沉积盆地_value", "state": { "selected": false, "opened": false } },
          { "id": 40, "parentId": 38, "text": "海相沉积盆地", "value": "海相沉积盆地_value", "state": { "selected": true, "opened": true } },
          { "id": 41, "parentId": 42, "text": "盆地/一级构造单元", "value": "value", "state": { "selected": false, "opened": true } },
          { "id": 42, "parentId": 39, "text": "隆起", "value": "value", "state": { "selected": false, "opened": true } },
          { "id": 44, "parentId": null, "text": "坳陷", "value": "value", "state": { "selected": false, "opened": true } },
          { "id": 45, "parentId": 38, "text": "地堑", "value": "value", "state": { "selected": false, "opened": true } },
          { "id": 53, "parentId": "#", "text": "盆地/亚一级构造单元", "value": "value", "state": { "selected": false, "opened": false } },
          { "id": 55, "parentId": 53, "text": "凸起", "value": "value", "state": { "selected": false, "opened": true } },
          { "id": 63, "parentId": "#", "text": "阶地", "value": "value", "state": { "selected": false, "opened": true } }
];;

function add_target() {

}
function update_target() {

}
function delete_target() {

}
//  data4 = [
//	{ id: '3DSeismics', text: '三维地震工区z', parentId: '#', state: { selected: true } },
//	{ id: 'ReserveParams-Oil', text: '储量参数-石油', parentId: '#' },
//	{ id: 'ReserveParams-Gas', text: '储量参数-天然气', parentId: '#' },
//	{ id: 'WellPoint', text: '井点井位数据', parentId: '#' },
//	{ id: 'WellTestResult', text: '井点试油成果数据', parentId: '#' },
//];

var $parent;
var targetTree = $( '#jtree_target' );  //目标数
var metaTree = $( '#jtree_meta' );  //元数据定义树

/*
专业元数据实体类型枚举
1、3DSeismics 三维地震工区
2、ReserveParams-Oil 储量参数 - 石油
3、ReserveParams-Gas 储量参数 - 天然气
4、WellPoint 井点井位数据
5、WellTestResult 井点试油成果数据
 */
var jsonTragets = [
	{id:'3DSeismics', text:'三维地震工区', parentId: '#', state: {selected: true}},
	{id:'ReserveParams-Oil', text:'储量参数-石油', parentId: '#'},
	{id:'ReserveParams-Gas', text:'储量参数-天然气', parentId: '#'},
	{id:'WellPoint', text:'井点井位数据', parentId: '#'},
	{id:'WellTestResult', text:'井点试油成果数据', parentId: '#'},
];
sdync()

/*
专业元数据结构定义
<ProData>
	<3DSeismics BlockId="101">
		<A caption="编号" units="" translate="" cell="A">101</A>
		<B caption="区块名称" units="" translate="" cell="B">沙丘5井南</B>
		<C caption="满覆盖面元" units="m×m" translate="" cell="C">25x20</C>
		<D caption="覆盖次数" units="" translate="" cell="D">7x20</D>
		<E caption="满覆盖面积" units="km&lt;sup&gt;2&lt;/sup&gt;" translate="" cell="E">183.12</E>
		<F caption="采集年份" units="" translate="" cell="F">1998</F>
		<G caption="处理次数" units="" translate="" cell="G">1</G>
	</3DSeismics>
</ProData>
 */
//var jsonMetaTest = [
	//{id:'01', text:'ProData', parent: '#', state: { opened : true }},
	//{id:'0101', text:'3DSeismics', parent: '01', state: { opened : true }},
	//{id:'010101', text:'A', parent: '0101', state: { opened : true, selected: true }},
	//{id:'010102', text:'B', parent: '0101', state: { opened : true }},
//	{id:'010103', text:'C', parent: '0101', state: { opened : true }},
//	{id:'010104', text:'D', parent: '0101', state: { opened : true }},
//	{id:'010105', text:'E', parent: '0101', state: { opened : true }},
//	{id:'010106', text:'F', parent: '0101', state: { opened : true }},
//	{id:'010107', text:'G', parent: '0101', state: { opened : true }}
//];

var jsonMetaTest = [
    {id:'01', text:'ProData', parent: '#', state: { opened : true }},
	{id:'0101', text:'3DSeismics', parent: '01', state: { opened : true }},
	{id:'010101', text:'A', parent: '0101'},
	{id:'010102', text:'B', parent: '0101'}
]
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
function GetProMetaDefineByMetaEntity(metaEntity) {
    var data;
    $.ajax({
        url: "/ProMetadata/GetProMetaDefineByMetaEntity",
        type: "get",
        data: { metaEntity: metaEntity },
        async: false,
        success: function (json) {
            data = json;
        }
    });
    return data;
}

// 正文高度
var fix_height = function() {
    var heightWithoutNavbar = $("body").height();
    $("#panel-details").css("height", heightWithoutNavbar - 80);
    $("#div_objectparams").css("height", $("#panel-details").height() - 390);
    $("#div_target").css("height", heightWithoutNavbar - 80);
}

var getCurTarget = function (jsonTragets, targetId) {
	//筛选
    var queryresult = Enumerable.From(jsonTragets)
        //.Where(function (i) { console.log(i.parent); return i.parent==parentid; });
        .Where("x=>x.id=='"+targetId+"'").ToArray();
    return queryresult[0];
}

//创建图层树
var initTargetTree = function (jsondata) {
	targetTree.jstree({
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
			"animation" : true,//动画
			"data" : jsondata
		}
	});
	//绑定事件
	targetTree.on('changed.jstree', targetSelected);
}

var targetSelected = function (e, data) {
    var metaEntity = data.node.id ?data.node.id :'';
    var result = GetProMetaDefineByMetaEntity(metaEntity);
    var metaTreeData = [];
    
    for (var i = 0; i < result.length; i++) {
        if (result[i].FieldCode == "Reserve" || result[i].FieldCode == "ProData") {
            metaTreeData.push({
                "id": result[i].FieldNo,
                "text": result[i].FieldCode,
                "parent": result[i].ParentNo == null ? "#" : result[i].ParentNo,
                "state": { opened: true }
            });
        } else {
            metaTreeData.push({
                "id": result[i].FieldNo,
                "text": result[i].FieldCode,
                "parent": result[i].ParentNo == null ? "#" : result[i].ParentNo
            });
        }
        
    }
    initMetaTree(metaTreeData);
	//var json = targetTree.jstree(true).get_json();

	//var targetId = data.node.id;
	//var target = getCurTarget(jsonTragets, targetId);
	//alert(target.gdb);

	//if (target.action && target.action != undefined) {
	//}
}

//加载图件的图层
var loadTargets = function (jsondata) {
	//console.log(jsondata);
	targetTree.jstree(true).settings.core.data = jsondata;
	//刷新数据
	targetTree.jstree(true).refresh();
}

//创建图层树
var initMetaTree = function (jsondata) {
	metaTree.jstree({
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
			"animation" : true,//动画
			"data" : jsondata
		}
	});

	//绑定事件
	metaTree.on('changed.jstree', function(){});
}
//var data_target;

//文档加载
var loadDocument = function () {
    //initTargetTree(jsonTragets);
    //data_target = jsonTragets;
    soUIDev.setDataById("sotree_target", "data_target", jsonTragets);
	//initMetaTree(jsonMetaTest);

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

	var editor_objectparams = CodeMirror.fromTextArea(document.getElementById("objectparams"), {
		lineNumbers: true,
		matchBrackets: true,
		styleActiveLine: true
	});

	/* Init DataTables */
   	var oTable = $('#editable').dataTable({
		bPaginate: false, //翻页功能
		bLengthChange: false, //改变每页显示数据数量
		bFilter: false, //过滤功能
		bSort: true, //排序功能
		bInfo: false,//页脚信息
		bStateSave: true,
		bAutoWidth: true//自动宽度
	});
	/* Apply the jEditable handlers to the table */
	oTable.$('td').editable('../example_ajax.php', {
		"callback": function (sValue, y) {
			var aPos = oTable.fnGetPosition(this);
			oTable.fnUpdate(sValue, aPos[0], aPos[1]);
		},
		"submitdata": function (value, settings) {
			return {
				"row_id": this.parentNode.getAttribute('id'),
				"column": oTable.fnGetPosition(this)[2]
			};
		},

		"width": "90%",
		"height": "100%"
	});
};

function fnClickAddRow() {
	$('#editable').dataTable().fnAddData(['请输入别名']);

}

//文档加载
$(document).ready(function () {
	$parent = window.parent;
	//屏蔽鼠标滚轮
	//document.onmousewheel = function() {return false;}

	//修复JoGis展示区
	$(window).bind("load resize click scroll", function () {
        if (!$("body").hasClass('body-small')) {
            fix_height();
        }
    });

    $(function () {
        targetTree.slimScroll({
	        height: '100%',
	        railOpacity: 0.9//,
            //alwaysVisible: true
	    });

		
        $("#div_objectparams").slimScroll({
	        height: '100%',
	        railOpacity: 0.9//,
            //alwaysVisible: true
	    });
    });

    //修复菜单被Ocx遮挡
    var menuitem = $(".dropdown-menu > li");
    fixOcxMask(menuitem);
});

//页面加载完毕后执行
window.onload = function(){
	//初始化目标树
    loadDocument();
}