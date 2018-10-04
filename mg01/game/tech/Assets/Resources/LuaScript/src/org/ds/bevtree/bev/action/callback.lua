--行为节点_回调节点
--@author jr.zeng
--2017年4月18日 下午5:02:41
local modname = "callback"
--==================global reference=======================

--===================namespace========================
local ns = "org.bevtree"
local using = {--[[namespace1,...]]}
local _ENV = namespace(ns, using)
--===================module property========================
local super = bev_base	--父类
callback = class(modname, super)
local callback = callback

local BEV_STATE = BEV_STATE
local result2bevState = bev_util.result2bevState

--===================module content========================

function callback:ctor(setting)

    self.m_callback = false
    self.m_param = false

    super.ctor(self, setting)

end


--@setting {callback, params}
function callback:setting(setting)

    super.setting(self, setting)
    
    self.m_callback = setting.callback  --由于是构造时传进来, 因此这个节点不应该被回收利用
    self.m_param = setting.param
end

function callback:__update(input)

	self.m_state = self:excute_method()

end


function callback:excute_method()

	local result, err = self.m_callback(self.m_param)
	local state = result2bevState(result)
	return state
end


return callback