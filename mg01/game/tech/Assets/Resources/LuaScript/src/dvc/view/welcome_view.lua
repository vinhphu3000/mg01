-- welcome_
--@author jr.zeng
--@date 2018/7/14  16:06
local modname = "welcome_view"
--==================global reference=======================

--===================namespace========================
local ns = 'dvc'
local using = {
	"org.kui",
	"org.action"
}

local _ENV = namespace(ns, using, modname)

--===================module property========================
local super = UIPop	--父类
welcome_view = class(modname, super, _ENV)
local welcome_view = welcome_view


local ui_root = "Container_Welcome/Container_Panel"

local ui_map = {

	btn_ok = "Button_Ok",
	label_ok = "Button_Ok/Label_Text",

}

welcome_view.pop_id = POP_ID.WELCOME

--===================module content========================

function welcome_view:__ctor()

	self.m_handler = self:get_handler(ui_root, ui_map)     --代理

	self.m_timeId = 0

end

function welcome_view:__show(showObj, ...)

	self:start_blink(0.8, -1)

end

function welcome_view:on_click_ok()

	--.print_t('on_click_ok')

	local function back()
		dvc_mgr:enter_stage()
	end

	self:start_blink(0.1, 6, back)
end


function welcome_view:setup_event()

	self.m_handler.btn_ok:attach_click(self.on_click_ok, self)

end


function welcome_view:clear_event()


end

function welcome_view:__destroy()

	self:stop_blink()

end


--//-------~★~-------~★~-------~★~点击闪烁~★~-------~★~-------~★~-------//

--@duration 闪烁间隔
--@repeat_cnt 重复次数
function welcome_view:start_blink(duration, repeat_cnt, cb)

	self:stop_blink()

	duration = duration or 1
	repeat_cnt = repeat_cnt or -1
	if repeat_cnt > 0 then
		repeat_cnt = repeat_cnt * 2
	end

	local i = 1

	local function back(obj)

		if obj._repeat == 0 then
			if cb then
				cb()
			end
		end

		i = i + 1
		if i % 2 == 0 then
			self.m_handler.label_ok:set_active(false)
		else
			self.m_handler.label_ok:set_active(true)
		end
	end

	self.m_handler.label_ok:set_active(true)
	self.m_timeId = setTimeOut(duration, back, nil, {repeat_cnt=repeat_cnt})

end


function welcome_view:stop_blink()

	if self.m_timeId > 0 then
		self.m_timeId = clearTimeOut(self.m_timeId)
	end

end

return welcome_view