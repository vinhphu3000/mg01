--TestPop3
--@author jr.zeng
--2017年10月16日 下午3:15:10
local modname = "TestPop3"
--==================global reference=======================

--===================namespace========================
local ns = "test"
local using = {"org.kui"}
local _ENV = namespace(ns, using, modname)
--===================module property========================
local super = UIPop    --父类
_ENV[modname] = class(modname, super)
local TestPop3 = _ENV[modname]

--===================module content========================


local ui_root = "Container_Test3/Container_panel"


local ui_map_list_item =
    {
        label_text = "Label_Text"
    }

local ui_map = {
    btn_1 = "Button_SystemAnnounce",
    btn_close = "Button_Close",

    input_1 = { path = "Input_name",
        label_textHolder = "Label_textholder",
    },
    icon_1 = "Image_Icon1",

    toggle_1 = "Toggle_position1",
    label_toggle1 = "Toggle_position1/Label_label",
    tglgroup_1 = "ToggleGroup_1",

    sv_itemList = { path = "ScrollView_ItemList",
        ui_map = {
	        container_content = "Image_mask/Container_content",
            container_item = "Image_mask/Container_content/Container_Item",
        }
    },

    slider_Schedule1 = "Slider_Schedule1",
    prog_exp = "ProgressBar_EXP",
}


TestPop3.pop_id = POP_ID.TEST_3

function TestPop3:__ctor()

    self.m_hdl = self:get_handler(ui_root, ui_map)     --代理

    local go = self.m_hdl.gameObject
    self.m_btn1 = self.m_hdl.btn_1
    self.m_btn1:setShrinkEnabled(false)
    --nslog.print_r("btn", self.m_btn1)

    --文本
    self.m_input1 = self.m_hdl.input_1
    local label = self.m_hdl.input_1.label_textHolder
    label:set_text("哇哈哈哈哈")
    local size = label:get_font_size()
    label:set_font_size( size - 18 )
    --输入框
    self.m_input1:set_input_text("你好啊")
    local trans = self.m_input1:get_rectTrans()
    --nslog.print_r("trans", trans )

    self.m_input1:ensure_component(CTypeName.KButton)
    --self.m_input1:limit_input_number()

    --ListView
    --self.m_hdl.sv_itemList.container_content:ensure_component(CTypeName.KListView)
    --self.m_listView = self.m_hdl.sv_itemList.container_content
    --ListViewScroll
    self.m_hdl.sv_itemList:ensure_component(CTypeName.KListViewLoop)
    self.m_listView = self.m_hdl.sv_itemList

    self.m_listView:set_layout_param({divNum = 3, itemGap = cc.p(20,30)})    --现在gap改变时会不准
    --self.m_listView:set_scrollable(false)

    self.m_hdl.tglgroup_1:allow_mult_in_tgl_group(true)

    self.m_info_arr = false


	self.m_btn_pos = new(Vec2, self.m_btn1:getAchPos() )
	self.m_btn1:setAchPos(self.m_btn_pos.x + 500, self.m_btn_pos.y)
end

function TestPop3:__show(showObj, ...)

    self.m_info_arr = ArrayUtil.genSequence(100)

    self.m_listView:show_list(self.m_info_arr,  {
        onItemData = self.update_item
    } )


    self.m_listView:jump_to_index(50, JumpPosType.CENTER)

    --self.m_hdl.slider_Schedule1:set_interactable(false)

    self.m_hdl.prog_exp:set_prog_value(0.2)

    self.m_hdl.icon_1:load_sprite("npc1026", "npc1026_mini", false)


    App.soundMgr:play_bgm("Sound/Bgm/battle04")
end

local aaa = 1
function TestPop3:on_click_btn1(go)

    --nslog.print_t( "on_click_btn1", go , self)
    self.m_hdl.label_toggle1:set_text("选项"..aaa)
    aaa =aaa + 1

    App.soundMgr:play_one_shot("Sound/UI/UI_EquipmentIntensify")

	self:test_action_1()
