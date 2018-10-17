-- proc_cpu_draw_card
--@author jr.zeng
--@date 2018/8/9  20:54
local modname = "proc_cpu_draw_card"
--==================global reference=======================

--===================namespace========================
local ns = 'dvc.proc'
local using = {'org.bevtree'}
local _ENV = namespace(ns, using, modname)

--===================module property========================
local super = proc_base	--父类
proc_cpu_draw_card = class(modname, super, _ENV)
local proc_cpu_draw_card = proc_cpu_draw_card

--===================module content========================

function proc_cpu_draw_card:__ctor()


end




--//-------∽-★-∽------∽-★-∽--------∽-★-∽数据管理∽-★-∽--------∽-★-∽------∽-★-∽--------//

function proc_cpu_draw_card:__update(input)

	self.m_state = BEV_STATE.SUCCESS

end


--//-------∽-★-∽------∽-★-∽--------∽-★-∽数据操作∽-★-∽--------∽-★-∽------∽-★-∽--------//

function proc_cpu_draw_card:__enter(input)

	stage_deck:draw_rand_card(self.m_agent)

end

function proc_cpu_draw_card:__exit(input)



end


return proc_cpu_draw_card