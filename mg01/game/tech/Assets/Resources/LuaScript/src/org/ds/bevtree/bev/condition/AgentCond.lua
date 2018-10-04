-- AgentCond
--@author jr.zeng
--@date 2018/8/23  14:34
local modname = "AgentCond"
--==================global reference=======================

--===================namespace========================
local ns = "org.bevtree"
local using = {--[[namespace1,...]]}
local _ENV = namespace(ns, using, modname)

--===================module property========================
local super = bev_base	--父类
AgentCond = class(modname, super, _ENV)
local AgentCond = AgentCond

local BEV_STATE = BEV_STATE

local bev_util = bev_util


--===================module content========================

function AgentCond:ctor(setting)

	--操作符
	self.m_operator = false


	super.ctor(self, setting)
end

--@setting {
--  memberTypeL
--  memberL
--  memberTypeR
--  memberR
function AgentCond:setting(setting)

	super.setting(self, setting)

	self.m_operator = setting.operator or OperateType.Equal    --默认是相等
	--nslog.print_r(setting)
end


--进入
function AgentCond:__enter(input)

	local valueL = self:get_property('opl')
	local valueR = self:get_property('opr')
	--nslog.print_t(valueL, valueR, self.m_operator)
	local result = bev_util.operate(valueL, valueR, self.m_operator)
	self.m_state = bev_util.result2bevState(result, true)
end


--退出
function AgentCond:__exit(input)


end

