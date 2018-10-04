--HelpBtn
--@author jr.zeng
--2017年10月18日 上午10:16:21
local modname = "HelpBtn"
--==================global reference=======================

--===================namespace========================
local ns = "org.kui"
local using = {--[[namespace1,...]]}
local _ENV = namespace(ns, using, modname)
--===================module property========================
local super = HelpUi	--父类
HelpBtn = class(modname, super, _ENV)
local HelpBtn = HelpBtn

local CTypeName = CTypeName
local EvtCenter = EvtCenter
local UI_EVT = UI_EVT

local function getKButton(go)
    return HelpBtn.need_component(go, CTypeName.KButton)
end

local function getKButtonShk(go)
    return HelpBtn.need_component(go, CTypeName.KButtonShrinkable)
end

--===================module content========================

function HelpBtn.get_pressure(go)
    
    local a = getKButton(go)
    if not a then
        return 0 end
     return a.pressure
end

function HelpBtn.get_maximumPossiblePressure(go)

    local a = getKButton(go)
    if not a then
        return 0 end
    return a.maximumPossiblePressure
end

--是否需要缩放效果
function HelpBtn.setShrinkEnabled(go, b)
    local a = getKButtonShk(go)
    if not a then
        return end
    a:ShrinkEnabled(b)
end

--//-------~★~-------~★~-------~★~事件相关~★~-------~★~-------~★~-------//
--需以attach和detach为前缀

HelpBtn.fnSet_gftr =
{
	attach_click = 1,
	attach_pointer_down =1,
	attach_pointer_up = 1,
	attach_pointer_enter = 1,
	attach_pointer_exit = 1,
	--
	detach_click = 1,
	detach_pointer_down = 1,
	detach_pointer_up = 1,
	detach_pointer_enter = 1,
	detach_pointer_exit = 1,
}

--监听事件_点击
function HelpBtn.attach_click(go, fun, target, refer)
    EvtCenter:attachGoEvt(go, UI_EVT.POINTER_CLICK, fun, target, refer)
end

function HelpBtn.detach_click(go, fun, target)
    EvtCenter:detachGoEvt(go, UI_EVT.POINTER_CLICK, fun, target)
end

--监听事件_鼠标按下
function HelpBtn.attach_pointer_down(go, fun, target, refer)
    EvtCenter:attachGoEvt(go, UI_EVT.POINTER_DOWN, fun, target, refer)
end

function HelpBtn.detach_pointer_down(go, fun, target)
    EvtCenter:detachGoEvt(go, UI_EVT.POINTER_DOWN, fun, target)
end

--监听事件_鼠标弹起
function HelpBtn.attach_pointer_up(go, fun, target, refer)
    EvtCenter:attachGoEvt(go, UI_EVT.POINTER_UP, fun, target, refer)
end

function HelpBtn.detach_pointer_up(go, fun, target)
    EvtCenter:detachGoEvt(go, UI_EVT.POINTER_UP, fun, target)
end

--监听事件_鼠标进入
function HelpBtn.attach_pointer_enter(go, fun, target, refer)
    EvtCenter:attachGoEvt(go, UI_EVT.POINTER_ENTER, fun, target, refer)
end

function HelpBtn.detach_pointer_enter(go, fun, target)
    EvtCenter:detachGoEvt(go, UI_EVT.POINTER_ENTER, fun, target)
end

--监听事件_鼠标离开
function HelpBtn.attach_pointer_exit(go, fun, target, refer)
    EvtCenter:attachGoEvt(go, UI_EVT.POINTER_EXIT, fun, target, refer)
end

function HelpBtn.detach_pointer_exit(go, fun, target)
    EvtCenter:detachGoEvt(go, UI_EVT.POINTER_EXIT, fun, target)
end


return HelpBtn