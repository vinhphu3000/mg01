--行为节点_基类
--@author jr.zeng
--2017年4月17日 上午10:06:45
local modname = "bev_base"
--==================global reference=======================

--===================namespace========================
local ns = "org.bevtree"
local using = {--[[namespace1,...]]}
local _ENV = namespace(ns, using, modname )
--===================module property========================
local super = nil	--父类
bev_base = class(modname, super)
local bev_base = bev_base
local M = bev_base

local BEV_STATE = BEV_STATE
local PropertyType = PropertyType

--===================module content========================

function bev_base:__ctor(setting)

    --log.debug(self.cname, "ctor")

    self.parent = false       --父节点
    
    self.m_isOpen = false

    self.m_state = BEV_STATE.NONE
    self.m_result = false
    
    self.m_cnd_ex = false   --外部条件

    self.m_board = false    --白板
	self.m_input = false    --输入(现在就是白板)
	self.m_agent = false    --代理
    
	self.m_setting = setting or {}
    self:setting(self.m_setting)
end

function bev_base:setting(setting)

end


--初始化
function bev_base:setup(agent, ...)

	self:set_agent(agent)

    self:__setup(...)

    if self.m_cnd_ex then
        self.m_cnd_ex:setup(...)
    end

    self.m_isOpen = true
    self.m_state = BEV_STATE.NONE
    
end

function bev_base:clear()

    if self.m_isOpen == false then
        return end
    self.m_isOpen = false

    if self.m_cnd_ex then
        self.m_cnd_ex:clear()
    end

	if self.m_state ~= BEV_STATE.NONE then
		--居然没有exit,强制exit一下
		self:exit({})
	end

    self:__clear()

    self.m_board = false
	self.m_input = false
	self.m_agent = false
end

--virual
function bev_base:__setup(...)

end

--virual
function bev_base:__clear()

end

--获取当前状态
function bev_base:getState()
    return self.m_state
end

--获取运行结果
function bev_base:getResult()
    return self.m_result
end

function bev_base:running()
    return self.m_state == BEV_STATE.RUNNING
end

--是否完成
function bev_base:done()
    return self.m_state == BEV_STATE.SUCCESS or  self.m_state == BEV_STATE.FAIL
end

function bev_base:success()
    return self.m_state == BEV_STATE.SUCCESS
end

function bev_base:failed()
    return self.m_state == BEV_STATE.FAIL
end

--设置白板
function bev_base:set_board(board)
    self.m_board = board
	self.m_input = board
end

function bev_base:get_board()
	return self.m_board
end

function bev_base:get_input()
	return self.m_input
end

--//-------∽-★-∽------∽-★-∽--------∽-★-∽agent相关∽-★-∽--------∽-★-∽------∽-★-∽--------//

--设置代理
function bev_base:set_agent(agent)
	if not agent then
		return end
	self.m_agent = agent
end

function bev_base:get_agent()
	return  self.m_agent
end

--获取setting中的属性值
function bev_base:get_property(name)

	local value = self.m_setting[name]
	if value == nil then
		assert(false, '找不到property：'..name)
	end

	local typeName = self.m_setting[name..'Type']
	if typeName then
		--setting有指定类型
		if typeName == PropertyType.Const then          --常量
			return value
		elseif typeName == PropertyType.Method or       --agent的函数
				typeName == PropertyType.Property then  --agent的属性
			local fun = value
			value = fun(self.m_agent)
		end
	end

	return value
end


--//-------∽-★-∽------∽-★-∽--------∽-★-∽数据管理∽-★-∽--------∽-★-∽------∽-★-∽--------//

function bev_base:update(input)
    
    if self.m_state == BEV_STATE.RUNNING then
        self:__update(input)
    end
    
    return self.m_state
end

function bev_base:__update(input)
    
end


--//-------∽-★-∽------∽-★-∽--------∽-★-∽进入条件∽-★-∽--------∽-★-∽------∽-★-∽--------//

--检测进入条件
function bev_base:check_cnd(input)
    return true
end


--设置外部条件
function bev_base:set_cnd(cnd)
    self.m_cnd_ex = cnd
end


--//-------∽-★-∽------∽-★-∽--------∽-★-∽进入∽-★-∽--------∽-★-∽------∽-★-∽--------//

--能否进入
function bev_base:can_enter(input)

    if self.m_cnd_ex then
        --先判断外部条件
        if self.m_cnd_ex:check_cnd(input) == false then
            return false
        end
    end
    
    if not self:check_cnd(input) then
        --内部条件不通过
        return false
    end

    return true
end

--进入节点
function bev_base:enter(input)

	if self.m_state ~= BEV_STATE.NONE then
		return self.m_state
	end

	self.m_state = BEV_STATE.RUNNING
	self.m_result = false

	self:__enter(input)

	self:print_t( "enter", self.m_state)

	return self.m_state
end

--退出节点
function bev_base:exit(input)

    if self.m_state == BEV_STATE.NONE then
        return self.m_state
    end

	--self:print_t( "exit", self.m_state)

    self.m_result = self.m_state
    
    self:__exit(input)

	self.m_state = BEV_STATE.NONE
	--nslog.debug("exit", BEV_STATE2NAME[self.m_result])
	return self.m_result
end

--进入
function bev_base:__enter(input)

end


--退出
function bev_base:__exit(input)

end


function bev_base:print_t(...)

	if not self.m_input.debug then
		return end
	nslog.print_t(self.cname, ...)
end

return bev_base