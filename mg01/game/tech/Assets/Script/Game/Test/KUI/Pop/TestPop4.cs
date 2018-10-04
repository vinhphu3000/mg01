/* ==============================================================================
 * TestPop4
 * @author jr.zeng
 * 2018/9/8 11:37:01
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using UnityEngine;
using Object = UnityEngine.Object;

using mg.org;
using mg.org.KUI;


public class TestPop4 : KUIPop
{

    KButton m_btnOk;
    KButton m_btnClose;
    //
    KInputField m_inputName;
    KText m_labelTestInput;
    //
    KToggle m_tgl_1;
    KText m_labelTgl_1;
    KToggleGroup m_tglGroup_1;
    //
    KSlider m_sliderSchedule;
    KText m_labelSchedule;
    //
    KProgressBar m_barLoading;
    //
    KImage m_icon;
    //
    KScrollView m_scrollView;
    KListViewScroll m_listView;

    public TestPop4()
    {
        m_popId = POP_ID.TEST_POP_4;
        m_layerIdx = POP_LAYER_IDX.LAYER_POP_1;

        ShowGameObject();

        //
        m_btnOk = GetChildByName<KButton>("Button_Ok", true);
        m_btnClose = GetChildByName<KButton>("Button_Close", true);
        //
        m_inputName = GetChildByName<KInputField>("Input_name", true);
        m_labelTestInput = GetChildByName<KText>("Label_testInput", true);
        //
        m_tgl_1 = GetChildByName<KToggle>("Toggle_position1", true);
        m_labelTgl_1 = GetChildByName<KText>(m_tgl_1.gameObject, "Label_Text", true);
        m_tglGroup_1 = GetChildByName<KToggleGroup>("ToggleGroup_a1", true);
        //
        m_sliderSchedule = GetChildByName<KSlider>("Slider_Schedule1", true);
        m_labelSchedule = GetChildByName<KText>("Label_testSilder", true);
        //
        m_barLoading = GetChildByName<KProgressBar>("ProgressBar_loading", true);
        //
        m_icon = GetChildByName<KImage>("Image_sharedAnchor", true);
        //
        m_scrollView = GetChildByName<KScrollView>("ScrollView_GuildList", true);
        m_listView = ComponentUtil.EnsureComponent<KListViewScroll>(m_scrollView.gameObject);
    }

    protected override void __Show(object showObj_, params object[] params_)
    {
        ShowButton();
        ShowText();
        ShowToggle();
        ShowSilder();
        ShowProBar();
        ShowImage();
        ShowScrollView();
        ShowListView();

    }

    protected override void SetupEvent()
    {
        CCApp.keyboard.Attach(KEY_EVENT.RELEASE, OnKeyPress, this);

    }

    protected override void ClearEvent()
    {
        CCApp.keyboard.Detach(KEY_EVENT.RELEASE, OnKeyPress);

    }
    protected override void __Destroy()
    {
        ClearButton();
        ClearText();
        ClearToggle();
        ClearSilder();
        ClearProBar();
        ClearImage();
        ClearScrollView();
        ClearListView();

        CCApp.soundMgr.StopBgm();
    }

    void OnKeyPress(object evt_)
    {
        KeyCode key = (KeyCode)evt_;
        switch (key)
        {
            case KeyCode.Keypad1:
                m_listView.JumpToIndex(11, KListViewScroll.JumpPosType.BOTTOM);
                break;
            case KeyCode.Keypad2:
                m_listView.JumpToIndex(5, KListViewScroll.JumpPosType.CENTER);
                break;
            case KeyCode.Keypad3:
                m_listView.JumpToIndex(14, KListViewScroll.JumpPosType.TOP);
                break;
            default:
                break;
        }

    }

    
    //-------~★~-------~★~-------~★~测试按钮~★~-------~★~-------~★~-------//

    void ShowButton()
    {
        m_btnOk.onClick.AddListener(OnClickBtn);
        m_btnClose.onClick.AddListener(OnClickBtn);
    }


    void ClearButton()
    {
        m_btnOk.onClick.RemoveListener(OnClickBtn);
        m_btnClose.onClick.RemoveListener(OnClickBtn);
    }

    void OnClickBtn(KButton btn_)
    {
        if (btn_ == m_btnOk)
        {
            Log.Debug("Click OK", this);
        }
        else if(btn_ == m_btnClose)
        {
            Close();
        }

    }


    //-------~★~-------~★~-------~★~测试文本~★~-------~★~-------~★~-------//
    
    void ShowText()
    {
        m_inputName.onValueChanged.AddListener(OnInputValue);
        m_inputName.onEndEdit.AddListener(OnInputEndEdit);
    }


    void ClearText()
    {

        m_inputName.onValueChanged.RemoveListener(OnInputValue);
        m_inputName.onEndEdit.RemoveListener(OnInputEndEdit);
    }

    void OnInputValue(string str_)
    {
        m_labelTestInput.text = "编辑中: " + str_;
    }

    void OnInputEndEdit(string str_)
    {
        m_labelTestInput.text = "编辑完成: " + str_;
    }


    //-------~★~-------~★~-------~★~测试单/多选框~★~-------~★~-------~★~-------//

    void ShowToggle()
    {
        m_tgl_1.onValueChanged.AddListener(OnToggleValue);

        m_tgl_1.needReqChange = true;   //需要请求改变
        m_tgl_1.onReqChange.AddListener(OnToggleReq);

        m_tglGroup_1.allowMultiple = true;  //多选
        m_tglGroup_1.onValueChange.AddListener(OnTglGroupValue);
    }
    
    void ClearToggle()
    {
        m_tgl_1.onValueChanged.RemoveListener(OnToggleValue);
        m_tgl_1.onReqChange.RemoveListener(OnToggleReq);

        m_tglGroup_1.onValueChange.RemoveListener(OnTglGroupValue);
    }


    void OnToggleValue(KToggle tgl_, bool b_)
    {
        m_labelTgl_1.text = b_ ? "选中" : "没选中";
    }

    void OnToggleReq(KToggle tgl_, bool b_)
    {
        m_tgl_1.Select(b_);
    }


    void OnTglGroupValue(KToggleGroup tglGroup_, int idx_, bool b_)
    {
        KToggle child = tglGroup_.GetToggle(idx_);
        KText label = GetChildByName<KText>(child.gameObject, "Label_Text");

        label.text = b_ ? "选中" : "没选中";
    }


    //-------~★~-------~★~-------~★~测试滑块~★~-------~★~-------~★~-------//

    void ShowSilder()
    {
        m_sliderSchedule.onValueChanged.AddListener(OnSliderValue);
        m_sliderSchedule.value = 0.5f;

    }
    

    void ClearSilder()
    {
        m_sliderSchedule.onValueChanged.RemoveListener(OnSliderValue);

    }


    void OnSliderValue(KSlider tgl_, float value_)
    {
        m_labelSchedule.text = value_.ToString();
    }


    //-------~★~-------~★~-------~★~测试进度条~★~-------~★~-------~★~-------//

    void ShowProBar()
    {
        m_barLoading.value = 0.8f;
    }
    

    void ClearProBar()
    {

    }



    //-------~★~-------~★~-------~★~测试图片~★~-------~★~-------~★~-------//

    void ShowImage()
    {
        SprAtlasCache.me.LoadSprite(this, m_icon, "npc1032", "npc1032", true);
    }



    void ClearImage()
    {

    }


    //-------~★~-------~★~-------~★~测试ScrollView~★~-------~★~-------~★~-------//

    void ShowScrollView()
    {

    }



    void ClearScrollView()
    {

    }


    //-------~★~-------~★~-------~★~测试ListView~★~-------~★~-------~★~-------//

    void ShowListView()
    {
        LayoutParam layoutParam = m_listView.layoutParam;
        //layoutParam.dir = LayoutDirection.LeftToRight;
        //layoutParam.divNum = 2;
        layoutParam.origin = new Vector2(0, -200f);
        layoutParam.itemGap = new Vector2(0, 50f);
        //m_listView.direction = KScrollView.ScrollDir.horizontal;
        m_listView.onDataChanged.AddListener(UpdateListItem);

        //int len = MathUtil.RandomInt(10, 30);
        int len = 200;
        m_listView.ShowLen(len, 100);
    }



    void ClearListView()
    {
        m_listView.ClearList();
        m_listView.onDataChanged.RemoveListener(UpdateListItem);

    }

    void UpdateListItem(GameObject item_, int index_, object data_)
    {
        GameObject select = GetChildByName(item_, "Image_bg/select");
        GameObject normal = GetChildByName(item_, "Image_bg/normal1");
        KText labelName = GetChildByName<KText>(item_, "Label_Name");
        KText labelWinNum = GetChildByName<KText>(item_, "Label_WinNum");
        KText labelMemberNum = GetChildByName<KText>(item_, "Label_MemberNum");

        bool b = (index_ % 2) == 0;
        normal.SetActive(b);
        select.SetActive(!b);

        //index_ += 1;
        labelName.text = "路人甲" + index_;
        labelWinNum.text = index_.ToString();
        labelWinNum.text = 999 + "人";
    }
    

}