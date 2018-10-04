--HelpScrollView
--@author jr.zeng
--2017年10月25日 下午7:59:14
local modname = "HelpScrollView"
--==================global reference=======================

--===================namespace========================
local ns = "org.kui"
local using = {--[[namespace1,...]]}
local _ENV = namespace(ns, using, modname)
--===================module property========================
local super = HelpUi	--父类
HelpScrollView= class(modname, super, _ENV)
local HelpScrollView = HelpScrollView

local CTypeName = CTypeName
local EvtCenter = EvtCenter
local UI_EVT = UI_EVT

local function getKScrollView(go)
    return HelpScrollView.need_component(go, CTypeName.KScrollView)
end


--===================module content========================

--滚动方向
function HelpScrollView.get_scroll_dir(go)
    
    local a = getKScrollView(go)
    if not a then
        return 0 end 
    return a.direction
end

function HelpScrollView.set_scroll_dir(go, scrollDir)

    local a = getKScrollView(go)
    if not a then
        return end 
    a.direction = scrollDir
end


--content的transform
function HelpScrollView.get_content_trans(go)
    
    local a = getKScrollView(go)
    if not a then
        return nil end 
    return a.contentTrans
end

--mask的transform
function HelpScrollView.get_mask_trans(go)

    local a = getKScrollView(go)
    if not a then
        return nil end 
    return a.maskTrans
end

--设置能否滚动
function HelpScrollView.set_scrollable(go, b)
    local a = getKScrollView(go)
    if not a then
        return end 
    a:SetScrollable(b)
end

--当前的内容size是否需要滚动
function HelpScrollView.need_scroll(go)
    local a = getKScrollView(go)
    if not a then
        return false end 
    return a:NeedScroll()
end

--重置content的位置
function HelpScrollView.reset_content_position(go)

    local a = getKScrollView(go)
    if not a then
        return end 
    return a:ResetContentPosition()
end


--停止弹性滑动
function HelpScrollView.stop_movement(go)

    local a = getKScrollView(go)
    if not a then
        return end 
    return a:StopMovement()
end

--跳到顶部
function HelpScrollView.jump_to_top(go)

    local a = getKScrollView(go)
    if not a then
        return end 
    return a:JumpToTop()
end


--跳到底部
function HelpScrollView.jump_to_bottom(go)

    local a = getKScrollView(go)
    if not a then
        return end 
    return a:JumpToBottom()
end

--//-------~★~-------~★~-------~★~事件相关~★~-------~★~-------~★~-------//

HelpScrollView.fnSet_gftr =
{
	attach_scroll_change = 1,
	--
	detach_scroll_change = 1,
}


--监听事件_滚动位置改变
function HelpScrollView.attach_scroll_change(go, fun, target, refer)
    EvtCenter:attachGoEvt(go, UI_EVT.VALUE_CHANGE, fun, target, refer)
end

function HelpScrollView.detach_scroll_change(go, fun, target)
    EvtCenter:detachGoEvt(go, UI_EVT.VALUE_CHANGE, fun, target)
end


return HelpScrollView