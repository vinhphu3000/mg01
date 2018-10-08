-- PopConst
--@author jr.zeng
--@date 2018/7/14  16:02
local modname = "PopConst"
--==================global reference=======================

--===================namespace========================
local ns = 'dvc'
local using = {--[[namespace1,...]]}
local _ENV = namespace(ns, using, modname)

--===================module content========================


--//-------~★~-------~★~-------~★~pop~★~-------~★~-------~★~-------//

local POP_ID = {

	WELCOME = {script_url = "src.dvc.view.welcome_view", prefab_url = "GUI/cn/Prefab/Canvas_Welcome", res_urls = {} },
	STAGE = {script_url = "src.dvc.view.stage_view", prefab_url = "GUI/cn/Prefab/Canvas_Stage", res_urls = {} },
}


PopMgr.addPopConfig(POP_ID)


--//-------~★~-------~★~-------~★~const~★~-------~★~-------~★~-------//

--玩家性别
PLAYER_SEX =
{
	MALE = 0,   --男
	FEMALE = 1, --女
}

--起始玩家决定类型
FIRST_PLAYER_TP =
{
	--从自己开始(单机)
	ME_FIRST = 0,
	--随机起始玩家
	RANDOM = 1,
}

--卡牌所在区域
CARD_ZONE_TP =
{
	--公共牌区
	GLOBAL = 0,
	--玩家游戏区
	GAME = 1,
	--玩家抽牌区
	DRAW = 2,
}

--卡牌状态
CARD_STATE =
{
	--覆盖
	CLOSE = 0,
	--翻开
	OPEN = 1,
}

--卡牌颜色
CARD_COLOR =
{
	--白
	WHITE = 1,
	--黑
	BLACK = 2,  --数字也代表优先级
}


--卡牌颜色名称
CARD_COLOR_NAME =
{
	[CARD_COLOR.WHITE] = '白',
	[CARD_COLOR.BLACK] = '黑',
}


--卡牌颜色的数量
CARD_COLOR_NUM = 2

--牌库类型
DECK_TYPE =
{
	--标准
	DEFAULT = 1,
}


local function card_cfg(pattern, weight)
	weight = weight or tonumber(pattern)
	return {pattern = pattern, weight = weight }
end

--牌库配置
DECK_CONFIG =
{
	[DECK_TYPE.DEFAULT] = {
		[CARD_COLOR.BLACK] = {
			card_cfg('0'), card_cfg('1'), card_cfg('2'), card_cfg('3'),
			card_cfg('4'), card_cfg('5'), card_cfg('6'), card_cfg('7'),
			card_cfg('8'), card_cfg('9'), card_cfg('10'), card_cfg('11'),
			card_cfg('-', -1)
		},
		[CARD_COLOR.WHITE] = {
			card_cfg('0'), card_cfg('1'), card_cfg('2'), card_cfg('3'),
			card_cfg('4'), card_cfg('5'), card_cfg('6'), card_cfg('7'),
			card_cfg('8'), card_cfg('9'), card_cfg('10'), card_cfg('11'),
			card_cfg('-', -1)
		},
	}

}


--关卡阶段
STAGE_PHASE = {
	NONE = 'None',
	--抽牌阶段
	DRAW = '_stg_ph_draw_',
	--游戏阶段
	GAME = '_stg_ph_game_',
	--算分阶段
	SCORE = '_stg_ph_score_',
}

--起始手牌数量
CARD_COUNT_PLAYER_START = 3

--玩家行动顺序类型
PLAYER_ACT_SEQ_TYPE =
{
	--顺时针
	CLOCKWISE = 0,
	--逆时针
	ANTI_CLOCKWISE = 1,
}

CPU_NAME_LIST =
{
	'路人甲',
	'瞎子乙',
	'大力丙',
	'保湿丁'
}

--//-------~★~-------~★~-------~★~Event~★~-------~★~-------~★~-------//


DVC_EVENT =
{
	--关卡阶段改变
	STG_PHASE_CHANGE = 'STG_PHASE_CHANGE',
	--当前玩家改变
	CUR_PLAYER_CHANGE = 'CUR_PLAYER_CHANGE',


}
