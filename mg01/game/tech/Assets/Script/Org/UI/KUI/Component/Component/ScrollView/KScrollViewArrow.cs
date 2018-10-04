/* ==============================================================================
 * KScrollViewArrow
 * @author jr.zeng
 * 2017/7/19 15:16:33
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;


namespace mg.org.KUI
{

    public class KScrollViewArrow : KContainer
    {
        StateImage m_nextImage;
        StateImage m_prevImage;

      

        protected override void __Initialize()
        {
            m_prevImage = GetChildComponent<StateImage>("Image_arrowPrev");
            m_nextImage = GetChildComponent<StateImage>("Image_arrowNext");

            if (m_prevImage)
                m_prevImage.Visible = false;
        }
        

        public void Refresh(Vector2 scrollPosition, KScrollView view)
        {
            bool prevVisible = false;
            bool nextVisible = false;

            //Log.Debug("scrollPosition " + scrollPosition, this);

            bool needScroll = view.NeedScroll();
            KScrollView.ScrollDir direction = view.direction;
            
            if (needScroll)
            {
                if (direction == KScrollView.ScrollDir.vertical)
                {
                    if (scrollPosition.y > 0)
                        nextVisible = true;
                    if (scrollPosition.y < 1)
                        prevVisible = true;
                }
                else
                {
                    if (scrollPosition.x > 0)
                        prevVisible = true;
                    if (scrollPosition.x < 1)
                        nextVisible = true;
                }
            }
            else
            {
                //内容比遮罩短时, 除非强行拽否则不显示箭头
                //需要配合scrollrect的修改
                if (direction == KScrollView.ScrollDir.vertical)
                {
                    if (scrollPosition.y > 1)   //强行往下拽, 显示下箭头
                        nextVisible = true;
                    if (scrollPosition.y < 0)   //强行往上拽, 显示上箭头
                        prevVisible = true;
                }
                else
                {
                    if (scrollPosition.x > 1)
                        prevVisible = true;
                    if (scrollPosition.x < 0)
                        nextVisible = true;
                }
            }

            if (m_prevImage)
                m_prevImage.Visible = prevVisible;

            if (m_nextImage)
                m_nextImage.Visible = nextVisible;
        }



    }


}