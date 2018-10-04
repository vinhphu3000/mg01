/* ==============================================================================
 * 鼠标输入
 * @author jr.zeng
 * 2017/8/9 17:43:17
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using UnityEngine;
using Object = UnityEngine.Object;

namespace mg.org
{

    /// <summary>
    /// 鼠标键值
    /// </summary>
    public class MouseKey
    {
        public const int MOUSE_LEFT = 0;
        public const int MOUSE_RIGHT = 1;
        //static public int MOUSE_2 = 2;
        
        public static int[] KEYS = new int[] {
            MOUSE_LEFT,
            MOUSE_RIGHT,
        };

    }


     public class Mouse : Subject
    {


        public class MouseFlag
        {
            //关注移动事件
            public const int evt_move = 0x0001;
            //关注滚轮事件
            public const int evt_wheel = 0x0002;
        }

        bool m_isOpen = false;

        //key2down
        Dictionary<int, bool> m_key2down = new Dictionary<int, bool>();
        //按下的数量
        int m_downNum = 0;

        List<int> m_delKeys = new List<int>();
        //需要检测是鼠标键值
        int[] m_checkKeys = MouseKey.KEYS;

        //检测鼠标移动
        bool m_moveEnable = false;
        string m_axisNameX = "Mouse X";
        string m_axisNameY = "Mouse Y";
        Vector2 m_axisMove = new Vector2();
        //Vector2 m_axisMovePrev = new Vector2(); //用作判断开始结束

        //检测滚轮
        bool m_wheelEnable = false;
        string m_axisNameWheel = "Mouse ScrollWheel";
        float m_axisWheel;

        public Mouse()
        {

        }
        
        public Mouse(int flag_)
        {
            m_moveEnable = (flag_ & MouseFlag.evt_move) != 0;
            m_wheelEnable = (flag_ & MouseFlag.evt_wheel) != 0;
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
            CCApp.SchUpdate(Step);
        }

        void ClearEvent()
        {
            CCApp.UnschUpdate(Step);

        }

        //-------∽-★-∽------∽-★-∽--------∽-★-∽数据管理∽-★-∽--------∽-★-∽------∽-★-∽--------//

        void Step(float delta_)
        {
            foreach (int key in m_checkKeys)
            {
                if ( Input.GetMouseButton(key) )
                {
                    SetKeyPressed(key, true);
                }
                else if (m_downNum > 0)
                {
                    SetKeyPressed(key, false);
                }
            }

            if (m_moveEnable)
            {
                m_axisMove.x = Input.GetAxis(m_axisNameX);
                m_axisMove.y = Input.GetAxis(m_axisNameY);
                if (m_axisMove.x != 0 || m_axisMove.y != 0)
                {
                    NotifyWithEvent(MOUSE_EVENT.MOVE, m_axisMove);
                }
            }

            if(m_wheelEnable)
            {
                m_axisWheel = Input.GetAxis(m_axisNameWheel);
                if (m_axisWheel != 0)
                {
                    NotifyWithEvent(MOUSE_EVENT.WHEEL, m_axisWheel);
                }
            }

        }

        //-------∽-★-∽------∽-★-∽--------∽-★-∽数据操作∽-★-∽--------∽-★-∽------∽-★-∽--------//


        //-------∽-★-∽------∽-★-∽--------∽-★-∽数据操作∽-★-∽--------∽-★-∽------∽-★-∽--------//

        //设置按下
        //PS: 暂时不支持按下的时候去设置为弹起
        public void SetKeyPressed(int key_, bool isPressed_)
        {

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
                NotifyWithEvent(MOUSE_EVENT.PRESS, key_);
                //Log.Debug("mouse press " + key_, this);
            }
            else
            {
                if (!m_key2down.ContainsKey(key_))
                    return;

                if (!m_key2down[key_])
                    return;

                m_key2down[key_] = false;
                --m_downNum;
                NotifyWithEvent(MOUSE_EVENT.RELEASE, key_);
                //Log.Debug("mouse release " + key_, this);
            }
        }

        /// <summary>
        /// 是否按下
        /// </summary>
        /// <param name="key_"></param>
        /// <returns></returns>
        public bool IsPressed(int key_)
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
            
            foreach (var kvp in m_key2down)
            {
                int key = kvp.Key;
                if (kvp.Value)
                {
                    m_delKeys.Add(key);
                }
            }

            if (m_delKeys.Count > 0)
            {
                foreach (int key in m_delKeys)
                {
                    m_key2down[key] = false;
                    NotifyWithEvent(MOUSE_EVENT.RELEASE, key);
                }
                m_delKeys.Clear();
            }
        }



    }

}