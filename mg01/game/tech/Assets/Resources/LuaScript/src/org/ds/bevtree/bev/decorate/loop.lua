--逻辑节点_循环
--@author jr.zeng
--2017年4月17日 上午11:55:54
--循环进入子节点，如果子节点返回失败就中断循环
local modname = "loop"
--==================global reference=======================

--===================namespace========================
local ns = "org.bevtree"
local using = {--[[namespace1,...]]}
local _ENV = namespace(ns, using)
--===================module property========================
local super = bev_parent	--父类
loop = class("loop", super)
local loop = loop

--===================module content========================

function loop:ctor(setting)

	self.m_count = 0

    super.ctor(self, setting)
end

--@setting {loop(循环次数) }
function loop:setting(setting)

    super.setting(self, setting)

end

--//-------∽-★-∽------∽-★-∽--------∽-★-∽数据管理∽-★-∽--------∽-★-∽------∽-★-∽--------//

function loop:__update(input)

    if self.m_cur_bev then

        self.m_cur_bev:update(input)
        
        if self.m_cur_bev:done() then
            --已完成
            if self.m_cur_bev:success() then
                --成功
                --nslog.debug(modname, "loop", self.m_count)
                self.m_count = self.m_count - 1
                if self.m_count == 0 then
                    --达到循环次数
                    self:set_cur_bev(nil, input)
                    self.m_state = BEV_STATE.SUCCESS
                else
                    --重新进入
                    self.m_cur_bev:exit(input)
                    if self.m_cur_bev:can_enter(input) then
                        self.m_cur_bev:enter(input)
                    else
                        --进入失败
                        self:set_cur_bev(nil, input)
                        self.m_state = BEV_STATE.FAIL
                    end
                end
            else
                --失败
                self:set_cur_bev(nil, input)
                self.m_state = BEV_STATE.FAIL
            end
        end
    else

        self.m_state = BEV_STATE.FAIL
    end
end

--//-------∽-★-∽------∽-★-∽--------∽-★-∽数据操作∽-★-∽--------∽-★-∽------∽-★-∽--------//

--检测进入条件
function loop:check_cnd(input)

    local bev = self:get_fore_bev()
    if bev then
        return bev:can_enter(input)
    end
    return false
end

function loop:__enter(input)

    self.m_count = self:get_property('count')

    local bev = self:get_fore_bev()
    if bev then
        self:set_cur_bev(bev, input)
    end
end



return loop