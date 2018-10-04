/* ==============================================================================
 * BundleTest
 * @author jr.zeng
 * 2017/11/22 16:11:55
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using UnityEngine;
using mg.org;
using mg.org.KUI;

using Object = UnityEngine.Object;

public class BundleTest : CCModule
{
    public BundleTest()
    {

    }

    override protected void __Setup(params object[] params_)
    {
        
    }

    override protected void __Clear()
    {
        
    }


    override protected void SetupEvent()
    {

        CCApp.keyboard.Attach(KEY_EVENT.PRESS, onKeyPressed, this);
    }

    override protected void ClearEvent()
    {
        CCApp.keyboard.Detach(KEY_EVENT.PRESS, onKeyPressed);

    }


    //-------∽-★-∽------∽-★-∽--------∽-★-∽数据管理∽-★-∽--------∽-★-∽------∽-★-∽--------//

    private void onKeyPressed(object evt_)
    {
        KeyCode key = (KeyCode)evt_;
        switch (key)
        {
            case KeyCode.Alpha1:

                Test_LoadBundle();

                break;
            case KeyCode.Alpha2:
                

                break;
        }

    }


    //-------∽-★-∽------∽-★-∽--------∽-★-∽数据操作∽-★-∽--------∽-★-∽------∽-★-∽--------//

    private void Test_LoadBundle()
    {
        string[] dependPaths = new string[] {
            "StreamingAssets/depend.j",
        };

        string[] bundlePaths = new string[] {
            "StreamingAssets/canvas_bag.j",
            "StreamingAssets/canvas_test3.j",
        };

        List<AssetBundle> bundleList = new List<AssetBundle>();

        foreach (string path in dependPaths)
        {
            AssetBundle b = AssetBundle.LoadFromFile(Application.dataPath + "/" + path);
            bundleList.Add(b);
        }

        AssetBundle bundle1 = AssetBundle.LoadFromFile(Application.dataPath + "/" + bundlePaths[1]);
        bundleList.Add(bundle1);

        GameObject prefab = bundle1.LoadAsset("canvas_test3") as GameObject;

        GameObject go =  GameObjUtil.Instantiate(prefab);
        DisplayUtil.AddChild(KUIApp.UILayer, go);

        //需要全部卸载掉才能重新加载， 不然会报错
        foreach (var b in bundleList)   
        {
            b.Unload(false);
        }
        Resources.UnloadUnusedAssets();
    }




}