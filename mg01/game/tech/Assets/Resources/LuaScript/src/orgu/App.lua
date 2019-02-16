--AppDelegate
--@author jr.zeng
--2017年9月25日 下午3:08:44
local modname = "App"
--==================global reference=======================

local CCApp = mg.org.CCApp

--===================namespace========================
local ns = "org"
local using = {--[[namespace1,...]]}
local _ENV = namespace(ns, using)
--===================module property========================
local super = nil	--父类
App = class(modname, super)
local App = App

local EVT_TYPE = EVT_TYPE
local ves3_pool

--===================module content========================

--为了效率高点
local __timerMgr = false
local __notifier = false
local __autoRelease = false
local __coMgr = false
local __evtCenter = false

function App:__ctor()

	self.trash = false
	self.trashTrans = false

	--观察者
    __notifier = new(Subject)
    self.notifer = __notifier
    --定时器管理
    __timerMgr = TimerMgr
	--自动释放
    __autoRelease = new(AutoRelease)
    self.autoRelease = __autoRelease
    --键盘
    self.keyboard = new(Keyboard)
	--协程管理
	__coMgr = new(coroutine_mgr)
	--事件中心
	__evtCenter = EvtCenter
	--资源管理
    self.resMgr = false
	--窗口
    self.popMgr = false
    --动作管理
	self.actionMgr = new(action.ActionMgr)

	ves3_pool = Vec3.pool

	nslog.print_t('__ctor')

    self:setting()
end


function App:setting()

	--CCApp.FrameRate = 30
	CCApp.FrameRate = 60
end

function App:setup()

    --log.debug(modname, "os_name", OS_NAME)
    self:ctor()

	if not self.trash then
		self.trash = GameObjUtil.create_gameobject('LuaTrash')
		GameObjUtil.dontDestroyOnLoad(self.trash)
		HelpGo.set_local_pos(self.trash, -1500, -1500, -1500)
		self.trash.isStatic = true
		self.trashTrans = self.trash.transform
	end

	--
	__evtCenter:setup()
	--
    __timerMgr:setup()
    self.keyboard:setup()
	--
	__coMgr:setup()
    --
    self.resMgr = self.resMgr or Inst(org.ResMgr)
    self.resMgr:setup()
	--
    self.popMgr = self.popMgr or new(kui.UIPopMgr)
    self.popMgr:setup()
    --
    self.soundMgr = self.soundMgr or new(org.SoundMgr)
    self.soundMgr:setup()
    --
    self.actionMgr:setup()

    self:setup_event()
end


function App:clear()


	self:clear_event()

	__timerMgr:clear()
	self.keyboard:clear()

	self.resMgr:clear()
	self.popMgr:clear()
	self.actionMgr:clear()

	--action.Action:clear()

	__notifier:detachAll()
	__autoRelease:clear()
	__coMgr:clear()
	__evtCenter:clear()

	Refer.clearNotify()

	if self.trash then
		self.trash, self.trashTrans = destroy(self.trash)
	end
end

function App:setup_event()


end


function App:clear_event()


end



--//-------∽-★-∽------∽-★-∽--------∽-★-∽update∽-★-∽--------∽-★-∽------∽-★-∽--------//


function App:update(dt)

    __notifier:notify(EVT_TYPE.UPDATE, dt)
    __timerMgr:update(dt)

	__coMgr:update(dt)

	__evtCenter:update(dt)
end

function App:lateUpdate(dt)


    ves3_pool:recycleBuzies()   --回收临时Vec3
    
    __autoRelease:excute()
end

function App:attach_update(listener, target, refer)

    __notifier:attach(EVT_TYPE.UPDATE, listener, target, refer)
end

function App:detach_update(listener, target)

    __notifier:detach(EVT_TYPE.UPDATE, listener, target)
end

--//-------∽-★-∽------∽-★-∽--------∽-★-∽观察者∽-★-∽--------∽-★-∽------∽-★-∽--------//

function App:attach(name, func, target, refer)
    __notifier:attach(name, func, target, refer)
end

--移除观察者
function App:detach(name, func, target)
    __notifier:detach(name, func, target)
end


function App:notify(type, data)
    __notifier:notify(type, data)
end

--用事件派发
function App:notifyWithEvent(type, data)
    __notifier:notifyWithEvent(type, data)
end

--//-------∽-★-∽------∽-★-∽--------∽-★-∽协程∽-★-∽--------∽-★-∽------∽-★-∽--------//

--运行协程
--return co_id
function App:run_coroutine(fun)
	return __coMgr:run(fun)
end

--停止协程
--@co_id
function App:stop_coroutine(co_id)
	return __coMgr:stop(co_id)
end



return App