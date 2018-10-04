-- 列表项_基类
--@author jr.zeng
--@date 2018/10/4  11:47
local modname = "ListViewItem"
--==================global reference=======================

--===================namespace========================
local ns = 'org.kui'
local using = {--[[namespace1,...]]}
local _ENV = namespace(ns, using, modname)

--===================module property========================
local super = UIAbs	--父类
ListViewItem = class(modname, super, _ENV)
local ListViewItem = ListViewItem

--===================module content========================

function ListViewItem:__ctor(go)

	self.index = 0
	self:showGameObjectEx(go)


end


function ListViewItem:__show(...)


end

function ListViewItem:setup_event()


end

function ListViewItem:clear_event()

end

function ListViewItem:__destroy()

end



return ListViewItem