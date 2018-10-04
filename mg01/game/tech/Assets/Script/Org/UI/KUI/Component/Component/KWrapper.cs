/* ==============================================================================
 * KObject
 * @author jr.zeng
 * 2017/9/1 10:56:16
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Object = UnityEngine.Object;


namespace mg.org.KUI
{

    public class KWrapper : UIBehaviour
    {
        
        public RectTransform rectTrans;

        protected override void Awake()
        {
            base.Awake();
            Initialize();
        }
        

        void Initialize()
        {
            rectTrans = GetComponent<RectTransform>();
            Log.Assert(rectTrans, "not ui object");
        }


        //-------∽-★-∽------∽-★-∽--------∽-★-∽Getter∽-★-∽--------∽-★-∽------∽-★-∽--------//

        /// <summary>
        /// 外框矩形
        /// </summary>
        public Rect rect
        {
            get { return rectTrans.rect; }
        }

        //-------∽-★-∽------∽-★-∽--------∽-★-∽数据操作∽-★-∽--------∽-★-∽------∽-★-∽--------//



        //-------∽-★-∽------∽-★-∽--------∽-★-∽static∽-★-∽--------∽-★-∽------∽-★-∽--------//

        static public KWrapper Ensure(GameObject go_)
        {
            KWrapper obj = go_.GetComponent<KWrapper>();
            if (obj == null)
                obj = go_.AddComponent<KWrapper>();
            return obj;
        }


    }

}