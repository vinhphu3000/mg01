-- notify_tips
--@author jr.zeng
--@date 2019/2/4  13:45
local modname = "notify_tips"
--==================global reference=======================

--===================namespace========================
local ns = nil
local using = {'org.kui'}
local _ENV = namespace(ns, using, modname)

--===================module property========================
local super = UIPop	--父类
notify_tips = class(modname, super, _ENV)
local notify_tips = notify_tips


local ui_root = 'Container_NotifyTips/Container_Panel'

local ui_map = {

	--btn_close = 'Button_close',
	container_tips1 = 'Container_tips1',

}


local ui_map_item =
{
	label_tips = "Label_Tips",
	image_bg = "Image_zhezhao",
}

local tips_num_max = 5

--窗口id
notify_tips.pop_id = POP_ID.NOTIFY_TIPS
--层级
notify_tips.layerId = POP_LAYER_ID.LAYER_TIPS

--===================module content========================

function notify_tips:__ctor()

	self.m_handler = self:get_handler(ui_root, ui_map)     --代理

	local tips =  self.m_handler.container_tips1
	tips:set_active(false)

	self.m_idx2item = {
		self:get_handler(tips, ui_map_item)
	}

	local parent = tips:get_parent()
	local ox,oy = self.m_handler.container_tips1.get_ach_pos()
	local ow,oh = self.m_handler.container_tips1.get_size_delta()
	for i=2, tips_num_max do

		local y = oy - ( (i-1) * oh + 1 )
		local new_go = instantiate(tips.gameObject)
		new_go:SetActive(false)
		DisplayUtil.addChild(parent, new_go)
		new_go:SetAchPos_(ox, y)
		self.m_idx2item[i] = self:get_handler(new_go, ui_map_item)
	end

	----

	self.m_msgQueue = new(Array)

	self.m_pause = false

end

function notify_tips:__show(showObj, ...)
    
    
end

function notify_tips:setup_event()
    
    
end

function notify_tips:clear_event()
    
    
end

function notify_tips:__destroy()
    
    
end


--//-------~★~-------~★~-------~★~default~★~-------~★~-------~★~-------//

function notify_tips:show_msg(str)
	
	self.m_msgQueue:push(str)

end


function notify_tips:check_start()

	local need_start = false


end


function notify_tips:update(dt)

end


--//-------~★~-------~★~-------~★~暂停~★~-------~★~-------~★~-------//


function notify_tips:pause()

	if self.m_pause then
		return end
	self.m_pause = true

	--self:detach_update(self.update, self)
end


function notify_tips:resume()

	if not self.m_pause then
		return end
	self.m_pause = false

	if #self.m_msgQueue > 0 then
		self:attach_update(self.update, self)
	end

end


return notify_tips