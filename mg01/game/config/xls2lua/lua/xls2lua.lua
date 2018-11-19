--windows时，console为gbk编码

require 'lua.util.functions'
local dump_tbl = require 'lua.util.datadump'


local lxls = require "lxls"
local serialize = require "serialize"
local lfs = require "lfs"

local utf8_to_gbk = lxls.utf8_to_gbk
local target_os = lxls.target_os
local tointeger = math.tointeger
local floor = math.floor
local tostring = tostring
local assert = assert
local type = type
local pairs = pairs
local ipairs = ipairs
local sgsub = string.gsub
local sgmatch = string.gmatch
local smatch = string.match
local next = next
local _printf = lxls.xls_printf
local warning = _printf

local OPEN_LANG = false
local LANG_MAP = nil
local LANG_KEY = nil

local argv = ... or 'nil'
print(argv)
local argv = load( "return " .. argv )()
argv = argv or {}

local is_client = argv.client or false   --是否客户端导表
local is_combine = argv.combine or false --是否合拼分表
local xls_path = argv.xls_path or '../xls/' --xls文件读入目录

local out_path      = argv.out_path or false         --lua文件输出目录(raw)
local combine_path  = argv.combine_path or false     --lua文件输出目录(combine)
local export_path   = argv.export_path or false      --lua文件输出目录(final)

local path_tmp = './tmp/'

local file_script_list      --导表配置的路径
local path_script_list      --导表脚本的目录

if is_client then
	file_script_list    = './client_script_list'
	path_script_list    = './game_script_client/'
else
	file_script_list    = './server_script_list'
	path_script_list    = './game_script_server/'
end

local out_file
local function write_log(str)
	if out_file then
		out_file:write(str .. "\n")
	end
end

--每种类型的默认值
local default_tbl = {
	int = 0,
	float = 0.0,
	string = '',
	formula = '',
	format_string = '',
	bool = false,
	lang_string = '',
}

--以防万一
local fucking_number_str = {
	["１"] = 1,
	["２"] = 2,
	["３"] = 3,
	["４"] = 4,
	["５"] = 5,
	["６"] = 6,
	["７"] = 7,
	["８"] = 8,
	["９"] = 9,
	["１０"] = 10,
}


local escape_char = {
	['n'] = '\n',
	['t'] = '\t',
	['r'] = '\r',
	['\\'] = '\\',
	['\"'] = '\"',
	["\'"] = "\'",
}


--合法的属性标志
local legal_flags = {
	['default'] = true,
	['key'] = true,
	['ignored'] = true,
	["server"] = true,
	["client"] = true,
	["lang"] = true,
}



--//-------~★~-------~★~-------~★~tool~★~-------~★~-------~★~-------//

local log_str = ''

local function printf(fmt, ...)
	local s = string.format(fmt, ...)
	if target_os == "windows" then
		s = utf8_to_gbk(s)
	end
	--log_str = log_str .. s .. '\n'
	write_log(s)
	print(s)
end

local function errorf(fmt, ...)
	local s = string.format(fmt, ...)
	if target_os == "windows" then
		s = utf8_to_gbk(s)
	end
	write_log(s)
	return error(s)
end

local function assertf(b, fmt, ...)
	if not b then
		return errorf(fmt, ...)
	end
	return b
end

local function print_t(...)

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
	printf(str)
end


local sep = string.match (package.config, "[^\n]+")

--不管如何都会加一个"\"
local function combine(...)
	local t = {...}
	return table.concat(t, sep) --a,b/,c -> a\b/\c
end

