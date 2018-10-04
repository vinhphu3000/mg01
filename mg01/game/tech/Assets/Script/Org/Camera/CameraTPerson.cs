/* ==============================================================================
 * 镜头_第三人称
 * @author jr.zeng
 * 2017/8/10 15:41:17
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using UnityEngine;
using Object = UnityEngine.Object;


namespace mg.org
{
    public class CameraTPerson : CameraBase
    {
        static Vector3 tmp_vec3 = Vector3.zero;

        //跟随的目标
        [SerializeField]
        protected Transform m_target;


        protected Vector3 m_tarOffset = new Vector3(0, 3, 0);   //跟随的坐标偏移

        protected Vector3 m_tarPosTo = new Vector3();
        protected Vector3 m_tarPosReal = new Vector3();


        protected float m_disDefault = 30;    //默认相机与目标的距离
        protected float m_axisXDefault = 45;  //默认相机与目标的X轴夹角
        protected float m_axisYDefault = 30;   //默认相机与目标的Y轴夹角

        protected float m_disReal;
        protected float m_axisXReal;
        protected float m_axisYReal;

        protected float m_disTo;
        protected float m_axisXTo;
        protected float m_axisYTo;

        protected Limitf m_axisXLimit = new Limitf(0, 89.99f);
        protected Limitf m_axisYLimit = new Limitf(0, 0);
        protected Limitf m_disLimit = new Limitf(1, 50);

        //缓动系数
        protected float m_easeFactor = 0.2f;

        protected override void Awake()
        {
            base.Awake();

        }

        protected override void Start()
        {
            base.Start();

            ResetToDefaultLocation();

        }


        protected virtual void Update()
        {
            SetTarPos(m_target.position, false);
            
            UpdateEase();
            UpdateCamersPos();
        }


        //-------∽-★-∽------∽-★-∽--------∽-★-∽数据操作∽-★-∽--------∽-★-∽------∽-★-∽--------//


        /// <summary>
        /// 设置默认参数
        /// </summary>
        /// <param name="dis_"></param>
        /// <param name="axisX_"></param>
        /// <param name="axisY_"></param>
        public void SetDefault(float dis_, float axisX_, float axisY_ = 0)
        {
            m_disDefault = dis_;
            m_axisXDefault = axisX_;
            m_axisYDefault = axisY_;

        }


        /// <summary>
        /// 重置相机到默认位置
        /// </summary>
        public void ResetToDefaultLocation()
        {
            m_disReal = m_disDefault;
            m_axisXReal = m_axisXDefault;
            m_axisYReal = m_axisYDefault;

            m_disTo = m_disDefault;
            m_axisXTo = m_axisXDefault;
            m_axisYTo = m_axisYDefault;

            SetTarPos(m_target.position, false);

            UpdateCamersPos();
        }



        //-------∽-★-∽------∽-★-∽镜头位置∽-★-∽------∽-★-∽--------//

        /// <summary>
        /// 目标偏移
        /// </summary>
        public Vector3 tarOffset
        {
            get { return m_tarOffset; }
            set { m_tarOffset = value; }
        }

        
        void SetTarPos(Vector3 pos_, bool ease_ = true)        
        {
            m_tarPosTo = pos_;

            if (!ease_)
            {
                m_tarPosReal = m_tarPosTo;
            }
        }

        /// <summary>
        /// 计算相机位置
        /// </summary>
        /// <param name="tarPos">目标位置</param>
        /// <param name="xAxis">仰角</param>
        /// <param name="yAxis">平角</param>
        /// <param name="distance">与目标的距离</param>
        /// <returns></returns>
        protected Vector3 CalcCameraPos(Vector3 tarPos, float xAxis, float yAxis, float distance)
        {
            tmp_vec3.Set(0, 0, -distance);
            Quaternion rotation = Quaternion.Euler(xAxis, yAxis, 0);
            return tarPos + rotation * tmp_vec3 + m_tarOffset;
        }

        //更新镜头坐标
        protected void UpdateCamersPos()
        {
            Vector3 cam_pos = CalcCameraPos(m_tarPosReal, m_axisXReal, m_axisYReal, m_disReal);

            transform.position = cam_pos;
            transform.LookAt(m_tarPosReal + m_tarOffset);
        }


        //-------∽-★-∽------∽-★-∽镜头目标∽-★-∽------∽-★-∽--------//

        public Transform target
        {
            get { return m_target; }

        }
        public void SetTarget(Transform target_)
        {
            if (m_target == target_)
                return;
            m_target = target_;
        }

        //-------∽-★-∽------∽-★-∽镜头距离∽-★-∽------∽-★-∽--------//

        /// <summary>
        /// 设置镜头距离
        /// </summary>
        /// <param name="dis_"></param>
        /// <param name="ease_"></param>
        public void SetDistance(float value_, bool ease_ = true)
        {
            m_disTo = m_disLimit.ClampNotZero(value_);

            if (!ease_)
            {
                m_disReal = m_disTo;
            }

        }

        /// <summary>
        /// 设置距离阈值
        /// </summary>
        /// <param name="min_"></param>
        /// <param name="max_"></param>
        public void SetDisLimit(float min_, float max_)
        {
            m_disLimit.min = min_;
            m_disLimit.max = min_;
        }


        public float distance
        {
            get { return m_disTo; }
        }

        //-------∽-★-∽------∽-★-∽轴方向∽-★-∽------∽-★-∽--------//


        public float axisX
        {
            get { return m_axisXTo; }
        }

        public float axisY
        {
            get { return m_axisYTo; }
        }


        /// <summary>
        /// 设置仰角
        /// </summary>
        /// <param name="value_"></param>
        /// <param name="ease_"></param>
        public void SetAxisX(float value_, bool ease_ = true)
        {
            m_axisXTo = m_axisXLimit.ClampNotZero(value_);

            if (!ease_)
            {
                m_axisXReal = m_axisXTo;
            }
        }

        /// <summary>
        /// 设置平角
        /// </summary>
        /// <param name="value_"></param>
        /// <param name="ease_"></param>
        public void SetAxisY(float value_, bool ease_ = true)
        {
            m_axisYTo = m_axisYLimit.ClampNotZero(value_);

            if (!ease_)
            {
                m_axisYReal = m_axisYTo;
            }
        }


        public void SetAxisXY(float valueX_, float valueY_, bool ease_ = true)
        {
            m_axisXTo = m_axisXLimit.ClampNotZero(valueX_);
            m_axisYTo = m_axisYLimit.ClampNotZero(valueY_);

            //Log.Debug("axisX " + m_axisXTo + " axisY " + m_axisYTo);

            if (!ease_)
            {
                m_axisXReal = m_axisXTo;
                m_axisYReal = m_axisYTo;
            }
        }

        /// <summary>
        /// 设置仰角阈值
        /// </summary>
        /// <param name="min_"></param>
        /// <param name="max_"></param>
        public void SetAxisXLimit(float min_, float max_)
        {
            m_axisXLimit.min = min_;
            m_axisXLimit.max = min_;
        }

        /// <summary>
        /// 设置平角阈值
        /// </summary>
        /// <param name="min_"></param>
        /// <param name="max_"></param>
        public void SetAxisYLimit(float min_, float max_)
        {
            m_axisYLimit.min = min_;
            m_axisYLimit.max = min_;
        }


        //-------∽-★-∽------∽-★-∽缓动相关∽-★-∽------∽-★-∽--------//

        /// <summary>
        /// 混动系数
        /// </summary>
        public float easeFactor
        {
            get { return m_easeFactor; }
            set { m_easeFactor = value; }
        }

        //更新缓动
        void UpdateEase()
        {
            if(m_tarPosTo != m_tarPosReal)
            {
                m_tarPosReal += (m_tarPosTo - m_tarPosReal) * 0.1f;
            }
            
            if (m_disTo != m_disReal)
            {
                m_disReal += (m_disTo - m_disReal) * m_easeFactor;
                if (Mathf.Abs(m_disTo - m_disReal) < 0.1)
                    m_disReal = m_disTo;
            }

            if (m_axisXTo != m_axisXReal)
            {
                m_axisXReal += (m_axisXTo - m_axisXReal) * m_easeFactor;
                if (Mathf.Abs(m_axisXTo - m_axisXReal) < 0.1)
                    m_axisXReal = m_axisXTo;
            }

            if (m_axisYTo != m_axisYReal)
            {
                m_axisYReal += (m_axisYTo - m_axisYReal) * m_easeFactor;
                if (Mathf.Abs(m_axisYTo - m_axisYReal) < 0.1)
                    m_axisYReal = m_axisYTo;
            }

        }


    }


}