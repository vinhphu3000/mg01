-- AgentAction
--@author jr.zeng
--@date 2018/8/22  21:32
local modname = "AgentAction"
--==================global reference=======================

--===================namespace========================
local ns = "org.bevtree"
local using = {--[[namespace1,...]]}
local _ENV = namespace(ns, using, modname)

--===================module property========================
local super = bev_base	--父类
AgentAction = class(modname, super, _ENV)
local AgentAction = AgentAction

local BEV_STATE = BEV_STATE
local result2bevState = bev_util.result2bevState

--===================module content========================

function AgentAction:ctor(setting)

	self.m_method = false

	self.m_resultOpt = false
	self.m_resultMethod = false

	super.ctor(self, setting)

	self.m_methodCalled = false

end


--@setting {
--  method
--  resultOpt       使用“决定状态的选项”所选择的值（Success、Failure或Running），表示该方法执行完毕后，动作节点将返回这个设置的值
--  resultMethod    当方法的原型是void Method(…)的时候，“决定状态的函数”的原型为：EBTStatus StatusFunctor()。
--                  当方法的原型是ReturnType Method(…)的时候，“决定状态的函数”的原型为：EBTStatus StatusFunctor(ReturnType param)

function AgentAction:setting(setting)

	super.setting(self, setting)

	self.m_method = setting.method

	if setting.resultOpt ~= nil then
		self.m_resultOpt = setting.resultOpt
	else
		self.m_resultOpt =  BEV_STATE.SUCCESS
	end

	if setting.resultMethod then
		self.m_resultMethod = setting.resultMethod
	end

end

function AgentAction:__update(input)

	--if not self.m_methodCalled then
	--	self.m_state = self:excute_method()
	--	self.m_methodCalled = true  --结果可能是RUNNING, 所以要用m_methodCalled标记
	--end
end


--进入
function AgentAction:__enter(input)

	self.m_methodCalled = false
	self.m_state = self:excute_method()
end


--退出
function AgentAction:__exit(input)

	self.m_methodCalled = false

end

function AgentAction:excute_method()

	local state
	local methodResult = self.m_method(self.m_agent)

	if self.m_resultMethod then
		--有决定状态的函数
		local result = self.m_resultMethod(self.m_agent, methodResult)
		state = result2bevState(result, true)
		nslog.print_t('resultMethod', state)
	elseif self.m_resultOpt ~= BEV_STATE.NONE then
		state = self.m_resultOpt
	else
		--使用method的返回值
		state = result2bevState(methodResult, true)
	end

	return state
end

return AgentAction