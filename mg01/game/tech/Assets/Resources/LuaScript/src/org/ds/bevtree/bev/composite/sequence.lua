--组合节点_串行节点
--@author jr.zeng
--2017年4月17日 下午5:02:06
--顺序进入，如果有节点失败则返回失败，全部成功才返回成功
local modname = "sequence"
--==================global reference=======================

--===================namespace========================
local ns = "org.bevtree"
local using = {--[[namespace1,...]]}
local _ENV = namespace(ns, using, modname)
--===================module property========================
local super = bev_parent	--父类
sequence = class("sequence", super)
local sequence = sequence


--[[TODO
此外， Sequence 上还可以添加‘中断条件’作为终止执行的条件。上图中红框所示就是可选的‘中断条件’。
该‘中断条件’在每处理下一个子节点的时候被检查，当为true时，则不再继续，返回失败（Failure）。
-]]


--===================module content========================

function sequence:ctor(setting)

    self.m_cur_idx = 1

    super.ctor(self, setting)
end


--//-------∽-★-∽------∽-★-∽--------∽-★-∽数据管理∽-★-∽--------∽-★-∽------∽-★-∽--------//

function sequence:__update(input)

    if self.m_cur_bev then

        self.m_cur_bev:update(input)

        if self.m_cur_bev:done() then
            --节点已完成
            if self.m_cur_bev:success() then
                
                self:set_cur_bev(nil, input)
                self.m_cur_idx = self.m_cur_idx + 1
                if self.m_cur_idx > self.m_bev_num then
                    --队列已完成
                    self.m_state = BEV_STATE.SUCCESS
                else
                    --nslog.debug( "next", self.m_cur_idx .. "/" .. self.m_bev_num)
                    local bev = self.m_bev_arr[self.m_cur_idx]
                    if bev:can_enter(input) then
                        self:set_cur_bev(bev, input)
                    else
                        --进入失败
                        self.m_state = BEV_STATE.FAIL
                    end
                end
            else
                --失败
                self.m_state = BEV_STATE.FAIL
            end
        end
    else
        self.m_state = BEV_STATE.FAIL
    end
end

function sequence:check_cnd(input)

	--只需要第一个节点能进入
	local bev = self:get_fore_bev()
	return bev:can_enter(input)
end


--//-------∽-★-∽------∽-★-∽--------∽-★-∽数据操作∽-★-∽--------∽-★-∽------∽-★-∽--------//

function sequence:__enter(input)

    local bev = self:get_fore_bev()
    self:set_cur_bev(bev, input)
    self.m_cur_idx = 1
end

function sequence:__exit(input)
    
    self.m_cur_idx = 1
end

return sequence