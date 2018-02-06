//modifyFileName
$('.form-inline').validate({
    rules: {
        modifyFileName: { required: true}
    },
    errorshow: function (error, element) {
        error.appendTo(element.siblings('span'));
    }
})
mini.parse();
var changPassResult;
function changePass(json) {
    $.ajax({
        url: "/User/checkPass",
        type: "get",
        data: { json: json },
        async: false,
        success: function (result) {
            changPassResult = result;
        }
    });
    return changPassResult;
}

function submitChangePass() {
    var form = new mini.Form("#form1");
    form.validate();
    if (form.isValid() == false) return;
    var data = form.getData();
    var json = mini.encode(data);
    console.log(json);
    var result = changePass(json);
    if (result == "Success") {
        toastr.success('修改密码成功!', '成功提示');
    } else {
        toastr.warning('修改密码失败！', '失败提示');
    }
    $('#myModal').modal('hide')
}
//输入框的验证
function updateError(e) {
    var id = e.sender.name + "_error";
    var el = document.getElementById(id);
    if (el) {
        el.innerHTML = e.errorText;
    }
}
function onUserNameValidation(e) {
    updateError(e);
}
function onPwdValidation(e) {
    updateError(e);
}

var $parent;
var labCurTargetName = $("#labCurTargetName");
var targetTree = $("#targetTree");
var catalogTree = $("#catalogTree");
var global_cur_target;
var global_cur_catalog;
var UserBehaviorData;   //所有的数据
var UserBehavoirSearchData;   //根据Title搜索拿到的数据
var FavoriteCatalogData;   //收藏文件夹数据
var FavoriteMessByCatalogIdData;  //根据收藏文件夹Id拿到所有的收藏列表
//get UserBehaviorData _type{"favorite" "download"} _count{Number}
function getUserBehaviorMess(_type) {
    $.ajax({
        url: "/UserBehavior/GetOptionMessByUserId",
        type: "get",
        data: { type: _type },
        async: false,
        success: function (json) {
            UserBehaviorData = json;
        }
    });
}
//通过title搜索拿到用户的行为信息
function getUserBehaviorMessBySearchMess(_type, _title) {
    $.ajax({
        url: "/UserBehavior/GetOptionMessByTitle",
        type: "get",
        data: { type: _type, title: _title },
        async: false,
        success: function (json) {
            UserBehavoirSearchData = json;
        }
    });
}
//拿到所有收藏文件夹的信息
function getFavoriteCatalogMess() {
    var data;
    $.ajax({
        url: "/User/GetFavoriteCatalogMess",
        type: "get",
        async: false,
        success: function (json) {
            FavoriteCatalogData = json;
            data = json;
        }
    })
    return data;
}

//根据收藏文件夹的Id和type拿到所有相关联的收藏列表的信息
function getFavoriteMessByCatalogId(_id, _type) {
    var data;
    $.ajax({
        url: "/User/GetFavoriteMessByCatalogId",
        type: "get",
        data: { id: _id, type: _type },
        async: false,
        success: function (json) {
            FavoriteMessByCatalogIdData = json;
            data = json
        }
    })
    return data;
}

//根据收藏文件夹的Id移除某一条的收藏文件夹
function removeFavoriteCatalog(_id, _type) {
    var result;
    $.ajax({
        url: "/User/RemoveFavoriteCatalog",
        type: "get",
        data: { id: _id, type: _type },
        async: false,
        success: function (json) {
            result = json;
        }
    });
    return result;
}

function addFavoriteCatalog(json) {
    $.ajax({
        url: "/User/AddFavoriteCatalog",
        type: "get",
        data: { json: json },
        async: false,
    });
}

function modifyFavoriteCatalog(json, currentModifyId) {
    $.ajax({
        url: "/User/ModifyFavoriteCatalog",
        type: "get",
        data: { json: json, CatalogId: currentModifyId },
        async: false,
    });
}

