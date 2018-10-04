-- StageInfo
--@author jr.zeng
--@date 2018/7/31  10:58
local modname = "StageInfo"
--==================global reference=======================

--===================namespace========================
local ns = 'dvc'
local using = {--[[namespace1,...]]}
local _ENV = namespace(ns, using, modname)

--===================module property========================
local super = nil	--父类
StageInfo = class(modname, super, _ENV)
local StageInfo = StageInfo

--===================module content========================

function StageInfo:__ctor()

	self.playerInfos = false
	--起始玩家确认类型
	self.firstPlayerType = 0
	--玩家行动顺序
	self.playerActSeqType = 0

	self.deckInfo = new(DeckInfo)

end


function StageInfo:init(data)

	self.playerInfos = data.playerInfos
	self.firstPlayerType = data.firstPlayerType or FIRST_PLAYER_TP.ME_FIRST
	self.playerActSeqType = data.playerActSeqType or PLAYER_ACT_SEQ_TYPE.CLOCKWISE

	self.deckInfo:init(data)
end

return StageInfo