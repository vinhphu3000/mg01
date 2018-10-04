--HelpToggle
--@author jr.zeng
--2017年10月28日 下午7:12:26
local modname = "HelpToggle"
--==================global reference=======================

--===================namespace========================
local ns = "org.kui"
local using = {--[[namespace1,...]]}
local _ENV = namespace(ns, using, modname)
--===================module property========================
local super = HelpUi	--父类
HelpToggle = class(modname, super, _ENV)
local HelpToggle = HelpToggle


local CTypeName = CTypeName
local EvtCenter = EvtCenter
local UI_EVT = UI_EVT

local function getKToggle(go)
    return HelpToggle.need_component(go, CTypeName.KToggle)
end


--===================module content========================

--是否需要请求改变
function HelpToggle.need_req_tgl(go, b)
    
    local a = getKToggle(go)
    if not a then
        return end
    a.needReqChange = b
end

--选中
--@ingoreInvoke_ 不派发事件
function HelpToggle.set_on_tgl(go, b, ingoreInvoke_)

    local a = getKToggle(go)
    if not a then
        return end
    a.Select(b, ingoreInvoke_)
end


--是否选中
function HelpToggle.is_sel_tgl(go)

    local a = getKToggle(go)
    if not a then
        return false end
    return a.isOn
end

--//-------~★~-------~★~-------~★~事件相关~★~-------~★~-------~★~-------//

HelpToggle.fnSet_gftr =
{
	attach_tgl_change = 1,
	attach_tgl_req = 1,
	--
	detach_tgl_change = 1,
	detach_tgl_req = 1,
}


--监听事件_选中
function HelpToggle.attach_tgl_change(go, fun, target, refer)
    EvtCenter:attachGoEvt(go, UI_EVT.VALUE_CHANGE, fun, target, refer)
end

function HelpToggle.detach_tgl_change(go, fun, target)
    EvtCenter:detachGoEvt(go, UI_EVT.VALUE_CHANGE, fun, target)
end

--监听事件_选中
function HelpToggle.attach_tgl_req(go, target, fun, refer)
    EvtCenter:attachGoEvt(go, UI_EVT.REQ_VALUE_CHANGE, fun, target, refer)
end

function HelpToggle.detach_tgl_req(go, fun, target)
    EvtCenter:detachGoEvt(go, UI_EVT.REQ_VALUE_CHANGE, fun, target)
end

return HelpToggle