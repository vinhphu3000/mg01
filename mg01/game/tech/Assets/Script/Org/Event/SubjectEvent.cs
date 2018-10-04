/* ==============================================================================
 * 观察者事件
 * @author jr.zeng
 * 2016/6/8 10:40:49
 * ==============================================================================*/


using System;
using System.Collections;


namespace mg.org
{
    public class SubjectEvent
    {
        static public Type Type = typeof(SubjectEvent);

        //事件类型
        public string type;
        //数据
        public object data;
        //当前目标
        public object curTarget;
        //来自对象池
        public bool isFromPool = false;
        //停止传递
        public bool isStopped = false;

        public SubjectEvent()
        {

        }

        /// <summary>
        /// 观察者事件
        /// </summary>
        /// <param name="evtType"></param>
        /// <param name="evtData"></param>
        public SubjectEvent(string evtType_, object evtData_ = null)
        {
            type = evtType_;
            data = evtData_;

        }

        virtual public void Clear()
        {
            type = null;
            data = null;
            curTarget = null;
        }

        /// <summary>
        /// 停止传递
        /// </summary>
        public void stopPropagation()
        {
            isStopped = true;
        }

        public SubjectEvent Clone()
        {
            SubjectEvent e = new SubjectEvent(type, data);
            e.curTarget = this.curTarget;
            return e;
        }




    }

}