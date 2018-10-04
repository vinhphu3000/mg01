--tracedoc
--@author jr.zeng
--2017年8月23日 下午3:17:40
local modname = "tracedoc"

--基于云风的tracedoc, 改为观察者默认

--==================global reference=======================

local next = next
local pairs = pairs
local ipairs = ipairs
local setmetatable = setmetatable
local getmetatable = getmetatable
local type = type
local rawset = rawset
local table = table
local string = string

--===================namespace========================
local ns = "org"
local using = {--[[namespace1,...]]}
local _ENV = namespace(ns, using)

local xcall = xcall
local TableUtil = TableUtil
--===================module property========================

tracedoc = {}
local tracedoc = tracedoc

--===================module content========================

-- nil
local NULL = setmetatable({} , { 
    __tostring = function() 
        return "NULL" 
    end 
    }) 
    
tracedoc.null = NULL

local tracedoc_type = setmetatable({}, { 
    __tostring = function() 
        return "TRACEDOC" 
    end 
    })
    
local tracedoc_len = setmetatable({} , { __mode = "kv" })

local function doc_next(doc, last_key)

	local lastversion = doc._lastversion
	if last_key == nil or lastversion[last_key] ~= nil then
		local next_key, v = next(lastversion, last_key)
		if next_key ~= nil then
			return next_key, doc[next_key]
		end
		last_key = nil
	end

	local changes = doc._changes._keys
	while true do
		local next_key = next(changes, last_key)
		if next_key == nil then
			return
		end
		local v = doc[next_key]
		if v ~= nil and lastversion[next_key] == nil then
			return next_key, v
		end
		last_key = next_key
	end
end

local function doc_pairs(doc)
	return doc_next, doc
end

local function find_length_after(doc, idx)

	local v = doc[idx + 1]
	if v == nil then
		return idx
	end
	repeat
		idx = idx + 1
		v = doc[idx + 1]
	until v == nil
	tracedoc_len[doc] = idx
	return idx
end

local function find_length_before(doc, idx)

	if idx <= 1 then
		tracedoc_len[doc] = nil
		return 0
	end
	repeat
		idx = idx - 1
	until idx <=0 or doc[idx] ~= nil
	tracedoc_len[doc] = idx
	return idx
end

local function doc_len(doc)

	local len = tracedoc_len[doc]
	if len == nil then
		len = #doc._lastversion
		tracedoc_len[doc] = len
	end

	if len == 0 then
		return find_length_after(doc, 0)
	end

	local v = doc[len]
	if v == nil then
		return find_length_before(doc, len)
	end
	return find_length_after(doc, len)
end

--需要转为doc
--@return false:不检测内部变化
local function need_trans_doc(tbl)
    
    if getmetatable(tbl) then
        --有元表,一般是类对象
        return false 
    end
    
    if #tbl > 0 then
        --数组,
        return false
    end
    return true 
end

--new_index
--@t doc.changes
local function doc_change(t, k, v)

	local doc = t._doc	
	if not doc._dirty then
		doc._dirty = true
		local parent = doc._parent
		while parent do
			if parent._dirty then
				break
			end
			parent._dirty = true
			parent = parent._parent
		end
	end

	if type(v) == "table" then
	
        if need_trans_doc(v) then
            --需要监测内部变化
			local lv = doc._lastversion[k]
			if getmetatable(lv) ~= tracedoc_type then
				-- gen a new table
                v = tracedoc.create_doc(v)
				v._parent = doc
			else
			     
				local keys = {}
				for k in pairs(lv) do
					keys[k] = true
				end
				-- deepcopy v
				for k,v in pairs(v) do
					lv[k] = v
					keys[k] = nil
				end
				-- clear keys not exist in v
				for k in pairs(keys) do
					lv[k] = nil
				end
				return
			end
		end
	end

	rawset(t,k,v)	--直接rawset到changes,下次不会再进doc_change

	if v == nil then--处理置nil的情况
		
		if t._keys[k] then	-- already set
			--记录过
			doc._lastversion[k] = nil

		elseif doc._lastversion[k] == nil then	-- ignore since lastversion is nil
			--本来已经是nil
			return
		end
		doc._lastversion[k] = nil
	end

	t._keys[k] = true
	
--	if doc.m_autoUpdId then
--	   if doc.m_autoUpd then
    --            doc.m_autoUpdId = setTimeOut(nil, doc.update, doc)
--	   end
--	end
end


--//-------~★~-------~★~-------~★~class~★~-------~★~-------~★~-------//

