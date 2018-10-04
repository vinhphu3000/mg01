/* ==============================================================================
 * StateText
 * @author jr.zeng
 * 2017/8/1 17:53:30
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using UnityEngine;
using Object = UnityEngine.Object;

namespace mg.org.KUI
{
    public class StateText : StateChangeable
    {
        public KText CurTextField
        {
            get
            {
                return GetCurStateComponent<KText>();
            }
        }

        public void ChangeAllStateText(string text)
        {
            for (int i = 0; i < this.StateCount; i++)
            {
                GetStateComponent<KText>(i).text = text;
            }
        }

        public void ChangeAllStateTextAlignment(TextAnchor anchor)
        {
            for (int i = 0; i < this.StateCount; i++)
            {
                GetStateComponent<KText>(i).alignment = anchor;
            }
        }

        public void Clear()
        {
            ChangeAllStateText(string.Empty);
        }

    }

}