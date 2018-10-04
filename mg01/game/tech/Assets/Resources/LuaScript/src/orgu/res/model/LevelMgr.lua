-- LevelMgr
--@author jr.zeng
--@date 2018/9/13  16:11
local modname = "LevelMgr"
--==================global reference=======================

local csLevelMgr = mg.org.LevelMgr.me

local safe_call = safe_call

--===================namespace========================
local ns = 'org'
local using = {--[[namespace1,...]]}
local _ENV = namespace(ns, using, modname)

--===================module property========================
local super = CCModule	--父类
LevelMgr = class(modname, super, _ENV)
local LevelMgr = LevelMgr


--===================module content========================

function LevelMgr:__ctor()

	self.m_notifier = new(Subject)  --独立派发器

end

function LevelMgr:__setup(...)


end

function LevelMgr:__clear()


end

function LevelMgr:setup_event()

	EvtCenter:attachEvt(RES_EVT.LOAD_LEVEL_COMPLETE, self.on_load_complete, self)
	EvtCenter:attachEvt(RES_EVT.LOAD_LEVEL_EXCEPTION, self.on_load_exception, self)
end

function LevelMgr:clear_event()

	EvtCenter:detachEvt(RES_EVT.LOAD_LEVEL_COMPLETE, self.on_load_complete, self)
	EvtCenter:detachEvt(RES_EVT.LOAD_LEVEL_EXCEPTION, self.on_load_exception, self)

end


--//-------∽-★-∽------∽-★-∽--------∽-★-∽数据管理∽-★-∽--------∽-★-∽------∽-★-∽--------//

--加载完成处理
function LevelMgr:on_load_complete(levelData)

	local url = levelData.url

	nslog.print_t('on_load_complete', url, levelData)
	--nslog.print_t(self.m_notifier:dump())

	local evtType = url
	--safe_call(self.notify, self, evtType, asset, url)
	self:notify(evtType, url)

	self:detachByType(url) --移除所有监听
end

function LevelMgr:on_load_exception(url)

	local evtType = url
	self:detachByType(evtType)  --移除所有监听

end

--//-------∽-★-∽------∽-★-∽--------∽-★-∽数据操作∽-★-∽--------∽-★-∽------∽-★-∽--------//


--同步加载
function LevelMgr:load_sync(url, isAdditive)

	local levelData = csLevelMgr:LoadSync(url, isAdditive)
	return levelData
end

--异步加载
function LevelMgr:load_async(url, isAdditive, on_complete, target, refer)

	url = string.lower(url)
	isAdditive = isAdditive or false

	refer = refer or target
	--local referId = Refer.format(refer,false)

	if csLevelMgr:IsLevelLoaded(url) then
		if on_complete then
			safe_call(on_complete, refer, url)
		end
		return false
	end

	local evtType = url
	self:attach(evtType, on_complete, target, refer)

	local levelData = csLevelMgr:LoadAsync(url, isAdditive, nil, nil)
	return levelData
end

return LevelMgr