local function split(s, c)
	local ret = {}
	if smatch(s, "^["..c.."]") then
		ret[1] = ""
	end

	local patt = "[^"..c.."]+"
	for v in sgmatch(s, patt) do
		ret[#ret+1] = v
	end
	return ret
end

local function is_empty(t)
	local k = next(t)
	return not k
end

--行列 -> [:]
local function tolocation(c, l)
	local v = l
	local ret = {}
	while true do
		v = v - 1
		local b = v % 26
		v = v // 26
		table.insert(ret, 1, string.char(string.byte('A') + b))
		if v == 0 then
			break
		end
	end
	l = table.concat(ret)
	return string.format("[%s:%s]", l, c)
end


local function try_make_dir(dir_path)
	if not dir_path then
		return end
	local ok = lfs.mkdir(dir_path)
	printf("create_dir %s  %s", dir_path, ok)
end


--//-------~★~-------~★~-------~★~file相关~★~-------~★~-------~★~-------//

--table -> 文件
local function table_to_file(t, path, comment)
	local s = serialize(t)
	if lxls.target_os == "windows" then
		path = lxls.utf8_to_gbk(path)
	end
	local fd = io.open(path, "wb")
	fd:write(comment or "")
	fd:write("\n\n")
	fd:write( "return ")
	fd:write(s)
	fd:close()
end

--table -> 文件
local function dump_to_file(t, path, comment)
	local s = dump_tbl(t)
	if lxls.target_os == "windows" then
		path = lxls.utf8_to_gbk(path)
	end
	local fd = io.open(path, "wb")
	fd:write(comment or "")
	fd:write("\n\n")
	fd:write(s)
	fd:close()
end

local path2wb = {}
local function get_workbook(xls_path)
	if not path2wb[xls_path] then

		printf('open workbook: %s', xls_path)

		local wb = lxls.workbook(xls_path)
		path2wb[xls_path] = wb
	end
	return path2wb[xls_path]
end

--收集所有xls
--
local function scan_all_xls(path, out_path, xls_list, is_sub)

	xls_list = xls_list or {}

	local dir_arr = {}

	for fileName in lfs.dir(path) do
		if fileName ~= "." and fileName ~= ".." then

			local filePath = combine(path, fileName)
			--local filePath = path .. fileName
			--print(filePath)

			local attr = lfs.attributes(filePath)
			if attr.mode == "directory" then
				local new_out_path = out_path and combine(out_path, fileName)
				dir_arr[#dir_arr+1] = {path=filePath, out_path=new_out_path}

			elseif smatch(fileName, ".+%.xls$") then

				if lxls.target_os == "windows" then
					filePath = lxls.gbk_to_utf8(filePath)
					fileName = lxls.gbk_to_utf8(fileName)
					out_path = out_path and lxls.gbk_to_utf8(out_path)
				end

				xls_list[#xls_list+1] = {
					name = fileName,
					xls_path = filePath,
					out_path = out_path,
					is_sub = is_sub,
				}
				printf("%s\t%s", filePath, fileName)
			end
		end
	end

	for i, obj in ipairs(dir_arr) do
		scan_all_xls(obj.path , obj.out_path, xls_list, true)
	end

	return xls_list
end

--查找所有子目录xls里，同名的sheet
local function fucking_find_same_sheet_and_xls(xls_list, xls_path, sheet_name)

	local ret = {}
	local xls_wb = get_workbook(xls_path)   --主xls
	local sheet = xls_wb:sheet_by_name(sheet_name)
	if sheet then
		ret[1] = {
			xls_path = xls_path,
			sheet_name = sheet_name,
		}
	else
		return ret
	end

	local wb

	for i,v in ipairs(xls_list) do      --查找所有子目录xls里，同名的sheet
		local name = v.name
		local is_sub = v.is_sub
		if is_sub then
			local xls_path = v.xls_path
			wb = get_workbook(v.xls_path)
			local all_names = wb:sheet_allnames()
			for _, v in ipairs(all_names) do
				if string.find(v, sheet_name.."_") then
					ret[#ret+1] = {
						xls_path = xls_path,
						sheet_name = v,
					}
				end
			end
		end
	end
	return ret
end

--//-------~★~-------~★~-------~★~类型相关~★~-------~★~-------~★~-------//

--生成字符串类型的值
local function gen_strf(t)

	return function (v, attr)
		if LANG_MAP and attr.lang then
			v = LANG_MAP[v] or v
		end

		if v then
			v = tostring(v)
			assert(v)
			if t ~= "format_string" and not smatch(v, '\n') then
				v = sgsub(v, "\\(.)", escape_char)
			end
			return v
		elseif attr.default then
			return default_tbl[t]
		end
	end
end

--基础类型转换
local internal_type =
{
	cellvalue = function (v)
		return v
	end,

	int = function (v, attr, c, l)
		if v then
			v = fucking_number_str[v] or v
			local numv = tonumber(v)
			if not numv then
				errorf("invalid int value:%s at%s", v, tolocation(c, l))
			end
			local vv = floor(v)
			if not vv then
				errorf("invalid int value:%s at%s", v, tolocation(c, l))
			end
			return vv
		elseif attr.default then
			return default_tbl.int
		end
	end,

	float = function (v, attr, c, l)
		if v then
			local vv = tonumber(v)
			if not vv then
				errorf("invalid float value:%s at%s", v, tolocation(c, l))
			end
			return vv
		elseif attr.default then
			return default_tbl.float
		end
	end,

	bool = function (v, attr)
		if v then
			return (v and (v==1 or v=='1') ) and true or false
		elseif attr.default then
			return default_tbl.bool
		end
	end,

	string = gen_strf("string"),
	formula = gen_strf("formula"),
	format_string = gen_strf("format_string"),
	lang_string = gen_strf("lang_string"),
}

--复合类型转换
local parser_types = {}

function parser_types.struct(s, c, l, enum_tbl)

	local x, fields = smatch(s, "^(x?)struct%(([^%)]+)%)?")     --struct(战斗属性[attr]|float[val] -> 战斗属性[attr]， float[val]    [^%)]：非')'的子集 ， ([^%)]+)匹配若干个子
	if fields then
		local ret = {
			type = "struct",
			fields = {},
			split_char = '|',
			strict = not (x and x =='x'),
		}
		for t, n in string.gmatch(fields, "([^%[%]|]+)%[([^%[%]|]+)%]") do  --战斗属性[attr] -> 战斗属性, attr
			if enum_tbl[t] or internal_type[t] then
				t = t
			else
				local lt = parser_types.list(t, c, l, enum_tbl)
				if not lt then
					errorf("invalid field type:%s header:%s at%s", t, s, tolocation(c, l))
				end
				t = lt
			end

			ret.fields[#ret.fields+1] = {
				field_type = t,
				field_name = n,
			}
		end
		assert(#ret.fields>0)
		return ret
	end
end

function parser_types.list(s, c, l, enum_tbl)
	local element_type = smatch(s, "^list.-<([^<>]+)>?") --开头list 不定个任意字符 ’<’ 任意个非<>的字符 0~1个’>‘
	if element_type then
		if enum_tbl[element_type] or internal_type[element_type] then
			return {
				type = "list",
				element_type = element_type,
				split_char = ',',
			}
		else
			local struct = parser_types.struct(element_type, c, l, enum_tbl)
			if struct then
				return {
					type = "list",
					element_type = struct,
					split_char = ',',
				}
			else
				errorf("invalid element_type:%s header:%s at%s", element_type, s, tolocation(c, l))
			end
		end
	end
end

function parser_types.xlist(s, c, l, enum_tbl)
	local element_type = smatch(s, "^xlist.-<([^<>]+)>?")
	if element_type then
		if enum_tbl[element_type] or internal_type[element_type] then
			return {
				type = "xlist",
				element_type = element_type,
				split_char = '|%s',
			}
		else
			local struct = parser_types.struct(s, c, l, enum_tbl)
			if struct then
				return {
					type = "xlist",
					element_type = struct,
					split_char = '|%s',
				}
			else
				errorf("invalid element_type:%s header:%s at%s",
				element_type, s, tolocation(c, l))
			end
		end
	end
end

--//-------~★~-------~★~-------~★~解析相关~★~-------~★~-------~★~-------//

local EMPTY = {}

--解析字段类型
local function parse_attr(sheet, col, enum_tbl, sheet_name)

	enum_tbl = enum_tbl or EMPTY

	local col_type = sheet:get_cellvalue(1, col)    --第一行：类型
	local col_name = sheet:get_cellvalue(2, col)    --第二行：变量名
	local col_comment = sheet:get_cellvalue(3, col) --第三行：描述

	-- check last col cell is empty
	-- wtf!!!!!!!!!!!!!!!!
	if not col_name and not col_comment then
		return nil
	end

	if not col_name then
		--没变量，但有描述
		return {
			name = "",
			ignored = true,
		}
	end

	col_type = col_type or "cellvalue"
	col_name = sgsub(col_name, "%s", "")    --去掉空格
	local real_num, real_name = smatch(col_name, "^(%d*)|(.+)$")    --10|attr_name  代表列表？

	local ts = split(col_type, "@") --string@key
	local raw_type = assert(ts[1])  --string

	local attr = {
		name = real_name or col_name,
		conv_type = false,
		comment = col_comment,
		list = not not real_name,       --？？
		list_count = real_num and tonumber(real_num) or 0,  --？？
	}

	-- set type
	if internal_type[raw_type] or enum_tbl[raw_type] then
		--基础类型 或 枚举
		attr.conv_type = raw_type

	else
		for k, f in pairs(parser_types) do
			local t = f(raw_type, 1, col, enum_tbl)
			if t then
				attr.conv_type = t
				goto FINISH_SET_TYPE
			end
		end
		errorf("unknow type:%s at%s", raw_type, tolocation(1, col))
	end
	::FINISH_SET_TYPE::

	--flag
	for i=2, #ts do
		local v = ts[i]
		if not legal_flags[v] then
			--不合法的flag
			errorf("error leagl flag: %s", v)
		end
		attr[v] = true
	end

	--lang
	if LANG_KEY and sheet_name and not attr.lang then
		if LANG_KEY[sheet_name] and LANG_KEY[sheet_name][col_name] then
			attr.lang = true
		end
	end
	return attr
end

--解析单个cell
local function dump_cell(raw_v, attr, c, l, enum_tbl)

	local t = attr.conv_type
	local f = internal_type[t]
	if f then
		--基础类型
		local v =  f(raw_v, attr, c, l)
		--if v == nil then
		--	errorf("nil value:%s at%s", v, tolocation(c, l))
		--end
		return v
	elseif enum_tbl[t] then
		--枚举
		local custom = enum_tbl[t]
		if raw_v == nil and attr.default then
			return default_tbl.int
		end
		local v = custom and (custom[raw_v] or custom[tonumber(raw_v)])
		if not v then
			errorf("无效的枚举:%s  类型:%s 列名:%s 位置:%s", raw_v, t, attr.name, tolocation(c, l))
		end
		return v
	elseif type(t) == "table" then
		--复合结构
		local tt = t.type       --struct(物品类型[id]|int[amount])
		local ret = {}
		if raw_v==nil then
			-- return attr.default and {} or nil
			return ret  --默认值是空表
		end
		local values = split(raw_v, t.split_char)   --1003|100000
		if tt == "struct" then
			for i,v in ipairs(t.fields) do
				local vv = values[i]
				if vv then
					local fn = v.field_name     --id
					local ft = v.field_type     --物品类型

					local sub_attr = {
						conv_type = ft,
						name = fn,
						lang = attr.lang,
					}

					local cv = dump_cell(vv, sub_attr, c, l, enum_tbl)
					ret[fn] = cv
				elseif attr.strict then
					errorf("struct data no empty value:%s at%s", raw_v, tolocation(c, l))
				end
			end
			return ret
		elseif tt == "xlist" or tt == "list" then
			for i,v in ipairs(values) do
				local et = t.element_type
				--- fucking fix!!!  zx2_wanjia 任务_客户端刷NPC_ljw.lua replace space
				if et == "string" then
					v = sgsub(v, "%s*$", "") --去掉两边空格
					v = sgsub(v, "^%s*", "")
				end

				local sub_attr = {
					conv_type = et,
					name = attr.name,
					lang = attr.lang,
				}

				local cv = dump_cell(v, sub_attr, c, l, enum_tbl)
				ret[i] = assert(cv)
			end
			return ret
		else
			errorf("unknow collection type:%s at%s", tt, tolocation(c, l))
		end

	else
		errorf("invalid type:%s comment:%s value:%s at%s", attr.name, attr.comment, raw_v, tolocation(c, l))
	end
end

--local function sheet_to_table(sheet, enum_tbl, sheet_name, xls_path)
local function sheet_to_table(xls_path, sheet_name, enum_tbl )

	local wb = get_workbook(xls_path)
	local sheet = wb:sheet_by_name(sheet_name)
	enum_tbl = enum_tbl or EMPTY

	local rows = sheet:get_totalrows()
	local cols = sheet:get_totalcols()

	local ret = {}
	local header = {}
	local main_key = nil
	local main_col = nil

	local header_map = {}
	for i=1, cols do    --A|B|C|D|...

		local attr = parse_attr(sheet, i, enum_tbl, sheet_name)     --解析类型文本
		if not attr then
			--空列, 后面跳过
			cols = i-1
			break
		end

		header[i] = attr
		local attr_name = attr.name
		if not attr.ignored and not attr.list then
			if header_map[attr_name] then
				errorf("重复的列名:%s 位置:%s 页:%s  路径:%s", attr_name, tolocation(2, i), sheet_name, xls_path)
			end
			header_map[attr_name] = attr
		end
		if attr.key then
			--主键
			main_col = i
			main_key = attr_name
		end

		if not main_key and attr.name == "id" then
			--没定义键，取名为"id"的键
			main_key = attr.name
		end
	end

	-- check need dump cols
	local first_not_ignore_col_idx = nil
	local ignore_cols = {}
	for i=1, cols do
		ignore_cols[i] = true
		local attr = header[i]
		if attr.ignored then
			goto NEXT_LOOP
		end

		if is_client and attr.server and not attr.client then
			goto NEXT_LOOP
		end

		if not is_client and attr.client and not attr.server then
			goto NEXT_LOOP
		end

		ignore_cols[i] = false
		::NEXT_LOOP::
		if not ignore_cols[i] and not first_not_ignore_col_idx then
			first_not_ignore_col_idx = i
		end
	end

	-- empty sheet
	if not first_not_ignore_col_idx then
		return ret
	end

	-- print("rows", rows, "cols", cols, first_not_ignore_clo_idx)
	-- dump all sheet
	for i=4, rows do
		-- check last row is empty cell
		if not sheet:get_cellvalue(i, first_not_ignore_col_idx) and
			not header[first_not_ignore_col_idx].default then
			--往下没有值了，后面忽略
			break
		end

		local entry = {}
		local is_end = true
		local name2list = {}
		local list2len = {}
		for k=1, cols do

			if ignore_cols[k] then
				--忽略
				goto CONTINUE
			end

			local attr = header[k]
			local attr_name = attr.name

			--不忽略的
			local raw_v = sheet:get_cellvalue(i, k)
			if raw_v and is_end then
				is_end = false
			end


			local v = dump_cell(raw_v, attr, i, k, enum_tbl)    --解析单元格
			if attr.list then
				local lc = attr.list_count
				local list = nil
				if lc>0 or raw_v or attr.default then
					list = name2list[attr_name] or {}
					entry[attr_name] = list
				end
				if not v then
					errorf("invalid value:%s at%s", v, tolocation(i, k))
				end
				if raw_v then
					list2len[list] = #list +1
				end
				list[#list +1] = v
				name2list[attr_name] = list
			else
				entry[attr_name] = v
			end

			::CONTINUE::
		end

		-- clear list 为什么
		for name, list in pairs(name2list) do
			local len = list2len[list] or 0
			for i = len+1, #list do
				list[i] = nil
			end
		end

		if main_key then
			--有主键
			local k = entry[main_key]
			if ret[k] then
				--键值重复
				errorf("duplicat key:%s at%s", k, tolocation(i, main_col))
			end
			if not is_end then
				ret[k] = entry
			end
			ret["______IS__MAP______"] = true
		else
			--列表
			if not is_end then
				ret[#ret+1] = entry
			end
		end

		if is_end then
			break end
	end


	return ret
end

local function dump_workbook(xls_path, enum_tbl, out_path, is_sub)

	local wb = get_workbook(xls_path)
	local c = wb:sheet_count()
	for i=1, c do

		local sheet_name = wb:sheet_name(i)

		local sheet = wb:sheet_by_idx(i)
		local rows = sheet:get_totalrows()
		if rows > 1 then

			local lua_path
			if out_path then
				lua_path = combine(out_path, sheet_name ..".lua")
				printf("    sheet:%s  --> %s", sheet_name, lua_path)
			else
				printf("    sheet:%s ", sheet_name)
			end

			local ret = sheet_to_table(xls_path, sheet_name, enum_tbl)

			if lua_path then

				local comment = string.format("-- %s", xls_path)

				if is_sub then
					--是子表，创建子目录
					try_make_dir(out_path)
				end

				table_to_file(ret, lua_path, comment)
			end
		end
	end
end

--收集分表
local function combine_xls_list(xls_list)

	local ret = {}
	for i,v in ipairs(xls_list) do

		if not v.is_sub then

			local wb = get_workbook(v.xls_path)
			local all_names = wb:sheet_allnames()

			local name2sheets = {}

			for _, sheet_name in ipairs(all_names) do

				local sheets = fucking_find_same_sheet_and_xls(xls_list, v.xls_path, sheet_name)
				-- table.sort(sheet, function(a, b)
				--         return a.xls < b.xls
				--     end)
				name2sheets[sheet_name] = sheets
			end

			ret[#ret+1] = {
				name = v.name,
				xls_path = v.xls_path,
				out_path = v.out_path,
				sheets = name2sheets,
			}
		end
	end
	return ret
end

local function dump_combine(xls_obj, enum_tbl, name2config)

	local wb = get_workbook(xls_obj.xls_path)
	local sheet_count = wb:sheet_count()

	local out_path = xls_obj.out_path
	local name2sheets = xls_obj.sheets

	local function get_table(xls_path, sheet_name)
		local wb = get_workbook(xls_path)
		local sheet = wb:sheet_by_name(sheet_name)
		printf("        sheet:%s from xls:%s", sheet_name, xls_path)
		local ret = sheet_to_table(xls_path, sheet_name, enum_tbl)
		return ret
	end

	-- force convert array
	--local ToArray = {
	--	["ai策略"] = true,
	--	["ai状态机"] = true,
	--	["ai事件"] = true,
	--}

	-- check array key
	--local ARRAY_KEY_CHECK = {
	--	["抽取规则"] = "rule_id",
	--}

	--local function check_array_key(sub_sheet, check_key)
	--	local all_keys_map = {}
	--	for i, t in ipairs(sub_sheet) do
	--		for i,v in ipairs(t.sheet) do
	--			local check_v = v[check_key]
	--			if all_keys_map[check_v] then
	--				errorf("duplicat key:%s value:%s at sheet:%s of xls:%s", check_key, v[check_key], t.sheet_name, t.xls)
	--			end
	--		end
	--		for i,v in ipairs(t.sheet) do
	--			local check_v = v[check_key]
	--			all_keys_map[check_v] = true
	--		end
	--	end
	--end

	--local function force_to_array(t)
	--	local ret = {}
	--	for k,v in pairs(t) do
	--		ret[#ret+1] = {
	--			k = k,
	--			v = v,
	--		}
	--	end
	--	table.sort(ret, function (a, b)
	--		return a.k < b.k
	--	end)
	--
	--	for i=1, #ret do
	--		ret[i] = ret[i].v
	--	end
	--	return ret
	--end


	local function combine_join(sub_sheets, sheet_name)

		local ret = {}

		-- ret["______IS__MAP______"] = sub_sheet[1].sheet["______IS__MAP______"]

		--检测字典与数组的冲突
		local is_map = 1
		for i,v in ipairs(sub_sheets) do
			if not is_empty(v.sheet) then   -- get the first not empty table
				if is_map == 1 then
					is_map = v.sheet["______IS__MAP______"]
				else
					if v.sheet["______IS__MAP______"] ~= is_map then
						errorf("conflict table type at sheet:%s of xls:%s", v.sheet_name, v.xls)
					end
				end
				v.sheet["______IS__MAP______"] = nil    --去掉原来的标志
			end
		end

		if is_map then
			--字典
			for i, t in ipairs(sub_sheets) do
				for k, v in pairs(t.sheet) do
					if ret[k] ~= nil then
						errorf("conflict key:%s at sheet:%s of xls:%s", k, t.sheet_name, t.xls)
					end
					ret[k] = v
				end
			end
		else

			--local check_key = ARRAY_KEY_CHECK[target_name]
			--if check_key then
			--	check_array_key(sub_sheets, check_key)
			--end

			for i, t in ipairs(sub_sheets) do
				for i,v in ipairs(t.sheet) do
					ret[#ret+1] = v
				end
			end
		end

		-- fucking force to array
		--if ToArray[sheet_name] then
		--	ret = force_to_array(ret)
		--end

		return ret, is_map
	end

	name2config = name2config or {}

	for i=1, sheet_count do

		local name = wb:sheet_name(i)
		local sheet = wb:sheet_by_idx(i)
		local rows = sheet:get_totalrows()
		if rows > 1 then
			--local lua_path = out_path and combine(out_path, name..".lua")
			local lua_path = combine_path and combine(combine_path, name..".lua")
			local sub_sheets = {}
			local comment = {}      --描述
			local sheets = name2sheets[name]    --sheets的第一个是主sheet
			printf("    sheet:%s --> %s", name, lua_path)
			for i,v in ipairs(sheets) do
				--printf( t2str(v) )
				sub_sheets[i] = {
					sheet = get_table(v.xls_path, v.sheet_name),
					sheet_name = v.sheet_name,
					xls_path = v.xls_path,
				}
				comment[i] = string.format("-- %s sheet_name:%s", v.xls_path, v.sheet_name)
			end

			local join_sheet, is_map = combine_join(sub_sheets, name)
			local comment_s = table.concat(comment, "\n")

			local config = {
				file_name = name,
				tbl = join_sheet,
				desc = comment_s,
				is_map = is_map,   --是否字典
			}

			--printf(t2str(config))

			name2config[name] = config

			if lua_path then
				table_to_file(join_sheet, lua_path, comment_s)
			end

		end
	end

end

--//-------~★~-------~★~-------~★~枚举相关~★~-------~★~-------~★~-------//

local function sheet_to_enum2val(xls_path, sheet_name, enum_name, no_name, ret)

	local item_wb = get_workbook(xls_path)
	local sheet = item_wb:sheet_by_name(sheet_name)
	if not sheet then
		errorf('找不到枚举的sheet：%s %s %s', xls_path, sheet_name, enum_name)
		return false
	end

	local cols = sheet:get_totalcols()
	local id_col, name_col
	for i=1, cols do
		local col_name = sheet:get_cellvalue(2, i)
		if col_name == "id" then
			id_col = i
		elseif col_name == "name" then
			name_col = i
		end
		if id_col and name_col then
			break
		end
	end

	if not id_col then
		errorf("need idx at %s %s %s", xls_path, sheet_name, enum_name)
	end
	ret = ret or {}
	local id_attr = parse_attr(sheet, id_col, nil, nil)
	local name_attr = not no_name and parse_attr(sheet, name_col, nil, nil)
	local rows = sheet:get_totalrows()

	for r =4, rows do
		local raw_id = sheet:get_cellvalue(r, id_col)
		if not raw_id then
			break
		end

		local id = dump_cell(raw_id, id_attr, r, id_col, nil)
		local key
		if not no_name then
			local raw_name = sheet:get_cellvalue(r, name_col)
			local name = dump_cell(raw_name, name_attr, r, name_col, nil)
			key = name
		else
			--不使用name，直接id做key
			key = id
		end

		if ret[key] or ret[id] then
			errorf("duplicat enum type:%s value:%s at sheet:%s%s of %s", enum_name, key, sheet_name, tolocation(r, id_col), xls_path)
		end

		ret[key] = id
		ret[id] = id
		ret[tostring(id)] = id
	end

	return ret
end

--转换枚举表
local function convet_enumerate(xls_path, xls_list, ret)

	local enum_path = combine(xls_path, "enumerate.xls")
	local data = sheet_to_table(enum_path, "enumerate")
	--print_t(data)

	ret = ret or {}
	for i,v in ipairs(data) do
		local enum_name = v.enum_name   --实际使用的枚举名
		local sheet_name = v.sheet_name
		local no_name = v.no_name > 0   --不使用名称

		local file_path = combine(xls_path, v.file_name)
		local list = fucking_find_same_sheet_and_xls(xls_list, file_path, sheet_name)    --查找所有指定xls的sheet
		for _, item in ipairs(list) do
			local xlsPath = item.xls_path
			local sheet_name = item.sheet_name
			
			local enum2val = ret[enum_name] or {}  --可能有多个sheet合并到一个enum
			enum2val = sheet_to_enum2val(xlsPath, sheet_name, enum_name, no_name, enum2val)
			if enum2val then
				ret[enum_name] = enum2val
			end
		end
	end

	if out_path then
		table_to_file(ret, combine(path_tmp, "enum.lua"))
	end

	return ret
end

--//-------~★~-------~★~-------~★~导表脚本相关~★~-------~★~-------~★~-------//

local function excute_script_list(script_path, name2config)

	local name2new = {}

	local script_list = require(script_path)

	for i,item in ipairs(script_list) do

		local includes = item.includes          --需要引用的表
		local export_name = item.export_name    --输出的表名
		local need_script = item.need_script    --需要导表脚本
		local lua_path = export_path and combine(export_path, export_name..".lua")

		local new

		if need_script then
			local ms = {}
			local desc_arr = {}
			for i,name in ipairs(includes) do
				local config = name2config[name] or name2new[name]
				assertf(config, string.format('找不到需要引用的表：%s', name))
				ms[name] = config.tbl
				desc_arr[#desc_arr+1] = config.desc
			end

			printf('%s -> %s', table.concat(desc_arr, '，'), export_name)

			local func = require(path_script_list..export_name) --获取导表脚本
			local tbl = func(ms)

			new = {
				file_name = export_name,
				tbl = tbl,
				desc = table.concat(desc_arr, '\n'), --合并注释
			}
		else
			local name = includes[1]
			local config = name2config[name]
			if not config then
				assertf(false, string.format('找不到需要引用的表：%s', name))
			end

			printf('%s -> %s', config.desc, export_name)

			new = {
				file_name = export_name,
				tbl = config.tbl,
				desc = config.desc,
			}
		end

		assertf(new.tbl, '导表脚本没有返回 %s', export_name)

		name2new[export_name] = new

		if lua_path then
			dump_to_file(new.tbl, lua_path, new.desc, true)
		end
	end

	return name2new
end

--//-------~★~-------~★~-------~★~main~★~-------~★~-------~★~-------//

print('[xls2lua] enter')


local function main()

	local time = os.time()

	try_make_dir(out_path)
	try_make_dir(combine_path)
	try_make_dir(export_path)

	local path = path_tmp .. 'log42.txt'
	out_file = io.open(path, 'wb')

	printf('is_client %s', is_client)
	printf('is_combine %s', is_combine)
	printf('xls_path %s', xls_path)
	printf('out_path %s', out_path)
	printf('combine_path %s', combine_path)
	printf('export_path %s', export_path)
	printf('\n')

	--print(package.config)

	printf('scan xls begin--------')

	local xls_list = scan_all_xls(xls_path, out_path)

	printf('scan xls finish--------\n')

	if OPEN_LANG  then
		--TODO
	end

	printf('check sheet begin--------')

	--处理枚举
	local enum_tbl = convet_enumerate(xls_path, xls_list, {})

	--检查sheet重复
	local sheetName2xls = {}
	for i,v in ipairs(xls_list) do

		local xls_path = v.xls_path
		local wb = get_workbook(xls_path)
		local names = wb:sheet_allnames()
		for _, name in ipairs(names) do
			local sheet = wb:sheet_by_name(name)
			local first_name = sheet:get_cellvalue(2, 1)
			if first_name then
				if sheetName2xls[name] then
					errorf("重复的页名:%s at %s and %s", name, xls_path, sheetName2xls[name])
				end
				sheetName2xls[name] = xls_path
			end
		end
	end

	printf('check sheet finish--------\n')

	if out_path then
		--输出源数据
		printf('convert raw lua begin--------')

		for i,v in ipairs(xls_list) do
			printf("-------> xls: %s [%d/%d] %s", v.xls_path, i, #xls_list, v.out_path)
			dump_workbook( v.xls_path, enum_tbl, v.out_path, v.is_sub)
		end

		printf('convert raw lua finish--------\n')
	end

	if is_combine then
		--输出合并完的数据
		local name2config = {}

		printf('combine lua begin--------')

		local combine_list = combine_xls_list(xls_list)
		--printf(t2str(combine_list))
		for i, detail in ipairs(combine_list) do
			printf("-------> xls: %s [%d/%d]", detail.xls_path, i, #combine_list)
			dump_combine(detail, enum_tbl, name2config)
		end

		printf('combine lua begin--------\n')

		if export_path then
			--输出最终数据
			printf('excute script begin--------')

			excute_script_list(file_script_list, name2config)

			printf('excute script finish--------\n')
		end
	end

	printf('耗时：%s秒\n', os.time()-time)

	out_file:close()
end



main()



print('[xls2lua] complete')