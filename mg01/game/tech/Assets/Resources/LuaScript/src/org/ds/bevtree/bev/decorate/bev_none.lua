-- 行为_隐形节点
--@author jr.zeng
--@date 2018/8/26  20:43
local modname = "bev_none"
--==================global reference=======================

--===================namespace========================
local ns = "org.bevtree"
local using = {--[[namespace1,...]]}
local _ENV = namespace(ns, using, modname)

--===================module property========================
local super = bev_parent	--父类
bev_none = class(modname, super, _ENV)
local bev_none = bev_none

local BEV_STATE = BEV_STATE

--===================module content========================

function bev_none:__ctor()
    
end

--//-------∽-★-∽------∽-★-∽--------∽-★-∽数据管理∽-★-∽--------∽-★-∽------∽-★-∽--------//

function bev_none:__update(input)

	if self.m_cur_bev then
		self.m_cur_bev:update(input)
		if self.m_cur_bev:done() then
			self.m_state = self.m_cur_bev:getState()
			self:set_cur_bev(nil, input)
		end
	end

end

--检测进入条件
function bev_none:check_cnd(input)

	local bev = self:get_fore_bev()
	if bev then --有子节点，返回其结果
		return bev:can_enter(input)
	end
	return true
end

function bev_none:__enter(input)

	local bev = self:get_fore_bev()
	if bev then --有子节点，进入
		self:set_cur_bev(bev, input)
	else    --没有则直接成功
		self.m_state = BEV_STATE.SUCCESS
	end
end



return bev_none