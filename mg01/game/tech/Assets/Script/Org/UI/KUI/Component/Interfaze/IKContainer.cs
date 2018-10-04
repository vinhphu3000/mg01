/* ==============================================================================
 * IKContainer
 * @author jr.zeng
 * 2017/6/16 14:37:04
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using UnityEngine;
using Object = UnityEngine.Object;

namespace mg.org.KUI
{

    public interface IKContainer
    {

        GameObject GetChild(string path);
        T GetChildComponent<T>(string path) where T : Component;
        T AddChildComponent<T>(string path) where T : Component;
        bool Visible { get; set; }

    }

}