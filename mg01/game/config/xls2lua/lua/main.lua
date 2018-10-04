
require 'lua.util.functions'
require 'lua.util.dump_tbl'

local argv = ... or 'nil'
local argv = load( "return " .. argv )()
--print_t(argv)

argv = argv or {
	is_client = true,
	export_tmp = false,
}

local is_client = argv.is_client    --是否客户端导表
local is_export_tmp = argv.export_tmp  --是否导出临时文件

local path_raw_data_lua     --lua源文件目录
local path_raw_data_enum    --枚举源文件目录
local out_path_tmp          --合并后的lua临时输出目录
local out_path_export       --处理后的lua输出目录
local file_script_list      --导表脚本的路径
local path_script_list      --导表脚本的路径

local path_tmp = './tmp/'

if is_client then
	path_raw_data_enum  = path_tmp..'lua_data_enum_c/'
	path_raw_data_lua   = path_tmp..'lua_data_raw_c/'
	path_script_list    = './game_script_client/'
	--
	out_path_tmp        = path_tmp..'lua_data_tmp_c/'
	out_path_export     = './game_data_client/'
	--
	file_script_list    = './client_script_list'
else
	path_raw_data_enum  = path_tmp..'lua_data_enum_s/'
	path_raw_data_lua   = path_tmp..'lua_data_raw_s/'
	path_script_list    = './game_script_server/'
	--
	out_path_tmp        = path_tmp..'lua_data_tmp_c/'
	out_path_export     = './game_data_server/'
	--
	file_script_list    = './server_script_list'
end

local extra_include =
{
	'./tools/lua/?.so',
	'./tools/lua/?.dll',
}

package.cpath = package.cpath .. table.concat(extra_include, ';')

local log_str = ''

local _print = print
local function print(...)

	_print(...)

	local arr = {...}
	log_str = log_str .. table.concat(arr, '\t') .. '\n'
end

--//-------~★~-------~★~-------~★~os相关~★~-------~★~-------~★~-------//

local gbk2utf8  --转utf8
local lfs

local OS_TYPE = {
	WIN = 'win',
	MAC = 'mac',
	LNX = 'linux',
}

local os_type

--检测os
local function check_os()

	local home_dir = os.getenv('HOME')
	if home_dir then
		if string.find(home_dir, "Users/") then
			os_type = OS_TYPE.MAC
		elseif string.find(home_dir, "Users\\") then
			os_type = OS_TYPE.WIN
		elseif string.find(home_dir, "home/") then
			os_type = OS_TYPE.LNX
		else
			assert(false, string.format("unknown os type - %s", home_dir))
		end
	else
		os_type = OS_TYPE.WIN
	end

	print('os_type', os_type)

	if os_type == OS_TYPE.WIN then
		local encode_c = require('encode_c')
		gbk2utf8 = encode_c.gbk2utf8
	else
		gbk2utf8 = function(str) return str end
	end

	if os_type == OS_TYPE.MAC then
		lfs = require("lfsmac")
	else
		lfs = require('lfs')
	end

end



--//-------~★~-------~★~-------~★~枚举相关~★~-------~★~-------~★~-------//

local enum2tbl = {}
local enum2tbl2= {}

Enum = {}

--@key 支持传key或者value
function Enum.get(name, key_or_value)

	local tbl = enum2tbl[name]
	if not tbl then
		assert(false, string.format('找不到枚举表【%s】', name))
	end

	if type(key_or_value) == 'number' then
		key_or_value = math.modf(key_or_value)      --数字要取整
	end


	local value = tbl[key_or_value]
	if not value then
		value = tbl[tonumber(key_or_value)] --转number再尝试一下
	end

	if not value then
		tbl = enum2tbl2[name]               --传入的可能是value(意思是仅检查value的合法性)
		value = tbl[key_or_value]
		if not value then
			value = tbl[tonumber(key_or_value)] --转number再尝试一下
		end
	end

	if not value then
		--print_t(key_or_value, tbl)
		assert(false, string.format('在枚举表【%s】 找不到ID【%s】', name, key_or_value))
	end
	return value
end


--//-------~★~-------~★~-------~★~源文件相关~★~-------~★~-------~★~-------//

local function get_file_suffix(path)
	local name = path
	local suffix
	local index = path:find("%.")
	if index then
		name = string.sub(path, 1, index-1)
		suffix = string.sub(path, index)
	end
	return suffix or '', name
end

local exclude_key =
{
	['______IS__MAP______'] = true,
	['______DESC______'] = true,
}


