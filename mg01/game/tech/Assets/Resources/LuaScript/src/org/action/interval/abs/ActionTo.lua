--ActionTo(抽象)
--@author jr.zeng
--2017年7月14日 下午4:17:33
local modname = "ActionTo"
--==================global reference=======================

--===================namespace========================
local ns = "org.action"
local using = {--[[namespace1,...]]}
local _ENV = namespace(ns, using)
--===================module property========================
local super = ActionInterval	--父类
_ENV[modname] = class(modname, super)
local ActionTo = _ENV[modname]



function lerp(from, to, rate)
	return from + ( to - from ) * rate
end

local lerp = lerp

--===================module content========================

function ActionTo:__ctor()

	self.m_fromValue = 0
	self.m_toValue = 1

end


function ActionTo:initWith(duration, toValue)

	self:initWithDuration(duration)

	self.m_toValue = toValue
end


--//-------~★~-------~★~-------~★~数据管理~★~-------~★~-------~★~-------//

--override
function ActionTo:__progress(rate)

	self:setCurValue( lerp(self.m_fromValue, self.m_toValue, rate) )
end


--virtual
function ActionTo:getCurValue()
	return 0
end

--virtual
function ActionTo:setCurValue(value)

end


--//-------~★~-------~★~-------~★~数据操作~★~-------~★~-------~★~-------//


--override
function ActionTo:__start()

	self.m_fromValue = self:getCurValue()

end


--override
function ActionTo:__clear()

	super.__clear(self)

	self.m_fromValue = 0
	self.m_toValue = 1

end

return ActionTo