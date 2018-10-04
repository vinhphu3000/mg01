/* ==============================================================================
 * IRef
 * @author jr.zeng
 * 2016/12/19 10:11:04
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



namespace mg.org
{
    public interface IRef 
    {

        bool Retain(object refer_);
        bool Release(object refer_);

        int RefCount { get; }

    }
}
