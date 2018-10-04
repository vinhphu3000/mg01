--UnityTest
--@author jr.zeng
--2017年9月21日 下午2:17:17
local modname = "UnityTest"
--==================global reference=======================

--===================namespace========================
local ns = "test"
local using = {--[[namespace1,...]]}
local _ENV = namespace(ns, using, modname)
--===================module property========================
local super = nil	--父类
_ENV[modname] = class(modname, super)
local UnityTest = _ENV[modname]

--===================module content========================

function UnityTest:__ctor()

end

function UnityTest:setup()

    --self:test_gameobject()
    --self:test_sluacustom()
    --self:test_load_res()
    self:test_pop()
    
    self:setup_event()
    
end

function UnityTest:clear()

end

function UnityTest:setup_event()

end

function UnityTest:clear_event()

end

--//-------~★~-------~★~-------~★~数据管理~★~-------~★~-------~★~-------//

--//-------∽-★-∽------∽-★-∽--------∽-★-∽test_gameobject∽-★-∽--------∽-★-∽------∽-★-∽--------//

function UnityTest:test_gameobject()

    local empty = Unity.GameObject("HHEmptyObject")
    print(empty)
    local trans = empty.transform
    local pos = trans.position
    nslog.print_r(pos, "pos")

    --pos.x = 10
    trans.position = {10,10,10}
    local pos1 = Vec3( trans:GetLocalPos_() )
    nslog.print_t(modname, "GetLocalPos_", pos1)
    pos1 = pos1 + Vec3(10, 10, 10) 
    local tmp = pos1 + Vec3(10, 10, 10) 
    tmp = tmp + Vec3(10, 10, 10) 
    tmp = tmp + Vec3.left
    trans:SetLocalPos_( tmp:get() )
    
    local cameraGo = Unity.GameObject.Find("UILayer/UICamera")
    local path = cameraGo:GetHierarchy_()
    nslog.debug(modname, path)
    
    local camera = cameraGo:GetComponent("Camera")
    
--    local camera
--    local Unity = Unity
--    for i=1, 10 do
--        camera = cameraGo:GetComponent("Camera")
--        --camera = cameraGo:GetComponent(Unity.Camera)
--    end
    print(camera)
    
    
end


--//-------∽-★-∽------∽-★-∽--------∽-★-∽test_sluacustom∽-★-∽--------∽-★-∽------∽-★-∽--------//

