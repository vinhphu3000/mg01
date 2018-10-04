/* ==============================================================================
 * 即时动作
 * @author jr.zeng
 * 2016/10/28 11:25:00
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace mg.org.Actions
{

    public class ActionInstant : ActionBase
    {
        public ActionInstant()
        {

        }

        public void Init()
        {
            m_inited = true;
        }

        public override void Step(float dt_)
        {
            Progress(1);
        }


        public override void Progress(float progress_)
        {
            if (progress_ >= 1)
            {
                Done();
            }
        }
        

    }


}