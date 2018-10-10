-- ListView
--@author jr.zeng
--@date 2018/9/29  10:43
local modname = "ListView"
--==================global reference=======================

--===================namespace========================
local ns = 'org.kui'
local using = {}
local _ENV = namespace(ns, using, modname)

--===================module property========================
local super = ImageAbs	--父类
ListView = class(modname, super, _ENV)
local ListView = ListView

local clear_tbl = clear_tbl
local cc = cc

local LayoutDir = LayoutDir
local LayoutUtil = LayoutUtil


--===================module content========================


function ListView:__dispose()

	self:clear_all_item_views(true)
	 self.m_itemPool:clear()

end

--@setting {
--  container                               摆放列表项的容器(gameobject或者uihandler)
--  on_item_data(item_go, data, index)      单个列表项数据回调
--
--}
function ListView:__ctor( setting)

	self.m_layoutParam = {}
	self.m_layoutItems = {}

	self.m_itemPool = new(Array)
	self.m_itemTemp = false

	self.m_index2item = {}
	self.m_item2data = {}

	self.m_itemCls = false
	self.m_item2view = {}

	self.m_dataList = false
	self.m_dataLenLast = 0

	self:setting(setting or empty_tbl )

	local go = setting.container.gameObject
	self.m_contentGo = go
	self.m_contentSize = cc.size()
	self.m_contentSizeDf = cc.size(go:GetSizeDelta_())  --起始的content尺寸
	self:showGameObjectEx(go)

	self:initialize()
    
end


function ListView:setting(setting)

	self.m_setting = setting

	self.m_layoutParam.dir = setting.dir or LayoutDir.TopToBottom
	self.m_layoutParam.divNum = setting.divNum or 1
	self.m_layoutParam.itemSize = setting.itemSize or false
	self.m_layoutParam.itemGap = setting.itemGap or cc.p(5,5)
	self.m_layoutParam.origin = setting.origin or cc.p(0,0)
	self.m_layoutParam.pivot = setting.pivot or false
	self.m_layoutParam.padding = setting.padding or cc.padding(0,0,0,0)

	self.m_itemCls = setting.item_cls or false
end


function ListView:initialize()

	self.m_itemTemp = GameObjUtil.fuzzySearchChild(self.m_gameObject, 'item')
	self.m_itemTemp:SetActive(false)

	if not self.m_layoutParam.itemSize then
		self.m_layoutParam.itemSize = cc.size( self.m_itemTemp:GetSizeDelta_() )
	end

	if not self.m_layoutParam.pivot then
		self.m_layoutParam.pivot = cc.p( self.m_itemTemp:GetPivot_() )
	end

	nslog.print_t('m_layoutParam', self.m_layoutParam)

end

function ListView:__show(dataList, ...)

	self.m_dataList = dataList or empty_tbl
	clear_tbl(self.m_item2data)

	self:create_layout_items()  --创建全部布局项

	self:show_list()
	self.m_dataLenLast = #self.m_dataList   --记录数据长度

end

function ListView:setup_event()
    
    
end

function ListView:clear_event()
    
    
end

function ListView:__destroy()
    
    self:clear_list()

	self.m_dataList = false
	self.m_dataLenLast = 0
end


--//-------~★~-------~★~-------~★~数据操作~★~-------~★~-------~★~-------//


function ListView:show_list()

	local item_go
	local index

	local len = #self.m_dataList
	for i=1, len do
		index = i
		item_go = self:create_item(index)
		self:set_item_data(item_go, self.m_dataList[i], index)
	end

	if self.m_dataLenLast > index then
		--比起上次有多出来的项
		index = index + 1
		for i=self.m_dataLenLast, index, -1 do
			self:recyle_item(i)
		end
	end

	self:refresh_content_size()
end


function ListView:clear_list()

	if not self.m_dataList then
		return end
	self.m_dataList = false
	self.m_dataLenLast = 0

	local keysToDel = TableUtil.get_keys(self.m_index2item)

	for i, idx in ipairs(keysToDel) do
		self:recyle_item(idx)
	end

	clear_tbl(self.m_index2item)
	clear_tbl(self.m_item2data)

	self:clear_layout_items()
	self:clear_all_item_views(false)

	self:on_clear_list()

end

function ListView:on_clear_list()

end

--刷新content的尺寸
function ListView:refresh_content_size()

	local w,h = LayoutUtil.calc_layout_size(self.m_layoutParam, #self.m_dataList)

	w = math.max(w, self.m_contentSizeDf.w)
	h = math.max(h, self.m_contentSizeDf.h)

	self.m_contentSize.w = w
	self.m_contentSize.h = h
	self.m_contentGo:SetSizeDelta_(w, h)
end

--//-------~★~-------~★~-------~★~数据管理~★~-------~★~-------~★~-------//

--创建item
function ListView:create_item(index)

	local item_go = self.m_index2item[index]
	if item_go then
		return item_go
	end

	item_go = self.m_itemPool:pop()
	if not item_go then
		item_go = instantiate(self.m_itemTemp)
		DisplayUtil.addChild(self.m_contentGo, item_go)
	end

	self.m_index2item[index] = item_go
	item_go:SetActive(true)
	item_go.name = 'item(' .. index .. ')'

	self:layout_item(index, item_go)
	return item_go
end

--回收item
function ListView:recyle_item(index)

	if not self.m_index2item[index] then
		return end

	local item_go = self.m_index2item[index]
	self:clear_item_data(item_go, index)

	self.m_index2item[index] = nil
	self.m_itemPool:add(item_go)

	item_go:SetActive(false)
end

--设置item数据
function ListView:set_item_data(item_go, data, index)

	local old =  self.m_item2data[item_go]
	if old ~= nil and old == data then
		--数据一样， 不重复设置
		return end

	self.m_item2data[item_go] = data

	if self.m_itemCls then
		local view = self:create_item_view(item_go, index)
		view.index = index
		view:show(data)
	else

		if self.m_setting.on_item_data then
			self.m_setting.on_item_data(item_go, data, index)
		end
	end

end


function ListView:clear_item_data(item_go, index)

	if self.m_item2data[item_go] == nil then
		return end
	self.m_item2data[item_go] = nil

	if self.m_itemCls then
		local view = self:create_item_view(item_go, index)
		view:destroy()
		view.index = -1
	end
end

--//-------~★~-------~★~-------~★~layout~★~-------~★~-------~★~-------//

--创建所有布局项
function ListView:create_layout_items()

	LayoutUtil.genLayoutItems(self.m_layoutParam, #self.m_dataList, self.m_layoutItems)
end


function ListView:clear_layout_items()

	LayoutUtil.clearLayoutItems(self.m_layoutItems)
end


function ListView:layout_item(index, item_go)
	local layoutItem = self.m_layoutItems[index]
	item_go:SetAchPos_(layoutItem.pos.x, layoutItem.pos.y)
	return layoutItem.pos
end


--//-------~★~-------~★~-------~★~ListViewItem~★~-------~★~-------~★~-------//


function ListView:create_item_view(item_go, index)

	local view = self.m_item2view[item_go]
	if view then
		return view
	end

	view = new(self.m_itemCls, item_go)
	view:retain(self)

	self.m_item2view[item_go] = view

	return view
end


function ListView:clear_all_item_views(del)

	if not self.m_itemCls then
		return end

	for go, view in pairs(self.m_item2view) do

		view:destroy()

		if del then
			view:release(self)
		end
	end

	if del then
		clear_tbl(self.m_item2view)
	end
end

return ListView