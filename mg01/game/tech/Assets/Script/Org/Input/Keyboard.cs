/* ==============================================================================
 * 键盘
 * @author jr.zeng
 * 2016/6/8 15:11:03
 * ==============================================================================*/

using UnityEngine;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace mg.org
{
    public class Keyboard : Subject
    {

        bool m_isOpen = false;
        //key2down
        Dictionary<KeyCode, bool> m_key2down;

        List<KeyCode> m_delKeys_key2down = new List<KeyCode>();

        //按下的数量
        int m_downNum = 0;

        public Keyboard()
        {
            m_key2down = new Dictionary<KeyCode, bool>();
        }

        public void Setup()
        {
            if (m_isOpen)
                return;
            m_isOpen = true;

            SetupEvent();

        }

        public void Clear()
        {
            if (!m_isOpen)
                return;
            m_isOpen = false;

            ClearEvent();
            ReleaseAllKeys();
        }

        void SetupEvent()
        {
            if (CCDefine.USE_KEYBOARD)
                CCApp.SchOnGUI(Step);   //键盘事件需要监听gui更新
        }

        void ClearEvent()
        {
            if (CCDefine.USE_KEYBOARD)
                CCApp.UnschOnGUI(Step);   //键盘事件需要监听gui更新

        }

        //-------∽-★-∽------∽-★-∽--------∽-★-∽数据管理∽-★-∽--------∽-★-∽------∽-★-∽--------//

        void Step(float delta_)
        {
           
            if (m_downNum > 0)
            {
                //有按下,检测弹起

                //注意: 不能在字典迭代时修改字典
                foreach (KeyValuePair<KeyCode, bool> kvp in m_key2down)
                {
                    KeyCode key = kvp.Key;
                    if (!Input.GetKey(key))
                    {
                        m_delKeys_key2down.Add(key);
                    }
                }

                if (m_delKeys_key2down.Count > 0)
                {
                    for (int i = 0; i < m_delKeys_key2down.Count; ++i)
                    {
                        SetKeyPressed(m_delKeys_key2down[i], false);
                    }
                    m_delKeys_key2down.Clear();
                }
            }

            if (Input.anyKeyDown)
            {
                //有按下
                Event e = Event.current;
                if (e.isKey)
                {
                    if (CheckPressCnd())
                    {
                        SetKeyPressed(e.keyCode, true);
                    }
                }
            }
        }


        //-------∽-★-∽------∽-★-∽--------∽-★-∽数据操作∽-★-∽--------∽-★-∽------∽-★-∽--------//

        //设置按下
        //PS: 暂时不支持按下的时候去设置为弹起
        public void SetKeyPressed(KeyCode key_, bool isPressed_)
        {
            if (key_ == KeyCode.None)
                //每次触发都会有none。。。
                return;

            if (isPressed_)
            {
                //按下
                if (m_key2down.ContainsKey(key_))
                {
                    if (m_key2down[key_])
                        return;
                }

                m_key2down[key_] = true;
                ++m_downNum;
                Notify(KEY_EVENT.PRESS, key_);

                //Log.Debug("key press " + key_, this);
            }
            else
            {
                if (!m_key2down.ContainsKey(key_))
                    return;

                if (!m_key2down[key_])
                    return;

                m_key2down[key_] = false;
                --m_downNum;
                Notify(KEY_EVENT.RELEASE, key_);

                //Log.Debug("key release " + key_, this);
            }
        }

        /// <summary>
        /// 是否按下
        /// </summary>
        /// <param name="key_"></param>
        /// <returns></returns>
        public bool IsPressed(KeyCode key_)
        {
            return (m_key2down.ContainsKey(key_) && m_key2down[key_]);
        }


        /// <summary>
        /// 有按下
        /// </summary>
        /// <returns></returns>
        public bool HasKeyPressed()
        {
            return m_downNum > 0;
        }

        /// <summary>
        /// 弹起所有
        /// </summary>
        public void ReleaseAllKeys()
        {
            if (m_downNum == 0)
                //没有按下
                return;
            m_downNum = 0;

            //var keys = from kvp in m_key2downDic
            //           where kvp.Value == true
            //           select kvp.Key;

            foreach (KeyValuePair<KeyCode, bool> kvp in m_key2down)
            {
                KeyCode key = kvp.Key;
                if (kvp.Value)
                {
                    m_delKeys_key2down.Add(key);
                }
            }

            if (m_delKeys_key2down.Count > 0)
            {
                foreach (KeyCode key in m_delKeys_key2down)
                {
                    m_key2down[key] = false;
                    Notify(KEY_EVENT.RELEASE, key);
                }
                m_delKeys_key2down.Clear();
            }
        }


        //-------∽-★-∽------∽-★-∽--------∽-★-∽static∽-★-∽--------∽-★-∽------∽-★-∽--------//

        public static bool CheckPressCnd()
        {
            if (KUI.KInputField.HasFocusInput()) 
            {
                //在输入文字
                return false;
            }

            return true;
        }

    }

}