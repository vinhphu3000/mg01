/* ==============================================================================
 * LUATest
 * @author jr.zeng
 * 2017/9/14 19:18:28
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using UnityEngine;
using Object = UnityEngine.Object;

using mg.org;
using mg.org.KUI;

using SLua;

public class LUATest : CCModule
{
    SluaHost m_luaHost = SluaHost.Inst;

    Notifer m_notifier = new Notifer();

    public LUATest()
    {

    }

    override protected void __Setup(params object[] params_)
    {

        m_luaHost.StartSvr();

        SluaHost.errorDelegateLua += OnLuaError;

        CCApp.appBhv.OnUpdate += OnSvrUpdate;
        CCApp.appBhv.OnLateUpdate += OnSvrLateUpdate;
    }

    override protected void __Clear()
    {
        SluaHost.errorDelegateLua -= OnLuaError;

        CCApp.appBhv.OnUpdate -= OnSvrUpdate;
        CCApp.appBhv.OnLateUpdate -= OnSvrLateUpdate;

        m_luaHost.StopSvr();
        LuaEvtCenter.Clear();

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

    void OnSvrUpdate(float dt)
    {
        m_luaHost.OnSvrUpdate(dt);
    }

    void OnSvrLateUpdate(float dt)
    {
        m_luaHost.OnSvrLastUpdate(dt);
    }


    void onKeyPressed(object evt_)
    {
        KeyCode key = (KeyCode)evt_;
        switch (key)
        {
            case KeyCode.Alpha0:
                
                KUIApp.PopMgr.ShowOrClose(POP_ID.TEST_KUI_3);

                break;
            case KeyCode.Alpha9:

                m_luaHost.SvrNotify(2, 2, 3);

                //m_luaHost.StopSvr();
                //测试错误处理
                //GameObject go = null;
                //go.transform.position = Vector3.zero;

                break;
            case KeyCode.Alpha8:

                //m_notifier.Attach("aaa", onBBBFun);
                //m_notifier.Attach("aaa", onAAAFun);
                //Attach("aaa", onAAAFun, this);
               
                for (int i=0;i<10;++i)   //测试autoRelease
                {
                    var view = new TestKUIPop3();
                }

                break;
        }
    }

    void onAAAFun(object evt_)
    {
        m_notifier.Detach("aaa", onBBBFun);

        Log.Debug("aaa", this);

    }
    void onBBBFun(object evt_)
    {
        //Detach("bbb", onAAAFun);
        m_notifier.Detach("aaa", onAAAFun);

        Log.Debug("bbb",this);

    }



    void OnLuaError(string content_)
    {

#if !RELEASE
        KUIApp.PopMgr.Show(POP_ID.ERROR_REPORT, content_);
#endif
    }


    void UnityLogCallback(string condition, string stackTrace, LogType type)
    {
        if (type == LogType.Error ||
            type == LogType.Exception ||
            type == LogType.Assert)
        {

#if !RELEASE
            string s = string.Format("|{0}| {1}\n{2}", type, condition, stackTrace);
            KUIApp.PopMgr.Show(POP_ID.ERROR_REPORT, s);
#endif

        }
    }


}