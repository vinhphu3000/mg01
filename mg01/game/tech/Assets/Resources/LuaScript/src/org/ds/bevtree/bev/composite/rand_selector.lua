-- 组合节点_随机选择节点
--@author jr.zeng
--@date 2018/9/5  21:16
local modname = "rand_selector"
--==================global reference=======================

--===================namespace========================
local ns = "org.bevtree"
local using = {--[[namespace1,...]]}
local _ENV = namespace(ns, using, modname)

--===================module property========================
local super = bev_parent	--父类
rand_selector = class(modname, super, _ENV)
local rand_selector = rand_selector

local BEV_STATE = BEV_STATE

--===================module content========================

function rand_selector:__ctor()

	self.m_enable_arr = false

end


--//-------∽-★-∽------∽-★-∽--------∽-★-∽数据管理∽-★-∽--------∽-★-∽------∽-★-∽--------//

function rand_selector:__update(input)

	if self.m_cur_bev then

		self.m_cur_bev:update(input)

		if self.m_cur_bev:done() then
			local state =  self.m_cur_bev:getState()
			self:set_cur_bev(nil, input)
			self.m_state = state
		end
	end
end

function rand_selector:check_cnd(input)

	local b = false

	self.m_enable_arr = false

	if self.m_bev_arr then

		local len = #self.m_bev_arr
		local bev
		for i=1, len do
			bev = self.m_bev_arr[i]
			if bev:can_enter(input) then
				--能进入
				self.m_enable_arr = self.m_enable_arr or {}
				self.m_enable_arr[#self.m_enable_arr+1] = bev
				b = true
			end
		end
	end

	return b
end

--//-------∽-★-∽------∽-★-∽--------∽-★-∽数据操作∽-★-∽--------∽-★-∽------∽-★-∽--------//

function rand_selector:__enter(input)

	local bev = self:calc_rand()
	if bev then
		self:set_cur_bev(bev, input)
	else
		self.m_state = BEV_STATE.FAIL
	end
end

function rand_selector:__exit(input)

	self.m_enable_arr = false

end

function rand_selector:calc_rand()

	if not self.m_enable_arr then
		return nil
	end

	if #self.m_enable_arr == 1 then
		return self.m_enable_arr[1]
	end

	local len = #self.m_enable_arr
	local id2w = {}
	local cur_w = 0
	local n,w
	for i=1, len do
		n = self.m_enable_arr[i]
		w = n:get_property('weight')
		id2w[i] = cur_w
		cur_w = cur_w + w
	end

	local a = MathUtil.rand_float(0,cur_w)
	nslog.print_t('权重算出', a)

	n = false
	for i=len, 1, -1 do
		n = self.m_enable_arr[i]
		w = id2w[i]
		if a >= w then
			nslog.print_t('选中了', i)
			break
		end
	end

	return n
end


return rand_selector