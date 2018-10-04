/* ==============================================================================
 * K输入框
 * @author jr.zeng
 * 2017/7/8 12:03:47
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace mg.org.KUI
{
    /// <summary>
    /// Input_content
    /// ├Label_text
    /// └Label_textholder
    /// </summary>
    public class KInputField : InputField
    {
        

        //编辑中
        bool m_editing = false;

        protected override void Awake()
        {
            base.Awake();
            Initialize();
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            StopEdit();
        }

        private void Initialize()
        {
            onValueChanged.AddListener(onValueChangedHandler);
            onEndEdit.AddListener(onEndEditHandler);
        }

        //-------∽-★-∽------∽-★-∽--------∽-★-∽数据管理∽-★-∽--------∽-★-∽------∽-★-∽--------//


        private void onValueChangedHandler(string text)
        {
            
            LuaEvtCenter.AddGoEvent(gameObject, KUI_EVT.VALUE_CHANGE, text);
        }


        private void onEndEditHandler(string text)
        {

            StopEdit();
            
            //if (this.enabled && gameObject.activeSelf)  //deactive也会导致edit end
            //{
            //    LuaEvtCenter.AddGoEvent(gameObject, KUI_EVT.EDIT_END, text); 
            //}

            LuaEvtCenter.AddGoEvent(gameObject, KUI_EVT.EDIT_END, text);
        }

        public override void OnSelect(BaseEventData eventData)
        {
            base.OnSelect(eventData);

            StartEdit();

           // Log.Debug("get focus", this);
        }

        //public override void OnSubmit(BaseEventData eventData)
        //{
        //    //触发了EndEdit后，再点回车会触发这个，且再次获得焦点
        //    base.OnSubmit(eventData);
        //    Log.Debug("OnSubmit", this);
        //}

        //开始输入
        void StartEdit()
        {
            if (m_editing) return;
            m_editing = true;

            //获取焦点
            AddFocusInput(this);

#if UNITY_STANDALONE
            //windows
            //考虑支持回车时,也会提交

#endif
        }

        void StopEdit()
        {
            if (!m_editing) return;
            m_editing = false;

            //丢失焦点
            RemoveFocusInput(this);
            //Log.Debug("lost focus", this);

        }


        //-------∽-★-∽------∽-★-∽--------∽-★-∽数据操作∽-★-∽--------∽-★-∽------∽-★-∽--------//

        /// <summary>
        /// 追加文本
        /// </summary>
        /// <param name="str"></param>
        public void AppendString(string str)
        {
            Append(str);
        }

        

        /// <summary>
        /// 获取焦点
        /// </summary>
        public void GainFocus()
        {
           this.ActivateInputField();
        }



        //-------∽-★-∽------∽-★-∽--------∽-★-∽MoveIndexEnd∽-★-∽--------∽-★-∽------∽-★-∽--------//

        Queue<MoveTextEndReq> m_moveTextReq = new Queue<MoveTextEndReq>();

        public void MoveIndexEnd(bool shift)
        {
            m_moveTextReq.Enqueue(new MoveTextEndReq { version = Time.frameCount, shift = shift });
        }

        void Update()
        {

            if (m_moveTextReq.Count > 0)
            {
                var frame = Time.frameCount;
                while (m_moveTextReq.Count > 0)
                {
                    var top = m_moveTextReq.Peek();
                    if (frame > top.version)
                    {
                        this.MoveTextEnd(top.shift);
                        m_moveTextReq.Dequeue();
                    }
                    else
                    {
                        break;
                    }
                }
            }
              
        }

        
        struct MoveTextEndReq
        {
            public int version;
            public bool shift;
        }



        //-------∽-★-∽------∽-★-∽--------∽-★-∽统计编辑中的文本∽-★-∽--------∽-★-∽------∽-★-∽--------//

        static Dictionary<KInputField, bool> __input2bool = new Dictionary<KInputField, bool>();

        /// <summary>
        /// 添加焦点输入框
        /// </summary>
        /// <param name="input_"></param>
        static public void AddFocusInput(KInputField input_)
        {
            __input2bool[input_] = true;

        }

        static public void RemoveFocusInput(KInputField input_)
        {
            if (!__input2bool.ContainsKey(input_))
                return;
            __input2bool.Remove(input_);
        }


        /// <summary>
        /// 当前有焦点输入框
        /// </summary>
        static public bool HasFocusInput()
        {
            return __input2bool.Count > 0;
        }


    }


}