--UIHandler
--@author jr.zeng
--2017年10月17日 下午4:23:33
local modname = "UIHandler"
--==================global reference=======================

local type = type
local pairs = pairs
local ipairs = ipairs
local rawget = rawget
local setmetatable = setmetatable
local assert = assert
local string = string

--===================namespace========================
local ns = "org.kui"
local using = {--[[namespace1,...]]}
local _ENV = namespace(ns, using, modname)
--===================module property========================
local super = nil	--父类
UIHandler = class(modname, super)
local UIHandler = UIHandler

local Refer = Refer

--函数参数格式
local key2fn_g = {}     --(go, ...)
local key2fn_gr = {}    --(go, refer, ...)
local key2fn_gftr = {}  --(go, function. target, refer)


-- {path = "", ui_map = {}}
local function formatUiMap(uiMap_)

	local ui_map = {}

    if not uiMap_ then
        return ui_map
    end

    local tp
    for k,v in pairs(uiMap_) do

        tp = type(v)
        if tp == "string" then
            ui_map[k] = {path = v}
        elseif tp == "table" then
            ui_map[k] = v
	        v.path = v.path or v[1] --path或者下标[1]都可以表示路径
        else
            nslog.error("invalid type: uiMap", tp, v)
        end
    end
    return ui_map
end

local mt = {
    __index = function(self, k)

        local go = self.gameObject
        local v
        
        if v == nil then
            if key2fn_g[k] then
                v = function(self, ...)
                    return key2fn_g[k](go, ...)
                end
            elseif key2fn_gftr[k] then
	            v = function(self, ...)
		            local params = {...}
		            local fun = params[1]
		            local target = params[2]
		            --local target = params[2] or self.m_refer
		            return key2fn_gftr[k](go, fun, target, self.m_refer )
	            end
            elseif key2fn_gr[k] then
                v = function(self, ...)
                    return key2fn_gr[k](go, self.m_refer, ... )
                end
            end
        end
        
        if v == nil then
            v = self.m_ui_map[k]    
            if v then   
                --是子对象,创建代理
                --nslog.print_t(self.gameObject, v)
                local go = self.gameObject:FindChild_(v.path)
	            if not go then
		            assert(false, "cant find gameobject: " .. k)
	            end
                v = UIHandler.new(go, v.ui_map or v, self.m_refer)
            end
        end
        
        if v == nil then
            --最后才访问类属性
            v = UIHandler[k]
        end

        if v ~= nil then
            self[k] = v     --下次不会再进__index
            return v
        end
        
        return nil
    end
}

--不导出的接口
local  not_export = 
    {
        ["ctor"] = 1,
        ["__ctor"] = 1,
        ["new"] = 1,
    }



--添加帮助
--@help HelpGo类
function UIHandler.addHelp(help)

    --local help_id = help.help_id 

    local fullName
    local funDic
    for k, fn in pairs(help) do
        
        if not not_export[k] then

	        if type(fn) == "function" then

		        if help.fnSet_gftr and help.fnSet_gftr[k] then
			        funDic = key2fn_gftr
		        elseif help.fnSet_gr and help.fnSet_gr[k] then
			        funDic = key2fn_gr
		        else
		            funDic = key2fn_g
		        end

		        if not funDic[k] then
			        funDic[k] = fn
		        else
			        nslog.warn("help方法重名:", k)
		        end

		        --fullName = k .. "4" .. help_id
		        --funDic[fullName] = fn
		        --nslog.debug("addHelp", fullName)
	        end
        end
    end
end


function UIHandler.dump()
    nslog.print_r("dump", key2fn_g, key2fn_gr)
end

--===================module content========================

--@ui gameobject/UIHander
--@refer_
function UIHandler:__ctor(ui_, ui_map, refer_)

    Refer.assert(refer_) 
    
    local tp = type(ui_)
    if tp == "userdata" then
        self.gameObject = ui_
    elseif tp == "table" and ui_.class == UIHandler then
        self.gameObject = ui_.gameObject
    else
        nslog.error("未知的类型", tp, ui_)
    end
    
    assert(self.gameObject, "miss gameobject")
    
    self.m_ui_map = formatUiMap(ui_map)
    self.m_refer = refer_
    
    setmetatable(self, mt)  --重置原表，不再是UIHandler
    --nslog.print_t("new UIHandler" , self)
end



return UIHandler