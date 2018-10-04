/* ==============================================================================
 * JsonAsset
 * @author jr.zeng
 * 2017/7/27 10:39:57
 * ==============================================================================*/

using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LitJson;

using UnityEngine;
using Object = UnityEngine.Object;

namespace Edit.PSD4UGUI
{
    public class JsonAsset 
    {

        string m_name;
        public string name  { get { return m_name; } }

        string m_text = null;
        public string text {  get { return m_text; }  }
        
        public JsonAsset(string path_)
        {
            m_name = mg.org.FileUtility.GetNameFromFullPath(path_, "");

            if (File.Exists(path_))
            {
                StreamReader reader = File.OpenText(path_);
                m_text = reader.ReadToEnd();
            }
        }


        public JsonData GetJsonData()
        {
            if (m_text != null)
            {
                return JsonMapper.ToObject(m_text);
            }
            return null;
        }

    }

}