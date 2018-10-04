--Vec2
--@author jr.zeng
--2017年9月23日 上午11:42:44
local modname = "Vec2"
--==================global reference=======================

local sqrt = math.sqrt
local abs = math.abs
local Epsilon = 0.0001

--===================namespace========================
local ns = "org"
local using = {--[[namespace1,...]]}
local _ENV = namespace(ns, using)
--===================module property========================
local super = nil	--父类
_ENV[modname] = class(modname, super)
local Vec2 = _ENV[modname]

--===================module content========================

function Vec2:ctor(x,y)
    
    self.x = x or 0
    self.y = y or 0
end


function Vec2:set(x,y)
	self.x = x or self.x
	self.y = y or self.y
end

function Vec2:get()
	return self.x, self.y
end

function Vec2:pack()
	return {self.x, self.y}
end

function Vec2:unpack()
	return self.x, self.y
end

--//-------~★~-------~★~-------~★~数据管理~★~-------~★~-------~★~-------//

--获取长度
function Vec2:get_len()
    return sqrt(self.x*self.x + self.y*self.y)
end

--获取长度的平方
function Vec2:get_lenSQ()
    return self.x*self.x + self.y*self.y
end

--//-------~★~-------~★~-------~★~数据操作~★~-------~★~-------~★~-------//

--单位化
function Vec2:normalize( v )
    local len = self:magnitude(v)
    self.x = self.x / len
    self.y = self.y / len
end


--拷贝
function Vec2:clone()
    return Vec2.new(self.x, self.y)
end

--加
function Vec2:add(  b )
    self.x,self.y = self.x+b.x, self.y+b.y
end

--减
function Vec2:sub(  b )
    self.x,self.y = self.x-b.x, self.y-b.y
end

--乘
function Vec2:mul( b )
    self.x,self.y = self.x*b, self.y*b
end

--除
function Vec2:div( a, b )
    self.x,self.y = self.x/b, self.y/b
end

--//-------~★~-------~★~-------~★~static~★~-------~★~-------~★~-------//

--TODO 以下设计为不能修改xy 
Vec2.one    = cc.p(1,1)
Vec2.zero   = cc.p(0,0)
Vec2.up     = cc.p(0,1)
Vec2.down   = cc.p(0,-1)
Vec2.left   = cc.p(-1,0)
Vec2.right  = cc.p(1,0)


--//-------~★~-------~★~-------~★~元方法~★~-------~★~-------~★~-------//

--加
function Vec2.__add( a, b )
    return Vec2.new(a.x+b.x, a.y+b.y)
end

--减
function Vec2.__sub( a, b )
    return Vec2.new(a.x-b.x, a.y-b.y)
end

--乘
function Vec2.__mul( a, b )
    return Vec2.new(a.x*b, a.y*b)
end

--除
function Vec2.__div( a, b )
    return Vec2.new(a.x/b, a.y/b)
end

--等号
function Vec2.__eq( a, b )
    return abs(a.x-b.x) < Epsilon and abs(a.y-b.y) < Epsilon
end

--取反
function Vec2.__unm( a )
    return Vec2.new(-a.x, -a.y)
end

--函数式调用
function Vec2.__call(t,x,y)
    return Vec2.new(x,y)
end

--Vec2.__tostring = function(self)
--    return string.format('(lua) Vec2(%f,%f)', self.x, self.y)
--end


return Vec2