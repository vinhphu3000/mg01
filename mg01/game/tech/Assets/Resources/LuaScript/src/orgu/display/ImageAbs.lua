--ImageAbs
--@author jr.zeng
--2017年9月30日 下午5:42:56
local modname = "ImageAbs"
--==================global reference=======================

--===================namespace========================
local ns = "org"
local using = {--[[namespace1,...]]}
local _ENV = namespace(ns, using, modname)
--===================module property========================
local super = Ref	--父类
_ENV[modname] = class(modname, super)
local ImageAbs = _ENV[modname]

local Refer = Refer
local App = App
local safe_call = safe_call

--===================module content========================

function ImageAbs:__dispose0()
    self:destroy()
    self:__clearGameObject()
end


function ImageAbs:__dispose()
	--子类重写
end

function ImageAbs:__ctor()

	--nslog.debug("__ctor step0")

    self.m_is_open = false
    
    self.m_gameObject = false
    self.m_goIsExternal = false --go是否外部托管

    self:autoRelease()
end


--显示
function ImageAbs:show(showObj, ...)

    self:__show(showObj, ...)
    self:setup_event()
    self.m_is_open = true
end

function ImageAbs:__show(showObj, ...)

end

--移除显示
function ImageAbs:removeChildThis()

end


function ImageAbs:setup_event()

end

function ImageAbs:clear_event()

end

--是否打开
function ImageAbs:isOpen()
    return self.m_is_open
end

--实际的销毁处理
function ImageAbs:__destroy()

end

function ImageAbs:destroy()

    if not self.m_is_open then
        return end
    self.m_is_open = false

    self:clear_event()
	safe_call(self.__destroy, self)
    
    Refer.notifyDeactive(self)
end

--销毁并移除显示
function ImageAbs:destroyRemove()

    if not self.m_is_open then
        return end
    self.m_is_open = false

    self:clear_event()
	safe_call(self.__destroy, self)

    self:removeChildThis()

    Refer.notifyDeactive(self)
end


--//-------~★~-------~★~-------~★~gameobject~★~-------~★~-------~★~-------//

function ImageAbs:showGameObject(url)
    
    local go = GameObjCache:load_sync(url)
    self:__showGameObject(go, false)
end

function ImageAbs:showGameObjectEx(go_)

    self:__showGameObject(go_, true)
end

--@isExternal_ 是否外部托管
function ImageAbs:__showGameObject(go_, isExternal_)
    
    if self.m_gameObject == go_ then
        return end
    
    self:__clearGameObject()
    
    self.m_gameObject = go_
    self.m_goIsExternal = isExternal_
    if not isExternal_ then
        local name = StringUtil.sub_to_first(go_.name, "%(")
        go_.name = string.format("%s(%s)", name, self.referId)
    end
    
    self:__onShowGameObject()
end

function ImageAbs:__clearGameObject()
    
    if not self.m_gameObject then
        return end
    
    if self.m_goIsExternal then
        --外部托管,只是移除引用
        self.m_gameObject = nil
        
    else
        --是自己托管的go, 则删除对象
        delete(self.m_gameObject)
        self.m_gameObject = nil
    end
    
end


function ImageAbs:__onShowGameObject()

end

function ImageAbs:__onClearGameObject()

end

--//-------~★~-------~★~-------~★~观察者~★~-------~★~-------~★~-------//

--//-------~★~监听别人~★~-------//

--@notifer 需要监听的派发器，缺省时为公共那个
function ImageAbs:add_listener(type, fun, target, notifer)
	notifer = notifer or App.notifer
	notifer:attach(type, fun, target, self)
end


function ImageAbs:remove_listener(type, fun, target, notifer)
	notifer = notifer or App.notifer
	notifer:detach(type, fun, target)
end

return ImageAbs