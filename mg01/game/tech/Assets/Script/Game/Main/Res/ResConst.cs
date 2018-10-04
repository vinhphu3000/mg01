/* ==============================================================================
 * ResConst
 * @author jr.zeng
 * 2016/10/5 11:43:25
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using mg.org;

public class ResConst
{
   
}



public class RES_ID
{

    //技能图标
    public const string SKILL_ICON = "skill_icon";


    public static ResCfgVo[] RES_REG = new ResCfgVo[]
    {

        //技能图标
        new ResCfgVo(RES_ID.SKILL_ICON,  "NUI/Icon/Skill/", ResSuffix.PNG, ResType.IMAGE, ResLocation.RSS),
        
    };
}

