--CCModule
--@author jr.zeng
--2017年9月29日 下午4:28:41
local modname = "CCModule"
--==================global reference=======================

--===================namespace========================
local ns = "org"
local using = {--[[namespace1,...]]}
local _ENV = namespace(ns, using)
--===================module property========================
local super = nil	--父类
_ENV[modname] = class(modname, super)
CCModule = _ENV[modname]
local CCModule = CCModule

local Refer = Refer
local App = App
--===================module content========================

function CCModule:__ctor()
    
    self.m_is_open = false
    self.m_notifier = false
end

function CCModule:setup(...)

    if self.m_is_open then
        return end
    
    self:__setup(...)
    self:setup_event()
    
    self.m_is_open = true    --有可能在__setup里调ctor导致置false,所以放后面
end


function CCModule:clear()

    if not self.m_is_open then
        return end
    self.m_is_open = false

    self:clear_event()
	safe_call(self.__clear, self)
    
    self:detachAll()
	Refer.notifyDeactive(self)
    
end

function CCModule:__setup(...)

end

function CCModule:__clear()

end

function CCModule:setup_event()

end

function CCModule:clear_event()

end


--//-------~★~-------~★~-------~★~数据管理~★~-------~★~-------~★~-------//


--//-------~★~-------~★~-------~★~数据操作~★~-------~★~-------~★~-------//

--//-------~★~-------~★~-------~★~观察者~★~-------~★~-------~★~-------//

--//-------~★~监听别人~★~-------//

--@notifer 需要监听的派发器，缺省时为公共那个
function CCModule:add_listener(type, fun, target, notifer)
	notifer = notifer or App.notifer
	notifer:attach(type, fun, target, self)
end


function CCModule:remove_listener(type, fun, target, notifer)
	notifer = notifer or App.notifer
	notifer:detach(type, fun, target)
end


function CCModule:attach_update(fun, target)
	App:attach_update(fun, target, self)
end

function CCModule:detach_update(fun, target)
	App:detach_update(fun, target)
end

--//-------~★~被别人监听~★~-------//

--添加监听
function CCModule:attach(type, fun, target, refer)
    if not self.m_notifier then
        self.m_notifier = App.notifer  end
    self.m_notifier:attach(type, fun, target, refer)
end

--移除监听
function CCModule:detach(type, fun, target)
	if not self.m_notifier then
		self.m_notifier = App.notifer  end
	self.m_notifier:detach(type, fun, target)
end

function CCModule:detachByType(type)
	if not self.m_notifier then
		self.m_notifier = App.notifer  end
	self.m_notifier:detachByType(type)
end

function CCModule:detachByType2(type, target)
	if not self.m_notifier then
		self.m_notifier = App.notifer  end
	self.m_notifier:detachByType2(type, target)
end

--派发
function CCModule:notify(name, ...)
	if not self.m_notifier then
		self.m_notifier = App.notifer  end
	self.m_notifier:notify(name, ...)
end

--用事件派发
function CCModule:notifyWithEvent(type, data)
	if not self.m_notifier then
		self.m_notifier = App.notifer  end
	self.m_notifier:notifyWithEvent(type, data)
end

function CCModule:detachAll()
    if self.m_notifier and
        self.m_notifier ~= App.notifer then --不是公共派发器
        self.m_notifier:detachAll()
    end
end

return CCModule