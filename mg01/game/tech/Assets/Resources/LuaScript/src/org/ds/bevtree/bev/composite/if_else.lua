-- 组合节点_if_else
--@author jr.zeng
--@date 2018/8/23  21:21
local modname = "if_else"
--==================global reference=======================

--===================namespace========================
local ns = "org.bevtree"
local using = {--[[namespace1,...]]}
local _ENV = namespace(ns, using, modname)

--===================module property========================
local super = bev_parent	--父类
if_else = class(modname, super, _ENV)
local if_else = if_else

local BEV_STATE = BEV_STATE
local bev_util = bev_util

--===================module content========================

function if_else:__ctor()

	self.m_cnd_bev = false

end

--//-------∽-★-∽------∽-★-∽--------∽-★-∽数据管理∽-★-∽--------∽-★-∽------∽-★-∽--------//

function if_else:__update(input)

	if self.m_cnd_bev then
		self.m_cnd_bev:update(input)
		self:check_cnd_done(input)
	else

		self.m_cur_bev:update(input)
		if self.m_cur_bev:done() then
			self.m_state = self.m_cur_bev:getState()
			self:set_cur_bev(nil, input)
		end
	end
end


function if_else:check_cnd(input)

	local cnd = self.m_bev_arr[1]   --第一个节点
	local b = cnd:can_enter(input)
	return b
end


--//-------∽-★-∽------∽-★-∽--------∽-★-∽数据操作∽-★-∽--------∽-★-∽------∽-★-∽--------//

function if_else:__enter(input)

	self.m_cnd_bev = self.m_bev_arr[1]   --第一个节点作为条件
	self:set_cur_bev(self.m_cnd_bev, input)
	self:check_cnd_done(input)
end

--检测条件节点完成
function if_else:check_cnd_done(input)

	assert(self.m_cnd_bev)

	if self.m_cnd_bev:done() then

		local state = self.m_cnd_bev:getState()
		self.m_cnd_bev = false
		self:set_cur_bev(nil, input)

		local next
		if state == BEV_STATE.SUCCESS then
			--条件满足，走第二个节点
			next = self.m_bev_arr[2]
		else
			next = self.m_bev_arr[3]
		end

		assert(next)
		self:set_cur_bev(next, input)
	end
end

function if_else:__exit(input)

	self.m_cnd_bev = false
end


return if_else