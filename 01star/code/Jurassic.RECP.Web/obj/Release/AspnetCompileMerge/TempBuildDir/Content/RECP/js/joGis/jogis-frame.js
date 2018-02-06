/*
 * JoGis封装组件
 * @param {[dom对象]} el      [容器对象]
 * @param {[json对象]} options [属性参数]
 */
function JoGis(_el, _options)
{
	this.el = _el;
	this.options = {
		Style: { border: "solid 1px #666", width:"100%" },
		ShowEagleEye : true,
		GDBPath : ""
	};

	if (_options && _options!=null)
		this.options = _options;

	this.init();
}

//JoGis初始化
JoGis.prototype.init = function() {
	//console.log(this);
}

/**
 * 获取图件图层Json数组
 * @return {[type]} [description]
 * 格式 [{index:0, name:""}, {index:0, name:""}]
 * index : 图层索引
 * name : 图层名称
 */
JoGis.prototype.getGeoLayersJson = function() {
	if (!this.el.LayerCount) return [];
	if (this.el.LayerCount == 0) return [];

	var json=[];// 定义一个json对象
	var entity={};// 实体

	for (var i = 0; i < this.el.LayerCount; i++)
    {
    	entity.index = i;
    	entity.name = this.el.GetLayerName(i);
 		json.push(entity);
 		entity={};
    }
    return json;
}

/**
 * 加载图件
 * @return {[string]}      [result code]
 *
 * result code
 * jogis000 : file参数为空
 * jogis001 : 加载成功
 * jogis002 : gdb路径无效
 * jogis003 : LoadMapFile函数调用错误
 */
JoGis.prototype.loadGeoMapFile = function () {
	if (this.options.GDBPath.length == 0)
		return "jogis000";

	//加载图件
	var ret = 0;
	try
	{
		ret = this.el.LoadMapFile(this.options.GDBPath);

		//设置是否可移动曲线类图元
		this.el.MoveStatus = false;
		//设置默认操作状态为选择
		this.el.OperateState = 13;
		//设置图元单选模式
		this.el.SelectMode = 0;
		//设置聚焦闪烁模式（0：不激活；1：激活）
		this.el.FocusMode = 1;
		//设置选中图元的突出显示方式  会导致浏览器崩溃
		//this.el.SetFocusStyle(3, 255, 0, 1);
	}
	catch(e)
	{
		alert("当前浏览器不是 IE9+ 版本！无法使用ActiveX组件。请用IE9、10、11打开。");
	}
	if (ret === 0) return "jogis003";
	return "jogis001";
}