--行为树工厂
--@author jr.zeng
--2017年4月18日 下午2:03:39
local modname = "bev_factory"
--==================global reference=======================

local unpack = table.unpack

--===================namespace========================
local ns = "org.bevtree"
local using = {--[[namespace1,...]]}
local _ENV = namespace(ns, using)
--===================module property========================
local super = nil	--父类
bev_factory = class("bev_factory", super)
local bev_factory = bev_factory
local M = bev_factory

--===================module content========================

function bev_factory:__ctor(bev_map, bevc_path)
    
    self.m_bev_map = bev_map or _ENV  --树配置蓝图(其实是个ns)
	self.m_bevc_path = bevc_path or false   --Dehavior配置文件路径

	self.m_id2cfg = {}


end

function bev_factory:setup()

end

function bev_factory:clear()

end


--获取指定节点的类
function bev_factory:get_class(type)
    local class = self.m_bev_map[type]
    if class == nil then
        assert(false, "miss class " .. type)
    end
    return class
end

--获取配置
function bev_factory:get_tree_cfg(cfg_id)

	local cfg = self.m_id2cfg[cfg_id]
	if cfg then
		return cfg
	end

	local tp

	local map = self.m_bev_map[cfg_id]
	if map then
		tp = type(map)
		if tp == "function" then
			cfg = map()     --用函数生成配置
			tp = type(cfg)
		else
			cfg = map
		end
	else
		if self.m_bevc_path then
			--有配置文件路径
			local lua_path = self.m_bevc_path .. '.' .. cfg_id
			local tbl = require(lua_path)
			cfg = self:parse_bevc_cfg(tbl)
			tp = 'table'
		end
	end

	if tp ~= "table" then
		assert(false, "配置类型不正确 " .. cfg_id)
	end
	--nslog.print_t('get_tree_cfg', cfg)
	self.m_id2cfg[cfg_id] = cfg
	return cfg
end

--//-------∽-★-∽------∽-★-∽--------∽-★-∽生成相关∽-★-∽--------∽-★-∽------∽-★-∽--------//


--创建行为树
function bev_factory:gen_bev_tree(cfg_id, setting)
    
    local bev = self:build_bev_by_id(cfg_id)
    local tree = new(bev_tree, setting)
    tree.cfg_id = cfg_id
    tree:add_bev(bev)
	--nslog.print_t("gen_bev_tree", tostring(bev), tostring(tree) )
    return tree
end

--根据id创建行为
function bev_factory:build_bev_by_id(cfg_id)

	local cfg = self:get_tree_cfg(cfg_id)
	local bev = self:__build_bev(cfg)
	return bev
end


--构建行为
function bev_factory:__build_bev(cfg)
	--nslog.print_t('__build_bev', cfg)
	local bev

	local type = type(cfg)
	if type == 'function' then
		cfg = cfg() --这种用法每次都会调用，不会缓存
		--nslog.print_r('function', cfg)
	elseif type == 'string' then
		--配置id
		cfg = self:get_tree_cfg(cfg)
	end

	if cfg.bev_type == BEV_TYPE.GOTO then

		bev = self:__build_bev_by_goto(cfg)
	else

		bev = new(self:get_class(cfg.bev_type), cfg.setting)
		if cfg.cnd then
			--有进入条件
			local cnd = self:__build_cnd(cfg.cnd)
			bev:set_cnd(cnd)
		end

		if cfg.childs then
			--有子节点
			local child
			for i=1, #cfg.childs do
				child = self:__build_bev( cfg.childs[i] )
				bev:add_bev(child)
			end
		end
	end
	return bev
end

--根据配置id构建行为
function bev_factory:__build_bev_by_goto(cfg)

    local to_cfg = self:get_tree_cfg(cfg.id)
    if to_cfg == nil then
        assert(false, "__build_bev_by_goto miss: " .. cfg.id)
        return nil
    end
    
    local new_cfg = to_cfg
    if cfg.cnd then
        --有条件
        new_cfg = map.seq({child = to_cfg})  --包一层
        new_cfg.cnd = cfg.cnd

--        new_cfg = clone(to_cfg)   --复制一个
--        if new_cfg.cnd then
--            new_cfg.cnd = __and({cnds = {cfg.cnd, new_cfg.cnd} })
--        else
--            new_cfg.cnd = cfg.cnd
--        end
    end
    
    local bev = self:__build_bev(new_cfg)
    return bev
end


--构建条件
function bev_factory:__build_cnd(cfg)

    local cnd = new(self:get_class(cfg.cnd_type), cfg.setting)
    
    if cfg.cnds then
        --有子条件
        local child
        for i=1, #cfg.cnds do
            child = self:__build_cnd(cfg.cnds[i])
            cnd:addCnd(child)
        end
    end
    return cnd
end


--//-------∽-★-∽------∽-★-∽--------∽-★-∽BehaviacDesigner相关∽-★-∽--------∽-★-∽------∽-★-∽--------//

function bev_factory:parse_bevc_cfg(tbl)

	local behavior = tbl.behavior
	local name = behavior.name                  --行为树名称
	local agentType = behavior.agentType        --agent类型
	local version = behavior.version            --版本号

	local node = behavior.node[1]
	local cfg = self:parse_bevc_node(node)
	return cfg
end

--解析node
function bev_factory:parse_bevc_node(node)

	if node.Weight then
		--有权重, 使用第一个子节点
		assert(node.node)
		node = node.node[1]
		node.weight = node.Weight
	end

	local class = node.CustomClass or node.class
	local cmd = self.m_bev_map[class]   --指向bev_map里的cmd
	if not cmd then
		cmd = self.m_bev_map['_none_']  --找不到则使用none节点替代
	end

	local chlids = false

	if node.node and #node.node > 0 then
		chlids = {}
		for i, childNode in ipairs(node.node ) do
			chlids[#chlids+1] = self:parse_bevc_node(childNode)
		end
	end

	local cfg
	if chlids then
		cfg = cmd(node, unpack(chlids)) --node作为setting
	else
		cfg = cmd(node)
	end

	return cfg
end


return bev_factory