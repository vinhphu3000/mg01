--StringUtil
--@author jr.zeng
--2017年4月19日 下午2:20:19
local modname = "StringUtil";
--==================global reference=======================

local tostring = tostring
local string = string
local ipairs = ipairs
local table = table

--===================namespace========================
local ns = "org"
local using = {--[[namespace1,...]]}
local _ENV = namespace(ns, using)
--===================module property========================
StringUtil = {}
local StringUtil = StringUtil

--===================module content========================



--字符串连接
--@str_arr {"a", "b", "c"}
--@sep "_"
--return "a_b_c"
function StringUtil.joint(str_arr, sep)

    return table.concat(str_arr, sep)
end


--字符串拼接
function StringUtil.concat(...)
    return table.concat({...})
end

--去除两侧空格
function StringUtil.trim(s)   

    return (string.gsub(s, "^%s*(.-)%s*$", "%1"))  
end  

--截断到首个指定字符
function StringUtil.sub_to_first(str, char)
    
    local index = string.find(str, char, 1)
    if index > 1 then
        return string.sub(str, 1, index-1)
    end
    return ""
end

function StringUtil.startWith(str1, str2)

	return string.find(str1, str2) == 1
end

return StringUtil