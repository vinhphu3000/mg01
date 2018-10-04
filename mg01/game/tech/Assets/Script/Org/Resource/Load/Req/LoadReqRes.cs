/* ==============================================================================
 * 资源请求
 * @author jr.zeng
 * 2017/5/20 14:21:44
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mg.org
{

    public class LoadReqRes : LoadReq
    {
        //资源id
        public string res_id;
        //资源名称
        public string res_name = null;

        string m_url;

        public LoadReqRes()
        {

        }
        
        public LoadReqRes(string resId_, string resName_)
        {
            Init(resId_, resName_);
        }


        public void Init(string resId_, string resName_)
        {
            res_id = resId_;
            res_name = resName_;

            ResCfgVo cfg = ResConfig.GetResCfg(resId_);
            m_url = ResConfig.GetResUrl(res_id, res_name, "");

        }


        public override string url
        {
            get { return m_url; }
        }
        


    }

}