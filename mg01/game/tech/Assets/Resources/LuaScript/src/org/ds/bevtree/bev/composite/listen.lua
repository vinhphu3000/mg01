--组合节点_ 等待事件
--@author jr.zeng
--@date 2018/9/3  21:05
local modname = "listen"
--==================global reference=======================

--===================namespace========================
local ns =  "org.bevtree"
local using = {--[[namespace1,...]]}
local _ENV = namespace(ns, using, modname)

--===================module property========================
local super = bev_parent	--父类
listen = class(modname, super, _ENV)
local listen = listen

local LISTEN_CHLID_POLICY   = LISTEN_CHLID_POLICY
local LISTEN_DONE_POLICY    = LISTEN_DONE_POLICY

--===================module content========================

function listen:ctor(setting)

	self.m_chlid = false

	self.m_eventType = false
	self.m_chlidPolicy = 0
	self.m_donePolicy = 0

	super.ctor(self, setting)
end


function listen:setting(setting)

	super.setting(self, setting)


	self.m_chlidPolicy = setting.chlidPolicy or LISTEN_CHLID_POLICY.ENTER_CHILD_IDLE
	self.m_donePolicy = setting.donePolicy or LISTEN_DONE_POLICY.DONE_ON_CHILD

end

--//-------∽-★-∽------∽-★-∽--------∽-★-∽数据管理∽-★-∽--------∽-★-∽------∽-★-∽--------//


function listen:__update(input)

	if self.m_cur_bev then

		self.m_cur_bev:update(input)

		if self.m_cur_bev:done() then
			--已完成
			local state = self:set_cur_bev(nil, input)

			if self.m_donePolicy == LISTEN_DONE_POLICY.DONE_ON_CHILD then
				--子节点完成就完成
				self.m_state = state
			end
		end
	end

end


function listen:check_cnd(input)


	return true
end

--//-------∽-★-∽------∽-★-∽--------∽-★-∽数据操作∽-★-∽--------∽-★-∽------∽-★-∽--------//

function listen:__enter(input)

	self.m_eventType = self:get_property('eventType')
	if not self.m_eventType then
		assert(false, '没有事件类型')
	end

	self.m_agent:attach(self.m_eventType, self.on_event, self)

end

function listen:__exit(input)

	self.m_agent:detach(self.m_eventType, self.on_event, self)
	self.m_eventType = false
end

function listen:on_event(data)

	nslog.print_t('监听到事件', self.m_eventType)

	local input = self.m_input

	local bev = self:get_fore_bev()
	if bev then

		if self.m_cur_bev == bev then
			--已经在运行
			if self.m_chlidPolicy == LISTEN_CHLID_POLICY.ENTER_CHILD_ALWAYS then
				--重新进入
				self:set_cur_bev(nil, input)
				self:set_cur_bev(bev, input)
			end
		else

			if bev:can_enter(input) then

				self:set_cur_bev(bev, input)
			else
				--不能进入，直接失败
				nslog.print_t('子节点进入失败')
				self.m_state = BEV_STATE.FAIL
			end
		end
	else
		nslog.print_t('没有子节点直接成功')
		--没有子节点，直接成功
		self.m_state = BEV_STATE.SUCCESS
	end
end

return listen