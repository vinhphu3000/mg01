--Vec3
--@author jr.zeng
--2017年9月23日 上午11:42:39
local modname = "Vec3"
--==================global reference=======================

local type = type
local math = math
local sqrt= math.sqrt
local acos= math.acos
local abs = math.abs


local Epsilon=0.0001
-- 0 ~ 180
local TO_DEGREE = 180 / 3.1415926

--===================namespace========================
local ns = "org"
local using = {--[[namespace1,...]]}
local _ENV = namespace(ns, using)
--===================module property========================
local super = nil	--父类
_ENV[modname] = class(modname, super)
local Vec3 = _ENV[modname]

local tmp_vec1 = Vec3.new()
local tmp_vec2 = Vec3.new()

--===================module content========================

function Vec3:ctor(x,y,z)

    self.x = x or 0
    self.y = y or 0
    self.z = z or 0
    
end


function Vec3:set(x,y,z)
	self.x = x or self.x
	self.y = y or self.y
	self.z = z or self.z
	return self
end

function Vec3:get()
	return self.x, self.y, self.z
end

function Vec3:pack()
	return {self.x, self.y, self.z}
end

function Vec3:unpack()
	return self.x, self.y, self.z
end

--//-------~★~-------~★~-------~★~数据管理~★~-------~★~-------~★~-------//


function Vec3:get_len()
    return sqrt(self.x^2+self.y^2+self.z^2)
end

function Vec3:get_lenSQ()
    return self.x^2+self.y^2+self.z^2
end

function Vec3:get_lenSQ_xz()
    return self.x^2 + self.z^2
end

--获取与指定向量的夹角
function Vec3:angle_off( vec )
    tmp_vec1:set(self.x, self.y, self.z)
    tmp_vec1:normalize()
    tmp_vec2:set(vec.x, vec.y, vec.z)
    tmp_vec2:normalize()
    local dot = Vec3.dot(tmp_vec1, tmp_vec2)
    return abs(acos(dot) * TO_DEGREE)
end

function Vec3:angle_off_xz( vec )
    tmp_vec1:set(self.x, 0.0, self.z)
    tmp_vec1:normalize()
    tmp_vec2:set(vec.x, 0.0, vec.z)
    tmp_vec2:normalize()
    local dot = Vec3.dot(tmp_vec1, tmp_vec2)
    return abs(acos(dot)*TO_DEGREE)
end

function Vec3:angle_off_signed( vec )
    tmp_vec1:set(self.x, self.y, self.z)
    tmp_vec1:normalize()
    tmp_vec2:set(vec.x, vec.y, vec.z)
    tmp_vec2:normalize()
    local dot = Vec3.dot(tmp_vec1, tmp_vec2)
    local y = tmp_vec1.x*tmp_vec2.z - tmp_vec1.z*tmp_vec2.x
    if y > 0 then
        return acos(dot)*TO_DEGREE
    end
    return -acos(dot)*TO_DEGREE
end

function Vec3:angle_off_signed_xz( vec )
    tmp_vec1:set(self.x, 0.0, self.z)
    tmp_vec1:normalize()
    tmp_vec2:set(vec.x, 0.0, vec.z)
    tmp_vec2:normalize()
    local dot = Vec3.dot(tmp_vec1, tmp_vec2)
    local y = tmp_vec1.x*tmp_vec2.z - tmp_vec1.z*tmp_vec2.x
    if y > 0 then
        return acos(dot)*TO_DEGREE
    end
    return -acos(dot)*TO_DEGREE
end

--//-------~★~-------~★~-------~★~数据操作~★~-------~★~-------~★~-------//

--单位化
function Vec3:normalize()
    local len = self:get_len()
    if abs(len -1) < Epsilon then
        
    elseif len > Epsilon then
        self.x,self.y,self.z = self.x/len,self.y/len,self.z/len
    else
        self:set(0,0,0)
    end
end

--单位化
--return 副本
function Vec3:normalize_clone(v)
    local len = self:get_len()
    if abs(len -1) < Epsilon then
        return self:clone()
    elseif len > Epsilon then
        return Vec3.new( self.x/len,self.y/len,self.z/len )
    else
        return Vec3.zero:clone()
    end
end

--获取距离
function Vec3:distance( vec )
    local dx, dy, dz = self.x-vec.x, self.y-vec.y, self.z-vec.z
    return sqrt(dx*dx+dy*dy+dz*dz)
end

--获取距离平方
function Vec3:distanceSQ( vec )
    local dx, dy, dz = self.x-vec.x, self.y-vec.y, self.z-vec.z
    return dx*dx + dy*dy + dz*dz
end

function Vec3:distance_xz( vec )
    local dx, dz = self.x-vec.x, self.z-vec.z
    return sqrt(dx*dx+dz*dz)
end

function Vec3:distanceSQ_xz( vec )
    local dx, dz = self.x-vec.x, self.z-vec.z
    return dx*dx+dz*dz
