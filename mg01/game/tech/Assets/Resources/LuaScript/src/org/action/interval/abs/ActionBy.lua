--ActionBy(抽象)
--@author jr.zeng
--2017年7月14日 下午7:36:25
local modname = "ActionBy"
--==================global reference=======================

--===================namespace========================
local ns = "org.action"
local using = {--[[namespace1,...]]}
local _ENV = namespace(ns, using)
--===================module property========================
local super = ActionTo	--父类
_ENV[modname] = class(modname, super)
local ActionBy = _ENV[modname]

local pack = table.pack
local unpack = table.unpack
local TableUtil = TableUtil
local merge_table = TableUtil.merge_table

--===================module content========================

function ActionBy:__ctor()

    
end


function ActionBy:initWith(duration, addValue)

	self:initWithDuration(duration)

	self.m_addValue = addValue
end



--override
function ActionBy:__start()

	self.m_fromValue = self:getCurValue()
	self.m_toValue = self.m_fromVal + self.m_addVal

end


--override
function ActionBy:__clear()

	super.__clear(self)

	self.m_addValue = false
end

return ActionBy