-- 关卡管理
--@author jr.zeng
--@date 2018/7/30  21:28
local modname = "stage_mgr"
--==================global reference=======================

--===================namespace========================
local ns = 'dvc'
local using = {'org.bevtree'}
local _ENV = namespace(ns, using, modname)

--===================module property========================
local super = CCModule	--父类
stage_mgr = class(modname, super, _ENV)
local stage_mgr = stage_mgr


--===================module content========================

function stage_mgr:__ctor(...)

	--关卡信息
	self.m_stageInfo = false

	--当前阶段
	self.m_phase = false
	--阶段流程
	self.m_phaseProc = false

	self.m_procInput = false

	--回合数
	self.m_round = 0
end

function stage_mgr:__setup(stageInfo)

    self:ctor()

	self.m_stageInfo = stageInfo

	--nslog.print_r('__setup', stageInfo)

	player_cache:setup(stageInfo)
	stage_deck:setup(stageInfo.deckInfo)

end

function stage_mgr:__clear()

	self:set_phase(false)

	stage_deck:clear()
	player_cache:clear()

	self.m_stageInfo = false

	self.m_round = 0

end

function stage_mgr:setup_event()

end

function stage_mgr:clear_event()

end

--
function stage_mgr:get_stageInfo()
	return self.m_stageInfo
end



--//-------∽-★-∽------∽-★-∽--------∽-★-∽数据管理∽-★-∽--------∽-★-∽------∽-★-∽--------//


--//-------∽-★-∽------∽-★-∽--------∽-★-∽数据操作∽-★-∽--------∽-★-∽------∽-★-∽--------//

--//-------∽-★-∽------∽-★-∽--------∽-★-∽关卡相关∽-★-∽--------∽-★-∽------∽-★-∽--------//

--开始关卡
function stage_mgr:start_stage()

	self:set_phase(STAGE_PHASE.DRAW)

end

--退出关卡
function stage_mgr:exit_stage()

	self:clear()
end


--//-------∽-★-∽------∽-★-∽--------∽-★-∽阶段相关∽-★-∽--------∽-★-∽------∽-★-∽--------//

function stage_mgr:get_phase()
	return self.m_phase
end

function stage_mgr:set_phase(phase)

	if self.m_phase == phase then
		return end
	self.m_phase = phase

	if phase then
		self:start_ph_proc(phase)
		self:notify(DVC_EVENT.STG_PHASE_CHANGE, phase)
	else
		self:clear_ph_proc()
	end
end


function stage_mgr:start_ph_proc(name)

	self:clear_ph_proc()

	self.m_phaseProc = dvc_mgr:gen_proc_tree(name, { })
	self.m_procInput = self.m_phaseProc:get_input()

	self.m_phaseProc:setup(self)
	self.m_phaseProc:enter(self.m_procInput)

	self:schUpdate(self.update_ph_proc, self)
end


function stage_mgr:clear_ph_proc()

	if not self.m_phaseProc then
		return end
	self.m_phaseProc:exit(self.m_procInput)
	self.m_phaseProc:clear()
	self.m_phaseProc = false
	self.m_procInput = false

	self:unschUpdate(self.update_ph_proc, self)
end


function stage_mgr:update_ph_proc(delta)

	self.m_procInput.delta = delta
	self.m_phaseProc:update(self.m_procInput)
	if self.m_phaseProc:done() then
		--流程结束了
		nslog.print_t('流程结束了', self.m_phase)
		self:clear_ph_proc()
	end
end


--//-------∽-★-∽------∽-★-∽--------∽-★-∽回合相关∽-★-∽--------∽-★-∽------∽-★-∽--------//

function stage_mgr:get_round()
	return self.m_round
end

function stage_mgr:set_round(round)

	if self.m_round == round then
		return end
	self.m_round = round


end


return stage_mgr