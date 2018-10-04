/* ==============================================================================
 * StageConfig
 * @author jr.zeng
 * 2016/11/25 16:38:17
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class ConfigStage
{

    static private Dictionary<int, StgCfgInfo> m_id2cfg = new Dictionary<int, StgCfgInfo>();


    public ConfigStage()
    {

    }

    static public void InitStgCfg(object[] datas_)
    {
        for (int i = 0; i < datas_.Length; ++i)
        {
            AddStgCfg(datas_[i] as StgCfgInfo);
        }

    }


    //-------∽-★-∽------∽-★-∽--------∽-★-∽数据管理∽-★-∽--------∽-★-∽------∽-★-∽--------//


    static private void AddStgCfg(StgCfgInfo data_)
    {
        int id = data_.id;
        if (m_id2cfg.ContainsKey(id))
            return;

        m_id2cfg[id] = data_;
    }

    static public StgCfgInfo GetStgCfg(int id_, bool alarm_ = true)
    {
        if (!m_id2cfg.ContainsKey(id_))
        {
            if (alarm_)
                mg.org.Log.Warn("miss cfg:" + id_, typeof(ConfigStage));
            return null;
        }
        return m_id2cfg[id_];
    }


    //-------∽-★-∽------∽-★-∽--------∽-★-∽数据操作∽-★-∽--------∽-★-∽------∽-★-∽--------//




}



/// <summary>
/// 关卡配置信息
/// </summary>
public class StgCfgInfo
{

    public int id;
    //关卡名称
    public string name;
    //关卡说明
    public string desc;
    //战斗id
    public int battle_id;

    public StgCfgInfo()
    {

    }


}

