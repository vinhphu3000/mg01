--HelpText
--@author jr.zeng
--2017年10月20日 上午11:13:13
local modname = "HelpText"
--==================global reference=======================

local tostring = tostring

--===================namespace========================
local ns = "org.kui"
local using = {--[[namespace1,...]]}
local _ENV = namespace(ns, using, modname)
--===================module property========================
local super = HelpUi	--父类
HelpText = class(modname, super, _ENV)
local HelpText = HelpText


local CTypeName = CTypeName

local function getKText(go)
    return HelpText.need_component(go, CTypeName.KText)
end

--===================module content========================

function HelpText.set_text(go, str)

    local text = getKText(go)
    if not text then
        return end
    text.text = tostring(str)
end

function HelpText.get_text(go)

    local text = getKText(go)
    if not text then
        return "" end
    return text.text
end

--设置字体大小
function HelpText.set_font_size(go, size)

    local text = getKText(go)
    if not text then
        return end
    text.fontSize = size
end

function HelpText.get_font_size(go, size)

    local text = getKText(go)
    if not text then
        return 0 end
    return text.fontSize
end

--设置字体颜色
--@color Color (颜色到时统一做 TODO)
function HelpText.set_font_color( go, color )
    local text = getKText(go)
    if not text then
        return end
    text.color = color
end

--设置行距
function HelpText.set_line_spacing(go, lineSpacing)
    local text = getKText(go)
    if not text then
        return end
    text.lineSpacing = lineSpacing
end

--待定
function HelpText.get_preferred_height( go )
    local text = getKText(go)
    if not text then
        return 0 end
    return text:GetPreferredHeight()
end

--待定
function HelpText.get_preferred_width( go )
    local text = getKText(go)
    if not text then
        return 0 end
    return text:GetPreferredWidth()
end



return HelpText