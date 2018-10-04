/* ==============================================================================
 * IPop
 * @author jr.zeng
 * 2016/12/15 11:31:24
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;


namespace mg.org
{

    public interface IPop : IImgAbs, ISubject
    {

        void Close();

        string popID { get; set; }

        int layerIdx { get ; } 
        POP_LIFE lifeType { get; }
    }


}
