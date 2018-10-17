-- proc_ply_place_card
--@author jr.zeng
--@date 2018/10/17  20:22
local modname = "proc_ply_place_card"
--==================global reference=======================

--===================namespace========================
local ns = 'dvc.proc'
local using = {'org.bevtree'}
local _ENV = namespace(ns, using, modname)

--===================module property========================
local super = proc_base	--父类
proc_ply_place_card = class(modname, super, _ENV)
local proc_ply_place_card = proc_ply_place_card

--===================module content========================

function proc_ply_place_card:__ctor()

end


--//-------∽-★-∽------∽-★-∽--------∽-★-∽数据管理∽-★-∽--------∽-★-∽------∽-★-∽--------//

function proc_ply_place_card:__update(input)

	self.m_state = BEV_STATE.SUCCESS

end


--//-------∽-★-∽------∽-★-∽--------∽-★-∽数据操作∽-★-∽--------∽-★-∽------∽-★-∽--------//

function proc_ply_place_card:__enter(input)


end

function proc_ply_place_card:__exit(input)



end

return proc_ply_place_card