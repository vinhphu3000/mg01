-- TestPop4
--@author jr.zeng
--@date 2018/2/6  15:03
local modname = "TestPop4"
--==================global reference=======================

--===================namespace========================
local ns = "test"
local using = {"org.action"}
local _ENV = namespace(ns, using, modname)

--===================module property========================
local super = UIPop	--父类
TestPop4 = class(modname, super, _ENV)
local TestPop4 = TestPop4

--===================module content========================

local ui_root = "Container_TestPop4/Container_Panel"

local ui_map = {

	btn_ok = "Button_Ok",
	btn_close = "Button_Close",

	toggle_1 = {"Toggle_position1",
		label_toggle1 = "Label_Text"
	},

	tglgroup_1 = "ToggleGroup_a1",

	input_name = "Input_name",
	label_testInput = "Label_testInput",

	slider_schedule1 = "Slider_Schedule1",
	label_testSilder = "Label_testSilder",

	progbar_loading = "ProgressBar_loading",

	image_icon = "Container_Image/Image_sharedAnchor",

	sv_guildlist = {"ScrollView_GuildList",
		image_mask = "Image_mask",
		container_content = "Image_mask/Container_content",
	},
}


local ui_map_item =
{
	container_select = "Image_bg/select",
	container_normal1 = "Image_bg/normal1",
	label_name = "Label_Name",
	label_winNum = "Label_WinNum",
	label_memberNum = "Label_MemberNum",
}

TestPop4.pop_id = POP_ID.TEST_4

function TestPop4:__ctor()

	self.m_handler = self:get_handler(ui_root, ui_map)     --代理

	local go = self.m_handler.gameObject

	--self.m_handler.sv_guildlist:ensure_component(CTypeName.KListViewScroll)

end

function TestPop4:__show(showObj, ...)

	self:show_btn()
	self:show_text()
	self:show_toggle()
	self:show_slider()
	self:show_progbar()
	self:show_image()
	self:show_scrollview()
	self:show_listview()
	--
	self:show_res()
	--self:show_my_list()

end

function TestPop4:setup_event()

	App.keyboard:attach(KEY_EVENT.RELEASE, self.on_key_press, self)

end


function TestPop4:clear_event()

	App.keyboard:detach(KEY_EVENT.RELEASE, self.on_key_press, self)

end


function TestPop4:__destroy()

	self:clear_btn()
	self:clear_text()
	self:clear_toggle()
	self:clear_slider()
	self:clear_progbar()
	self:clear_image()
	self:clear_scrollview()
	self:clear_listview()
	--
	self:clear_res()
	--self:clear_my_list()
end


function TestPop4:on_key_press(evt_)

	--nslog.print_t("on_key_press", evt_.data)

	local code = evt_.data
	if code == KeyCode.Keypad1 then

		GameObjUtil.remove_from_parent(self.m_handler.btn_ok.gameObject)

	elseif code == KeyCode.Keypad2 then

		GameObjUtil.change_parent(self.m_handler.btn_ok.gameObject, self.m_handler.gameObject)
	end

end

--//-------~★~-------~★~-------~★~button~★~-------~★~-------~★~-------//


function TestPop4:show_btn()

	self.m_handler.btn_ok:attach_click(self.on_click_ok, self)
	self.m_handler.btn_close:attach_click(self.on_click_close, self)


end

function TestPop4:clear_btn()

	self.m_handler.btn_ok:detach_click(self.on_click_ok, self)
	self.m_handler.btn_close:detach_click(self.on_click_close, self)
end


local a = 1
function TestPop4:on_click_ok(go)

	--nslog.print_t('on_click_ok', self, go)
	self.m_handler.toggle_1.label_toggle1:set_text(a.."次")
	a = a + 1
end

function TestPop4:on_click_close(go)

	self:close()
end

--//-------~★~-------~★~-------~★~text~★~-------~★~-------~★~-------//

function TestPop4:show_text()

	self.m_handler.input_name:attach_input_change(self.on_input_change, self)
	self.m_handler.input_name:set_input_type( InputType.Password )

end


function TestPop4:clear_text()

	self.m_handler.input_name:detach_input_change(self.on_input_change, self)

end


function TestPop4:on_input_change(go, str)

	self.m_handler.label_testInput:set_text(str)
end


--//-------~★~-------~★~-------~★~toggle~★~-------~★~-------~★~-------//


function TestPop4:show_toggle()

	self.m_handler.toggle_1.label_toggle1:set_text("骄傲的勾选框")
	self.m_handler.tglgroup_1:allow_mult_tglgroup(true)


	self.m_handler.tglgroup_1:attach_tglgroup_change(self.on_toggle_group, self)
