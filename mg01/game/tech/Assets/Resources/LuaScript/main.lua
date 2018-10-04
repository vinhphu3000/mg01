--main
--@author jr.zeng
--2017年9月20日 上午10:03:47
local modname = "main"

function main()
	print('enter main')

	return require("src.game_init")
end



--在unity中使用lua,提升效率的准则

--1. 多使用局部变量,   
--  以下这种很耗时
--  local go = GameObject:Find("aaa")
--  for k,v in pairs(arr) do
--      local a = go:GetComponent("aaa")
--      a:XXX(v) 
--  end
--  高校的做法
--  local go = GameObject:Find("aaa")
--  local a = go:GetComponent("aaa")
--  for k,v in pairs(arr) do
--      a:XXX(c) 
--  end

--2.使用对象池
--典型的应用是每帧修改GameObject的位置
--function update(dt)
--    go.transform.position = Vec3(x,y,z)   --每帧都会创建Vec3, 如Vec3是Unity类, 消耗更大
--end
--此时最好为Vec3做对象池

--3.多个字符串拼接使用table.concat
--比使用".."连接高效
--原因是concat只会创建一个buffer, 而".."每次回产生一个新的字符串

--4.优化配置表
--优化结构, 减少内存, 但可读性会变差

--5.next比pairs耗时

--6.
