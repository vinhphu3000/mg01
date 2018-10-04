--动作管理
--@author jr.zeng
--2017年7月14日 下午3:10:07
local modname = "ActionMgr"
--==================global reference=======================

--===================namespace========================
local ns = "org.action"
local using = {--[[namespace1,...]]}
local _ENV = namespace(ns, using)
--===================module property========================
local super = nil	--父类
_ENV[modname] = class(modname, super)
local ActionMgr = _ENV[modname]

local Action = Action

--===================module content========================

function ActionMgr:__ctor()
    
    self.m_actionNum = 0
    self.m_actions = new(Array)
    self.m_delArr = new(Array)
    
    self.isAutoUpdate = true    --是否自动更新
    
    self.m_schUpdated = false
    self.m_invalid = false
end

function ActionMgr:setup()


end

function ActionMgr:clear()

    self:stopAllActions()
end

function ActionMgr:setup_event()

end


function ActionMgr:clear_event()

end


function ActionMgr:schUpdate(b)
    
    if self.m_schUpdated == b then
        return end
    self.m_schUpdated = b
    if b then
        App:schUpdate(self.update, self)
    else
        App:unschUpdate(self.update, self)
        --nslog.debug(modname, "unschUpdate")
    end
end

--//-------~★~-------~★~-------~★~数据管理~★~-------~★~-------~★~-------//

function ActionMgr:update(dt)
    
    if self.m_actionNum > 0 then

        self.m_invalid = true
        
        local action
        for i=1, self.m_actionNum do
            
            action = self.m_actions[i]
            if action:running() then
                action:update(dt)
            end
            
            if action:isDone() or not action:running() then
            
                if self.m_actionNum == 0 then
                    --列表已清空, 为了支持在action中清空lite
                    break
                else
                    self.m_delArr:add(action)
                    action:clear()
                end
            end
        end
        
        self.m_invalid = false
        
        if #self.m_delArr > 0 then
            --需要删除
            local action
            for i=1, #self.m_delArr do

                action = self.m_delArr[i]
                if self.m_actions:remove(action) then
                    --nslog.debug(modname, "移除动作", action.cname)
                    clear_action(action)
                end
            end
            self.m_delArr:clear()
            
            self.m_actionNum = #self.m_actions
            if self.m_actionNum == 0 then
	            --停止更新
	            nslog.debug(modname, "stop udpate")
                self:schUpdate(false)
            end
        end 
    else
    
        self:schUpdate(false)
    end
end

--//-------~★~-------~★~-------~★~数据操作~★~-------~★~-------~★~-------//

function ActionMgr:run(target, action)
    
    action:startWithTarget(target)

    self.m_actions:add(action)

    self.m_actionNum = #self.m_actions
    if self.isAutoUpdate and self.m_actionNum == 1 then
		--自动更新
        self:schUpdate(true)
    end
    
    return action
end


function ActionMgr:stop(target)
    
    if #self.m_actions == 0 then
        return end
     
    local action
    for i = #self.m_actions, 1, -1 do
        
        action = self.m_actions[i]
        if action:getTarget() == target then
	        --这里只是调用clear, 由update作统一回收
	        action:clear()
        end
    end
end

function ActionMgr:stopAction(action)

	action:clear()
end


function ActionMgr:stopAllActions()
    
    if #self.m_actions == 0 then
        return end
    
    for i, action in ipairs(self.m_actions) do
        action:clear()
        clear_action(action)
    end

    self.m_actionNum = 0
    self.m_actions:clear()
    self.m_delArr.clear()

    self:schUpdate(false)
end

return ActionMgr