/* ==============================================================================
 * AllocUtil
 * @author jr.zeng
 * 2017/6/19 14:37:02
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using UnityEngine;
using Object = UnityEngine.Object;

namespace mg.org
{

    public class AllocUtil
    {

        //分配对象id
        static int __allocObjId = 1;
        public static int GetObjId()
        {
            return __allocObjId++;
        }

        //分配事件id
        static int __allocEvtId = 1;
        public static int getEvtId()
        {
            return __allocEvtId++;
        }

    }

}