/* ==============================================================================
 * KEvtCenter
 * @author jr.zeng
 * 2017/6/19 12:01:36
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using UnityEngine;
using Object = UnityEngine.Object;

namespace mg.org.KUI
{

    public class KEvtCenter4Lua
    {
        static bool __enable = false;


        static KEvtCenter4Lua()
        {
            __enable = CCDefine.USE_LUA;
        }

        //-------∽-★-∽------∽-★-∽--------∽-★-∽Common∽-★-∽--------∽-★-∽------∽-★-∽--------//

        internal static List<int> UIVoidEvent = new List<int>();
        internal static List<BoolVal> UIBoolEvent = new List<BoolVal>();
        internal static List<FloatVal> UIFloatEvent = new List<FloatVal>();
        internal static List<StringVal> UIStringEvent = new List<StringVal>();
        internal static List<PositionVal> UIPositionEvent = new List<PositionVal>();
        internal static List<IndexVal> UIIndexEvent = new List<IndexVal>();

        public static void AddUIVoidEvent(int event_id)
        {
            if (!__enable) return;

            UIVoidEvent.Add(event_id);
        }

        public static void AddUIBoolEvent(int event_id, bool isValOn)
        {
            if (!__enable) return;

            UIBoolEvent.Add(new BoolVal { id = event_id, isOn = isValOn });
        }

        public static void AddUIFloatEvent(int event_id, float fval)
        {
            if (!__enable) return;

            UIFloatEvent.Add(new FloatVal { id = event_id, value = fval });
        }

        public static void AddUIStringEvent(int event_id, string sval)
        {
            if (!__enable) return;

            UIStringEvent.Add(new StringVal { id = event_id, value = sval });
        }


        public static void AddUIPositionEvent(int event_id, float x, float y)
        {
            if (!__enable) return;

            UIPositionEvent.Add(new PositionVal { id = event_id, x = x, y = y });
        }

        public static void AddUIIndexEvent(int event_id, int index_)
        {
            if (!__enable) return;

            UIIndexEvent.Add(new IndexVal { id = event_id, index = index_ });
        }

        //-------∽-★-∽------∽-★-∽--------∽-★-∽List∽-★-∽--------∽-★-∽------∽-★-∽--------//

        internal static List<ListVal> UIListEvent = new List<ListVal>();

        public static void AddUIListEvent(int event_id, int index)
        {
            if (!__enable) return;

            UIListEvent.Add(new ListVal { id = event_id, index = index });
        }


        //-------∽-★-∽------∽-★-∽--------∽-★-∽Button∽-★-∽--------∽-★-∽------∽-★-∽--------//


        internal static List<BtnVal> BtnClicked = new List<BtnVal>();
        internal static List<BtnVal> BtnPointerDown = new List<BtnVal>();
        internal static List<BtnVal> BtnPointerUp = new List<BtnVal>();

        public static void AddBtnClickEvent(int id, int inst_id)
        {
            if (!__enable) return;

            BtnClicked.Add(new BtnVal { id = id, inst_id = inst_id });
        }


        public static void AddBtnPointerDownEvent(int id, int inst_id)
        {
            if (!__enable) return;

            BtnPointerDown.Add(new BtnVal { id = id, inst_id = inst_id });
        }
        public static void AddBtnPointerUpEvent(int id, int inst_id)
        {
            if (!__enable) return;

            BtnPointerUp.Add(new BtnVal { id = id, inst_id = inst_id });
        }


        //-------∽-★-∽------∽-★-∽--------∽-★-∽Drag∽-★-∽--------∽-★-∽------∽-★-∽--------//
        
        internal static List<DragVal> DragEventList = new List<DragVal>();
        internal static List<DragVal> DragEndEventList = new List<DragVal>();

        public static void AddDragEvent(int id, int inst_id, float x, float y, float pos_x, float pos_y)
        {
            if (!__enable) return;

            DragEventList.Add(new DragVal { id = id, inst_id = inst_id, x = x, y = y, pos_x = pos_x, pos_y = pos_y });
        }

        public static void AddDragEndEvent(int id, int inst_id, float x, float y, float pos_x, float pos_y)
        {
            if (!__enable) return;

            DragEndEventList.Add(new DragVal { id = id, inst_id = inst_id, x = x, y = y, pos_x = pos_x, pos_y = pos_y });
        }

        //-------∽-★-∽------∽-★-∽--------∽-★-∽Slider∽-★-∽--------∽-★-∽------∽-★-∽--------//

        internal static List<SiliderValue> SliderValueChanged = new List<SiliderValue>();
        public static void AddSliderChangeEvent(int id, float val, int inst_id)
        {
            if (!__enable) return;

            SliderValueChanged.Add(new SiliderValue { id = id, val = val, inst_id = inst_id });
        }

        //-------∽-★-∽------∽-★-∽--------∽-★-∽Toggle∽-★-∽--------∽-★-∽------∽-★-∽--------//

        //internal static List<ToggleEvent> ToggleToggled = new List<ToggleEvent>();
        //public static void AddToggleChangeEvent(int id, bool isOn)
        //{
        //    if (!__enable) return;

        //    ToggleToggled.Add(new ToggleEvent { id = id, isOn = isOn });
        //}

        internal static List<ToggleGroupEvent> ToggleGroupChanged = new List<ToggleGroupEvent>();
        public static void AddToggleGroupEvent(int id, int index, bool isOn)
        {
            if (!__enable) return;

            ToggleGroupChanged.Add(new ToggleGroupEvent { id = id, index = index, isOn = isOn });
        }

        //-------∽-★-∽------∽-★-∽--------∽-★-∽ScrollRect∽-★-∽--------∽-★-∽------∽-★-∽--------//

        internal static List<SREvent> SRectValChanged = new List<SREvent>();
        public static void AddSRectChangeEvent(int id, Vector2 v, int inst_id)
        {
            if (!__enable) return;

            SRectValChanged.Add(new SREvent { id = id, x = v.x, y = v.y, inst_id = inst_id });
        }

        //-------∽-★-∽------∽-★-∽--------∽-★-∽LongTap∽-★-∽--------∽-★-∽------∽-★-∽--------//

        internal static List<LongTapStartEvent> LongTapStartEventList = new List<LongTapStartEvent>();
        internal static List<LongTapEvent> LongTapEventList = new List<LongTapEvent>();
        internal static List<LongTapEndEvent> LongTapEndEventList = new List<LongTapEndEvent>();


        public static void AddLongTapStartEvent(int id, int inst_id)
        {
            if (!__enable) return;

            LongTapStartEventList.Add(new LongTapStartEvent { id = id, inst_id = inst_id });
        }

        public static void AddLongTapEvent(int id, int inst_id)
        {
            if (!__enable) return;

            LongTapEventList.Add(new LongTapEvent { id = id, inst_id = inst_id });
        }

        public static void AddLongTapEndEvent(int id, int inst_id)
        {
            if (!__enable) return;

            LongTapEndEventList.Add(new LongTapEndEvent { id = id, inst_id = inst_id });
        }

        //-------∽-★-∽------∽-★-∽--------∽-★-∽struct∽-★-∽--------∽-★-∽------∽-★-∽--------//

        public struct BtnVal
        {
            public int id;
            public int inst_id;
        }

        public struct DragVal
        {
            public int id;
            public int inst_id;
            public float x;
            public float y;
            public float pos_x;
            public float pos_y;
        }

        public struct ListVal
        {
            public int id;
            public int index;
        }

        public struct BoolVal
        {
            public int id;
            public bool isOn;
        }

       

        public struct FloatVal
        {
            public int id;
            public float value;
        }

        public struct StringVal
        {
            public int id;
            public string value;
        }

        public struct PositionVal
        {
            public int id;
            public float x;
            public float y;
        }

        public struct IndexVal
        {
            public int id;
            public float index;
        }

        internal struct SiliderValue
        {
            public int id;
            public float val;
            public int inst_id;
        }
        internal struct ToggleEvent
        {
            public int id;
            public bool isOn;
            public int inst_id;
        }

        internal struct ToggleGroupEvent
        {
            public int id;

            public int inst_id;
            public int index;
            public bool isOn;
        }

        internal struct SREvent
        {
            public int id;
            public float x;
            public float y;
            public int inst_id;
        }

        internal struct LongTapEvent
        {
            public int id;
            public int inst_id;
        }

        internal struct LongTapStartEvent
        {
            public int id;
            public int inst_id;
        }

        internal struct LongTapEndEvent
        {
            public int id;
            public int inst_id;
        }

    }

}