function UnityTest:test_sluacustom()
    
    do return end
    
    nslog.debug(modname, mg.SluaCustom.test_statc_str)
    local me = mg.SluaCustom.me
    nslog.print_t(modname, "me is", me)
    
    local custom = mg.SluaCustom()
    
    nslog.print_t(modname, "custom is", custom)
    
    --字典
    local dic = custom.test_dic -- userdata,没事别用
    nslog.print_t(modname, dic, dic["bbb"])
    --枚举 
    custom.test_enum = 2
    nslog.print_t(modname, "test_enum", custom.test_enum)
    --数组
    local go_arr = custom:test_goArr() -- userdata,没事别用
    nslog.print_t(modname, "go_arr", go_arr)
    --table互传   没事别用
    local go_arr = custom:test_goArrTable()
    nslog.print_t(modname, "go_arr", go_arr)
    custom:test_setGoArrTable(go_arr)
    --函数重载
    mg.SluaCustom.test_overload_func(go_arr[1])
    mg.SluaCustom.test_overload_func(Unity.Camera)
    --不定参
    custom:test_variant_func("test_variant_func", go_arr[2], "bbb", go_arr[3])
    
    
    --回调
    --Delegate
    local function callback0()
        nslog.debug(modname, "callback0")
    end
    
    local function callback1(val)
        --nslog.print_t("callback1", val)
    end
    
    local function callback2(val, val2)
        nslog.print_t(val, val2)
    end
    
    custom.callback_0 = {"+=", callback0}
    custom.callback_1 = {"+=", callback1}
    custom:Callback0()
    
    local num = 10000
    nslog.debug(modname, "Callback1", "start", num)
    for i=1, num do
        custom:Callback1(go_arr[4])
    end
    nslog.debug(modname, "Callback1", "finish")  --10000次大约20ms
    
    custom.callback_0 = {"-=", callback0}
    custom.callback_1 = nil
    custom:Callback0()
    custom:Callback1()

    --Action与 Func
    local function action1(i, go)

        nslog.print_t(i, go)
    end
    
    local function func1(i, go)

        --nslog.print_t(i, go)
        return go
    end
    
    custom.action_1 = {"+=", action1}
    custom.func_1 = {"+=", func1}
    custom:CallActon1(999, go_arr[2])

    local go 
    nslog.debug(modname, "func1", "start", num)
    for i=1, num do
         go = custom:CallFunc1(888, go_arr[1])
    end
    nslog.print_t("func1 result is", go)     --10000次大约20ms
    
    custom.action_1 = {"-=", action1}
    custom.func_1 = nil
    custom:CallActon1(999, go_arr[2])
    custom:CallFunc1(888, go_arr[1])
    
    --KComponentEvent
    local onClick = custom.onClick
    nslog.print_t("onClick is", onClick)
    
    local function on_click(go)
        --nslog.print_t("on_click", go)
    end
    onClick:AddListener(on_click)
    nslog.debug(modname, "CallOnClick", "start", num)
    for i=1, num do
        custom:CallOnClick(777, go_arr[3])
    end
    nslog.debug(modname, "CallOnClick", "finish")   --10000次大约20ms
    
    onClick:RemoveListener(on_click)
    custom:CallOnClick(666, go_arr[4])
    
    local function back(i)
        nslog.print_t("test callback1 is", i)
    end
    custom:CallCALLBACK_1(6543, go_arr[2], back)
end


--//-------∽-★-∽------∽-★-∽--------∽-★-∽test_load_res∽-★-∽--------∽-★-∽------∽-★-∽--------//

function UnityTest:test_load_res()

    --do return end

    nslog.print_t("SluaCustom", mg.SluaCustom)
    nslog.print_t("AssetCache", mg.org.AssetCache)
    local assetCache = mg.org.AssetCache.me
    nslog.print_t("assetCache", assetCache)
    
    local url = "GUI/cn/Prefab/Canvas_Announce"
    
    local prefab = assetCache:LoadSync(modname, url)
    nslog.print_t(url, prefab)

    local url = "GUI/cn/Prefab/Canvas_Test1"
    local function back(assetData)
        nslog.print_t(url, assetData.asset)
    end
    local assetData = assetCache:LoadAsync(url, back, modname)

    local url = "GUI/cn/Prefab/Canvas_Test2"
    local assetData = AssetCache:load_async(url, self.on_load_back, self)
    
    nslog.debug("asasas")
    nslog.print_upv()
    
    log_trace("打印出来啦")
end
    
function UnityTest:on_load_back(asset, url)
    
    nslog.print_t(url, asset)
    
    local assetData = AssetCache:load_async( url, self.on_load_back2, self)
end

function UnityTest:on_load_back2(asset, url)

    nslog.print_t("on_load_back2", url, asset)
end

--//-------∽-★-∽------∽-★-∽--------∽-★-∽test_pop∽-★-∽--------∽-★-∽------∽-★-∽--------//

function UnityTest:test_pop()
    
--    local view = new(ImageAbs)
--    view:showGameObject("GUI/cn/Prefab/Canvas_Test3")
--    view:retain(self)
    
    local url = "GUI/cn/Prefab/Canvas_Test2"
    local assetData = GameObjCache:load_async(url, self.on_go_loaded, self)

end

function UnityTest:on_go_loaded(go_, url_)

    nslog.print_t("on_go_loaded", go_, url_)
end
    
return UnityTest