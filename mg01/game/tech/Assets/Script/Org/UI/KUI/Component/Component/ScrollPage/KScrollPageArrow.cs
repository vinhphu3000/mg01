/* ==============================================================================
 * KScrollPageArrow
 * @author jr.zeng
 * 2017/7/19 17:35:44
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using UnityEngine;
using Object = UnityEngine.Object;


namespace mg.org.KUI
{
    public class KScrollPageArrow : KContainer
    {
        private StateImage m_nextImage;
        private StateImage m_prevImage;

       

        protected override void __Initialize()
        {
            m_nextImage = GetChildComponent<StateImage>("Image_arrowNext");
            m_prevImage = GetChildComponent<StateImage>("Image_arrowPrev");
        }

        public void Refresh(int page, int totalPage)
        {
            m_nextImage.Visible = (page < totalPage - 1);
            m_prevImage.Visible = (page > 0);
        }
    }

}