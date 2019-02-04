--UnityTestPop
--@author jr.zeng
--2017年10月12日 下午4:02:57
local modname = "UnityTestPop"
--==================global reference=======================

--===================namespace========================
local ns = "test"
local using = {--[[namespace1,...]]}
local _ENV = namespace(ns, using, modname)
--===================module property========================
local super = CCModule	--父类
_ENV[modname] = class(modname, super)
local UnityTestPop = _ENV[modname]

local KeyCode = KeyCode

--===================module content========================

function UnityTestPop:__setup(...)
    
    self:ctor()

    nslog.debug("setup")

	App.popMgr:show(POP_ID.NOTIFY_TIPS)
end

function UnityTestPop:__clear()

end

function UnityTestPop:setup_event()
    
    App.keyboard:attach(KEY_EVENT.RELEASE, self.on_key_press, self)
    
end

function UnityTestPop:clear_event()

    App.keyboard:detach(KEY_EVENT.RELEASE, self.on_key_press, self)
end


--//-------~★~-------~★~-------~★~数据管理~★~-------~★~-------~★~-------//


function UnityTestPop:on_key_press(evt_)

	nslog.print_t("on_key_press", evt_.data)

    local code = evt_.data
    if code == KeyCode.Alpha1 then
        
        App.popMgr:showOrClose(POP_ID.TEST_4)
        --App.popMgr:show(POP_ID.TEST_1)
        
    elseif code == KeyCode.Alpha2 then

        --App.popMgr:showOrClose(POP_ID.TEST_3)

        --local a
        --a.a = 1

    elseif code == KeyCode.Alpha3 then
        
        --nslog.debug("aaa", t2str2(self, 99))
	    App:run_coroutine(self.test_courutine)

	    --a.a = 1

    elseif code == KeyCode.Alpha4 then

    end
     
end


function UnityTestPop.test_courutine()

	nslog.print_t("1")

	co.yield(co.wait_for_seconds(2))

	nslog.print_t("2")

	co.yield(co.wait_for_seconds(3))

	nslog.print_t("3")

	local a = 1
	local function b()
		a = a + 1
		nslog.print_t(a)
		co.yield(co.wait_for_seconds(1))
		return a >= 20
	end

	co.yield(co.wait_until(b))

	nslog.print_t("4")


end

return UnityTestPop