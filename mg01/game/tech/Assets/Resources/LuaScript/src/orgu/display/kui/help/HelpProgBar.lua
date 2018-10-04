--HelpProgBar
--@author jr.zeng
--2017年10月30日 下午4:09:30
local modname = "HelpProgBar"
--==================global reference=======================

--===================namespace========================
local ns = "org.kui"
local using = {--[[namespace1,...]]}
local _ENV = namespace(ns, using, modname)
--===================module property========================
local super = HelpUi	--父类
HelpProgBar = class(modname, super, _ENV)
local HelpProgBar = HelpProgBar


local CTypeName = CTypeName
local EvtCenter = EvtCenter
local UI_EVT = UI_EVT

local function getKProgressBar(go)
    return HelpProgBar.need_component(go, CTypeName.KProgressBar)
end

--===================module content========================

--@value 0~1
function HelpProgBar.set_prog_value(go, value)
  local a = getKProgressBar(go)
    if not a then
        return end
    a.value = value
end

function HelpProgBar.get_prog_value(go)
    local a = getKProgressBar(go)
    if not a then
        return 0 end
    return a.value
end

--@percent 0~100
function HelpProgBar.set_prog_percent(go, percent)
    local a = getKProgressBar(go)
    if not a then
        return end
    a.percent = percent
end

function HelpProgBar.get_prog_percent(go)
    local a = getKProgressBar(go)
    if not a then
        return 0 end
    return a.percent
end


return HelpProgBar