end


--scrollview
function TestPop3:on_sv_change(go, x, y)

    nslog.debug("on_sv_change", x, y)
end

--输入框内容改变
function TestPop3:on_input_change(go, str)

    nslog.debug("on_input_change", str)
end

--单选框
function TestPop3:on_toggle(go, b)

    nslog.debug("on_toggle", b)

    local b = self.m_hdl.toggle_1:is_tgl_on()

    App.soundMgr:set_total_on(b)
end

--多选框
function TestPop3:on_toggle_group(go, index, b)

    nslog.debug("on_toggle_group", index, b)

end

--滑动条
function TestPop3:on_slider(go, value)

    nslog.debug("on_slider", value)

    App.soundMgr:set_bgm_volume(value)

	self.m_input1:set_input_text(value)
end


function TestPop3:setup_event()

    self.m_btn1:attach_click(self.on_click_btn1)

    self.m_hdl.btn_close:attach_click(function (self)

            nslog.print_t("on_click_close", self)
            self:close()
    end)

    self.m_hdl.sv_itemList:attach_scroll_change(self.on_sv_change)
    self.m_input1:attach_input_change(self.on_input_change)

    self.m_hdl.toggle_1:attach_tgl_change(self.on_toggle)
    self.m_hdl.tglgroup_1:attach_tgl_change_in_group(self.on_toggle_group)

    self.m_hdl.slider_Schedule1:attach_slider_change(self.on_slider)
end

function TestPop3:clear_event()

    self.m_btn1:detach_click(self.on_click_btn1)
    --self.btn_close:detach_click(self.on_click_btn1, self)

	self.m_hdl.btn_close:detachAllGoEvt()

    self.m_hdl.sv_itemList:detach_scroll_change(self.on_sv_change)
    self.m_input1:detach_input_change(self.on_input_change)
	
    self.m_hdl.toggle_1:detach_tgl_change(self.on_toggle)
    self.m_hdl.tglgroup_1:detach_tgl_change_in_group(self.on_toggle_group)
	
    self.m_hdl.slider_Schedule1:detach_slider_change(self.on_slider)
end

function TestPop3:__destroy()

    self.m_listView:clear_list()

    App.soundMgr:stop_bgm()
    App.soundMgr:stop_all_eff()
end


--//-------~★~-------~★~-------~★~列表~★~-------~★~-------~★~-------//


--列表项更新
function TestPop3:update_item(item, index, data)

	local handler = self:get_handler(item, ui_map_list_item)
	handler.label_text:set_text(data)
	--nslog.debug("update_item", item, index)
end

--//-------~★~-------~★~-------~★~Action~★~-------~★~-------~★~-------//

function TestPop3:test_action_1()

	App.actionMgr:stop(self.m_btn1.gameObject)

	self.m_btn1:setAchPos( self.m_btn_pos:get() )

	local function getPos() return self.m_btn1:getAchPos() end
	local function setPos(x,y) self.m_btn1:setAchPos(x,y) end

	local action1 = action.seq(
		action.ease( action.mov_to3(2, {1147, -626} ) ) ,
		action.delay(2),
		action.ease(
			action.set_to(2, {
				toValue = self.m_btn_pos:pack(),
				getter = getPos,
				setter = setPos,
			})
		)
	)


	local action2 = action.seq(
	action.ease( action.mov_to3(2, {1147, -626} ) ) ,
	action.delay(2),
	action.ease(
	action.set_to(2, {
		toValue = self.m_btn_pos:pack(),
		getter = getPos,
		setter = setPos,
	})
	)
	)


	--local action = Action.to(2, {
	--	values = {1147, -626},
	--	setter = callback(self.m_btn1.gameObject.SetAchPos_, self.m_btn1.gameObject),
	--	getter = callback(self.m_btn1.gameObject.GetAchPos_, self.m_btn1.gameObject),
	--})

	App.actionMgr:run(self.m_btn1.gameObject, action)

end



return TestPop3