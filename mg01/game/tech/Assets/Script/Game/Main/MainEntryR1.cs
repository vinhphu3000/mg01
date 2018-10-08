/* ==============================================================================
 * MainEntryR1
 * @author jr.zeng
 * 2017/9/14 19:05:37
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using UnityEngine;
using Object = UnityEngine.Object;

using mg.org;
using mg.org.KUI;

public class MainEntryR1 : MainEntry
{
    public MainEntryR1()
    {

    }


    override protected void __Setup(params object[] params_)
    {
        base.__Setup(params_);

        DataConfig.Init(new DataCfgParser_Python(), ConfigConst.CONFIG_REQS);     //C#配置先放放
        
        //窗口注册
        CC_POP_ID.AddPrefebPath(POP_ID.pop2prefeb);   

        KUIApp.Setup();

        LoadResources();
        //LoadResourcesBack();
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


    public void LoadResources()
    {
        //LoadReq[] objs = new LoadReq[0];
        //objs = ArrayUtil.Concat<LoadReq>(objs, DataConfig.GetLoadObjs());//配置表

        List<LoadReq> reqs = new List<LoadReq>();
        reqs.Add(new LoadReqLevel("SceneEmpty", false));
        reqs.Add(new LoadReqDelay(1));

        LoadReqQueue reqQue = new LoadReqQueue(reqs.ToArray() );

        LoadMgr.DoMainLoad(reqQue, LoadResourcesBack, POP_ID.LOADING_1);

        //LoadResourcesBack();

    }

    public void LoadResourcesBack()
    {

        Log.Info("LoadResourcesBack", this);

        EnterGame();
    }


    //进入游戏
    override public void EnterGame()
    {
        //base.EnterGame();

        InstUtil.Get<LUATest>().Setup();
        
    }



    override public void QuitGame()
    {
        base.QuitGame();

        InstUtil.Get<LUATest>().Clear();
    }
}