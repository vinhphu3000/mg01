--SetTo
--@author jr.zeng
--2017年7月14日 下午4:17:33
local modname = "SetTo"
--==================global reference=======================

--===================namespace========================
local ns = "org.action"
local using = {--[[namespace1,...]]}
local _ENV = namespace(ns, using)
--===================module property========================
local super = ActionInterval	--父类
_ENV[modname] = class(modname, super)
local SetTo = _ENV[modname]


local pack = table.pack
local unpack = table.unpack
local merge_table = TableUtil.merge_table

local lerp = lerp

--===================module content========================

function SetTo:__ctor()


	self.m_setter = false
	self.m_getter = false

	self.m_curValues = false

end

--@setting
--  toValue {v1,v2,v3}
--  getter: function return v1,v2,v3 end
--  setter  function(v1,v2,v3)
function SetTo:initWith(duration, setting)

	self:initWithDuration(duration)

	self.m_setter = setting.setter
	self.m_getter = setting.getter

	if type(setting.toValue) == "table" then
		self.m_toValue = setting.toValue
	else
		self.m_toValue = {setting.toValue}
	end

end


--//-------~★~-------~★~-------~★~数据管理~★~-------~★~-------~★~-------//

--override
function SetTo:__progress(value)

	local from,to
	for i=1, #self.m_fromValue do
		from = self.m_fromValue[i]
		to = self.m_toValue[i]
		self.m_curValues[i] = lerp(from, to, value)   --当前值
	end

	self:setCurValue(self.m_curValues)
end



function SetTo:getCurValue()
	return pack( self.m_getter() )  -- v1,v2,v3 -> {v1,v2,v3}
end

--@value {v1,v2,v3}
function SetTo:setCurValue(value)
	self.m_setter( unpack(value) )
end


--//-------~★~-------~★~-------~★~数据操作~★~-------~★~-------~★~-------//


--override
function SetTo:__start()

	self.m_fromValue = self:getCurValue()

	--相互填补空缺
    merge_table(self.m_toValue, self.m_fromValue)
    merge_table(self.m_fromValue, self.m_toValue)

	self.m_curValues = {}
end

--override
function SetTo:__clear()
    
    super.__clear(self)

	self.m_setter = false
	self.m_getter = false

	self.m_curValues = false
end

return SetTo