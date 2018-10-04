--gameobject缓存
--@author jr.zeng
--2017年11月9日 下午8:15:39
local modname = "GameObjCache"
--==================global reference=======================

local csAssetCache = mg.org.AssetCache.me
local csGameObjCache = mg.org.GameObjCache.me

local type = type
local safe_call = safe_call

--===================namespace========================
local ns = "org"
local using = {--[[namespace1,...]]}
local _ENV = namespace(ns, using, modname)
--===================module property========================
local super = CCModule	--父类
GameObjCache = class(modname, super, _ENV)
local GameObjCache = GameObjCache
local self = GameObjCache


local Refer = Refer
local assetCache = AssetCache


--===================module content========================

function GameObjCache:__ctor()

    self.m_notifier = new(Subject)  --独立派发器
    
end

function GameObjCache:__setup()

    self:ctor()


end

function GameObjCache:__clear()

end

function GameObjCache:setup_event()

end

function GameObjCache:clear_event()


end


--//-------~★~-------~★~-------~★~GameObject加载~★~-------~★~-------~★~-------//


function GameObjCache:load_sync(url)

    local go = csGameObjCache:LoadSync(url)
    return go
end

function GameObjCache:load_async(url, on_complete, target, refer)

    if type(on_complete) ~= "function" then
        --不能没有回调
        return end

	url = string.lower(url)

	refer = refer or target
    Refer.assert(refer)    --这里纯粹为了回调

    if csAssetCache:HasAsset(url) then
        local go = csGameObjCache:CreateGo(url, nil)
        safe_call(on_complete, target, go, url)
	    --on_complete(refer, go, url)
        return
    end
    
    local evtType = url
    self:attach(evtType, on_complete, target, refer)     --自己监听加载完成事件
    
    assetCache:load_async(url, self.on_go_loaded, nil)         --注意:这里没有传引用者,因此在加载完成之前,这资源未被持有
end

--移除go加载监听
function GameObjCache:detach_load(url, target)

	url = string.lower(url)
    local evtType = url
    self:detachByType2(evtType, target)
end

--@url 已经是小写路径
function GameObjCache.on_go_loaded(prefab, url)

	--nslog.print_t('on_go_loaded', prefab, url)

    local function back(tar, fn)
        local go = csGameObjCache:CreateGo(url, nil) --创建go
        --safe_call(fn, tar, go, url)
	    fn( tar, go, url )
    end

    local evtType = url
    self.m_notifier:notifyEach(evtType, back)
    self:detachByType(evtType)  --移除所有监听
end


return GameObjCache