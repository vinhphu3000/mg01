/* ==============================================================================
 * 配置解析器
 * @author jr.zeng
 * 2016/9/17 9:44:04
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mg.org
{


    public class DataCfgParser
    {
        public DataCfgParser()
        {

        }

        /// <summary>
        /// 解析配置文件
        /// </summary>
        /// <param name="source_"></param>
        /// <param name="dataTp_"></param>
        /// <returns></returns>
        virtual public object[] Parse(object source_, Type dataTp_)
        {
            return null;
        }


    }

}