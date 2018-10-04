/* ==============================================================================
 * ConfigBattle
 * @author jr.zeng
 * 2016/11/25 17:07:19
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class ConfigBattle
{

    static private Dictionary<int, BttCfgInfo> m_id2cfg = new Dictionary<int, BttCfgInfo>();

    public ConfigBattle()
    {

    }

    static public void InitBTCfg(object[] datas_)
    {
        for (int i = 0; i < datas_.Length; ++i)
        {
            AddBttCfg(datas_[i] as BttCfgInfo);
        }

    }


    //-------∽-★-∽------∽-★-∽--------∽-★-∽数据管理∽-★-∽--------∽-★-∽------∽-★-∽--------//


    static private void AddBttCfg(BttCfgInfo data_)
    {
        int id = data_.id;
        if (m_id2cfg.ContainsKey(id))
            return;

        m_id2cfg[id] = data_;
    }

    static public BttCfgInfo GetBttCfg(int id_, bool alarm_ = true)
    {
        if (!m_id2cfg.ContainsKey(id_))
        {
            if (alarm_)
                mg.org.Log.Warn("miss BttCfg:" + id_, typeof(ConfigBattle));
            return null;
        }
        return m_id2cfg[id_];
    }


}



/// <summary>
/// 战斗配置信息
/// </summary>
public class BttCfgInfo
{

    public int id;
    //战斗类型
    //public BattleType battleType;

    //场景id
    public int scene_id;

    //关卡名称
    public string name;
    //关卡说明
    public string desc;

    //下一战斗
    public int next_id;

    public BttCfgInfo()
    {

    }


}
