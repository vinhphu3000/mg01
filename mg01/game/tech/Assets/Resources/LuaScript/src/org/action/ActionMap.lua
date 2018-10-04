--Action工厂
--@author jr.zeng
--2017年7月14日 下午4:56:18
local modname = "Action"
--==================global reference=======================

--===================namespace========================
local ns = "org.action"
local using = {--[[namespace1,...]]}
local _ENV = namespace(ns, using)
--===================module property========================
local super = nil	--父类

--===================module content========================

local pool = Inst(ClassPools)

local __new = new
local function new(cls)
	return pool:pop(cls)
end

--清除并回收action
function action.clear_action(action)

	action:clear()

	if action.__fromPool then
		pool:push(action)
		--nslog.debug(modname, "回收", action.cname)
	end
end

--//-------~★~-------~★~-------~★~基础动作~★~-------~★~-------~★~-------//

--动作_延时
function delay(duration)

    local action = new(ActionDelay)
    action:initWithDuration(duration)
    return action
end

--动作_队列_任意个动作
function seq(...)

    local action = new(ActionSequence)
    action:initWithActions(...)
    return action
end

--动作_队列_两个动作
function seq2(action1, action2)
    
    local action = new(ActionSequence)
    action:initWithTwoActions(action1, action2)
    return action
end

--动作_并行_任意个动作
function spawn(...)
    
    local action = new(ActionSpawn)
    action:initWithActions(...)
    return action
end

--动作_并行_两个个动作
function spawn2(action1, action2)

    local action = new(ActionSpawn)
    action:initWithTwoActions(action1, action2)
    return action
end

--动作_循环
function loop(action_, repeatCnt)
    
    local action = new(ActionLoop)
    action:initWith(action_, repeatCnt)
    return action
end


--动作_一直循环
--要优化,现在不能放seq里
function forever(action_)

    local action = new(ActionForever)
    action:initWith(action_)
    return action
end

--动作_缓动
--@easeType 缓动类型
function ease(action_, easeType)
    
    local action = new(ActionEase)
    action:initWith(action_, easeType)
    return action
end

--动作_回调
function call(func)
    
    local action = new(ActionCall)
    action:initWithFunc(func)
    return action
end


--动作_SetTo
function set_to(duration, setting)

	local action = new(SetTo)
	action:initWith(duration, setting)
	return action
end



--动作_增量
function set_by(duration, setting)

	local action = new(ActionBy)
	action:initWith(duration, setting)
	return action
end


--//-------~★~-------~★~-------~★~gameobject相关动作~★~-------~★~-------~★~-------//



