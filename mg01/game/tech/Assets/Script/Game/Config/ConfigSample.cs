/* ==============================================================================
 * 配置范例
 * @author jr.zeng
 * 2016/9/7 17:55:29
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class ConfigSample
{

    static private Dictionary<int, TestCfgData> m_id2cfg = new Dictionary<int, TestCfgData>();

    public ConfigSample()
    {

    }


    static public void InitBase(object[] datas_)
    {
        for (int i = 0; i < datas_.Length; ++i)
        {
            AddCfg(datas_[i] as TestCfgData);
        }

    }


    //-------∽-★-∽------∽-★-∽--------∽-★-∽数据管理∽-★-∽--------∽-★-∽------∽-★-∽--------//


    static private void AddCfg(TestCfgData data_)
    {
        int id = data_.id;
        if (m_id2cfg.ContainsKey(id) )
            return;

        m_id2cfg[id] = data_;
    }

    static public TestCfgData GetCfg(int id_, bool alarm_=true)
    {
        if (!m_id2cfg.ContainsKey(id_))
        {
            if (alarm_)
                mg.org.Log.Warn("miss cfg:" + id_, typeof(ConfigSample));
            return null;
        }
        return m_id2cfg[id_];
    }


    //-------∽-★-∽------∽-★-∽--------∽-★-∽数据操作∽-★-∽--------∽-★-∽------∽-★-∽--------//



}

//-------∽-★-∽------∽-★-∽--------∽-★-∽数据结构∽-★-∽--------∽-★-∽------∽-★-∽--------//

// 测试配置信息
public class TestCfgData 
{
    public int id;
    public string name;
    public int scene_id;
    public float transfer_id;
    public string triger_range;
    public string skin;
    public string desc;

    public string uuid;

    public TestCfgData()
    {

    }

    public TestCfgData(int id_, int scene_id_, float transfer_id_)
    {
        id = id_;
        scene_id = scene_id_;
        transfer_id = transfer_id_;
    }

    public void Analyse()
    {
        uuid = id + "_" + name;
    }
}