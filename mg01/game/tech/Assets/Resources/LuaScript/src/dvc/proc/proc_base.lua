-- proc_base
--@author jr.zeng
--@date 2018/8/4  11:44
local modname = "proc_base"
--==================global reference=======================

--===================namespace========================
local ns = 'dvc.proc'
local using = {'org.bevtree'}
local _ENV = namespace(ns, using, modname)

--===================module property========================
local super = bev_base	--父类
proc_base = class(modname, super, _ENV)
local proc_base = proc_base

--===================module content========================

function proc_base:__ctor()



end


function proc_base:__setup(...)

	super.__setup(self, ...)

end

function proc_base:__clear()

	super.__clear(self)


end


--//-------∽-★-∽------∽-★-∽--------∽-★-∽数据管理∽-★-∽--------∽-★-∽------∽-★-∽--------//


function proc_base:__update(input)

end



--//-------∽-★-∽------∽-★-∽--------∽-★-∽数据操作∽-★-∽--------∽-★-∽------∽-★-∽--------//

function proc_base:__enter(input)

end

function proc_base:__exit(input)

end


return proc_base