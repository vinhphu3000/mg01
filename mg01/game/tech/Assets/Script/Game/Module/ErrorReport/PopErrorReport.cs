/* ==============================================================================
 * PopErrorReport
 * @author jr.zeng
 * 2017/9/19 11:52:00
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using UnityEngine;
using Object = UnityEngine.Object;

using mg.org;
using mg.org.KUI;

public class PopErrorReport : KUIPop
{
    KButton m_btn;
    KText m_text;

    int m_clickCnt = 0;
    int m_clickCntMax = 3;  //关闭需要的点击次数

    public PopErrorReport()
    {
        m_popId = POP_ID.ERROR_REPORT;
        m_layerIdx = POP_LAYER_IDX.LAYER_TOP;

        ShowGameObject();


        m_btn = GetChildByName<KButton>("Container_ErrorReport/Container_Panel/Button_Center");
        m_text = GameObjUtil.FindChlid<KText>(m_btn.gameObject, "Label_Text");

    }


    protected override void __Show(object showObj_, params object[] params_)
    {

        string str = (string)showObj_;
        m_text.text = str;

        m_clickCnt = 0;
    }

    void OnClickBtn(KButton btn_)
    {
        m_clickCnt++;
        if (m_clickCnt >= m_clickCntMax)
        {
            Close();
        }
        Log.Debug("OnClickBtn " + m_clickCnt, this);
    }


    protected override void SetupEvent()
    {
        m_btn.onClick.AddListener(OnClickBtn);
    }

    protected override void ClearEvent()
    {
        m_btn.onClick.RemoveListener(OnClickBtn);

    }

    protected override void __Destroy()
    {
        
    }

}