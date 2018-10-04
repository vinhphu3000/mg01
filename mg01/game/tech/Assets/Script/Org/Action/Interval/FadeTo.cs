/* ==============================================================================
 * 动作_FadeTo
 * @author jr.zeng
 * 2016/10/27 15:42:26
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace mg.org.Actions
{

    public class FadeTo : ActionInterval
    {

        public float m_from = 1f;
        public float m_to = 1f;
        
#if CC_USE_NGUI
        UIRect m_uiRect;
#endif

        Material m_material;
        SpriteRenderer m_spRender;

        public FadeTo()
        {

        }


        public void InitWithAlpha(float duration_, float to_)
        {
            InitWithDuration(duration_);

            m_to = Mathf.Clamp01(to_);
        }


        public override void StartWithTarget(GameObject target_)
        {
            base.StartWithTarget(target_);

            RefreshRender();

            m_from = value;
        }

        void RefreshRender()
        {

#if CC_USE_NGUI
            m_uiRect = m_target.GetComponent<UIRect>();
            if (m_uiRect != null)
                return;
#endif
            m_spRender = m_target.GetComponent<SpriteRenderer>();
            if (m_spRender != null)
                return;

            Renderer render = m_target.GetComponent<Renderer>();
            if (render != null)
            {
                m_material = render.material;
                if (m_material != null)
                    return;
            }
            
#if CC_USE_NGUI
            m_uiRect = m_target.GetComponentInChildren<UIRect>();
             if (m_uiRect != null)
                return;
#endif
        }

        protected override void OnProgress(float progress_)
        {

            //value = m_from * (1f - progress_) + m_to * progress_;
            value = Mathf.Lerp(m_from, m_to, progress_);
        }


        public float value
        {
            get
            {
#if CC_USE_NGUI
                if (m_uiRect != null)
                    return m_uiRect.alpha;
#endif
                if (m_spRender != null)
                    return m_spRender.color.a;

                if (m_material != null)
                    return m_material.color.a;
                return 1f;
            }

            set
            {

#if CC_USE_NGUI
                if (m_uiRect != null)
                {
                    m_uiRect.alpha = value;
                    return;
                }
#endif
                if (m_spRender != null)
                {
                    Color c = m_spRender.color;
                    c.a = value;
                    m_spRender.color = c;
                    return;
                }

                if (m_material != null)
                {
                    Color c = m_material.color;
                    c.a = value;
                    m_material.color = c;
                    return;
                }
            }
        }

        protected override void OnClear()
        {
            base.OnClear();
        
        
#if CC_USE_NGUI
            m_uiRect = null;
#endif
            m_material = null;
            m_spRender = null;
        }


        //-------∽-★-∽------∽-★-∽--------∽-★-∽static∽-★-∽--------∽-★-∽------∽-★-∽--------//


        static public FadeTo Create(float duration_, float to_)
        {
            FadeTo action = new FadeTo();
            action.InitWithAlpha(duration_, to_);

            return action;
        }



    }


}