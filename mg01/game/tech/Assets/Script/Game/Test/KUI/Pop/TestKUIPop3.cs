/* ==============================================================================
 * TestKUIPop3
 * @author jr.zeng
 * 2017/8/26 11:56:40
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using UnityEngine;
using Object = UnityEngine.Object;

using mg.org;
using mg.org.KUI;

public class TestKUIPop3 : KUIPop
{

    KToggle m_toggle1;
    KToggleGroup m_toggleGroup1;
    //--
    KInputField m_inputName;
    //--
    KButton m_buttonAnnounce;
    KButton m_buttonClose;
    //--
    KProgressBar m_barExp;
    //--
    KSlider m_sliderSchedule1;
    //--
    KScrollView m_scrollViewItemList;


    Dictionary<int, GameObject> m_idx2item = new Dictionary<int, GameObject>();

    KListView m_listView;
    KListViewScroll m_listViewScroll;

    KImage m_icon1;

    public TestKUIPop3()
    {
        m_popId = POP_ID.TEST_KUI_3;
        m_layerIdx = POP_LAYER_IDX.LAYER_POP_1;

        ShowGameObject();

        m_toggle1 = GetChildByName<KToggle>("Toggle_position1", true);
        m_toggle1.needReqChange = true;

        m_toggleGroup1 = GetChildByName<KToggleGroup>("ToggleGroup_1", true);
        //m_toggleGroup1.allowMultiple = true;
        //m_toggleGroup1.needReqChange = true;
        //m_toggleGroup1.allowSwitchOff = true;

        m_inputName = GetChildByName<KInputField>("Input_name", true);


        m_buttonAnnounce = GetChildByName<KButton>("Button_SystemAnnounce", true);
        m_buttonClose = GetChildByName<KButton>("Button_Close", true);


        m_barExp = GetChildByName<KProgressBar>("ProgressBar_EXP", true);
        m_sliderSchedule1 = GetChildByName<KSlider>("Slider_Schedule1", true);

        m_icon1 = GetChildByName<KImage>("Image_Icon1", true);


        m_scrollViewItemList = GetChildByName<KScrollView>("ScrollView_ItemList", true);
    }


    protected override void __Show(object showObj_, params object[] params_)
    {
        ShowImage();
        ShowToggle();
        ShowText();
        ShowBtn();
        ShowProgressBar();
        ShowSlider();
        //ShowLayout();
        //ShowListView();
        ShowListViewScroll();

        CCApp.soundMgr.PlayBgm("Sound/Bgm/battle06");
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

        ClearToggle();
        ClearText();
        ClearBtn();
        ClearProgressBar();
        ClearSlider();
        //ClearLayout();
        //ClearListView();
        ClearListViewScroll();

        CCApp.soundMgr.StopBgm();
    }


    void OnKeyPress(object evt_)
    {
        KeyCode key = (KeyCode)evt_;
        switch (key)
        {
            case KeyCode.RightArrow:

                //m_inputName.GainFocus();
                AppExp(5);
                SprAtlasCache.me.UnloadSprite("npc1026");

                break;
            case KeyCode.LeftArrow:

                SubExp(5);
                SprAtlasCache.me.LoadSprite(this, m_icon1, "npc1026", "npc1026_mini", true);

                break;
            case KeyCode.UpArrow:

                Log.Debug(m_icon1 != null ? "还活着" : "挂了");

                break;
            case KeyCode.DownArrow:

                GameObjUtil.Delete(m_icon1.gameObject);

                break;
            case KeyCode.Keypad1:

                m_listViewScroll.JumpToIndex(4, KListViewScroll.JumpPosType.TOP);

                break;
            case KeyCode.Keypad2:
                
                m_listViewScroll.JumpToIndex(4, KListViewScroll.JumpPosType.CENTER);

                break;
            case KeyCode.Keypad3:
                
                //m_listViewScroll.JumpToIndex(4, KListViewScroll.JumpPosType.BOTTOM);
                m_listViewScroll.JumpToTop();
                break;
            case KeyCode.Keypad4:

                List<int> datas = new List<int>();
                int num = 5;
                for (int i = 0; i < num; ++i)
                {
                    datas.Add(i);
                }

                m_listViewScroll.ShowList(datas);
                
                break;
        }

    }



    //-------∽-★-∽------∽-★-∽--------∽-★-∽Image∽-★-∽--------∽-★-∽------∽-★-∽--------//

     void ShowImage()
    {
        //SprAtlasCache.me.LoadSprite(this, m_icon1,  "SkillIcon1", "E10000", false);
        SprAtlasCache.me.LoadSprite(this, m_icon1, "npc1103", null, true);

    }

    //-------∽-★-∽------∽-★-∽--------∽-★-∽Toggle∽-★-∽--------∽-★-∽------∽-★-∽--------//


    void ShowToggle()
    {
        m_toggle1.onValueChanged.AddListener(OnToggle);
        m_toggle1.onReqChange.AddListener(OnReqToggle);

        m_toggleGroup1.onReqChange.AddListener(OnToggleGroupReq);
        m_toggleGroup1.onValueChange.AddListener(OnToggleGroup);

    }

    void ClearToggle()
    {

        m_toggle1.onValueChanged.RemoveListener(OnToggle);
        m_toggle1.onReqChange.RemoveListener(OnReqToggle);

        m_toggleGroup1.onReqChange.RemoveListener(OnToggleGroupReq);
        m_toggleGroup1.onValueChange.RemoveListener(OnToggleGroup);
    }


    void OnToggle(KToggle toggle_, bool isOn_)
    {
        CCApp.soundMgr.totalOn = !isOn_;
    }

    void OnReqToggle(KToggle toggle_, bool isOn_)
    {
        //toggle_.isOn = isOn_;
        toggle_.Select(isOn_, false);
    }


    void OnToggleGroup(KToggleGroup toggle_, int index_, bool isOn_)
    {
        Log.Debug("OnToggleGroup:" + index_ + " " + isOn_, this);

    }

    void OnToggleGroupReq(KToggleGroup toggle_, int index_, bool isOn_)
    {

        Log.Debug("OnToggleGroupReq:" + index_ + " " + isOn_, this);

        toggle_.Select(index_, isOn_, true);

    }


    //-------∽-★-∽------∽-★-∽--------∽-★-∽text∽-★-∽--------∽-★-∽------∽-★-∽--------//

    void ShowText()
    {

        m_inputName.onValueChanged.AddListener(OnInputChanged);
        m_inputName.onEndEdit.AddListener(OnInputEnd);
    }

    void ClearText()
    {

        m_inputName.onValueChanged.RemoveListener(OnInputChanged);
        m_inputName.onEndEdit.RemoveListener(OnInputEnd);
    }


    void OnInputChanged(string str_)
    {

        //Log.Debug("OnInputChanged:" + str_, this);

    }

    void OnInputEnd(string str_)
    {


        //Log.Debug("OnInputEnd:" + str_, this);
    }



    //-------∽-★-∽------∽-★-∽--------∽-★-∽text∽-★-∽--------∽-★-∽------∽-★-∽--------//

    void ShowBtn()
    {
        m_buttonAnnounce.onClick.AddListener(OnClickBtn);
        m_buttonClose.onClick.AddListener(OnClickClose);
    }

    void ClearBtn()
    {
        m_buttonAnnounce.onClick.RemoveListener(OnClickBtn);
        m_buttonClose.onClick.RemoveListener(OnClickClose);
    }

    void OnClickBtn(KButton btn_)
    {
        //CCApp.soundMgr.PlayOneShot("Sound/UI/UI_EquipmentIntensify");
        CCApp.soundMgr.PlayUi("Sound/UI/UI_EquipmentIntensify");
    }

    void OnClickClose(KButton btn_)
    {
        Close();
    }


    //-------∽-★-∽------∽-★-∽--------∽-★-∽ProgressBar∽-★-∽--------∽-★-∽------∽-★-∽--------//


    void ShowProgressBar()
    {
        m_barExp.percent = 0;

    }

    void ClearProgressBar()
    {
        m_barExp.percent = 0;
    }

    void AppExp(int value_)
    {

        m_barExp.percent += value_;
    }
    void SubExp(int value_)
    {

        m_barExp.percent -= value_;
    }

    //-------∽-★-∽------∽-★-∽--------∽-★-∽Slider∽-★-∽--------∽-★-∽------∽-★-∽--------//

    void ShowSlider()
    {
        m_sliderSchedule1.onValueChanged.AddListener(OnSilderChange);
        m_sliderSchedule1.SlicedFilled = true;

        m_sliderSchedule1.value = 0.5f;
    }

    void ClearSlider()
    {
        m_sliderSchedule1.onValueChanged.RemoveListener(OnSilderChange);

    }

    void OnSilderChange(KSlider target, float value_)
    {
        m_inputName.text = value_.ToString();

        CCApp.soundMgr.bgmVolume = value_;
    }


    //-------∽-★-∽------∽-★-∽--------∽-★-∽Layout∽-★-∽--------∽-★-∽------∽-★-∽--------//

    void ShowLayout()
    {
        LayoutParam param = new LayoutParam { };
        param.padding = new Padding(20, 20, 20, 20);
        param.itemGap = new Vector2(50, 50);
        param.divNum = 2;

        GameObject itemGo = GameObjUtil.FindChild(m_scrollViewItemList.gameObject, "Image_mask/Container_content/Container_Item");
        GameObject container = GameObjUtil.GetParent(itemGo);

        itemGo.SetActive(false);

        for (var i = 0; i < 10; ++i)
        {
            GameObject item = GameObjUtil.Instantiate(itemGo);

            item.SetActive(true);
            DisplayUtil.AddChild(container, item);

            LayoutUtil.LayItem(param, i, item);

            m_idx2item[i] = item;
        }

    }

    void ClearLayout()
    {

        foreach (var kvp in m_idx2item)
        {
            GameObjUtil.Delete(kvp.Value);
        }

        m_idx2item.Clear();

    }

    //-------∽-★-∽------∽-★-∽--------∽-★-∽ListView∽-★-∽--------∽-★-∽------∽-★-∽--------//


    void ShowListView()
    {
        GameObject container = GameObjUtil.FindChild(m_scrollViewItemList.gameObject, "Image_mask/Container_content");

        List<int> datas = new List<int>();
        int num = 10;
        for (int i=0; i< num; ++i)
        {
            datas.Add(i);
        }

        m_listView = ComponentUtil.EnsureComponent<KListView>(container);
        m_listView.itemViewType = typeof(Item1);
        m_listView.onDataChanged.AddListener(UpdateListItem);


        LayoutParam param = m_listView.layoutParam;
        //param.padding = new Padding(20, 20, 20, 20);
        param.itemGap = new Vector2(10, 50);
        //param.divNum = 2;
        

        m_listView.ShowList(datas);

    }

    void ClearListView()
    {
        m_listView.ClearList();
        m_listView.onDataChanged.RemoveListener(UpdateListItem);
    }


    void UpdateListItem(GameObject item_, int index_, object data_)
    {


    }


    //-------∽-★-∽------∽-★-∽--------∽-★-∽ListViewScroll∽-★-∽--------∽-★-∽------∽-★-∽--------//


    void ShowListViewScroll()
    {
        
        
        m_listViewScroll = ComponentUtil.EnsureComponent<KListViewScroll>(m_scrollViewItemList.gameObject);
        m_listViewScroll.itemViewType = typeof(Item1);
        m_listViewScroll.onDataChanged.AddListener(UpdateListItem2);

        LayoutParam param = m_listViewScroll.layoutParam;
        //param.padding = new Padding(20, 20, 20, 20);
        param.itemGap = new Vector2(10, 50);
        param.divNum = 3;
        //param.dir = LayoutDirection.LeftToRight;

        List<int> datas = new List<int>();
        int num = 999;
        for (int i = 0; i < num; ++i)
        {
            datas.Add(i);
        }


        //m_listViewScroll.direction = KScrollView.ScrollDir.horizontal;

        m_listViewScroll.ShowList(datas);

    }

    void ClearListViewScroll()
    {
        m_listViewScroll.ClearList();
        m_listViewScroll.onDataChanged.RemoveListener(UpdateListItem2);
    }


    void UpdateListItem2(GameObject item_, int index_, object data_)
    {


    }



    //-------∽-★-∽------∽-★-∽--------∽-★-∽KListViewItem∽-★-∽--------∽-★-∽------∽-★-∽--------//


    class Item1 : KListView.KListViewItem
    {

        KText m_label;

        protected override void __Dispose()
        {
            Log.Debug("析构啦");
        }

        public Item1()
        {
            
        }


        protected override void __ShowGameObject()
        {
            base.__ShowGameObject();

            m_label = GetChildByName<KText>("Label_Text");
        }

        protected override void __Show(object showObj_, params object[] params_)
        {

            m_label.text = m_index.ToString();
        }

       
        protected override void SetupEvent()
        {

        }

        protected override void ClearEvent()
        {

        }


        protected override void __Destroy()
        {

           
        }

    }

}