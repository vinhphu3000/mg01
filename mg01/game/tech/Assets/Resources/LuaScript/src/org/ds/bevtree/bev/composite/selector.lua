--组合节点_选择节点
--@author jr.zeng
--2017年4月17日 下午2:48:53
--顺序进入，如果当前子节点返回成功，返回成功，否则进入下一个子节点，全部返回失败才返回失败
local modname = "selector"
--==================global reference=======================

--===================namespace========================
local ns = "org.bevtree"
local using = {--[[namespace1,...]]}
local _ENV = namespace(ns, using)
--===================module property========================
local super = bev_parent	--父类
selector = class("selector", super)
local selector = selector

local BEV_STATE = BEV_STATE

--[[
此外， Selector 上还可以添加‘中断条件’作为终止执行的条件。上图中红框所示就是可选的‘中断条件’。
该‘中断条件’在每处理下一个子节点的时候被检查，当为true时，则不再继续，返回失败（Failure）。
--]]

--===================module content========================

function selector:ctor(setting)
    

    self.m_enter_idx = 0 --目标节点序号
    self.m_cur_idx = 0  --上一节点序号

    super.ctor(self, setting)
end


--//-------∽-★-∽------∽-★-∽--------∽-★-∽数据管理∽-★-∽--------∽-★-∽------∽-★-∽--------//

function selector:__update(input)

    if self.m_cur_bev then

        self.m_cur_bev:update(input)

	    if self.m_cur_bev:done() then

			local state =  self.m_cur_bev:getState()
		    self:set_cur_bev(nil, input)

		    if state == BEV_STATE.SUCCESS then
				--只要一个成功就成功
			    self.m_state = BEV_STATE.SUCCESS
			else

			    while( self.m_cur_idx < self.m_bev_num) do

				    self.m_cur_idx = self.m_cur_idx + 1

				    local bev = self.m_bev_arr[self.m_cur_idx]
				    if bev:can_enter(input) then
					    nslog.debug("选中节点", self.m_cur_idx)
					    self:set_cur_bev(bev, input)
					    break
				    end
				end

			    if not self.m_cur_bev then
				    --没有可进入的节点
				    self.m_state = BEV_STATE.FAIL
			    end
			end
	    end

    else
        --理论上不会有这种情况
        self.m_state = BEV_STATE.FAIL
    end
end

function selector:check_cnd(input)

	local b = false

    self.m_enter_idx = 0

    if self.m_bev_arr then
        local len = #self.m_bev_arr
	    local bev
	    for i=1, len do
		    bev = self.m_bev_arr[i]
		    if bev:can_enter(input) then
			    --能进入
			    self.m_enter_idx = i
			    b = true
			    break
		    end
	    end
    end
    return b
end

--//-------∽-★-∽------∽-★-∽--------∽-★-∽数据操作∽-★-∽--------∽-★-∽------∽-★-∽--------//

function selector:__enter(input)

    if self.m_enter_idx == 0 then
	    nslog.error("没有选中节点")
        return end
	self.m_cur_idx = self.m_enter_idx
	self.m_enter_idx = 0

    local bev = self.m_bev_arr[self.m_cur_idx]
    self:set_cur_bev(bev, input)
end

function selector:__exit(input)

    self.m_enter_idx = 0
    self.m_cur_idx = 0  --上一节点序号
end



return selector