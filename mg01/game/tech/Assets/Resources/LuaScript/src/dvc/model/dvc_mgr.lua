-- dvc_mgr
--@author jr.zeng
--@date 2018/7/30  20:11
local modname = "dvc_mgr"
--==================global reference=======================

--===================namespace========================
local ns = 'dvc'
local using = {'org.bevtree'}
local _ENV = namespace(ns, using, modname)

--===================module property========================
local super = CCModule	--父类
dvc_mgr = class(modname, super, _ENV)
local dvc_mgr = dvc_mgr


--===================module content========================

function dvc_mgr:__ctor(...)
    
	self.my_uuid = 0


	local map = require 'src.dvc.proc.util.proc_map'
	self.m_procfactory = new(bev_factory, map)

	--参赛的玩家信息列表
	self.m_playerInfos = false

	self.myPlayInfo = false
	self:create_my_info({
		name ='爆石流翔',
		sex = PLAYER_SEX.MALE,
	})

end

function dvc_mgr:__setup(...)
    
    self:ctor()

	self:enter_welcome()
end

function dvc_mgr:__clear()


	self.m_playerInfos = false
end

function dvc_mgr:setup_event()

	App.keyboard:attach(KEY_EVENT.RELEASE, self.on_key_press, self)
    
end

function dvc_mgr:clear_event()


	App.keyboard:detach(KEY_EVENT.RELEASE, self.on_key_press, self)
end

--生成流程树
function dvc_mgr:gen_proc_tree(name, setting)
	local tree = self.m_procfactory:gen_bev_tree(name, setting)
	return tree
end

--生成流程节点
function dvc_mgr:gen_proc_bev(name)
	local bev = self.m_procfactory:build_bev_by_id(name)
	return bev
end

--//-------∽-★-∽------∽-★-∽--------∽-★-∽数据管理∽-★-∽--------∽-★-∽------∽-★-∽--------//

function dvc_mgr:on_key_press(evt_)

	local code = evt_.data
	if code == KeyCode.Alpha1 then

	elseif code == KeyCode.Alpha2 then

	elseif code == KeyCode.Alpha3 then

	elseif code == KeyCode.Alpha4 then

	end

end


--//-------∽-★-∽------∽-★-∽--------∽-★-∽数据操作∽-★-∽--------∽-★-∽------∽-★-∽--------//


--//-------∽-★-∽------∽-★-∽--------∽-★-∽玩家相关∽-★-∽--------∽-★-∽------∽-★-∽--------//

--创建自己的玩家信息
function dvc_mgr:create_my_info(data)

	local info = new(PlayerInfo)
	self.my_uuid = info.uuid
	info.name = data.name
	info.sex = data.sex
	info.is_me = true
	self.myPlayInfo = info
end


--创建参赛的玩家信息列表
function dvc_mgr:create_player_infos(num)

	num = math.max(2, num)

	self.m_playerInfos = {self.myPlayInfo}

	local npc_num = num - 1
	local npc_info
	for i=1, npc_num do
		npc_info = new(PlayerInfo)
		npc_info.name = CPU_NAME_LIST[i]
		npc_info.sex = math.random(1,2)
		npc_info.is_cpu = true
		--npc_info.index = 1
		self.m_playerInfos[#self.m_playerInfos+1] = npc_info
	end

	for i, playerInfo in ipairs(self.m_playerInfos) do
		playerInfo.index = i
	end
end


--//-------∽-★-∽------∽-★-∽--------∽-★-∽欢迎阶段∽-★-∽--------∽-★-∽------∽-★-∽--------//

function dvc_mgr:enter_welcome()

	App.popMgr:show(POP_ID.WELCOME)
	--self:enter_stage()
end

function dvc_mgr:exit_welcome()

	App.popMgr:close(POP_ID.WELCOME)
end

--//-------∽-★-∽------∽-★-∽--------∽-★-∽关卡阶段∽-★-∽--------∽-★-∽------∽-★-∽--------//

function dvc_mgr:enter_stage()

	self:exit_welcome()

	self:create_player_infos(4)

	local stage_info = new(StageInfo)
	stage_info:init({
		deckType = DECK_TYPE.DEFAULT,
		firstPlayerType = FIRST_PLAYER_TP.ME_FIRST,
		playerInfos = self.m_playerInfos,
	})

	stage_mgr:setup(stage_info)
	stage_mgr:start_stage()
end

function dvc_mgr:exit_stage()

	stage_mgr:exit_stage()
end

--//-------∽-★-∽------∽-★-∽--------∽-★-∽结算阶段∽-★-∽--------∽-★-∽------∽-★-∽--------//


return dvc_mgr