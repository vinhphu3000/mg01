--UIPop
--@author jr.zeng
--2017年10月12日 下午5:10:21
local modname = "UIPop"
--==================global reference=======================

--===================namespace========================
local ns = "org.kui"
local using = {--[[namespace1,...]]}
local _ENV = namespace(ns, using, modname)
--===================module property========================
local super = UIAbs	--父类
_ENV[modname] = class(modname, super)
local UIPop = _ENV[modname]

--===================module content========================

--窗口id
UIPop.pop_id = 0
--生命周期
UIPop.lifeType = POP_LIFE.STACK
--层级
UIPop.layerId = POP_LAYER_ID.LAYER_POP_1

function UIPop:__ctor()
    
    --nslog.debug("__ctor step2")
    
    self.m_canvas = false
    self.m_planeDistance = 999
    self.m_sortingLayer = 0
    self.m_sortingOrder = 0
    
    --打开的时间戳
    self.popTime = 0
    self.trashSlot = 0  --垃圾桶里的槽位
    
    self:showGameObject()
end

function UIPop:show(showObj, ...)

    self:__show(showObj, ...)
    self:setup_event()
    
    self:pop()
    
    self.m_is_open = true    --开启时访问为false, 用以区分首次打开 ,因此不支持在show的时候close
end


function UIPop:pop()
    
    App.popMgr:pop(self)
    
end

function UIPop:close()

    App.popMgr:close(self.pop_id)
end

--//-------~★~-------~★~-------~★~Canvas~★~-------~★~-------~★~-------//

function UIPop:setPlaneDistance(value)
    
    self.m_planeDistance = value
    if self.m_canvas then
        self.m_canvas.planeDistance = self.m_planeDistance
    end
end

function UIPop:setSortingLayer(value)

    self.m_sortingLayer = value
    if self.m_canvas then
        self.m_canvas.sortingLayerName = tostring(self.m_sortingLayer)
    end
end

function UIPop:setSortingOrder(value)

    self.m_sortingOrder = value
    if self.m_canvas then
        self.m_canvas.sortingOrder = self.m_sortingOrder
    end
end

--//-------~★~-------~★~-------~★~GameObject~★~-------~★~-------~★~-------//


function UIPop:showGameObject()
    
    local url = PopMgr.getPrefabUrl(self.pop_id)
    super.showGameObject(self, url)
end

function UIPop:__onShowGameObject()
    
    super.__onShowGameObject(self)

    local PopMgr = App.popMgr
    
    DisplayUtil.addChild(PopMgr:getUILayer(), self.m_gameObject)
    self.m_gameObject:SetActive(true)
	self.m_gameObject.layer = CAMERA_LAYER.UI

    self.m_canvas = self.m_gameObject:GetComponent("Canvas")
    self.m_canvas.worldCamera = PopMgr:getUICamera()
    self.m_canvas.sortingLayerName = tostring(self.m_sortingLayer)
    self.m_canvas.sortingOrder = self.m_sortingOrder
    self.m_canvas.planeDistance = self.m_planeDistance
    
end

function UIPop:__onClearGameObject()

    super.__onClearGameObject(self)
    
    self.m_canvas = nil

end


return UIPop