--static
function tracedoc.new(init)
    
    local doc = tracedoc.create_doc(init)
	--attr
    rawset(doc, "m_changes", {})
    rawset(doc, "m_subject", new(Subject))
    --rawset(doc, "m_autoUpd", false) --自动更新
    --rawset(doc, "m_autoUpdId", 0)
	--function
    rawset(doc, "update", tracedoc.update)
    rawset(doc, "attach", tracedoc.attach)
    rawset(doc, "detach", tracedoc.detach)
    
	return doc
end

function tracedoc.ignore(doc, enable)
    rawset(doc, "_ignore", enable)  -- ignore it during commit when enable
end

function tracedoc.opaque(doc, enable)
    rawset(doc, "_opaque", enable)
end

--static
--@init 初始table
function tracedoc.create_doc(init)

    local doc = {
        _dirty = false,
        _parent = false,    --table会有父级
        _changes = { _keys = {} , _doc = nil }, --记录变量的主体
        _lastversion = {},  --记录旧值
    }

    doc._changes._doc = doc
    setmetatable(doc._changes, {
        __index = doc._lastversion, --rawget不到时,拿不到会拿旧值
        __newindex = doc_change,
    })

    setmetatable(doc, {
        __newindex = doc._changes,  --直接set到changes
        __index = doc._changes,
        __pairs = doc_pairs,
        __len = doc_len,
        __metatable = tracedoc_type,    -- avoid copy by ref
    })

    if init then
        for k,v in pairs(init) do
            doc[k] = v
        end
    end

    return doc
end

--@return {_n=1, [k]=v, ...}
function tracedoc.commit(doc, result, prefix)
    
    if doc._ignore then
		return result
	end

	doc._dirty = false
	local lastversion = doc._lastversion
	local changes = doc._changes
	local keys = changes._keys
	local dirty = false
	for k in pairs(keys) do
		local v = changes[k]
		local lastv = lastversion[k]
		keys[k] = nil
		if lastv ~= v or v == nil then
			dirty = true
			if result then
				local key = prefix and prefix .. k or k
				if v == nil then
					result[key] = NULL
				else
					result[key] = v
				end
				result._n = (result._n or 0) + 1
			end
			lastversion[k] = v
		end
		rawset(changes,k,nil)	-- don't mark k in keys
	end

	for k,v in pairs(lastversion) do

		if getmetatable(v) == tracedoc_type and v._dirty then
			if result then
				local key = prefix and prefix .. k or k
				local change
				if v._opaque then
					change = tracedoc.commit(v)
				else
					local n = result._n
					tracedoc.commit(v, result, key .. ".")
					if n ~= result._n then
						change = true
					end
				end
				if change then
					if result[key] == nil then
						result[key] = v
						result._n = (result._n or 0) + 1
					end
					dirty = true
				end
			else
				local change = tracedoc.commit(v)
				dirty = dirty or change
			end
		end
	end

	return result or dirty
end

--检测更新
function tracedoc.update(doc)

--    if doc.m_autoUpdId > 0 then
--        doc.m_autoUpdId = clear_timeout( doc.m_autoUpdId )
--    end
    
    if not doc._dirty then
        return end
       
    local changes = tracedoc.commit(doc, doc.m_changes)
    changes._n = changes._n or 0
    if changes._n == 0 then
        return end
    
    for k, v in pairs(changes) do
        doc.m_subject:notify(k, doc, v, changes)    --这里连"_n"也会派发。。。
    end
    
    for k,v in pairs(changes) do    --清空
        changes[k] = nil
    end
end

--监听属性变化
function tracedoc.attach(doc, key, func, target)
	doc.m_subject:attach(key, func, target)
end

function tracedoc.detach(doc, key, func, target)
	doc.m_subject:detach(key, func, target)
end

--清空数据
function tracedoc.clear(doc)
    
    --TODO
        
    doc.m_subject:detachAll()
end

--function tracedoc.dump(doc)
--
--	local last = {}
--	for k,v in next, doc._lastversion do
--		table.insert(last, string.format("%s:%s",k,v))
--	end
--	local changes = {}
--	for k,v in next, doc._changes do
--		if tostring(k):byte() ~= 95 then	-- '_'
--			table.insert(changes, string.format("%s:%s",k,v))
--		end
--	end
--	local keys = {}
--	for k in next, doc._changes._keys do
--		table.insert(keys, k)
--	end
--
--	return string.format("last [%s]\nchanges [%s]\nkeys [%s]",table.concat(last, " "), table.concat(changes," "), table.concat(keys," "))
--end

return tracedoc