end


function TestPop4:clear_toggle()


	self.m_handler.tglgroup_1:detach_tglgroup_change(self.on_toggle_group, self)

end

function TestPop4:on_toggle_group(go, index, b)

	self.m_handler.toggle_1.label_toggle1:set_text((b and "选中 " or "取消 ") .. index)
end


--//-------~★~-------~★~-------~★~silder~★~-------~★~-------~★~-------//


function TestPop4:show_slider()

	self.m_handler.slider_schedule1:attach_slider_change(self.on_slider_change, self)

	self.m_handler.label_testSilder:set_text( self.m_handler.slider_schedule1:get_slider_value() )
end


function TestPop4:clear_slider()


	self.m_handler.slider_schedule1:detach_slider_change(self.on_slider_change, self)
end

function TestPop4:on_slider_change(go, value)

	self.m_handler.label_testSilder:set_text(value)
end

--//-------~★~-------~★~-------~★~progressbar~★~-------~★~-------~★~---

function TestPop4:show_progbar()

	local function getValue()
		return self.m_handler.progbar_loading:get_prog_value()
	end

	local function setValue(value)
		self.m_handler.progbar_loading:set_prog_value(value)
	end

	self.m_handler.progbar_loading:set_prog_value(0)

	local action1 = action.ease(
		action.set_to(4, {
			toValue = 1,
			getter = getValue,
			setter = setValue,
		}),
		EaseType.BounceIn
	)

	App.actionMgr:run(self, action1)
end


function TestPop4:clear_progbar()

	App.actionMgr:stop(self)
end

--//-------~★~-------~★~-------~★~image~★~-------~★~-------~★~-------//

function TestPop4:show_image()

	self.m_handler.image_icon:load_sprite('npc1031', true)

end


function TestPop4:clear_image()

end


--//-------~★~-------~★~-------~★~scrollview~★~-------~★~-------~★~-------//

function TestPop4:show_scrollview()

end


function TestPop4:clear_scrollview()

end


--//-------~★~-------~★~-------~★~klistview~★~-------~★~-------~★~-------//

function TestPop4:show_listview()

	local list_view = self.m_handler.sv_guildlist

	list_view:set_layout_param({
		itemGap = cc.p(20,30),
	})
	--self.m_handler.sv_guildlist:set_scrollable(false)

	local data
	local data_list = {}
	local len = 999
	for i=1, len do
		data = {idx=i, name="你是谁 "..i }
		data_list[#data_list+1] = data
	end

	list_view:show_list(data_list, self.update_item, 11)
end


function TestPop4:clear_listview()

	self.m_handler.sv_guildlist:clear_list()

end

function TestPop4:update_item(item, index, data)

	local handler = self:get_handler(item, ui_map_item)

	local b = index % 2 == 0
	handler.container_select:set_active(not b)
	handler.container_normal1:set_active(b)

	handler.label_name:set_text(data.name)
	handler.label_winNum:set_text(data.idx)
	handler.label_memberNum:set_text(99 .. "人")

end


--//-------~★~-------~★~-------~★~Resource~★~-------~★~-------~★~-------//


function TestPop4:show_res()

	local go_path = 'GUI/cn/Prefab/Canvas_Test3'
	--GameObjCache:load_async(go_path, self.on_go_loaded, self)

end

function TestPop4:clear_res()


end


function TestPop4:on_go_loaded(go, url)

	--nslog.print_t('on_go_loaded', go, url)

end

--//-------~★~-------~★~-------~★~klistview~★~-------~★~-------~★~-------//

function TestPop4:show_my_list()

	if not self.m_listView then

		self.m_listView = ListView.new({
			container = self.m_handler.sv_guildlist.container_content,
			divNum = 1,
			--on_item_data = callback(self.on_item_data, self),
			item_cls = TestListItem,

		})

		retain(self.m_listView, self)
	end


	local data
	local data_list = {}
	local len = 13
	for i=1, len do
		data = {idx=i, name="你是谁 "..i }
		data_list[#data_list+1] = data
	end

	self.m_listView:show(data_list)
end


function TestPop4:clear_my_list()

	if self.m_listView then
		self.m_listView:destroy()
	end

end

function TestPop4:on_item_data(item, index, data)

	local handler = self:get_handler(item, ui_map_item)

	local b = index % 2 == 0
	handler.container_select:set_active(not b)
	handler.container_normal1:set_active(b)

	handler.label_name:set_text(data.name)
	handler.label_winNum:set_text(data.idx)
	handler.label_memberNum:set_text(99 .. "人")

end


return TestPop4