--HelpListView
--@author jr.zeng
--2017年10月26日 上午11:41:03
local modname = "HelpListView"
--==================global reference=======================

--===================namespace========================
local ns = "org.kui"
local using = {--[[namespace1,...]]}
local _ENV = namespace(ns, using, modname)
--===================module property========================
local super = HelpUi	--父类
HelpListView = class(modname, super, _ENV)
local HelpListView = HelpListView

local CType = CType
local EvtCenter = EvtCenter
local UI_EVT = UI_EVT
local LayoutParam = LayoutParam


local function getKListView(go)
    return HelpListView.need_component(go, CType.KListView)
end


local function getKListViewScroll(go)
    return HelpListView.need_component(go, CType.KListViewScroll)
end


--===================module content========================

--形参以(go, refer)开头的函数
HelpListView.fnSet_gr =
{
	show_list =1,
}


--设置布局参数
function HelpListView.set_layout_param(go, layoutParam_)

	local a = getKListView(go)
	if not a then
		return end

	local lp = LayoutParam(layoutParam_)

	a:InitLayoutParam(
		lp.dir,
		lp.divNum,
		lp.itemSize.w, lp.itemSize.h,
		lp.itemGap.x, lp.itemGap.y,
		lp.origin.x, lp.origin.y,
		lp.pivot.x, lp.pivot.y,
		lp.padding.left, lp.padding.top, lp.padding.right, lp.padding.bottom
	)

end

--显示列表
--@params {
--  data_list       数据列表
--  on_item_data    数据回调
--  jump_idx        跳转到序号
--  jump_type       跳转类型
--}
function HelpListView.show_list(go, target_, data_list, on_item_data, jumpIdx, jumpType)

	local a = getKListView(go)
	if not a then
		return end

	--local data_list = param.data_list
	--local on_item_data = param.on_item_data
	--local jumpIdx = param.jump_idx and param.jump_idx-1 or -1   --cs从0开始
	--local jumpType = param.jump_type or JumpPosType.CENTER

	jumpIdx = jumpIdx and jumpIdx-1 or -1
	jumpType = jumpType or JumpPosType.TOP

	local function onItemData(target, go, item, index)

		on_item_data(target, item, index, data_list[index])
	end

	EvtCenter:attachGoEvt(go, UI_EVT.LIST_ITEM_DATA, onItemData, target_)

	local len = #data_list
	a:ShowLen(len, jumpIdx, jumpType)
end

	--清除列表
function HelpListView.clear_list(go)

    local a = getKListView(go)
    if not a then
        return end

    a:ClearList()
    --清除列表时,清除监听
    EvtCenter:detachGoEvtByType(go, UI_EVT.LIST_ITEM_DATA)
end

--根据序号获取项go
function HelpListView.get_item_by_index(go, index)

    local a = getKListView(go)
    if not a then
        return nil end
    return a:GetItemByIndex(index)
end


--跳到指定序号
function HelpListView.jump_to_index(go, index_, posType_)

    local a = getKListViewScroll(go)
    if not a then
        return end 
    return a:JumpToIndex(index_, posType_)
end


--//-------~★~-------~★~-------~★~事件相关~★~-------~★~-------~★~-------//


return HelpListView