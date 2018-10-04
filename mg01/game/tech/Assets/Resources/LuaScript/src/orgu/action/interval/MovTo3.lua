--移动到(anchoredPosition)
--@author jr.zeng
--@date 2018/1/2  22:53
local modname = "MovTo3"
--==================global reference=======================

--===================namespace========================
local ns = "org.action"
local using = {--[[namespace1,...]]}
local _ENV = namespace(ns, using, modname)

--===================module property========================
local super = SetTo	--父类
MovTo3 = class(modname, super, _ENV)
local MovTo3 = MovTo3

local pack = table.pack
local unpack = table.unpack

--===================module content========================

--TODO 还不完美

function MovTo3:__ctor()



end

--@toPos {x,y}
function MovTo3:initWith(duration, toPos)

	self:initWithDuration(duration)

	self.m_toValue = toPos
end


--//-------~★~-------~★~-------~★~数据管理~★~-------~★~-------~★~-------//


function MovTo3:getCurValue()
	return pack( self.m_target:GetAchPos_() )  -- v1,v2,v3 -> {v1,v2,v3}
end

--@value {v1,v2,v3}
function MovTo3:setCurValue(value)
	self.m_target:SetAchPos_( unpack(value) )
end


--//-------~★~-------~★~-------~★~数据操作~★~-------~★~-------~★~-------//




return MovTo3