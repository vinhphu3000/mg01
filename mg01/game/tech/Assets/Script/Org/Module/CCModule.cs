/* ==============================================================================
 * CCModule
 * @author jr.zeng
 * 2016/6/22 16:01:40
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace mg.org
{
    public class CCModule : BaseObject, ISubject
    {

        protected bool m_isOpen = false;
        protected bool m_schUpdated = false;
        protected Subject m_notifier = null;

        public CCModule()
        {


        }

        public void Setup(params object[] params_)
        {
            if (m_isOpen) return;
            m_isOpen = true;

            __Setup(params_);
            SetupEvent();
        }

        public void Clear()
        {
            if (!m_isOpen) return;
            m_isOpen = false;

            ClearEvent();
            UnscheduleUpdate();
            DetachAll();

            __Clear();

            NotifyDeactive();
        }


        virtual protected void __Setup(params object[] params_)
        {

        }

        virtual protected void __Clear()
        {


        }


        virtual protected void SetupEvent()
        {

        }

        virtual protected void ClearEvent()
        {

        }

        //-------∽-★-∽------∽-★-∽--------∽-★-∽数据管理∽-★-∽--------∽-★-∽------∽-★-∽--------//


        //-------∽-★-∽------∽-★-∽--------∽-★-∽观察者∽-★-∽--------∽-★-∽------∽-★-∽--------//

       
        
        public void Attach(string type_, CALLBACK_1 callback_, object refer_)
        {
            if (m_notifier == null)
                m_notifier = CCApp.subject;
            m_notifier.Attach(type_, callback_, refer_);
        }

        public void Detach(string type_, CALLBACK_1 callback_)
        {
            if (m_notifier == null)
                return;
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

        public void ClearNotifier()
        {
            if (m_notifier == null)
                return;
            m_notifier.DetachAll();
        }



        //-------∽-★-∽------∽-★-∽--------∽-★-∽Schedule∽-★-∽--------∽-★-∽------∽-★-∽--------//

        protected void ScheduleUpdate()
        {
            if (m_schUpdated)
                return;
            m_schUpdated = true;
            CCApp.SchUpdate(Step);
        }

        protected void UnscheduleUpdate()
        {
            if (!m_schUpdated)
                return;
            m_schUpdated = false;
            CCApp.UnschUpdate(Step);
        }


        virtual public void Step(float dt_)
        {

        }


    }
}
