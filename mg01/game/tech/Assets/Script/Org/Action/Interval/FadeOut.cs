/* ==============================================================================
 * FadeOut
 * @author jr.zeng
 * 2016/11/1 17:03:54
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace mg.org.Actions
{


    public class FadeOut : FadeTo
    {
        static public FadeIn Create(float duration_)
        {
            FadeIn action = new FadeIn();
            action.InitWithAlpha(duration_, 0);

            return action;
        }
    }

}