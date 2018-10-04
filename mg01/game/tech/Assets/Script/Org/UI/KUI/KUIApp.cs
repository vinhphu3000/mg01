/* ==============================================================================
 * KUIApp
 * @author jr.zeng
 * 2017/6/21 17:53:34
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using UnityEngine;
using Object = UnityEngine.Object;

namespace mg.org.KUI
{
    public class KUIApp
    {
        static bool m_isOpen = false;

        static GameObject m_uiLayer;
        static Camera m_uiCamera;

        //窗口管理
        static KUIPopMgr m_popMgr;

        static KUIApp()
        {
            m_uiLayer = GameObject.Find("UILayer");
            m_uiCamera = m_uiLayer.transform.FindChild("UICamera").GetComponent< Camera>();
        }

        static void Setting()
        {


        }
        static public void Setup()
        {
            if (m_isOpen) return;
            m_isOpen = true;

            __Setup();
            SetupEvent();
        }

        static public void Clear()
        {
            if (!m_isOpen) return;
            m_isOpen = false;

            ClearEvent();

            __Clear();
        }

        static void __Setup()
        {
            
            PopMgr.Setup();
        }


        static void __Clear()
        {

            PopMgr.Clear();
        }

        static void SetupEvent()
        {

        }

        static void ClearEvent()
        {

        }
        
        static public GameObject UILayer
        {
            get { return m_uiLayer; }
        }

        static public Camera UICamera
        {
            get { return m_uiCamera; }
        }


        static public KUIPopMgr PopMgr
        {
            set { if (m_popMgr == null) m_popMgr = value; }
            get
            {
                if (m_popMgr == null)
                    m_popMgr = new KUIPopMgr();
                return m_popMgr;
            }
        }


    }
}