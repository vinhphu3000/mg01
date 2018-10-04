/* ==============================================================================
 * DontDestroyOnLoad
 * @author jr.zeng
 * 2017/6/7 16:27:30
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using UnityEngine;
using Object = UnityEngine.Object;

namespace mg.org
{


    public class DontDestroyOnLoad : MonoBehaviour
    {
        //public bool ignoreRestart = false;
        void Awake()
        {
            //if (!ignoreRestart)
            //{
            //    ResMgr.AddImmortalObject(gameObject);
            //}
            GameObjUtil.DontDestroyOnLoad(gameObject);
            Destroy(this);
        }
    }

}
