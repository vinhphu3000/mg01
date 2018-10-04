local modname = "namespace"

local g = _G
local rawget = rawget
local rawset = rawset
local setmetatable = setmetatable
local setfenv = setfenv --lua5.1
local type = type

--local co = coroutine.running()
--local setlocal = debug.setlocal

--req_path = {}

string.split = function(str, sep)
    local tbl = {}
    for word in string.gmatch(str, '([^' .. sep .. ']+)') do
        table.insert(tbl, word)
    end
    --print("string.split", #tbl)
    return tbl
end


string.isNullOrEmpty = function(str)

	return str == nil or string.len(str) == 0
end

--合并数组,会排除重复项
local function concat_rip_dup(arr1, arr2, except)

    local result = {}
    local check = {}
    local a
    for i = 1, #arr1 do
        a = arr1[i]
        if a ~= except then
            if not check[a] then
                check[a] = true
                result[#result+1] = a
            end
        end
    end
    
    if arr2 then
        for i = 1, #arr2 do
            a =  arr2[i]
            if a ~= except then
                if not check[a] then
                    check[a] = true
                    result[#result+1] = a
                end
            end
        end
    end
    
    if #result > 0 then
        return result
    end
    
    return nil
end

--//-------~★~-------~★~-------~★~namespace~★~-------~★~-------~★~-------//

local __env = { __nsname__ = "env"}  
--__env.__ns__ = __env
g["g_ns"] = __env

local __name2env = {}
local __using_default = {   --默认using
	'org',
	'org.kui',
}

--创建命名空间
local function createNameSpaceWithNames(names_)

    local name
    local parent = __env
    local child
    
    for i=1, #names_ do
        name = names_[i]
        
        child = parent[name]
        if child == nil then
            child = {
                __upns__ = parent,  --记录父空间
                __prns__ = {},      --私有空间(在同一空间才能访问)
                --__nsname__ = name,
            }
            --child.__ns__ = child    --记录自己的引用
            parent[name] = child
        end
        parent = child
    end

    return child
end

local function createNameSpace(name_)

    local ns = __name2env[name_]
    if ns then
        return ns
    end

    local names = name_:split(".")
    ns = createNameSpaceWithNames(names)
    __name2env[name_] = ns
    --ns.__nsname_full__ = name_
    
    if __env.log then
        ns.__prns__.nslog = __env.log.get_nslog(name_) --空间日志
    end
    
    return ns
end

--获取命名空间
local function getNameSpace(name_)
    return __name2env[name_]
end

function namespace(name_, using_, modname)

    local cur
    if type(name_) == "string" then
        cur = createNameSpace(name_)
    else
        cur = __env --取根
        name_ = __env.__nsname__
    end
    
    local using
    if using_ and #using_ > 0 then
        using = concat_rip_dup(using_, __using_default, name_)
    else
        using = concat_rip_dup(__using_default, nil, name_)
    end

    --TODO get到val时, rawset到ns里, 下次可以省去__index, 缺点是不支持重名和修改常量
    
    local ns = {}
    
    if modname then
        --有指明modname, 创建一个nslog
        if __env.log then
            ns.nslog = __env.log.get_nslog(name_, modname)
        end
    end
    
    setmetatable(ns, {

            __index = function(tb, k)

--                if __env.log then 
--                    --排查有没有每帧调用
--                    __env.log.debug(modname, "g_v", k)
--                end
                
                local val
                
                --step1: 当前ns
                if cur.__upns__ then    --是根空间的话,延后到step3
                    --不是根空间
                    val = rawget(cur, k)    --先从当前空间取
                    --val = cur[k]
                    if val then
                        return val
                    end
                    
                    val = rawget(cur.__prns__, k)   --再从私有空间取 
                    if val then
                        return val
                    end
                end

                --step2: using
                if using then       --有指定其他空间, 从里面取
                    for i=1, #using do
                        local o = getNameSpace(using[i])
                        if o then
                            val = rawget(o, k)
                            --val = o[k]
                            if val then
                                return val
                            end
                        end
                    end
                end

                --step3: 往上查找
                --方案1, 更合理, 但消耗大一些
                local _ns = cur.__upns__
                if _ns then
                    while _ns do
                        --val = ns[k]
                        val = rawget(_ns, k)
                        if val then
                            return val
                        end
                        _ns = _ns.__upns__
                    end
                else
                
                    val = rawget(__env, k)    --这里才从跟__env取, 且没有私有空间
                    --val = cur[k]
                    if val then
                        return val
                    end
                end


                --方案2, 省略往上查找, 只找根, 消耗小一些
                --                if cur.__upns__ then
                --                    --不是根空间
                --                    val = __env[k]  --再从根空间取, 这种情况几率小一些
                --                    if val then
                --                        return val
                --                    end
                --                end

                
                --step4: G表
                val = rawget(g, k) --从G表取, 使用频率最高
                if val then
                    return val
                end

                return nil
            end,

            __newindex = function(tb, k, v)
                rawset(cur, k, v)
            end} )
    
    if setfenv then
        setfenv(2, ns)
    end
    
    --setlocal(co, 2, "_ENV", ns)
    return ns
end


function dump_namespace()

    --TODO
    g_ns.log.print_t("dump_namespace")
    
    g_ns.log.debug("dump_org", g_ns.t2str(g_ns.org,0)) 
end

