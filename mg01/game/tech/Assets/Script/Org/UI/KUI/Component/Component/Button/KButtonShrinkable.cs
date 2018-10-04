/* ==============================================================================
 * KButton_带缩放动画
 * @author jr.zeng
 * 2017/6/19 11:19:59
 * ==============================================================================*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Object = UnityEngine.Object;

namespace mg.org.KUI
{

    public class KButtonShrinkable : KButton
    {

        private static bool __ignoreTimeScale = false;
        private static float __duration = 0.1f;

        private static IEnumerator StartTween(KButtonShrinkable btn)
        {
            float elapsedTime = 0;

            while (elapsedTime < __duration)
            {
                elapsedTime += __ignoreTimeScale ? Time.unscaledDeltaTime : Time.deltaTime;
                float precentage = Mathf.Clamp01(elapsedTime / __duration);
                btn.DoTween(precentage);
                yield return null;
            }
            
            btn.ClearShrink(btn.m_direction == ZOOM_IN);
        }

        //组件的中心必须是0.5|0.5
        const float MAX_SCALE = 1.0f;   
        const float MIN_SCALE = 0.9f;

        const int ZOOM_OUT = -1;
        const int ZOOM_IN = 1;

        bool m_inited = false;
       


        private bool m_shrinkEnabled = true;
        IEnumerator m_shrinkTween;
        bool m_tweening = false;

        int m_direction;
        float m_targetScale;
        float m_startScale;

        Vector3 m_initScale;
        float m_dtScale = 1;

        
        
        protected override void OnDisable()
        {
            base.OnDisable();
            ClearShrink();
        }

        public void ResetTransformRecord()
        {
            //Initialize();
        }

        public void ClearTransformRecord()
        {
            ClearShrink();
            //is_inited = false;
        }
        

        public override void OnPointerDown(PointerEventData eventData)
        {

            base.OnPointerDown(eventData);

            if (!m_tweening)
            {
                m_tweening = true;
                m_initScale = transform.localScale; //记录起始比例
            }

            Shrink(ZOOM_OUT);
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);
            Shrink(ZOOM_IN);
        }

        //-------∽-★-∽------∽-★-∽--------∽-★-∽Shrink∽-★-∽--------∽-★-∽------∽-★-∽--------//

        // 屏蔽缩放功能
        public void ShrinkEnabled(bool b_)
        {
            m_shrinkEnabled = b_;
        }

        void Shrink(int direction)
        {
            if (!m_shrinkEnabled)
                return;

            m_direction = direction;
            m_startScale = m_dtScale;
            m_targetScale = (m_direction == ZOOM_OUT) ? MIN_SCALE : MAX_SCALE;
            //if (is_leftTop)
            //{
            //    RectTransform rect = transform as RectTransform;
            //    RectTransformHelper.SetPivotSmart(rect, new Vector2(0.5f, 0.5f), true);
            //}

            if (m_shrinkTween != null)
            {
                StopCoroutine(m_shrinkTween);
                m_shrinkTween = null;
            }

            m_shrinkTween = StartTween(this);
            StartCoroutine(m_shrinkTween);
        }

        public void ClearShrink(bool force = true)
        {
            if (m_shrinkTween != null)
            {
                StopCoroutine(m_shrinkTween);
                m_shrinkTween = null;

                //while tweenning, we should reset status
            }


            if (force)
            {
                if (m_tweening)
                {
                    m_tweening = false;
                    m_dtScale = 1;
                    transform.localScale = m_initScale;
                }
            }

        }

        void DoTween(float precentage)
        {
            m_dtScale = m_startScale + (m_targetScale - m_startScale) * precentage;
            transform.localScale = new Vector3(m_initScale.x * m_dtScale, m_initScale.y * m_dtScale, m_initScale.z);

            //if(is_leftTop)
            //{
            //    float x = _centerX - _originalWidth * 0.5f * scale;
            //    float y = _centerY + _originaHeight * 0.5f * scale;
            //    transform.localPosition = new Vector3(x, y, 0);
            //}
        }
        
        



    }

}