/* ==============================================================================
 * GUITest
 * @author jr.zeng
 * 2017/2/27 12:36:52
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using mg.org;
using mg.org.KUI;


public class KUITest : CCModule
{
    public KUITest()
    {

    }

    override protected void __Setup(params object[] params_)
    {

        
    }

    override protected void __Clear()
    {


    }


    override protected void SetupEvent()
    {

        CCApp.keyboard.Attach(KEY_EVENT.PRESS, onKeyPressed, this);
    }

    override protected void ClearEvent()
    {
        CCApp.keyboard.Detach(KEY_EVENT.PRESS, onKeyPressed);

    }


    //-------∽-★-∽------∽-★-∽--------∽-★-∽数据管理∽-★-∽--------∽-★-∽------∽-★-∽--------//

    private void onKeyPressed(object evt_)
    {
        KeyCode key = (KeyCode)evt_;
        switch (key)
        {
            case KeyCode.Alpha1:
                

                break;
            case KeyCode.Alpha2:

                //KUIApp.PopMgr.ShowOrClose(POP_ID.TEST_KUI_3);
                KUIApp.PopMgr.ShowOrClose(POP_ID.TEST_POP_4);

                break;
        }

    }


    //-------∽-★-∽------∽-★-∽--------∽-★-∽数据操作∽-★-∽--------∽-★-∽------∽-★-∽--------//


}