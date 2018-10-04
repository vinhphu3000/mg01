--functions2
--@author jr.zeng
--2017年11月2日 下午9:06:09
local modname = "functions2"
--==================global reference=======================

--===================namespace========================
local ns = nil
local using = {--[[namespace1,...]]}
local _ENV = namespace(ns, using, modname)
--===================module property========================

local function test()
    
    def.foobar(table.self,number.x,number.y,string.name,table.arg,
        function(self,x,y,name,arg)
            print(self)
            return x+y
        end)
end

local meta_arg_tostring =
    {
        __tostring=function (t) 
            return t[1].." "..t[2] 
        end
    }
    


local function make_type(typename)

    return setmetatable({typename},{
    
        __tostring=function(t) 
            return typename 
        end,
        
        __index=function(t,k) 
            return setmetatable({typename,k}, meta_arg_tostring) 
        end,
    })
end

local meta_type={__index=function(t,k) return k end }
local const_table = function (t,k,v) error "const table" end

local type_gen=setmetatable({
    number=make_type "number",
    boolean=make_type "boolean",
    string=make_type "string",
    table=make_type "table",
    def=setmetatable({},{
        __index = function (t,k)
            return function(...)
                local vtbl=getfenv(3)
                vtbl.__meta[k]=arg
                vtbl[k]=setfenv(arg[#arg],_G)
                print(k,unpack(arg))    -- 输出函数的原型信息
            end
        end,
        __newindex = const_table
    })
},{__index=_G,__newindex = const_table } )

local class_vtbl = setmetatable ({},{__index=
    function (t,k)
        local ret={ __meta={} }
        local gen=setfenv(k,type_gen)
        setfenv(function () gen() end,ret)()
        ret={ __index=ret }
        t[k]=ret
        return ret
    end})

function create(class, obj)
    return setmetatable(obj or {},class_vtbl[class])
end