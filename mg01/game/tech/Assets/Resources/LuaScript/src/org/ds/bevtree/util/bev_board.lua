--行为树白板
--@author jr.zeng
--2017年4月17日 下午8:33:51
local modname = "bev_board"
--==================global reference=======================

--===================namespace========================
local ns = "org.bevtree"
local using = {--[[namespace1,...]]}
local _ENV = namespace(ns, using)
--===================module property========================
local super = nil	--父类
bev_board = class("bev_board", super)
local bev_board = bev_board
local M = bev_board

--===================module content========================

function bev_board:__ctor(setting)

    self:setting(setting)

	--流逝时间
	self.delta = 0
end

function bev_board:setting(setting)
    

end

function bev_board:setup(...)

end

function bev_board:clear()

	self.delta = 0
end

--function bev_board:get_factory()
--    return self.m_factory
--end

--//-------∽-★-∽------∽-★-∽--------∽-★-∽数据管理∽-★-∽--------∽-★-∽------∽-★-∽--------//

--//-------∽-★-∽------∽-★-∽--------∽-★-∽数据操作∽-★-∽--------∽-★-∽------∽-★-∽--------//

return bev_board