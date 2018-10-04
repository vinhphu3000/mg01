--HelpInput
--@author jr.zeng
--2017年10月18日 下午5:57:54
local modname = "HelpInput"
--==================global reference=======================

--===================namespace========================
local ns = "org.kui"
local using = {--[[namespace1,...]]}
local _ENV = namespace(ns, using, modname)
--===================module property========================
local super = HelpUi	--父类
HelpInput = class(modname, super, _ENV)
local HelpInput = HelpInput

local CTypeName = CTypeName
local EvtCenter = EvtCenter
local UI_EVT = UI_EVT

local function getKInput(go)
    return HelpInput.need_component(go, CTypeName.KInput)
end

--输入类型
InputType =
    {
        Standard = 0,
        AutoCorrect = 1,
        Password = 2
    }

--行类型
InputLineType = 
    {
        SingleLine = 0,
        MultiLineSubmit = 1,
        MultiLineNewline = 2
    }

--文本输入类型限制
InputContentType = 
    {
        Standard = 0,
        Autocorrected = 1,
        IntegerNumber = 2,
        DecimalNumber = 3,
        Alphanumeric = 4,
        Name = 5,
        EmailAddress = 6,
        Password = 7,
        Pin = 8,
        Custom = 9
    }

--===================module content========================

function HelpInput.set_input_text(go, str)

    local input = getKInput(go)
    if input then
        input.text = str
    end
end

--追加文本
function HelpInput.append_input_text(go, str)
    
    local a = getKInput(go)
    if not a then
       return end
    a:AppendString(str)
end

--设置placeholder的文本
function HelpInput.set_placeholder_text(go, text)
    local a = getKInput(go)
    if not a then
        return end
    a.placeholder.text = text
end 

--设置字符数量上限
function HelpInput.set_char_limit(go, length)
    local a = getKInput(go)
    if not a then
        return end
    a.characterLimit = length
end 

--设置输入类型
--@type InputType
function HelpInput.set_input_type(go, type)
    local a = getKInput(go)
    if not a then
        return end
    a.inputType = type
end 

--设置行类型
--@type InputLineType
function HelpInput.set_input_line_type(go, type)
    local a = getKInput(go)
    if not a then
        return end
    a.lineType = type
end 

--设置格式类型
--@type InputContentType
function HelpInput.set_input_content_type(go, type)
    local a = getKInput(go)
    if not a then
        return end
    a.contentType = type
end 

--限定只能输入数字
function HelpInput.limit_input_number(go)
    local a = getKInput(go)
    if not a then
        return end
    a.contentType = InputContentType.IntegerNumber
end 

--获取焦点
function HelpInput.gainFocus(go)

    local a = getKInput(go)
    if not a then
        return end
    a:GainFocus()
end

--光标跳到末尾
function HelpInput.move_index_end(go, isShift)

    local a = getKInput(go)
    if not a then
        return end
    a:MoveIndexEnd(isShift)
end



--//-------~★~-------~★~-------~★~事件相关~★~-------~★~-------~★~-------//
--需以attach和detach为前缀

HelpInput.fnSet_gftr =
{
	attach_input_change = 1,
	attach_edit_end =1,
	--
	detach_input_change = 1,
	detach_edit_end = 1,
}

--监听事件_文本改变
--@fun (go, str)
function HelpInput.attach_input_change(go, fun, target, refer)
    EvtCenter:attachGoEvt(go, UI_EVT.VALUE_CHANGE, fun, target, refer)
end

function HelpInput.detach_input_change(go, fun, target)
    EvtCenter:detachGoEvt(go, UI_EVT.VALUE_CHANGE, fun, target)
end

--监听事件_编辑结束
--@fun (go, str)
function HelpInput.attach_edit_end(go, fun, target, refer)
    EvtCenter:attachGoEvt(go, UI_EVT.EDIT_END, fun, target, refer)
end

function HelpInput.detach_edit_end(go, fun, target)
    EvtCenter:detachGoEvt(go, UI_EVT.EDIT_END, fun, target)
end



return HelpInput