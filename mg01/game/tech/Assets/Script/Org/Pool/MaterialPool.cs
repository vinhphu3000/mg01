/* ==============================================================================
 * 材质池
 * @author jr.zeng
 * 2016/11/7 15:33:12
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

public class MaterialPool
{

    string m_shaderName;
    //原始材质
    Material m_bsMaterial = null;

    Stack<Material> m_objArr = new Stack<Material>();

    public MaterialPool(string shaderName_)
    {
        m_shaderName = shaderName_;

    }

    public Material BSMaterial
    {
        get
        {
            if(m_bsMaterial == null)
                m_bsMaterial = new Material(Shader.Find(m_shaderName));
            return m_bsMaterial;
        }
    }

    public Material GetMaterial()
    {
        Material obj = null;
        if (m_objArr.Count > 0)
        {
            obj = m_objArr.Pop();
        }

        if (obj != null)
            return obj;

        obj = GameObject.Instantiate<Material>(BSMaterial);

        return obj;
    }


    //回收对象
    public void Recycle(Material obj_)
    {
        obj_.CopyPropertiesFromMaterial(BSMaterial);
        m_objArr.Push(obj_);
    }

    public int RemainCount
    {
        get { return m_objArr.Count; }
    }


    //清空对象池
    public void Clear()
    {
        m_objArr.Clear();
    }

}
