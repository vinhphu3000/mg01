-- bev_util
--@author jr.zeng
--@date 2018/8/23  15:50
local modname = "bev_util"
--==================global reference=======================

--===================namespace========================
local ns = 'org.bevtree'
local using = {--[[namespace1,...]]}
local _ENV = namespace(ns, using, modname)

--===================module property========================
bev_util ={}
local bev_util = bev_util

local BEV_STATE = BEV_STATE
local OPR_TYPE = OperateType

--===================module content========================


--值->节点状态(通用)
--@noRunning 不使用running
function bev_util.result2bevState(result, useRunning)

	if result == nil then
		return BEV_STATE.SUCCESS
	elseif result == true then
		return BEV_STATE.SUCCESS
	elseif result == false then
		return BEV_STATE.FAIL
	elseif result == BEV_STATE.RUNNING then
		if useRunning then
			return BEV_STATE.RUNNING
		else
			return BEV_STATE.SUCCESS
		end
	elseif result == BEV_STATE.SUCCESS then
		return BEV_STATE.SUCCESS
	elseif result == BEV_STATE.FAIL then
		return BEV_STATE.FAIL
	end
	return BEV_STATE.SUCCESS
end


function bev_util.operate(valueL, valueR, opr_type)

	if opr_type == OPR_TYPE.Add then
		return valueL + valueR
	elseif opr_type == OPR_TYPE.Sub then
		return valueL - valueR
	elseif opr_type == OPR_TYPE.Mul then
		return valueL * valueR
	elseif opr_type == OPR_TYPE.Div then
		return valueL / valueR
	elseif opr_type == OPR_TYPE.Equal then
		return valueL == valueR
	elseif opr_type == OPR_TYPE.NotEqual then
		return valueL ~= valueR
	elseif opr_type == OPR_TYPE.Greater then
		return valueL > valueR
	elseif opr_type == OPR_TYPE.Less then
		return valueL < valueR
	elseif opr_type == OPR_TYPE.GreaterEqual then
		return valueL >= valueR
	elseif opr_type == OPR_TYPE.LessEqual then
		return valueL <= valueR
	else
		assert(false, '无效操作符：' .. opr_type)
	end
end



return bev_util