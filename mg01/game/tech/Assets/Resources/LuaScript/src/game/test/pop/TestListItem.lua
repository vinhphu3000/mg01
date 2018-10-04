-- TestListItem
--@author jr.zeng
--@date 2018/10/4  15:08
local modname = "TestListItem"
--==================global reference=======================

--===================namespace========================
local ns = "test"
local using = {}
local _ENV = namespace(ns, using, modname)

--===================module property========================
local super = ListViewItem	--父类
TestListItem = class(modname, super, _ENV)
local TestListItem = TestListItem

--===================module content========================

local ui_map =
{
	container_select = "Image_bg/select",
	container_normal1 = "Image_bg/normal1",
	label_name = "Label_Name",
	label_winNum = "Label_WinNum",
	label_memberNum = "Label_MemberNum",
}

function TestListItem:__ctor()

	self.m_handler = self:get_handler(nil, ui_map)

end

function TestListItem:__show(data, ...)

	local handler = self.m_handler

	local b = self.index % 2 == 0
	handler.container_select:set_active(not b)
	handler.container_normal1:set_active(b)

	handler.label_name:set_text(data.name)
	handler.label_winNum:set_text(data.idx)
	handler.label_memberNum:set_text(99 .. "人")
end

function TestListItem:setup_event()


end

function TestListItem:clear_event()


end

function TestListItem:__destroy()

	super.__destroy(self)

	--nslog.print_t('destroy', self.index)
end


return TestListItem