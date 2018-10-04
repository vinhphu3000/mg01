/* ==============================================================================
 * ConfigScene
 * @author jr.zeng
 * 2016/11/28 17:19:19
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class ConfigScene
{

    static private Dictionary<int, SceneCfgInfo> m_id2cfg = new Dictionary<int, SceneCfgInfo>();

    public ConfigScene()
    {

    }



    static public void InitSceneCfg(object[] datas_)
    {
        for (int i = 0; i < datas_.Length; ++i)
        {
            AddSceneCfg(datas_[i] as SceneCfgInfo);
        }

    }


    //-------∽-★-∽------∽-★-∽--------∽-★-∽数据管理∽-★-∽--------∽-★-∽------∽-★-∽--------//


    static private void AddSceneCfg(SceneCfgInfo data_)
    {
        int id = data_.id;
        if (m_id2cfg.ContainsKey(id))
            return;

        m_id2cfg[id] = data_;
    }

    static public SceneCfgInfo GetSceneCfg(int id_, bool alarm_ = true)
    {
        if (!m_id2cfg.ContainsKey(id_))
        {
            if (alarm_)
                mg.org.Log.Warn("miss SceneCfg:" + id_, typeof(ConfigScene));
            return null;
        }
        return m_id2cfg[id_];
    }


}


/// <summary>
/// 场景配置信息
/// </summary>
public class SceneCfgInfo
{

    public int id;
    //场景名称
    public string sceneName;
    //场景说明
    public string desc;

    //场景类型
    //public SceneType sceneType;
    //场景视图名称
    public string sceneView;
    //场景资源(暂时指的是预制)
    public string sceneRes;


    public SceneCfgInfo()
    {

    }


}