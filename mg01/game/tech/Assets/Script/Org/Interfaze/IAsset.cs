/* ==============================================================================
 * IAsset
 * @author jr.zeng
 * 2017/10/31 19:20:24
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using UnityEngine;
using Object = UnityEngine.Object;


namespace mg.org
{
    public interface IAsset
    {
        void OnLoaded();
        void Unload();
    }

}

