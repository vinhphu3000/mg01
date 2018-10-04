/* ==============================================================================
 * StringUtil
 * @author jr.zeng
 * 2016/11/24 15:51:23
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace mg.org
{

    public class StringUtil
    {

        
        /// <summary>
        /// 截断到首个指定字符
        /// </summary>
        /// <param name="str_"></param>
        /// <param name="char_"></param>
        /// <returns></returns>
        public static string SubToFirst(string str_, string char_)
        {
            int index = str_.IndexOf(char_);
            if (index >= 0)
                str_ = str_.Substring(0, index);
            return str_;
        } 

    }


}