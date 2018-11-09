local c = require "lxls.c"
local target_os = c.target_os

local M = {
    target_os = target_os,
    utf8_to_gbk = c.utf8_to_gbk,
    gbk_to_utf8 = c.gbk_to_utf8,
}


local wb_mt = {}

function wb_mt:sheet_count()
    local c_obj = self._c_obj
    return c_obj:get_sheetcount()
end


function wb_mt:sheet_name(idx)
    local c_obj = self._c_obj
    return c_obj:get_sheetname(idx)
end


function wb_mt:sheet_allnames()
    local c = self:sheet_count()
    local ret = {}
    for i=1, c do
        ret[i] = self:sheet_name(i)
    end
    return ret
end


function wb_mt:sheet_by_idx(idx)
    local c_obj = self._c_obj
    local sheet_cache = self._sheet_cache
    local sheet = sheet_cache[idx]
    if not sheet then
        sheet = c_obj:get_sheet(idx)
        sheet_cache[idx] = sheet
    end
    return sheet
end


function wb_mt:sheet_by_name(name)
    local c_obj = self._c_obj
    local name2idx = self._name2idx
    if not name2idx then
        name2idx = {}
        local c = self:sheet_count()
        for i=1,c do
            local name = self:sheet_name(i)
            name2idx[name] = i
        end
        self._name2idx = name2idx
    end

    local sheetidx = name2idx[name]
    if not sheetidx then
        return nil
    end
    return self:sheet_by_idx(sheetidx)
end


function M.workbook(xls_path)
    local c_workbook = c.workbook
    local wb_obj = c_workbook(xls_path)
    local raw = {
        _c_obj = wb_obj,
        _sheet_cache = {},
        _name2idx = false,
    }
    setmetatable(raw, {__index = wb_mt})
    return raw
end


function M.xls_printf(fmt, ...)
    local s = string.format(fmt, ...)
    if target_os == "windows" then
        s = c.utf8_to_gbk(s)
    end
    print(s)
end


function M.xls_print(...)
    if target_os == "windows" then
        local len = select("#", ...)
        local args = {}
        for i=1, len do
            local v = select(i, ...)
            if type(v) == "string" then
                v = c.utf8_to_gbk(v)
            end
            args[i] = v
            print(table.unpack(args))
        end
    else
        print(...)
    end
end


local function convert(sheet)
    local rows = sheet:get_totalrows()
    local cols = sheet:get_totalcols()
    local ret = {}
    for i=1, rows do
        local line = {}
        for k=1, cols do
            local v = sheet:get_cellvalue(i, k)
            line[k] = v
        end
        ret[i] = line
    end
    return ret
end


function M.xls_to_table(xls_path)
    local workbook = c.workbook(xls_path)
    local count = workbook:get_sheetcount()
    local ret = {}
    local map = {}
    for i=1, count do
        local sheet = workbook:get_sheet(i)
        local name = workbook:get_sheetname(i)
        local data = {
            name = name,
            table = convert(sheet),
        }
        ret[i] = data
        map[name] = data
    end
    return ret, map
end


return M
