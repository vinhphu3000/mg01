/* ==============================================================================
 * JsonUtil
 * @author jr.zeng
 * 2017/12/4 15:19:01
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using LitJson;

using UnityEngine;
using Object = UnityEngine.Object;

namespace Edit
{
    public class JsonUtil
    {
        /// <summary>
        /// 将对象保存为json
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data_"></param>
        /// <param name="path_"></param>
        public static void WriteToJson(object data_, string path_, bool isPretty = false)
        {
            TextWriter tw = new StreamWriter(path_);
            string jsonStr = JsonMapper.ToJson(data_);

            if (isPretty)
                jsonStr = JsonPrettyPrint.Format(jsonStr);
            
            tw.Write(jsonStr);
            tw.Flush();
            tw.Close();
        }

       
    }


}