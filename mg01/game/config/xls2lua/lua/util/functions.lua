--functions
--@author jr.zeng
--2017年4月14日 下午12:17:41
local modname = "functions"
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

--t2str
local string = string
local srep = string.rep
local tostring = tostring
local tconcat = table.concat
local tinsert = table.insert
--local print = print

--upvalue
local debug = debug
local getlocal = debug.getlocal

--===================module property========================



--//-------~★~-------~★~-------~★~print相关~★~-------~★~-------~★~-------//

--打印table(排除cls)
function print_t(...)

	local str = ""
	local arg = {...}
	local len = #arg
	if len > 0 then
		local v
		for i=1, len  do
			v = arg[i]
			if v == nil then
				str = str .. "nil  "
			elseif type(v) == "table" then
				local s = t2str(v, 4, true)   --默认扫描3层
				str = str .. "\n" .. s .. "\n"
			else
				str = str .. tostring(v) .. "  "
			end
		end
	end
	--str = debug.traceback(str, 2)
	print(str)
end



--表->字符串
--@root
--@dep    扫描深度
--@no_cls  排除cls
function t2str(root, dep, no_cls)


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
			tp = nil

			if no_cls then
				if string.find(key, "_") == 1 then
					--私有成员不扫描
					b = false
				end
			end

			if b and no_cls then
				--不打印class
				tp = type(val)
				if tp == "table" then

					if val.cname then
						--cls obj
						val = val.cname .. " classobj"  --只显示名称
						tp = "string"
					elseif getmetatable(val) then
						b = false
					end

				elseif tp == "function" then
					--不打印class的话, 也不打印function
					b = false
				end
			end

			if b then
				tinsert(tmp_arr, {k=key, v=val, tp = tp or type(val)})
			end
		end


		--Array.sortOn(tmp_arr, "k", Array.ASCENDING) --按名称升序

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

				local new_key = name .. "." .. key  --排除重复用
				cache[v] = new_key

				if cur_dep > dep then
					--超过扫描深度,省略
					tinsert(temp, link .. key .." {...}" )
				else
					tinsert(temp, link .. key .. _dump(v, space .. (i ~= len and "│" or " " ).. srep(" ", #key), new_key, cur_dep ) )
				end

			elseif tp == "function" then

				tinsert(temp, link.. key .. " (" .. tostring(v)..")")
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
		result = tostring(root) .. "\n" .. _dump(root, "","", 0)
	else
		result = tostring(root)
	end
	return result
end