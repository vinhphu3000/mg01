--Test
--@author jr.zeng
--2017年4月17日 下午7:44:35
local modname = "Test"
--==================global reference=======================

--===================namespace========================
local ns = "org.bevtree.test"
local using = {--[[namespace1,...]]}
local _ENV = namespace(ns, using, modname)
--===================module property========================
local super = nil	--父类
Test = class("Test", super)
local Test = Test
local M = Test

--===================module content========================

function Test:__ctor()

    self.m_running = false

	local map = require_org "ds.bevtree.test.bev_test"
	self.m_factory = new(bev_factory, map)

    self.m_tree = false
    self.m_input = false


    --local cfg = map:get_tree_cfg(1)
    --log.print_t(cfg)

end


function Test:setup()

	self:ctor()

    self:setup_bev_tree("test_1")
    self:start_bev_tree()
end


function Test:clear()
    
    self:clear_bev_tree()
    
end

function Test:setup_event()

end


function Test:clear_event()

end


--//-------∽-★-∽------∽-★-∽--------∽-★-∽数据管理∽-★-∽--------∽-★-∽------∽-★-∽--------//


function Test:update(dt)
    
    self.m_input.delta = dt
    self.m_tree:update(self.m_input)
    if self.m_tree:done() then
        self:stop_bev_tree()
    end
end

--//-------∽-★-∽------∽-★-∽--------∽-★-∽数据操作∽-★-∽--------∽-★-∽------∽-★-∽--------//


function Test:setup_bev_tree(cfg_id)
    
    if self.m_tree then
        return end

    self.m_tree = self.m_factory:gen_bev_tree(cfg_id, {
	    restart_when_done = true,
    })

    self.m_tree:setup()
	self.m_input = self.m_tree:get_input()

end

function Test:clear_bev_tree()
    
    if not self.m_tree then
        return end
    
    self:stop_bev_tree()
    self.m_tree:clear()
    self.m_tree = false
	self.m_input = false
end

function Test:start_bev_tree()
    
    if self.m_running then
	    nslog.print_t("运行中")
        return end
    self.m_running = true
    App:schUpdate(self.update, self)
    
    self.m_tree:enter(self.m_input)
end

function Test:stop_bev_tree()

    if not self.m_running then
        return end
    self.m_running = false
    App:unschUpdate(self.update, self)

    self.m_tree:exit(self.m_input)
end

return Test