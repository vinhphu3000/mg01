-- player_cache
--@author jr.zeng
--@date 2018/7/31  15:01
local modname = "player_cache"
--==================global reference=======================

--===================namespace========================
local ns = 'dvc'
local using = {--[[namespace1,...]]}
local _ENV = namespace(ns, using, modname)

--===================module property========================
local super = CCModule	--父类
player_cache = class(modname, super, _ENV)
local player_cache = player_cache


--===================module content========================

function player_cache:__ctor()

	--起始玩家类型
	self.m_first_player_type = false
	--玩家行动顺序类型
	self.m_act_queue = 0


	self.m_uuid2player = false
	--玩家列表（按index排序）
	self.m_idxPlayers = false
	--玩家列表（按actIdx排序）
	self.m_actPlayers = false
	--玩家数量
	self.m_player_num = 0


	self.m_cur_idx = 0
	--当前玩家
	self.m_cur_player = false
	--自己的player
	self.m_my_player = false

end

function player_cache:__setup(stageInfo)

	self:ctor()

	self.m_first_player_type = stageInfo.firstPlayerType
	self.m_act_queue = stageInfo.playerActQueue

	self:setup_players(stageInfo.playerInfos)
	self:decide_start_player()

end

function player_cache:__clear()

	self:clear_players()

	self.m_first_player_type = false
end

function player_cache:setup_event()


end

function player_cache:clear_event()


end


--//-------∽-★-∽------∽-★-∽--------∽-★-∽数据管理∽-★-∽--------∽-★-∽------∽-★-∽--------//


function player_cache:setup_players(playerInfos)

	nslog.print_r('setup_players', playerInfos)

	self.m_uuid2player = {}
	self.m_idxPlayers = {}

	local player

	local playerInfo
	local len = #playerInfos
	for i=1, len do
		playerInfo = playerInfos[i]

		player = new(Player)
		player:setup(playerInfo)
		self.m_uuid2player[player.uuid] = player
		self.m_idxPlayers[#self.m_idxPlayers+1] = player

		if playerInfo.is_me then
			self.m_my_player = player
		end
	end

	self.m_player_num = len

	Array.sortOn(self.m_idxPlayers, 'index', Array.ASCENDING)

end


function player_cache:clear_players()

	if not self.m_idxPlayers then
		return end

	for i,player in pairs(self.m_idxPlayers) do
		player:clear()
	end

	self.m_uuid2player = false
	self.m_idxPlayers = false
	self.m_actPlayers =false

	self.m_cur_player = false
	self.m_my_player = false
end

function player_cache:get_player(uuid)
	return self.m_uuid2player[uuid]
end


--//-------∽-★-∽------∽-★-∽--------∽-★-∽数据操作∽-★-∽--------∽-★-∽------∽-★-∽--------//


--//-------∽-★-∽------∽-★-∽--------∽-★-∽行动玩家∽-★-∽--------∽-★-∽------∽-★-∽--------//

--确定起始玩家
function player_cache:decide_start_player()

	self.m_actPlayers = {}

	local index
	if self.m_first_player_type == FIRST_PLAYER_TP.RANDOM then
		index = math.random(1, #self.m_idxPlayers)
	else
		index = self.m_my_player.index
	end

	local player

	for i=1, self.m_player_num do
		local j = i+(index-1)
		if  j > self.m_player_num then
			j = j - self.m_player_num
		end
		player = self.m_idxPlayers[j]
		player.act_idx = i  --行动序号
		self.m_actPlayers[#self.m_actPlayers+1] = player
	end

	--self:reset_first_player()
end


--设置当前玩家
function player_cache:set_cur_player(player)

	if self.m_cur_player == player then
		return end
	self.m_cur_player = player
	self.m_cur_idx = player.act_idx

	nslog.print_t(string.format('<color=#ffdf58ff>%s 的行动！！</color>', player:get_label() ))

	self:notify(DVC_EVENT.CUR_PLAYER_CHANGE, player)
end


--重置起始玩家
function player_cache:reset_first_player()
	local player = self.m_actPlayers[1]
	self:set_cur_player(player)
	return player
end


--获取起始玩家
function player_cache:get_first_player()
	return self.m_actPlayers[1]
end

--获取下一个行动的玩家
function player_cache:get_next_player()

	if self.m_cur_idx >= self.m_player_num then
		return nil
	end

	local next_id = self.m_cur_idx + 1
	return self.m_actPlayers[next_id]
end




return player_cache