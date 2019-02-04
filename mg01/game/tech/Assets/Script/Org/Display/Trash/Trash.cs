/* ==============================================================================
 * Trash
 * @author jr.zeng
 * 2017/6/7 14:34:30
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using UnityEngine;
using Object = UnityEngine.Object;


namespace mg.org
{

    public class Trash : Disposal
    {

        protected List<GameObject> m_objArr = new List<GameObject>();

        protected GameObject m_gameObject;
        protected Transform m_transform;

        protected override void __Dispose(bool disposing_)
        {
            Clear();

            if (disposing_)
            {
                if (m_gameObject != null)
                {
                    GameObjUtil.Delete(m_gameObject);
                    m_gameObject = null;
                }
            }
            else
            {
                if (m_gameObject != null)
                    Log.Fatal("gameObject销毁失败", this.GetType());
            }
        }


        public Trash(string name_ = "Trash")
        {
            InitGameObject(name_);
        }

        public int RemainCount
        {
            get { return m_objArr.Count; }
        }
        
        //-------∽-★-∽------∽-★-∽--------∽-★-∽gameObject∽-★-∽--------∽-★-∽------∽-★-∽--------//
        
        public GameObject gameObject
        {
            get { return m_gameObject; }
        }

        public Transform transform
        {
            get { return m_transform; }
        }

        public override string name
        {
            get { return m_name; }
            set
            {
                m_name = value;
                if (m_gameObject != null)
                    m_gameObject.name = m_name;
            }
        }

        //初始化容器
        protected void InitGameObject(string name_)
        {
            if (m_gameObject)
                return;

            string name = string.IsNullOrEmpty(name_) ? this.GetType().Name : name_;    //容器名称
            m_gameObject = GameObjUtil.CreateGameobj(name);
            m_gameObject.isStatic = true;   //设置为静态对象
            m_transform = m_gameObject.transform;

            GameObjUtil.DontDestroyOnLoad(m_gameObject);

            AllocPoolPos(m_gameObject);
        }

        //-------∽-★-∽------∽-★-∽--------∽-★-∽数据管理∽-★-∽--------∽-★-∽------∽-★-∽--------//

        protected GameObject CreateObj()
        {
            return null;
        }

        //-------∽-★-∽------∽-★-∽--------∽-★-∽数据操作∽-★-∽--------∽-★-∽------∽-★-∽--------//

        /// <summary>
        /// 回收对象
        /// </summary>
        /// <param name="obj_"></param>
        public void Push(GameObject child_)
        {
            if (Contains(child_))
                return;

            GameObjUtil.RecordLocalMatrix(child_.transform);  //记录位置
            child_.transform.parent = m_transform;
            GameObjUtil.ApplyLocalMatrix(child_.transform);   //恢复位置

            m_objArr.Add(child_);
        }
        

        public GameObject Pop(GameObject child_, GameObject toParent_)
        {
            if (!Contains(child_))
                return null;
            
            m_objArr.Remove(child_);

            GameObjUtil.ChangeParent(child_, toParent_);
            return child_;
        }

        /// <summary>
        /// 销毁垃圾桶里的子对象
        /// </summary>
        /// <param name="child_"></param>
        /// <returns></returns>
        public bool Delete(GameObject child_)
        {
            if (!Contains(child_))
                return false;

            m_objArr.Remove(child_);
            GameObjUtil.Delete(child_);
            return true;
        }

        public bool Contains(GameObject child_)
        {
            if(m_objArr.Contains(child_))
            {
                if (child_.transform.parent != m_transform)
                {
                    Log.Assert("意外的容器!!");
                }
                return true;
            }
            return false;
        }
        
        /// <summary>
        /// 清空所有子对象
        /// 仅清除对象列表
        /// </summary>
        public void Clear()
        {
            if (m_objArr.Count == 0)
                return;

            GameObject go;
            var enumerator = m_objArr.GetEnumerator();
            while (enumerator.MoveNext())
            {
                go = enumerator.Current;
                GameObjUtil.Delete(go); //直接销毁,会不会导致其他引用者产生空对象?
            }
            enumerator.Dispose();

            m_objArr.Clear();
        }


        //-------∽-★-∽------∽-★-∽--------∽-★-∽容器位置分配∽-★-∽--------∽-★-∽------∽-★-∽--------//

        static int m_allocIndex = 0;
        static int m_allocCol = 5;  //一行的个数  

        static Vector2 m_allocPos = new Vector2(-1000, -1000);
        static Vector2 m_allocSize = new Vector2(-500, -500);

        //分配对象池的位置
        public static void AllocPoolPos(GameObject go_)
        {
            int col = m_allocIndex % m_allocCol;    //行
            int row = m_allocIndex / m_allocCol;    //列

            float x = m_allocPos.x + m_allocSize.x * (col + 1);
            float y = m_allocPos.y + m_allocSize.y * (row + 1);

            DisplayUtil.SetPos2(go_, x, y);

            ++m_allocIndex;
        }



    }

}