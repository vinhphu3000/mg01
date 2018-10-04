--组合节点_并行_全部节点完成才done
--@author jr.zeng
--2017年4月17日 下午5:20:11
--同时进入，有节点失败则返回失败，全部成功才返回成功
local modname = "parallel"
--==================global reference=======================

--===================namespace========================
local ns = "org.bevtree"
local using = {--[[namespace1,...]]}
local _ENV = namespace(ns, using)
--===================module property========================
local super = bev_parent	--父类
parallel = class("parallel", super)
local parallel = parallel

local BEV_STATE = BEV_STATE
local SUCC_POLICY = SUCC_POLICY
local FAIL_POLICY = FAIL_POLICY
local clear_tbl = clear_tbl

--===================module content========================

function parallel:ctor(setting)

    self.m_idx2state = {}

	--成功条件类型
	self.m_succPolicy = 0
	--失败条件类型
	self.m_failPolicy = 0


    super.ctor(self, setting)
end


--@setting {
--  succPolicy(成功策略)，
--  failPolicy(失败策略)，
--}
function parallel:setting(setting)

	super.setting(self, setting)

	self.m_succPolicy = setting.succPolicy or SUCC_POLICY.SUCC_ON_ALL
	self.m_failPolicy = setting.failPolicy or FAIL_POLICY.FAIL_ON_ONE

end


--//-------∽-★-∽------∽-★-∽--------∽-★-∽数据管理∽-★-∽--------∽-★-∽------∽-★-∽--------//

local done_num = 0
local succ_num = 0
local fail_num = 0
local state

function parallel:__update(input)

	done_num, succ_num, fail_num = 0, 0, 0

    local bev
    for i=1, #self.m_bev_arr do

	    state = self.m_idx2state[i]
        if state ~= BEV_STATE.RUNNING then
            done_num = done_num + 1
	        if state == BEV_STATE.SUCCESS then
		        succ_num = succ_num + 1
		    else
				fail_num = fail_num + 1
		    end
        else
        
            bev = self.m_bev_arr[i]
	        self.m_idx2state[i] = bev:update(input)
            if bev:done() then
                done_num = done_num + 1

	            state = self.m_idx2state[i]
	            --self:print_t( "子节点完成了", bev.cname, bev.m_state, state)

	            if state == BEV_STATE.SUCCESS then
		            succ_num = succ_num + 1
		            if self.m_succPolicy == SUCC_POLICY.SUCC_ON_ONE then
			            --有一个成功了就成功
			            --self:print_t( "一个成功了就成功")
			            self.m_state = BEV_STATE.SUCCESS
		            end
		        else
		            fail_num = fail_num + 1
		            if self.m_failPolicy == FAIL_POLICY.FAIL_ON_ONE then
			            --有一个失败了就失败
			            self.m_state = BEV_STATE.FAIL
		            end
		        end
	            bev:exit(input)
            end
        end
    end

	if done_num >= self.m_bev_num then
		--全部完成
		if self.m_state ==  BEV_STATE.RUNNING then
			if self.m_succPolicy == SUCC_POLICY.SUCC_ON_ALL then
				--全部成功才成功
				if succ_num >= self.m_bev_num then
					self.m_state = BEV_STATE.SUCCESS
				else
					self.m_state = BEV_STATE.FAIL
				end
			elseif self.m_failPolicy == FAIL_POLICY.FAIL_ON_ALL then
				--全部失败才失败
				if fail_num >= self.m_bev_num then
					self.m_state = BEV_STATE.FAIL
				else
					self.m_state = BEV_STATE.SUCCESS
				end
			else
				nslog.print('怎么会！！', self.m_succPolicy, self.m_failPolicy)
				self.m_state = BEV_STATE.FAIL
			end
		end
	end

end


function parallel:check_cnd(input)

	clear_tbl(self.m_idx2state)

	succ_num = 0

	local b = true
    if self.m_bev_arr then
        local bev
        for i=1, #self.m_bev_arr do
            bev = self.m_bev_arr[i]
	        if bev:can_enter(input) then
		        self.m_idx2state[i] = BEV_STATE.RUNNING
		        succ_num = succ_num + 1
	        else
		        self.m_idx2state[i] = BEV_STATE.FAIL
		        --不能进入
		        if self.m_succPolicy == SUCC_POLICY.SUCC_ON_ALL then
			        --全部成功才成功
			        b = false
			        break
		        elseif self.m_failPolicy == FAIL_POLICY.FAIL_ON_ONE then
					--一个失败就失败
			        b = false
			        break
		        end
	        end
        end

	    if b then
		    if succ_num == 0 then
			    --没有一个能进
			    b = false
		    end
	    end

	else
	    b = false
    end
    return b
end


--//-------∽-★-∽------∽-★-∽--------∽-★-∽数据操作∽-★-∽--------∽-★-∽------∽-★-∽--------//

function parallel:__enter(input)

    local bev
    for i=1, #self.m_bev_arr do
	    if self.m_idx2state[i] == BEV_STATE.RUNNING then
		    bev = self.m_bev_arr[i]
		    bev:enter(input)
	    end
    end
end

function parallel:__exit(input)

	clear_tbl(self.m_idx2state)
end



return parallel