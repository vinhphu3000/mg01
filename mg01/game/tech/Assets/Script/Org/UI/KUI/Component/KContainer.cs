/* ==============================================================================
 * KContainer
 * @author jr.zeng
 * 2017/6/16 14:42:03
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Object = UnityEngine.Object;

namespace mg.org.KUI
{

    public class KContainer : UIBehaviour, IKContainer, IRefer
    {
        bool m_inited = false;
        protected override void OnDestroy()
        {
            base.OnDestroy();

            NotifyDispose();
        }


        protected override void Awake()
        {
            base.Awake();

            Initialize();

        }

        public void Initialize()
        {
            if (m_inited)
                return;
            m_inited = true;

            __Initialize();
        }

        protected virtual void __Initialize()
        {


        }


        public GameObject GetChild(string path)
        {
            return KComponentUtil.NeedChild(gameObject, path);
        }

        public T GetChildComponent<T>(string path) where T : Component
        {
            return KComponentUtil.NeedChildComponent<T>(gameObject, path);
        }

        public T AddChildComponent<T>(string path) where T : Component
        {
            return ComponentUtil.AddChildComponent<T>(gameObject, path);
        }

        /// <summary>
        /// 是否可见
        /// </summary>
        public bool Visible
        {
            get { return gameObject.activeSelf;  }
            set
            {
                gameObject.SetActive(value);
            }
        }


        /// <summary>
        /// 尺寸自适应
        /// </summary>
        public void RecalculateSize()
        {
            RectTransform rect = GetComponent<RectTransform>();
            float width = 0;
            float height = 0;
            for (int i = 0; i < transform.childCount; i++)
            {
                RectTransform childRect = transform.GetChild(i).GetComponent<RectTransform>();
                if (childRect.gameObject.activeSelf == false)
                {   //不可见的不计算
                    continue;
                }
                Vector2 position = childRect.anchoredPosition;
                Vector2 size = childRect.sizeDelta;
                if (width < (position.x + size.x))
                {
                    width = position.x + size.x;
                }
                if (height < (size.y - position.y))
                {
                    height = size.y - position.y;
                }
            }
            rect.sizeDelta = new Vector2(width, height);
        }

        
        //-------∽-★-∽------∽-★-∽--------∽-★-∽IRefer∽-★-∽--------∽-★-∽------∽-★-∽--------//
        public virtual string ReferId
        {
            get { return Refer.TransID(this); }
        }

        public void NotifyDeactive()
        {
            Refer.NotifyDeactive(this);
        }

        public void NotifyDispose()
        {
            Refer.NotifyDispose(this);
        }


    }

}