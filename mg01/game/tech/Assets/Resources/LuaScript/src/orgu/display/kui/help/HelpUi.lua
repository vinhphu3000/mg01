--helper_ui
--@author jr.zeng
--2017年10月19日 下午5:34:16
local modname = "HelpUi"
--==================global reference=======================

local csKuiUtil = mg.org.KUI.KuiUtil

--===================namespace========================
local ns = "org.kui"
local using = {--[[namespace1,...]]}
local _ENV = namespace(ns, using, modname)
--===================module property========================
local super = HelpGo	--父类
HelpUi = class(modname, super, _ENV)
local HelpUi = HelpUi

local CTypeName = CTypeName
local EvtCenter = EvtCenter


local function getRectTransform(go)
    --return go:NeedComponent_("RectTransform")
    return go.transform
end

local function getSelectable(go)
    return go:NeedComponent_(CTypeName.Selectable)
end

local function getGraphic(go)
	return go:NeedComponent_(CTypeName.Graphic)
end

--===================module content========================


--能否用户交互
function HelpUi.set_interactable(go, b)
    local a = getSelectable(go)
    if not a then
        return end
    a.interactable = b
end

--//-------~★~-------~★~-------~★~Graphic~★~-------~★~-------~★~-------//

--设置32位颜色值 0xff0000ff
function HelpUi.set_color32(go, i)
	local a = getGraphic(go)
	if not a then
		return end
	a:SetColor32_(i)
end


--设置24位颜色值 0xff0000
function HelpUi.set_color24(go, i)
	local a = getGraphic(go)
	if not a then
		return end
	a:SetColor24_(i)
end



--//-------~★~-------~★~-------~★~RectTransform~★~-------~★~-------~★~-------//

--获取RectTransform
function HelpUi.get_rectTrans(go)
	return go.transform
end


--设置锚定坐标
function HelpUi.set_achPos(go_, x, y)
	go_:SetAchPos_(x, y)
end

--获取锚定坐标
--return x,y
function HelpUi.get_achPos(go_)
	return go_:GetAchPos_()
end

--轴点
function HelpUi.set_pivot(go_, x, y)
	go_:SetPivot_(x, y)
end

function HelpUi.set_pivot_smart(go, x, y)
	go:SetPivotSmart_(x,y)
end

--return x,y
function HelpUi.get_pivot(go_)
	return go_:GetPivot_()
end



--设置size
function HelpUi.set_sizeDelta(go_, x, y)
	go_:SetSizeDelta_(x, y)
end

--获取size
--return x,y
function HelpUi.get_sizeDelta(go_)
	return go_:GetSizeDelta_()
end


function HelpUi.get_rect(go_)
	return go_:GetRect_()
end

function HelpUi.get_rectSize(go_)
	return go_:GetRectSize_()
end


--//-------~★~-------~★~-------~★~事件相关~★~-------~★~-------~★~-------//

--移除所有监听
function HelpUi.detachAllGoEvt(go_)

	EvtCenter:detachAllGoEvt(go_)
end


return HelpUi