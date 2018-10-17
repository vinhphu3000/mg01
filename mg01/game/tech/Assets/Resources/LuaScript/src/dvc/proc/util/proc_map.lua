-- proc_map
--@author jr.zeng
--@date 2018/8/4  11:44
local modname = "proc_map"
--==================global reference=======================

--===================namespace========================
local ns = 'dvc.proc'
local using = {'org.bevtree'}
local _ENV = namespace(ns, using, modname)

local gen_bev_cfg = gen_bev_cfg
local PROC_TYPE = PROC_TYPE

local tinsert = tinsert

--===================module content========================

--阶段流程_抽牌
_stg_ph_draw_ = function()

	local function a()
		print('ph_draw_')
	end

	local root = _seq_(nil,
		_wait_({time=2}),
		_callback_({callback=a}),
		--'player_in_ph_draw__'
		_act_each_player_({proc_id='_ply_proc_ph_draw_'})
	)
	return root
end


--玩家流程_抽牌阶段
_ply_proc_ph_draw_ = function()

	local card_num = CARD_COUNT_PLAYER_START

	local root  = _seq_(nil,
		_wait_({time=1}),
		_loop_({count=card_num}, _ply_proc_draw_card_() ),
		_loop_({count=card_num}, _ply_proc_place_card_() ),
		_wait_({time=1})
	)
	return root
end


--玩家抽一张卡
_ply_proc_draw_card_ = function()

	local root  = _seq_(nil,
		_wait_({time=1}),
		_ply_draw_card_()
	)
	return root
end

--玩家选一张卡加入游戏区
_ply_proc_place_card_ = function()

	local root  = _seq_(nil,
		_wait_({time=1}),
		_ply_place_card_()
	)
	return root
end


--//-------∽-★-∽cpu∽-★-∽--------//

--玩家流程_抽牌阶段
_ply_proc_ph_draw_cpu_ = function()

	local card_num = CARD_COUNT_PLAYER_START

	local root  = _seq_(nil,
	_wait_({time=1}),
	_loop_({count=card_num}, _cpu_proc_draw_card_() ),
	_loop_({count=card_num}, _cpu_proc_place_card_() ),
	_wait_({time=1})
	)
	return root
end

--cpu抽一张卡
_cpu_proc_draw_card_ = function()

	local root  = _seq_(nil,
	_wait_({time=0.2}),
	_cpu_draw_card_()
	)
	return root
end

--cpu选一张卡加入游戏区
_cpu_proc_place_card_ = function()

	local root  = _seq_(nil,
	_wait_({time=0.2}),
	_cpu_place_card_()
	)
	return root
end


--//-------∽-★-∽------∽-★-∽--------∽-★-∽流程相关∽-★-∽--------∽-★-∽------∽-★-∽--------//

--每个玩家轮流行动
function _act_each_player_(setting)
	local cfg = gen_bev_cfg(PROC_TYPE.ACT_EACH_PLAYER, setting)
	return cfg
end

--玩家抽牌
function _ply_draw_card_(setting)
	local cfg = gen_bev_cfg(PROC_TYPE.PLY_DRAW_CARD, setting)
	return cfg
end

function _ply_place_card_(setting)
	local cfg = gen_bev_cfg(PROC_TYPE.PLY_PLACE_CARD, setting)
	return cfg
end

function _cpu_draw_card_(setting)
	local cfg = gen_bev_cfg(PROC_TYPE.CPU_DRAW_CARD, setting)
	return cfg
end

function _cpu_place_card_(setting)
	local cfg = gen_bev_cfg(PROC_TYPE.CPU_PLACE_CARD, setting)
	return cfg
end


return _ENV