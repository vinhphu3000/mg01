/* ==============================================================================
 * MainEntryR2
 * @author jr.zeng
 * 2017/12/7 11:17:04
 * ==============================================================================*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using UnityEngine;
using Object = UnityEngine.Object;

using mg.org;
using mg.org.KUI;
using mg.org.bundle;

public class MainEntryR2 : MainEntry
{


    public MainEntryR2()
    {

    }

    override protected void __Setup(params object[] params_)
    {
        base.__Setup(params_);


        //窗口注册
        CC_POP_ID.AddPrefebPath(POP_ID.pop2prefeb);

        KUIApp.Setup();

        LoadResources();
    }

    override protected void __Clear()
    {

        KUIApp.Clear();


        base.__Clear();
    }

    override protected void SetupEvent()
    {

        base.SetupEvent();

        Application.logMessageReceived += UnityLogCallback; //监听unitys的log
    }

    override protected void ClearEvent()
    {

        base.ClearEvent();

        Application.logMessageReceived -= UnityLogCallback;
    }

    //-------∽-★-∽------∽-★-∽--------∽-★-∽数据管理∽-★-∽--------∽-★-∽------∽-★-∽--------//


    void UnityLogCallback(string condition, string stackTrace, LogType type)
    {
        if (type == LogType.Error ||
            type == LogType.Exception ||
            type == LogType.Assert)
        {

#if !RELEASE
            string s = string.Format("|{0}| {1}\n{2}", type, condition, stackTrace);
            //弹窗
            KUIApp.PopMgr.Show(POP_ID.ERROR_REPORT, s);
#endif

        }
    }

    //-------∽-★-∽------∽-★-∽--------∽-★-∽数据操作∽-★-∽--------∽-★-∽------∽-★-∽--------//

        
    void LoadResources()
    {
        List<LoadReq> reqs = new List<LoadReq>();

        reqs.Add( new LoadReqDelay(1) );
        //reqs.Add( new LoadReqProg(new ProgData(), OnLoadReqProg));
        reqs.Add(new LoadReqLevel("SceneEmpty", false));

        LoadReqQueue reqQue = new LoadReqQueue( reqs.ToArray() );

        LoadMgr.DoMainLoad(reqQue, LoadResourcesBack, null);
        //LoadMgr.DoMainLoad(reqQue, LoadResourcesBack, POP_ID.LOADING_1);
    }


    void OnLoadReqProg(IProgress prog_)
    {
        ProgData prog = prog_ as ProgData;
        CCApp.StartCoroutine(ProcessInit(prog));
    }

    public static IEnumerator ProcessInit(ProgData prog_)
    {
        yield return null;

        string res_cfg_path;
#if UNITY_ANDROID
		res_cfg_path = AssetCacheBdl.RES_CFG_PATH;
#else
        res_cfg_path = "file:///" + AssetCacheBdl.RES_CFG_PATH;
#endif

        WWW www = new WWW(res_cfg_path);
        while(!www.isDone)
        {
            yield return null;
        }

        if(!string.IsNullOrEmpty( www.error ))
        {
            //加载出错
            yield break;
        }

        prog_.progress = 0.1f;

        AbsResConfig.ReadFromFile(www);
        www.Dispose();

        yield return null;

        prog_.progress = 1f;
        prog_.isDone = true;
    }

    public void LoadResourcesBack()
    {

        Log.Info("LoadResourcesBack", this);

        EnterGame();
    }


    override public void QuitGame()
    {


        base.QuitGame();

    }

    //进入游戏
    override public void EnterGame()
    {


        InstUtil.Get<KUITest>().Setup();
        //InstUtil.Get<BundleTest>().Setup();

    }

}