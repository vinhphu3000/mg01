/* ==============================================================================
 * LuaEvtCenter
 * @author jr.zeng
 * 2017/10/24 16:59:58
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using UnityEngine;
using Object = UnityEngine.Object;


namespace mg.org
{
    public class LuaEvtCenter
    {
        static public bool Enable = false;

        static ClassPool2<LuaEvt> __evtPool = ClassPools.me.CreatePool<LuaEvt>();
        static ClassPool2<LuaGoEvt> __goEvtPool = ClassPools.me.CreatePool<LuaGoEvt>();

        internal static List<LuaEvt> __events = new List<LuaEvt>();
        internal static List<LuaGoEvt> __goEvents = new List<LuaGoEvt>();

        static LuaEvtCenter()
        {

        }
        

        public static void Clear()
        {
            ClearEvents();
            ClearGoEvents();
        }


        //-------∽-★-∽------∽-★-∽--------∽-★-∽通用事件∽-★-∽--------∽-★-∽------∽-★-∽--------//


        public static void AddEvent(string type_, params object[] args_)
        {
            if (!Enable)
                return;

            LuaEvt evt = __evtPool.Pop();
            evt.type = type_;
            evt.args = args_;
            __events.Add(evt);
        }

        /// <summary>
        /// 清空gameobject事件列表
        /// </summary>
        public static void ClearEvents()
        {
            if (__events.Count == 0)
                return;

            LuaEvt evt;
            int len = __events.Count;
            for (int i = 0; i < len; ++i)
            {
                evt = __events[i];
                evt.Clear();
                __evtPool.Push(evt);
            }

            __events.Clear();

        }

        //-------∽-★-∽------∽-★-∽--------∽-★-∽gameobject事件∽-★-∽--------∽-★-∽------∽-★-∽--------//

        /// <summary>
        /// 添加gameobject事件
        /// </summary>
        /// <param name="go_"></param>
        /// <param name="type_"></param>
        /// <param name="args"></param>
        public static void AddGoEvent(GameObject go_, string type_, params object[] args_)
        {
            if (!Enable)
                return;

            LuaGoEvt evt = __goEvtPool.Pop();
            evt.go = go_;
            evt.type = type_;
            evt.args = args_;
            __goEvents.Add(evt);
        }

        /// <summary>
        /// 清空gameobject事件列表
        /// </summary>
        public static void ClearGoEvents()
        {
            if (__goEvents.Count == 0)
                return;

            LuaGoEvt evt;
            int len = __goEvents.Count;
            for (int i=0; i< len; ++i)
            {
                evt = __goEvents[i];
                evt.Clear();
                __goEvtPool.Push(evt);
            }

            __goEvents.Clear();

        }







        internal class LuaEvt
        {

            public string type;
            public object[] args;  //参数

            public virtual void Clear()
            {
                type = null;
                args = null;
            }

        }

        internal class LuaGoEvt : LuaEvt
        {
            public GameObject go;

            override public void Clear()
            {
                base.Clear();

                go = null;
            }
        }


    }

}