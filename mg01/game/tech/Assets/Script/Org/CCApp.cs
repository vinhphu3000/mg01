/* ==============================================================================
 * 游戏代理
 * @author jr.zeng
 * 2016/6/8 10:40:49
 * ==============================================================================*/


using System;
using System.Collections;
using System.Threading;

using UnityEngine;


namespace mg.org
{
    public class CCApp
    {

        static string __typeName = typeof(CCApp).Name;

        static bool m_isOpen = false;

        static bool m_appPause = false; //应用暂停
        static bool m_appFocus = false; //获得焦点

        //主线程id
        static int m_mainThreadId; 

        //根对象
        static GameObject m_gRoot;
        //App行为
        static CCAppBhv m_appBhv;


        //观察者
        static Subject m_subject = new Subject();

        static CALLBACK_Float m_onUpdate = null;
        static CALLBACK_Float m_onLateUpdate = null;
        static CALLBACK_Float m_onGui = null;

        //键盘
        static Keyboard m_keyboard = new Keyboard();
        //声音管理
        static SoundMgr m_soundMgr = null;
        //自动释放池
        static AutoRelease m_autoRelease = new AutoRelease();

        //资源管理
        static ResMgr m_resMgr = null;

        static ClassPools m_classPools = null;


        static CCApp()
        {
            InitMainThread();

            Setting();

        }

        //参数设置
        static void Setting()
        {
            QualitySettings.vSyncCount = 0; //垂直同步

            FrameRate = CCDefine.FPS_DEFAULT;
        }

        static public void Setup(GameObject root_)
        {
            if (m_isOpen) return;
            m_isOpen = true;

            m_gRoot = root_;

            Setting();
            __Setup();
            SetupEvent();
        }

        static public void Clear()
        {
            if (!m_isOpen) return;
            m_isOpen = false;

            ClearEvent();
            __Clear();
        }


        static void __Setup()
        {

            m_appBhv = ComponentUtil.EnsureComponent<CCAppBhv>(m_gRoot);

            InitTrash();
            

            m_resMgr = m_resMgr ?? new ResMgr();
            m_resMgr.Setup();

            m_soundMgr = m_soundMgr ?? new SoundMgr(m_gRoot);
            m_soundMgr.Setup();

            m_keyboard.Setup();

            m_classPools = ClassPools.me;

            //
            TimerMgr.inst.Setup();
            //
            ActionMgr.inst.Setup();
            //
            UserPrefs.Setup();

            if (CCDefine.DEBUG)
            {
                //显示帧频
                ComponentUtil.EnsureComponent<FpsTicker>(m_gRoot);
            }


            LogFile.Run();
        }

        static void __Clear()
        {

            m_keyboard.Clear();

            m_subject.DetachAll();

            m_autoRelease.Excute();
            
            m_resMgr.Clear();
            m_soundMgr.Clear();

            //对象池清除
            m_classPools.Clear();
            m_classPools = null;

            //单例清除
            TimerMgr.inst.Clear();
            ActionMgr.inst.Clear();
            
            UserPrefs.Clear();
            Refer.ClearNotify();

            //清空垃圾桶
            ClearTrash();

            GameObject.Destroy(m_appBhv);
            m_appBhv = null;

            m_gRoot = null;

            m_onLateUpdate = null;
            m_onUpdate = null;
            m_onGui = null;

        }


        static void SetupEvent()
        {

        }

        static void ClearEvent()
        {

        }


        //-------∽-★-∽------∽-★-∽--------∽-★-∽getter/setter∽-★-∽--------∽-★-∽------∽-★-∽--------//

        static public GameObject root
        {
            get { return m_gRoot; }
        }

        static public CCAppBhv appBhv
        {
            get { return m_appBhv; }
        }
        
        /// <summary>
        /// 键盘
        /// </summary>
        static public Keyboard keyboard
        {
            get { return m_keyboard; }
        }

        /// <summary>
        /// 派发器
        /// </summary>
        static public Subject subject
        {
            get { return m_subject; }
        }

        /// <summary>
        /// AutoRelease
        /// </summary>
        static public AutoRelease autoRelease
        {
            get { return m_autoRelease; }
        }


        /// <summary>
        /// 资源管理
        /// </summary>
        static public ResMgr resMgr
        {
            set
            {
                if (m_resMgr == null)
                    m_resMgr = value;
            }
            get { return m_resMgr; }
        }

