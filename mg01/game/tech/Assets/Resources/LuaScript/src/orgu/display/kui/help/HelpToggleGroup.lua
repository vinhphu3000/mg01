--HelpToggleGroup
--@author jr.zeng
--2017年10月30日 上午11:44:18
local modname = "HelpToggleGroup"
--==================global reference=======================

--===================namespace========================
local ns = "org.kui"
local using = {--[[namespace1,...]]}
local _ENV = namespace(ns, using, modname)
--===================module property========================
local super = HelpUi	--父类
HelpToggleGroup = class(modname, super, _ENV)
local HelpToggleGroup = HelpToggleGroup


local CTypeName = CTypeName
local EvtCenter = EvtCenter
local UI_EVT = UI_EVT


local function getKToggleGroup(go)
    return HelpToggleGroup.need_component(go, CTypeName.KToggleGroup)
end

--===================module content========================

--是否需要请求改变
function HelpToggleGroup.need_req_tglgroup(go, b)

    local a = getKToggleGroup(go)
    if not a then
        return end
    a.needReqChange = b
end

--是否允许取消
function HelpToggleGroup.allow_off_tglgroup(go, b)

    local a = getKToggleGroup(go)
    if not a then
        return end
    a.allowSwitchOff = b
end


--是否多选
function HelpToggleGroup.allow_mult_tglgroup(go, b)

    local a = getKToggleGroup(go)
    if not a then
        return end
    a.allowMultiple = b
end

--获取指定单选框
--return KToggle
function HelpToggleGroup.get_tgl_tglgroup(go, index)
	local a = getKToggleGroup(go)
	if not a then
		return end
	return a:GetToggle(index-1)
end

--获取选中的序号
function HelpToggleGroup.get_sel_idx_tglgroup(go)

    local a = getKToggleGroup(go)
    if not a then
        return 0 end
    return a.index + 1   --C#从0开始
end

--某项是否选中
function HelpToggleGroup.is_idx_sel_tglgroup(go, index)
    
    local a = getKToggleGroup(go)
    if not a then
        return false end
    return a:isSelected(index+1)    --C#从0开始
end

--选中某项
--@index
--@isOn
--@ingoreInvoke_ 是否派发事件
function HelpToggleGroup.set_on_tglgroup(go, index, isOn, ingoreInvoke_)
    local a = getKToggleGroup(go)
    if not a then
        return end
    return a:Select(index, isOn, ingoreInvoke_)
end

--取消所有选中
function HelpToggleGroup.cancel_all_tglgroup(go)
    local a = getKToggleGroup(go)
    if not a then
        return end
    return a:CancelAll()
end


--//-------~★~-------~★~-------~★~事件相关~★~-------~★~-------~★~-------//

HelpToggleGroup.fnSet_gftr =
{
	attach_tglgroup_change = 1,
	attach_tglgroup_req = 1,
	--
	detach_tglgroup_change = 1,
	detach_tglgroup_req = 1,
}

--监听事件_选中/取消
function HelpToggleGroup.attach_tglgroup_change(go, fun, target, refer)
    EvtCenter:attachGoEvt(go, UI_EVT.VALUE_CHANGE, fun, target, refer)
end

function HelpToggleGroup.detach_tglgroup_change(go, fun, target)
    EvtCenter:detachGoEvt(go, UI_EVT.VALUE_CHANGE, fun, target)
end

--监听事件_请求选中/取消
function HelpToggleGroup.attach_tglgroup_req(go, fun, target, refer)
    EvtCenter:attachGoEvt(go, UI_EVT.REQ_VALUE_CHANGE, fun, target, refer)
end

function HelpToggleGroup.detach_tglgroup_req(go, fun, target)
    EvtCenter:detachGoEvt(go, UI_EVT.REQ_VALUE_CHANGE, fun, target)
end


return HelpToggleGroup