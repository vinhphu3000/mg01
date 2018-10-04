using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using UnityEngine;
using UnityEditor;
using Object = UnityEngine.Object;

namespace Edit.PSD4UGUI
{
    public class BaseRecorder : EditorWindow
    {


        Color _defaultColor = GUI.backgroundColor;
        Color _redColor = new Color(171f / 255f, 26f / 255f, 37f / 255f);
        Color _greenColor = new Color(26f / 255f, 171f / 255f, 37f / 255f);
        Color _blueColor = new Color(0, 122f / 255f, 204f / 255f);

        
        protected int _languageIndex;
        protected string _btnLabel;
        protected GameObject _panelPrefabObj;

        //用于显示目标列表
        protected Dictionary<GameObject, List<GameObject>> _parent2targets;

        void Awake()
        {
            _languageIndex = LanguageSetting.GetLanguageIndex(KAssetManager.language);  //默认语言
            _parent2targets = null;
        }

        void OnGUI()
        {
            try
            {
                ShowLanguageSetting();
                ShowPanelPrefabInput();
                CreateGenerateButton();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        //显示语言设置
        void ShowLanguageSetting()
        {
            GUILayout.Label("设置语言:", GUILayout.Width(70));
            _languageIndex = EditorGUILayout.Popup(_languageIndex, LanguageSetting.languages, GUILayout.Width(120));
            KAssetManager.language = LanguageSetting.GetLanguage(_languageIndex);
        }

        //显示go输入框
        void ShowPanelPrefabInput()
        {
            bool showBtnClicked = false;
            GUILayout.Space(10);
            GUILayout.BeginHorizontal();
            GUILayout.Label("面板：", GUILayout.Width(50));
            _panelPrefabObj = EditorGUILayout.ObjectField(_panelPrefabObj, typeof(GameObject), true, GUILayout.Width(300)) as GameObject;
            if (_panelPrefabObj != null)
            {
                GUI.backgroundColor = _greenColor;
                showBtnClicked = GUILayout.Button("显示位置", GUILayout.Width(80));
                GUI.backgroundColor = _defaultColor;
            }
            GUILayout.EndHorizontal();

            if (showBtnClicked == true)
            {
                OnClickTargets();
            }

            if (_panelPrefabObj != null)
            {
                ShowTargets();
            }
        }

        //显示生成按钮
        void CreateGenerateButton()
        {
            GUILayout.Space(10);
            GUILayout.BeginHorizontal();
            _defaultColor = GUI.backgroundColor;
            bool result = ValidateInput();
            if (result == true)
            {
                GUI.backgroundColor = _greenColor;
            }
            else
            {
                GUI.backgroundColor = _redColor;
            }

            if (GUILayout.Button(_btnLabel, GUILayout.Width(440), GUILayout.Height(50)))
            {
                if (result == true)
                {
                    OnClickGenerate();
                }
            }
            GUI.backgroundColor = _defaultColor;
            GUILayout.EndHorizontal();
        }

        //判断input是否可用
        protected virtual bool ValidateInput()
        {
            if (_panelPrefabObj == null)
            {
                _btnLabel = "未找到面板Prefab配置";
                return false;
            }
            
            _btnLabel = "生成记录";
            return true;
        }


        //-------∽-★-∽------∽-★-∽--------∽-★-∽目标位置∽-★-∽--------∽-★-∽------∽-★-∽--------//

        
        protected virtual void OnClickTargets()
        {


        }


        //显示目标的位置
        protected virtual void ShowTargets()
        {

            if (_parent2targets == null)
                return;

            foreach (GameObject spot in _parent2targets.Keys)
            {
                GUILayout.Space(10);
                GUILayout.BeginHorizontal();
                GUILayout.Space(50);
                GUILayout.Label("挂接点：", GUILayout.Width(50));
                EditorGUILayout.ObjectField(spot, typeof(GameObject), true, GUILayout.Width(250));
                GUILayout.EndHorizontal();
                List<GameObject> effectList = _parent2targets[spot];
                foreach (GameObject effect in effectList)
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Space(100);
                    GUILayout.Label("目标：", GUILayout.Width(50));
                    EditorGUILayout.ObjectField(effect, typeof(GameObject), true, GUILayout.Width(200));
                    GUILayout.EndHorizontal();
                }
            }
        }

        //-------∽-★-∽------∽-★-∽--------∽-★-∽生成记录∽-★-∽--------∽-★-∽------∽-★-∽--------//
        

        //生成记录
        protected virtual void OnClickGenerate()
        {


        }

    }

}