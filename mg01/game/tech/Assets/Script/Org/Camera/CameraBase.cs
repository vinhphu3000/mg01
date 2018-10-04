/* ==============================================================================
 * 镜头_第一人称
 * @author jr.zeng
 * 2017/8/10 16:03:39
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using UnityEngine;
using Object = UnityEngine.Object;

namespace mg.org
{
    public class CameraBase : MonoBehaviour
    {

        protected Camera m_camera;

        protected virtual void Awake()
        {
            m_camera = GetComponent<Camera>();

        }


        protected virtual void Start()
        {


        }
        protected virtual void OnEnable()
        {


        }

        protected virtual void OnDisable()
        {


        }

        protected virtual void OnDestroy()
        {

        }

    }


}