/* ==============================================================================
 * LoadingView1
 * @author jr.zeng
 * 2017/8/8 11:15:45
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using UnityEngine;
using Object = UnityEngine.Object;

using mg.org;
using mg.org.KUI;

public class LoadingView1 : KUIPop
{
    KText m_txtPercent;
    KText m_txtTips;

    KProgressBar m_barLoading;


    AbstractLoader m_loader;

    float m_curPer = 0;
    float m_tmpPer = 0;
    float m_perStep = 0.01f;

    bool m_isComplete = false;


    public LoadingView1()
    {

        m_popId = POP_ID.LOADING_1;
        m_layerIdx = POP_LAYER_IDX.LAYER_LOADING;
        m_lifeType = POP_LIFE.FOREVER;

        ShowGameObject();
        
        m_barLoading = GetChildByName<KProgressBar>("ProgressBar_Load", true);
        m_barLoading.value = 0;

        m_txtPercent = GetChildByName<KText>("Label_Percent", true);
        m_txtTips = GetChildByName<KText>("Label_Tips", true);

    }

    protected override void __Show(object showObj_, params object[] params_)
    {
        base.__Show(showObj_);

        m_loader = showObj_ as AbstractLoader;

        m_isComplete = false;
        m_curPer = 0;
        m_tmpPer = 0;

        RefreshPercent();
        UpdateBar(m_tmpPer);

        m_txtTips.text = "欢迎来到新世界~~";
    }

    //更新实际进度
    protected void RefreshPercent()
    {
        SetPercent(m_loader.Progress);
    }

    void SetPercent(float progress_)
    {
        m_curPer = progress_;
        if (m_curPer < m_tmpPer)
        {
            //进度减小了。。
            m_tmpPer = m_curPer - 0.01f;
        }

    }


    //更新进度条
    void UpdateBar(float percent_)
    {

        m_barLoading.value = percent_;
        m_txtPercent.text = (int)(percent_ * 100) + "%";
        
    }


    //加载完成
    void Step(float dt_)
    {
        if (m_tmpPer < m_curPer)
        {
            m_tmpPer += m_perStep;
            if (m_tmpPer > m_curPer)
                m_tmpPer = m_curPer;
            UpdateBar(m_tmpPer);
        }
        else
        {

            if (m_isComplete)
            {
                //通知完成
                NotifyComplete();
            }
        }
    }



    void NotifyComplete()
    {
        NotifyWithEvent(LOAD_EVT.VIEW_COMPLETE);
    }


    void OnLoadComplete()
    {
        m_isComplete = true;

    }

    void onLoadEvt(object evt_)
    {
        SubjectEvent evt = evt_ as SubjectEvent;
        switch (evt.type)
        {

            case LOAD_EVT.PROGRESS:
                //进度更新
                RefreshPercent();

                break;
            case LOAD_EVT.COMPLETE:
                //全部完成
                OnLoadComplete();

                break;
        }

    }

    

    protected override void SetupEvent()
    {
        m_loader.Attach(LOAD_EVT.PROGRESS, onLoadEvt, null);
        m_loader.Attach(LOAD_EVT.COMPLETE, onLoadEvt, null);

        CCApp.SchUpdate(Step);
    }

    protected override void ClearEvent()
    {
        m_loader.Detach(LOAD_EVT.PROGRESS, onLoadEvt);
        m_loader.Detach(LOAD_EVT.COMPLETE, onLoadEvt);

        CCApp.UnschUpdate(Step);
    }

    protected override void __Destroy()
    {

        m_loader = null;
    }


}