local M = {}

local function split(str, sep)
    local s, e = str:find(sep)
    if s then
        return str:sub(0, s - 1), str:sub(e + 1)
    end
    return str
end

local function start_with(str, sep)
    return str:sub(0, #sep) == sep
end

function M.startWith(str, sep)
    return str:sub(0, #sep) == sep
end

function M.split_all(str, sep)
    local res = {}
    local lhs, rhs
    while true do
        lhs, rhs = split(str, sep)
        table.insert(res, lhs)
        if not rhs then
            break
        end
        str = rhs
    end
    return res
end

function M.parse_num_num_list(str, key_name, value_name)
    local ret = {}
    local it = string.gmatch(str, '[^ ]+')
    while true do
        local sub = it()
        if not sub then
            break
        end
        local num1, num2 = string.match(sub, '(%d+)|(%d+)')
        local entry = {}
        entry[key_name] = tonumber(num1)
        entry[value_name] = tonumber(num2)
        table.insert(ret, entry)
    end
    return ret
end

function M.string_trim(s)
    return string.gsub(s, "^%s*(.*)%s*$", "%1")
end

M.print_table = (function()
    local print = print
    local tconcat = table.concat
    local tinsert = table.insert
    local srep = string.rep
    local type = type
    local pairs = pairs
    local tostring = tostring
    local next = next

    return function(root)
        local cache = {  [root] = "." }
        local function _dump(t,space,name)
            local temp = {}
            for k,v in pairs(t) do
                local key = tostring(k)
                if cache[v] then
                    tinsert(temp,"+" .. key .. " {" .. cache[v].."}")
                elseif type(v) == "table" then
                    local new_key = name .. "." .. key
                    cache[v] = new_key
                    tinsert(temp,"+" .. key .. _dump(v,space .. (next(t,k) and "|" or " " ).. srep(" ",#key),new_key))
                else
                    tinsert(temp,"+" .. key .. " [" .. tostring(v).."]")
                end
            end
            return tconcat(temp,"\n"..space)
        end

        print(string.format('[[ <%s> ========', root))
        print(_dump(root, "",""))
        print(string.format('======== <%s> ]]', root))
    end
end)()

function M.error(fmt, ...)
    local info = debug.getinfo(2, "Sl")
    local s = info.source .. "@" .. info.currentline .. ":"  .. string.format(fmt, ...)
    error(s)
end

function M.parse_dates(s)
    s = s or ""
    if s == "" then
        return true, nil
    end

    local ret = {}
    for _, _s in ipairs(M.split_all(s, ";")) do
        local year_a, month_a, day_a, year_b, month_b, day_b = string.match(
            _s, "^(%d+)%.(%d+)%.(%d+)-(%d+)%.(%d+)%.(%d+)$"
        )
        if year_a == nil then
            return false, nil
        end

        local b_year = tonumber(year_a)
        local b_month = tonumber(month_a)
        local b_day = tonumber(day_a)
        local e_year = tonumber(year_b)
        local e_month = tonumber(month_b)
        local e_day = tonumber(day_b)

        if not(
            (b_year < e_year) or
            (b_year == e_year and b_month < e_month) or
            (b_year == e_year and b_month == e_month and b_day <= e_day)
        ) then
            return false
        end

        table.insert(
            ret,
            {b_year, b_month, b_day, e_year, e_month, e_day}
        )
    end
    return true, ret
end

function M.parse_weeks(weekdays)
    weekdays = weekdays or {}
    if not next(weekdays) then
        return true, nil
    end

    local ret = {}
    for _, wday in ipairs(weekdays) do
        if not (wday >= 1 and wday <= 7) then
            return false
        end

        wday = (wday % 7) + 1 -- 转成lua的wday, 周日是1
        ret[wday] = true
    end
    return true, ret
end

function M.parse_hours(s)
    s = s or ""
    if s == "" then
        return true, nil
    end

    local ret = {}
    for _, _s in ipairs(M.split_all(s, ";")) do
        local hour_a, min_a, hour_b, min_b = string.match(
            _s, "^(%d+):(%d+)-(%d+):(%d+)$"
        )
        if hour_a == nil then
            return false, nil
        end

        local begin_hour = tonumber(hour_a)
        local begin_min = tonumber(min_a)
        local end_hour = tonumber(hour_b)
        local end_min = tonumber(min_b)

        if not(
            (begin_hour < end_hour) or
            (begin_hour == end_hour and begin_min <= end_min)
        ) then
            return false
        end

        table.insert(
            ret,
            {begin_hour, begin_min, end_hour, end_min}
        )
    end

    return true, ret
end


function M.parse_time_range(s)
    local y_a, mon_a, d_a, h_a, min_a, s_a, y_b, mon_b, d_b, h_b, min_b, s_b = string.match(
        s,
        "^(%d+)%-(%d+)%-(%d+) (%d+)%:(%d+)%:(%d+)|(%d+)%-(%d+)%-(%d+) (%d+)%:(%d+)%:(%d+)$"
    )
    if y_a == nil then
        error("parse_fail")
    end

    local dt_a = {
        year = y_a,
        month = mon_a,
        day = d_a,
        hour = h_a,
        min = min_a,
        sec = s_a,
    }

    local dt_b = {
        year = y_b,
        month = mon_b,
        day = d_b,
        hour = h_b,
        min = min_b,
        sec = s_b,
    }

    local dt_list = {dt_a, dt_b}

    for _, dt in ipairs(dt_list) do
        for k, v in pairs(dt) do
            dt[k] = math.tointeger(v)
        end
        
        assert(dt.year >= 2018, "year")
        assert(dt.month >= 1 and dt.month <=12, "month")
        assert(dt.day >= 1 and dt.day <= 31, "day")
        assert(dt.hour >= 0 and dt.hour <= 23, "hour")
        assert(dt.min >= 0 and dt.day <= 59, "min")
        assert(dt.sec >= 0 and dt.sec <= 59, "sec")
    end

    local ts_a = os.time(dt_a)
    local ts_b = os.time(dt_b)
    assert(ts_a < ts_b, "begin time is larger than end time")
    
    return {from = dt_a, to = dt_b}
end


function M.table_setdefault(d, k, default)
    local v = d[k]
    if v ~= nil then
        return v
    end
    d[k] = default
    return default
end

function M.deepcopy(object)
    local lookup_table = {}
    local function _copy(object)
        if type(object) ~= "table" then
            return object
        elseif lookup_table[object] then
            return lookup_table[object]
        end
        local new_table = {}
        lookup_table[object] = new_table
        for index, value in pairs(object) do
            new_table[_copy(index)] = _copy(value)
        end
        return new_table
    end
    return _copy(object)
end

-- 字符串转其他单位
function M.change_unit(value, unit)
    if unit == "int" or unit == "float" then
        value = tonumber(value)
        if not value then
            return false
        end
        if unit == "int" then
            value = math.floor(value + 0.5)
        else
            value = math.floor(value * 100 + 0.5) / 100
        end
    end
    return value
end
-- 转下技能附加信息配置的格式
-- base_table [id value1 value2]
function M.parse_skill_add_info(base_data_list, skill_add_cfg)
    if #base_data_list == 0 then
        return true
    end
    local new_date_list = {}
    local value_name = "args"
    local split_name = "#"
    for _, base_data in ipairs(base_data_list) do
        local cfg = skill_add_cfg[base_data.id]
        local str = base_data[value_name]
        local value_list = M.split_all(str, split_name)
        -- 单位转换
        local units = cfg.unit
        local new_data = {id = base_data.id, [value_name] = {}}
        for i, unit in ipairs(units) do
            if not value_list[i] then
                return false
            end
            local value = M.change_unit(value_list[i], unit)
            -- print(value_list[i], unit)
            if value then
                new_data[value_name][i] = value
            else
                return false
            end
        end
        table.insert(new_date_list, new_data)
    end
    return true, new_date_list
end

local function cell_to_value(col_type, value, log_tag)
    if col_type == 'string' then
        return value
    elseif col_type == 'int' then
        return math.tointeger(value)
    elseif col_type == 'float' then
        return tonumber(value)
    elseif start_with(col_type, 'struct') then
        local tmp = M.split_all(M.split_all(col_type, "%(")[2], "%)")[1]
        local tmp_lst = M.split_all(tmp, "|")
        local val_lst = M.split_all(value, "|")

        local ret = {}
        if #tmp_lst > #val_lst then
            error(string.format("[%s]illegal len[%s]", log_tag, col_type))
        end
    for i,v in ipairs(tmp_lst) do
            local tt = M.split_all(v, "%[")
            local key = M.split_all(tt[2], "%]")[1]
            local sub_col_type = tt[1]
            ret[key] = cell_to_value(sub_col_type, val_lst[i], log_tag)
        end
        return ret
    elseif start_with(col_type, 'list') then
        local sub_col_type = M.split_all(M.split_all(col_type, "%<")[2], "%>")[1]
        local value_lst = M.split_all(value, ",")
        local ret = {}
        for i,v in pairs(value_lst) do
            ret[i] = cell_to_value(sub_col_type, v, log_tag)
        end
        return ret
    elseif start_with(col_type, 'xlist') then
        local sub_col_type = M.split_all(M.split_all(col_type, "%<")[2], "%>")[1]
        local value_lst = M.split_all(value, "|")
        local ret = {}
        for i,v in pairs(value_lst) do
            ret[i] = cell_to_value(sub_col_type, v, log_tag)
        end
        return ret
    else
        error(string.format("[%s]illegal type[%s]", log_tag, col_type))
    end
end

function M.load_const_table(data, log_tag)
    local ret = {}
    for k, v in pairs(data) do
        ret[v.name] = cell_to_value(v.type, v.value or v.val, log_tag)
    end
    return ret
end

function M.create_treasure_pet_npc(treasure_pets, treasure_pets_const, pets, monsters, npcs)
    local map = {}
    local model = npcs[treasure_pets_const.pet_npc_id]
    assert(model, "no treasure_pets_npc"..treasure_pets_const.pet_npc_id)
    local npc_id, monster_id
    local copy_monster_data_list = {"role_id"}--, "halfbody"}
    for pet_id, v in pairs(treasure_pets) do
        npc_id = v.npc_id
        assert(not npcs[npc_id], "error npc id".. npc_id)
        assert(not map[npc_id], "error npc_id"..npc_id)
        map[npc_id] = M.deepcopy(model)
        map[npc_id].id = npc_id
        monster_id = pets[pet_id].monster_id
        map[npc_id].name = string.format(map[npc_id].name, pets[pet_id].name)
        for _, key in ipairs(copy_monster_data_list) do
            map[npc_id][key] = monsters[monster_id][key]
        end
    end
    for key, v in pairs(map) do
        npcs[key] = v
    end
end

local function readfile(file)
    local fh, err = io.open(file , "rb")
    if not fh then
        print("readfile.err", file, err)
        return
    end

    local data = fh:read("*a")
    fh:close()
    return data
end

local function is_blocked2(block_map, x, y)
    local info = block_map
    if info then
        local px, py = x - info[3], y - info[4]
        px = math.floor(px*info[5] + 0.5)
        py = math.floor(py*info[5] + 0.5)

        if (px < 0 or px >= info[1]) or
            (py < 0 or py >= info[2]) then
            return true
        end
        local idx = (info[2] - py -1) * info[1] + px
        local idx2 = idx%63
        local idx1= math.floor((idx-idx2)/63 + 0.02)
        local val = info[6][idx1 +1]
        return (not val) and true or (not (( val & (1 << idx2))  > 0))
    end
    return true
end

function M.is_blocked(scene_name, x, y)
    local block_tex = 'block_texture/'.. scene_name .. ".lua.bytes"
    local block_data = readfile(block_tex)
    local block_map = load(block_data, block_tex, 't')()
    return is_blocked2(block_map, x, y)
end

return M
