-- msg_mgr
--@author jr.zeng
--@date 2019/2/5  22:57
local modname = "msg_mgr"
--==================global reference=======================

--===================namespace========================
local ns = nil
local using = {--[[namespace1,...]]}
local _ENV = namespace(ns, using, modname)

--===================module property========================
local super = CCModule	--父类
msg_mgr = class(modname, super, _ENV)
local msg_mgr = msg_mgr


--===================module content========================

function msg_mgr:__ctor()


end

function msg_mgr:__setup(...)

	self:ctor()

	App.popMgr:show(POP_ID.MSG_TIPS)

end

function msg_mgr:__clear()


end

function msg_mgr:setup_event()


end

function msg_mgr:clear_event()


end


--//-------∽-★-∽------∽-★-∽--------∽-★-∽数据管理∽-★-∽--------∽-★-∽------∽-★-∽--------//

--//-------∽-★-∽------∽-★-∽--------∽-★-∽数据操作∽-★-∽--------∽-★-∽------∽-★-∽--------//


--//-------∽-★-∽------∽-★-∽--------∽-★-∽msg_tips∽-★-∽--------∽-★-∽------∽-★-∽--------//

function msg_mgr:__get_msg_tips()
	local pop = App.popMgr:getPopOpened(POP_ID.MSG_TIPS)
	return pop
end

function msg_mgr:show_msg(a1, ...)
	local pop = self:__get_msg_tips()
	if pop then
		pop:show_msg(a1)
	end
end

function msg_mgr:pause_msg(is_pause)

	local pop = self:__get_msg_tips()
	if pop then
		if is_pause then
			pop:pause()
		else
			pop:resume()
		end
	end
end

return msg_mgr