        /// <summary>
        /// 声音管理
        /// </summary>
        static public SoundMgr soundMgr
        {
            set
            {
                if (m_soundMgr == null)
                    m_soundMgr = value;
            }
            get { return m_soundMgr; }
        }

        //-------∽-★-∽------∽-★-∽--------∽-★-∽数据管理∽-★-∽--------∽-★-∽------∽-★-∽--------//

        static internal void Step(float dt_)
        {
            if (m_onUpdate != null)
                m_onUpdate(dt_);
            //m_autoPool.Clear();
        }

        static internal void LateStep(float dt_)
        {
            if (m_onLateUpdate != null)
                m_onLateUpdate(dt_);

            m_autoRelease.Excute(); //调式会死机？
            //m_classPools.Step(dt_);
        }

        static internal void OnGUI(float dt_)
        {
            if (m_onGui != null)
                m_onGui(dt_);
            
        }


        //-------∽-★-∽------∽-★-∽--------∽-★-∽数据操作∽-★-∽--------∽-★-∽------∽-★-∽--------//

        /// <summary>
        /// 调用携程
        /// </summary>
        /// <param name="routine_"></param>
        /// <returns></returns>
        static public Coroutine StartCoroutine(IEnumerator routine_)
        {
            return m_appBhv.StartCoroutine(routine_);
        }

        static public void StopCoroutine(IEnumerator routine_)
        {
            m_appBhv.StopCoroutine(routine_);
        }
        //-------∽-★-∽------∽-★-∽--------∽-★-∽线程相关∽-★-∽--------∽-★-∽------∽-★-∽--------//

        static void InitMainThread()
        {
            m_mainThreadId = Thread.CurrentThread.ManagedThreadId;
        }

        /// <summary>
        /// 当前是否主线程
        /// </summary>
        static public bool IsMainThread()
        {
            return Thread.CurrentThread.ManagedThreadId == m_mainThreadId;
        }

        //-------∽-★-∽------∽-★-∽--------∽-★-∽垃圾桶∽-★-∽--------∽-★-∽------∽-★-∽--------//

        //垃圾桶
        //static CCTrash m_trash;
        static Trash m_trash;


        /// <summary>
        /// 垃圾桶
        /// </summary>
        //static public CCTrash Trash
        static public Trash Trash
        {
            get { return m_trash; }
        }

        static void InitTrash()
        {
            if (m_trash == null)
            {
                //m_trash = new CCTrash(CCTrashType.far_from_camera);
                m_trash = new Trash("Trash");
            }

            //m_trash.Setup(null);
        }

        static void ClearTrash()
        {
            m_trash.Clear();
            //m_trash.Dispose();
        }

        //-------∽-★-∽------∽-★-∽--------∽-★-∽进程相关∽-★-∽--------∽-★-∽------∽-★-∽--------//

        /// <summary>
        /// 退出应用
        /// </summary>
        static public void QuitApp()
        {
            if (System.Diagnostics.Process.GetCurrentProcess() != null)
            {
                //System.Diagnostics.Process.GetCurrentProcess().Kill()
                System.Diagnostics.Process.GetCurrentProcess().WaitForExit();
            }
            else
            {
                Application.Quit();
            }
        }

        //-------∽-★-∽------∽-★-∽--------∽-★-∽帧频相关∽-★-∽--------∽-★-∽------∽-★-∽--------//

        //帧频
        static int m_frameRate = 0;

        static public int FrameRate
        {
            get { return m_frameRate; }

            set
            {
                if (m_frameRate == value)
                    return;
                m_frameRate = value;

                Application.targetFrameRate = m_frameRate;
            }
        }

        //-------∽-★-∽------∽-★-∽--------∽-★-∽事件相关∽-★-∽--------∽-★-∽------∽-★-∽--------//

        static public void Attach(string type_, CALLBACK_1 callback_, object refer_)
        {
            m_subject.Attach(type_, callback_, refer_);
        }

        static public void Detach(string type_, CALLBACK_1 callback_)
        {
            m_subject.Detach(type_, callback_);
        }

        static public bool Notify(string type_, object data_=null)
        {
            return m_subject.Notify(type_, data_);
        }

        static public bool NotifyEvent(SubjectEvent evt_)
        {
            return m_subject.NotifyEvent(evt_);
        }

        static public bool NotifyWithEvent(string type_, object data_=null)
        {
            return m_subject.NotifyWithEvent(type_, data_);
        }


