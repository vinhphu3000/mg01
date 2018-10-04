--ActionCall
--@author jr.zeng
--2017年7月17日 下午3:58:14
local modname = "ActionCall"
--==================global reference=======================

--===================namespace========================
local ns = "org.action"
local using = {--[[namespace1,...]]}
local _ENV = namespace(ns, using)
--===================module property========================
local super = ActionInstant	--父类
_ENV[modname] = class(modname, super)
local ActionCall = _ENV[modname]

--===================module content========================

function ActionCall:__ctor()
    
    self.m_callback = false
    self.m_called = false
    
end


function ActionCall:initWithFunc(func)
    
    self:init()
    
    self.m_callback = func
    
end


--//-------~★~-------~★~-------~★~数据管理~★~-------~★~-------~★~-------//


--//-------~★~-------~★~-------~★~数据操作~★~-------~★~-------~★~-------//


--override
function ActionCall:__done()

	if not self.m_called then
		self.m_called = true
		self.m_callback()
	end
end

--override
function ActionCall:__reset()

    super.__reset(self)

    self.m_called = false
end

--override
function ActionCall:__clear()

    super.__clear(self)

    self.m_callback = false
    self.m_called = false
end



return ActionCall