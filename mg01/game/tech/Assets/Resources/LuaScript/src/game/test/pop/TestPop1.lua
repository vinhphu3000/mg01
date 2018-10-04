--TestPop1
--@author jr.zeng
--2017年10月16日 上午11:40:33
local modname = "TestPop1"
--==================global reference=======================

--===================namespace========================
local ns = "test"
local using = {"org.kui"}
local _ENV = namespace(ns, using, modname)
--===================module property========================
local super = UIPop	--父类
_ENV[modname] = class(modname, super)
local TestPop1 = _ENV[modname]

--===================module content========================

TestPop1.pop_id = POP_ID.TEST_1

function TestPop1:__ctor()
    
    
    
end

function TestPop1:__show(showObj, ...)

end

function TestPop1:setup_event()

end

function TestPop1:clear_event()

end

function TestPop1:__destroy()

end


return TestPop1