--LayoutUtil
--@author jr.zeng
--2017年10月26日 下午3:30:27
local modname = "LayoutUtil"
--==================global reference=======================

--===================namespace========================
local ns = "org.kui"
local using = {--[[namespace1,...]]}
local _ENV = namespace(ns, using, modname)
--===================module property========================

LayoutUtil = {}
local LayoutUtil = LayoutUtil

local cc = cc

--===================module content========================

--布局方向
LayoutDir =
{
	TopToBottom = 0,     --上下布局
	LeftToRight = 1,     --左右布局
}

--默认锚点(左上角)
LayoutPivot =  { x=0,y=1 }

local LayoutDir = LayoutDir
local LayoutPivot = LayoutPivot

--布局参数
function LayoutParam(param)
    
    param = param or {}
    --布局方向
    param.dir = param.dir or LayoutDir.TopToBottom     
    --每行/列的个数
    param.divNum = param.divNum or 1      
    --每项的尺寸(0,0时,会取实际尺寸)
    param.itemSize = param.itemSize or cc.size(0,0)      --等于0， 不处理
    --每项间距(x水平间距，y垂直间距)
    param.itemGap = param.itemGap or cc.p(5,5)
    --原点
    param.origin = param.origin or cc.p(0,0)   
    --每项的中点, 默认左上角
    param.pivot = param.pivot or cc.p(-1,-1)    --小于0， 不处理
    --每项的中点, 默认左上角
    param.padding = param.padding or cc.padding(0,0,0,0)          
    
    return param
end


--//-------~★~-------~★~-------~★~LayoutUtil~★~-------~★~-------~★~-------//

--计算总行列数
function LayoutUtil.calc_total_line()


end


--单项布局
function LayoutUtil.lay_item()


end



--@matchPivot
function LayoutUtil.calc_item_pos(layoutParam, index, matchPivot,  layoutItem)

	local x,y = layoutParam.origin.x,  layoutParam.origin.y

	local divNum = layoutParam.divNum
	local itemSize = layoutParam.itemSize
	local itemGap = layoutParam.itemGap
	local padding = layoutParam.padding
	local pivot = layoutParam.pivot

	index = index - 1   --转为0开始

	if layoutParam.dir == LayoutDir.TopToBottom then
		x = x + padding.left + (index % divNum) * (itemSize.w + itemGap.x)
		y = y + padding.top - math.floor(index / divNum) * (itemSize.h + itemGap.y)
	else
		x = x + padding.left + math.floor(index / divNum) * (itemSize.w + itemGap.x)
		y = y + padding.top - (index % divNum) * (itemSize.h + itemGap.y)
	end

	if layoutItem then
		layoutItem.rect.x = x
		layoutItem.rect.y = y - itemSize.h   --Y轴映射为右下坐标系
		layoutItem.rect.w = itemSize.w
		layoutItem.rect.h = itemSize.h
	end

	if matchPivot then

		if pivot.x ~= LayoutPivot.x or pivot.y ~= LayoutPivot.y then
			x = x + (pivot.x - LayoutPivot.x) * itemSize.w
			y = y + (pivot.y - LayoutPivot.y) * itemSize.h
		end
	end

	if layoutItem then

		layoutItem.pos.x, layoutItem.pos.y = x, y

		if matchPivot then
			layoutItem.pivot.x, layoutItem.pivot.y = pivot.x, pivot.y
		else
			layoutItem.pivot.x, layoutItem.pivot.y = LayoutPivot.x, LayoutPivot.y
		end
	end

	return x, y
end


--计算布局的总尺寸
function LayoutUtil.calc_layout_size(layoutParam, count)

	local w,h = 0,0

	local col,row = 0,0

	if layoutParam.dir == LayoutDir.TopToBottom then
		col = layoutParam.divNum
		row = math.ceil(count / layoutParam.divNum)
	else
		col = math.ceil(count / layoutParam.divNum)
		row = layoutParam.divNum
	end


	if col * row > 0 then

		local padding = layoutParam.padding

		w = padding.left + padding.right + col * layoutParam.itemSize.w + (col-1) * layoutParam.itemGap.x
		h = padding.top + padding.bottom + row * layoutParam.itemSize.h + (row-1) * layoutParam.itemGap.y

		if layoutParam.origin.x > 0 then
			--原点按中心在左上计算, 否则不计入
			w = w + layoutParam.origin.x
		end

		if layoutParam.origin.y > 0 then
			--y越往下，高度越高
			h = h - layoutParam.origin.y
		end

	end

	return w, h
end



--//-------~★~-------~★~-------~★~layoutItem~★~-------~★~-------~★~-------//


function LayoutUtil.newLayoutItem()

	local item = {
		rect = cc.rect(),
		pos = cc.p(),
		pivot = cc.p(),
	}

	return item
end


function LayoutUtil.clearLayoutItems(items)

	if #items == 0 then
		return end

	for i, item in ipairs(items) do


	end

	clear_tbl(items)
end


function LayoutUtil.genLayoutItems(layoutParam, num, out_list)

	out_list = out_list or {}

	local remain = #out_list - num

	local item
	for i=1, num do

		item = out_list[i]
		if not item then
			item = LayoutUtil.newLayoutItem()
			out_list[i] = item
		end

		LayoutUtil.calc_item_pos(layoutParam, i, true, item)
	end


	if remain > 0 then

		for i=num+remain, num -1 do
			item = out_list[i]
			out_list[i] = nil
			--recyleItem
		end
	end

end


return LayoutUtil