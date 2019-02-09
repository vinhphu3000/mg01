--PopMgr
--@author jr.zeng
--2017年10月14日 下午4:53:34
local modname = "PopMgr"
--==================global reference=======================

--===================namespace========================
local ns = "org"
local using = {--[[namespace1,...]]}
local _ENV = namespace(ns, using, modname)
--===================module property========================
local super = CCModule	--父类
PopMgr = class(modname, super)
local PopMgr = PopMgr

local POP_LIFE = POP_LIFE

POP_ID = {}  --所有的窗口id将被注册到这里

--===================module content========================

PopMgr.__id2config = {}

function PopMgr:__ctor()
    
    self.m_id2pop = {}
    
    self.m_stackMax = 5 --最大存栈数量
    
    self.m_openArr = new(Array)
    self.m_closeArr = new(Array)
    
    
end


function PopMgr:__setup(...)

    
end

function PopMgr:__clear()
    
    self:__removeAllPops()
    
end

function PopMgr:setup_event()


end

function PopMgr:clear_event()

end


--//-------~★~-------~★~-------~★~配置相关~★~-------~★~-------~★~-------//

--注册窗口配置
function PopMgr.addPopConfig(id2cfg)

	for id, cfg in pairs(id2cfg) do
		POP_ID[id] = id     --赋值给全局POP_ID

		if PopMgr.__id2config[id] then
			assert(false, 'POP_ID重复：' .. id)   --不能重复
		end

		PopMgr.__id2config[id] = cfg
	end
end

function PopMgr.getPopConfig(popId_)
    local cfg = PopMgr.__id2config[popId_]
    if not cfg then
        nslog.error("找不到窗口配置", popId_)
        return nil
    end
    return cfg
end

function PopMgr.getPrefabUrl(popId_)
    return PopMgr.getPopConfig(popId_).prefab_url
end

--//-------~★~-------~★~-------~★~层级管理~★~-------~★~-------~★~-------//


function PopMgr:__addToLayer(pop_)

end

--//-------~★~-------~★~-------~★~窗口操作~★~-------~★~-------~★~-------//

--打开窗口
function PopMgr:show(popId_, ...)
    
    local pop = self.m_id2pop[popId_]
    if pop then
        pop:show(...)
        return pop
    end
    
    --TODO 支持异步加载 
    
    pop = self:__createPop(popId_) 
    if not pop then
        return nil
    end 
    
    self:__addPop(pop)
    pop:show(...)
    return pop
end


function PopMgr:showOrClose(popId_, ...)
    
    if self:popIsOpen(popId_) then
        self:close(popId_)
    else
        self:show(popId_, ...)
    end
end


--//-------~★~窗口弹出~★~-------//


function PopMgr:pop(pop_)
    
    local pop_id = pop_.pop_id
    nslog.debug("pop open", pop_id)
    
    self:__addToOpen(pop_)
    self:__addToLayer(pop_)
    
    self:notifyWithEvent(POP_EVT.POP_OPEN, pop_id)
    
    if not pop_:isOpen() then
        --第一次弹出, 播放动画
        self:__aniPop(pop_)
    end
end

--弹出动画
function PopMgr:__aniPop(pop_)

end

--//-------~★~窗口关闭~★~-------//


function PopMgr:close(popId_, force_)
    
    local pop = self:getPopOpened(popId_)
    if not pop then
        return end
        
    if force_ then
        --强制关闭
        self:excuteClose(pop)
    end
    
    self:__aniClose(pop)
end

--关闭动画
function PopMgr:__aniClose(pop_)
    
    self:__excuteClose()
end

function PopMgr:__excuteClose(pop_)
    
    if not pop_:isOpen() then
        return end

    local pop_id = pop_.pop_id
    nslog.debug("pop close", pop_id)
    
    self:__destroyPop(pop_)
    self:__addToClose(pop_)
    
    self:notifyWithEvent(POP_EVT.POP_CLOSE, pop_id)
end

function PopMgr:__destroyPop(pop_)
    
    pop_:destroyRemove()
end

--关闭全部
function PopMgr:closeAll()
    
    local len =  #self.m_openArr
    if len == 0 then
        return end
    
    local pop
    for i=1, len do
        pop = self.m_openArr[i]
        self:close(pop.pop_id, true)    --默认强制关闭
    end
end

--//-------~★~-------~★~-------~★~窗口管理~★~-------~★~-------~★~-------//

--添加窗口
function PopMgr:__addPop(pop_)
    
    local pop_id = pop_.pop_id
    if not self.m_id2pop[pop_id] then
        
        self.m_id2pop[pop_id] = pop_
        pop_:retain(self)
    else
        
        if self.m_id2pop[pop_id] ~= pop_ then
            nslog.error("重复的pop", pop_id)
        end
    end
end

--移除窗口
function PopMgr:__delPop(popId_)
    
    if not self.m_id2pop[popId_] then
        return end
        
    local pop = self.m_id2pop[popId_]
    self.m_id2pop[popId_] = nil
    
    self:__removeFromStack(pop)
    
    pop:destroy()
    pop:release(self)
end

--移除全部窗口
function PopMgr:__removeAllPops()
    
    if not next(self.m_id2pop) then
        return end
        
    self:__clearStack()
    
    for id,pop in pairs(self.m_id2pop) do
        pop:destroy()
        pop:release(self)    
    end
    
    clear_tbl(self.m_id2pop)
end

--获取已打开的窗口
function PopMgr:getPopOpened(popId_)
    
    local pop = self.m_id2pop[popId_]
    if pop and pop:isOpen() then
        return pop
    end
    return nil
end

--窗口是否打开
function PopMgr:popIsOpen(popId_)

    local pop = self.m_id2pop[popId_]
    if pop and pop:isOpen() then
        return true
    end
    return false
end

--创建窗口
function PopMgr:__createPop( popId_ )

	local cfg =  PopMgr.getPopConfig( popId_ )
	local path = cfg.script_url --脚本路径
	local class = require(path)
	local pop = new(class)

	if pop.pop_id ~= popId_ then
		assert(false, string.format("窗口id不一致 %s $s", popId_, pop.pop_id  ) )
	end

	return pop
end

--//-------~★~-------~★~-------~★~堆栈管理~★~-------~★~-------~★~-------//

--添加到开启列表
function PopMgr:__addToOpen(pop_)
    
    self.m_openArr:remove(pop_)
    self.m_openArr:add(pop_)
    
    if self:needStack(pop_) then
        self.m_closeArr:remove(pop_)
    end
end

--添加到关闭列表
function PopMgr:__addToClose(pop_)

    self.m_openArr:remove(pop_)
    
    if self:needStack(pop_) then
        --放到栈尾
        
        self.m_closeArr:remove(pop_)
        self.m_closeArr:add(pop_)
        
        if #self.m_closeArr > self.m_stackMax then
            --超过最大容量,删除栈头
            self:__delPop(self.m_closeArr[1].pop_id)
        end
    elseif pop_.lifeType == POP_LIFE.WEAK then
        --弱引用
        self:__delPop(pop_.pop_id)
    end
    
end


function PopMgr:__removeFromStack(pop_)
    
    self.m_openArr:remove(pop_)
    
    if self:needStack(pop_) then
        self.m_closeArr:remove(pop_)
    end
end

function PopMgr:__clearStack()
    
    self.m_openArr:clear()
    self.m_closeArr:clear()
    
end


--是否需要存栈
function PopMgr:needStack(pop_)
    if pop_.lifeType == POP_LIFE.STACK then
        return true
    end
    return false
end

return PopMgr