// 正文高度
var fix_height = function () {
    var heightWithoutNavbar = $("body").height() - 40;
    //$("#joGIS1").css("height", heightWithoutNavbar);
    $("#table_list").css("height", heightWithoutNavbar);
}

var getCurTarget = function (jsonTragets, targetId) {
    //筛选
    var queryresult = Enumerable.From(jsonTragets)
        //.Where(function (i) { console.log(i.parent); return i.parent==parentid; });
        .Where("x=>x.id=='" + targetId + "'").ToArray();
    return queryresult[0];
}

var createTargetTree = function (jsonTragets) {
    //console.log(targetTree);
    targetTree.jstree({
        'core': {
            expand_selected_onload: false,
            multiple: false,
            animation: true,//动画
            data: jsonTragets
        },
        'plugins': ["dnd", "state", "types"],
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
        }
    });

    //绑定事件
    targetTree.on('select_node.jstree', function (e, data) {
        var objId = data.node.id;
        labCurTargetName.html("当前正在查阅" + global_cur_target.text + "的相关成果");
    });
}

var createCatalogTree = function (jsonCatalog) {
    catalogTree.jstree({
        'core': {
            expand_selected_onload: false,
            multiple: false,
            animation: true,//动画
            data: jsonCatalog
        },
        'plugins': ["dnd", "state", "types"],
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
        }
    });
}

function createGallery(data) {
    var container = $("#gallery");
    for (var i = 0 ; i < data.length; i++) {
        var l = "<a href=\"../../demodata/thumb/" + data[i].typename + "/" + data[i].src + "\" data-gallery=\"\"><img src=\"../../demodata/thumb-sm/" + data[i].typename + "/" + data[i].thumb + "\"></a>";
        container.prepend($(l));
    }
}

function createResult(tablelst, data) {
    var container = $(tablelst).find("tbody");
    container.empty();
    var tr, td;
    for (var i = 0 ; i < data.length; i++) {
        tr = $("<tr style=\"display:table-row;\"></tr>");
        td = $("<td><span class=\"ibx-hotTop-rank current\">" + (i + 1) + "</span></td>");
        tr.append(td);
        td = $("<td><span>" + data[i].DC_Title_Text + "</span></td>");
        tr.append(td);
        td = $("<td>" + data[i].DC_Author + "</td>");
        tr.append(td);
        td = $("<td>" + data[i].EP_ProductType + "</td>");
        tr.append(td);
        td = $("<td>" + data[i].DC_Description + "</td>");
        tr.append(td);
        td = $("<td>" + eval('new ' + eval(data[i].DC_Date_Created).source).toLocaleDateString() + "</td>");
        tr.append(td);
        td = $("<td>" + data[i].CreatedBy + "</td>");
        tr.append(td);
        td = $("<td>" + eval('new ' + eval(data[i].IndexedDate).source).toLocaleDateString() + "</td>");
        tr.append(td);
        td = $("<td class=\"text-navy\"><i class=\"fa fa-level-up\"></i> "+data[i].CountLook+"</td>");
        tr.append(td);
        td = $("<td></td>");
        td.append($("<button class=\"btn btn-info btn-circle\" onclick=\"openDetailPage('" + data[i].iiid + "')\" type=\"button\" data-toggle=\"tooltip\" data-placement=\"left\" title=\"浏览\"><i class=\"fa fa-sticky-note-o\"></i></button>"));
        td.append($("<button class=\"btn btn-danger btn-circle btn-outline\" onclick=\"collectFavoriteList('" + data[i].RecNo + "')\" type=\"button\" data-toggle=\"tooltip\" data-placement=\"left\" title=\"收藏\"><i class=\"fa fa-heart\"></i></button>"));
        td.append($("<button class=\"btn btn-success btn-circle\" type=\"button\" data-toggle=\"tooltip\" data-placement=\"left\" title=\"下载\"><i class=\"fa fa-download\"></i></button>"));
        tr.append(td);
        container.append(tr);
    }
    $(tablelst).footable();
}

