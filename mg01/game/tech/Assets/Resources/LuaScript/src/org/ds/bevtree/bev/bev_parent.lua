-- 行为_节点容器
--@author jr.zeng
--@date 2018/4/16  11:08
local modname = "bev_parent"
--==================global reference=======================

--===================namespace========================
local ns = "org.bevtree"
local using = {--[[namespace1,...]]}
local _ENV = namespace(ns, using, modname)

--===================module property========================
local super = bev_base	--父类
bev_parent = class(modname, super, _ENV)
local bev_parent = bev_parent

local BEV_STATE = BEV_STATE

--===================module content========================

function bev_parent:__ctor(setting)

	self.m_bev_arr = false
	self.m_bev_num = 0
	self.m_cur_bev = false

end



--初始化
function bev_parent:setup(agent, ...)

	self.m_agent = agent

	self:__setup(...)
	self:setup_all_bev(...)

	if self.m_cnd_ex then
		self.m_cnd_ex:setup(...)
	end

	self.m_isOpen = true
	self.m_state = BEV_STATE.NONE
end



function bev_parent:clear()

	if self.m_isOpen == false then
		return end
	self.m_isOpen = false

	if self.m_cnd_ex then
		self.m_cnd_ex:clear()
	end

	if self.m_state ~= BEV_STATE.NONE then
		--居然没有exit,强制exit一下
		self:exit({})
	end

	self:clear_cur_bev()
	self:clear_all_bev()


	self:__clear()

	self.m_board = false
	self.m_input = false
	self.m_agent = false
end


--设置白板
function bev_parent:set_board(board)

	self.m_board = board
	self.m_input = board

	if self.m_bev_arr then
		for k,v in pairs(self.m_bev_arr) do
			v:set_board(board)
		end
	end
end

--设置代理
function bev_parent:set_agent(agent)
	if not agent then
		return end
	self.m_agent = agent

	if self.m_bev_arr then
		for k,v in pairs(self.m_bev_arr) do
			v:set_agent(agent)
		end
	end
end


--//-------∽-★-∽------∽-★-∽--------∽-★-∽数据管理∽-★-∽--------∽-★-∽------∽-★-∽--------//

function bev_parent:__update(input)

end

--//-------∽-★-∽------∽-★-∽--------∽-★-∽进入∽-★-∽--------∽-★-∽------∽-★-∽--------//


--退出节点
function bev_parent:exit(input)

	if self.m_state == BEV_STATE.NONE then
		return end

	--self:print_t( "exit", self.m_state)

	self.m_result = self.m_state
	self.m_state = BEV_STATE.NONE

	self:set_cur_bev(nil, input)
	self:exit_all_bev(input)

	self:__exit(input)

	--nslog.debug("exit", BEV_STATE2NAME[self.m_result])
	return self.m_result
end


--//-------∽-★-∽------∽-★-∽--------∽-★-∽当前节点∽-★-∽--------∽-★-∽------∽-★-∽--------//


--设置当前节点
function bev_parent:set_cur_bev(bev, input)

	if self.m_cur_bev == bev then
		return end

	local state

	if self.m_cur_bev then
		state = self.m_cur_bev:exit(input)
		self.m_cur_bev = false
	end

	self.m_cur_bev = bev
	if bev then
		state = bev:enter(input)
	end

	return state
end

--清除当前节点, 与setup对应
function bev_parent:clear_cur_bev()

	if not self.m_cur_bev then
		return end
	self.m_cur_bev:clear()
	self.m_cur_bev = nil
end

--//-------∽-★-∽------∽-★-∽--------∽-★-∽子节点管理∽-★-∽--------∽-★-∽------∽-★-∽--------//


--添加节点
function bev_parent:add_bev(bev)

	if not self.m_bev_arr then
		self.m_bev_arr = {}
	end

	self.m_bev_arr[#self.m_bev_arr+1] = bev
	self.m_bev_num = self.m_bev_num + 1

	bev.parent = self   --父节点

	if self.m_board then
		bev:set_board(self.m_board)
	end
end


--初始化子节点
function bev_parent:setup_all_bev(...)

	if self.m_bev_arr then
		local bev
		for i=1, #self.m_bev_arr do
			bev = self.m_bev_arr[i]
			bev:setup(self.m_agent, ...)
		end
	end
end

function bev_parent:clear_all_bev()

	if self.m_bev_arr then
		local bev
		for i=1, #self.m_bev_arr do
			bev = self.m_bev_arr[i]
			bev:clear()
		end
	end
end

function bev_parent:exit_all_bev(input)

	if self.m_bev_arr then
		local bev
		for i=1, #self.m_bev_arr do
			bev = self.m_bev_arr[i]
			bev:exit(input)
		end
	end
end

--获取首个子节点
function bev_parent:get_fore_bev()
	if self.m_bev_arr then
		return self.m_bev_arr[1]
	end
	return nil
end




return bev_parent