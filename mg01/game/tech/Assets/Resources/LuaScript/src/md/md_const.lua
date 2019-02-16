-- md_const
--@author jr.zeng
--@date 2019/2/11  15:14
local modname = "md_const"
--==================global reference=======================

--===================namespace========================
local ns = 'md'
local using = {--[[namespace1,...]]}
local _ENV = namespace(ns, using, modname)

local get_evt_type = get_evt_type

--===================module content========================


--属性类型
ATTR_TYPE =
{
	--生存点
	SURVIVAL = 'survival',
	--移动力
	MOVEMENT = 'movement',
	--力量
	STRENGTH = 'strength',
	--回避
	EVASION = 'evasion',
	--幸运
	LUCK = 'luck',
	--速度
	SPEED = 'speed',
	--疯狂值
	

}



--事件
MD_EVT =
{

}