var loadDocument = function () {
    //渲染浏览记录
    getUserBehaviorMess("look");
    createResult($("#table_list1"), UserBehaviorData);
    UserBehaviorData = [];
    //收藏记录
    getUserBehaviorMess("favorite");
    createResult($("#table_list2"), UserBehaviorData);
    UserBehaviorData = [];
    //下载记录
    getUserBehaviorMess("download");
    createResult($("#table_list3"), UserBehaviorData);
    UserBehaviorData = [];

    queryresult = Enumerable.From(dataGdbThumbs)
        .Where(function (i) { return i.typeid == 'kfbst' || i.typeid == 'kfjwt'; }).ToArray();
    createResult($("#table_list4"), queryresult);
}

var currentModifyId;
//文档加载
$(document).ready(function () {
    $parent = $(window.parent.document);
    $.jgrid.defaults.styleUI = 'Bootstrap';
    //修复JoGis展示区
    $(window).bind("load resize click scroll", function () {
        if (!$("body").hasClass('body-small')) {
            fix_height();
        }
    });

    var ViewModel = function () {
        var self = this;
        getFavoriteCatalogMess();
        self.FavoriteCatalog = ko.observableArray(FavoriteCatalogData);
        self.myFavorite = function () {
            siwtchtab(2);
        };
        self.addFavoriteCatalog = function () {
            $('#myModal2').modal('show');
        };
        self.removeCatalog = function ($parent) {
            var Id = $parent.FavCatalogId;

            var data = getFavoriteMessByCatalogId(Id, "favorite");
            if (data.length == 0) {
                if (confirm("您的文件夹为空，确认删除？")) {
                    removeFavoriteCatalog(Id)
                    window.location.reload();
                }
            }
            else {
                toastr.warning('文件夹不为空，不能删除！！请先删除文件夹中内容')
            }
        };
        self.editCatalog = function ($parent) {
            //$("#mini_text_modify").text("之前的文件夹名称为：" + $parent.FavName.trim());
            //$("#modifyFileName").text($parent.FavName.trim());
            $("#modifyFileName").val($parent.FavName.trim());
            currentModifyId = $parent.FavCatalogId;
            $('#myModal3').modal('show');
        };
        self.loadCatalogList = function ($parent) {
            var FavCatalogId = $parent.FavCatalogId;
            $("#myFavoriteCatalogTitle").text($parent.FavName + "收藏文件夹");
            loadFavoriteCatalog(FavCatalogId);
            FavoriteMessByCatalogIdData = [];
        }
    };
    var currentViewModel = new ViewModel();
    ko.applyBindings(currentViewModel);
    initDropdown();
});

//页面加载完毕后执行
window.onload = function () {
    try {
        //初始化目标树
        loadDocument();
    }
    catch (e) { }
    //resizeWindow();
}

function initDropdown() {
    $(".chosen-select").each(function () {
        $(this).chosen({
            allow_single_deselect: true,
            disable_search_threshold: 10,
            no_results_text: '未找到此选项!',
            width: '100%'
        });
        $(this).bind('chosen:showing_dropdown', targetSelectDataBind);
    });
}
//绑定下拉选择框的事件
function targetSelectDataBind(event) {
    var target = event.currentTarget;
    $(target).empty();
    //我需要拿到所有收藏夹的名字
    var queryresult = getFavoriteCatalogMess();
    $(target).append($("<option value=\"\">选择收藏夹...</option>"));
    for (var i = 0 ; i < queryresult.length; i++) {
        $(target).append($("<option value=\"" + queryresult[i].FavCatalogId + "\" hassubinfo=\"true\">" + queryresult[i].FavName + "</option>"));
    }
    $(target).trigger("chosen:updated");
}

