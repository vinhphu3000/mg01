--UIPopMgr
--@author jr.zeng
--2017年10月12日 下午5:13:10
local modname = "UIPopMgr"
--==================global reference=======================

local Object = Unity.Object
local GameObject = Unity.GameObject

--===================namespace========================
local ns = "org.kui"
local using = {--[[namespace1,...]]}
local _ENV = namespace(ns, using, modname)
--===================module property========================
local super = PopMgr	--父类
UIPopMgr = class(modname, super)
local UIPopMgr = UIPopMgr

local Array = Array

--===================module content========================

function UIPopMgr:__ctor()
    
    self.m_canvasNumMax = 20    --画布渲染上限
    self.m_canvasDisStart = 5   --画布镜距初始值
    self.m_canvasDisPer = 30    --窗口间距
    self.m_canvasDisMax = 15    --最大渲染距离
	self.m_orderAddPer = 50     --order间距
    
    self.m_canvasDisMax = self.m_canvasDisStart + self.m_canvasNumMax * self.m_canvasDisPer + 5
    
    self.m_uiLayer = GameObject.Find("UILayerLua")
    local cameraGo = GameObject.Find("UILayerLua/UICamera")
    self.m_uiCamera = cameraGo:GetComponent("Camera")
    self.m_uiCamera.farClipPlane = self.m_canvasDisMax
    
    nslog.print_t("self.m_uiCamera", self.m_uiCamera)
    
    self.m_sortTimeId = 0
    
    self.m_trash = {}
    
end

function UIPopMgr:__setup(...)

    super.__setup(self, ...)

end

function UIPopMgr:__clear()
    
    super.__clear(self)

    self.m_trash = {}
end

function UIPopMgr:setup_event()


end

function UIPopMgr:clear_event()
    
     if self.m_sortTimeId > 0 then
        self.m_sortTimeId = clearTimeOut(self.m_sortTimeId)
     end
    
end

function UIPopMgr:getUILayer()
    return self.m_uiLayer
end

function UIPopMgr:getUICamera()
    return self.m_uiCamera
end

--//-------~★~-------~★~-------~★~数据管理~★~-------~★~-------~★~-------//

function UIPopMgr:__destroyPop(pop_)

    pop_:destroy()
    self:__addToTrash(pop_)
end

function UIPopMgr:__removeFromStack(pop_)

    super.__removeFromStack(self, pop_)
    self:__removeFromTrash(pop_)
end

--//-------~★~-------~★~-------~★~动画相关~★~-------~★~-------~★~-------//

--弹出动画
function UIPopMgr:__aniPop(pop_)
    
end

--关闭动画
function UIPopMgr:__aniClose(pop_)

    self:__excuteClose(pop_)
end

--//-------~★~-------~★~-------~★~层级管理~★~-------~★~-------~★~-------//

function UIPopMgr:__addToLayer(pop_)
    
    pop_.popTime = DateUtil.time_from_start()
    --nslog.debug("popTime", pop_.popTime, pop_.pop_id)
    self:__removeFromTrash(pop_)
    
    if self.m_sortTimeId == 0 then
        self.m_sortTimeId = setTimeOut(nil, self.__sortAllPop, self)
    end

end

--窗口排序
function UIPopMgr:__sortAllPop()
    
    local pop_arr = self.m_openArr:to_table()
    Array.sortOn(pop_arr, {"layerId", "popTime"}, Array.DESCENDING) --排序
    --nslog.debug(t2str(pop_arr, 3))
    --nslog.print_r("pop_arr", pop_arr)
    
    local pop
    local dis
    local len = #pop_arr
    for i=1, len do
        dis = self.m_canvasDisStart + self.m_canvasDisPer * (i-1)   --越大越远
        
        pop = pop_arr[i]
        pop:setSortingLayer(pop.layerId)
        pop:setSortingOrder( (len - i + 1) * self.m_orderAddPer )    --越大越近
        pop:setPlaneDistance(dis)
    end
    
    self.m_sortTimeId = 0
end


--//-------~★~-------~★~-------~★~垃圾桶~★~-------~★~-------~★~-------//

function UIPopMgr:__addToTrash(pop_)
    
    if pop_.trashSlot > 0 then
        return end
    
    local slot
    for k,v in pairs(self.m_trash) do
        if not v then
            slot = k    --找到空槽位
            break
        end
    end
    
    if not slot then
        slot = #self.m_trash + 1
    end
    
    self.m_trash[slot] = true
    pop_.trashSlot = slot
    --nslog.debug("__addToTrash", slot)
    
    local dis = self.m_canvasDisMax + self.m_canvasDisPer * slot   --越大越远
	pop_:setSortingLayer(POP_LAYER_ID.LAYER_NONE)   --重置layer，然后会阻挡鼠标事件
	pop_:setSortingOrder(0)    --不置0会阻挡鼠标事件
    pop_:setPlaneDistance(dis)
end

function UIPopMgr:__removeFromTrash(pop_)

    if pop_.trashSlot <= 0 then
        return end
    
    local slot = pop_.trashSlot
    pop_.trashSlot = 0
    self.m_trash[slot] = false
    --nslog.debug("__removeFromTrash", slot)
end

return UIPopMgr