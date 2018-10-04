--ActionForever
--@author jr.zeng
--2017年7月17日 下午3:33:47
local modname = "ActionForever"
--==================global reference=======================

--===================namespace========================
local ns = "org.action"
local using = {--[[namespace1,...]]}
local _ENV = namespace(ns, using)
--===================module property========================
local super = ActionBase	--父类
_ENV[modname] = class(modname, super)
local ActionForever = _ENV[modname]

--===================module content========================

function ActionForever:__ctor()
    
    self.m_action = nil
    
end


function ActionForever:isDone()
	return false    --不会完成
end


function ActionForever:initWith(action)

	self.m_action = action

	if not self.m_action.getElapsed then
		nslog.error(modname, "action必须是ActionInterval子类")
	end

	self.m_inited = true
end


--//-------~★~-------~★~-------~★~数据管理~★~-------~★~-------~★~-------//

--override
function ActionForever:update(dt)
    
    self.m_action:update(dt)
    
    if self.m_action:isDone() then
        --nslog.debug(modname, "done")
        
        local duration = self.m_action:getDuration()
        local diff = self.m_action:getElapsed() - duration  --获取超过的时间
        --nslog.debug(modname,diff, "diff")
        
        if diff > duration then
            --超过了一个周期
            diff = diff % duration
        end
        
        self.m_action:startWithTarget(self.m_target)
        --to prevent jerk. issue #390, 1247
        self.m_action:update(0)
        self.m_action:update(diff)  --把时间补回来
    end
end

--//-------~★~-------~★~-------~★~数据操作~★~-------~★~-------~★~-------//

--override
function ActionForever:__start()

	super.__start(self)

	self.m_action:startWithTarget(self.m_target)
end


--override
function ActionForever:__reset()

    super.__reset(self)

    self.m_action:reset()
end

--override
function ActionForever:__clear()

    super.__clear(self)

    clear_action(self.m_action)
    self.m_action = nil
end

return ActionForever