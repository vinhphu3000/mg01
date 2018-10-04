--EvtCenter
--@author jr.zeng
--2017年10月20日 下午5:32:17
local modname = "EvtCenter"
--==================global reference=======================

local luaEvtCenter = mg.org.LuaEvtCenter
local unpack = table.unpack

--===================namespace========================
local ns = "org"
local using = {--[[namespace1,...]]}
local _ENV = namespace(ns, using, modname)
--===================module property========================
local super = CCModule	--父类
EvtCenter = class(modname, super, _ENV)
local EvtCenter = EvtCenter

local Refer = Refer
local clear_tbl = clear_tbl
local safe_call = safe_call


local function getEvtType(type_)
	return type_
end

local function getGoEvtType(go_, type_)
    local instID = go_:GetInstanceID()
    return type_.. "@" .. instID, instID
end

--===================module content========================

local evtArr = {}
local evtType = false
local curIdx = 0

local paramNum = 0
local params = {}

local totalEvtCnt = 0   --总事件数量
local evtCnt = 0        --通用事件数量
local goEvtCnt = 0      --go事件数量

local evt_go = false

function EvtCenter:__ctor()
    
    self.m_notifier = new(CBoard)
	self.m_goNotifier = new(CBoard)


end

function EvtCenter:__setup(...)

    self:ctor()


end

function EvtCenter:__clear()
    
    self:clearAllGoEvt()
    
end

function EvtCenter:setup_event()

end

function EvtCenter:clear_event()

end

--//-------~★~-------~★~-------~★~数据管理~★~-------~★~-------~★~-------//


function EvtCenter:update(dt)

	evtCnt, goEvtCnt = luaEvtCenter.GetEvents(evtArr)

	totalEvtCnt = evtCnt + goEvtCnt
	if totalEvtCnt > 0 then
		--有事件
		--nslog.print_t("evtCnt", goEvtCnt, evtArr)
		curIdx = 1

		if evtCnt > 0 then
			for i=1, evtCnt do
				evtType = evtArr[curIdx]    curIdx = curIdx + 1
				paramNum = evtArr[curIdx]   curIdx = curIdx + 1 --参数数量
				if paramNum > 0 then
					for j=1, paramNum do
						params[j] = evtArr[curIdx]    curIdx = curIdx + 1
					end
					safe_call(self.notifyEvt, self, evtType, unpack(params))
					clear_tbl(params)
				else
					safe_call(self.notifyEvt, self, evtType)
				end
			end
		end

		if goEvtCnt > 0 then
			for i=1, goEvtCnt do
				evtType = evtArr[curIdx]    curIdx = curIdx + 1
				evt_go = evtArr[curIdx]     curIdx = curIdx + 1
				paramNum = evtArr[curIdx]   curIdx = curIdx + 1 --参数数量
				if paramNum > 0 then
					for j=1, paramNum do
						params[j] = evtArr[curIdx]    curIdx = curIdx + 1
					end
					safe_call(self.notifyGoEvt, self, evt_go, evtType, unpack(params))
					clear_tbl(params)
				else
					safe_call(self.notifyGoEvt, self, evt_go, evtType)
				end
				evt_go = false
			end
		end
		clear_tbl(evtArr)
	end

end

--//-------~★~-------~★~-------~★~普通事件相关~★~-------~★~-------~★~-------//

function EvtCenter:notifyEvt( type_, ...)

	--nslog.print_t("notifyEvt", type_, ...)
	local type = getEvtType(type_)
	self.m_notifier:notify(type, ...)
end


--监听事件
function EvtCenter:attachEvt(type_, fun_, target_, refer)

	--nslog.print_t("attachEvt", refer)

	refer = refer or target_
	Refer.assert(refer)    --必须是引用者

	local type = getEvtType(type_)
	self.m_notifier:attach(type, fun_, target_, refer )
end

--移除事件
function EvtCenter:detachEvt(type_, fun_, target_)

	local type = getEvtType(type_)
	local b = self.m_notifier:detach(type, fun_, target_)
	if b then
		nslog.debug("移除监听成功", type)
	else
		nslog.debug("移除监听失败", type)
	end
end


--移除该go指定事件的全部监听
function EvtCenter:detachEvtByType( type_)

	local type = getEvtType(type_)
	local b = self.m_notifier:detachByType(type)
	if b then
		nslog.debug("移除监听成功", type)
	else
		nslog.debug("移除监听失败", type)
	end
end

--//-------~★~-------~★~-------~★~gameobject事件相关~★~-------~★~-------~★~-------//

function EvtCenter:notifyGoEvt(go_, type_, ...)

	--nslog.print_t("notifyGoEvt", go_, type_, ...)
	local type, instID = getGoEvtType(go_, type_)
	self.m_goNotifier:notify(type, go_, ...)
end


--监听go事件
function EvtCenter:attachGoEvt(go_, type_, fun_, target_, refer)

	--nslog.print_t("attachGoEvt", refer)

	refer = refer or target_
    Refer.assert(refer)    --必须是引用者

    local type, instID = getGoEvtType(go_, type_)
    local obs = self.m_goNotifier:attach(type, fun_, target_, refer )
	obs.flag = instID
end

--移除go事件
function EvtCenter:detachGoEvt(go_, type_, fun_, target_)

    local type, instID = getGoEvtType(go_, type_)
    local b = self.m_goNotifier:detach(type, fun_, target_)
    if b then
        nslog.debug("移除监听成功", type)
    else
        nslog.debug("移除监听失败", type)
	    nslog.print_t(self.m_goNotifier:dump())
    end
end

--移除该go指定事件的全部监听
function EvtCenter:detachGoEvtByType(go_, type_)

	local type, instID = getGoEvtType(go_, type_)
	local b = self.m_goNotifier:detachByType(type)
	if b then
		nslog.debug("移除监听成功", type)
	else
		nslog.debug("移除监听失败", type)
		nslog.print_t(self.m_goNotifier:dump())
	end
end

--移除该go的全部监听
function EvtCenter:detachAllGoEvt(go_)

	local instID = go_:GetInstanceID()
	local b = self.m_goNotifier:detachByFlag(instID)
	if b then
		nslog.debug("移除go所有监听", go_.name, instID)
	end
end


function EvtCenter:clearAllGoEvt()

	self.m_goNotifier:detachAll()

end

return EvtCenter