--AssetCache
--@author jr.zeng
--2017年9月29日 下午4:24:09
local modname = "AssetCache"
--==================global reference=======================

local type = type
local safe_call = safe_call

local csAssetCache = mg.org.AssetCache.me

--===================namespace========================
local ns = "org"
local using = {--[[namespace1,...]]}
local _ENV = namespace(ns, using)
--===================module property========================
local super = CCModule	--父类
AssetCache = class(modname, super)
local AssetCache = AssetCache

local Refer = Refer


--===================module content========================

function AssetCache:__ctor()
    
    self.m_notifier = new(Subject)  --独立派发器
end

function AssetCache:__setup()
    
    self:ctor()
    
    
end

function AssetCache:__clear()

end

function AssetCache:setup_event()
    
	EvtCenter:attachEvt(RES_EVT.LOAD_COMPLETE, self.on_load_complete, self)
	EvtCenter:attachEvt(RES_EVT.LOAD_EXCEPTION, self.on_load_exception, self)
    
end

function AssetCache:clear_event()

	EvtCenter:detachEvt(RES_EVT.LOAD_COMPLETE, self.on_load_complete, self)
	EvtCenter:detachEvt(RES_EVT.LOAD_EXCEPTION, self.on_load_exception, self)
end

--//-------~★~-------~★~-------~★~数据管理~★~-------~★~-------~★~-------//

--加载完成处理
function AssetCache:on_load_complete(assetData)

    local url = assetData.url
    local asset = assetData.asset

	--nslog.print_t('on_load_complete', url, asset)
	--nslog.print_t(self.m_notifier:dump())

	local evtType = url
    --safe_call(self.notify, self, evtType, asset, url)
	self:notify(evtType, asset, url)

    self:detachByType(url) --移除所有监听
end

--加载异常处理
function AssetCache:on_load_exception(url)

    local evtType = url
    self:detachByType(evtType)  --移除所有监听
    --
    GameObjCache:detachByType(evtType)
end


--//-------~★~-------~★~-------~★~数据操作~★~-------~★~-------~★~-------//

--释放资源引用
--@url 缺省则释放refer的所有引用
function AssetCache:release_asset(refer_, url)

    local referId = Refer.format(refer_, false)
    if url then
        csAssetCache:ReleaseByUrl(referId, url)
    else
        csAssetCache:ReleaseByRefer(referId)
    end
end

--卸载指定资源
function AssetCache:unload_asset(url)
    
    csAssetCache:UnloadAsset(url)
end


--//-------~★~-------~★~-------~★~资源加载~★~-------~★~-------~★~-------//

--同步加载
--@refer 直接传到c#的AssetCache
function AssetCache:load_sync(url, refer_)

    local referId = Refer.format(refer_,false)
    local asset = csAssetCache:LoadSync(refer_, url)
    return asset
end

--异步加载
--@refer 直接传到c#的AssetCache
function AssetCache:load_async(url, on_complete, target, refer)

	url = string.lower(url)

	refer = refer or target
    local referId = Refer.format(refer,false)
    
    local asset = csAssetCache:GetAsset(url)
    if asset then
        if referId then
            csAssetCache:RetainByUrl(referId, url)  --持有引用
        end
        if on_complete then
            safe_call(on_complete, refer, asset, url)
        end
        return false
    end

    local evtType = url
    self:attach(evtType, on_complete, target, refer)
    
    local assetData = csAssetCache:LoadAsync(url, nil, referId)
    return assetData
end

--移除加载回调
function AssetCache:detach_async( url, on_complete, target)

	url = string.lower(url)
    local evtType = url
    self:detach(evtType, on_complete, target)
end


function AssetCache:has_asset( url)
	return csAssetCache:HasAsset(url)
end

return AssetCache