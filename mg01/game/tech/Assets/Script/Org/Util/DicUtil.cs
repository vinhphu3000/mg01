/* ==============================================================================
 * 字典工具
 * @author jr.zeng
 * 2016/6/8 10:40:49
 * ==============================================================================*/

using System;
using System.Collections;
using System.Collections.Generic;


namespace mg.org
{
    public class DicUtil
    {

        /// <summary>
        /// 转为值数组
        /// </summary>
        /// <typeparam name="K"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="dic_"></param>
        /// <returns></returns>
        static public V[] ToValues<K, V>(Dictionary<K, V> dic_)
        {
            V[] result = new V[dic_.Count];
            if (dic_.Count > 0)
            {
                int i = 0;
                foreach (KeyValuePair<K, V> kvp in dic_)
                {
                    result[i] = kvp.Value;
                    i++;
                }
            }
            return result;
        }

        /// <summary>
        /// 转为键数组
        /// </summary>
        /// <typeparam name="K"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="dic_"></param>
        /// <returns></returns>
        static public K[] ToKeys<K, V>(Dictionary<K, V> dic_)
        {
            K[] result = new K[dic_.Count];
            if (dic_.Count > 0)
            {
                int i = 0;
                foreach (KeyValuePair<K, V> kvp in dic_)
                {
                    result[i] = kvp.Key;
                    i++;
                }
            }
            return result;
        }

        /// <summary>
        /// 根据键来移除
        /// </summary>
        /// <typeparam name="K"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="dic_"></param>
        /// <param name="values"></param>
        static public bool RemoveByKeys<K, V>(Dictionary<K, V> dic_, List<K> keys_)
        {
            bool b = false;
            foreach (var key in keys_)
            {
                if (dic_.Remove(key))
                    b = true;
            }
            return b;
        }

    }
}