/* ==============================================================================
 * 引用者
 * @author jr.zeng
 * 2017/5/31 16:24:53
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using UnityEngine;
using Object = UnityEngine.Object;

public interface IRefer
{
    string ReferId { get; }
    
    void NotifyDeactive();
    void NotifyDispose();

}