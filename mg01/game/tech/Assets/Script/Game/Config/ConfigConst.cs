/* ==============================================================================
 * 配置表常量
 * @author jr.zeng
 * 2016/9/17 10:23:27
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using mg.org;

public class ConfigConst
{

    //配置表注册
    static public DataConfigReg[] CONFIG_REQS = new DataConfigReg[]
    {
        //new DataConfigReg("transfer_pos",  typeof(TestCfgData), ConfigSample.InitBase),
        //new DataConfigReg("test_config",  typeof(TestCfgData), ConfigSample.InitBase),
        new DataConfigReg("stage_config",  typeof(StgCfgInfo), ConfigStage.InitStgCfg),
        new DataConfigReg("battle_config",  typeof(BttCfgInfo), ConfigBattle.InitBTCfg),
        new DataConfigReg("scene_config",  typeof(SceneCfgInfo), ConfigScene.InitSceneCfg),
        
    };



}


