--基于update的协程管理
--@author jr.zeng
--@date 2018/1/2  10:19
local modname = "coroutine_mgr"
--==================global reference=======================

local coroutine = coroutine

--===================namespace========================
local ns = "org"
local using = {--[[namespace1,...]]}
local _ENV = namespace(ns, using, modname)

--===================module property========================
local super = nil	--父类
local mt = class(modname, super, _ENV)
coroutine_mgr = mt
local coroutine_mgr = mt

co = {}
local co = co

local __id = 0
local function gen_id()
	__id = __id + 1
	return __id
end

local dead_stacks = {}  --这个栈不会释放
local dead_count = 0

local current_dt = 0

--===================module content========================

function mt:__ctor()

	self.m_id2stack = {}

	self.m_invalid = false

end

function mt:setup()

	self:ctor()


end

function mt:clear()


	self:__clear_all_stacks()
	self.m_invalid = false

end


--//-------~★~-------~★~-------~★~数据管理~★~-------~★~-------~★~-------//

function mt:update(dt)

	current_dt = dt
	dead_count = 0

	self.m_invalid = true

	for id, s in pairs(self.m_id2stack) do

		if #s > 0 then

			local co = s:tail()
			local st = coroutine.status(co)
			if st == "dead" then
				--运行完毕
				s:pop()

			elseif st == "suspended" then
				--挂起
				local suc, yieldret = coroutine.resume(co)
				if suc then
					if type(yieldret) == "thread" then
						--如果返回的是协程,添加到栈顶
						s:add(yieldret)

					elseif type(yieldret) == "string" then
						--
						if yieldret == "BREAK" then
							--跳出
							s:clear()
						end
					elseif type(yieldret) == "table" then

						if type(yieldret.isDone) == "function"  then
							s:add( co.wait_is_done(yieldret) )
						end

					end
				else
					s:clear()
				end
			end
		end

		if #s == 0 then
			dead_count = dead_count + 1
			dead_stacks[dead_count] = id        --一直覆盖这个栈
		end
	end

	self.m_invalid = false

	if dead_count > 0 then
		for i = 1, dead_count do
			nslog.print_t("co remove", dead_stacks[i])
			self:__clear_stack(dead_stacks[i])
		end
	end
end


--//-------~★~-------~★~-------~★~数据操作~★~-------~★~-------~★~-------//

--运行协程
function coroutine_mgr:run(fun)

	local id = gen_id()
	local stack = self:__create_stack(id)
	stack:add( coroutine.create(fun) )
	return id
end

--停止协程
function coroutine_mgr:stop(id)

	if id > 0 then
		if self.m_invalid  then
			--update中
			dead_count = dead_count + 1
			dead_stacks[dead_count] = id        --一直覆盖这个栈
		else
			self:__clear_stack(id)
		end
	end
	return 0
end

--停止全部协程
function coroutine_mgr:stop_all()

	self:__clear_all_stacks()
end

--//-------~★~-------~★~-------~★~栈管理~★~-------~★~-------~★~-------//

--创建协程栈
function mt:__create_stack(id)

	local stack = self.m_id2stack[id]
	if stack then
		return stack
	end

	local stack = new(Array)
	self.m_id2stack[id] = stack
	return stack
end

--清除协程栈
function mt:__clear_stack(id)

	local stack = self.m_id2stack[id]
	if not stack then
		return end

	stack:clear()
	self.m_id2stack[id] = nil
end

--清除所有协程栈
function mt:__clear_all_stacks()

	for k,stack in pairs( self.m_id2stack ) do

		stack:clear()
	end

	clear_tbl( self.m_id2stack  )
end


--//-------~★~-------~★~-------~★~co(快捷调用)~★~-------~★~-------~★~-------//


co.yield = coroutine.yield
co.resume = coroutine.resume
co.create = coroutine.create
co.status = coroutine.status

--//-------~★~协程指令~★~-------//

--打断协程
function co.break_()
	return "BREAK"
end


local function _wait_for_seconds(sec)
	local duration = sec
	return function()
		while duration > 0 do
			duration = duration - current_dt
			coroutine.yield()
		end
	end
end


--等待(s)
function co.wait_for_seconds(sec)
	return coroutine.create(_wait_for_seconds(sec))
end

local function _wait_for_frames(cnt)
	local frame_cnt = cnt
	return function()
		while frame_cnt > 0 do
			frame_cnt = frame_cnt - 1
			coroutine.yield()
		end
	end
end

--等待(frame)
function co.wait_for_frames(cnt)
	return coroutine.create(_wait_for_frames(cnt))
end

local function _wait_util(fun)
	return function()
		while not fun() do
			coroutine.yield()
		end
	end
end

--等待(直到fun返回true)
function co.wait_until(fun)
	return coroutine.create( _wait_util(fun) )
end


local function _wait_is_done(tbl)
	return function()
		while not tbl:isDone() do
			coroutine.yield()
		end
	end
end

--等待(isDone对象)
function co.wait_is_done(tbl)
	return coroutine.create( _wait_is_done(tbl) )
end

return mt