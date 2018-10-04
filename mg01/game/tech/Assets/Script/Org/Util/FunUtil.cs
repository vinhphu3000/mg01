/* ==============================================================================
 * CCFunDefine
 * @author jr.zeng
 * 2016/8/23 14:54:12
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Reflection;

using UnityEngine;

namespace mg.org
{


    //-------∽-★-∽------∽-★-∽--------∽-★-∽delegate∽-★-∽--------∽-★-∽------∽-★-∽--------//

    //回调函数_0个参数
    public delegate void CALLBACK_0();
    public delegate void CALLBACK_1(object value_);

    public delegate void CALLBACK_Float(float value_);

    public delegate void CALLBACK_Objs(object[] objs_);
    //加载器回调
    public delegate void CALLBACK_LoadReq(LoadReq req_);
    public delegate void CALLBACK_AssetData(AssetData data_);

    public delegate void CALLBACK_GO(GameObject go_);

    //观察者回调


    //-------∽-★-∽------∽-★-∽--------∽-★-∽FunUtil∽-★-∽--------∽-★-∽------∽-★-∽--------//

    public class FunUtil
    {






    }
}