--行为树配置蓝图
--@author jr.zeng
--2017年4月18日 下午4:08:20
local modname = "bev_map"
--==================global reference=======================

--===================namespace========================
local ns = "org.bevtree"
local using = {--[[namespace1,...]]}
local _ENV = namespace(ns, using)
--===================module property========================

--example
--bev_map[0] = function()
--
--    local root = loop({
--        loop = 5,
--        child = seq({
--            childs = {
--                wait({time = 2}),
--                wait({time = 2}),
--                wait({time = 2}),
--            }
--        })  
--    })
--    return root
--end

local BEV_TYPE = BEV_TYPE

--//-------∽-★-∽------∽-★-∽--------∽-★-∽配置生成∽-★-∽--------∽-★-∽------∽-★-∽--------//


--@setting {child, childs, cnd, ...}
function gen_bev_cfg(type, setting, ...)

    setting = setting or {}

    local a = {bev_type = type, setting = setting}

	if setting.child then
		--有子节点
		a.childs = a.childs or {}
		a.childs[#a.childs+1] = setting.child
		--setting.child = nil  --为什么要清掉传入的？
	end

	if setting.childs then
		--log.print_r('setting.childs', setting.childs)
		--有子节点列表
		a.childs = a.childs or {}
		for i=1, #setting.childs do
			a.childs[#a.childs+1] = setting.childs[i]
		end
		--setting.childs = nil    --为什么要清掉传入的？
	end

	local chlids = {...}    --不定参代表子节点列表
	if #chlids > 0 then
		a.childs = a.childs or {}
		for i=1, #chlids do
			a.childs[#a.childs+1] = chlids[i]
		end
	end

	if setting.cnd then
		--有进入条件
		a.cnd = setting.cnd
		--setting.cnd = nil
	end

	if setting.wait then
		a = _seq_({ childs = {a, _wait_({ time=setting.wait }) } })
		--setting.wait = nil
	end

    return a
end

--@setting {cnd, cnds, ...}
function gen_cnd_cfg(type, setting)
    
    assert(type, "miss cnd type")
    
    setting = setting or {}

    local a = {cnd_type = type, setting = setting}
    if setting.cnd then
        --有子条件
        a.cnds = a.cnds or {}
        a.cnds[#a.cnds+1] = a.cnd
        --setting.cnd = nil
    end

    if setting.cnds then  
        --有子条件列表
        a.cnds = a.cnds or {}
        for i=1, #setting.cnds do
            a.cnds[#a.cnds+1] = setting.cnds[i]
        end
        --setting.cnds = nil
    end

    return a
end

local gen_bev_cfg = gen_bev_cfg
local gen_cnd_cfg = gen_cnd_cfg


--//-------∽-★-∽------∽-★-∽--------∽-★-∽cmd∽-★-∽--------∽-★-∽------∽-★-∽--------//

--跳转节点
--@id
function _goto_(setting, id)
	local cfg = {bev_type = BEV_TYPE.GOTO, setting=setting,  id = id}
	return cfg
end


----composite----

--串行_顺序进入，如果有节点失败则返回失败，全部成功才返回成功
function _seq_(setting, ...)
	local cfg = gen_bev_cfg(BEV_TYPE.SEQUENCE, setting, ...)
	return cfg
end

--并行_同时进入，有节点失败则返回失败，全部成功才返回成功
function _paral_(setting, ...)
	local cfg = gen_bev_cfg(BEV_TYPE.PARALLEL, setting, ...)
	return cfg
end

--选择_顺序进入，如果当前子节点返回成功，返回成功，否则进入下一个子节点，全部返回失败才返回失败
--@setting {childs}
function _sel_(setting, ...)
    local cfg = gen_bev_cfg(BEV_TYPE.SELECT, setting, ...)
    return cfg
end

--随机选择
function _rand_sel_(setting, ...)
	local cfg = gen_bev_cfg(BEV_TYPE.RAND_SELECT, setting, ...)
	return cfg
end

--条件选择
function _if_else_(setting, ...)
	local cfg = gen_bev_cfg(BEV_TYPE.IF_ELSE, setting, ...)
	return cfg
end

--等待事件
function _listen_(setting, ...)
	local cfg = gen_bev_cfg(BEV_TYPE.LISTEN, setting, ...)
	return cfg
end

----action----

function _idle_(setting)
	local cfg = gen_bev_cfg(BEV_TYPE.IDLE, setting)
	return cfg
end

--延时节点
--@setting {time(s),
--          rand_min, rand_max:随机时间 }
function _wait_(setting)
	local cfg = gen_bev_cfg(BEV_TYPE.WAIT, setting)
	return cfg
end

--回调节点
--@setting {callback, params}
function _callback_(setting)
	local cfg = gen_bev_cfg(BEV_TYPE.CALLBACK, setting)
	return cfg
end

--动作节点(agent)
--@setting
function _agent_action_(setting)
	local cfg = gen_bev_cfg(BEV_TYPE.AGENT_ACTION, setting)
	return cfg
end


----condition----

--条件节点(agent)
--@setting
function _agent_cond_(setting)
	local cfg = gen_bev_cfg(BEV_TYPE.AGENT_COND, setting)
	return cfg
end

----decorate----

--隐形节点_不会产生任何干扰
function _none_(setting, ...)
	local cfg = gen_bev_cfg(BEV_TYPE.NONE, setting, ...)
	return cfg
end


--循环_循环进入子节点，如果子节点返回失败就中断循环
--@setting {count}
function _loop_(setting, ...)
	local cfg = gen_bev_cfg(BEV_TYPE.LOOP, setting, ...)
	return cfg
end


return _ENV