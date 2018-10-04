--HelpSlider
--@author jr.zeng
--2017年10月30日 下午3:31:53
local modname = "HelpSlider"
--==================global reference=======================

--===================namespace========================
local ns = "org.kui"
local using = {--[[namespace1,...]]}
local _ENV = namespace(ns, using, modname)
--===================module property========================
local super = HelpUi	--父类
HelpSlider = class(modname, super, _ENV)
local HelpSlider = HelpSlider

local CTypeName = CTypeName
local EvtCenter = EvtCenter
local UI_EVT = UI_EVT

local function getKSlider(go)
    return HelpSlider.need_component(go, CTypeName.KSlider)
end

--===================module content========================

--设置滑动块的值
function HelpSlider.get_slider_value(go)
    
    local a = getKSlider(go)
    if not a then
        return 0 end
    return a.value
end


--@value 0~1
function HelpSlider.set_slider_value(go, value)

    local a = getKSlider(go)
    if not a then
        return end
    a.value = value
end


--//-------~★~-------~★~-------~★~事件相关~★~-------~★~-------~★~-------//

HelpSlider.fnSet_gftr =
{
	attach_slider_change = 1,
	--
	detach_slider_change = 1,
}

--监听事件_数值改变
--@fun( go, value(0~1) )
function HelpSlider.attach_slider_change(go, fun, target, refer)
    EvtCenter:attachGoEvt(go, UI_EVT.VALUE_CHANGE, fun, target, refer)
end

function HelpSlider.detach_slider_change(go, fun, target)
    EvtCenter:detachGoEvt(go, UI_EVT.VALUE_CHANGE, fun, target)
end

return HelpSlider