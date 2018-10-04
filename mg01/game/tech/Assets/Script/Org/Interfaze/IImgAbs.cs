/* ==============================================================================
 * IImgAbs
 * @author jr.zeng
 * 2016/9/19 10:59:36
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace mg.org
{
    public interface IImgAbs : IRef
    {

        void Show();
        void Show(object showObj_, params object[] params_);
        bool isOpen { get; }
        GameObject gameObject { get; }
        void Destroy();
        void DestroyRemove();
    }
}
