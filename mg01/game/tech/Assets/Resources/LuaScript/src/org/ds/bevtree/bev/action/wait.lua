--行为节点_延时
--@author jr.zeng
--2017年4月17日 上午10:32:49
local modname = "wait"
--==================global reference=======================

--===================namespace========================
local ns = "org.bevtree"
local using = {--[[namespace1,...]]}
local _ENV = namespace(ns, using)
--===================module property========================
local super = bev_base	--父类
wait = class("wait", super)
local wait = wait
local M = wait

--===================module content========================

function wait:ctor(setting)

    self.m_remain_time = 0
	self.m_rand_time = 0

    super.ctor(self, setting)
end

--@setting { time:延时时间(s)
--           rand_min, rand_max:随机时间 }
function wait:setting(setting)

    super.setting(self, setting)

	if setting.rand_max and setting.rand_max > 0 then
		--随机等待时间
		self.m_rand_time = MathUtil.rand_float(setting.rand_min or 0.01, setting.rand_max )
	end

end

--//-------∽-★-∽------∽-★-∽--------∽-★-∽数据管理∽-★-∽--------∽-★-∽------∽-★-∽--------//

function wait:__update(input)

    if self.m_remain_time > 0 then
        self.m_remain_time = self.m_remain_time - input.delta
        if self.m_remain_time <= 0 then
            --时间到
	        self.m_state = BEV_STATE.SUCCESS
        end
    end
end



--//-------∽-★-∽------∽-★-∽--------∽-★-∽数据操作∽-★-∽--------∽-★-∽------∽-★-∽--------//


function wait:__enter(input)

	if self.m_rand_time > 0 then
		self.m_remain_time = self.m_rand_time
	else
		self.m_remain_time = self:get_property('time')
	end

end


return wait