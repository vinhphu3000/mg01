--ActionEase
--@author jr.zeng
--2017年7月17日 下午5:01:01
local modname = "ActionEase"
--==================global reference=======================

local COS = math.cos
local SIN = math.sin
--local POW = function(base, exp)
--    return base^exp
--end
local PI = math.pi
local PI2 = PI * 2

--===================namespace========================
local ns = "org.action"
local using = {--[[namespace1,...]]}
local _ENV = namespace(ns, using)
--===================module property========================
local super = ActionInterval	--父类
_ENV[modname] = class(modname, super)
local ActionEase = _ENV[modname]

local clamp = MathUtil.clamp

--缓动类型
EaseType = 
    {
        Linear = 0,
        EaseIn = 1,
        EaseOut = 2,
        EaseInOut = 3,
        BounceIn = 4,
        BounceOut = 5,
    }
    
local EaseType = EaseType

EaseFunc = {}
local EaseFunc = EaseFunc

--===================module content========================

function ActionEase:__ctor()

    self.m_action = nil
    self.m_easeType = 1
    
    self.m_easeFunc = nil
    
end

--@action 需要缓动的动作
--@ease_type 缓动类型EaseType
function ActionEase:initWith(action, ease_type)

    self.m_action = action
    self.m_easeType = ease_type or EaseType.EaseInOut
    self.m_easeFunc = EaseFunc.get_func(self.m_easeType)
    
    self:initWithDuration(self.m_action:getDuration())
end


--//-------~★~-------~★~-------~★~数据管理~★~-------~★~-------~★~-------//



--override
function ActionEase:__progress(value)

	value = self.m_easeFunc(value)
	self.m_action:progress(value)
end

--//-------~★~-------~★~-------~★~数据操作~★~-------~★~-------~★~-------//


--override
function ActionEase:__start()

    super.__start(self)

    self.m_action:startWithTarget(self.m_target)
end



--override
function ActionEase:__reset()

    super.__reset(self)

    self.m_action:reset()
end

--override
function ActionEase:__clear()

    super.__clear(self)

    clear_action(self.m_action)    --回收
    self.m_action = nil
end


--//-------~★~-------~★~-------~★~缓动函数相关~★~-------~★~-------~★~-------//

function EaseFunc.get_func(type)

    if type == EaseType.EaseIn then
        return EaseFunc.easeIn
    elseif type == EaseType.EaseOut then
        return EaseFunc.easeOut
    elseif type == EaseType.EaseInOut then
        return EaseFunc.easeInOut
    elseif type == EaseType.BounceIn then
        return EaseFunc.bounceIn
    elseif type == EaseType.BounceOut then
        return EaseFunc.bounceOut
    else
    
        return EaseFunc.linear
    end
end


function EaseFunc.linear(val)
    return val
end

function EaseFunc.easeIn(val)
    val = 1 - SIN( 0.5 * PI * (1 - val))
    return val
end


function EaseFunc.easeOut(val)
    val = SIN(0.5 * PI * val)
    return val
end


function EaseFunc.easeInOut(val)
    val = val - SIN(val * PI2) / PI2;
    return val
end

local function bounceLogic(time)

    if (time < 1 / 2.75) then
        return 7.5625 * time * time
    elseif (time < 2 / 2.75) then
        time = time - 1.5 / 2.75
        return 7.5625 * time * time + 0.75

    elseif(time < 2.5 / 2.75) then
        time = time - 2.25 / 2.75
        return 7.5625 * time * time + 0.9375
    end
    time = time - 2.625 / 2.75
    return 7.5625 * time * time + 0.984375
end

function EaseFunc.bounceIn(val)
    val = bounceLogic(val)
    return val
end

function EaseFunc.bounceOut(val)
    val = 1 - bounceLogic(1-val)
    return val
end


return ActionEase