end

function Vec3:crossWith(vec1, vec2)
    return self:set(
        (vec1.y * vec2.z) - (vec1.z * vec2.y),
        (vec1.z * vec2.x) - (vec1.x * vec2.z),
        (vec1.x * vec2.y) - (vec1.y * vec2.x)  )
end


--点乘
function Vec3:dot(vec)
    return self.x * vec.x + self.y * vec.y + self.z* vec.z
end

--克隆
function Vec3:clone()
    return Vec3.new(self.x, self.y, self.z)
end


function Vec3:add( b )
    local tp = type(b)
    if tp == 'table' then
        self.x,self.y,self.z = self.x+b.x, self.y+b.y, self.z+b.z
        return
    elseif tp == 'number' then
        self.x,self.y,self.z = self.x+b, self.y+b, self.z+b
        return
    end
    nslog.error(modname, "wrong type:", tp, b)
end

function Vec3:sub( b )
    local tp = type(b)
    if tp == 'table' then
        self.x,self.y,self.z = self.x-b.x, self.y-b.y, self.z-b.z
        return
    elseif tp == 'number' then
        self.x,self.y,self.z = self.x-b, self.y-b, self.z-b
        return
    end
    nslog.error(modname, "wrong type:", tp, b)
end

function Vec3:mul( b )
    self.x,self.y,self.z = self.x*b, self.y*b, self.z*b
end

function Vec3:div( b )
    self.x,self.y,self.z = self.x/b, self.y/b, self.z/b
end

--//-------~★~-------~★~-------~★~循环利用~★~-------~★~-------~★~-------//

local pool = new(ClassPool, Vec3, true)
Vec3.pool = pool

local function alloc(x,y,z)
    local vec = pool:pop()
    vec.is_tmp = true
    vec:set(x,y,z)
    --nslog.debug(modname, "tmp", x, y, z)
    return vec
end

--根据输入决定返回结果是临时还是创建
local function alloc_r(v1, v2, x, y, z)
    if v1.is_tmp or (v2 and v2.is_tmp) then
	    --只要有一个是临时的,结果也分配临时的
        --nslog.debug(modname, "result is tmp",x,y,z)
        return alloc(x,y,z)
    end
    --nslog.debug(modname, "result is new",x,y,z)
    return Vec3.new(x,y,z)
end

--获取临时对象
--function Vec3.TMP(x,y,z)
--    return alloc(x,y,z)
--end

--函数式调用, 会返回临时对象
getmetatable(Vec3).__call = function(t,x,y,z)  
    return alloc(x,y,z)
end

--//-------~★~-------~★~-------~★~static~★~-------~★~-------~★~-------//

function Vec3.cross(vec1, vec2)
    return Vec3.new(
        (vec1.y * vec2.z) - (vec1.z * vec2.y),
        (vec1.z * vec2.x) - (vec1.x * vec2.z),
        (vec1.x * vec2.y) - (vec1.y * vec2.x)  )
end

--TODO 以下设计为不能修改xy 
Vec3.one          = cc.p3(1 ,1 ,1)
Vec3.zero         = cc.p3(0 ,0 ,0)
Vec3.up           = cc.p3(0 ,1 ,0)
Vec3.down         = cc.p3(0 ,-1 ,0)
Vec3.left         = cc.p3(-1 ,0 ,0)
Vec3.right        = cc.p3(1 ,0 ,0)
Vec3.forward      = cc.p3(0 ,0 ,1)
Vec3.back         = cc.p3(0 ,0 ,-1)


--//-------~★~-------~★~-------~★~元方法~★~-------~★~-------~★~-------//

--加
function Vec3.__add(a, b)
    local tp = type(b)
    if tp == 'table' then
        return alloc_r(a, b, 
            a.x + b.x,
            a.y + b.y,
            a.z + b.z)
            
    elseif tp == 'number' then
        return alloc_r(a, nil, 
            a.x + b,
            a.y + b,
            a.z + b)
    else
        nslog.error(modname, "wrong type:", tp, b)
    end
end

--减
function Vec3.__sub(a,b)
    local tp = type(b)
    if tp == 'table' then
        return alloc_r(a, b, 
            a.x - b.x,
            a.y - b.y,
            a.z - b.z)
    elseif tp == 'number' then
        return alloc_r(a, nil, 
            a.x - b,
            a.y - b,
            a.z - b)
    else
        nslog.error(modname, "wrong type:", tp, b)
    end
end

--乘
function Vec3.__mul(a,b)
    return alloc_r(a, nil, 
        a.x * b,
        a.y * b,
        a.z * b)
end

--除
function Vec3.__div(a,b)
    return alloc_r(a, nil, 
        a.x / b,
        a.y / b,
        a.z / b)
end

--取反
function Vec3.__unm( a )
    return alloc_r(a, nil, 
        -a.x, 
        -a.y, 
        -a.z)
end


return Vec3