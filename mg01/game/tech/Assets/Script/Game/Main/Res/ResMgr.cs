/* ==============================================================================
 * ResMgr
 * @author jr.zeng
 * 2016/10/8 17:24:35
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using mg.org;

public class ResMgr : mg.org.ResMgr
{
    public ResMgr()
    {

    }

    protected override void RegResource()
    {
        base.RegResource();

        ResConfig.AddResCfg(RES_ID.RES_REG);
    }

}
