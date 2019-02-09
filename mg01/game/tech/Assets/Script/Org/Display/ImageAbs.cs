/* ==============================================================================
 * 视图抽象类
 * @author jr.zeng
 * 2016/9/19 10:48:17
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using Object = UnityEngine.Object;

namespace mg.org
{


    public class ImageAbs : Ref, IImgAbs, ISubject
    {

        protected bool m_isOpen = false;

        //本视图类的go
        protected GameObject m_gameObject;
        //go是否外部引入
        bool m_goIsExternal;

        //派发器
        protected Subject m_notifier;
        //已启动更新
        bool m_isSchUpdte = false;


        protected override void __Dispose(bool disposing_)
        {
            Destroy();
            __Dispose();
            ClearGameObject();
        }


        //析构函数
        virtual protected void __Dispose()
        {

        }

        public ImageAbs()
        {
            AutoRelease(); 
            
        }

      
        public void Show()
        {
            Show(null);
        }


        virtual public void Show(object showObj_, params object[] params_)
        {

            __Show(showObj_, params_);

            m_isOpen = true;

            SetupEvent();


        }

        virtual protected void __Show(object showObj_ , params object[] params_)
        {

        }


        virtual protected void SetupEvent()
        {

        }

        virtual protected void ClearEvent()
        {

        }


        public bool isOpen
        {
            get { return m_isOpen; }
        }
        

        override public String name
        {
            get { return m_name; }
            set 
            { 
                m_name = value;
                if (m_gameObject && !m_goIsExternal)
                {
                    //不是外部go,名字赋为内类名字
                    m_gameObject.name = String.Format("{0}({1})", m_name, TypeName);
                }
            }
        }

        public virtual bool visible
        {
            get { return gameObject.activeSelf; }
            set
            {
                gameObject.SetActive(value);
            }
        }

        virtual protected void __Destroy()
        {

        }

        

        /// <summary>
        /// 清除但不移除
        /// </summary>
        public void Destroy()
        {
            if (!m_isOpen)
                return;
            m_isOpen = false;

            //清空事件
            ClearEvent();
            //清空观察者
            DetachAll();
            //停止update
            UnschUpdate();
            //执行清除
            __Destroy();

            //通知沉默
            NotifyDeactive();

        }

        /// <summary>
        /// 清除 且 移除显示
        /// 跟gameobject.destroy不一样 
        /// 此接口只清除数据, 不销毁本对象
        /// </summary>
        public void DestroyRemove()
        {
            if (!m_isOpen)
                return;
            m_isOpen = false;

            //清空事件
            ClearEvent();
            //清空观察者
            DetachAll();
            //停止update
            UnschUpdate();

            //执行清除
            __Destroy();

            //通知沉默
            NotifyDeactive();

            if (!m_disposed)   
            {
                //已经析构了就不用移除
                RemoveChildThis();  //移除自己
            }
        }

        

        //-------∽-★-∽------∽-★-∽--------∽-★-∽加载相关∽-★-∽--------∽-★-∽------∽-★-∽--------//

        //public Object LoadSync(string resId_, string resName_)
        //{
        //    Object data = CCApp.resMgr.LoadSync(resId_, resName_, this);
        //    return data;
        //}

        //public void LoadAsync(string resId_, string resName_, CALLBACK_1 onComplete_)
        //{
        //    CCApp.resMgr.LoadAsync(resId_, resName_, onComplete_, this);
        //}


        //-------∽-★-∽------∽-★-∽--------∽-★-∽GameObject∽-★-∽--------∽-★-∽------∽-★-∽--------//


        public Transform transform
        {
            get { return m_gameObject.transform; }
        }


        public GameObject gameObject
        {
            get { return m_gameObject; }

        }

        public GameObject parent
        {
            get { return m_gameObject.transform.parent.gameObject; }

        }


        public bool goIsExternal
        {
            get { return m_goIsExternal; }

        }


        /// <summary>
        /// 显示默认go, 子类重写
        /// </summary>
        virtual protected void ShowGameObject()
        {
            if (m_gameObject == null)
            {
                //如果没有, 则创建一个空容器
                ShowGameObject(GameObjUtil.CreateGameobj(TypeName), false);
            }
        }

        /// <summary>
        /// 显示外部go
        /// </summary>
        /// <param name="go_"></param>
        public void ShowGameObjectEx(GameObject go_)
        {
            ShowGameObject(go_, true);
        }

        /// <summary>
        /// 显示go
        /// </summary>
        /// <param name="go_"></param>
        /// <param name="isExternal_">go是否外部引入</param>
        protected void ShowGameObject(GameObject go_, bool isExternal_ = false)
        {
            if (m_gameObject == go_)
                return;

            ClearGameObject();

            m_gameObject = go_;

            m_goIsExternal = isExternal_;
            if (!m_goIsExternal)
            {
                //不是外部go,名字赋为自身的名字
                string name = StringUtil.SubToFirst( m_gameObject.name, "(");
                name = String.Format("{0}({1})", name, ReferId);
                m_gameObject.name = name;
            }

            __ShowGameObject();
        }


        protected void ShowGameObject(string resId_, string resName_)
        {
            GameObject obj = CCApp.resMgr.LoadGameObj(resId_, resName_) as GameObject;
            ShowGameObject(obj, false); //因为是自己create, 所以非外部
        }
        

        protected void ClearGameObject()
        {
            if (m_gameObject == null)
                return;

            __ClearGameObject();

            if (m_goIsExternal)
            {
                //如果是外部go, 只是移除引用
                m_gameObject = null;
            }
            else
            {
                //是自己托管的go, 则删除对象
                GameObjUtil.Delete(m_gameObject);
                //GameObjUtil.DestroyNow(m_gameObject);
                m_gameObject = null;
            }
        }

        virtual protected void __ShowGameObject()
        {

        }

        virtual protected void __ClearGameObject()
        {

        }


        //-------∽-★-∽------∽-★-∽--------∽-★-∽Transform∽-★-∽--------∽-★-∽------∽-★-∽--------//
        

        //移出父级
        virtual protected void RemoveChildThis()
        {
            if (m_goIsExternal)
                //外部go,不负责移除
                return;

            GameObjUtil.RemoveFromParent(this.gameObject);
        }


        public virtual GameObject GetChildByName(string name_, bool isRecursive = false)
        {
            GameObject go = GameObjUtil.FindChild(this.gameObject, name_, isRecursive);
            if (go == null)
                Log.Warn("找不到子对象:" + name_, this);
            return go;
        }

        public virtual GameObject GetChildByName(GameObject parent_, string name_, bool isRecursive = false)
        {
            GameObject go = GameObjUtil.FindChild(parent_, name_, isRecursive);
            if (go == null)
                Log.Warn("找不到子对象:" + name_, this);
            return go;
        }

        public virtual T GetChildByName<T>(string name_, bool isRecursive = false)
        {
            T cmpt = default(T);
            GameObject go = GetChildByName(name_, isRecursive);
            if (go != null)
            {
                cmpt = go.GetComponent<T>();
                if (cmpt == null)
                    Log.Warn(string.Format("找不到组件:{0} {1}", name_, typeof(T).Name), this);
            }
            return cmpt;
        }

        public virtual T GetChildByName<T>(GameObject parent_, string name_, bool isRecursive = false)
        {
            T cmpt = default(T);
            GameObject go = GetChildByName(parent_, name_, isRecursive);
            if (go != null)
            {
                cmpt = go.GetComponent<T>();
                if (cmpt == null)
                    Log.Warn(string.Format("找不到组件:{0} {1}", name_, typeof(T).Name), this);
            }
            return cmpt;
        }


        /// <summary>
        /// 替换子对象
        /// </summary>
        /// <param name="name_"></param>
        /// <param name="replace_"></param>
        public void ReplaceChildByName(string name_, GameObject replace_)
        {
            GameObject child = GetChildByName(name_, true);
            if (child == null)
                return;

            GameObject parent = GameObjUtil.GetParent(child);
            GameObjUtil.RecordLocalMatrix(child.transform);
            GameObjUtil.ApplyLocalMatrix(replace_.transform);

            GameObjUtil.Delete(child);  //删除原来的子对象
            GameObjUtil.ChangeParent(replace_, parent);
        }

      
        //-------∽-★-∽------∽-★-∽--------∽-★-∽Subject∽-★-∽--------∽-★-∽------∽-★-∽--------//

       

        public void Attach(string type_, CALLBACK_1 callback_, object target_=null)
        {
            if (m_notifier == null)
                m_notifier = new Subject();
            m_notifier.Attach(type_, callback_, target_);
        }

        public void Detach(string type_, CALLBACK_1 callback_)
        {
            if (m_notifier == null)
                return ;
            m_notifier.Detach(type_, callback_);
        }

        public void DetachByType(string type_)
        {
            if (m_notifier == null)
                return;
            m_notifier.DetachByType(type_);
        }

        public void DetachAll()
        {
            if (m_notifier == null)
                return;
            m_notifier.DetachAll();
        }

        public bool Notify(string type_, object data_ = null)
        {
            if (m_notifier == null)
                return false;
            return m_notifier.Notify(type_, data_);
        }

        public bool NotifyEvent(SubjectEvent evt_)
        {
            if (m_notifier == null)
                return false;
            return m_notifier.NotifyEvent(evt_);
        }

        public bool NotifyWithEvent(string type_, object data_ = null)
        {
            if (m_notifier == null)
                return false;
            return m_notifier.NotifyWithEvent(type_, data_);
        }

        public bool HasAttach(string type_)
        {
            if (m_notifier == null)
                return false;
            return m_notifier.HasAttach(type_);
        }




        //-------∽-★-∽------∽-★-∽--------∽-★-∽Schedule∽-★-∽--------∽-★-∽------∽-★-∽--------//

        protected void SchUpdate()
        {
            if (m_isSchUpdte)
                return;
            m_isSchUpdte = true;
            __SchUpdate();
        }


        protected void UnschUpdate()
        {
            if (!m_isSchUpdte)
                return;
            m_isSchUpdte = false;
            __UnschUpdate();
        }

        //实际监听update, 默认监听CCApp, 可根据需要重写
        virtual protected void __SchUpdate()
        {
            CCApp.SchUpdate(Step);
        }

        virtual protected void __UnschUpdate()
        {
            CCApp.UnschUpdate(Step);
        }

        
        //为跟原生Update函数作区别, 一律叫Step
        virtual public void Step(float dt_)
        {

        }

    }


}