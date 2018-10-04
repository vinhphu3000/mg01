/* ==============================================================================
 * SluaHost
 * @author jr.zeng
 * 2018/5/2 10:40:06
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;


using UnityEngine;
using Object = UnityEngine.Object;


using SLua;


namespace mg.org
{
    
    public class SluaHost
    {

        static SluaHost m_instance;

        public static SluaHost Inst
        {
            get
            {
                if (m_instance == null)
                    m_instance = new SluaHost();
                return m_instance;
            }
        }

        

        //jit模式
        JITBUILDTYPE m_jitType = JITBUILDTYPE.none;
        //是否默认打印堆栈
        bool m_printTrack = false;


#if UNITY_EDITOR
        static string SCRIPT_PATH = Path.Combine(Application.dataPath, "Resources/LuaScript/");
#endif

        //是否已启动
        bool m_launched = false;
        //启动进度
        float m_progress = 0;

        LuaSvr m_luaSvr = null;
        LuaState m_luaState = null;
        LuaTable m_luaTable = null;
        

        [CustomLuaClass]
        public delegate void LuaDelegate_Update(float dt);
        //[CustomLuaClass]  //自己导出LuaDelegation
        public delegate void LuaDelegate_Notify( params object[] args);
        [CustomLuaClass]
        public delegate void LuaDelegate_NotifyError(string str);
        
        LuaDelegate_Update m_luaUpdate = null;
        LuaDelegate_Update m_luaLateUpdate = null;
        LuaDelegate_Notify m_luaNotify = null;
        LuaDelegate_NotifyError m_luaNotifyError = null;   //通知lua虚拟机cs报错了

        //lua报错回调
        public delegate void OutputDelegate(string msg);
        static public OutputDelegate errorDelegateLua = null;       

        public SluaHost()
        {

            SLuaSetting.Instance.jitType = m_jitType;
            SLuaSetting.Instance.PrintTrace = m_printTrack;

            if (m_instance == null)
                m_instance = this;
        }

        public float Progress
        {
            get { return m_progress; }
        }

        public bool Launched
        {
            get { return m_launched; }
        }


        //-------∽-★-∽------∽-★-∽--------∽-★-∽数据管理∽-★-∽--------∽-★-∽------∽-★-∽--------//





        //-------∽-★-∽------∽-★-∽--------∽-★-∽数据操作∽-★-∽--------∽-★-∽------∽-★-∽--------//


        public void StartSvr()
        {
            if (m_luaSvr != null)
                //已经启动
                return;

            m_launched = false;
            m_progress = 0;

            m_luaSvr = new LuaSvr();
            m_luaState = LuaSvr.mainState;
            m_luaState.loaderDelegate = LoadLuaScript;

            m_luaSvr.init(OnProgress, OnLaunched);

        }


        //lua文件加载的代理
        byte[] LoadLuaScript(string fn)
        {
            fn = fn.Replace(".", "/");

            byte[] bytes = null;

#if UNITY_EDITOR
            if (m_jitType == JITBUILDTYPE.none)
            {
                //asset = (TextAsset)Resources.Load(fn);

                fn = SCRIPT_PATH + fn + ".lua";

                if (!File.Exists(fn))
                {
                    SLua.Logger.LogError("LuaLoader:Failed to load:" + fn);
                    return null;
                }

                bytes = File.ReadAllBytes(fn);
            }
            else
            {
                //使用jit
                TextAsset asset = null;
                string jit_fn = null;
                if (SLuaSetting.Instance.jitType == JITBUILDTYPE.X86)
                {
                    jit_fn = "Assets/Slua/jit/jitx86/" + fn + ".bytes";
                }
                else if (SLuaSetting.Instance.jitType == JITBUILDTYPE.X64)
                {
                    jit_fn = "Assets/Slua/jit/jitx64/" + fn + ".bytes";
                }
                else if (SLuaSetting.Instance.jitType == JITBUILDTYPE.GC64)
                {
                    jit_fn = "Assets/Slua/jit/jitgc64/" + fn + ".bytes";
                }

                asset = UnityEditor.AssetDatabase.LoadAssetAtPath<TextAsset>(jit_fn);
                if (asset == null)
                {
                    SLua.Logger.LogError("LuaLoader:Failed to load:" + fn);
                    return null;
                }

                bytes = asset.bytes;
            }

#else
            //TODO
            //asset = (TextAsset)Resources.Load(fn);
#endif

            return bytes;
        }

        void OnProgress(int p)
        {
            m_progress = p * 0.01f;
        }


        void OnLaunched()
        {
            m_launched = true;
            SLua.Logger.Log("OnSvrLaunched");

            DoBind(m_luaState.L);

            LuaEvtCenter.Enable = true;

            //入口文件可以不叫main，但要有一个global的main方法， 其返回一个含有luaSvr_update等方法的一个表
            m_luaTable = m_luaSvr.start("main") as LuaTable;
            
            m_luaUpdate = ( (LuaFunction)m_luaTable["luaSvr_update"] ).cast<LuaDelegate_Update>();
            m_luaLateUpdate = ( (LuaFunction)m_luaTable["luaSvr_lateUpdate"] ).cast<LuaDelegate_Update>();
            m_luaNotify = ((LuaFunction)m_luaTable["luaSvr_notify"]).cast<LuaDelegate_Notify>();
            //m_luaNotify = (LuaFunction)m_luaTable["luaSvr_notify"];
            m_luaNotifyError = ((LuaFunction)m_luaTable["luaSvr_notifyCsError"]).cast<LuaDelegate_NotifyError>();

            Application.logMessageReceived += UnityLogCallback; //监听unitys的log


            LuaFunction luaLaunch = (LuaFunction)m_luaTable["luaSvr_launch"];
            if (luaLaunch != null)
                luaLaunch.call();
            
        }


        void UnityLogCallback(string condition, string stackTrace, LogType type)
        {
            if (type == LogType.Error ||
                type == LogType.Exception ||
                type == LogType.Assert)
            {

                //只处理报错
                if (m_luaNotifyError != null)
                {
                    string s = string.Format("|{0}| {1}\n{2}", type, condition, stackTrace);
                    m_luaNotifyError(s); //通知lua
                }

                
            }
        }



        public void StopSvr()
        {
            if (m_luaSvr == null)
                return;

            if(!m_launched)
            {
                SLua.Logger.LogError("lua虚拟机还没启动完成");
                return;
            }

            Application.logMessageReceived -= UnityLogCallback; //监听unitys的log
            
            m_luaTable = null;

            m_luaState.loaderDelegate = null;
            m_luaState.Dispose();

            m_luaUpdate = null;
            m_luaLateUpdate = null;
            m_luaNotify = null;
            m_luaNotifyError = null;

            m_luaState = null;
            m_luaSvr = null;
            m_launched = false;


            LuaEvtCenter.Enable = false;

            //errorDelegateLua = null; //因为是static，由外面自己维护
        }
        
        public void OnSvrUpdate(float dt)
        {
            if (!m_launched)
                return;
            m_luaUpdate(dt);
            m_luaState.tick();
        }


        public void OnSvrLastUpdate(float dt)
        {
            if (!m_launched)
                return;
            m_luaLateUpdate(dt);
        }
        
        /// <summary>
        /// cs向lua发送通知
        /// </summary>
        /// <param name="evtType_"></param>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <param name="v3"></param>
        public void SvrNotify( params object[] args)
        {
            if (m_luaNotify != null)
            {
                m_luaNotify(args);
                //m_luaNotify.call( args);
            }
        }



        //-------∽-★-∽------∽-★-∽--------∽-★-∽lua绑定∽-★-∽--------∽-★-∽------∽-★-∽--------//


        void DoBind(IntPtr L)
        {

            //LuaDLL.lua_pushcfunction(L, lua_log_warn);
            //LuaDLL.lua_setglobal(L, "log_warn");
            LuaDLL.lua_pushcfunction(L, lua_log_error);
            LuaDLL.lua_setglobal(L, "log_error");
            LuaDLL.lua_pushcfunction(L, lua_log_print);
            LuaDLL.lua_setglobal(L, "log_print");
            //LuaDLL.lua_pushcfunction(L, lua_log_trace);
            //LuaDLL.lua_setglobal(L, "log_trace");

            //LuaDLL.lua_pushcfunction(L, lua_gc);
            //LuaDLL.lua_setglobal(L, "gc");
        }


        //纯粹为了打印时间
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        internal static int lua_log_print(IntPtr L)
        {
            int n = LuaDLL.lua_gettop(L);
            string s = "";

            LuaDLL.lua_getglobal(L, "tostring");

            for (int i = 1; i <= n; i++)
            {
                if (i > 1)
                {
                    s += "    ";
                }

                LuaDLL.lua_pushvalue(L, -1);
                LuaDLL.lua_pushvalue(L, i);
                LuaDLL.lua_call(L, 1, 1);
                s += LuaDLL.lua_tostring(L, -1);
                LuaDLL.lua_pop(L, 1);
            }

            LuaDLL.lua_settop(L, n);

            s = DateTime.Now.ToString("[yyyy-MM-dd HH:mm:ss.fff]") + s;     

            SLua.Logger.Log(s);

            return 0;
        }

        /// <summary>
        /// 打印报错
        /// (safe_call捕获的报错会走这里)
        /// </summary>
        /// <param name="L"></param>
        /// <returns></returns>
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        internal static int lua_log_error(IntPtr L)
        {
            int n = LuaDLL.lua_gettop(L);
            string s = "";

            LuaDLL.lua_getglobal(L, "tostring");

            for (int i = 1; i <= n; i++)
            {
                if (i > 1)
                {
                    s += "    ";
                }

                LuaDLL.lua_pushvalue(L, -1);
                LuaDLL.lua_pushvalue(L, i);
                LuaDLL.lua_call(L, 1, 1);
                s += LuaDLL.lua_tostring(L, -1);
                LuaDLL.lua_pop(L, 1);
            }

            LuaDLL.lua_settop(L, n);

            //SLua.Logger.LogError(s);
            //普通log
            SLua.Logger.Log(
#if UNITY_EDITOR
                "<color=red>|Error| </color>" +
#else
                "|Error|" +
#endif
                s);
            
            if (errorDelegateLua != null)
            {
                errorDelegateLua(s);
            }

#if !RELEASE
            //弹窗
            //KUI.KUIApp.PopMgr.Show(POP_ID.ERROR_REPORT, s);
#endif


            return 0;
        }



    }


}


//重写LuaDelegation
namespace SLua
{
    public partial class LuaDelegation : LuaObject
    {

        [UnityEngine.Scripting.Preserve]
        static internal void Lua_mg_org_SluaHost_LuaDelegate_Notify(LuaFunction ld, object[] a1)
        {
            IntPtr l = ld.L;
            int error = pushTry(l);

            //pushValue(l, a1);     //直接生成的代码，没有了不定参，所以要改
            for (int n = 0; a1 != null && n < a1.Length; n++)
            {
                pushValue(l, a1[n]);
            }

            ld.pcall(1, error);
            LuaDLL.lua_settop(l, error - 1);
        }
    }
}


//SLua修改点

//1. LuaCodeGen.RegFunction 加入以下代码
//jr.zeng@20170922 接入Manual 
//Type ot;
//if (overloadedClass.TryGetValue(t, out ot))
//{
//    MethodInfo mi = ot.GetMethod("reg", BindingFlags.Static | BindingFlags.Public);
//    if (mi != null)
//        Write(file, ot.Name + ".reg(l);");
//}

//2. LuaCodeGen.DontExport 加入以下代码
//jr.zeng@20180505  可以在ICustomExportPost里CheckDontExport
//object[] aCustomImport = new object[] { mi };
//foreach (object result in LuaCodeGen.InvokeEditorMethod<ICustomExportPost>("OnCheckDontExport", ref aCustomImport))
//{
//    if ((bool)result)
//        return true;
//}


//3. 做以下修改
//1) SLuaState:  tick函数改为public
//2) SLuaState:  注释 lgo.onUpdate = this.tick;
//3) setting: 修改绑定文件输出路径 Assets/Slua/LuaObject/ -> Assets/Script/Slua/LuaBind/;