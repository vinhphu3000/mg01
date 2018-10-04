/* ==============================================================================
 * 由鼠标控制第三人称镜头
 * @author jr.zeng
 * 2017/8/10 20:12:13
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using UnityEngine;
using Object = UnityEngine.Object;


namespace mg.org
{
    public class CameraTPMouse : MonoBehaviour
    {

        CameraTPerson m_camera;
        
        Mouse m_mouse = new Mouse(Mouse.MouseFlag.evt_move| Mouse.MouseFlag.evt_wheel);


        void Awake()
        {
            m_camera = GetComponent<CameraTPerson>();
        }


        void OnEnable()
        {
            SetupEvent();
            m_mouse.Setup();
        }


        void OnDisable()
        {
            ClearEvent();
            m_mouse.Clear();
        }
        

        void SetupEvent()
        {
            m_mouse.Attach(MOUSE_EVENT.MOVE, OnMouseMove, null);
            m_mouse.Attach(MOUSE_EVENT.WHEEL, OnMouseWheel, null);
        }

        void ClearEvent()
        {
            m_mouse.Detach(MOUSE_EVENT.MOVE, OnMouseMove);
            m_mouse.Detach(MOUSE_EVENT.WHEEL, OnMouseWheel);

        }

        //-------∽-★-∽------∽-★-∽--------∽-★-∽数据管理∽-★-∽--------∽-★-∽------∽-★-∽--------//

        void OnMouseMove(object evt_)
        {
            if (!m_mouse.IsPressed(MouseKey.MOUSE_RIGHT))
                return;

            SubjectEvent evt = evt_ as SubjectEvent;
            Vector2 axis = (Vector2)evt.data;

            float axisX = m_camera.axisX;
            float axisY = m_camera.axisY;

            float axisXTo = axisX - axis.y * Time.deltaTime * 500;
            float axisYTo = axisY + axis.x * Time.deltaTime * 500;

            m_camera.SetAxisXY(axisXTo, axisYTo, true);
        }

        void OnMouseWheel(object evt_)
        {
            SubjectEvent evt = evt_ as SubjectEvent;
            float axis = -(float)evt.data;

            float dis = m_camera.distance;
            float disTo = dis + axis * Time.deltaTime * 500;
            m_camera.SetDistance(disTo);
        }

        //-------∽-★-∽------∽-★-∽--------∽-★-∽数据操作∽-★-∽--------∽-★-∽------∽-★-∽--------//
        



    }

}