//点击收藏按钮后
var idPrepareToCollect   //点击的那条收藏记录的流水号
function collectFavoriteList(RecNo) {
    idPrepareToCollect = RecNo;
    //$("#myFavoriteCatalogTitle").text($parent.FavName + "收藏文件夹");
    var result;
    $.ajax({
        url: "/UserBehavior/GetOptionMessByRecNo",
        type: "get",
        data: { RecNo: idPrepareToCollect },
        async: false,
        success: function (json) {
            result = json;
        }
    });
    if (result[0].FlagFavorite == 0) {
        $("#chooseFavCatalogTip").text("这条记录还未被收藏");
    } else if (result[0].FlagFavorite == 1 &&result[0].FavCatalogId==null) {
        $("#chooseFavCatalogTip").text("这条记录被收藏了，但没指定具体的收藏夹");

    } else if (result[0].FlagFavorite == 1 && result[0].FavCatalogId != null) {
        var result2;
        $.ajax({
            url: "/User/GetFavoriteCatalogMessByFavCatalogId",
            type: "get",
            data: { FavCatalogId: result[0].FavCatalogId },
            async: false,
            success: function (json) {
                result2 = json;
            }
        });
        $("#chooseFavCatalogTip").text("这条记录已经被收藏到："+result2[0].FavName);
    } else {
        $("#chooseFavCatalogTip").text("出现错误");
    }
    $('#myModal4').modal('show');
}

//点击收藏按钮--选择收藏文件夹--保存后
function saveFavoriteCatalogName() {
    //拿到了需要收藏文件夹的Id idPrepareToCollect--那条记录的流水号
    var _favCatalogId = $("#choose_FavoriteCatalog").val();
    var _recNo = idPrepareToCollect;
    var result;
    //用户收藏
    $.ajax({
        url: "/User/Collect",
        type: "get",
        data: { RecNo: _recNo, FavCatalogId: _favCatalogId },
        async: false,
        success: function (json) {
            result = json;
        }
    });
    if (result == "CollectSuccess") {
        toastr.success('收藏成功！', '成功提示');
    } else if (result == "HasCollected") {
        toastr.warning('这个成果已经被收藏过！', '警告提示');
    } else if (result == "ChangeFavCatalogSuccess") {
        toastr.success('文件夹修改成功！', '成功提示');
    } else {
        toastr.warning('收藏失败！', '失败提示');
    }
    $('#myModal4').modal('hide')
}

//加载文件夹里面的内容
function loadFavoriteCatalog(FavCatalogId) {
    getFavoriteMessByCatalogId(FavCatalogId, "favorite");
    siwtchtab(6);
    createResult($("#table_list6"), FavoriteMessByCatalogIdData);
}
//增加文件夹
function submitAddFavoriteCatalog() {
    var form = new mini.Form("#form2");
    form.validate();
    if (form.isValid() == false) return;
    var data = form.getData();
    var json = mini.encode(data);
    if (addFavoriteCatalog(json)) {
        toastr.success('增加文件夹成功！', '成功提示')
    }
    $('#myModal2').modal('hide')
    window.location.reload();
}
//修改文件夹
function submitModifyFavoriteCatalog() {
    //var form = new mini.Form("#form3");
    //form.validate();
    //if (form.isValid() == false) return;
    var data = form.getData();
    var json = mini.encode(data);
    if (modifyFavoriteCatalog(json, currentModifyId)) {
        toastr.success('修改文件夹成功！', '成功提示')
    }
    $('#myModal3').modal('hide')
    window.location.reload();
}

//左边树点击切换事件
function siwtchtab(curIndex) {
    var dis;
    $("#content").find(".ibox").each(function () {
        dis = ($(this).attr("tbindex") != curIndex) ? 'none' : 'block';
        $(this).attr("style", "display:" + dis);
    });
}
function openDetailPage(iiid) {
    alert(iiid);
    window.open("/Render/Detail?iiid=" + iiid);
}
