using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using UnityEngine;
using mg.org.KUI;

using Object = UnityEngine.Object;


namespace Edit.PSD4UGUI
{

   

    public abstract class ComponentBuilder
    {

        static List<BuildHelper> tmpHelpers = new List<BuildHelper>();

        protected List<string> _paramList = new List<string>();

        public ComponentBuilder()
        {

        }


        public abstract string Identifier { get; }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="go"></param>
        /// <param name="interactable"></param>
        /// <param name="applyDeferred">该节点是否需要应用Deferred参数</param>
        public abstract void Build(GameObject go, bool applyDeferred = true);


        protected void RefreshParamList(GameObject go)
        {
            _paramList.Clear();
            go.GetComponents<BuildHelper>(tmpHelpers);
            for (int i = 0; i < tmpHelpers.Count; i++)
            {
                _paramList.Add(tmpHelpers[i].param);
            }
            tmpHelpers.Clear();
            //这里是不是应该删除所有BuildHelper,而不是在最后统一删除
        }

        protected bool HasParam(string param)
        {
            foreach (string s in _paramList)
            {
                if (s == param)
                {
                    return true;
                }
            }
            return false;
        }

    }

}