--处理源文件
local function scan_all_file(path, name2config, is_sub)

	print('\nscan_all_file', path)

	local dir_arr

	for file_name in lfs.dir(path) do

		local file_path = path .. file_name
		local file_type = lfs.attributes(file_path, "mode")
		if file_type == 'directory' then

			if file_name ~= "." and file_name ~= ".." then
				dir_arr = dir_arr or {}
				dir_arr[#dir_arr+1] = file_path .. '/'
			end

		elseif file_type == 'file' then
			local suffix, name = get_file_suffix(file_name)
			if suffix == '.lua' then
				print(gbk2utf8(file_name))

				local tbl = require(path .. name)
				local real_name = name

				if is_sub then
					--分表
					local _start,_end,_name,_user = string.find(file_name, "(.+)_(.+)%.lua$")
					if _start then
						real_name = _name   --截取到原表名
					end

					--print('是分表', gbk2utf8(name), gbk2utf8(real_name),_start,_end)
					local config = name2config[gbk2utf8(real_name)]
					if config then
						--已经有这个表了，合并
						print(string.format('合并分表【%s】->【%s】', gbk2utf8(name), gbk2utf8(real_name)))

						local old_tbl = config.tbl
						for k, v in pairs(tbl) do

							if not exclude_key[k] then

								if config.is_map then
									--是字典
									if old_tbl[k] then
										assert(false, string.format('配置表【%s】 ID【%s】已经有值：%s',  gbk2utf8(name), k, t2str(old_tbl[k]) ))
									else
										old_tbl[k] = v
									end
								else
									--是数组
									old_tbl[#old_tbl+1] = v
								end
							end
						end
					else
						print(string.format('--WARINNING--WARINNING--WARINNING--WARINNING-- 找不到原配置表【%s】',  gbk2utf8(real_name) ))
						--assert(false, error)
					end
				else

					local config = {
						file_name = real_name,
						tbl = tbl,
						desc = tbl.______DESC______,
						is_map = tbl.______IS__MAP______,   --是否字典
					}

					for k,v in pairs(exclude_key) do
						tbl[k] = nil
					end

					name2config[gbk2utf8(real_name)] = config
				end

				--name = gbk2utf8(name)   --转为utf8再存

			end
		end
	end

	if dir_arr then
		for i=1, #dir_arr do
			scan_all_file(dir_arr[i], name2config, true)
		end
	end

end

--处理枚举文件
local function scan_all_enum(path)

	print('\nscan_all_enum')

	for file_name in lfs.dir(path) do

		local file_path = path .. file_name
		local file_type = lfs.attributes(file_path, "mode")
		if file_type == 'file' then
			local suffix, name = get_file_suffix(file_name)
			if suffix == '.lua' then
				print(gbk2utf8(file_path))
				local key2value = require(path .. name)
				name = gbk2utf8(name)   --转为utf8再存，不然Enum.get时会编码不对
				enum2tbl[name] = key2value

				local value2value = {}
				for k,v in pairs(key2value) do
					value2value[v] = v
				end
				enum2tbl2[name] = value2value
			end
		end
	end
end

local dump_tbl = dump_tbl
--导出配置文件
local function export_all_config(name2config, out_path)

	print('\nexport_all_config', out_path)

	for name, config in pairs(name2config) do
		local path = out_path .. config.file_name .. '.lua'
		print(gbk2utf8(path), '', config.desc)
		local str = dump_tbl(config.tbl)
		str = string.format('--%s\n',  config.desc) .. str  --加上注释
		local out_file = io.open(path, 'wb')
		out_file:write(str)
		out_file:close()
	end


end


--//-------~★~-------~★~-------~★~文件合并相关~★~-------~★~-------~★~-------//

--执行导表脚本
--@script_path 脚本列表的文件路径
local function excute_script_list(script_path, name2config)

	print('\nexcute_script_list', script_path)

	local name2new = {}

	local script_list = require(script_path)

	for i,item in ipairs(script_list) do

		local includes = item.includes          --需要引用的表
		local export_name = item.export_name    --输出的表名
		local need_script = item.need_script    --需要导表脚本

		local new

		if need_script then
			local ms = {}
			local desc_arr = {}
			for i,name in ipairs(includes) do
				local config = name2config[name]
				assert(config, string.format('找不到需要引用的表：%s', name))
				ms[name] = config.tbl
				desc_arr[#desc_arr+1] = config.desc
			end

			print(string.format('【%s】 -> %s', table.concat(desc_arr, '，'), export_name))

			local func = require(path_script_list..export_name)
			local tbl = func(ms)

			new = {
				file_name = export_name,
				tbl = tbl,
				desc = table.concat(desc_arr, ' ; '), --合并注释
			}
		else
			local name = includes[1]
			local config = name2config[name]
			if not config then
				assert(false, string.format('找不到需要引用的表：%s', name))
			end

			print(string.format('【%s】 -> %s', config.desc, export_name))

			new = {
				file_name = export_name,
				tbl = config.tbl,
				desc = config.desc,
			}
		end

		name2new[export_name] = new
	end

	return name2new
end

print('[main.lua] enter')

local function main()

	check_os()
	scan_all_enum(path_raw_data_enum)   --处理枚举

	local name2tmp = {}
	scan_all_file(path_raw_data_lua, name2tmp)    --处理源文件

	--lfs.mkdir(out_path_tmp)

	if is_export_tmp then
		--输出临时文件
		export_all_config(name2tmp, out_path_tmp)
	else
		print('\n不输出临时文件')
	end

	local name2new = excute_script_list(file_script_list, name2tmp)
	export_all_config(name2new, out_path_export)

	local path = path_tmp .. 'log.txt'
	local out_file = io.open(path, 'wb')
	out_file:write(log_str)
	out_file:close()

end

main()

print('\n[main.lua] complete')