        //-------∽-★-∽------∽-★-∽--------∽-★-∽AppPause/AppFocus/OnApplicationQuit∽-★-∽--------∽-★-∽------∽-★-∽--------//

        /// <summary>
        /// 应用暂停
        /// </summary>
        static public bool AppPause
        {
            get { return m_appPause; }
            set
            {
                if (m_appPause == value)
                    return;
                m_appPause = value;

                if (value)
                    OnAppPause();
                else
                    OnAppResume();
            }
        }

        
        static void OnAppPause()    //应用暂停
        {
            LogFile.Flush();

            //Log.Info("OnAppPause", __typeName);
        }

        
        static void OnAppResume()   //应用回复
        {

        }

        /// <summary>
        /// 应用获得焦点
        /// </summary>
        static public bool AppFocus
        {
            get { return m_appFocus; }
            set
            {
                if (m_appFocus == value)
                    return;
                m_appFocus = value;

                if (value)
                    OnAppFocusGain();
                else
                    OnAppFocusLost();
            }
        }

        static void OnAppFocusGain()    //应用获得焦点
        {

        }

        static void OnAppFocusLost()    //应用失去焦点
        {
            LogFile.Flush();

            //Log.Info("OnAppFocusLost", __typeName);

        }

        static public void OnAppQuit()    //应用退出
        {
            LogFile.Close();

            //Log.Info("OnAppQuit", __typeName);
        }

        //-------∽-★-∽------∽-★-∽--------∽-★-∽Schedule∽-★-∽--------∽-★-∽------∽-★-∽--------//

        /// <summary>
        /// 监听更新
        /// </summary>
        /// <param name="callback_"></param>
        static public void SchUpdate(CALLBACK_Float callback_)
        {
            m_onUpdate -= callback_;
            m_onUpdate += callback_;
        }

        static public void UnschUpdate(CALLBACK_Float callback_)
        {
            m_onUpdate -= callback_;
        }

        static public void SchUpdateOrNot(CALLBACK_Float callback_, bool b_)
        {
            if (b_)
                SchUpdate(callback_);
            else
                UnschUpdate(callback_);
        }

        /// <summary>
        /// 监听gui更新
        /// </summary>
        /// <param name="callback_"></param>
        static public void SchOnGUI(CALLBACK_Float callback_)
        {
            m_onGui -= callback_;
            m_onGui += callback_;
        }

        static public void UnschOnGUI(CALLBACK_Float callback_)
        {
            m_onGui -= callback_;
        }




    }

    //-------∽-★-∽------∽-★-∽--------∽-★-∽CCAppBhv∽-★-∽--------∽-★-∽------∽-★-∽--------//

    public class CCAppBhv : MonoBehaviour
    {

        public Action<float> OnUpdate;
        public Action<float> OnLateUpdate;

        public CCAppBhv()
        {
            //GameObject.FindObjectOfType(typeof(GameObject));
        }

        //当一个脚本实例被载入时Awake被调用，要先于Start
        //this.gameObject.GetComponent<Transform>().position = new Vector3();
        //this.transform.parent = null;
        //void Awake() { }

        //Start仅在Update函数第一次被调用前调用
        //void Start() { }

        //Reset是在用户点击检视面板的Reset按钮或者首次添加该组件时被调用.此函数只在编辑模式下被调用.
        //Reset最常用于在检视面板中给定一个最常用的默认值.
        //void Reset() { }

        void OnGUI()
        {
            CCApp.OnGUI(Time.deltaTime);
        }

        void Update()
        {
            CCApp.Step(Time.deltaTime);
            
            if (OnUpdate != null)
                OnUpdate(Time.deltaTime);
        }

        void LateUpdate()
        {
            CCApp.LateStep(Time.deltaTime);

            if (OnLateUpdate != null)
                OnLateUpdate(Time.deltaTime);
        }

        //void FixedUpdate()
        //{
        //    CCApp.FixedStep(Time.fixedDeltaTime);
        //}

        //物体启用时被调用
        //void OnEnable() { }

        //物体被禁用时调用
        //void OnDisable() { }

        //void OnDestroy()
        //{
            //物体被删除时调用
            //CCApp.Clear();
        //}


        void OnApplicationQuit()
        {
            CCApp.OnAppQuit();
        }

        void OnApplicationPause(bool paused)
        {
            
            CCApp.AppPause = paused;
        }

        void OnApplicationFocus(bool hasFocus)
        {

            CCApp.AppFocus = hasFocus;
        }


    }

}