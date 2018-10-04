--t2str
--@author jr.zeng
--2017年11月9日 下午3:46:00
local modname = "t2str"
--==================global reference=======================

local type = type
local pairs = pairs
local ipairs = ipairs
local unpack = table.unpack
local setmetatable = setmetatable
local getmetatable = getmetatable
local rawget = rawget
local rawset = rawset
local next = next


local string = string
local srep = string.rep
local tostring = tostring
local tconcat = table.concat
local tinsert = table.insert

--===================namespace========================
local ns = nil
local using = {--[[namespace1,...]]}
local _ENV = namespace(ns, using, modname)
--===================module property========================

local function tokey(key)
    if type(key) == "number" then 
        return string.format("[%s]", tostring(key))
    else 
        return string.format("%s", tostring(key))
    end
end

local function tovalue(v)
    if type(v) == "string" then
        return string.format("\"%s\"", tostring(v))
    else
        return string.format("%s", tostring(v))
    end
end




--表->字符串
--@root
--@dep    扫描深度
--@no_cls  排除cls
function t2str(root, dep, no_cls)

    local Array = org.Array

    dep = dep or 3
    --log.debug(modname, flag)

    local cache = {  [root] = "." }
    local function _dump(t, space, name, cur_dep)

        cur_dep = cur_dep + 1

        local key, val, b, tp
        local tmp_arr = {}
        for k, v in pairs(t) do

            key = tostring(k)
            val = v
            b = true
            tp = nil

            if string.find(key, "_") == 1 then
                --私有成员不扫描
                b = false
            end

            if b and no_cls then
                --不打印class
                tp = type(val)
                if tp == "table" then

                    if val.cname then
                        --cls obj
                        val = val.cname .. " classobj"  --显示一下名称
                        tp = "string"
                    elseif getmetatable(val) then
                        b = false
                    end

                elseif tp == "function" then
                    b = false
                end
            end

            if b then
                tinsert(tmp_arr, {k=key, v=val, tp = tp or type(val)})
            end
        end

        Array.sortOn(tmp_arr, "k", Array.ASCENDING) --按名称升序

        local temp = {}
        local len = #tmp_arr
        for i=1, len  do
            local a = tmp_arr[i]
            local key, v, tp = a.k, a.v, a.tp
            local link
            --print(key)
            if i ~= len then   --有下一个
                link = i==1 and "┬" or "├"
            else
                link = i==1 and "-" or "└"
            end

            if cache[v] then
                --已经解析过,只提示路径
                tinsert(temp, link .. key .. " {" .. cache[v].."}") 

            elseif tp == "table" then

                local new_key = name .. "." .. key
                cache[v] = new_key
                if cur_dep > dep then
                    --超过扫描深度,省略
                    tinsert(temp, link .. key .." {...}" )
                else
                    tinsert(temp, link .. key .. _dump(v, space .. (i ~= len and "│" or " " ).. srep(" ", #key), new_key, cur_dep ) )
                end

            elseif tp == "function" then

                tinsert(temp,link.. key .. " (" .. tostring(v)..")")
            else

                tinsert(temp, link.. key .. " (" .. tostring(v)..")")
                --tinsert(temp, link.. key .. " = " .. tostring(v))
            end
        end

        if #temp == 0 then
            --没有符合的内容, 空表
            tinsert(temp, " {}")
        end

        return tconcat(temp, "\n"..space)
    end

    local result
    if type(root) == "table" then
        result = tostring(root) .. "\n" .. _dump(root, "","", 1)
    else
        result = tostring(root)
    end
    return result
end




--表->字符串(打印成lua格式)
function t2str2(root, dep, no_cls)

    local Array = org.Array

    dep = dep or 3
    --log.debug(modname, flag)

    local cache = {  [root] = "." }
    local function _dump(t, space, name, cur_dep)

        cur_dep = cur_dep + 1

        local key, val, b, tp
        local tmp_arr = {}
        for k, v in pairs(t) do

            key = tostring(k)
            val = v
            b = true    --是否打印
            tp =  type(val)

            if string.find(key, "_") == 1 then
                --私有成员不扫描
                b = false
            elseif tp == "function" then
                --不打印function
                b = false
            end

            if b and no_cls then
                --不打印class
                if tp == "table" then
                    if val.cname then
                        --cls obj
                        b = false
                    elseif getmetatable(val) then
                        --有元表
                        b = false
                    end
                end
            end

            if b then
                tinsert(tmp_arr, {k=tokey(k), v=val, tp = tp or type(val)})
            end
        end

        Array.sortOn(tmp_arr, "k", Array.ASCENDING) --按名称升序

        local temp = {}
        local len = #tmp_arr
        for i=1, len  do
            local a = tmp_arr[i]
            local key, v, tp = a.k, a.v, a.tp


            if cache[v] then
                --已经解析过,只提示路径
                tinsert(temp,  space .. key .. " = " .. tovalue(cache[v])..",") 

            elseif tp == "table" then

                local new_key = name .. "." .. key  --排除重复用
                cache[v] = new_key

                if cur_dep > dep then
                    --超过扫描深度,省略
                    tinsert(temp,  space .. key .." = \"...\"," )
                else
                    tinsert(temp,  space .. key .. " = {" )
                    tinsert(temp,   _dump(v, space .. " " .. srep(" ", #key), new_key, cur_dep ) )
                    tinsert(temp,  space .. "}," )
                end
            else

                tinsert(temp, space .. key .. " = " ..tovalue( v).. "," )
                --tinsert(temp, link.. key .. " = " .. tostring(v))
            end
        end

        --        if #temp == 0 then
        --            --没有符合的内容, 空表
        --            tinsert(temp, " {}")
        --        end

        return tconcat(temp, "\n" )
    end


    local result
    if type(root) == "table" then
        result = tostring(root) .. "\n{\n" .. _dump(root, " ","", 0) .. "\n}"
    else
        result = tostring(root)
    end
    return result
end


