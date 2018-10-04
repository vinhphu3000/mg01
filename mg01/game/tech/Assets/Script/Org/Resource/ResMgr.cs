/* ==============================================================================
 * CCResMgrNew
 * @author jr.zeng
 * 2017/5/23 23:17:09
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using UnityEngine;
using Object = UnityEngine.Object;

namespace mg.org
{

    public class ResMgr : CCModule
    {
        public ResMgr()
        {
            RegResource();

        }

        //资源注册
        protected virtual void RegResource()
        {
            ResConfig.AddResCfg(CC_RES_ID.RES_REG);
        }


        override protected void __Setup(params object[] params_)
        {
            base.__Setup();

            AssetCache.me.Setup();
            GameObjCache.me.Setup();
            SprAtlasCache.me.Setup();
            SoundCache.me.Setup();
            LevelMgr.me.Setup();
        }

        override protected void __Clear()
        {
            AssetCache.me.Clear();
            GameObjCache.me.Clear();
            SprAtlasCache.me.Clear();
            SoundCache.me.Clear();
            LevelMgr.me.Clear();
        }

        override protected void SetupEvent()
        {

        }

        override protected void ClearEvent()
        {

        }

        //-------∽-★-∽------∽-★-∽--------∽-★-∽数据管理∽-★-∽--------∽-★-∽------∽-★-∽--------//



      
        //-------∽-★-∽------∽-★-∽--------∽-★-∽加载相关∽-★-∽--------∽-★-∽------∽-★-∽--------//

        public Object LoadSync(string resId_, string resName_, object refer)
        {
            string url = ResConfig.GetResUrl(resId_, resName_, ""); //去除后缀名
            Object data = AssetCache.me.LoadSync(url, refer);
            return data;
        }

        public void LoadAsync(string resId_, string resName_, CALLBACK_1 onComplete_, object refer)
        {
            string url = ResConfig.GetResUrl(resId_, resName_, ""); //去除后缀名
            AssetCache.me.LoadAsync(url, onComplete_, refer);
        }

        public void LoadAsync_Prefab(string resId_, string resName_, CALLBACK_GO onComplete_, object refer)
        {
            string url = ResConfig.GetResUrl(resId_, resName_, ""); //去除后缀名
            GameObjCache.me.LoadAsync(url, onComplete_, refer);
        }

        //-------∽-★-∽------∽-★-∽--------∽-★-∽创建相关∽-★-∽--------∽-★-∽------∽-★-∽--------//

        public GameObject LoadGameObj(string resId_, string resName_)
        {
            string url = ResConfig.GetResUrl(resId_, resName_, ""); //去除后缀名
            return GameObjCache.me.LoadSync(url);
        }
        


    }


}
