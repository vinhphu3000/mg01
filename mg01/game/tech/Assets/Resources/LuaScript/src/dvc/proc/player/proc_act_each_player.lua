-- proc_act_each_player
--@author jr.zeng
--@date 2018/8/15  16:18
local modname = "proc_act_each_player"
--==================global reference=======================

--===================namespace========================
local ns = 'dvc.proc'
local using = {'org.bevtree'}
local _ENV = namespace(ns, using, modname)

--===================module property========================
local super = proc_base	--父类
proc_act_each_player = class(modname, super, _ENV)
local proc_act_each_player = proc_act_each_player

--===================module content========================

function proc_act_each_player:setting(setting)

	super.setting(self, setting)

	self.m_proc_id = setting.proc_id
end


function proc_act_each_player:__ctor()

	self.m_actProc = false

	self.m_id2proc = false
	self.m_actProcRunning = false
end


function proc_act_each_player:__setup(...)

	super.__setup(self, ...)

	self.m_id2proc = {}
end

function proc_act_each_player:__clear()

	super.__clear(self)

	self:clear_act_proc()

	self.m_actProc = false
	self.m_id2proc = false
end

function proc_act_each_player:get_act_proc(player)

	local proc_id = self.m_proc_id
	if player:is_cpu() then
		proc_id = proc_id .. 'cpu_'
	end

	local proc = self.m_id2proc[proc_id]
	if not proc then
		proc = dvc_mgr:gen_proc_bev(proc_id)
		self.m_id2proc[proc_id] = proc
	end
	return proc
end

--//-------∽-★-∽------∽-★-∽--------∽-★-∽数据管理∽-★-∽--------∽-★-∽------∽-★-∽--------//

function proc_act_each_player:__update(input)


	if self.m_actProcRunning then

		self.m_actProc:update(input)
		if self.m_actProc:done() then

			self:clear_act_proc(input)

			local player = player_cache:get_next_player()   --下一个玩家
			if player then
				player_cache:set_cur_player(player)

				local proc = self:get_act_proc(player)
				self:run_act_proc(proc, input, player)
			else
				self.m_state = BEV_STATE.SUCCESS
			end
		end
	end

end


--//-------∽-★-∽------∽-★-∽--------∽-★-∽数据操作∽-★-∽--------∽-★-∽------∽-★-∽--------//

function proc_act_each_player:__enter(input)

	local player = player_cache:reset_first_player()
	local proc = self:get_act_proc(player)
	self:run_act_proc(proc, input, player)
end

function proc_act_each_player:__exit(input)

	self:clear_act_proc(input)
end

--//-------∽-★-∽------∽-★-∽--------∽-★-∽玩家流程∽-★-∽--------∽-★-∽------∽-★-∽--------//

function proc_act_each_player:run_act_proc(proc, input, player)

	assert(not self.m_actProcRunning)

	self.m_actProc = proc
	self.m_actProc:setup(player)
	self.m_actProc:set_board(self.m_board)

	self.m_actProc:enter(input)
	self.m_actProcRunning = true
end

function proc_act_each_player:clear_act_proc(input)

	if not self.m_actProcRunning then
		return end
	self.m_actProcRunning = false

	if input then
		self.m_actProc:exit(input)
	end

	self.m_actProc:clear()
	self.m_actProc = false
end


return proc_act_each_player