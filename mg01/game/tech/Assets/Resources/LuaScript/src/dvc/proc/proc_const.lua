-- proc_const
--@author jr.zeng
--@date 2018/8/4  12:02
local modname = "proc_const"
--==================global reference=======================

--===================namespace========================
local ns = 'dvc.proc'
local using = {--[[namespace1,...]]}
local _ENV = namespace(ns, using, modname)

--===================module content========================

-- 流程类型
PROC_TYPE =
{
	BASE = 'proc_base',

	--每个玩家轮流行动
	ACT_EACH_PLAYER = 'proc_act_each_player',
	--玩家抽牌
	PLAYER_DRAW_CARD = 'proc_player_draw_card',
	--玩家放牌
	PLAYER_PLACE_CARD = 'proc_player_place_card',


}