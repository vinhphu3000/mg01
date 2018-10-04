--bev_const
--@author jr.zeng
--2017年4月17日 上午10:21:54
local modname = "bev_const"
--==================global reference=======================

--===================namespace========================
local ns = "org.bevtree"
local using = {--[[namespace1,...]]}
local _ENV = namespace(ns, using)
--===================module property========================

--行为节点状态
BEV_STATE = 
    {
        NONE    = 0,
        RUNNING = 1,    --进行中
        SUCCESS = 2,    --成功
        FAIL    = 3,    --失败
    }
local BEV_STATE = BEV_STATE

--节点状态名称
BEV_STATE2NAME = {}
for k,v in pairs(BEV_STATE) do
    BEV_STATE2NAME[v] = k
end



--节点类型
BEV_TYPE = 
    {
	    BASE            = "bev_base",
	    PARENT          = "bev_parent",
	    GOTO            = "GOTO",           --节点id(配置表用)

	    --composite
	    SEQUENCE        = "sequence",       --串行
	    PARALLEL        = "parallel",       --并行
        SELECT          = "selector",       --选择
	    RAND_SELECT     = "rand_selector",  --随机选择
	    IF_ELSE         = 'if_else',        --条件选择
	    LISTEN          = 'listen',         --等待事件

	    --decorate
	    NONE            = "bev_none",
	    LOOP            = "loop",           --循环

	    --action
	    IDLE            = "idle",           --空闲
	    WAIT            = "wait",           --延时
	    CALLBACK        = "callback",       --回调
	    AGENT_ACTION    = 'AgentAction',    --动作(agent)

	    --conditon
	    AGENT_COND      = 'AgentCond',      --条件(agent)

    }

--并行节点_成功策略
SUCC_POLICY =
{
	SUCC_ON_ONE = 0,
	SUCC_ON_ALL = 1,
}


--并行节点_失败策略
FAIL_POLICY =
{
	FAIL_ON_ONE = 0,
	FAIL_ON_ALL = 1,
}

--监听节点_进入子节点策略
LISTEN_CHLID_POLICY =
{
	ENTER_CHILD_IDLE = 0,   -- 子节点空闲才进入
	ENTER_CHILD_ALWAYS = 1, -- 收到事件就重进子节点
}


--监听节点_完成策略
LISTEN_DONE_POLICY =
{
	DONE_ON_CHILD = 0,      -- 子节点完成便完成
	RUNNING_FOREVER = 1,    -- 一直running
}


--操作符类型
OperateType =
{
	Assign = 'Assign',
	Add = 'Add',
	Sub = 'Sub',
	Mul = 'Mul',
	Div = 'Div',
	Equal = 'Equal',
	NotEqual = 'NotEqual',
	Greater = 'Greater',
	Less = 'Less',
	GreaterEqual = 'GreaterEqual',
	LessEqual = 'LessEqual',
	Invalid = 'Invalid',
}


--属性类型
PropertyType =
{
	--常量
	Const = 0,
	--agent的函数
	Method = 1,
	--agent的属性
	Property = 2,
}